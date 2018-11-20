using BookstackApi.Data.Models;
using System.Collections.Generic;

namespace BookstackApi.Data
{
    public interface IBook
    {
        IEnumerable<Book> GetAll();
        IEnumerable<Book> GetByTag(string tag);
        Book GetById(int bookId);
    }
}
