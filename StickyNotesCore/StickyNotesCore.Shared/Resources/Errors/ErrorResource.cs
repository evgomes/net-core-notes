namespace StickyNotesCore.Shared.Resources.Errors
{
	public record ErrorResource
	{
		public bool Success => false;
		public string Message { get; private set; }

		public ErrorResource(string? message)
		{
			Message = message ?? "An error happened when calling the API. Please, try again later.";
		}
	}
}
