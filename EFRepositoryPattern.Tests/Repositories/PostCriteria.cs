using System;

namespace EFRepositoryPattern.Tests.Repositories
{
	public class PostCriteria
	{
		public DateTime? AfterDate { get; set; }
		public DateTime? BeforeDate { get; set; }
		public string Title { get; set; }

		public PostCriteria(DateTime? afterDate, DateTime? beforeDate, string title)
		{
			AfterDate = afterDate;
			BeforeDate = beforeDate;
			Title = title;
		}
	}
}