using BookstackApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookstackApi.Data
{
    public interface IBookTag
    {
        IEnumerable<BookTag> GetAll();
        IEnumerable<BookTag> GetByBookId(int bookId);
        Task<BookTag> GetBookTagByIdAsync(int bookTagId);
        Task AddBookTagAsync(int bookId, BookTag bookTag);
        Task EditBookCommentAsync(BookTag bookTag);
        Task DeleteBookCommentAsync(BookTag bookTag);
        // Delete Comment

        // TODO: Update Comment
        bool BookTagExist(int bookTagId);
    }
}
