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

namespace FileUpload.Angular.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        public string UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = HostingEnvironment.MapPath("~/locker/");

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
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
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
}
