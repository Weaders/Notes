using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesMVC.Migrations
{
    public partial class RolesAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "403579fe-1d53-4105-b306-9e50945f93b8", "f0c11efa-bd4e-48eb-994c-0a9c5ca3c1fb", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6f8d148a-03f0-4b62-85df-91e809880d00", "ce0bfbf9-1859-4d19-af47-0f4e2da7f5ef", "Member", "MEMBER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "403579fe-1d53-4105-b306-9e50945f93b8", "f0c11efa-bd4e-48eb-994c-0a9c5ca3c1fb" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "6f8d148a-03f0-4b62-85df-91e809880d00", "ce0bfbf9-1859-4d19-af47-0f4e2da7f5ef" });

        }
    }
}
