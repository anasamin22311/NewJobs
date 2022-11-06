using Jobs.Data;
using Jobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public Program(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task CreateDefaultRolesOrUsers()
    {
        IdentityRole role = new IdentityRole();
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            role.Name = "Admin";
            await roleManager.CreateAsync(role);
            ApplicationUser user = new ApplicationUser();
            user.UserName = "Anas Amin";
            user.Email = "anasamin22311@gmail.com";
            var check = await userManager.CreateAsync(user,"Anas_8264");
            if (check.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

        }
    }
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Create Default Role or User
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();


        builder.Services.AddRazorPages();
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        // builder.Services.AddTransient<IEmailSender, IEmailSender>();



        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}