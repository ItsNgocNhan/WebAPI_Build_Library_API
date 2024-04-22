using System.ComponentModel.DataAnnotations;

namespace WebAPI_Build_Library_API.Models.Domain
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        //Relation
        public List<Book> Books { get; set; }
    }
}
