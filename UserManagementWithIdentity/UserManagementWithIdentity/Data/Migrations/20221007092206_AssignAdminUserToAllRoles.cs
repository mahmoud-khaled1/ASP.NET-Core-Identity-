using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementWithIdentity.Data.Migrations
{
    public partial class AssignAdminUserToAllRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into [security].[UserRole] (UserId,RoleId) select'80dadb3f-de78-4627-8370-632a693ab432',Id from [security].[Roles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [security].[UserRole] where UserId=80dadb3f-de78-4627-8370-632a693ab432");
        }
    }
}
