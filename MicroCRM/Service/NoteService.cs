using MicroCRM.Models;
using MicroCRM.Repositories;
using MicroCRM.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Service
{
    public class NoteService : INoteService
    {
        private readonly INotesRepositiory _notesRepository;
        private readonly IProjectsRepository _projectRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<NoteService> _logger;

        public NoteService(INotesRepositiory notesRepository, 
            UserManager<IdentityUser> userManager, 
            IProjectsRepository projectRepository,
            ILogger<NoteService> logger)
        {
            _notesRepository = notesRepository;
            _userManager = userManager;
            _logger = logger;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<NoteModel>> GetNotesAsync()
        {
            return await _notesRepository.GetNotesAsync();
        }
        public async Task<NoteModel> GetNoteAsync(Guid id)
        {
            return await _notesRepository.GetNoteAsync(id);
        }
        public async Task<NoteModel> AddNoteAsync(NoteModel note)
        {
            return await _notesRepository.AddNoteAsync(note);
        }
        public async Task<NoteModel> UpdateNoteAsync(NoteModel note)
        {
            return await _notesRepository.UpdateNoteAsync(note);
        }
        public async Task<NoteModel> DeleteNoteAsync(Guid id)
        {
            return await _notesRepository.DeleteNoteAsync(id);
        }
        public async Task<IEnumerable<NoteModel>> GetNotesByProjectIdAsync(int projectId)
        {
            return await _notesRepository.GetNotesByProjectIdAsync(projectId);
        }
    }
}
