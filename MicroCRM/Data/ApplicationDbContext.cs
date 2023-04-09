using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroCRM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
                if (!Database.EnsureCreated())
                {
                    Database.Migrate();
                }
            }

            public DbSet<ClientModel> Clients { get; set; }
            public DbSet<ProjectModel> Projects { get; set; }
            public DbSet<NoteModel> Notes { get; set; }
            public DbSet<TaskModel> Tasks { get; set; } 
    }
}