using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementWithIdentity.Data.Migrations
{
    public partial class seedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // insert into table Roles , First we create empty migration then write this here:-
            
            // insert User Role
            migrationBuilder.InsertData(table: "Roles", columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp"},
                values:new object[] {Guid.NewGuid().ToString(),"User","User".ToUpper(),Guid.NewGuid().ToString()}
                ,schema:"security");

            //insert Admin Role
            migrationBuilder.InsertData(table: "Roles", columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }
                , schema: "security");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from security.Roles");
        }
    }
}
