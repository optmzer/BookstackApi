using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstackApi.Data;
using BookstackApi.Data.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using BookstackApi.Helpers;
using Microsoft.Extensions.Configuration;

namespace BookstackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBook _bookService;
        private readonly IConfiguration _configuration;

        public BooksController(IBook bookService, IConfiguration configuration)
        {
            _bookService = bookService;
            _configuration = configuration;
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

        // GET: api/Books/Search/:title
        [HttpGet, Route("Search/{title}")]
        public IActionResult GetBookByTitle([FromRoute] string title)
        {
            if(title == "" || title == null)
            {
                return RedirectToAction("GetBook");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _bookService.GetByTitle(title);

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


        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]BookForm book)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            try
            {
                using (var stream = book.Image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(book.Image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }
                    //BookTag tagsList = new 

                    Book bookItem = new Book()
                    {
                        Created = DateTime.Now,
                        Title = book.Title,
                        CoverUrl = cloudBlock.SnapshotQualifiedUri.AbsoluteUri,
                        Author = book.Author,
                        ISBN = book.ISBN,
                        YearPublished = book.YearPublished,
                        BookRating = book.BookRating,
                        BookReview = book.BookReview,
                        BookTags = _bookService.ParseTags(book.tags)
                    };

                    await _bookService.AddBookAsync(bookItem);

                    return Ok($"File: {book.Title} has successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }
        }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:ACCOUNT_NAME"];
            var accountKey = _configuration["AzureBlob:ACCOUNT_KEY"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("images");

            string storageConnectionString = _configuration["AzureBlob:CONNECTION_STRING"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
            {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
            }
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