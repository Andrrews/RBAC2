using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RBAC2.Database.Migrations
{
    /// <inheritdoc />
    public partial class DELETETRIGGERS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Delete;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
