using MicroCRM.Models;
using MicroCRM.Repositories;
using Microsoft.AspNetCore.Identity;
using MicroCRM.Service;

namespace MicroCRM.Service
{
    public class TasksService:ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IClientRepository _clientRepository;
        private readonly UserManager<IdentityUser> _userManager;


        public TasksService(ITasksRepository tasksRepository, IClientRepository clientRepository, UserManager<IdentityUser> userManager)
        {
            _tasksRepository = tasksRepository;
            _clientRepository = clientRepository;
            _userManager = userManager;
        }
        public async Task<IEnumerable<TaskModel>> GetTasksAsync()
        {
            return await _tasksRepository.GetTasksAsync();
        }
        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid clientId)
        {
            return await _tasksRepository.GetTasksByUserIdAsync(clientId);
        }
        public async Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _tasksRepository.GetTasksByProjectIdAsync(projectId);
        }
        public async Task<IEnumerable<TaskModel>> GetTasksByStatusAsync(bool isCompleted)
        {
            return await _tasksRepository.GetTasksByStatusAsync(isCompleted);
        }
        public async Task<TaskModel> GetTaskByIdAsync(Guid id)
        {
            return await _tasksRepository.GetTaskByIdAsync(id);
        }
        public async Task CreateTaskAsync(TaskModel task)
        {
            await _tasksRepository.CreateTaskAsync(task);
        }
        public async Task<TaskModel> UpdateTaskAsync(TaskModel task)
        {
            return await _tasksRepository.UpdateTaskAsync(task);
        }
        public async Task<TaskModel> DeleteTaskAsync(Guid id)
        {
            return await _tasksRepository.DeleteTaskAsync(id);
        }

        public async Task ToggleTask(TaskModel task)
        {
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                await _tasksRepository.UpdateTaskAsync(task);
            }
        }
    }
}
