namespace StickyNotesCore.API.Domain.Models
{
	public class Note
	{
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Text { get; set; } = null!;
    }
}
