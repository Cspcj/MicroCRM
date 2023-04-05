using MicroCRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using MicroCRM.Service;
using Microsoft.AspNetCore.Identity;
using MicroCRM.Models;

namespace MicroCRM.Controllers
{
    public class TasksController : Controller
    {
        // create controller for tasks
        private readonly ITasksService _tasksService;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<TasksController> _logger;
        private readonly IProjectsService _projectsService;

        public TasksController(ITasksService tasksService,
                            IClientService clientService,
                            IProjectsService projectsService,
                            UserManager<IdentityUser> userManager,
                            ILogger<TasksController> logger)
        {
            _tasksService = tasksService;
            _logger = logger;
            _userManager = userManager;
            _clientService = clientService;
            _projectsService = projectsService;
            _tasksService = tasksService;

        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Manager"))
            {
                var tasks = await _tasksService.GetTasksAsync();
                return View("Index", tasks);
            }
            else if (User.IsInRole("Employee") || User.IsInRole("Client"))
            {
                IdentityUser user = await _userManager.GetUserAsync(User);

                var tasks = await _tasksService.GetTasksByUserIdAsync(Guid.Parse(user.Id));
                return View("Index", tasks);
            }
            else
            {
                return View("Index", new List<TaskModel>());
            }
            return View("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IdentityUser u = await _userManager.GetUserAsync(User);
            var clients = await _clientService.GetClientsAsync();
            var client = clients.FirstOrDefault(x => x.ClientEmail == u.Email);

            var projects = new List<ProjectModel>();
            projects = ((User.IsInRole("Manager"))? await _projectsService.GetProjectsAsync() : await _projectsService.GetProjectsByClientIdAsync(client.ClientID)).ToList<ProjectModel>();
            projects.Add(new ProjectModel()
            {
                ProjectId = -1,
                ProjectName = "Unassigned",
                ProjectDescription = "Unassigned",
                ClientID = Guid.Empty,
                IsArchived = true,
                IsDeleted = false,
                ProjectLocation = "Unassigned",
                ProjectLocationCity = "Unassigned",
                Id = Guid.Parse(u.Id),
                Region = "Unassigned"
            });
            ViewBag.Projects = projects;
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var task = new TaskModel();
            if (ModelState.IsValid)
            {
                await TryUpdateModelAsync(task);
                await _tasksService.CreateTaskAsync(task);
                return RedirectToAction("Index");
            }
            return View("Create", task);
        }

    }
}
