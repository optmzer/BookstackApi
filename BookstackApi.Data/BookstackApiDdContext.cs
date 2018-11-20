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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("BookstackApi.Data.Models.BookComment", b =>
            {
                b.HasOne("BookstackApi.Data.Models.Book")
                    .WithMany("ListComments")
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("BookstackApi.Data.Models.BookTag", b =>
            {
                b.HasOne("BookstackApi.Data.Models.Book")
                    .WithMany("BookTags")
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
