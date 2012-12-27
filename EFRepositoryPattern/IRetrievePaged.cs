using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EFRepository
{
    public interface IRetrievePaged<T, in TCriteria>
        where TCriteria : class
        where T : class
    {
        IEnumerable<T> Retrieve(int pageSize, int pageIndex, out int virtualCount, TCriteria criteria = null,
                              params Order<T>[] orderBy);
    }
}