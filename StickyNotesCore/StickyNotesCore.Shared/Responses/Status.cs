namespace StickyNotesCore.Shared.Responses
{
	public enum Status : byte
	{
		Completed = 1,
		Created = 2,
		Patched = 3,
		Deleted = 4,
		NotFound = 5,
		InvalidData = 6,
		OperationError = 7,
	}
}
