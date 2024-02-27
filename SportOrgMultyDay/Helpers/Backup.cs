using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Helpers
{
    public static class Backup
    {
        public static bool BackUp(string filename, string backupName)
        {
            if (!File.Exists(filename))
                return false;
            try
            {
                string archivePath = $"backups_{backupName}.zip";
                if (File.Exists(archivePath))
                    using (ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Update))
                    {
                        zipArchive.CreateEntryFromFile(filename, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ".json");
                    }
                else
                    using (ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
                    {
                        zipArchive.CreateEntryFromFile(filename, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ".json");
                    }
            }
            catch (Exception e)
            {
                return DialogResult.No == MessageBox.Show(e.Message + " \n\nЗакрыть программу?", "Архивация не удалась", MessageBoxButtons.YesNo);
            }
            return false;
        }
    }
}
