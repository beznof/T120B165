using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languages.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "LanguageDictionaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageDictionaries_OwnerId",
                table: "LanguageDictionaries",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageDictionaries_Users_OwnerId",
                table: "LanguageDictionaries",
                column: "OwnerId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageDictionaries_Users_OwnerId",
                table: "LanguageDictionaries");

            migrationBuilder.DropIndex(
                name: "IX_LanguageDictionaries_OwnerId",
                table: "LanguageDictionaries");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "LanguageDictionaries");
        }
    }
}
