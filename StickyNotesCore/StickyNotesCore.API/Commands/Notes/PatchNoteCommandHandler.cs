using MediatR;
using Microsoft.EntityFrameworkCore;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Commands.Notes
{
	public class PatchNoteCommandHandler : IRequestHandler<PatchNoteCommand, StatusResponse<Note>>
	{
		private readonly ILogger<CreateNoteCommandHandler> _logger;
		private readonly StickyNotesContext _context;

		public PatchNoteCommandHandler(StickyNotesContext context, ILogger<CreateNoteCommandHandler> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<StatusResponse<Note>> Handle(PatchNoteCommand request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var note = await _context.Notes.FirstOrDefaultAsync(note => note.Id == request.Id);
			if (note == null)
			{
				return new StatusResponse<Note>($"The note with ID {request.Id} was not found.", Status.NotFound);
			}

			note.Text = request.Text.Trim();
			note.ModifiedOn = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			_logger.LogInformation("Patched note ID {0}", note.Id);
			return new StatusResponse<Note>(note, Status.Patched);
		}
	}
}
