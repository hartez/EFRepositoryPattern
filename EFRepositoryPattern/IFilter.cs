using System;
using System.Linq.Expressions;

namespace EFRepository
{
	public interface IFilter<T>
	{
		Expression<Func<T, bool>> ToFilterExpression();
	}
}