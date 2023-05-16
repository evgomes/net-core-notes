using MediatR;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Domain.Commands.Notes
{
	public record DeleteNoteCommand : IRequest<StatusResponse>
	{
		public Guid Id { get; init; }
	}
}
