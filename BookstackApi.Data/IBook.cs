using BookstackApi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookstackApi.Data
{
    public interface IBook
    {
        // TODO: Add 
        // IEnumerable<Book> GetByTag(string tag);
        // IEnumerable<Book> GetByAuthor(string tag);
        // IEnumerable<Book> GetByTitle(string tag);
        bool BookExists(int id);
        Book GetById(int bookId);
        IEnumerable<Book> GetAll();
        IEnumerable<Book> GetLatest(int numberOfBooks);
        IEnumerable<Book> GetByTag(string tag);

        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Book book);
        Task EditBookAsync(Book book);
    }
}
