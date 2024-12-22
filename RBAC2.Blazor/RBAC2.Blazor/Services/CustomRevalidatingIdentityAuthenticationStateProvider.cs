using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RBAC2.Database;
using RBAC2.Database.Entities;

namespace RBAC2.Blazor.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za ponowną walidację stanu uwierzytelnienia użytkownika.
    /// </summary>
    public class CustomRevalidatingIdentityAuthenticationStateProvider<TUser> : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private readonly IServiceScopeFactory _scopeFactory; // Fabryka zakresów usług
        private readonly IdentityOptions _options; // Opcje tożsamości

        /// <summary>
        /// Konstruktor klasy, inicjalizuje fabrykę zakresów usług i opcje tożsamości.
        /// </summary>
        /// <param name="loggerFactory">Fabryka loggerów.</param>
        /// <param name="scopeFactory">Fabryka zakresów usług.</param>
        /// <param name="optionsAccessor">Dostęp do opcji tożsamości.</param>
        public CustomRevalidatingIdentityAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = optionsAccessor.Value;
        }

        /// <summary>
        /// Interwał ponownej walidacji ustawiony na 30 minut.
        /// </summary>
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        /// <summary>
        /// Metoda walidująca stan uwierzytelnienia użytkownika.
        /// </summary>
        /// <param name="authenticationState">Stan uwierzytelnienia użytkownika.</param>
        /// <param name="cancellationToken">Token anulowania.</param>
        /// <returns>True, jeśli stan uwierzytelnienia jest ważny, w przeciwnym razie false.</returns>
        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope(); // Tworzenie nowego zakresu usług
            try
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>(); // Pobieranie usługi użytkownika
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); // Pobieranie menedżera użytkowników

                var login = authenticationState.User.Identity.Name; // Pobieranie nazwy użytkownika

                if (string.IsNullOrEmpty(login))
                {
                    return false; // Jeśli nazwa użytkownika jest pusta, zwróć false
                }

                var identityUser = await userService.FindIdentityUserByLoginAsync(login); // Znajdowanie użytkownika na podstawie loginu
                if (identityUser == null)
                {
                    return false; // Jeśli użytkownik nie istnieje, zwróć false
                }

                if (userManager.SupportsUserSecurityStamp)
                {
                    var principalStamp = authenticationState.User.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType); // Pobieranie znacznika bezpieczeństwa z bieżącego stanu uwierzytelnienia
                    var userStamp = await userManager.GetSecurityStampAsync(identityUser); // Pobieranie znacznika bezpieczeństwa użytkownika

                    return principalStamp == userStamp; // Porównywanie znaczników bezpieczeństwa
                }

                return true; // Jeśli menedżer użytkowników nie obsługuje znaczników bezpieczeństwa, zwróć true
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync(); // Asynchroniczne zwalnianie zasobów
                }
                else
                {
                    scope.Dispose(); // Zwalnianie zasobów
                }
            }
        }

        /// <summary>
        /// Metoda pobierająca stan uwierzytelnienia użytkownika.
        /// </summary>
        /// <returns>Stan uwierzytelnienia użytkownika.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            using var scope = _scopeFactory.CreateScope(); // Tworzenie nowego zakresu usług
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>(); // Pobieranie usługi użytkownika
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); // Pobieranie menedżera użytkowników

            var login = System.Security.Principal.WindowsIdentity.GetCurrent().Name; // Pobieranie bieżącej nazwy użytkownika

            if (string.IsNullOrEmpty(login))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // Jeśli nazwa użytkownika jest pusta, zwróć pusty stan uwierzytelnienia
            }

            var identityUser = await userService.FindIdentityUserByLoginAsync(login); // Znajdowanie użytkownika na podstawie loginu
            if (identityUser == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // Jeśli użytkownik nie istnieje, zwróć pusty stan uwierzytelnienia
            }

            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, identityUser.UserName) // Dodawanie roszczenia z nazwą użytkownika
                        };

            var roles = await userManager.GetRolesAsync(identityUser); // Pobieranie ról użytkownika
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // Dodawanie roszczeń z rolami użytkownika
            }

            var identity = new ClaimsIdentity(claims, "WindowsAuth"); // Tworzenie tożsamości z roszczeniami
            var principal = new ClaimsPrincipal(identity); // Tworzenie głównego podmiotu z tożsamością

            return new AuthenticationState(principal); // Zwracanie stanu uwierzytelnienia
        }
    }
}
