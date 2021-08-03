using FTPPMAC.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Action
{
    public class UploadFileFTPAction
    {
        private string host = "ftp://112.78.4.162/";
        private string path = "SL";
        private string user = "demo1";
        private string pass = "123456";
        public void Upload(string folder , string file)
        {
            Log_Controller log = new Log_Controller();

            try
            {

                if(!CheckExistDirectory(path))
                {
                    CreateDirectory(path);
                }

                string pathFileUpload = Path.Combine(folder, file);

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + path + "/"+ file);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(user, pass);

                // Copy the contents of the file to the request stream.
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(pathFileUpload))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    log.WriteLog($"Upload File {file} Complete status with {response.StatusDescription}", "Upload Success", false);
                }
            }
            catch(WebException ex)
            {
                log.WriteLog($"Upload file {file} fail with error: {ex.Message}", "Upload Fail", true);
            }
            
        }    

        public bool CreateDirectory(string path)
        {
            Log_Controller log = new Log_Controller();

            bool IsCreated = true;
            try
            {
                WebRequest request = WebRequest.Create(host+ path);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(user, pass);
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    log.WriteLog($"Create folder {path} completed", "Create Success", false);
                }
            }
            catch (Exception ex)
            {
                IsCreated = false;
            }
            return IsCreated;
        }

        public bool CheckExistDirectory(string path)
        {
            bool isexist = false;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host+ path);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    isexist = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
            }
            return isexist;
        }
    }
}
