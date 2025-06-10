using Domain.Enums;

namespace Domain.Entities;

public class NoteList
{
    public Guid Id { get; set; }

    public Guid NoteId { get; set; }
    public Note Note { get; set; }

    public Guid UserId { get; set; }

    public AccessLevel AccessLevel { get; set; }
}