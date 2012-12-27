namespace EFRepository
{
    public interface IStore<T, TIdentity>
        where T : class
    {
        TIdentity Save(T entity);
    }
}