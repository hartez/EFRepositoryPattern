using EFRepository;
using EFRepository.Implementations;
using EFRepositoryPattern.Tests.Models;

namespace EFRepositoryPattern.Tests.Repositories
{
    public class CommentRepository : IStore<Comment, int>
    {
        private readonly StoreRepository<Comment, int> _storeRepository;

        public CommentRepository(BlogContext context)
        {
            _storeRepository = new StoreRepository<Comment, int>(context, context.Comments, comment => comment.ID);
        }

        public int Save(Comment entity)
        {
            return _storeRepository.Save(entity);
        }
    }
}