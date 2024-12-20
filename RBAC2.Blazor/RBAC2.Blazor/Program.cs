using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RBAC2.Blazor.Components;
using RBAC2.Database;
using RBAC2.Database.Seed;

namespace RBAC2.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;
            var databasePath = Path.Combine(solutionDirectory, "rbac.db");
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddDbContextFactory<RbacDbContext>(options =>
                options.UseSqlite($"Data Source={databasePath}"));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<RbacDbContext>();


            var app = builder.Build();
            //using (var scope = app.Services.CreateScope())
            //{

                //!!!!!!!Przyk³adowe dane do tabeli Users odpalam raz 
                //var dbContext = scope.ServiceProvider.GetRequiredService<RbacDbContext>();
                //dbContext.Database.Migrate();
                //RbacDbContextSeed.Seed(dbContext);

                //!!!! JEDNORAZOWY TRANSFER Z Users do ASPNETUsers ze zrobieniem wi¹zañ 
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //       await OnceTransferUsersToAspNetUsers.SeedUsersAsync(services);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"B³¹d podczas seeda u¿ytkowników: {ex.Message}");
            //    }
            //}

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
