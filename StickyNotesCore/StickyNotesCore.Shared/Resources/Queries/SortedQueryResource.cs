using System.ComponentModel.DataAnnotations;

namespace StickyNotesCore.Shared.Resources.Queries
{
	public record SortedQueryResource : PagedQueryResource
	{
		/// <summary>
		/// The field name to sort records. The field name must be part of the matching model.
		/// </summary>
		public string? OrderBy { get; init; }

		/// <summary>
		/// The sort order for results. It can be 1 (Ascending) or 2 (Descending).
		/// </summary>
		[Required(ErrorMessage = "The sort order is required.")]
		public SortOrderResource SortOrder { get; init; }
	}
}
