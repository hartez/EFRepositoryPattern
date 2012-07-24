using System;
using Examples.Models;
using Examples.Repositories;

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

			Console.WriteLine("Posts created");

			Console.WriteLine("Listing posts created between 2012-01-03 and 2012-01-07");
			IPostRepository repo = new PostRepository(context);

			var posts = repo.Search(new PostFilter(new DateTime(2012, 1, 3), new DateTime(2012, 1, 7), String.Empty));

			foreach (var post in posts)
			{
				Console.WriteLine("{0} : {1}({2}) - {3}", post.ID, post.Title, post.PublishDate, post.Text);

			}

			Console.ReadLine();
		}
	}
}