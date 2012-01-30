using System.Linq;

namespace EFRepository
{
	public static class QueryableExtensions
	{
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, Order<T> order)
		{
			return ApplyOrder(query, order, "OrderBy");
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, Order<T> order)
		{
			return ApplyOrder(query, order, "ThenBy");
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, Order<T> order)
		{
			return ApplyOrder(query, order, "OrderByDescending");
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IQueryable<T> query, Order<T> order)
		{
			return ApplyOrder(query, order, "ThenByDescending");
		}

		private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> query, Order<T> order, string methodName)
		{
			var result = typeof(Queryable).GetMethods().Single(
				method => method.Name == methodName
				          && method.IsGenericMethodDefinition
				          && method.GetGenericArguments().Length == 2
				          && method.GetParameters().Length == 2)
				.MakeGenericMethod(typeof(T), order.PropertyInfo.PropertyType)
				.Invoke(null, new object[] { query, order.OrderByExpression });

			return (IOrderedQueryable<T>)result;
		}
	}
}