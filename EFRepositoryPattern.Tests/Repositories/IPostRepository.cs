using EFRepository;
using EFRepositoryPattern.Tests.Models;

namespace EFRepositoryPattern.Tests.Repositories
{
	internal interface IPostRepository : IStore<Post, int>, IRepository<Post, int>, IFilterRepository<Post, PostFilter>
	{

	}
}