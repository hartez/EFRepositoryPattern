using EFRepository;
using EFRepositoryPattern.Tests.Models;

namespace EFRepositoryPattern.Tests.Repositories
{
	internal interface IPostRepository : IRepository<Post, int>, IFilterRepository<Post, PostFilter>
	{
	}
}