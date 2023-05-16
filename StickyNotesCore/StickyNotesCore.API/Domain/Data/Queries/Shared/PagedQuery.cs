namespace StickyNotesCore.API.Domain.Data.Queries.Shared
{
	public abstract class PagedQuery
	{
		public int Page { get; init; }
		public int ItemsPerPage { get; init; }

		public PagedQuery()
		{
			if (Page <= 0)
			{
				Page = 1;
			}

			if (ItemsPerPage <= 0)
			{
				ItemsPerPage = 10;
			}
		}
	}
}
