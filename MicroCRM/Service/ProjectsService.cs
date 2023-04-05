using MicroCRM.Models;
using MicroCRM.Repositories;

namespace MicroCRM.Service
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserViewService _userViewService;
        private readonly IClientService _clientService;

        public ProjectsService(IProjectsRepository projectRepository, 
            IClientRepository clientRepository, 
            IUserViewService userViewService, 
            IClientService clientService)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _userViewService = userViewService;
            _clientService = clientService;
        }
        public async Task<IEnumerable<ProjectModel>> GetProjectsAsync()
        {
            return await _projectRepository.GetProjectsAsync();
        }
        public async Task<ProjectModel> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task<IEnumerable<ProjectModel>> GetProjectsByClientIdAsync(Guid id)
        {
            return await _projectRepository.GetProjectsByClientIdAsync(id);
        }

        public async Task<ProjectModel> CreateNewProjectAsync(ProjectModel project)
        {
            return await _projectRepository.CreateNewProjectAsync(project);
        }
        public async Task<ProjectModel> UpdateProjectAsync(ProjectModel project)
        {
            return await _projectRepository.UpdateProjectAsync(project);
        }
        public async Task<ProjectModel> DeleteProjectAsync(int id)
        {
            return await _projectRepository.DeleteProjectAsync(id);
        }
    }
}
