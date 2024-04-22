using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Build_Library_API.Models.Domain
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        //Realtion
        public List<Book_Author>? Book_Authors { get; set; }
    }
}
