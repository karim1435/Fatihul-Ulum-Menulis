using System;
using System.Collections.Generic;
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
                case ".gif":
                    return true;
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
            {
                System.IO.File.Delete(path);
            }
        }
        public void SaveImage(HttpPostedFileBase file,string pathFolder,string filePath)
        {
            var dirPath = Path.Combine(Server.MapPath(pathFolder),filePath);
            file.SaveAs(dirPath);
        }

    }
}