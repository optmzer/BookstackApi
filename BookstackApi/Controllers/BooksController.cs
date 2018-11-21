using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstackApi.Data;
using BookstackApi.Data.Models;

namespace BookstackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBook _bookService;

        public BooksController(IBook bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public IEnumerable<Book> GetBook()
        {
            return _bookService.GetAll();
        }

        // GET: api/Books/5
        [HttpGet("{bookId}")]
        public IActionResult GetBook([FromRoute] int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _bookService.GetById(bookId);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [HttpPut("{bookId}")]
        public async Task<IActionResult> PutBook([FromRoute] int bookId, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bookId != book.Id)
            {
                return BadRequest();
            }

            var bookExists = _bookService.BookExists(bookId);

            try
            {
                await _bookService.EditBookAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bookExists)
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

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookService.AddBookAsync(book);

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _bookService.GetById(bookId);

            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBookAsync(book);

            return Ok(book);
        }
        
    }
}