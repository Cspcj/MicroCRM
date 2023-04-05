using Microsoft.AspNetCore.Mvc;
using MicroCRM.Service;
using MicroCRM.Models;
using MicroCRM.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILogger<IClientService> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientsController(IClientService service, ILogger<IClientService> logger, UserManager<IdentityUser> userManager)
        {
            _clientService = service;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var _clients = await _clientService.GetClientsAsync();
            return View("Index", _clients);
        }

        [HttpGet]
        public IActionResult CreateNewClient()
        {
            return View("CreateNewClient");
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewClient(IFormCollection collection)
        {
            ClientModel _client = new ClientModel();
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adding new Client");
                TryUpdateModelAsync(_client);
                var result = await _clientService.CreateNewClientAsync(_client);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        
        public async Task<IActionResult> Details(Guid id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            return View("Details", client);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            return View("Delete", client);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            //var client = new ClientModel();
            //if (ModelState.IsValid)
            //{
            //    TryUpdateModelAsync(client);
            //}
            
            var _client = await _clientService.DeleteClientAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
            var client = await _clientService.GetClientByIdAsync(id);
            return View("Edit", client); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection collection)
        {
            var client = new ClientModel();
            if (ModelState.IsValid)
            {
                TryUpdateModelAsync(client);
                var _client = await _clientService.UpdateClientAsync(client);
            }
            return RedirectToAction("Index");
        }
    }

}
