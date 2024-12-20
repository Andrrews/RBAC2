using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RBAC2.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Delete;");
            // Trigger dla INSERT
            migrationBuilder.Sql(@"
                        CREATE TRIGGER SyncUserToIdentity_Insert
                        AFTER INSERT ON Users
                        BEGIN
                            INSERT INTO AspNetUsers (Id, UserName, Email, NormalizedUserName, NormalizedEmail, EmailConfirmed)
                            SELECT NEW.IdentityUserId, NEW.Email = NULL , NEW.Email= NULL, UPPER(NEW.Email), UPPER(NEW.Email), 1
                            WHERE NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = NEW.IdentityUserId);

                            UPDATE Users
                            SET IdentityUserId = NEW.IdentityUserId
                            WHERE Id = NEW.Id;
                        END;
                    ");


            // Trigger dla UPDATE
            migrationBuilder.Sql(@"
                        CREATE TRIGGER SyncUserToIdentity_Update
                        AFTER UPDATE ON Users
                        BEGIN
                            UPDATE AspNetUsers
                            SET UserName = NEW.Email,
                                Email = NEW.Email,
                                NormalizedUserName = UPPER(NEW.Email),
                                NormalizedEmail = UPPER(NEW.Email)
                            WHERE Id = NEW.IdentityUserId;
                        END;
                    ");

            // Trigger dla DELETE
            migrationBuilder.Sql(@"
                            CREATE TRIGGER SyncUserToIdentity_Delete
                            AFTER DELETE ON Users
                            BEGIN
                                DELETE FROM AspNetUsers
                                WHERE Id = OLD.IdentityUserId;
                            END;
                        ");
            /// <inheritdoc />
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SyncUserToIdentity_Delete;");
        }
    }
}
