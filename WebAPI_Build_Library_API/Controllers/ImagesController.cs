using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Build_Library_API.Models;
using WebAPI_Build_Library_API.Models.DTO;
using WebAPI_Build_Library_API.Repositories;

namespace WebAPI_Build_Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        //Phương thức upload
        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);
            if(ModelState.IsValid)
            {
                //Convert DTO to Domain Model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FizeSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };
                _imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 1040000)
            {
                ModelState.AddModelError("file", "File size too big, please upload file <10MB");
            }
        }
        [HttpGet]
        public IActionResult GetAllInfoImages() 
        {
            var allImages = _imageRepository.GetAllInfoImages();
            return Ok(allImages);
        }
        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadImage(int id) 
        {
            var result = _imageRepository.DownloadFile(id);
            return File(result.Item1, result.Item2, result.Item3);
        }
    }
}
