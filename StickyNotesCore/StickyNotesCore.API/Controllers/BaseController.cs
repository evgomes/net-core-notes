using Microsoft.AspNetCore.Mvc;
using StickyNotesCore.Shared.Resources.Errors;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Controllers
{
	public class BaseController : Controller
	{
		/// <summary>
		/// Returns an API response according to the status of a status response.
		/// </summary>
		/// <param name="response">Status response object.</param>
		/// <returns>Response for the request.</returns>
		/// <exception cref="NotImplementedException">This exception is thrown when there is no response type for a given status.</exception>
		protected virtual IActionResult ApiResponse(StatusResponse response)
		{
			if (response == null)
			{
				return BadRequest(new ErrorResource("An unhandled error happened during your request. Please, try again later."));
			}

			switch (response.Status)
			{
				case Status.Completed:
				case Status.Created:
				case Status.Patched:
				case Status.Deleted:
					return NoContent();
				case Status.NotFound:
					return NotFound(new ErrorResource(response.Message));
				case Status.InvalidData:
				case Status.OperationError:
					return BadRequest(new ErrorResource(response.Message));
				default:
					throw new NotImplementedException(nameof(response.Status));
			}
		}

		/// <summary>
		/// Returns an API response according to the status of a status response.
		/// </summary>
		/// <typeparam name="TModel">Status response model type.</typeparam>
		/// <typeparam name="TResource">Resource type to return in case of a successful request.</typeparam>
		/// <param name="response">Status response object.</param>
		/// <param name="createdUri">The URL to retrieve the created resource in case of a created response.</param>
		/// <param name="resource">Resource to return as part of the request.</param>
		/// <returns>Response for the request.</returns>
		/// <exception cref="NotImplementedException">This exception is thrown when there is no response type for a given status.</exception>
		protected virtual IActionResult ApiResponse<TModel, TResource>(StatusResponse<TModel>? response, string? createdUri = null, TResource? resource = default)
		{
			if (response == null)
			{
				return BadRequest(new ErrorResource("An unexpected error happened during your request. Please, try again later."));
			}

			switch (response.Status)
			{
				case Status.Created:
					return Created(createdUri!, resource);
				case Status.Completed:
				case Status.Patched:
				case Status.Deleted:
					return Ok(resource);
				case Status.NotFound:
					return NotFound(new ErrorResource(response.Message));
				case Status.InvalidData:
				case Status.OperationError:
					return BadRequest(new ErrorResource(response.Message));
				default:
					throw new NotImplementedException(nameof(response.Status));
			}
		}
	}
}
