using BookstackApi.Data;
using BookstackApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstackApi.Services
{

    public class BookCommentService : IBookComment
    {
        private readonly BookstackApiDdContext _context;

        public BookCommentService(BookstackApiDdContext context)
        {
            _context = context;
        }

        public IEnumerable<BookComment> GetAllComments()
        {
            return _context.BookComments;
        }

        public async Task AddBookCommentAsync(int bookId, BookComment comment)
        {
            // Get book
            var book = _context.Book
                    .Where(b => b.Id == bookId)
                    .Include(b => b.ListComments)
                    .FirstOrDefault();

            // Lock for modification
            _context.Update(book);

            // Append comments
            List<BookComment> result = book.ListComments.ToList<BookComment>();
            result.Add(comment);

            book.ListComments = result;
            //_context.Entry(book).State = EntityState.Modified;
            //_context.Entry(comments).State = EntityState.Modified;
            //_context.Add(comment);
            await _context.SaveChangesAsync();
        }

        public bool CommentExist(int id)
        {
            return _context.BookComments.Any(e => e.Id == id);
        }

        public async Task DeleteBookCommentAsync(BookComment comment)
        {
            _context.BookComments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task EditBookCommentAsync(BookComment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<BookComment> GetByBookId(int bookId)
        {
            if(_context.Book.Any(e => e.Id == bookId))
            {
                var book = _context.Book
                    .Where(b => b.Id == bookId)
                    .Include(b => b.ListComments)
                    .FirstOrDefault();

                var comments = book
                    .ListComments;

                return comments;
            }
            // Returns an empty list instead of null
            return (new List<BookComment>() { new BookComment
                    {
                        Author = "",
                        Content = "",
                        Created = DateTime.Now
                    } });
        }

        public async Task<BookComment> GetCommentById(int id)
        {
            return await _context.BookComments.FindAsync(id);
        }

    }
}
