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

namespace SportOrgMultyDay.Processing.Parsing.Things
{
    public static class OrgeoCsvParser
    {
        public static string AddRegionsKodToOrgs(JToken race, string csvStr)
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
                        if (csvPers != null)
                        {
                            org["name"] = $"{csvPers.Kod}_{orgName}";
                            org["region"] = csvPers.Kod.ToString();
                            msgLog += $"    Переиминована команда - {org["name"]}...\n";
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
            using StringReader reader = new StringReader(csvStr);
            return fastCSV.ReadStream<CsvPersons>(reader, ';', (o, c) =>
            {
                o.Group = c[0];
                o.Gender = c[1];
                o.LastName = c[2];
                o.FirstName = c[3];
                o.Organization = c[4];
                o.Kod = Convert.ToInt32(c[5]);
                o.Region = c[6];
                o.SfrKval = c[7];
                o.Kval = c[8];
                o.Year = Convert.ToInt32(c[9]);
                o.CardNumber = c[10];
                o.Description = c[11];
                o.SubmittedBy = c[12];
                o.Phone = c[13];
                o.Email = c[14];
                o.ApplicationNumber = c[15];
                o.SubmissionTime = c[16];
                o.FromUrl = c[17];
                return true;
            });
        }
    }

    public class CsvPersons
    {
        public string Group;
        public string Gender;
        public string LastName;
        public string FirstName;
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
