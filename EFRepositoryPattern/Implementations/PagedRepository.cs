using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using EFRepository.Queryable;

namespace EFRepository.Implementations
{
    public class PagedRepository<T, TCriteria> : IRetrievePaged<T, TCriteria>
        where T : class
        where TCriteria : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly Func<TCriteria, Expression<Func<T, bool>>> _expressionBuilder;

        public PagedRepository(DbSet<T> dbSet, Func<TCriteria, Expression<Func<T, bool>>> expressionBuilder)
        {
            _dbSet = dbSet;
            _expressionBuilder = expressionBuilder;
        }

        public IEnumerable<T> Retrieve(int pageSize, int pageIndex, out int virtualCount, TCriteria criteria = null, params Order<T>[] orderBy)
        {
            return QueryHelpers<T>.Page(
                QueryHelpers<T>.BuildQuery(_dbSet, _expressionBuilder(criteria), orderBy), pageSize, pageIndex, out virtualCount);
        }
    }
}