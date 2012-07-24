using EFRepository;
using Examples.Models;

namespace Examples.Repositories
{
	internal interface IPostRepository : IRepository<Post, int>, ISearchRepository<Post, PostFilter>
	{
	}
}