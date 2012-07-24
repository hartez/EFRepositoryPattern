using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EFRepository
{
	public interface IFilterRepository<T, in TFilter>
		where TFilter : class
		where T : class
	{
		IEnumerable<T> Filter(int pageSize, int pageIndex, out int count, TFilter filterBy = null,
		                      params Order<T>[] orderBy);

		IEnumerable<T> Filter(TFilter filterBy = null,
		                      params Order<T>[] orderBy);

		Expression<Func<T, bool>> FilterExpressionFrom(TFilter filter);
	}
}