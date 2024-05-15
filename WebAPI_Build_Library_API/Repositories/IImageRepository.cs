using WebAPI_Build_Library_API.Models;

namespace WebAPI_Build_Library_API.Repositories
{
    public interface IImageRepository
    {
        Image Upload(Image image);
        List<Image> GetAllInfoImages();
        (byte[], string, string) DownloadFile(int Id);  
    }
}
