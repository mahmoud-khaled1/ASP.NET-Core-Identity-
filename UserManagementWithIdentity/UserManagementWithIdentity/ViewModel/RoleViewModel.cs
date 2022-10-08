using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace UserManagementWithIdentity.ViewModel
{
    public class RoleViewModel
    {
        [StringLength(256)]
        public string Name { get; set; }
    }
}
