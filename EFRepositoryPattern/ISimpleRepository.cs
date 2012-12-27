namespace EFRepository
{
    public interface ISimpleRepository<T, TIdentity> : IStore<T, TIdentity>, IRetrieve<T, TIdentity>
        where T : class 
    {

    }
}