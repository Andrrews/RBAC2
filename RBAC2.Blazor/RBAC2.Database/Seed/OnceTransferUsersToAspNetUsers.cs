using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace RBAC2.Database.Seed
{
    public class OnceTransferUsersToAspNetUsers
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RbacDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Pobierz istniejących użytkowników z tabeli Users
                var users = dbContext.Users.ToList();

                foreach (var user in users)
                {
                    // Jeśli IdentityUserId jest puste, tworzymy nowego użytkownika w AspNetUsers
                    if (string.IsNullOrEmpty(user.IdentityUserId))
                    {
                        // Tworzymy nowego IdentityUser
                        var identityUser = new IdentityUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = user.Login,
                            Email = null,
                            NormalizedUserName = user.Login.ToLower(),
                            NormalizedEmail = null,
                            EmailConfirmed = true // Zakładamy, że email jest potwierdzony
                        };

                        // Tworzymy użytkownika w Identity
                        var result = await userManager.CreateAsync(identityUser, "TemporaryPassword123!");

                        if (result.Succeeded)
                        {
                            // Powiązanie tabeli Users z nowo utworzonym użytkownikiem Identity
                            user.IdentityUserId = identityUser.Id;
                        }
                        else
                        {
                            // Logowanie błędów
                            Console.WriteLine($"Błąd podczas dodawania użytkownika {user.Login}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                // Zapisz zmiany w tabeli Users
                await dbContext.SaveChangesAsync();

                Console.WriteLine("Migracja danych użytkowników zakończona pomyślnie.");
            }
        }
    }
}
