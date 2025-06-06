using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing.SFR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<PhoneFTP> Devices { get; } = new();
        private readonly Action<string> _sendLog;

        public PhoneFTPManager(string ipsPath, Action<string, bool> sendLog = null)
        {
            _sendLog = sendLog != null ? (msg => sendLog(msg, true)) : (_ => { });
            LoadFromFile(ipsPath);
        }

        public async Task<string> DownloadAndArchiveLogsAsync()
        {
            string remoteDir = "/Download";
            string remoteFile = "StartLog_G07A.txt";
            string remotePath = $"{remoteDir}/{remoteFile}";

            string dateOnly = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTimeTag = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string todayFolder = Path.Combine(exeDir, "Logs", dateOnly);
            Directory.CreateDirectory(todayFolder);

            List<string> successIps = new();
            List<string> failedIps = new();
            List<string> downloadedFiles = new();

            _sendLog($"[{DateTime.Now:HH:mm:ss}] 🔽 Начато скачивание логов с устройств...");

            var tasks = Devices.Select(async device =>
            {
                string safeIp = device.IP.Replace(".", "_");
                string localFile = Path.Combine(todayFolder, $"log_{safeIp}.txt");

                bool downloaded = await Task.Run(() => device.DownloadFile(remoteFile, localFile));
                if (downloaded)
                {
                    successIps.Add(device.IP);
                    downloadedFiles.Add(localFile);

                    //string header = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Лог с {device.IP}\n";
                    string header = $"\n";
                    string original = await File.ReadAllTextAsync(localFile, Encoding.UTF8);
                    await File.WriteAllTextAsync(localFile, header + original, Encoding.UTF8);

                    _sendLog($"✅ Получен файл с {device.IP}");

                    await Task.Run(() => device.MakeDirectory($"{remoteDir}/old"));

                    string newRemoteFileName = $"StartLog_G07A_{dateTimeTag}_{safeIp}.txt";
                    string renameTarget = $"{remoteDir}/old/{newRemoteFileName}";
                    bool renamed = await Task.Run(() => device.Rename(remotePath, renameTarget));
                    _sendLog(renamed
                        ? $"📁 Перемещено на {device.IP} → {renameTarget}"
                        : $"⚠️ Не удалось переместить на {device.IP}");
                }
                else
                {
                    failedIps.Add(device.IP);
                    _sendLog($"❌ Ошибка при скачивании с {device.IP}");
                }
            });

            await Task.WhenAll(tasks);

            // Объединение только тех логов, которые были скачаны сейчас
            string summaryFile = Path.Combine(todayFolder, $"summary_{dateTimeTag}.txt");
            foreach (var file in downloadedFiles.OrderBy(f => f))
            {
                string content = await File.ReadAllTextAsync(file, Encoding.UTF8);
                await File.AppendAllTextAsync(summaryFile, content + "\n", Encoding.UTF8);
            }

            _sendLog($"📝 Итоговый лог: {summaryFile}");
            _sendLog($"✅ Успешно: {string.Join(", ", successIps)}");
            _sendLog($"❌ Неудачи: {string.Join(", ", failedIps)}");

            if (!File.Exists(summaryFile))
            {
                _sendLog("⚠️ Файл summary не создан, так как не было успешно получено ни одного лога.");
                return "";
            }

            return await File.ReadAllTextAsync(summaryFile, Encoding.UTF8);

        }

        public void LoadFromFile(string ipsPath)
        {
            if (!File.Exists(ipsPath))
                throw new FileNotFoundException("Файл с IP не найден", ipsPath);

            var ips = File.ReadAllLines(ipsPath)
                          .Select(line => line.Trim())
                          .Where(line => !string.IsNullOrWhiteSpace(line));

            string configPath = "ftpconfig.json";
            PhoneFtpConfig phoneCfg;
            if (File.Exists(configPath))
            {
                try
                {
                    var json = File.ReadAllText(configPath);
                    phoneCfg = Newtonsoft.Json.JsonConvert.DeserializeObject<PhoneFtpConfig>(json) ?? new();
                }
                catch (Exception ex)
                {
                    _sendLog($"⚠️ Ошибка чтения ftpconfig.json: {ex.Message}");
                    phoneCfg = new();
                }
            }
            else
            {
                phoneCfg = new();
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(phoneCfg, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(configPath, json, new UTF8Encoding(false));
                    _sendLog($"⚠️ Конфиг {configPath} не найден. Создан шаблон.");
                }
                catch (Exception ex)
                {
                    _sendLog($"⚠️ Не удалось создать {configPath}: {ex.Message}");
                }
            }

            Devices.Clear();
            foreach (string ip in ips)
            {
                Devices.Add(new PhoneFTP(ip, phoneCfg.UserName, phoneCfg.Password));
            }
        }

        public async void SendBaseToAllFromRace(JToken race)
        {
            _sendLog("Экспорт SFRx-файла и отправка на устройства...");

            try
            {
                JToken data = PBData(race);
                string raceDate = PDStartDate(data);

                string sftxTxt = SFRxManager.RaceToSFRx(out string sfrxGenLog, race);
                _sendLog(sfrxGenLog);

                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string cacheDir = Path.Combine(exeDir, "cache");
                Directory.CreateDirectory(cacheDir);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string cacheFilename = $"{raceDate}_{timestamp}.sfrx";
                string cacheFilePath = Path.Combine(cacheDir, cacheFilename);

                File.WriteAllText(cacheFilePath, sftxTxt, new UTF8Encoding(false));
                _sendLog($"  Файл сохранён: {cacheFilePath}");

                string remoteFileName = $"{raceDate}.sfrx";

                _sendLog($"  Отправка на {Devices.Count} устройства");

                var uploadTasks = Devices.Select(async device =>
                {
                    bool ok = await Task.Run(() => device.UploadFile(cacheFilePath, "/Download/", remoteFileName));
                    string msg = ok
                        ? $"  ✅ Отправлено на {device.IP}"
                        : $"  ❌ Ошибка при отправке на {device.IP}";
                    _sendLog(msg);
                }).ToArray();

                await Task.WhenAll(uploadTasks);
                _sendLog("  Выполнение задачи Отправка FTP завершено.");
            }
            catch (Exception ex)
            {
                string err = $"  ❌ Ошибка в SendBaseToAllFromRace:\n{ex.Message}";
                _sendLog(err);
                LogError("ftp_send_sfrx", ex);
            }
        }

        public void UploadFileToAll(string localFilePath, string remoteSubDir = "/Download/")
        {
            foreach (var device in Devices)
            {
                _sendLog($"→ Отправка на {device.IP}...");
                bool ok = device.UploadFile(localFilePath, remoteSubDir);
                _sendLog(ok
                    ? $"✅ Успешно: {device.IP}"
                    : $"❌ Ошибка: {device.IP}");
            }
        }

        public void DownloadFileFromAll(string remoteFilename, string localTargetDir)
        {
            Directory.CreateDirectory(localTargetDir);
            foreach (var device in Devices)
            {
                string safeIp = device.IP.Replace(".", "_");
                string localPath = Path.Combine(localTargetDir, $"log_{safeIp}.txt");

                _sendLog($"← Скачивание с {device.IP}...");
                bool ok = device.DownloadFile(remoteFilename, localPath);
                _sendLog(ok
                    ? $"✅ Успешно: {device.IP}"
                    : $"❌ Ошибка: {device.IP}");
            }
        }

        public void CreateDirOnAll(string remoteDir)
        {
            foreach (var device in Devices)
            {
                bool ok = device.MakeDirectory(remoteDir);
                _sendLog(ok
                    ? $"📁 Папка создана на {device.IP}"
                    : $"⚠️ Не удалось создать папку на {device.IP}");
            }
        }

        public void RenameOnAll(string remoteOldPath, string remoteNewPathTemplate)
        {
            foreach (var device in Devices)
            {
                string safeIp = device.IP.Replace(".", "_");
                string remoteNewPath = remoteNewPathTemplate.Replace("{ip}", safeIp);
                bool ok = device.Rename(remoteOldPath, remoteNewPath);
                _sendLog(ok
                    ? $"🔁 Переименовано на {device.IP}"
                    : $"⚠️ Не удалось переименовать на {device.IP}");
            }
        }
    }
}
