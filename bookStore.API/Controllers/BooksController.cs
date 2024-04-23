using bookStore.API.Models;
using bookStore.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet("")]//for geting all book
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await bookRepository.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")] //for getting a single book
        public async Task<IActionResult> GetBookById([FromRoute]int id)
        {
            var book = await bookRepository.GetBookByIdAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            return Ok(book);

        }

        [HttpPost("")]//adding a Book
        public async Task<IActionResult> AddNewBook([FromBody] BookModel bookModel)
        {
            var id = await bookRepository.AddBookAsync(bookModel);
            return CreatedAtAction(nameof(GetBookById), new { id = id, controller = "books" }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromBody] BookModel bookModel, [FromRoute] int id)
        {
            await bookRepository.UpdateBookAsync(id, bookModel);
            return Ok(bookModel);
        }

        //For HttpPatch need to install 2 more packge
        //1. JsonPatch
        //2. NewtonsoftJson
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookPatch([FromBody] JsonPatchDocument bookModel, [FromRoute] int id )
        {
            await bookRepository.UpdateBookPatchAsync(id, bookModel);
            return Ok(bookModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute]int id)
        {
            await bookRepository.DeleteBookAsync(id);
            
            return Ok("Successfully Deleted");
        }
    }
}
