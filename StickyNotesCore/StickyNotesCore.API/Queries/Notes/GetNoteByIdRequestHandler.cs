using MediatR;
using Microsoft.EntityFrameworkCore;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.API.Domain.Queries.Notes;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Queries.Notes
{
	public class GetNoteByIdRequestHandler : IRequestHandler<GetNoteByIdRequest, StatusResponse<Note>>
	{
		private readonly StickyNotesContext _context;

		public GetNoteByIdRequestHandler(StickyNotesContext context)
        {
			_context = context;
		}

        public async Task<StatusResponse<Note>> Handle(GetNoteByIdRequest request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var note = await _context.Notes.FirstOrDefaultAsync(note => note.Id == request.Id);
			if (note == null)
			{
				return new StatusResponse<Note>($"The note with ID {request.Id} was not found.", Status.NotFound);
			}

			return new StatusResponse<Note>(note, Status.Completed);
		}
	}
}
