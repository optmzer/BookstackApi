using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstackApi.Data.Models;
using BookstackApi.Data;

namespace BookstackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookTagsController : ControllerBase
    {
        private readonly IBookTag _bookTagService;

        public BookTagsController(IBookTag bookTagService)
        {
            _bookTagService = bookTagService;
        }

        // GET: api/BookTags
        [HttpGet]
        public IEnumerable<BookTag> GetBookTags()
        {
            return _bookTagService.GetAll();
        }

        // GET: api/BookTags/5
        [HttpGet("{bookId}")]
        public IActionResult GetBookTag([FromRoute] int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookTag = _bookTagService.GetByBookId(bookId);

            if (bookTag == null)
            {
                return NotFound();
            }

            return Ok(bookTag);
        }

        // PUT: api/BookTags/5
        [HttpPut("{bookTagId}")]
        public async Task<IActionResult> PutBookTag([FromRoute] int bookTagId, [FromBody] BookTag bookTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bookTagId != bookTag.Id)
            {
                return BadRequest();
            }


            try
            {
                await _bookTagService.EditBookCommentAsync(bookTag);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bookTagService.BookTagExist(bookTagId))
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

        // POST: api/BookTags/{bookId}
        [HttpPost("{bookId}")]
        public async Task<IActionResult> PostBookTag([FromRoute] int bookId, [FromBody] BookTag bookTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookTagService.AddBookTagAsync(bookId, bookTag);

            return CreatedAtAction("GetBookTag", new { bookId = bookTag.Id }, bookTag);
        }

        // DELETE: api/BookTags/5
        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteBookTag([FromRoute] int tagId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookTag = await _bookTagService.GetBookTagByIdAsync(tagId);
            if (bookTag == null)
            {
                return NotFound();
            }

            await _bookTagService.DeleteBookCommentAsync(bookTag);

            return Ok(bookTag);
        }
    }
}