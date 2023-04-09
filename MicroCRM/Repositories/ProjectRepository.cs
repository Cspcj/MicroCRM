using MicroCRM.Data;
using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroCRM.Repositories
{
    public class ProjectRepository : IProjectsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectRepository> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ProjectRepository(ApplicationDbContext context,
            ILogger<ProjectRepository> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<ProjectModel>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<ProjectModel> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<IEnumerable<ProjectModel>> GetProjectsByClientIdAsync(Guid id)
        {
            var projectList = await _context.Projects.Where(x => x.Id == id).ToListAsync();
           return projectList; 
        }

        public async Task<ProjectModel> CreateNewProjectAsync(ProjectModel project)
        {
            if (project != null)
            {
                project.IsArchived = false;
                project.IsDeleted = false;
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
            }
            return project;
        }

        public async Task<ProjectModel> UpdateProjectAsync(ProjectModel project)
        {
            if (project != null)
            {
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
            }
            return project;
        }

        public async Task<ProjectModel> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return project;
        }
    }
}
