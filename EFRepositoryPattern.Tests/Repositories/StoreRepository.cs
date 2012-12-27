using System;
using System.Data.Entity;
using EFRepository;

namespace EFRepositoryPattern.Tests.Repositories
{
    /// <summary>
    /// A simple generic Save implementation that will work for most of our EF entities
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <typeparam name="TIdentity">The type used for the identity</typeparam>
    public class StoreRepository<T, TIdentity> : IStore<T, TIdentity>
        where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly Func<T, TIdentity> _id;

        public StoreRepository(DbContext context, DbSet<T> dbSet, Func<T, TIdentity> id)
        {
            _context = context;
            _dbSet = dbSet;
            _id = id;
        }

        public TIdentity Save(T entity)
        {
            // For most of our identities in the database, we use basic types like int, long, or GUID
            // and let the database assign the values.
            // So to check if this is a brand new object that we're adding to the context, we can
            // simply check whether the identity value is set to its default. If so, then it's new
            // and needs to be added to the context. If not, we assume that it's already in the context
            // and we just need to save the changes. 
            if(_id(entity).Equals(default(TIdentity)))
            {
                _dbSet.Add(entity);
            }

            _context.SaveChanges();

            return _id(entity);
        }
    }
}