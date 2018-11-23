using BookstackApi.Data;
using BookstackApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstackApi.Services
{
    public class BookstackService : IBook
    {
        private readonly BookstackApiDdContext _context;

        public BookstackService (BookstackApiDdContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Book
                .Include(book => book.BookTags)
                .Include(book => book.ListComments);
        }

        public Book GetById(int bookId)
        {
            //var book = await _context.Book.FindAsync(bookId);
            //return book.Include(b => b.ListComments);

            return GetAll()
                .Where(book => book.Id == bookId)
                .FirstOrDefault();
        }

        public IEnumerable<Book> GetByTag(string tag)
        {
            return GetAll()
                .Where(book => book.BookTags.Any(tags => tags.Description == tag));
        }

        /**
         * Creates a reference to the image in SQL database
         */
        public async Task AddBookAsync(Book book)
        {
            //init tags if they are null
            if (book.BookTags == null)
            {
                book.BookTags = new List<BookTag>() {
                    new BookTag {
                        Description = "add tags..."
                    }
                };
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Book> GetLatest(int numberOfBooks)
        {
            return GetAll()
                .OrderByDescending(book => book.Created)
                .Take(numberOfBooks);
        }

        public async Task DeleteBookAsync(Book book)
        {
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task EditBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
        }

        public bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        public List<BookTag> ParseTags(string tags)
        {
            return tags.Split(",").Select(tag => new BookTag
            {
                Description = tag
            }).ToList();
        }   

        public IEnumerable<Book> GetByTitle(string title)
        {
            return GetAll()
                .Where(book => book.Title.ToLower().Contains(title.ToLower()));
        }
    }
}
