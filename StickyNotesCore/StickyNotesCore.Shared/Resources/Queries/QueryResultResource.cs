namespace StickyNotesCore.Shared.Resources.Queries
{
	public record QueryResultResource<TModel>
	{
		public List<TModel> Items { get; init; } = new List<TModel>();
		public int TotalItems { get; init; }
	}
}
