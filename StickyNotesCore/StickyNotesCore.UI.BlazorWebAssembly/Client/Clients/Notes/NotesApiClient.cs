using StickyNotesCore.Shared.Resources.Notes;
using StickyNotesCore.Shared.Resources.Queries;
using System.Text.Json;
using System.Text;
using StickyNotesCore.UI.BlazorWebAssembly.Client.Extensions;

namespace StickyNotesCore.UI.BlazorWebAssembly.Client.Clients.Notes
{
	public class NotesApiClient : INotesApiClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<NotesApiClient> _logger;

		public NotesApiClient(HttpClient client, ILogger<NotesApiClient> logger)
		{
			_client = client;
			_logger = logger;
		}

		public async Task<(bool Success, string? Message, NoteResource? Resource)> PostAsync(CreateNoteResource resource)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, $"api/notes");
			request.Content = new StringContent(JsonSerializer.Serialize(resource), Encoding.UTF8, "application/json");

			return await _client.SendRequestAsync<NoteResource>(request, "Could not create note:", _logger);
		}

		public async Task<(bool Success, string? Message, NoteResource? Resource)> PatchAsync(Guid id, PatchNoteResource resource)
		{
			var request = new HttpRequestMessage(HttpMethod.Patch, $"api/notes/{id}");
			request.Content = new StringContent(JsonSerializer.Serialize(resource), Encoding.UTF8, "application/json");

			return await _client.SendRequestAsync<NoteResource>(request, "Could not update note:", _logger);
		}

		public async Task<(bool Success, string? Message)> DeleteAsync(Guid id)
		{
			var request = new HttpRequestMessage(HttpMethod.Delete, $"api/notes/{id}");
			return await _client.SendRequestAsync(request, "Could not delete note:", _logger);
		}

		public async Task<(bool Success, string? Message, NoteResource? Resource)> GetByIdAsync(Guid id)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, $"api/notes/{id}");
			return await _client.SendRequestAsync<NoteResource>(request, "Could not get note by ID:", _logger);
		}

		public async Task<QueryResultResource<NoteResource>> ListAsync(NotesQueryResource queryResource)
		{
			var url = $"api/notes{queryResource.ToQueryString()}";
			return await _client.GetQueryResultAsync<NoteResource, ILogger<NotesApiClient>>(url, _logger);
		}
	}
}
