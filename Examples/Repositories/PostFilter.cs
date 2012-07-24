using System;
using System.Linq.Expressions;
using EFRepository;
using Examples.Models;
using LinqKit;

namespace Examples.Repositories
{
	internal class PostFilter : IFilter<Post>
	{
		private DateTime? _beforeDate;
		private DateTime? _afterDate;
		private string _title;

		public PostFilter(DateTime? afterDate, DateTime? beforeDate, string title)
		{
			_beforeDate = beforeDate;
			_afterDate = afterDate;
			_title = title;
		}

		public Expression<Func<Post, bool>> ToFilterExpression()
		{
			Expression<Func<Post, bool>> filterExpression = PredicateBuilder.True<Post>();

			if(!String.IsNullOrEmpty(_title))
			{
				Expression<Func<Post, bool>> expr = post => post.Title.Contains(post.Title);
				filterExpression = filterExpression.And(expr);
			}

			if (_beforeDate.HasValue)
			{
				var dt = _beforeDate.Value;
				Expression<Func<Post, bool>> expr =
					post => post.PublishDate.CompareTo(dt) < 0;

				filterExpression = filterExpression.And(expr);
			}

			if (_afterDate.HasValue)
			{
				var dt = _afterDate.Value;
				Expression<Func<Post, bool>> expr =
					post => post.PublishDate.CompareTo(dt) >= 0;

				filterExpression = filterExpression.And(expr);
			}

			return filterExpression;
		}
	}
}