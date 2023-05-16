using MediatR;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Domain.Queries.Notes
{
	public record GetNoteByIdRequest : IRequest<StatusResponse<Note>>
	{
		public Guid Id { get; init; }
	}
}
