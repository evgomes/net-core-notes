using StickyNotesCore.Shared.Resources.Errors;
using StickyNotesCore.Shared.Resources.Queries;
using System.Text.Json;

namespace StickyNotesCore.UI.BlazorWebAssembly.Client.Extensions
{
	public static class HttpClientExtensions
	{
		public static async Task<QueryResultResource<TModel>> GetQueryResultAsync<TModel, TLogger>
		(
			this HttpClient client,
			string url,
			ILogger logger
		)
		{
			try
			{
				var response = await client.GetAsync(url);
				var json = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					return JsonSerializer.Deserialize<QueryResultResource<TModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
				}

				var errorResource = JsonSerializer.Deserialize<ErrorResource>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				logger.LogWarning($"Request error: {errorResource!.Message}");

				return new QueryResultResource<TModel>
				{
					Items = new List<TModel>(),
					TotalItems = 0
				};
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Could not get query result.");
				return new QueryResultResource<TModel>
				{
					Items = new List<TModel>(),
					TotalItems = 0
				};
			}
		}

		public static async Task<(bool Success, string? Message, TResource? Resource)> SendRequestAsync<TResource>
		(
			this HttpClient client,
			HttpRequestMessage request,
			string partialErrorMessage,
			ILogger logger
		)
		{
			try
			{
				var response = await client.SendAsync(request);
				var json = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					var errorResource = JsonSerializer.Deserialize<ErrorResource>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					return (false, errorResource!.Message, default);
				}

				var resource = JsonSerializer.Deserialize<TResource>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return (true, null, resource);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Request error: {ex.Message} {ex.InnerException?.Message}");
				return (false, $"{partialErrorMessage} {ex.Message} {ex.InnerException?.Message}", default);
			}
		}

		public static async Task<(bool Success, string? Message)> SendRequestAsync
		(
			this HttpClient client,
			HttpRequestMessage request,
			string partialErrorMessage,
			ILogger logger
		)
		{
			try
			{
				var response = await client.SendAsync(request);
				var json = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					var errorResource = JsonSerializer.Deserialize<ErrorResource>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					return (false, errorResource!.Message);
				}

				return (true, null);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Request error: {ex.Message} {ex.InnerException?.Message}");
				return (false, $"{partialErrorMessage} {ex.Message} {ex.InnerException?.Message}");
			}
		}
	}
}
