using MediatR;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Commands.Notes
{
	public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, StatusResponse<Note>>
	{
		private readonly ILogger<CreateNoteCommandHandler> _logger;
		private readonly StickyNotesContext _context;

		public CreateNoteCommandHandler(StickyNotesContext context, ILogger<CreateNoteCommandHandler> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<StatusResponse<Note>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var note = new Note
			{
				Text = request.Text.Trim(),
				CreatedOn = DateTime.UtcNow,
			};

			_context.Notes.Add(note);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Added note ID {0}", note.Id);
			return new StatusResponse<Note>(note, Status.Created);
		}
	}
}
