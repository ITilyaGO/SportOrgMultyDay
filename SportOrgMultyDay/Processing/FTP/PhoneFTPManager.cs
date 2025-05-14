using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing.SFR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.FTP
{
    public class PhoneFTPManager
    {
        public string Log { get; private set; } = string.Empty;
        public List<PhoneFTP> Devices { get; } = new();

        public PhoneFTPManager(string ipsPath)
        {
            LoadFromFile(ipsPath);
        }

        public void LoadFromFile(string ipsPath)
        {
            if (!File.Exists(ipsPath))
                throw new FileNotFoundException("Файл с IP не найден", ipsPath);

            var ips = File.ReadAllLines(ipsPath)
                          .Select(line => line.Trim())
                          .Where(line => !string.IsNullOrWhiteSpace(line));

            Devices.Clear();
            Devices.AddRange(ips.Select(ip => new PhoneFTP(ip)));
        }

        public void SendBaseToAllFromRace(JToken race)
        {
            Log += "Экспорт SFRx-файла и отправка на устройства...\n";

            try
            {
                JToken data = PBData(race);
                string raceDate = PDStartDate(data); // формат: yyyy-MM-dd

                string sftxTxt = SFRxManager.RaceToSFRx(out string sfrxGenLog, race);
                Log += sfrxGenLog;

                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string cacheDir = Path.Combine(exeDir, "cache");
                Directory.CreateDirectory(cacheDir);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string cacheFilename = $"{raceDate}_{timestamp}.sfrx";
                string cacheFilePath = Path.Combine(cacheDir, cacheFilename);


                File.WriteAllText(cacheFilePath, sftxTxt, new UTF8Encoding(false));
                Log += $"  Файл сохранён: {cacheFilePath}\n";

                string remoteFileName = $"{raceDate}.sfrx";

                foreach (var device in Devices)
                {
                    bool ok = device.UploadFile(cacheFilePath, "/Download/", remoteFileName);
                    Log += ok
                        ? $"  ✅ Отправлено на {device.IP}\n"
                        : $"  ❌ Ошибка при отправке на {device.IP}\n";
                }
            }
            catch (Exception ex)
            {
                Log += $"  ❌ Ошибка в SendBaseToAllFromRace:\n{ex.Message}\n";
                LogError("ftp_send_sfrx", ex);
            }
        }

        public void UploadFileToAll(string localFilePath, string remoteSubDir = "/Download/")
        {
            foreach (var device in Devices)
            {
                Log += $"→ Отправка на {device.IP}...\n";
                bool ok = device.UploadFile(localFilePath, remoteSubDir);
                Log += ok
                    ? $"✅ Успешно: {device.IP}\n"
                    : $"❌ Ошибка: {device.IP}\n";
            }
        }

        public void DownloadFileFromAll(string remoteFilename, string localTargetDir)
        {
            Directory.CreateDirectory(localTargetDir);
            foreach (var device in Devices)
            {
                string safeIp = device.IP.Replace(".", "_");
                string localPath = Path.Combine(localTargetDir, $"log_{safeIp}.txt");

                Log += $"← Скачивание с {device.IP}...\n";
                bool ok = device.DownloadFile(remoteFilename, localPath);
                Log += ok
                    ? $"✅ Успешно: {device.IP}\n"
                    : $"❌ Ошибка: {device.IP}\n";
            }
        }

        public void CreateDirOnAll(string remoteDir)
        {
            foreach (var device in Devices)
            {
                bool ok = device.MakeDirectory(remoteDir);
                Log += ok
                    ? $"📁 Папка создана на {device.IP}\n"
                    : $"⚠️ Не удалось создать папку на {device.IP}\n";
            }
        }

        public void RenameOnAll(string remoteOldPath, string remoteNewPathTemplate)
        {
            foreach (var device in Devices)
            {
                string safeIp = device.IP.Replace(".", "_");
                string remoteNewPath = remoteNewPathTemplate.Replace("{ip}", safeIp);
                bool ok = device.Rename(remoteOldPath, remoteNewPath);
                Log += ok
                    ? $"🔁 Переименовано на {device.IP}\n"
                    : $"⚠️ Не удалось переименовать на {device.IP}\n";
            }
        }

        public string GetLog()
        {
            string result = Log;
            Log = string.Empty;
            return result;
        }
    }
}