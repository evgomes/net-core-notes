namespace StickyNotesCore.Shared.Responses
{
	public record StatusResponse : Response
	{
		public Status Status { get; init; }

		public StatusResponse(Status status)
		{
			Success = true;
			Message = string.Empty;
			Status = status;
		}

		public StatusResponse(string errorMessage, Status status)
		{
			Success = false;
			Message = errorMessage;
			Status = status;
		}
	}

	public record StatusResponse<TResource> : Response<TResource>
	{
		public Status Status { get; init; }

		public StatusResponse(TResource resource, Status? status) : base(resource)
		{
			Status = status ?? Status.Completed;
		}

		public StatusResponse(string errorMessage, Status status) : base(errorMessage)
		{
			Status = status;
		}
	}
}
