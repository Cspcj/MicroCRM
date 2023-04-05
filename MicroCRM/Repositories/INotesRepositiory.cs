using MicroCRM.Models;

namespace MicroCRM.Repositories
{
    public interface INotesRepositiory
    {
        Task<IEnumerable<NoteModel>> GetNotesAsync();
        Task<NoteModel> GetNoteAsync(Guid id);
        Task<NoteModel> AddNoteAsync(NoteModel note);
        Task<NoteModel> UpdateNoteAsync(NoteModel note);
        Task<NoteModel> DeleteNoteAsync(Guid id);
        Task<IEnumerable<NoteModel>> GetNotesByProjectIdAsync(int projectId);
    }
}
