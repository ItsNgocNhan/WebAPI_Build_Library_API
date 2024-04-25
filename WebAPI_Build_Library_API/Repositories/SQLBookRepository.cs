using Microsoft.EntityFrameworkCore;
using System;
using WebAPI_Build_Library_API.Data;
using WebAPI_Build_Library_API.Models.Domain;
using WebAPI_Build_Library_API.Models.DTO;

namespace WebAPI_Build_Library_API.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BookWithAuthorAndPublisherDTO> GetAllBooks()
        {
            var allBooks = _dbContext.Books.Select(Books => new BookWithAuthorAndPublisherDTO()
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
            return allBooks;
        }
        public BookWithAuthorAndPublisherDTO GetBookById(int id)
        {
            var bookWithDomain = _dbContext.Books.Where(n => n.Id == id);
            //Map Domain Model to DTOs
            var bookWithIdDTO = bookWithDomain.Select(book => new
           BookWithAuthorAndPublisherDTO()
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
            }).FirstOrDefault();
            return bookWithIdDTO;
        }
        public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            //map DTO to Domain Model
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
            //Use Domain Model to add Book 
            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();
            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = id
                };
                _dbContext.Book_Authors.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return addBookRequestDTO;
        }
        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
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
                _dbContext.SaveChanges();
            }
            var authorDomain = _dbContext.Book_Authors.Where(a => a.BookId == id).ToList();
            if (authorDomain != null)
            {
                _dbContext.Book_Authors.RemoveRange(authorDomain);
                _dbContext.SaveChanges();
            }
            foreach (var authorid in bookDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorid
                };
                _dbContext.Book_Authors.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return bookDTO;
        }
        public Book? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain;
        }
    }
}
