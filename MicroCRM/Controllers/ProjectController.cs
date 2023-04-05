using MicroCRM.Models;
using MicroCRM.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace MicroCRM.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectsService _projectService;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProjectController(IProjectsService projectService,
            IClientService clientService,
            UserManager<IdentityUser> userManager)
        {
            _projectService = projectService;
            _clientService = clientService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Manager") || User.IsInRole("Employee"))
            {
                var result = await _projectService.GetProjectsAsync();
                return View("Index", result);
            }
            else
            {
                IdentityUser u = await _userManager.GetUserAsync(User);
                var clients = await _clientService.GetClientsAsync();
                var client = clients.FirstOrDefault(x => x.ClientEmail == u.Email);
                
                var result = await _projectService.GetProjectsByClientIdAsync(client.ClientID);
                return View("Index", result);
            }
            return View("Index","Home");   
        }

        [Authorize(Roles ="Employee,Manager")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var clients = await _clientService.GetClientsAsync();
            ViewBag.Clients = clients.ToList();

            var users = await _userManager.GetUsersInRoleAsync("Employee");
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            users.AddRange<IdentityUser>(managers);
            ViewBag.Owners = users.ToList();

            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            ProjectModel model = new ProjectModel();
            if (ModelState.IsValid)
            {
                await TryUpdateModelAsync(model);
                var project = await _projectService.CreateNewProjectAsync(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var _project = await _projectService.GetProjectByIdAsync(id);

            var clients = await _clientService.GetClientsAsync();
            ViewBag.Clients = clients.ToList();

            var users = await _userManager.GetUsersInRoleAsync("Employee");
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            users.AddRange<IdentityUser>(managers);
            ViewBag.Owners = users.ToList();

            return View("Edit", _project);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(IFormCollection collection)
        {
            var model = new ProjectModel();
            if (ModelState.IsValid && collection != null)
            {
                TryUpdateModelAsync(model);
                var result = await _projectService.UpdateProjectAsync(model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var _project = await _projectService.GetProjectByIdAsync(id);
            return View("Delete", _project);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            var _project = await _projectService.DeleteProjectAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);
            return View("Details", result);
        }
    }
}
