using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Diagnostics;

namespace SportOrgMultyDay.Processing.FTP
{

    public class PhoneFTP
    {
        public string IP { get; }
        public int Port { get; set; } = 2221;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public PhoneFTP(string ip, string username, string password )
        {
            IP = ip;
            Username = username;
            Password = password;
        }

        private string GetFtpUri(string remotePath)
        {
            return $"ftp://{IP}:{Port}{remotePath}";
        }

        public bool UploadFile(string localPath, string remoteRelativePath = "/Download/", string remoteFilenameOverride = null)
        {
            try
            {
                string filename = remoteFilenameOverride ?? Path.GetFileName(localPath);
                string remoteFilePath = GetFtpUri(remoteRelativePath + filename);
                var request = (FtpWebRequest)WebRequest.Create(remoteFilePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                request.UseBinary = true;
                request.UsePassive = true;
                request.Timeout = 5000;

                byte[] fileContents = File.ReadAllBytes(localPath);
                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(fileContents, 0, fileContents.Length);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    return response.StatusCode == FtpStatusCode.ClosingData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке на {IP}: {ex.Message}");
                return false;
            }
        }


        public bool DownloadFile(string remoteFilename, string localPath)
        {
            try
            {
                string remoteFilePath = GetFtpUri($"/Download/{remoteFilename}");
                var request = (FtpWebRequest)WebRequest.Create(remoteFilePath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                request.UseBinary = true;
                request.UsePassive = true;
                request.Timeout = 5000;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = File.Create(localPath))
                {
                    responseStream.CopyTo(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при скачивании с {IP}: {ex.Message}");
                return false;
            }
        }

        public bool Rename(string remoteOldPath, string remoteNewPath)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(GetFtpUri(remoteOldPath));
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = remoteNewPath;
                request.Credentials = new NetworkCredential(Username, Password);
                request.Timeout = 5000;
                request.UseBinary = true;
                request.UsePassive = true;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse()) { }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка переименования на {IP}: {ex.Message}");
                return false;
            }
        }

        public bool MakeDirectory(string remoteDir)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(GetFtpUri(remoteDir));
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(Username, Password);
                request.Timeout = 3000;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse()) { }

                return true;
            }
            catch
            {
                // Не страшно, если папка уже существует
                return false;
            }
        }
    }

}
