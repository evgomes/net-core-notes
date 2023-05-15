using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StickyNotesCore.API.Controllers.Configuration
{
	public static class ModelStateExtensions
	{
		public static string GetErrorMessage(this ModelStateDictionary dictionary)
		{
			var formattedError = dictionary
				.SelectMany(message => message!.Value!.Errors)
				.Select(message => message.ErrorMessage)
				.Aggregate((previousError, error) => string.Concat(previousError, " ", error));

			return formattedError;
		}
	}
}
