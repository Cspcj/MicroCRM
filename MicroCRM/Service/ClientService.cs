using MicroCRM.Models;
using MicroCRM.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace MicroCRM.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly ILogger<IClientService> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public ClientService(IClientRepository repository,
            ILogger<IClientService> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IProjectsRepository projectsRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _clientRepository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _projectsRepository = projectsRepository;
        }

        public async Task<IEnumerable<ClientModel>> GetClientsAsync()
        {
            return await _clientRepository.GetClientsAsync();
        }

        public async Task<ClientModel> GetClientByIdAsync(Guid id)
        {
            return await _clientRepository.GetClientByIdAsync(id);
        }

        public async Task<IdentityUser> CreateNewIdentityUser(ClientModel client)
        {
            try
            {
                var user = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = client.ClientEmail,
                    NormalizedUserName = client.ClientEmail.Normalize(),
                    Email = client.ClientEmail,
                    NormalizedEmail = client.ClientEmail.Normalize(),
                    EmailConfirmed = true,
                    PhoneNumber = client.ClientPhoneNumber

                };
                var result = await _userManager.CreateAsync(user, _configuration.GetValue<string>("defaultClientPassword"));


                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Client");
                    return user;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(string.Empty, error.Description);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new user");
            }
            return null;
        }

        public async Task<ClientModel> CreateNewClientAsync(ClientModel client)
        {

            if (client != null)
            {
                var user = await CreateNewIdentityUser(client);
                if (user != null)
                {
                    client.Id = Guid.Parse(user.Id);
                    await _clientRepository.CreateClientAsync(client);
                }
                return client;
            }



            //{
            //    _logger.LogInformation("CreateNewClientAsync triggered");
            //    IdentityUser user = new IdentityUser
            //    {
            //        //Id = Guid.NewGuid().ToString(),
            //        UserName = client.ClientEmail,
            //        NormalizedUserName = client.ClientEmail.Normalize(),
            //        Email = client.ClientEmail,
            //        NormalizedEmail = client.ClientEmail.Normalize(),
            //        EmailConfirmed = true,
            //        PhoneNumber = client.ClientPhoneNumber                    
            //    };

            //    var pass = _configuration.GetValue<string>("defaultClientPassword");
            //    var result = await _userManager.CreateAsync(user, pass);
            //    _logger.LogInformation($"Creating client: {result}");
            //    _logger.LogInformation("____------------------------------------------------------");
            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("User created a new account with password.");
            //        var roleResult = await _userManager.AddToRoleAsync(user, "Client");
            //        if (roleResult.Succeeded)
            //        {
            //            _logger.LogInformation("User added to role.");
            //        }
            //        else
            //        {
            //            _logger.LogInformation("User not added to role.");
            //        }
            //        client.ClientID = Guid.NewGuid();
            //        await _clientRepository.CreateClientAsync(client);

            //    }
            //    else
            //    {
            //        _logger.LogInformation("User not created.");
            //    }
            return client;
        }

        public async Task<ClientModel> CreateClientFromExistingAsync(ClientModel client, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClientModel> DeleteClientAsync(Guid id)
        {
            // delete client from userManager
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client != null)
            {
                var user = await _userManager.FindByIdAsync(client.Id.ToString());
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User deleted.");
                    }
                    else
                    {
                        _logger.LogInformation("User not deleted.");
                    }
                }
                return await _clientRepository.DeleteClientAsync(id);
            }

            // replacing all the references to the said client in projects
            // and archiving the projects in witch the client was involved

            var projects = await _projectsRepository.GetProjectsAsync();

            if (projects != null)
            {
                var projectsToArchive = projects.Where(x => x.ClientID == id).ToList();
                foreach (var project in projectsToArchive)
                {
                    project.ClientID = Guid.Empty;
                    project.IsArchived = true;
                    project.ProjectName += " - Archived project";
                    project.ProjectDescription += $"\n Client name: {client.ClientName} " +
                                                    $"\n Client Company: {client.ClientCompany} " +
                                                    $"\n Client contact: {client.ClientEmail} {client.ClientPhoneNumber}";
                    await _projectsRepository.UpdateProjectAsync(project);
                }
            }
            return client;
        }

        public async Task<ClientModel> UpdateClientAsync(ClientModel client)
        {

            return await _clientRepository.UpdateClientAsync(client);
        }

        public async Task<ClientModel> GetClientByIdentityUserIdAsync(Guid userId)
        {
            ClientModel client = await _clientRepository.GetClientByIdentityUserIdAsync(userId);
            return client;
        }
    }
}
