using ImageOrc.Helper;
using IronOcr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class TranslateImageController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadFile _uploadFile;
        public TranslateImageController(IConfiguration configuration, IUploadFile uploadFile)
        {
            _configuration = configuration;
            _uploadFile = uploadFile;
        }
        public IActionResult Index()
        {
            TranslateImageModel model = new TranslateImageModel();
            model.ShowText = "內容 : ";
            model.ShowTranslateText = "翻譯 : ";

            DropDownListInit(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index([FromForm] TranslateImageModel model)
        {
            DropDownListInit(model);
            try
            {
                var uploadFile = new uploadFile
                {
                    file = model.PhotoUpload,
                    folder = _configuration["UploadPath:TranslateImage"]
                };
                var imgName = _uploadFile.UploadData(uploadFile);
                if (model.PhotoUpload != null && imgName == null)
                    return BadRequest("圖片上傳失敗");
                //設定路徑
                string imgPath = null;
                if (imgName != null)
                    imgPath = _configuration["UploadPath:TranslateImage"] + "/" + imgName;
                imgPath = Path.Combine(@"wwwroot/", imgPath);
                //string imagePath = @"F:\Ian\ImageOCR\ImageOCR\image\A.jpg";
                string traineddata = string.Empty;
                string sl = string.Empty;
                if (!string.IsNullOrEmpty(model.SelectLanguage))
                {
                    switch (model.SelectLanguage)
                    {
                        case "0":
                            traineddata = "eng.traineddata";
                            sl = "en";
                            break;
                        case "1":
                            traineddata = "jpn.traineddata";
                            sl = "ja";
                            break;
                        case "2":
                            traineddata = "kor.traineddata";
                            sl = "ko";
                            break;
                    }
                }
                var Ocr = new IronTesseract();
                Ocr.UseCustomTesseractLanguageFile(Path.Combine(@"wwwroot/libary/tessdata", traineddata));

                using (var Input = new OcrInput(imgPath))
                {
                    var Result = Ocr.Read(Input);
                    model.ShowText = Result.Text;
                    string tr = TranslateText(model.ShowText,sl);
                    model.ShowTranslateText = tr;
                }

                //string imageText = new IronTesseract().Read(imgPath).Text;
                //model.ShowText = imageText;
                //string tr = TranslateText(model.ShowText);
                //model.ShowTranslateText = tr;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return View(model);
        }

        public TranslateImageModel DropDownListInit(TranslateImageModel model)
        {
            List<SelectListItem> SelectLanguageListItem = new List<SelectListItem>();
            SelectLanguageListItem.Add(new SelectListItem { Text = "英文", Value = "0" });
            SelectLanguageListItem.Add(new SelectListItem { Text = "日文", Value = "1" });
            SelectLanguageListItem.Add(new SelectListItem { Text = "韓文", Value = "2" });
            model.SelectLanguageList = SelectLanguageListItem;
            return model;
        }

        public string TranslateText(string input,string sl_text)
        {
            // Set the language from/to in the url (or pass it into this function)
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             sl_text, "zh-TW", Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;

            // Get all json data
            var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(result);

            // Extract just the first array element (This is the only data we are interested in)
            var translationItems = jsonData[0];

            // Translation Data
            string translation = "";

            // Loop through the collection extracting the translated objects
            foreach (object item in translationItems)
            {
                // Convert the item array to IEnumerable
                IEnumerable translationLineObject = item as IEnumerable;

                // Convert the IEnumerable translationLineObject to a IEnumerator
                IEnumerator translationLineString = translationLineObject.GetEnumerator();

                // Get first object in IEnumerator
                translationLineString.MoveNext();

                // Save its value (translated text)
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

            // Remove first blank character
            if (translation.Length > 1) { translation = translation.Substring(1); };

            // Return translation
            return translation;
        }
    }
}
