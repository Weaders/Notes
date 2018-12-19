using Microsoft.EntityFrameworkCore.Migrations;
using NotesMVC.Services.Encrypter;

namespace NotesMVC.Migrations
{
    public partial class AddCryptoNameToNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CryptoName",
                table: "Notes",
                nullable: true);

            migrationBuilder.Sql($"UPDATE Notes SET CryptoName='{CryptographType.AES.Type}' ");            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CryptoName",
                table: "Notes");
        }
    }
}
