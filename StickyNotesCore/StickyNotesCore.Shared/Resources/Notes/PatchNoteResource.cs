using System.ComponentModel.DataAnnotations;

namespace StickyNotesCore.Shared.Resources.Notes
{
	public record PatchNoteResource
	{
		[Required(ErrorMessage = "The text is required.")]
		[MaxLength(255, ErrorMessage = "The text must not have more than 255 characters.")]
		public string? Text { get; init; }
	}
}
