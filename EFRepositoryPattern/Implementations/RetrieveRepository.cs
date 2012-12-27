using System.Data.Entity;

namespace EFRepository.Implementations
{
    public class RetrieveRepository<T, TIdentity> : IRetrieve<T, TIdentity>
        where T : class
    {
        private readonly DbSet<T> _dbSet;

        public RetrieveRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public T Retrieve(TIdentity id)
        {
            return _dbSet.Find(id);
        }
    }
}