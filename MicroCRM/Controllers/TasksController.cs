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
            else if (User.IsInRole("Employee"))
            {
                IdentityUser user = await _userManager.GetUserAsync(User);
                // get coresponding to current user

                var projects = await _projectsService.GetProjectsAsync();
                var taskList = await _tasksService.GetTasksAsync();
                var tasks = new List<TaskModel>();
                foreach (var item in projects)
                {
                    if (item.Id == Guid.Parse(user.Id))
                    {
                        var local_tasks = taskList.Where(x => x.ProjectId == item.ProjectId).ToList<TaskModel>();
                        tasks.AddRange(local_tasks);
                    }
                }


                //var tasks = await _tasksService.GetTasksByUserIdAsync(Guid.Parse(user.Id));
                return View("Index", tasks);
            }
            else
            {
                IdentityUser user = await _userManager.GetUserAsync(User);
                // get coresponding to current user
                var clients = await _clientService.GetClientsAsync();
                var client = clients.FirstOrDefault(x => x.ClientEmail == user.Email);

                var projects = await _projectsService.GetProjectsAsync();
                var tasks = new List<TaskModel>();
                foreach (var item in projects)
                {
                    if (item.ClientID == client.ClientID)
                    {
                        var local_tasks = await _tasksService.GetTasksByProjectIdAsync(item.ProjectId);
                        tasks.AddRange(local_tasks);    
                    }
                }
                return View("Index", tasks);
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
            
            if (User.IsInRole("Manager"))
            {
                projects = (List<ProjectModel>)await _projectsService.GetProjectsAsync();
            }
            else if (User.IsInRole("Employee"))
            {
                var local = (List<ProjectModel>)await _projectsService.GetProjectsAsync();
                projects = local.Where(x => x.Id == Guid.Parse(u.Id)).ToList<ProjectModel>();
            }
            else
            {
                var local = await _projectsService.GetProjectsAsync();
                projects = local.Where(x => x.ClientID == client.ClientID).ToList<ProjectModel>();
            }
            //{
            //    var allProjects = await _projectService.GetProjectsAsync();
            //}
            //projects = ((User.IsInRole("Manager")) ? await _projectService.GetProjectsAsync() : await _projectService.GetProjectsByClientIdAsync(client.ClientID)).ToList<ProjectModel>();
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

        [HttpGet]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var task = await _tasksService.GetTaskByIdAsync(id);
            await _tasksService.ToggleTask(task);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAsync(Guid id) 
        { 
            var task = await _tasksService.GetTaskByIdAsync(id);
            var userName = await _userManager.GetUserNameAsync(await _userManager.FindByIdAsync(task.UserId.ToString()));
            ViewBag.UserName = userName;
            return View("Delete", task);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            var task = new TaskModel();
            if (ModelState.IsValid)
            {
                await TryUpdateModelAsync(task);
                await _tasksService.DeleteTaskAsync(id);
            }
            return RedirectToAction("Index");
        }

    }
}
