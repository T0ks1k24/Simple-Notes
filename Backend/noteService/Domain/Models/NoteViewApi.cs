namespace Domain.Models;

public class NoteViewApi
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime UpdatedAt { get; set; }
}