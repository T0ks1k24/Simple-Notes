using Domain.Models;

namespace Application.Interfaces;

public interface INoteService
{
    Task CreateNoteAsync(NoteAddDto dto, Guid userId);
    Task<List<NoteDto>> GetNoteByUserIdAsync(Guid userId);
    Task UpdateNote(Guid id, NoteUpdateDto noteUpdateDto);
    Task DeleteNote(Guid noteId, Guid userId);
    Task<NoteDto?> GetNoteById(Guid noteId, Guid userId);

}