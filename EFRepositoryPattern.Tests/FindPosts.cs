using System;
using System.Diagnostics;
using System.Linq;
using EFRepository;
using EFRepository.Queryable;
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
            var context = new BlogContext();
            
            context.Database.ExecuteSqlCommand("delete from posts");

            for(int i = 0; i < 10; i++)
            {
                var post = new Post
                               {
                                   Title = "Post Title " + i,
                                   Text = "Text of Post " + i,
                                   PublishDate = new DateTime(2012, 1, 1).AddDays(i)
                               };

                context.Posts.Add(post);
                context.SaveChanges();
            }
        }

        #endregion

        [Test]
        public void BetweenDates()
        {
            IPostRepository repo = new PostRepository(new BlogContext());

            var criteria = new PostCriteria(new DateTime(2012, 1, 3), new DateTime(2012, 1, 7), String.Empty);

            var posts = repo.Retrieve(criteria, Order<Post>.ByDescending(post => post.PublishDate));

            posts.Count().Should().Be(4);
        }

        [Test]
        public void ByTitle()
        {
            IPostRepository repo = new PostRepository(new BlogContext());

            var criteria = new PostCriteria(null, null, "9");

            var posts = repo.Retrieve(criteria, Order<Post>.ByDescending(post => post.PublishDate));

            posts.Count().Should().Be(1);
            posts.First().Title.Should().Be("Post Title 9");
        }

        [Test]
        public void BetweenDatesUsingSQL()
        {
            IPostRepository repo = new AlternatePostRepository(new BlogContext());

            var criteria = new PostCriteria(new DateTime(2012, 1, 3), new DateTime(2012, 1, 7), String.Empty);

            var posts = repo.Retrieve(criteria, Order<Post>.ByDescending(post => post.PublishDate));

            posts.Count().Should().Be(4);
        }
    }
}