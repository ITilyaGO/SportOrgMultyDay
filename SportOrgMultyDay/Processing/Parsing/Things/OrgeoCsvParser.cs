using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Combine.ResultsCountInGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Logger;
using System.Text.RegularExpressions;

namespace SportOrgMultyDay.Processing.Parsing.Things
{
    public static class OrgeoCsvParser
    {
        public static string AddRegionsKodToOrgs(JToken race, string csvStr, bool renameOrgs)
        {
            string msgLog = "Добавление регионов из CSV в названия команд...\n";
            try
            {
                List<CsvPersons> csvPersons = ParseCsvUTF8(csvStr);
                msgLog += $"  Строки в CSV {csvPersons.Count}\n";
                JArray orgs = PBOrganizations(race);
                msgLog += $"  Команды в базе {orgs.Count}\n";
                msgLog += $"  Переиминование команд...\n";
                int complited = 0;
                for (int i = 0; i < orgs.Count; i++)
                {
                    try
                    {
                        JToken org = orgs[i];
                        string orgName = POName(org);
                        CsvPersons csvPers = csvPersons.Find(c => c.Organization == orgName);
                        if (csvPers == null)
                        {
                            string noRegionOrgName = Regex.Replace(orgName, @"^\d\d_", "");
                            csvPers = csvPersons.Find(c => c.Organization == noRegionOrgName);
                            msgLog += $"    Команда не найдена. Поиск команды без региона - {noRegionOrgName}";
                        }
                        if (csvPers != null)
                        {
                            if (renameOrgs)
                            {
                                org["name"] = $"{csvPers.Kod}_{orgName}";
                                msgLog += $"    Переиминована команда - {org["name"]}";
                            }
                            org["code"] = csvPers.Kod.ToString();
                            msgLog += $"    Установлен код - {org["code"]}.";
                            org["region"] = csvPers.Region;
                            msgLog += $"    Установлен код - {org["region"]}.\n";

                            complited++;
                        }
                        else
                        {
                            msgLog += $"    В CSV не нашлось команды - {orgName}...\n";
                        }
                    }
                    catch (Exception ex)
                    {
                        msgLog += $"\n  ERROR: {ex.Message} i:{i}";
                    }
                }
                msgLog += $"  Переиминование завершено. Переиминовано {complited}/{orgs.Count}...\n";
            }
            catch (Exception ex)
            {
                LogError("h021hdf7gsw", ex);
                msgLog += $"\n  ERROR: {ex.Message}";
            }
            return msgLog;
        }

        public static List<CsvPersons> ParseCsvUTF8(string csvStr)
        {
            using var reader = new StringReader(csvStr);

            // 1) Заголовок
            var headerLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(headerLine))
                return new List<CsvPersons>();

            var headers = headerLine.Split(';');
            // Словарь: имя колонки (trim + ignore case) -> её индекс
            var idx = headers
                .Select((h, i) => (Name: h.Trim(), Index: i))
                .ToDictionary(x => x.Name, x => x.Index, StringComparer.OrdinalIgnoreCase);

            // 2) Читаем все оставшиеся строки через fastCSV
            //    fastCSV.ReadStream читает с текущей позиции reader
            var list = fastCSV.ReadStream<CsvPersons>(reader, ';', (o, c) =>
            {
                // Вспомог local-функция для безопасного доступа
                string G(string name) =>
                    idx.TryGetValue(name, out var i) && i < c.Count
                        ? c[i]
                        : string.Empty;

                o.Group = G("Группа");
                o.Gender = G("Пол");
                o.LastName = G("Фамилия");
                o.MiddleName = G("Отчество");
                o.FirstName = G("Имя");
                o.Organization = G("Команда");
                o.Kod = int.TryParse(G("Код"), out var k) ? k : 0;
                o.Region = G("Регион");
                o.SfrKval = G("Квал. SFR");
                o.Kval = G("Квал.");
                // Если нужно Year как DOB или др. дата — можно парсить DateTime
                // Здесь пример int
                o.Year = int.TryParse(G("Дата рожд."), out var y) ? y : 0;
                o.CardNumber = G("№ чипа");
                o.Description = G("Примечания");
                o.SubmittedBy = G("Кем подана");
                o.Phone = G("Телефон");
                o.Email = G("E-mail");
                o.ApplicationNumber = G("Номер заявки");
                o.SubmissionTime = G("Время подачи");
                o.FromUrl = G("Вошел по ссылке");

                return true;
            });

            return list.ToList();
        }
    }

    public class CsvPersons
    {
        public string Group;
        public string Gender;
        public string LastName;
        public string FirstName;
        public string MiddleName;
        public string Organization;
        public int Kod;
        public string Region;
        public string SfrKval;
        public string Kval;
        public int Year;
        public string CardNumber;
        public string Description;
        public string SubmittedBy;
        public string Phone;
        public string Email;
        public string ApplicationNumber;
        public string SubmissionTime;
        public string FromUrl;
    }
}
