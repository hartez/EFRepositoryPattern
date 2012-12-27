using System.Collections.Generic;
using EFRepository.Queryable;

namespace EFRepository.Interfaces
{
    public interface IRetrievePaged<T, in TCriteria>
        where TCriteria : class
        where T : class
    {
        IEnumerable<T> Retrieve(int pageSize, int pageIndex, out int virtualCount, TCriteria criteria = null,
                              params Order<T>[] orderBy);
    }
}