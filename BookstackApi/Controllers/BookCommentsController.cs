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
        [HttpGet("{id}")]
        public IActionResult GetBookComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookComment = _bookCommentService.GetByBookId(id);

            if (bookComment == null)
            {
                return NotFound();
            }

            return Ok(bookComment);
        }

        // PUT: api/BookComments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookComment([FromRoute] int id, [FromBody] BookComment bookComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookComment.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bookCommentService.EditBookCommentAsync(bookComment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bookCommentService.CommentExist(id))
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

        // POST: api/BookComments/5
        [HttpPost("{id}")]
        public async Task<IActionResult> PostBookComment([FromRoute] int id, [FromBody] BookComment bookComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookCommentService.AddBookCommentAsync(id, bookComment);

            return CreatedAtAction("GetBookComment", new { id = bookComment.Id }, bookComment);
        }

        // DELETE: api/BookComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookComment = await _bookCommentService.GetCommentById(id);
            if (bookComment == null)
            {
                return NotFound();
            }

            await _bookCommentService.DeleteBookCommentAsync(bookComment);

            return Ok(bookComment);
        }
    }
}