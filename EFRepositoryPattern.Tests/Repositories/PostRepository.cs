using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFRepository;
using EFRepositoryPattern.Tests.Models;
using LinqKit;

namespace EFRepositoryPattern.Tests.Repositories
{
    class PostRepository : IPostRepository
	{
		private readonly BlogContext _context;

        private readonly StoreRepository<Post, int> _storeRepository;

		public PostRepository(BlogContext context)
		{
			_context = context;
            _storeRepository = new StoreRepository<Post, int>(_context, _context.Posts, post => post.ID);
		}

        public int Save(Post entity)
        {
            return _storeRepository.Save(entity);
        }

        public Post Retrieve(int id)
		{
			return _context.Posts.Find(id);
		}

	    public IEnumerable<Post> RetrieveAll()
	    {
	        return _context.Posts;
	    }

	    public IEnumerable<Post> Filter(int pageSize, int pageIndex, out int count, PostFilter filterBy = null, params Order<Post>[] orderBy)
		{
			return QueryHelpers<Post>.Page(
				QueryHelpers<Post>.BuildQuery(_context.Posts, FilterExpressionFrom(filterBy), orderBy), pageSize, pageIndex, out count);
		}

		public IEnumerable<Post> Filter(PostFilter filterBy = null, params Order<Post>[] orderBy)
		{
			return QueryHelpers<Post>.BuildQuery(_context.Posts, FilterExpressionFrom(filterBy), orderBy);
		}

		public Expression<Func<Post, bool>> FilterExpressionFrom(PostFilter filter)
		{
			// Start with our base expression - we want all posts
			Expression<Func<Post, bool>> filterExpression = PredicateBuilder.True<Post>();

			if (filter == null)
			{
				return filterExpression;
			}

			// If a title has been specified in the filter, add an expression to include
			// any Post where the Title contains the value specified in the filter
			if (!String.IsNullOrEmpty(filter.Title))
			{
				Expression<Func<Post, bool>> expr = post => post.Title.Contains(post.Title);
				filterExpression = filterExpression.And(expr);
			}

			// If a 'Before' date is specified, add an expression to include posts which have
			// a PublishedDate less than the date specified
			if (filter.BeforeDate.HasValue)
			{
				var dt = filter.BeforeDate.Value;
				Expression<Func<Post, bool>> expr =
					post => post.PublishDate.CompareTo(dt) < 0;

				filterExpression = filterExpression.And(expr);
			}

			// If an 'After' date is specified, add an expression to include posts which have
			// a PublishedDate greater than the date specified
			if (filter.AfterDate.HasValue)
			{
				var dt = filter.AfterDate.Value;
				Expression<Func<Post, bool>> expr =
					post => post.PublishDate.CompareTo(dt) >= 0;

				filterExpression = filterExpression.And(expr);
			}

			return filterExpression;
		}
	}
}