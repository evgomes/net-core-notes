using StickyNotesCore.API.Domain.Data.Queries.Shared;
using StickyNotesCore.API.Domain.Models;

namespace StickyNotesCore.API.Domain.Data.Queries.Notes
{
	public class NotesQuery : SortedQuery<Note>
	{
		public string? Text { get; init; }
	}
}
