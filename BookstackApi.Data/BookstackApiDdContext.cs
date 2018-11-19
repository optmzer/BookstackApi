using BookstackApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookstackApi.Data
{
    public class BookstackApiDdContext : DbContext
    {
        public BookstackApiDdContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BookModel> Book { get; set; }
        public DbSet<CommentModel> BookComments { get; set; }
    }
}
