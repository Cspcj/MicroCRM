using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroCRM.Helpers
{
    public class DatabaseSeed
    {
        private readonly ILogger<DatabaseSeed> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private string[] defaultRoles = { "Manager", "Employee", "Client", "Unassigned" };

        // injecting necessary services

        public DatabaseSeed(ILogger<DatabaseSeed> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        public async Task SeedDatabaseRoles()
        {
            _logger.LogInformation($"{DateTime.Now} : Seeding started");

            // role seeding
            if (_roleManager != null)
            {
                foreach (var role in defaultRoles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        var newRole = new IdentityRole(role);
                        await _roleManager.CreateAsync(newRole);
                        _logger.LogInformation($"{DateTime.Now} : Adding to database a new role - {role}");
                    }
                }
            }
        }

        // seed default user
        public async Task SeedDatabaseAdmin()
        {
            string userName = _configuration.GetValue<string>("defaultUserEmail");
            _logger.LogInformation($"Creating username {userName}");

            if (await _userManager.FindByNameAsync(userName) == null)
            {
                _logger.LogInformation("Adding default user");
                var newUser = new IdentityUser
                {
                    UserName = userName,
                    NormalizedUserName = userName.Normalize(),
                    Email = userName,
                    EmailConfirmed = true,
                    PhoneNumber = "123456789"
                };

                var result = await _userManager.CreateAsync(newUser);
                _logger.LogInformation($"created {result}");
                if (result.Succeeded)
                {
                    _logger.LogInformation("Assigning role to default user");
                    await _userManager.AddToRoleAsync(newUser, "Manager");
                    var password = _configuration.GetValue<string>("defaultUserPassword");
                    _logger.LogInformation("Adding password");
                    await _userManager.AddPasswordAsync(newUser, password);
                }
            }

            userName = "someone@local.com";
            _logger.LogInformation($"Creating username {userName}");

            if (await _userManager.FindByNameAsync(userName) == null)
            {
                _logger.LogInformation("Adding default user");
                var newUser = new IdentityUser
                {
                    UserName = userName,
                    NormalizedUserName = userName.Normalize(),
                    Email = userName,
                    EmailConfirmed = true,
                    PhoneNumber = "123456789"
                };

                var result = await _userManager.CreateAsync(newUser);
                _logger.LogInformation($"created {result}");
                if (result.Succeeded)
                {
                    _logger.LogInformation("Assigning role to default user");
                    //await _userManager.AddToRoleAsync(newUser, "Employee");
                    var password = "Pass*1234";
                    _logger.LogInformation("Adding password");
                    await _userManager.AddPasswordAsync(newUser, password);
                }
            }

        }
    }
}

