using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Api.Models
{
    public class FileUploadRequest
    {
        public string UploadName { get; set; }
        public IEnumerable<IFormFile> File { get; set; }
    }
}
