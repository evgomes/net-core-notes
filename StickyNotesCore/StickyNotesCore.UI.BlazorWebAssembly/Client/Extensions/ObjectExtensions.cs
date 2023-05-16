using System.Collections;
using System.Text.Json;
using System.Web;

namespace StickyNotesCore.UI.BlazorWebAssembly.Client.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Serializes an object to a query string. Example:
		/// 
		/// var user = new User
		/// {
		///     Id = 1,
		///     Name = "Test"
		/// };
		/// 
		/// var queryString = user.ToQueryString();
		/// 
		/// Result: ?Id=1&Name=Test
		/// 
		/// Reference: https://stackoverflow.com/questions/6848296/how-do-i-serialize-an-object-into-query-string-format
		/// </summary>
		/// <param name="data">Object to serialize.</param>
		/// <returns>Query string.</returns>
		/// <remarks>
		/// To bind lists of values, ASP.NET Core expects parameters with the same name, but distinct values. 
		/// For example, consider a situation where we have the following list: 
		/// 
		/// var ids = new List<string> { 1, 2 }.
		/// 
		/// To correctly bind this list in the controller side, ASP.NET Core checks for the following pattern in the query string:
		/// "Ids=1&Ids=2".
		/// 
		/// This method handles lists serialization considering these rules.
		/// 
		/// Reference: https://stackoverflow.com/questions/43397851/pass-array-into-asp-net-core-route-query-string
		/// </remarks>
		public static string ToQueryString(this object data)
		{
			var jsonString = JsonSerializer.Serialize(data);
			var propertiesDictionary = JsonSerializer.Deserialize<IDictionary<string, object>>(jsonString) ?? new Dictionary<string, object>();

			var queryStringPieces = propertiesDictionary.SelectMany(param =>
			{
				if (IsEnumerableType(param.Value))
				{
					var values = (param.Value as IEnumerable<object> ?? new List<object>()).Select(value => value?.ToString() ?? string.Empty);
					return values.Select(value => $"{HttpUtility.UrlEncode(param.Key)}={HttpUtility.UrlEncode(value)}").ToArray();
				}
				else
				{
					return new string[] { $"{HttpUtility.UrlEncode(param.Key)}={HttpUtility.UrlEncode(param.Value?.ToString() ?? string.Empty)}" };
				}
			});

			var queryString = string.Join("&", queryStringPieces);
			return string.Concat("?", queryString);
		}

		private static bool IsEnumerableType(object value)
		{
			if (value == null)
			{
				return false;
			}

			var type = value.GetType();
			return type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type);
		}

		/// <summary>
		/// Clones a given object using JSON serialization.
		/// </summary>
		/// <typeparam name="T">Object type.</typeparam>
		/// <param name="source">Source object to clone.</param>
		/// <returns>Cloned object.</returns>
		public static T? Clone<T>(this T source)
		{
			if (ReferenceEquals(source, null))
			{
				return default;
			}

			var serialized = JsonSerializer.Serialize(source);
			return JsonSerializer.Deserialize<T>(serialized);
		}
	}
}
