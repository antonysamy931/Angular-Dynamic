using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using ImageResizer;

namespace FileUpload.Angular.Controllers
{
    public class UploadController : ApiController
    {
        private Dictionary<string, string> settings = new Dictionary<string, string>();

        public UploadController()
        {
            settings.Add("1", "width=200;height=200;format=jpg;mode=max");
            settings.Add("2", "width=400;height=400;format=jpg;mode=max");
        }
        [HttpPost]
        public string UploadFiles(int id)
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/" + id + "/");

            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }

            HttpFileCollection hfc = HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    string newPath = sPath + Path.GetFileName(hpf.FileName);
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(newPath))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(newPath);
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                    foreach (var item in settings)
                    {
                        var filePath = sPath + Path.GetFileNameWithoutExtension(hpf.FileName) + "_" + item.Key + ".jpg";
                        ImageBuilder.Current.Build(newPath, filePath, new ResizeSettings(item.Value));
                    }
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else
            {
                return "Upload Failed";
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(string name)
        {
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/");
            sPath = sPath + name + ".jpg";
            if (!File.Exists(sPath))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            FileStream fileStream = File.Open(sPath, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage { Content = new StreamContent(fileStream) };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            response.Content.Headers.ContentLength = fileStream.Length;
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPdf(string name)
        {
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/");
            sPath = sPath + name + ".pdf";
            if (!File.Exists(sPath))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            FileStream fileStream = File.Open(sPath, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage { Content = new StreamContent(fileStream) };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = fileStream.Length;
            return response;
        }

        [HttpGet]
        public byte[] GetImage(string name)
        {
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/");
            sPath = sPath + name + ".jpg";
            if (!File.Exists(sPath))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            FileStream fileStream = File.Open(sPath, FileMode.Open);
            byte[] content = ReadFully(fileStream);
            fileStream.Dispose();
            return content;
        }

        [HttpGet]
        public byte[] GetPdfByte(string name)
        {
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/");
            sPath = sPath + name + ".pdf";
            if (!File.Exists(sPath))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            FileStream fileStream = File.Open(sPath, FileMode.Open);
            byte[] content = ReadFully(fileStream);
            fileStream.Dispose();
            return content;
        }

        [HttpGet]
        public List<ImageList> GetImages(int id)
        {
            List<ImageList> images = new List<ImageList>();
            var directoryPath = HostingEnvironment.MapPath("~/locker/" + id + "/");
            var FilesInfo = new DirectoryInfo(directoryPath).GetFiles("*.jpg");
            foreach (var file in FilesInfo)
            {
                ImageList image = new ImageList();
                image.Name = file.Name.Split('.')[0];
                image.Url = "/Locker/" + id + "/" + file.Name;
                FileStream stream = File.Open(file.FullName, FileMode.Open);
                image.Content = ReadFully(stream);
                stream.Dispose();
                images.Add(image);
            }
            return images;
        }

        [HttpPost]
        public void Delete(int id, string name)
        {
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/" + id + "/");
            var newPath = sPath + name;
            File.Delete(newPath + ".jpg");
            foreach (var item in settings)
            {
                File.Delete(newPath + string.Format("_{0}.jpg", item.Key));
            }
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                buffer = ms.ToArray();
                ms.Dispose();
            }
            return buffer;
        }
    }

    public class ImageList
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Url { get; set; }
    }
}
