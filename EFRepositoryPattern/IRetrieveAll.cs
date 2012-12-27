using System.Collections.Generic;
using EFRepository.Queryable;

namespace EFRepository
{
    public interface IRetrieveAll<T>
    {
        IEnumerable<T> RetrieveAll(params Order<T>[] orderby);
    }
}