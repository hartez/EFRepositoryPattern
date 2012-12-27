using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFRepository.Implementations;
using EFRepository.Queryable;
using EFRepositoryPattern.Tests.Models;
using LinqKit;

namespace EFRepositoryPattern.Tests.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _context;

        private readonly RetrieveAllRepository<Post> _retrieveAllRepository;
        private readonly SimpleRepository<Post, int> _simpleRepository;
        private readonly MatchingRepository<Post, PostCriteria> _matchingRepository;
        private readonly PagedRepository<Post, PostCriteria> _pagedRepository; 

        public PostRepository(BlogContext context)
        {
            _context = context;
            _simpleRepository = new SimpleRepository<Post, int>(_context, _context.Posts, post => post.ID);
            _retrieveAllRepository = new RetrieveAllRepository<Post>(_context.Posts);
            _matchingRepository = new MatchingRepository<Post, PostCriteria>(_context.Posts, ExpressionFrom);
            _pagedRepository = new PagedRepository<Post, PostCriteria>(_context.Posts, ExpressionFrom);
        }

        #region IPostRepository Members

        public int Save(Post entity)
        {
            return _simpleRepository.Save(entity);
        }

        public Post Retrieve(int id)
        {
            return _simpleRepository.Retrieve(id);
        }

        public IEnumerable<Post> RetrieveAll(params Order<Post>[] orderBy)
        {
            return _retrieveAllRepository.RetrieveAll();
        }

        public IEnumerable<Post> Retrieve(PostCriteria criteria = null, params Order<Post>[] orderBy)
        {
            return _matchingRepository.Retrieve(criteria, orderBy);
        }

        #endregion

        public IEnumerable<Post> Retrieve(int pageSize, int pageIndex, out int virtualCount, PostCriteria criteria = null,
            params Order<Post>[] orderBy)
        {
            return _pagedRepository.Retrieve(pageSize, pageIndex, out virtualCount, criteria, orderBy);
        }

        private Expression<Func<Post, bool>> ExpressionFrom(PostCriteria filter)
        {
            // Start with our base expression - we want all posts
            Expression<Func<Post, bool>> filterExpression = PredicateBuilder.True<Post>();

            if(filter == null)
            {
                return filterExpression;
            }

            // If a title has been specified in the filter, add an expression to include
            // any Post where the Title contains the value specified in the filter
            if(!String.IsNullOrEmpty(filter.Title))
            {
                Expression<Func<Post, bool>> expr = post => post.Title.Contains(post.Title);
                filterExpression = filterExpression.And(expr);
            }

            // If a 'Before' date is specified, add an expression to include posts which have
            // a PublishedDate less than the date specified
            if(filter.BeforeDate.HasValue)
            {
                DateTime dt = filter.BeforeDate.Value;
                Expression<Func<Post, bool>> expr =
                    post => post.PublishDate.CompareTo(dt) < 0;

                filterExpression = filterExpression.And(expr);
            }

            // If an 'After' date is specified, add an expression to include posts which have
            // a PublishedDate greater than the date specified
            if(filter.AfterDate.HasValue)
            {
                DateTime dt = filter.AfterDate.Value;
                Expression<Func<Post, bool>> expr =
                    post => post.PublishDate.CompareTo(dt) >= 0;

                filterExpression = filterExpression.And(expr);
            }

            return filterExpression;
        }
    }
}