using StickyNotesCore.API.Domain.Data.Queries.Shared;
using System.Linq.Expressions;

namespace StickyNotesCore.API.Domain.Data.Queries.Extensions
{
	public static class QueryableExtensions
	{
		/// <summary>
		/// Applies pagination to an IQueryable instance.
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="query">Pagination data.</param>
		/// <returns>Paged queryable.</returns>
		public static IQueryable<TModel> WithPagination<TModel>(this IQueryable<TModel> queryable, PagedQuery query)
		{
			if (query == null)
			{
				return queryable;
			}

			return queryable.Skip((query.Page - 1) * query.ItemsPerPage).Take(query.ItemsPerPage);
		}

		/// <summary>
		/// Applies sorting to a query using a string field to determine the sort field name.
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="query">Sorting data.</param>
		/// <returns>Sorted query.</returns>
		public static IQueryable<TModel> WithSorting<TModel>(this IQueryable<TModel> queryable, SortedQuery<TModel> query)
		{
			if (query == null || string.IsNullOrWhiteSpace(query.OrderBy))
			{
				return queryable;
			}

			return (query.SortOrder == SortOrder.Ascending) ? queryable.OrderBy(query.OrderBy) : queryable.OrderByDescending(query.OrderBy);
		}

		/// <summary>
		/// Returns a query result from a paged query.
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="query">Pagination data.</param>
		/// <returns>Query result.</returns>
		public static async Task<QueryResult<TModel>> ToQueryResultAsync<TModel>(this IQueryable<TModel> queryable, PagedQuery query)
		{
			return await Task.Run(() =>
			{
				var totalItems = queryable.Count();
				var items = queryable.WithPagination(query).ToList();

				return new QueryResult<TModel>
				{
					Items = items,
					TotalItems = totalItems
				};
			});
		}

		/// <summary>
		/// Returns a query result from a sorted query.
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="query">Sorting and pagination data.</param>
		/// <returns>Query result.</returns>
		public static async Task<QueryResult<TModel>> ToSortedQueryResultAsync<TModel>(this IQueryable<TModel> queryable, SortedQuery<TModel> query)
		{
			return await Task.Run(() =>
			{
				var totalItems = queryable.Count();
				var items = queryable
					.WithSorting(query)
					.WithPagination(query)
					.ToList();

				return new QueryResult<TModel>
				{
					Items = items,
					TotalItems = totalItems
				};
			});
		}

		/// <summary>
		/// Checks if a given argument matches its default value. If not, applies a filter expression to a query.
		/// 
		/// Reference: https://stackoverflow.com/questions/65351/null-or-default-comparison-of-generic-argument-in-c-sharp
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <typeparam name="TField">Field type to compare with its default value.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="field">Field to validate the default value.</param>
		/// <param name="filterExpression">Filtering expression.</param>
		/// <returns>Query.</returns>
		public static IQueryable<TModel> WithFilter<TModel, TField>(this IQueryable<TModel> queryable, TField field, Expression<Func<TModel, bool>> filterExpression)
		{
			if (!object.Equals(field, default(TField)))
			{
				queryable = queryable.Where(filterExpression);
			}

			return queryable;
		}

		/// <summary>
		/// Applies ascending sorting to a query using a field name for that.
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="field">Field name (a valid property name).</param>
		/// <returns>Query.</returns>
		public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> queryable, string field)
		{
			return BuildOrderBy(queryable, field, "OrderBy");
		}

		/// <summary>
		/// Applies descending sorting to a query using a field name for that.
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="field">Field name (a valid property name).</param>
		/// <returns>Query.</returns>
		public static IQueryable<TModel> OrderByDescending<TModel>(this IQueryable<TModel> queryable, string field)
		{
			return BuildOrderBy(queryable, field, "OrderByDescending");
		}

		/// <summary>
		/// Builds a generic lambda expression to apply ordering to a query using a field name.
		/// Reference: https://stackoverflow.com/questions/34899933/sorting-using-property-name-as-string/34966471
		/// </summary>
		/// <typeparam name="TModel">Entity type.</typeparam>
		/// <param name="queryable">Query.</param>
		/// <param name="field">Field name (a valid property name).</param>
		/// <param name="orderByMethodName">It should be "OrderBy" or "OrderByDescending".</param>
		/// <returns></returns>
		private static IQueryable<TModel> BuildOrderBy<TModel>(this IQueryable<TModel> queryable, string field, string orderByMethodName = "OrderBy")
		{
			if (string.IsNullOrWhiteSpace(field))
			{
				return queryable;
			}

			if (typeof(TModel).GetProperty(field) == null)
			{
				throw new ArgumentException($"There is no field {field} on {typeof(TModel).Name} to apply ordering.");
			}

			// LAMBDA: x => x[PropertyName]
			var parameter = Expression.Parameter(typeof(TModel), "x");
			Expression property = Expression.Property(parameter, field);
			var lambda = Expression.Lambda(property, parameter);

			// REFLECTION: source.OrderBy(x => x.Property) or source.OrderByDescenging(x => x.Property)
			var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == orderByMethodName && x.GetParameters().Length == 2);
			var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TModel), property.Type);
			var result = orderByGeneric.Invoke(null, new object[] { queryable, lambda });

			return result as IOrderedQueryable<TModel>;
		}
	}
}
