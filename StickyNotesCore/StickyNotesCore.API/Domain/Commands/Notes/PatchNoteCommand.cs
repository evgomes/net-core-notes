using MediatR;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Domain.Commands.Notes
{
	public class PatchNoteCommand : IRequest<StatusResponse<Note>>
	{
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
	}
}
