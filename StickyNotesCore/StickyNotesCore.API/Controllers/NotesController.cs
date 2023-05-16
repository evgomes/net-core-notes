using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Data.Queries.Notes;
using StickyNotesCore.API.Domain.Queries.Notes;
using StickyNotesCore.Shared.Resources.Errors;
using StickyNotesCore.Shared.Resources.Notes;
using StickyNotesCore.Shared.Resources.Queries;

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

		/// <summary>
		/// Deletes a sticky note using the note's ID.
		/// </summary>
		/// <param name="id">Note ID.</param>
		/// <returns>Response for the request.</returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(typeof(ErrorResource), 400)]
		public async Task<IActionResult> DeleteAsync(Guid id)
		{
			return ApiResponse(await _mediator.Send(new DeleteNoteCommand { Id = id }));
		}

		/// <summary>
		/// Lists all the sticky notes that match provided filters.
		/// </summary>
		/// <param name="queryResource">Resource containing query filters.</param>
		/// <returns>The query result.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(QueryResultResource<NoteResource>), 200)]
		[ProducesResponseType(typeof(ErrorResource), 400)]
		[ProducesResponseType(typeof(ErrorResource), 401)]
		[ProducesResponseType(typeof(ErrorResource), 403)]
		public async Task<IActionResult> ListAsync([FromQuery] NotesQueryResource queryResource)
		{
			var queryResult = await _mediator.Send(new ListNotesRequest { Query = _mapper.Map<NotesQuery>(queryResource) });
			return Ok(_mapper.Map<QueryResultResource<NoteResource>>(queryResult));
		}

		/// <summary>
		/// Retrieves a sticky note by its ID.
		/// </summary>
		/// <param name="id">Note ID.</param>
		/// <returns>Response for the request.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(NoteResource), 200)]
		[ProducesResponseType(typeof(ErrorResource), 400)]
		public async Task<IActionResult> GetByIdAsync(Guid id)
		{
			var response = await _mediator.Send(new GetNoteByIdRequest { Id = id });
			var noteResource = (response.Success) ? _mapper.Map<NoteResource>(response.Resource) : null;
			return ApiResponse(response, resource: noteResource);
		}
	}
}