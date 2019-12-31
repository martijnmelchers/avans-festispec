using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Saus");
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var filepathResult = ""; 
            foreach (var formFile in Request.Form.Files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", formFile.FileName);
                if (formFile.Length > 0)
                {
                    using (var inputStream = new FileStream(filePath, FileMode.Create))
                    {
                        // read file to stream
                        await formFile.CopyToAsync(inputStream);
                        // stream to byte array
                        byte[] array = new byte[inputStream.Length];
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.Read(array, 0, array.Length);
                        // get file name
                        string fName = formFile.FileName;

                        filepathResult = String.Format("/Uploads/{0}", formFile.FileName);
                    }
                }
            }
            return Ok(filepathResult);
        }
    }
}
