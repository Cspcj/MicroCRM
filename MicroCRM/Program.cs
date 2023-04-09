using MicroCRM.Data;
using MicroCRM.Helpers;
using MicroCRM.Repositories;
using MicroCRM.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroCRM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add identity related services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddUserManager<UserManager<IdentityUser>>();
            builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/stop/index");


            builder.Services.AddControllersWithViews();
            builder.Services.AddLogging();

            // database seed service
            builder.Services.AddTransient<DatabaseSeed, DatabaseSeed>();


            // services
            builder.Services.AddTransient<IClientService, ClientService>();
            builder.Services.AddTransient<IUserViewService, UserViewService>();
            builder.Services.AddTransient<IProjectsService, ProjectsService>();
            builder.Services.AddTransient<INoteService, NoteService>();
            builder.Services.AddTransient<ITasksService, TasksService>();

            // repositories
            builder.Services.AddTransient<IClientRepository, ClientRepository>();
            builder.Services.AddTransient<IProjectsRepository, ProjectRepository>();
            builder.Services.AddTransient<INotesRepositiory, NotesRepository>();
            builder.Services.AddTransient<ITasksRepository, TasksRepository>();

            // 
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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbSeed = services.GetRequiredService<DatabaseSeed>();
                dbSeed.SeedDatabaseRoles().Wait();
                dbSeed.SeedDatabaseAdmin().Wait();
            }

            app.Run();
        }
    }
}