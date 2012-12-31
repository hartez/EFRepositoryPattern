using System.Collections.Generic;
using EFRepository.Queryable;

namespace EFRepository
{
    public interface IRetrieveMatching<T, in TCriteria>
        where TCriteria : class
        where T : class
    {
        IEnumerable<T> Retrieve(TCriteria criteria = null, params Order<T>[] orderBy);
    }
}