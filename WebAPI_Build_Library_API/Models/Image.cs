using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Build_Library_API.Models
{
    public class Image
    {
        public int Id { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FizeSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
