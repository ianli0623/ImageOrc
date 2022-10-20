using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class TranslateImageModel
    {
        public string SelectLanguage { get; set; }
        public List<SelectListItem> SelectLanguageList { get; set; }
        public string ShowText { get; set; }
        public string ShowTranslateText { get; set; }
        public IFormFile PhotoUpload { get; set; }
    }
}
