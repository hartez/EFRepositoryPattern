using System;
using System.Linq;
using EFRepositoryPattern.Tests.Models;
using EFRepositoryPattern.Tests.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace EFRepositoryPattern.Tests
{
    [TestFixture]
    public class RetrieveAll
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
        public void ShouldHaveTenPosts()
        {
            IPostRepository repo = new PostRepository(new BlogContext());

            var posts = repo.RetrieveAll();

            posts.Count().Should().Be(10);
        }
    }
}