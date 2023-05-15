using MediatR;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Domain.Commands.Notes
{
	public class CreateNoteCommand : IRequest<StatusResponse<Note>>
	{
		public string Text { get; set; } = null!;
	}
}
