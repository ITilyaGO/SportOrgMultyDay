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
using static SportOrgMultyDay.Data.QualificationNames;
using static SportOrgMultyDay.Processing.Logger;

using SportOrgMultyDay.Data;
using Microsoft.VisualBasic.Logging;

namespace SportOrgMultyDay.Processing.Parsing.Things
{
    public static class YarfsoParser
    {
        public static string ImportFromYarfso(JToken jBase, string inputFile,
            bool payAmountToComment = false,
            bool payAmountToWorldCode = true,
            bool replaceQual = false,
            bool writeOldQual = false)
        {
            string msgLog = "Установка квалификаций и статусов оплаты из списка Yarfso...\n";

            msgLog += "  Установка квалификаций с Yarfso - " + (replaceQual ? "Да" : "Нет") + "\n";
            msgLog += "  Запомнить старую квалификацию - " + (writeOldQual ? "Да" : "Нет") + "\n";
            msgLog += "  Оплата в Межд. Код - " + (payAmountToWorldCode ? "Да" : "Нет") + "\n";
            msgLog += "  Сумма оплаты в комментарий - " + (payAmountToComment ? "Да" : "Нет") + "\n";

            try
            {
                Dictionary<string, YarfsoPerson> yarPersons = ParseYarfsoFile(inputFile, out string log);
                msgLog += log;
                if (yarPersons == null) return msgLog;


                JArray races = PBRaces(jBase);
                for (int i = 0; i < races.Count; i++)
                {
                    JToken race = races[i];

                    msgLog += $"  Обработка дня: {i}\n";
                    JArray persons = PBPersons(race);
                    int okCount = 0, errCount = 0;
                    foreach (JToken person in persons)
                    {
                        string fullName = $"{PPSurname(person)} {PPName(person)} {PPYear(person)}";
                        if (yarPersons.TryGetValue(YarfsoPerson.PrepareString(fullName), out YarfsoPerson yarPerson))
                        {
                            if (writeOldQual)
                                person["comment"] += "OQ: " + person["qual"];

                            if (payAmountToComment || payAmountToWorldCode)
                                person["is_paid"] = yarPerson.IsPaid;
                            if (replaceQual)
                                person["qual"] = yarPerson.QualId;
                            if (payAmountToComment)
                                person["comment"] += "Сумма: " + (int)yarPerson.PayAmount;
                            if (payAmountToWorldCode)
                                person["world_code"] = (int)yarPerson.PayAmount;
                            okCount++;
                        }
                        else
                        {
                            person["qual"] = 0;
                            msgLog += $"    Участник не найден: {PPToString(person)}\n";
                            errCount++;
                        }
                    }
                    msgLog += $"    Завершено. Обработано {okCount}/{persons.Count} участников. Не найден: {errCount}\n";
                }
                msgLog += $"  Все дни обработаны\n";

            }
            catch (Exception ex)
            {
                LogError("84jf8fwelk", ex);
                msgLog += "\nERROR SetQualFromYarfso() вызвало ошибку\n";
            }
            return msgLog;
        }

        static Dictionary<string, YarfsoPerson> ParseYarfsoFile(string inputFile, out string log)
        {
            log = "  Парсинг файла...\n";
            try
            {
                Dictionary<string, YarfsoPerson> yarPersons = new();
                string[] inputStrings = inputFile.Split('\n');
                log += $"    Найдено строк: {inputStrings.Length}\n";

                for (int i = 1; i < inputStrings.Length; i++)
                {
                    string inputString = inputStrings[i];
                    if (inputString == null || inputString.Length == 0)
                        log += $"      Строка {i} пуста\n";
                    try
                    {
                        YarfsoPerson yarPerson = new YarfsoPerson(inputString);
                        yarPersons.Add(yarPerson.ToPrepareString(), yarPerson);
                    }
                    catch (Exception ex)
                    {
                        log += $"      Ошибка {ex} Строка:{i} Содержание:[{inputString}]\n";
                    }
                }
                log += $"    Успешно распознаны: {yarPersons.Count}\n";
                return yarPersons;
            }
            catch (Exception ex)
            {
                LogError("j9kd7d23js", ex);
                log += "\nERROR ParseYarfsoFile() вызвало ошибку\n";
            }
            return null;
        }

    }

    internal class YarfsoPerson
    {
        public string FullName { get; set; }
        public int Year { get; set; }
        public string Team { get; set; }
        public string Qual { get; set; }
        public int QualId => DictStringToId[Qual];
        public bool IsPaid { get; set; }
        public double PayAmount { get; set; }

        public YarfsoPerson(string dataString)
        {
            Dictionary<string, string> parsedData = ParseString(dataString);
            FullName = parsedData["fullName"];
            Year = int.Parse(parsedData["year"]);
            Team = parsedData["team"];
            Qual = parsedData["qual"];
            IsPaid = parsedData["isPaid"].Contains("ОПЛАЧЕНО");
            PayAmount = double.Parse(parsedData["amount"].Replace('.', ',')); ;
        }

        private static Dictionary<string, string> ParseString(string input)
        {
            string[] data = input.Split(';');

            //Dictionary<string, string> parsedData = new Dictionary<string, string>()
            //{
            //    { "fullName", data[0] },
            //    { "gender", data[1] },
            //    { "year", data[2] },
            //    { "team", data[3] },
            //    { "qual", data[4] },
            //    { "med", data[5] },
            //    { "group", data[6] },
            //    { "days", data[7] },
            //    { "request", data[8] },
            //    { "amount", data[9] },
            //    { "isPaid", data[10] }
            //};

            Dictionary<string, string> parsedData = new Dictionary<string, string>()
            {
                { "fullName", data[0] },
                { "amount", data[1] },
                { "isPaid", data[2] },
                { "team", data[3] },
                { "year", data[4] },
                { "group", data[5] },
                { "days", data[6] },
                { "qual", data[7] },
                { "med", data[8] }
            };


            return parsedData;
        }

        public override string ToString()
        {
            return FullName + " " + Year;
        }
        public string ToPrepareString()
        {
            return PrepareString(ToString());
        }
        public static string PrepareString(string str)
        {
            return str.ToLower().Replace('ё', 'е').Replace(" ", "");
        }
    }
}
