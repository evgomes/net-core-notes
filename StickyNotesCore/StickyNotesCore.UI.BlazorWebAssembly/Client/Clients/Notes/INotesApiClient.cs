using StickyNotesCore.Shared.Resources.Notes;
using StickyNotesCore.Shared.Resources.Queries;

namespace StickyNotesCore.UI.BlazorWebAssembly.Client.Clients.Notes
{
	public interface INotesApiClient
	{
		Task<(bool Success, string? Message, NoteResource? Resource)> PostAsync(CreateNoteResource resource);
		Task<(bool Success, string? Message, NoteResource? Resource)> PatchAsync(Guid id, PatchNoteResource resource);
		Task<(bool Success, string? Message)> DeleteAsync(Guid id);
		Task<(bool Success, string? Message, NoteResource? Resource)> GetByIdAsync(Guid id);
		Task<QueryResultResource<NoteResource>> ListAsync(NotesQueryResource queryResource);
	}
}
