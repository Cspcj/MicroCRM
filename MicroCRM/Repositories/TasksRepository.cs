using MicroCRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MicroCRM.Service;
using MicroCRM.Data;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Repositories
{
    public class TasksRepository:ITasksRepository
    {
        // add dependencies
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TasksRepository> _logger;
        private readonly IProjectsService _projectsService;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksRepository(ApplicationDbContext context,
                       ILogger<TasksRepository> logger,
                       IProjectsService projectsService,
                       IClientService clientService,
                       UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _projectsService = projectsService;
            _clientService = clientService;
            _userManager = userManager;
        }
        public async Task<IEnumerable<TaskModel>> GetTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }
        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userId)
        {
            var tasks = await _context.Tasks.Where(x => x.UserId == userId).ToListAsync();
            return tasks;
        }
        public async Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId)
        {
            var tasks = await _context.Tasks.Where(x => x.ProjectId == projectId).ToListAsync();
            return tasks;
        }
        public async Task<IEnumerable<TaskModel>> GetTasksByStatusAsync(bool isCompleted)
        {
            var tasks = await _context.Tasks.Where(x => x.IsCompleted == isCompleted).ToListAsync();
            return tasks;
        }
        public async Task<TaskModel> GetTaskByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }
        public async Task CreateTaskAsync(TaskModel task)
        {
            // add the task
            task.DateCreated = DateTime.Now;
            _logger.LogInformation("Adding task");
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }
        public async Task<TaskModel> UpdateTaskAsync(TaskModel task)
        {
            // update the task
            _logger.LogInformation("Updating task");
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            // return the task
            return task;
        }
        public async Task<TaskModel> DeleteTaskAsync(Guid id)
        {
            // get the task
            var task = await _context.Tasks.FindAsync(id);
            // delete the task
            _logger.LogInformation("Deleting task");
            if (task == null)
            {
                return null;
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            // return the task
            return task;
        }   
    }
}
