using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Build_Library_API.CustomActionFilter;
using WebAPI_Build_Library_API.Data;
using WebAPI_Build_Library_API.Models.Domain;
using WebAPI_Build_Library_API.Models.DTO;
using WebAPI_Build_Library_API.Repositories;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [HttpPost("add-book")]
        [ValidateModel]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
                return Ok(bookAdd);
            }
            else return BadRequest(ModelState);
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
        #region Private methods
        private bool ValidateAddBook(AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), $"Please add book data"); 
                return false;
            }
            // kiem tra Description NotNull 
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description),
                $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }
            // kiem tra rating (0,5) 
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate),
                $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion

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
