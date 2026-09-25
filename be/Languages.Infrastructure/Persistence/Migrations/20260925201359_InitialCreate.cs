using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languages.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageFamilies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageFamilies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LanguageFamilyID = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Languages_LanguageFamilies_LanguageFamilyID",
                        column: x => x.LanguageFamilyID,
                        principalTable: "LanguageFamilies",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LanguageLevels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageID = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLevels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LanguageLevels_Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageDictionaries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LanguageID = table.Column<int>(type: "int", nullable: false),
                    LevelID = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageDictionaries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LanguageDictionaries_LanguageLevels_LevelID",
                        column: x => x.LevelID,
                        principalTable: "LanguageLevels",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LanguageDictionaries_Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneticTranscription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DictionaryID = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DictionaryEntries_LanguageDictionaries_DictionaryID",
                        column: x => x.DictionaryID,
                        principalTable: "LanguageDictionaries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntrySynonyms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DictionaryEntryID = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntrySynonyms", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DictionaryEntrySynonyms_DictionaryEntries_DictionaryEntryID",
                        column: x => x.DictionaryEntryID,
                        principalTable: "DictionaryEntries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntries_DictionaryID",
                table: "DictionaryEntries",
                column: "DictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntrySynonyms_DictionaryEntryID",
                table: "DictionaryEntrySynonyms",
                column: "DictionaryEntryID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageDictionaries_LanguageID",
                table: "LanguageDictionaries",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageDictionaries_LevelID",
                table: "LanguageDictionaries",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageLevels_LanguageID",
                table: "LanguageLevels",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LanguageFamilyID",
                table: "Languages",
                column: "LanguageFamilyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryEntrySynonyms");

            migrationBuilder.DropTable(
                name: "DictionaryEntries");

            migrationBuilder.DropTable(
                name: "LanguageDictionaries");

            migrationBuilder.DropTable(
                name: "LanguageLevels");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "LanguageFamilies");
        }
    }
}
