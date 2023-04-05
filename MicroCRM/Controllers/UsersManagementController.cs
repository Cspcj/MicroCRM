using Microsoft.AspNetCore.Mvc;
using MicroCRM.Service;
using MicroCRM.Repositories;
using MicroCRM.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;

namespace MicroCRM.Controllers
{
    [Authorize (Roles = "Manager")]
    public class UsersManagementController : Controller
    {

        // create constructor with dependecies
        private readonly IUserViewService _userService;
        private readonly ILogger<IUserViewService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;


        public UsersManagementController(IUserViewService userViewService, 
            ILogger<IUserViewService> logger, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userService = userViewService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View("Index", await _userService.GetUsersAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            var _roles = _roleManager.Roles.Where(x=> x.Name!= "Client").ToList();
            ViewBag.data = _roles;
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            //try
            //{
            //    _userService.CreateNewUser(collection["Email"], collection["PhoneNumber"], collection["Role"]);
            //}
            //catch (Exception ex)
            //{

            //}
            //return RedirectToAction("Index");

            try
            {                
                var user = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = collection["Email"],
                    Email = collection["Email"],
                    PhoneNumber = collection["PhoneNumber"],
                    EmailConfirmed = true
                    
                };
                var result = await _userManager.CreateAsync(user, _configuration.GetValue<string>("defaultUserPassword"));
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(collection["Role"]);
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var _user = await _userService.GetUserByIdAsync(id);
            return View("Delete", _user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            IdentityUser _user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(_user);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var _user = await _userService.GetUserByIdAsync(id);
            var _roles = _roleManager.Roles.ToList();
            ViewBag.data = _roles;
            return View("Edit", _user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(id.ToString());
                _user.UserName = collection["Email"];
                _user.Email = collection["Email"];
                _user.PhoneNumber = collection["PhoneNumber"];
                await _userManager.UpdateAsync(_user);

                var _u = await _userService.GetUserByIdAsync(id);
                var oldRole = await _roleManager.FindByNameAsync(_u.Role);
                await _userManager.RemoveFromRoleAsync(_user, oldRole.Name);

                var _role = await _roleManager.FindByIdAsync(collection["Role"]);
                await _userManager.AddToRoleAsync(_user, _role.Name);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Edit");
            }
        }
    }
}
