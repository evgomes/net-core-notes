using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.Shared.Resources.Errors;
using StickyNotesCore.Shared.Resources.Notes;

namespace StickyNotesCore.API.Controllers
{
	[Route("api/notes")]
	[ApiController]
	public class NotesController : BaseController
	{
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public NotesController(IMediator mediator, IMapper mapper)
		{
			_mediator = mediator;
			_mapper = mapper;
		}

		/// <summary>
		/// Creates a new sticky note.
		/// </summary>
		/// <param name="resource">Resource containing the sticky note data.</param>
		/// <returns>Response for the request.</returns>
		[HttpPost]
		[ProducesResponseType(typeof(NoteResource), 201)]
		[ProducesResponseType(typeof(ErrorResource), 400)]
		public async Task<IActionResult> PostAsync([FromBody] CreateNoteResource resource)
		{
			var response = await _mediator.Send(_mapper.Map<CreateNoteCommand>(resource));
			var noteResource = (response.Success) ? _mapper.Map<NoteResource>(response.Resource) : null;
			return ApiResponse(response, $"/api/notes/{response?.Resource?.Id}", noteResource);
		}

		/// <summary>
		/// Updates a sticky note according to request data.
		/// </summary>
		/// <param name="id">Sticky note ID.</param>
		/// <param name="resource">Resource containing sticky note data to patch.</param>
		/// <returns>Response for the request.</returns>
		[HttpPatch("{id}")]
		[ProducesResponseType(typeof(NoteResource), 200)]
		[ProducesResponseType(typeof(ErrorResource), 400)]
		public async Task<IActionResult> PatchAsync(Guid id, [FromBody] PatchNoteResource resource)
		{
			var command = _mapper.Map<PatchNoteCommand>(resource);
			command.Id = id;

			var response = await _mediator.Send(command);

			var articleResource = (response.Success) ? _mapper.Map<NoteResource>(response.Resource) : null;
			return ApiResponse(response, resource: articleResource);
		}
	}
}