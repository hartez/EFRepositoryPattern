using System;
using System.Collections.Generic;
using EFRepository.Queryable;

namespace EFRepository
{
    public interface IRetrieveBlargh<T, in TIdentity, in TCriteria>
        where TCriteria : class
        where T : class
    {
        IEnumerable<T> RetrieveNext(int pageSize, TIdentity anchor, out int virtualCount, TCriteria criteria = null,
            params Order<T>[] orderBy);
    }
}