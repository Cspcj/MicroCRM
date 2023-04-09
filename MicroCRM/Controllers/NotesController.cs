using MicroCRM.Service;
using MicroCRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Controllers
{
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IProjectsService _projectService;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;

        public NotesController(INoteService noteService, IProjectsService projectService, 
                               IClientService clientService, UserManager<IdentityUser> userManager)
        {
            _noteService = noteService;
            _projectService = projectService;
            _clientService = clientService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Manager"))
            {
                var notes = await _noteService.GetNotesAsync();
                return View("Index", notes);
            }
            else if (User.IsInRole("Employee")) 
            {
                IdentityUser u = await _userManager.GetUserAsync(User);

                var projects = await _projectService.GetProjectsAsync();
                var result = new  List<NoteModel>();
                var notes = await _noteService.GetNotesAsync();

                foreach (var proj in projects)
                {
                    if(proj.Id == Guid.Parse(u.Id))
                    {
                        object value = notes.Where(x => x.ProjectId == proj.ProjectId).ToList();
                        result.AddRange((List<NoteModel>)value);
                    }
                }
                return View("Index", result);
            }
            else
            {
                // get current user 
                IdentityUser u = await _userManager.GetUserAsync(User);
                // get client coresponding to current user
                var clients = await _clientService.GetClientsAsync();
                var client = clients.FirstOrDefault(x => x.ClientEmail == u.Email);
                //get all projects for current client
                var projects = await _projectService.GetProjectsAsync();

                List<NoteModel> notes = new List<NoteModel>();

                foreach (var project in projects)
                {
                    List<NoteModel> local = new List<NoteModel>();    
                    if (project.ClientID == client.ClientID)
                    {
                        local = (List<NoteModel>) await _noteService.GetNotesByProjectIdAsync(project.ProjectId);
                    }
                    notes.AddRange(local);
                }

                return View("Index", notes);
            }
            return View("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var note = await _noteService.GetNoteAsync(id);
            return View("Details", note);
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
                projects = (List<ProjectModel>) await _projectService.GetProjectsAsync();
            }
            else if (User.IsInRole("Employee"))
            {
                var local = (List<ProjectModel>)await _projectService.GetProjectsAsync();
                projects = local.Where(x => x.Id == Guid.Parse(u.Id)).ToList<ProjectModel>();
            }
            else
            {
                var local = await _projectService.GetProjectsAsync();
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
            var model = new NoteModel();
            if (ModelState.IsValid)
            {
                await TryUpdateModelAsync(model);
                var note = await _noteService.AddNoteAsync(model);
                return RedirectToAction("Details", new { id = note.NoteId });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var note = await _noteService.GetNoteAsync(id);
            var projects = await _projectService.GetProjectsAsync();
            ViewBag.Projects = projects;
            return View("Edit", note);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, IFormCollection collection)
        {
            var model = new NoteModel();
            if (ModelState.IsValid)
            {
                await TryUpdateModelAsync(model);
                var note = await _noteService.UpdateNoteAsync(model);
                return RedirectToAction("Details", new { id = note.NoteId });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var note = await _noteService.GetNoteAsync(id);
            return View("Delete", note);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            var result = await _noteService.DeleteNoteAsync(id);
            return RedirectToAction("Index");
        }


    }
}
