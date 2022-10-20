using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Project.Models;
using System;
using System.IO;

namespace ImageOrc.Helper
{
    #region 檔案上傳 介面
    public interface IUploadFile
    {
        /// <summary>
        /// Upload Data
        /// </summary>
        /// <param name="uploadData"></param>
        /// <returns></returns>
        string UploadData(uploadFile uploadData);

        /// <summary>
        /// 從附檔名取得檔案型態
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        string getFileContentType(string FileName);
    }
    #endregion 檔案上傳 介面

    #region 檔案上傳 實作
    public class UploadFile : IUploadFile
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        #region 建構式
        public UploadFile(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }
        #endregion 建構式

        /// <summary>
        /// Upload Data
        /// </summary>
        /// <param name="uploadData"></param>
        /// <returns></returns>
        #region Upload Data --UploadData(uploadFile uploadData)
        public string UploadData(uploadFile uploadData)
        {
            if (uploadData.file != null)
            {
                if (uploadData.file.Length <= 0) return null;
                //string path = System.IO.Path.Combine(_env.WebRootPath, uploadData.folder);
                string path = System.IO.Path.Combine(@"wwwroot/", uploadData.folder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var ext = Path.GetExtension(uploadData.file.FileName).ToLower();
                var fileName = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.Ticks + Path.GetExtension(uploadData.file.FileName).ToLower();
                var filePath = Path.Combine(path, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadData.file.CopyTo(stream);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
                return fileName;

            }
            else
            {
                return null;
            }
        }
        #endregion Upload Data --UploadData(uploadFile uploadData)

        /// <summary>
        /// 從附檔名取得檔案型態
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        #region 從附檔名取得檔案型態 --getFileContentType(string FileName)
        public string getFileContentType(string FileName)
        {
            var extension = Path.GetExtension(FileName);
            string typeS;
            switch (extension)
            {
                case ".doc":
                case ".dot":
                    typeS = "application/msword";
                    break;
                case ".docx":
                    typeS = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".dotx":
                    typeS = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;
                case ".docm":
                    typeS = "application/vnd.ms-word.document.macroEnabled.12";
                    break;
                case ".dotm":
                    typeS = "application/vnd.ms-word.template.macroEnabled.12";
                    break;
                case ".xls":
                case ".xlt":
                case ".xla":
                    typeS = "application/vnd.ms-excel";
                    break;
                case ".ppt":
                case ".pot":
                case ".pps":
                case ".ppa":
                    typeS = "application/vnd.ms-powerpoint";
                    break;
                case ".xlsx":
                    typeS = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".mdb":
                    typeS = "application/vnd.ms-access";
                    break;
                case ".mp3":
                    typeS = "audio/mpeg3";
                    break;
                case ".gif":
                    typeS = "image/gif";
                    break;
                case ".jpg":
                case "jpeg":
                    typeS = "image/jpeg";
                    break;
                case ".wav":
                    typeS = "audio/wav";
                    break;
                default:
                    typeS = "text/plain";
                    break;
            }

            return typeS;
        }
        #endregion 從附檔名取得檔案型態 --getFileContentType(string FileName)

    }
    #endregion 檔案上傳 實作
}
