using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class uploadFile
    {
        public List<IFormFile> multifiles { get; set; }
        public IFormFile file { get; set; }
        public string folder { get; set; }
    }
}
