using MicroCRM.Models;


namespace MicroCRM.Repositories
{
    public interface IProjectsRepository 
    {
        Task<IEnumerable<ProjectModel>> GetProjectsAsync();
        public Task<IEnumerable<ProjectModel>> GetProjectsByClientIdAsync(Guid id);
        Task<ProjectModel> GetProjectByIdAsync(int id);
        Task<ProjectModel> UpdateProjectAsync(ProjectModel project);
        Task<ProjectModel> CreateNewProjectAsync(ProjectModel project);
        Task<ProjectModel> DeleteProjectAsync(int id);
    }
}
