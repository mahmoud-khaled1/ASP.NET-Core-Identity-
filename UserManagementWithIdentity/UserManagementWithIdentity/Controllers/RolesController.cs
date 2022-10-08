using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UserManagementWithIdentity.ViewModel;

namespace UserManagementWithIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager; 
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Add(RoleViewModel role)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            var roleExist = await _roleManager.RoleExistsAsync(role.Name);
            if(roleExist)
            {
                ModelState.AddModelError("Name", "Role is exists!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            await _roleManager.CreateAsync(new IdentityRole(role.Name.Trim()));
            return RedirectToAction(nameof(Index));

        }
    }
}
