using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using EFRepository.Interfaces;
using EFRepository.Queryable;

namespace EFRepositoryPattern.Tests.Repositories
{
    public class MatchingRepository<T, TCriteria> : IRetrieveMatching<T, TCriteria>
        where T : class
        where TCriteria : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly Func<TCriteria, Expression<Func<T, bool>>> _expressionBuilder;

        public MatchingRepository(DbSet<T> dbSet, Func<TCriteria, Expression<Func<T, bool>>> expressionBuilder)
        {
            _dbSet = dbSet;
            _expressionBuilder = expressionBuilder;
        }

        public IEnumerable<T> Retrieve(TCriteria criteria = null, params Order<T>[] orderBy)
        {
            return QueryHelpers<T>.BuildQuery(_dbSet, _expressionBuilder(criteria), orderBy);
        }
    }
}