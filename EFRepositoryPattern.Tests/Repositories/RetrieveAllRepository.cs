using System.Collections.Generic;
using System.Data.Entity;
using EFRepository;

namespace EFRepositoryPattern.Tests.Repositories
{
    public class RetrieveAllRepository<T> : IRetrieveAll<T>
        where T : class
    {
        private readonly DbSet<T> _dbSet;

        public RetrieveAllRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public IEnumerable<T> RetrieveAll(params Order<T>[] @orderby)
        {
            return QueryHelpers<T>.BuildQuery(_dbSet, orderby);
        }
    }
}