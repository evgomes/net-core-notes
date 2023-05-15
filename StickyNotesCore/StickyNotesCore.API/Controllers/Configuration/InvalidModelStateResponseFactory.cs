using Microsoft.AspNetCore.Mvc;
using StickyNotesCore.Shared.Resources.Errors;

namespace StickyNotesCore.API.Controllers.Configuration
{
	public static class InvalidModelStateResponseFactory
	{
		public static IActionResult ProduceErrorResponse(ActionContext context)
		{
			var error = context.ModelState.GetErrorMessage();
			var response = new ErrorResource(message: error);

			return new BadRequestObjectResult(response);
		}
	}
}
