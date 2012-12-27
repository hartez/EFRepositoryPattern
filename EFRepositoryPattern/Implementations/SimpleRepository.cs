using System;
using System.Data.Entity;

namespace EFRepository.Implementations
{
    /// <summary>
    /// Generic implementation for a repo that can store and retrieve entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TIdentity"></typeparam>
    public class SimpleRepository<T, TIdentity> : ISimpleRepository<T, TIdentity>
        where T : class
    {
        private readonly RetrieveRepository<T, TIdentity> _retrieveRepository;
        private readonly StoreRepository<T, TIdentity> _storeRepository;

        public SimpleRepository(DbContext context, DbSet<T> dbSet, Func<T, TIdentity> id)
        {
            _retrieveRepository = new RetrieveRepository<T, TIdentity>(dbSet);
            _storeRepository = new StoreRepository<T, TIdentity>(context, dbSet, id);
        }

        public TIdentity Save(T entity)
        {
            return _storeRepository.Save(entity);
        }

        public T Retrieve(TIdentity id)
        {
            return _retrieveRepository.Retrieve(id);
        }
    }
}