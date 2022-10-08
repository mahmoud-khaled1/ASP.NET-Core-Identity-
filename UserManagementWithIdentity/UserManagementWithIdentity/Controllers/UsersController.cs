using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementWithIdentity.Models;
using UserManagementWithIdentity.ViewModel;

namespace UserManagementWithIdentity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UsersController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager=userManager;
            _roleManager=roleManager;   
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.firstName,
                LastName = user.lastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();

            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if(user==null)
            {
                return NotFound();
            }
            var Roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = Roles.Select(role => new RoleCheckBoxViewModel {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsChecked = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role  in model.Roles)
            {
                if(userRoles.Any(r=> r==role.RoleName) && !role.IsChecked)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
                if (!userRoles.Any(r => r == role.RoleName) && role.IsChecked)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
               
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleCheckBoxViewModel { RoleId=r.Id, RoleName= r.Name }).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                Roles = roles
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(!model.Roles.Any(r=>r.IsChecked))
            {
                ModelState.AddModelError("Roles", "Please Select at Least one Role !");
                return View(model);
            }
            // check if Email is exist in DataBase
            if(await _userManager.FindByEmailAsync(model.Email)!=null)
            {
                ModelState.AddModelError("Email", "Email is already Exist !");
                return View(model);
            }
            // add new User to database 
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                firstName = model.FirstName,
                lastName = model.LastName,
            };
            var result=await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            // assign checked Roles to user
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r=>r.IsChecked).Select(r => r.RoleName));
            

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileFormViewModel
            {
                Id = userId,
                FirstName = user.firstName,
                LastName=user.lastName,
                Email=user.Email,
                UserName=user.UserName
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            if(userWithSameEmail!=null && userWithSameEmail.Id!=model.Id)
            {
                ModelState.AddModelError("Email", "this Email is already assigned to another user !");
                return View(model);
            }
            user.firstName = model.FirstName;
            user.lastName = model.LastName;
            user.Email = model.Email;

            await _userManager.UpdateAsync(user);


            return RedirectToAction(nameof(Index));
        }

    }
}
