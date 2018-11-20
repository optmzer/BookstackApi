using BookstackApi.Data;
using BookstackApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstackApi.Services
{
    public class BookTagService : IBookTag
    {
        private readonly BookstackApiDdContext _context;
        public BookTagService(BookstackApiDdContext context)
        {
            _context = context;
        }

        public async Task<BookTag> GetBookTagByIdAsync(int bookTagId)
        {
            return await _context.BookTags.FindAsync(bookTagId);
        }

        public async Task AddBookTagAsync(int bookId, BookTag bookTag)
        {
            // Get book
            var book = _context.Book
                    .Where(b => b.Id == bookId)
                    .Include(b => b.BookTags)
                    .FirstOrDefault();

            // Lock for modification
            _context.Update(book);

            // Append comments
            List<BookTag> result = book.BookTags.ToList<BookTag>();
            result.Add(bookTag);

            book.BookTags = result;
            await _context.SaveChangesAsync();
        }

        public bool BookTagExist(int bookTagId)
        {
            return _context.BookTags.Any(e => e.Id == bookTagId);
        }

        public async Task DeleteBookCommentAsync(BookTag bookTag)
        {
            _context.BookTags.Remove(bookTag);
            await _context.SaveChangesAsync();
        }

        public async Task EditBookCommentAsync(BookTag bookTag)
        {
            _context.Entry(bookTag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<BookTag> GetAll()
        {
            return _context.BookTags;
        }

        public IEnumerable<BookTag> GetByBookId(int bookId)
        {
            if (_context.Book.Any(e => e.Id == bookId))
            {
                var book = _context.Book
                    .Where(b => b.Id == bookId)
                    .Include(b => b.BookTags)
                    .FirstOrDefault();

                var tags = book
                    .BookTags;

                return tags;
            }
            // Returns an empty list instead of null
            return (new List<BookTag>() { new BookTag
                    {
                        Description = "add more tags...",
                    } });
        }
    }
}
