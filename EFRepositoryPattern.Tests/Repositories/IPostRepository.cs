using EFRepository;
using EFRepositoryPattern.Tests.Models;

namespace EFRepositoryPattern.Tests.Repositories
{
	public interface IPostRepository : 
        IStore<Post, int>, 
        IRetrieve<Post, int>, 
        IRetrieveAll<Post>,
	    IRetrieveMatching<Post, PostCriteria>
	{
	}
}