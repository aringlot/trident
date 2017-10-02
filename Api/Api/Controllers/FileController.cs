using System.IO;
using System.Text;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    public class FileController : Controller
    {
        [HttpGet]
        public IActionResult Get(string id)
        {
            var encoding = Encoding.UTF8;

            var contentString = $"{encoding.GetString(encoding.GetPreamble())}1;2;3;4";
            var contentArray = Encoding.UTF8.GetBytes(contentString);

            var stream = new MemoryStream(contentArray);

            var response = File(stream, "text/csv");
            return response;
        }

        [HttpPost]
        public IActionResult Post(FileUploadRequest request)
        {
            return Content(request.UploadName);
        }

    }
}
