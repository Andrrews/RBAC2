using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;

namespace RBAC2.Blazor.Middlewares
{
    /// <summary>
    /// Ta klasa obsługuje wynik autoryzacji w middleware.
    /// Implementuje interfejs IAuthorizationMiddlewareResultHandler.
    /// </summary>
    public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        /// <summary>
        /// Obsługuje wynik autoryzacji.
        /// Aktualnie po prostu wywołuje następne middleware w potoku.
        /// </summary>
        /// <param name="next">Następne middleware do wykonania.</param>
        /// <param name="context">HttpContext bieżącego żądania.</param>
        /// <param name="policy">Polityka autoryzacji do zastosowania.</param>
        /// <param name="authorizeResult">Wynik sprawdzenia autoryzacji.</param>
        /// <returns>Zadanie reprezentujące operację asynchroniczną.</returns>
        public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            return next(context);
        }
    }
}
