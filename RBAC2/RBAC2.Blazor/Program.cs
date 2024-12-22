using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RBAC2.Blazor.Components;
using RBAC2.Blazor.Middlewares;
using RBAC2.Blazor.Services;
using RBAC2.Database;
using RBAC2.Domain;
using RBAC2.Domain.Services;

namespace RBAC2.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddDbContextFactory<RbacDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RbacDbConn")));

            // Rejestracja to¿samoœci u¿ytkownika i ról oraz konfiguracja Entity Framework do przechowywania danych to¿samoœci
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<RbacDbContext>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // Register domain services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IClaimDictionaryService, ClaimDictionaryService>();

            // Rejestracja dostawcy stanu uwierzytelnienia
            builder.Services.AddScoped<AuthenticationStateProvider, CustomRevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

            // Rejestracja dostawcy stanu uwierzytelnienia dla œrodowiska hosta
            builder.Services.AddScoped<IHostEnvironmentAuthenticationStateProvider>(sp =>
            {
                // Jest to bezpieczne, poniewa¿ `RevalidatingIdentityAuthenticationStateProvider` rozszerza `ServerAuthenticationStateProvider`
                var provider = (ServerAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>();
                return provider;
            });

            // Rejestracja niestandardowego obs³ugiwacza wyników poœrednika autoryzacji
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();

            // Rejestracja dostêpu do kontekstu HTTP
            builder.Services.AddHttpContextAccessor();

            // Rejestracja schematu uwierzytelniania Negotiate
            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            // Rejestracja us³ug autoryzacji
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            //To te¿ trzeba dodaæ middleware'y za autentykacje i autoryzacje
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
