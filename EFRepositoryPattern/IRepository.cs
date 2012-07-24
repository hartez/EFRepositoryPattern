using System.Collections.Generic;

namespace EFRepository
{
	public interface IRepository<T, TIdentity>
		where T : class
	{
		TIdentity Save(T entity);

		T Retrieve(TIdentity id);
	}
}