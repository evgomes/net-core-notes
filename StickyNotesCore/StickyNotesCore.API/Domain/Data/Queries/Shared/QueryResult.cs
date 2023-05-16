namespace StickyNotesCore.API.Domain.Data.Queries.Shared
{
    public record QueryResult<TModel>
    {
        public List<TModel> Items { get; init; } = new List<TModel>();
        public int TotalItems { get; init; }
    }
}
