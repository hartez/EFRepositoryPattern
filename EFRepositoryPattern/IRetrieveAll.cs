using System.Collections.Generic;

namespace EFRepository
{
    public interface IRetrieveAll<T>
    {
        IEnumerable<T> RetrieveAll(params Order<T>[] orderby);
    }
}