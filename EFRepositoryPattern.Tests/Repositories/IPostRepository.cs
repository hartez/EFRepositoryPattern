using EFRepository;
using EFRepositoryPattern.Tests.Models;

namespace EFRepositoryPattern.Tests.Repositories
{
	internal interface IPostRepository : 
        IStore<Post, int>, 
        IRetrieve<Post, int>, 
        IRetrieveAll<Post>,
	IFilterRepository<Post, PostFilter>
	{

	}
}