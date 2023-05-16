using MediatR;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Data.Queries.Extensions;
using StickyNotesCore.API.Domain.Data.Queries.Shared;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.API.Domain.Queries.Notes;

namespace StickyNotesCore.API.Queries.Notes
{
	public class ListNotesRequestHandler : IRequestHandler<ListNotesRequest, QueryResult<Note>>
	{
		private readonly StickyNotesContext _context;

		public ListNotesRequestHandler(StickyNotesContext context)
		{
			_context = context;
		}

		public async Task<QueryResult<Note>> Handle(ListNotesRequest request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			// Note: in a large application, we could have an index for the text field, and maybe an efficient stored procedure to retrieve data.
			return await _context
				.Notes
				.WithFilter(request.Query.Text, notes => notes.Text.ToLower().Contains(request.Query.Text!.ToLower()))
				.ToSortedQueryResultAsync(request.Query);
		}
	}
}
