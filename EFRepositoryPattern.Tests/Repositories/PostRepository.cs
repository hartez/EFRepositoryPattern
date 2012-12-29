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
        protected readonly BlogContext _context;

        private readonly RetrieveAllRepository<Post> _retrieveAllRepository;
        private readonly SimpleRepository<Post, int> _simpleRepository;
        private readonly MatchingRepository<Post, PostCriteria> _matchingRepository;
        private readonly PagedRepository<Post, PostCriteria> _pagedRepository; 

        public PostRepository(BlogContext context)
        {
            _context = context;
            _simpleRepository = new SimpleRepository<Post, int>(_context, _context.Posts, post => post.ID);
            _retrieveAllRepository = new RetrieveAllRepository<Post>(_context.Posts);
            _matchingRepository = new MatchingRepository<Post, PostCriteria>(_context.Posts, ExpressionBuilder);
            _pagedRepository = new PagedRepository<Post, PostCriteria>(_context.Posts, ExpressionBuilder);
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

        public virtual IEnumerable<Post> Retrieve(PostCriteria criteria = null, params Order<Post>[] orderBy)
        {
            return _matchingRepository.Retrieve(criteria, orderBy);
        }

        #endregion

        public IEnumerable<Post> Retrieve(int pageSize, int pageIndex, out int virtualCount, PostCriteria criteria = null,
            params Order<Post>[] orderBy)
        {
            return _pagedRepository.Retrieve(pageSize, pageIndex, out virtualCount, criteria, orderBy);
        }

        private static Expression<Func<Post, bool>> ExpressionBuilder(PostCriteria criteria)
        {
            // Start with our base expression - we want all posts
            //Expression<Func<Post, bool>> criteriaExpression = PredicateBuilder.True<Post>();
            var criteriaExpression = PredicateBuilder.True<Post>();

            if(criteria == null)
            {
                return criteriaExpression;
            }

            // If a title has been specified in the filter, add an expression to include
            // any Post where the Title contains the value specified in the filter
            if(!String.IsNullOrEmpty(criteria.Title))
            {
                criteriaExpression = criteriaExpression.And(post => post.Title.Contains(criteria.Title));
            }

            // If a 'Before' date is specified, add an expression to include posts which have
            // a PublishedDate less than the date specified
            if(criteria.BeforeDate.HasValue)
            {
                criteriaExpression = criteriaExpression.And(post => post.PublishDate.CompareTo(criteria.BeforeDate.Value) < 0);
            }

            // If an 'After' date is specified, add an expression to include posts which have
            // a PublishedDate greater than the date specified
            if(criteria.AfterDate.HasValue)
            {
                criteriaExpression = criteriaExpression.And(post => post.PublishDate.CompareTo(criteria.AfterDate.Value) >= 0);
            }

            return criteriaExpression;
        }
    }
}