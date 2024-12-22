using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RBAC2.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimsDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimDictionaries",
                schema: "dbo",
                columns: table => new
                {
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimDictionaries", x => x.ClaimId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimDictionaries",
                schema: "dbo");
        }
    }
}
