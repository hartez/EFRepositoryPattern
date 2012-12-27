using System.Collections.Generic;

namespace EFRepository
{
    public interface IStore<T, TIdentity>
        where T : class
    {
        TIdentity Save(T entity);
    }

    public interface IRepository<T, TIdentity> 
	{
		T Retrieve(TIdentity id);

	    IEnumerable<T> RetrieveAll();
	}
}