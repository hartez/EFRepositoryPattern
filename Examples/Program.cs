using System;
using EFRepository;
using EFRepositoryPattern.Tests.Models;
using EFRepositoryPattern.Tests.Repositories;

namespace Examples
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var context = new BlogContext();

			context.Database.ExecuteSqlCommand("delete from posts");

			// Create some posts

			for (int i = 0; i < 10; i++)
			{
				var post = new Post { Title = "Post Title " + i, Text = "Text of Post " + i, 
					PublishDate = new DateTime(2012, 1, 1).AddDays(i) };

				context.Posts.Add(post);
				context.SaveChanges();	
			}

			Console.WriteLine("Posts created\n");

			Console.WriteLine("Listing posts created between 2012-01-03 and 2012-01-07 in reverse date order");
			IPostRepository repo = new PostRepository(context);

			var criteria = new PostCriteria(new DateTime(2012, 1, 3), new DateTime(2012, 1, 7), String.Empty);

			var posts = repo.Retrieve(criteria, Order<Post>.ByDescending(post => post.PublishDate));

			foreach (var post in posts)
			{
				Console.WriteLine("{0} : {1}({2}) - {3}", post.ID, post.Title, post.PublishDate, post.Text);
			}

			Console.WriteLine("\nPress enter to exit");
			Console.ReadLine();
		}
	}
}