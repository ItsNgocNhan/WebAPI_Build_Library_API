using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Build_Library_API.Data;
using WebAPI_Build_Library_API.Models.Domain;
using WebAPI_Build_Library_API.Models.DTO;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebAPI_Build_Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public BookController(AppDbContext appDbContext)
        {
            appDbContext = appDbContext;
        }

        [HttpGet("get-all-books")]
        public IActionResult GetAll()
        {
            var allBooksDomain = appDbContext.Books;
            var allBooksDTO = allBooksDomain.Select(Books => new BookWithAuthorAndPublisherDTO()
            {
                Id = Books.Id,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.DateRead.Value : null,
                Rate = Books.IsRead ? Books.Rate.Value : null,
                Genre = Books.Genre,
                CoverUrl = Books.CoverURL,
                PublisherName = Books.Publisher.Name,
                AuthorNames = Books.Book_Authors.Select(Book_Author => Book_Author.Author.FullName).ToList()
            }).ToList();
            return Ok(allBooksDTO);
        }
        //[HttpGet("{id}")]
        //public ActionResult<Book> GetBook(int id)
        //{
        //    var book = appDbContext.Books.Find(id);

        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return book;
        //}
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            var bookDomainModel = new Book
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverURL = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = addBookRequestDTO.PublisherId
            };
            appDbContext.Books.Add(bookDomainModel);
            appDbContext.SaveChanges();
            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = id
                };
                appDbContext.Book_Authors.Add(_book_author);
                appDbContext.SaveChanges();
            }
            return Ok();
        }
        //[HttpPost]
        //public IActionResult PostBook(Book book)
        //{
        //    appDbContext.Books.Add(book);
        //    appDbContext.SaveChanges();

        //    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        //}

        //[HttpPut("{id}")]
        //public IActionResult PutBook(int id, Book book)
        //{
        //    if (id != book.Id)
        //    {
        //        return BadRequest();
        //    }

        //    appDbContext.Entry(book).State = EntityState.Modified;
        //    appDbContext.SaveChanges();

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteBook(int id)
        //{
        //    var book = appDbContext.Books.Find(id);

        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    appDbContext.Books.Remove(book);
        //    appDbContext.SaveChanges();

        //    return NoContent();
        //}
    }
}
