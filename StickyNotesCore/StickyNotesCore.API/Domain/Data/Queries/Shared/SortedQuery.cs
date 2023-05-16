namespace StickyNotesCore.API.Domain.Data.Queries.Shared
{
	public abstract class SortedQuery<TModel> : PagedQuery
	{
		public string? OrderBy { get; init; }
		public SortOrder SortOrder { get; init; }
	}
}
