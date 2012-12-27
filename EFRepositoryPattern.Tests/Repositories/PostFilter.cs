using System;

namespace EFRepositoryPattern.Tests.Repositories
{
	internal class PostFilter
	{
		public DateTime? AfterDate { get; set; }
		public DateTime? BeforeDate { get; set; }
		public string Title { get; set; }

		public PostFilter(DateTime? afterDate, DateTime? beforeDate, string title)
		{
			AfterDate = afterDate;
			BeforeDate = beforeDate;
			Title = title;
		}
	}
}