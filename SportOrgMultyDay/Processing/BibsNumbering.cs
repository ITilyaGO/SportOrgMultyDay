using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using static SportOrgMultyDay.Processing.Logger;
using Microsoft.VisualBasic.Logging;
using System.Linq.Expressions;

namespace SportOrgMultyDay.Processing
{
    public class BibsNumbering
    {
        public static string SetNumbers(JToken jRace, string bibsSample, bool debug = false)
        {
            bool checkNumberExist = true;
            string log = "Установка номеров участникам групп...\n";
            JArray persons = PBPersons(jRace);
            JArray groups = PBGroups(jRace);

            List<NumbersOfGroup> numbersOfGroups = ParseBibsSample(bibsSample, ref log);
            if (numbersOfGroups == null)
                return log;

            List<int> personBibs = new List<int>();
            foreach (JToken person in persons)
            {
                int bib = PPBib(person);
                if (bib == -1) continue;
                personBibs.Add(bib);
            }

            log += $"  Установка номеров для групп...\n";
            foreach (NumbersOfGroup numberOfGroup in numbersOfGroups)
            {
                JToken group = FGByName(numberOfGroup.GroupName, groups);
                if (group == null)
                {
                    log += $"  Группа [{numberOfGroup}] не найдена\n";
                    continue;
                }
                string groupId = PGId(group);
                List<JToken> groupPersons = FPAllByGroup(groupId, persons);
                if (groupPersons == null)
                {
                    log += $"  Ошибка при поиске участников группы {numberOfGroup.GroupName} ID:{groupId}\n";
                    continue;
                }
                if (groupPersons.Count > numberOfGroup.NumbersCount)
                {
                    string msg = $" В группе {numberOfGroup.GroupName} недостаточно номеров. Участников: {groupPersons.Count} Номеров: {numberOfGroup.NumbersCount}";
                    log += msg + "\n";
                    if (MessageBox.Show(msg + "Пропустить группу и продолжить?", "Недостаточно номеров", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        continue;
                    else
                        return log;
                }
                //log += $"  Установка номеров для группы {numberOfGroup.GroupName} Участники:{groupPersons.Count} Доступно номеров:{numberOfGroup.NumbersCount} ID:{groupId}\n";


                int currentNumber = numberOfGroup.StartBib;
                foreach (JToken person in groupPersons)
                {
                    if (personBibs.Contains(currentNumber))
                    {
                        string msg = $"   Номер {currentNumber} уже существует.\n";
                        if (checkNumberExist)
                        {
                            switch (MessageBox.Show(msg + "Все равно установить номер и спрашивать дальше?\n Отмена - Остановка установки номеров\n Да - Установить и спрашивать\n Нет - Установить не спрашивать ", "Номер существует", MessageBoxButtons.YesNoCancel))
                            {
                                case DialogResult.Cancel:
                                    return log;
                                case DialogResult.No:
                                    checkNumberExist = false;
                                    break;
                            }
                        }
                        log += msg;
                    }
                    int beforBib = PPBib(person);
                    person["bib"] = currentNumber;
                    if (debug)
                        log += $"      Установка номера. Номер до:{beforBib} Установлено: {PersonToString.BibName(person)}\n";
                    currentNumber++;
                }
                log += $"    Номера установлены с {numberOfGroup.StartBib} по {currentNumber-1}. {numberOfGroup}\n";
            }
            log += $"  Установка номеров завершена!\n";

            return log;
        }

        internal static List<NumbersOfGroup> ParseBibsSample(string sample, ref string log)
        {
            log += $"  Парсинг списка номеров групп...\n";
            string[] groupSamples = sample.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            List<NumbersOfGroup> numbersOfGroups = new List<NumbersOfGroup>();
            
            for (int i = 0; i < groupSamples.Length; i++)
            {
                string groupSample = groupSamples[i];
                NumbersOfGroup numbersOfGroup = ParseGroupSample(groupSample);
                if (numbersOfGroup == null)
                {
                    log += $"    Парсинг строки {i} [{groupSample}] не удался\n";
                    log += "  Установка номеров прервана на шаге парсинга списка номеров. Проверьте правильность написания списка.\n";
                    return null;
                }
                else
                {
                    numbersOfGroup.StringNumber = i;
                    log += $"    Парсинг удался: {numbersOfGroup}\n";
                    numbersOfGroups.Add(numbersOfGroup);
                }
            }
            return numbersOfGroups;
        }

        internal static NumbersOfGroup ParseGroupSample(string groupSample)
        {
            string[] parts = groupSample.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return null;
            string groupName = parts[0];
            string numbersRange = parts[1];
            string[] rangeParts = numbersRange.Split('-');
            if (int.TryParse(rangeParts[0], out int startBib) && int.TryParse(rangeParts[1], out int endBib))
                return new NumbersOfGroup(groupName, startBib, endBib);
            else
                return null;
        }
    }


    internal class NumbersOfGroup
    {
        public int StringNumber { get; set; }
        public int StartBib { get; set; }
        public int EndBib { get; set; }
        public string GroupName { get; set; }
        public int NumbersCount => (EndBib - StartBib) + 1;

        public NumbersOfGroup(string group, int startBib = 0, int endBib = 0, int stringNumber = 0)
        {
            GroupName = group;
            StartBib = startBib;
            EndBib = endBib;
            StringNumber = stringNumber;
        }

        public override string ToString()
        {
            return $"Строка: {StringNumber+1} Имя: {GroupName} Номера: {StartBib}-{EndBib}";
        }
    }
}
