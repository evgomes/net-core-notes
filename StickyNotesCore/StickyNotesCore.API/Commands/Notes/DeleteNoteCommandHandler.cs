using MediatR;
using Microsoft.EntityFrameworkCore;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Commands.Notes
{
	public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, StatusResponse>
	{
		private readonly StickyNotesContext _context;
		private readonly ILogger<DeleteNoteCommandHandler> _logger;

		public DeleteNoteCommandHandler(StickyNotesContext context, ILogger<DeleteNoteCommandHandler> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<StatusResponse> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var note = await _context.Notes.FirstOrDefaultAsync(note => note.Id == request.Id);
			if (note == null)
			{
				return new StatusResponse($"The note with ID {request.Id} was not found.", Status.NotFound);
			}

			_context.Notes.Remove(note);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Deleted note ID: {Id}.", note.Id);
			return new StatusResponse(Status.Deleted);
		}
	}
}
