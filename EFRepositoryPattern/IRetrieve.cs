namespace EFRepository
{
    public interface IRetrieve<T, TIdentity> 
	{
		T Retrieve(TIdentity id);
	}
}