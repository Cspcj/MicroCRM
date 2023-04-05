using MicroCRM.Models;
using MicroCRM.Repositories;

namespace MicroCRM.Service
{
    public interface INoteService
    {
        Task<IEnumerable<NoteModel>> GetNotesAsync();
        Task<NoteModel> GetNoteAsync(Guid id);
        Task<NoteModel> AddNoteAsync(NoteModel note);
        Task<NoteModel> UpdateNoteAsync(NoteModel note);
        Task<NoteModel> DeleteNoteAsync(Guid id);
        Task<IEnumerable<NoteModel>> GetNotesByProjectIdAsync(int projectId);
    }
}
