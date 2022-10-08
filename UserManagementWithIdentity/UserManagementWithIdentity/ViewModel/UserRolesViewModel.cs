namespace UserManagementWithIdentity.ViewModel
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleCheckBoxViewModel> Roles { get; set; }
    }
}
