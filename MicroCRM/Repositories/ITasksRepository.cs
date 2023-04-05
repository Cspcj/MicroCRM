using MicroCRM.Models;


namespace MicroCRM.Repositories
{
    public interface ITasksRepository
    {
        //task repository interface
        Task<IEnumerable<TaskModel>> GetTasksAsync();
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid clientId);
        Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId);
        Task<IEnumerable<TaskModel>> GetTasksByStatusAsync(bool isCompleted);
        Task<TaskModel> GetTaskByIdAsync(Guid id);
        Task CreateTaskAsync(TaskModel task);
        Task<TaskModel> UpdateTaskAsync(TaskModel task);
        Task<TaskModel> DeleteTaskAsync(Guid id);
        
    }
}
