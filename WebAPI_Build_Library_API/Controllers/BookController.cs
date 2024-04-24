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
                AuthorNames = Books.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).ToList();
            return Ok(allBooksDTO);
        }
        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var BookWithDomian = appDbContext.Books.Where(n => n.Id == id);
            if (BookWithDomian == null)
            {
                return NotFound();
            }
            var BookWithIdDTO = BookWithDomian.Select(book => new BookWithAuthorAndPublisherDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverURL,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            });
            return Ok(BookWithIdDTO);
        }
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
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO) 
        {
            var bookDomain = appDbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null) 
            {
                bookDomain.Title = bookDTO.Title;
                bookDomain.Description = bookDTO.Description;
                bookDomain.IsRead = bookDTO.IsRead;
                bookDomain.DateRead = bookDTO.DateRead;
                bookDomain.Rate = bookDTO.Rate;
                bookDomain.Genre = bookDTO.Genre;
                bookDomain.CoverURL = bookDTO.CoverUrl;
                bookDomain.DateAdded = bookDTO.DateAdded;
                bookDomain.PublisherID = bookDTO.PublisherId;
                appDbContext.SaveChanges();
            }
            var authorDomain = appDbContext.Book_Authors.Where(a => a.BookId == id).ToList();
            if (authorDomain != null)
            {
                appDbContext.Book_Authors.RemoveRange(authorDomain);
                appDbContext.SaveChanges();
            }
            foreach (var authorid in bookDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorid,
                };

                appDbContext.Book_Authors.Add(_book_author);
                appDbContext.SaveChanges();
            }
            return Ok(bookDTO);
        }
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var bookDomain = appDbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                appDbContext.Books.Remove(bookDomain);
                appDbContext.SaveChanges();
            }
            return Ok();
        }
    }
}
