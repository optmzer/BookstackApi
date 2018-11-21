using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstackApi.Data;
using BookstackApi.Data.Models;

namespace BookstackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCommentsController : ControllerBase
    {
        //private readonly BookstackApiDdContext _context;
        private readonly IBookComment _bookCommentService;

        public BookCommentsController(IBookComment bookCommentService)
        {
            _bookCommentService = bookCommentService;
        }

        // GET: api/BookComments
        [HttpGet]
        public IEnumerable<BookComment> GetBookComments()
        {
            return _bookCommentService.GetAllComments();
        }

        // GET: api/BookComments/5
        [HttpGet("{bookId}")]
        public IActionResult GetBookComments([FromRoute] int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookComment = _bookCommentService.GetByBookId(bookId);

            if (bookComment == null)
            {
                return NotFound();
            }

            return Ok(bookComment);
        }

        // GET: api/BookComments/Comment5
        [HttpGet("Comment/{commentId}")]
        public IActionResult GetBookComment([FromRoute] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookComment = _bookCommentService.GetCommentById(commentId);

            if (bookComment == null)
            {
                return NotFound();
            }

            return Ok(bookComment);
        }
        // PUT: api/BookComments/5
        [HttpPut("{commentId}")]
        public async Task<IActionResult> PutBookComment([FromRoute] int commentId, [FromBody] BookComment bookComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentId != bookComment.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bookCommentService.EditBookCommentAsync(bookComment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bookCommentService.CommentExist(commentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BookComments/{int bookId}
        [HttpPost("{bookId}")]
        public async Task<IActionResult> PostBookComment([FromRoute] int bookId, [FromBody] BookComment bookComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookCommentService.AddBookCommentAsync(bookId, bookComment);

            return CreatedAtAction("GetBookComment", new { commentId = bookComment.Id }, bookComment);
        }

        // DELETE: api/BookComments/{int commentId}
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteBookComment([FromRoute] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookComment = await _bookCommentService.GetCommentById(commentId);
            if (bookComment == null)
            {
                return NotFound();
            }

            await _bookCommentService.DeleteBookCommentAsync(bookComment);

            return Ok(bookComment);
        }
    }
}