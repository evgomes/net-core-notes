using MediatR;
using StickyNotesCore.API.Domain.Data.Queries.Notes;
using StickyNotesCore.API.Domain.Data.Queries.Shared;
using StickyNotesCore.API.Domain.Models;

namespace StickyNotesCore.API.Domain.Queries.Notes
{
	public class ListNotesRequest : IRequest<QueryResult<Note>>
	{
		public NotesQuery Query { get; init; } = null!;
	}
}
