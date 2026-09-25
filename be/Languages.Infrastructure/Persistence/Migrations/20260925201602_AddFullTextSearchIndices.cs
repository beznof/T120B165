using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languages.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextSearchIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG [DefaultFullTextCatalog] AS DEFAULT;", suppressTransaction: true);
            
            migrationBuilder.Sql(
                """
                CREATE FULLTEXT INDEX ON [Languages] ([Name] LANGUAGE 0)
                KEY INDEX [PK_Languages]
                ON [DefaultFullTextCatalog]
                WITH CHANGE_TRACKING AUTO;
                """, suppressTransaction: true);
            
            migrationBuilder.Sql(
                """
                CREATE FULLTEXT INDEX ON [DictionaryEntries] ([Text] LANGUAGE 0)
                KEY INDEX [PK_DictionaryEntries]
                ON [DefaultFullTextCatalog]
                WITH CHANGE_TRACKING AUTO;
                """, suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FULLTEXT INDEX ON Languages;", suppressTransaction: true);
            
            migrationBuilder.Sql("DROP FULLTEXT INDEX ON DictionaryEntries;", suppressTransaction: true);
            
            migrationBuilder.Sql("DROP FULLTEXT CATALOG DefaultFullTextCatalog;", suppressTransaction: true);
        }
    }
}
