using System.Collections.Generic;
using EFRepository;
using Examples.Models;

namespace Examples.Repositories
{
	class PostRepository : IPostRepository
	{
		private BlogContext _context;

		public PostRepository(BlogContext context)
		{
			_context = context;
		}

		public int Save(Post entity)
		{
			if(entity.ID == 0)
			{
				_context.Posts.Add(entity);
			}

			_context.SaveChanges();

			return entity.ID;
		}

		public Post Retrieve(int id)
		{
			return _context.Posts.Find(id);
		}


		public IEnumerable<Post> Search(int pageSize, int pageIndex, out int count, PostFilter filterBy = null, params Order<Post>[] orderBy)
		{
			return QueryHelpers<Post>.Page(
				QueryHelpers<Post>.BuildQuery(_context.Posts, filterBy.ToFilterExpression(), orderBy), pageSize, pageIndex, out count);
		}

		public IEnumerable<Post> Search(PostFilter filterBy = null, params Order<Post>[] orderBy)
		{
			return QueryHelpers<Post>.BuildQuery(_context.Posts, filterBy.ToFilterExpression(), orderBy);
		}
	}
}