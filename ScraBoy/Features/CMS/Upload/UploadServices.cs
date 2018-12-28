using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Upload
{
    public class UploadController:Controller
    { 
        public string GetFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
        public string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }
        public string GetFullFile(string fileName)
        {
            return GetFileName(fileName) + DateTime.Now.ToString("yymmssfff") + GetExtension(fileName);
        }
        public bool CheckFileType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch(ext.ToLower())
            {
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }

        }
        public void DeleteOldImage(string image)
        {
            var path = Server.MapPath(image);

            if(System.IO.File.Exists(path))
                System.IO.File.Delete(path);

        }
        public void SaveImage(HttpPostedFileBase file,string pathFolder,string filePath)
        {
            var dirPath = Path.Combine(Server.MapPath(pathFolder),filePath);

            Image image = Image.FromStream(file.InputStream,true,true);

            EncoderParameters encoder_params = new EncoderParameters(1);
            encoder_params.Param[0] = new EncoderParameter(Encoder.Quality,25L);

            ImageCodecInfo image_codec_info =
                GetEncoderInfo("image/jpeg");
            image.Save(dirPath,image_codec_info,encoder_params);
        }
        private ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for(int i = 0; i <= encoders.Length; i++)
            {
                if(encoders[i].MimeType == mime_type) return encoders[i];
            }
            return null;
        }
    }
}