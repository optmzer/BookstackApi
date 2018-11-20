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
            return GetAll().Where(book => book.Id == bookId)
                .First();
        }

        public IEnumerable<Book> GetByTag(string tag)
        {
            return GetAll()
                .Where(book => book.BookTags.Any(tags => tags.Description == tag));
        }


        public void DeleteBook(int bookId)
        {

        }

        /**
         * Creates a reference to the image in SQL database
         */
        public async Task SetBook(Book book)
        {
            //init tags if they are null
            if (book.BookTags == null)
            {
                book.BookTags = new List<BookTag>() {
                    new BookTag {
                        Description = "add tags"
                    }
                };
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
        }

        public List<BookTag> ParseTags(string tags)
        {
            return tags.Split(",").Select(tag => new BookTag
            {
                Description = tag
            }).ToList();
        }
    }
}
