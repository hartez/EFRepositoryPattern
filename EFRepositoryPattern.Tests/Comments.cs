using System;
using System.Collections.ObjectModel;
using System.Linq;
using EFRepository.Queryable;
using EFRepositoryPattern.Tests.Models;
using EFRepositoryPattern.Tests.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace EFRepositoryPattern.Tests
{
    [TestFixture]
    public class Comments
    {
        private int CreatePost(string title, string text, DateTime published, params Comment[] comments)
        {
            IPostRepository repo = new PostRepository(new BlogContext());

            var post = new Post
                           {
                               Title = title,
                               Text = text,
                               PublishDate = published
                           };

            post.Comments = new Collection<Comment>();

            foreach(var comment in comments)
            {
                post.Comments.Add(new Comment() { Text = comment.Text, Author = comment.Author});    
            }

            return repo.Save(post);
        }

        [Test]
        public void VirtualCountEqualsTotalItems()
        {
            var comments = new Comment[12];

            for (int i = 0; i < 12; i++)
            {
                comments[i] = new Comment() { Text = "This is comment " + i.ToString("D2"), Author = "Chuck Finley" };
            }

            var postId = CreatePost("Comments Test", "Comments Test", DateTime.Now, comments);

            var repo = new CommentRepository(new BlogContext());

            int virtualItemCount;
            int pageSize = 5;

            var criteria = new CommentCriteria() {PostId = postId};
            var order =  Order<Comment>.By(c => c.Text);

            var page0 = repo.Retrieve(pageSize, 0, out virtualItemCount, criteria, order).ToList();
            virtualItemCount.Should().Be(12);

            var page1 = repo.Retrieve(pageSize, 1, out virtualItemCount, criteria, order).ToList();
            virtualItemCount.Should().Be(12);

            var page2 = repo.Retrieve(pageSize, 2, out virtualItemCount, criteria, order).ToList();
            virtualItemCount.Should().Be(12);
        }

        [Test]
        public void PagesGetCorrectItems()
        {
            var comments = new Comment[12];

            for(int i = 0; i < 12; i++)
            {
                comments[i] = new Comment() {Text = "This is comment " + i.ToString("D2"), Author = "Chuck Finley"};
            }

            var postId = CreatePost("Comments Test", "Comments Test", DateTime.Now, comments);

            var repo = new CommentRepository(new BlogContext());

            int virtualItemCount;
            int pageSize = 5;
            var criteria = new CommentCriteria() { PostId = postId };
            var order = Order<Comment>.By(c => c.Text);

            var page0 = repo.Retrieve(pageSize, 0, out virtualItemCount, criteria, order).ToList();
            page0.Count().Should().Be(5);
            page0.First().Text.Should().EndWith("00");
            page0.Last().Text.Should().EndWith("04");

            var page1 = repo.Retrieve(pageSize, 1, out virtualItemCount, criteria, order).ToList();
            page1.Count().Should().Be(5);
            page1.First().Text.Should().EndWith("05");
            page1.Last().Text.Should().EndWith("09");

            var page2 = repo.Retrieve(pageSize, 2, out virtualItemCount, criteria, order).ToList();
            page2.Count().Should().Be(2);
            page2.First().Text.Should().EndWith("10");
            page2.Last().Text.Should().EndWith("11");
        }

        [Test]
        public void CommentsShouldSavedWithPost()
        {
            var title = "Comments Test";
            var text = "Comments Test";
            var publishDate = DateTime.Now;

            var comment1 = new Comment() {Text = "This is a comment", Author = "Chuck Finley"};
            var comment2 = new Comment() { Text = "This is a comment", Author = "Chuck Finley" };

            var postId = CreatePost(title, text, publishDate, comment1, comment2);

            var repo = new CommentRepository(new BlogContext());

            int virtualItemCount;
            var comments = repo.Retrieve(10, 0, out virtualItemCount, new CommentCriteria() {PostId = postId}).ToList();

            comment1.PostID = postId;
            comment2.PostID = postId;
            var comparisonComments = new Collection<Comment>() {comment1, comment2};

            comments.Count().Should().Be(2);
            comments.ShouldAllBeEquivalentTo(comparisonComments, 
                options => options.Excluding(o => o.ID));
        }

        [Test]
        public void CommentsCanBeFilteredByPost()
        {
            var comment1 = new Comment() { Text = "This is a comment for the first post", Author = "Chuck Finley" };
            var comment2 = new Comment() { Text = "This is a comment for the first post", Author = "Chuck Finley" };
            var comment3 = new Comment() { Text = "This is a comment for the second post", Author = "Santos L. Helper" };
            var comment4 = new Comment() { Text = "This is a comment for the second post", Author = "Santos L. Helper" };

            var post1Id = CreatePost("Comments Test 1", "Comments Test 1", DateTime.Now, comment1, comment2);
            var post2Id = CreatePost("Comments Test 2", "Comments Test 2", DateTime.Now, comment3, comment4);

            var repo = new CommentRepository(new BlogContext());

            int virtualItemCount;
            var comments = repo.Retrieve(10, 0, out virtualItemCount, new CommentCriteria() { PostId = post2Id }).ToList();

            comment3.PostID = post2Id;
            comment4.PostID = post2Id;
            var comparisonComments = new Collection<Comment>() { comment3, comment4 };

            comments.Count().Should().Be(2);
            comments.ShouldAllBeEquivalentTo(comparisonComments,
                options => options.Excluding(o => o.ID));
        }
    }
}