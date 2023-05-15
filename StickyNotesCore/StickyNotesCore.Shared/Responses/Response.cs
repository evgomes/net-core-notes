namespace StickyNotesCore.Shared.Responses
{
	public record Response
	{
		public bool Success { get; init; }
		public string? Message { get; init; }
	}

	public record Response<TResource> : Response
	{
		public TResource? Resource { get; init; }

		public Response(TResource resource)
		{
			Success = true;
			Message = string.Empty;
			Resource = resource;
		}

		public Response(string errorMessage)
		{
			Success = false;
			Message = errorMessage;
			Resource = default;
		}
	}
}
