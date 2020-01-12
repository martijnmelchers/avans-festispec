using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Festispec.Web.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return Ok("");
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var filepathResult = ""; 
            foreach (var formFile in Request.Form.Files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", formFile.FileName);
                if (formFile.Length <= 0) continue;
                await using (var inputStream = new FileStream(filePath, FileMode.Create))
                {
                    // read file to stream
                    await formFile.CopyToAsync(inputStream);
                    // stream to byte array
                    var array = new byte[inputStream.Length];
                    
                    inputStream.Seek(0, SeekOrigin.Begin);
                    inputStream.Read(array, 0, array.Length);

                    filepathResult = $"/Uploads/{formFile.FileName}";
                }
            }
            return Ok(filepathResult);
        }
    }
}
