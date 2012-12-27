using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EFRepository;
using EFRepositoryPattern.Tests.Models;
using EFRepositoryPattern.Tests.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace EFRepositoryPattern.Tests
{
    [TestFixture]
    public class FindPosts
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _context = new BlogContext();

            _context.Database.ExecuteSqlCommand("delete from posts");

            for(int i = 0; i < 10; i++)
            {
                var post = new Post
                               {
                                   Title = "Post Title " + i,
                                   Text = "Text of Post " + i,
                                   PublishDate = new DateTime(2012, 1, 1).AddDays(i)
                               };

                _context.Posts.Add(post);
                _context.SaveChanges();
            }

            Debug.WriteLine("Posts created\n");
        }

        #endregion

        private BlogContext _context;

        [Test]
        public void BetweenDates()
        {
            Debug.WriteLine("Listing posts created between 2012-01-03 and 2012-01-07 in reverse date order");
            IPostRepository repo = new PostRepository(_context);

            var filter = new PostFilter(new DateTime(2012, 1, 3), new DateTime(2012, 1, 7), String.Empty);

            IEnumerable<Post> posts = repo.Filter(filter, Order<Post>.ByDescending(post => post.PublishDate));

            foreach(Post post in posts)
            {
                Debug.WriteLine("{0} : {1}({2}) - {3}", post.ID, post.Title, post.PublishDate, post.Text);
            }

            posts.Count().Should().Be(4);
        }
    }
}