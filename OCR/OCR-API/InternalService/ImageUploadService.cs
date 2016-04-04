using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OCR_API.InternalService
{
    public class ImageUploadService
    {

        public void uploadImage(string file, string url)
        {

           
            FileWebRequest request = (FileWebRequest)FileWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/plain";
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            try
            {
                byte[] fileToSend = File.ReadAllBytes(file);
                request.ContentLength = fileToSend.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileToSend, 0, fileToSend.Length);
                requestStream.Close();

                FileWebResponse response = (FileWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception e)
            {
                string hola = e.Message;
                
            }
            
        }
        

        

    }


}