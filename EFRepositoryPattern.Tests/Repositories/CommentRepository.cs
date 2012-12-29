using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFRepository;
using EFRepository.Implementations;
using EFRepository.Queryable;
using EFRepositoryPattern.Tests.Models;
using LinqKit;

namespace EFRepositoryPattern.Tests.Repositories
{
    public class CommentRepository : IStore<Comment, int>, IRetrievePaged<Comment, CommentCriteria>
    {
        private readonly StoreRepository<Comment, int> _storeRepository;
        private readonly PagedRepository<Comment, CommentCriteria> _pagedRepository;

        public CommentRepository(BlogContext context)
        {
            _storeRepository = new StoreRepository<Comment, int>(context, context.Comments, comment => comment.ID);
            _pagedRepository = new PagedRepository<Comment, CommentCriteria>(context.Comments, ExpressionBuilder);
        }

        private Expression<Func<Comment, bool>> ExpressionBuilder(CommentCriteria commentCriteria)
        {
            var criteriaExpression = ExpressionExtensions.GenerateBasePredicate<Comment>();

            if (commentCriteria == null)
            {
                return criteriaExpression;
            }

            if(commentCriteria.PostId.HasValue)
            {
                Expression<Func<Comment, bool>> expr = comment => comment.PostID == commentCriteria.PostId.Value;
                criteriaExpression = criteriaExpression.And(expr);
            }

            return criteriaExpression;
        }

        public int Save(Comment entity)
        {
            return _storeRepository.Save(entity);
        }

        public IEnumerable<Comment> Retrieve(int pageSize, int pageIndex, out int virtualCount, CommentCriteria criteria = null, params Order<Comment>[] orderBy)
        {
            return _pagedRepository.Retrieve(pageSize, pageIndex, out virtualCount, criteria, orderBy);
        }
    }
}