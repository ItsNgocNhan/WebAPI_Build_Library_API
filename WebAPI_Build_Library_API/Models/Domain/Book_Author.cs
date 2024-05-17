using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Build_Library_API.Models.Domain
{
    public class Book_Author
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        //Relation
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}