using BookstackApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstackApi.Data
{
    public class BookstackApiDdContext : DbContext
    {
        public BookstackApiDdContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
    }
}
