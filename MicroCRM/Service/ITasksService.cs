using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Service
{
    public interface ITasksService
    {
        // add tasks service interface
        Task<IEnumerable<TaskModel>> GetTasksAsync();
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid clientId);
        Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId);
        Task<IEnumerable<TaskModel>> GetTasksByStatusAsync(bool isCompleted);
        Task<TaskModel> GetTaskByIdAsync(Guid id);
        Task CreateTaskAsync(TaskModel task);
        Task<TaskModel> UpdateTaskAsync(TaskModel task);
        Task<TaskModel> DeleteTaskAsync(Guid id);
        Task ToggleTask(TaskModel task);
    }
}
