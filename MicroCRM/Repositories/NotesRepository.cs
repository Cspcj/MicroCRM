using MicroCRM.Models;
using MicroCRM.Repositories;
using MicroCRM.Service;
using MicroCRM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroCRM.Repositories
{
    public class NotesRepository : INotesRepositiory
    {
        // add dependencies
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotesRepository> _logger;

        public NotesRepository(ApplicationDbContext context,
            ILogger<NotesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<NoteModel>> GetNotesAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<IEnumerable<NoteModel>> GetNotesByProjectIdAsync(int projectId)
        {
            var notes = await _context.Notes.Where(x => x.ProjectId == projectId).ToListAsync();
            return notes;
        }

        public async Task<NoteModel> GetNoteAsync(Guid id)
        {
            return await _context.Notes.FindAsync(id);
        }

        public async Task<NoteModel> AddNoteAsync(NoteModel note)
        {
            // add the note
            note.DateCreated = DateTime.Now;
            _logger.LogInformation("Adding note");
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            // return the note
            return note;
        }

        public async Task<NoteModel> UpdateNoteAsync(NoteModel note)
        {
            // update the note
            _logger.LogInformation("Updating note");
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            // return the note
            return note;
        }

        public async Task<bool> EntryExists(Guid id)
        {
            return await _context.Notes.AnyAsync(x=>x.NoteId == id);
        }

        public async Task<NoteModel> DeleteNoteAsync(Guid id)
        {
            // get the note
            var note = await _context.Notes.FindAsync(id);
            // delete the note
            if (note != null)
            {
                _logger.LogInformation("Deleting note");
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
            // return the note
            return note;
        }
    }
}
