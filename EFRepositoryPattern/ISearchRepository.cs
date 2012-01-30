using System.Collections.Generic;

namespace EFRepository
{
	public interface ISearchRepository<T, in TFilter>
		where TFilter : class
		where T : class
	{
		IEnumerable<T> Search(int pageSize, int pageIndex, out int count, TFilter filterBy = null,
		                      params Order<T>[] orderBy);

		IEnumerable<T> Search(TFilter filterBy = null,
		                      params Order<T>[] orderBy);
	}
}