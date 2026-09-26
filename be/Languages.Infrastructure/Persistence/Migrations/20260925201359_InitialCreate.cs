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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageFamilies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LanguageFamilyId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_LanguageFamilies_LanguageFamilyId",
                        column: x => x.LanguageFamilyId,
                        principalTable: "LanguageFamilies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LanguageLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageLevels_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageDictionaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    LanguageLevelId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageDictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageDictionaries_LanguageLevels_LanguageLevelId",
                        column: x => x.LanguageLevelId,
                        principalTable: "LanguageLevels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LanguageDictionaries_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneticTranscription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DictionaryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryEntries_LanguageDictionaries_DictionaryId",
                        column: x => x.DictionaryId,
                        principalTable: "LanguageDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryEntrySynonyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DictionaryEntryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntrySynonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryEntrySynonyms_DictionaryEntries_DictionaryEntryId",
                        column: x => x.DictionaryEntryId,
                        principalTable: "DictionaryEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntries_DictionaryId",
                table: "DictionaryEntries",
                column: "DictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntrySynonyms_DictionaryEntryId",
                table: "DictionaryEntrySynonyms",
                column: "DictionaryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageDictionaries_LanguageId",
                table: "LanguageDictionaries",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageDictionaries_LanguageLevelId",
                table: "LanguageDictionaries",
                column: "LanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageLevels_LanguageId",
                table: "LanguageLevels",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LanguageFamilyId",
                table: "Languages",
                column: "LanguageFamilyId");
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
