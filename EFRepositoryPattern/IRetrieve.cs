using System.Collections.Generic;

namespace EFRepository
{
    public interface IRetrieve<T, TIdentity> 
	{
		T Retrieve(TIdentity id);
	}

    public interface IRetrieveAll<T>
    {
        IEnumerable<T> RetrieveAll();
    }

    public interface IRetrieveOrdered<T, TIdentity> : IRetrieve<T, TIdentity>
    {
        IEnumerable<T> RetrieveAll(params Order<T>[] orderby);
    }
}