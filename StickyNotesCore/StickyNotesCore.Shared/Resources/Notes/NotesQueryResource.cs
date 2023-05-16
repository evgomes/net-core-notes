using StickyNotesCore.Shared.Resources.Queries;

namespace StickyNotesCore.Shared.Resources.Notes
{
	public record NotesQueryResource : SortedQueryResource
	{
		public string? Text { get; init; }
	}
}
