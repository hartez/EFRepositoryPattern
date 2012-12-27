using EFRepository;
using Examples.Models;

namespace Examples.Repositories
{
	internal interface IPostRepository : IRetrieve<Post, int>, IFilterRepository<Post, PostFilter>
	{
	}
}