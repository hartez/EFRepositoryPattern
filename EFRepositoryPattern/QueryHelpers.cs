using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace EFRepository
{
	public static class QueryHelpers<T>
	{
		public static IEnumerable<T> Page(IQueryable<T> query, int pageSize, int pageIndex, out int count)
		{
			count = query.Count();
			return query.Skip(pageSize * pageIndex).Take(pageSize);
		}

		public static IEnumerable<T> Page(IQueryable<T> query, int pageSize, int pageIndex)
		{
			return query.Skip(pageSize * pageIndex).Take(pageSize);
		}

	    public static IQueryable<T> BuildQuery(IQueryable<T> query,
	        params Order<T>[] orderBy)
	    {
	        return BuildQuery(query, null, orderBy);
	    }

	    public static IQueryable<T> BuildQuery(IQueryable<T> query,
		                                       Expression<Func<T, bool>> filterBy,
		                                       params Order<T>[] orderBy)
		{
			if (filterBy != null)
			{
				query = query.AsExpandable().Where(filterBy);
			}

			if (orderBy != null && orderBy.Length > 0)
			{
				if (orderBy[0].Descending)
				{
					query = query.OrderByDescending(orderBy[0]);
				}
				else
				{
					query = query.OrderBy(orderBy[0]);
				}

				for (int n = 1; n < orderBy.Length; n++)
				{
					if (orderBy[n].Descending)
					{
						query = (query as IOrderedQueryable<T>).ThenByDescending(orderBy[n]);
					}
					else
					{
						query = (query as IOrderedQueryable<T>).ThenBy(orderBy[n]);
					}
				}
			}

			return query;
		}
	}
}