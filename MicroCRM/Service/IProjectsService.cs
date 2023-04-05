using MicroCRM.Models;

namespace MicroCRM.Service
{
    public interface IProjectsService
    {
        public Task<IEnumerable<ProjectModel>> GetProjectsAsync();
        public Task<IEnumerable<ProjectModel>> GetProjectsByClientIdAsync(Guid id);
        public Task<ProjectModel> GetProjectByIdAsync(int id);
        public Task<ProjectModel> CreateNewProjectAsync(ProjectModel project);
        public Task<ProjectModel> UpdateProjectAsync(ProjectModel project);
        public Task<ProjectModel> DeleteProjectAsync(int id);

    }
}
