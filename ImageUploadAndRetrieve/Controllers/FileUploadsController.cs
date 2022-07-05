using ImageUploadAndRetrieve.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageUploadAndRetrieve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<string> Post([FromForm] FileUpload fileUpload)
        {
            try
            {
                if (fileUpload.files.Length > 0)
                {
                    string path = this.webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "Upload Done";
                    }
                }
                else
                {
                    return "Failed.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get([FromRoute] string fileName)
        {
            string path = this.webHostEnvironment.WebRootPath + "\\uploads\\";
            var filePath = path + fileName + ".png";
            if (System.IO.File.Exists(filePath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
                return File(b, "image/png");
            }
            return null;
        }
    }
}
