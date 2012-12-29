using System;
using System.Linq.Expressions;
using LinqKit;

namespace EFRepository.Implementations
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> GenerateBasePredicate<T>()
            where T : class
        {
            return PredicateBuilder.True<T>();
        }
    }
}