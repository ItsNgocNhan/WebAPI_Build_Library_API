using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Build_Library_API.Data;
using WebAPI_Build_Library_API.Models.Domain;
using WebAPI_Build_Library_API.Models.DTO;
using WebAPI_Build_Library_API.Repositories;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebAPI_Build_Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        public BookController(AppDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
        }
        [HttpGet("get-all-books")]
        public IActionResult GetAll()
        {
            // su dung reposity pattern 
            var allBooks = _bookRepository.GetAllBooks();
            return Ok(allBooks);
        }
        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            return Ok(bookWithIdDTO);
        }
        [HttpPost("add - book")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
            return Ok(bookAdd);
        }
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            var updateBook = _bookRepository.UpdateBookById(id, bookDTO);
            return Ok(updateBook);
        }
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var deleteBook = _bookRepository.DeleteBookById(id);
            return Ok(deleteBook);
        }

        //public BookController(_dbContext _dbContext)
        //{
        //    //FIX: 
        //    this._dbContext = _dbContext;
        //}

        //[HttpGet("get-all-books")]
        //public IActionResult GetAll()
        //{
        //    var allBooksDomain = _dbContext.Books;
        //    var allBooksDTO = allBooksDomain.Select(Books => new BookWithAuthorAndPublisherDTO()
        //    {
        //        Id = Books.Id,
        //        Title = Books.Title,
        //        Description = Books.Description,
        //        IsRead = Books.IsRead,
        //        DateRead = Books.IsRead ? Books.DateRead : null,
        //        Rate = Books.IsRead ? Books.Rate : null,
        //        Genre = Books.Genre,
        //        CoverUrl = Books.CoverURL,
        //        PublisherName = Books.Publisher.Name,
        //        AuthorNames = Books.Book_Authors.Select(n => n.Author.FullName).ToList()
        //    }).ToList();
        //    return Ok(allBooksDTO);
        //}

        //[HttpGet]
        //[Route("get-book-by-id/{id}")]
        //public IActionResult GetBookById([FromRoute] int id)
        //{
        //    var BookWithDomian = _dbContext.Books.Where(n => n.Id == id);
        //    if (BookWithDomian == null)
        //    {
        //        return NotFound();
        //    }
        //    var BookWithIdDTO = BookWithDomian.Select(book => new BookWithAuthorAndPublisherDTO()
        //    {
        //        Id = book.Id,
        //        Title = book.Title,
        //        Description = book.Description,
        //        IsRead = book.IsRead,
        //        DateRead = book.DateRead,
        //        Rate = book.Rate,
        //        Genre = book.Genre,
        //        CoverUrl = book.CoverURL,
        //        PublisherName = book.Publisher.Name,
        //        AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
        //    });
        //    return Ok(BookWithIdDTO);
        //}

        //[HttpPost("add-book")]
        //public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        //{
        //    var bookDomainModel = new Book
        //    {
        //        Title = addBookRequestDTO.Title ?? "Untitled",
        //        Description = addBookRequestDTO.Description ?? "Untitled",
        //        IsRead = addBookRequestDTO.IsRead,
        //        DateRead = addBookRequestDTO.DateRead,
        //        Rate = addBookRequestDTO.Rate,
        //        Genre = addBookRequestDTO.Genre,
        //        CoverURL = addBookRequestDTO.CoverUrl,
        //        DateAdded = addBookRequestDTO.DateAdded,
        //        PublisherID = addBookRequestDTO.PublisherId
        //    };
        //    _dbContext.Books.Add(bookDomainModel);
        //    _dbContext.SaveChanges();
        //    foreach (var id in addBookRequestDTO.AuthorIds)
        //    {
        //        var _book_author = new Book_Author()
        //        {
        //            BookId = bookDomainModel.Id,
        //            AuthorId = id
        //        };
        //        _dbContext.Book_Authors.Add(_book_author);
        //        _dbContext.SaveChanges();
        //    }
        //    return Ok();
        //}

        //[HttpPut("update-book-by-id/{id}")]
        //public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO)
        //{
        //    var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
        //    if (bookDomain != null)
        //    {
        //        bookDomain.Title = bookDTO.Title ?? "Untitled";
        //        bookDomain.Description = bookDTO.Description ?? "Untitled";
        //        bookDomain.IsRead = bookDTO.IsRead;
        //        bookDomain.DateRead = bookDTO.DateRead;
        //        bookDomain.Rate = bookDTO.Rate;
        //        bookDomain.Genre = bookDTO.Genre;
        //        bookDomain.CoverURL = bookDTO.CoverUrl;
        //        bookDomain.DateAdded = bookDTO.DateAdded;
        //        bookDomain.PublisherID = bookDTO.PublisherId;
        //        _dbContext.SaveChanges();
        //    }
        //    var authorDomain = _dbContext.Book_Authors.Where(a => a.BookId == id).ToList();
        //    if (authorDomain != null)
        //    {
        //        _dbContext.Book_Authors.RemoveRange(authorDomain);
        //        _dbContext.SaveChanges();
        //    }
        //    foreach (var authorid in bookDTO.AuthorIds)
        //    {
        //        var _book_author = new Book_Author()
        //        {
        //            BookId = id,
        //            AuthorId = authorid,
        //        };

        //        _dbContext.Book_Authors.Add(_book_author);
        //        _dbContext.SaveChanges();
        //    }
        //    return Ok(bookDTO);
        //}
        //[HttpDelete("delete-book-by-id/{id}")]
        //public IActionResult DeleteBookById(int id)
        //{
        //    var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
        //    if (bookDomain != null)
        //    {
        //        _dbContext.Books.Remove(bookDomain);
        //        _dbContext.SaveChanges();
        //    }
        //    return Ok();
        //}
    }
}
