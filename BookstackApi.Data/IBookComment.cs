using BookstackApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookstackApi.Data
{
    public interface IBookComment
    {
        Task<BookComment> GetCommentById(int id);
        IEnumerable<BookComment> GetAllComments();
        // Get comments for a book
        IEnumerable<BookComment> GetByBookId(int bookId);
        // Add Comment
        Task AddBookCommentAsync(int bookId, BookComment comment);
        Task DeleteBookCommentAsync(BookComment comment);
        Task EditBookCommentAsync(BookComment comment);
        // Delete Comment

        // TODO: Update Comment
        bool CommentExist(int id);
    }
}
