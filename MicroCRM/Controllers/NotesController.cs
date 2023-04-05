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

            if (User.IsInRole("Manager") || User.IsInRole("Employee"))
            {
                var notes = await _noteService.GetNotesAsync();
                return View("Index", notes);
            }
            else
            {
                IdentityUser u = await _userManager.GetUserAsync(User);
                var clients = await _clientService.GetClientsAsync();
                var client = clients.FirstOrDefault(x => x.ClientEmail == u.Email);
                var projects = await _projectService.GetProjectsByClientIdAsync(client.ClientID);

                List<NoteModel> notes = new List<NoteModel>();

                foreach (var project in projects)
                {
                    notes.AddRange(await _noteService.GetNotesByProjectIdAsync(project.ProjectId));
                }

                return View("Index", notes);
            }
            return View("Index", "Home");
        }
            //    return View("Index", notes);
            //}



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
            projects = ((User.IsInRole("Manager")) ? await _projectService.GetProjectsAsync() : await _projectService.GetProjectsByClientIdAsync(client.ClientID)).ToList<ProjectModel>();
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
