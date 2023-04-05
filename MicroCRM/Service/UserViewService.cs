using MicroCRM.ViewModels;
using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MicroCRM.Service
{
    public class UserViewService : IUserViewService
    {
        private readonly ILogger<IUserViewService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        //private readonly RoleStore<IdentityRole> _roleStore;

        public UserViewService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<IUserViewService> logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
        {

            var userViewModels = new List<UserViewModel>();

            foreach (var user in await _userManager.Users.ToListAsync())
            {
                _logger.LogInformation("User: " + user.UserName);
                var _role = await _roleManager.FindByIdAsync(user.Id.ToString());
                if (_userManager.GetRolesAsync(user).Result.FirstOrDefault() == null)
                {
                    _logger.LogInformation("Role: " + "Unassigned");
                    await _userManager.AddToRoleAsync(user, "Unassigned");
                }
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
                };
                userViewModels.Add(userViewModel);
            }

            return userViewModels.OrderByDescending(x => x.Role).ToList();
        }

        public async Task CreateNewUser(string userEmail, string phoneNumber, string RoleId)
        {
            try
            {
                var user = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userEmail,
                    Email = userEmail,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true

                };
                var result = await _userManager.CreateAsync(user, _configuration.GetValue<string>("defaultUserPassword"));
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(RoleId);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError(string.Empty, error.Description);
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError(string.Empty, ex.Message);
            }
        }

        public async Task<UserViewModel> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
            //var _role = await _roleManager.FindByIdAsync(id.ToString());
            var _role = _userManager.GetRolesAsync(user).Result.First().ToString();
            UserViewModel result = new UserViewModel()
            {
                Id = id.ToString(),
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = _role
            };

            return result;
        }
    }
}
