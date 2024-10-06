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
using HtmlAgilityPack;
using SportOrgMultyDay.Data;

namespace SportOrgMultyDay.Processing
{
    public class BibsNumbering
    {

        public static string SetNumbers(JToken jRace, string bibsSample, bool isDebug = false, bool isRelay = false, bool isCreateReserv = false)
        {
            if (isRelay)
                return SetRelayNumbers(jRace, bibsSample, isDebug, isCreateReserv);
            else
                return SetDefaultNumbers(jRace, bibsSample, isDebug, isCreateReserv);
        }

        public static string SetRelayNumbers(JToken jRace, string bibsSample, bool isDebug = false, bool isCreateReserv = false, int stageCount = 2, int bibNormalization = 3)
        {
            bool checkNumberExist = true;
            string log = "Установка эстафетных номеров участникам групп...\n";
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

                List<PersonRelayData> personRelayDatas = new();
                foreach (JToken person in groupPersons)
                {
                    PersonRelayData personRD = new(person, out string llog);
                    personRelayDatas.Add(personRD);
                    log += llog;
                }

                IEnumerable<IGrouping<string, PersonRelayData>> personRelayGrouped = personRelayDatas.GroupBy(p => p.ApplicationId).OrderByDescending(g => g.Count()); ;

                if (personRelayGrouped.Count() > numberOfGroup.NumbersCount)
                {
                    string msg = $" В группе {numberOfGroup.GroupName} недостаточно номеров. Участников: {groupPersons.Count} Номеров: {numberOfGroup.NumbersCount}";
                    log += msg + "\n";
                    if (MessageBox.Show(msg + "Пропустить группу и продолжить?", "Недостаточно номеров", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        continue;
                    else
                        return log;
                }

                int currentNumber = numberOfGroup.StartBib;
                foreach (IGrouping<string, PersonRelayData> prGroup in personRelayGrouped)
                {
                    foreach (PersonRelayData personRelay in prGroup)
                    {
                        JToken person = personRelay.Person;
                        int beforBib = PPBib(person);
                        string strBib = personRelay.Stage.ToString() + StringNormalizer(currentNumber.ToString(), bibNormalization);
                        int relayBib = int.Parse(strBib);

                        if (personBibs.Contains(relayBib))
                        {
                            string msg = $"   Номер {relayBib} уже существует.";
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
                            log += msg + "Перезаписан.\n";
                        }
                        person["bib"] = relayBib;

                        if (isDebug)
                            log += $"      Установка номера. Номер до:{beforBib} Установлено: {PersonToString.BibName(person)}\n";
                    }
                    currentNumber++;
                }
                string reservCreateLog = string.Empty;
                if (isCreateReserv)
                {
                    int createdReservCount = 0;
                    if (isDebug)
                        log += $"      Создание резервов...\n";
                    while (currentNumber <= numberOfGroup.EndBib && createdReservCount < numberOfGroup.MaxReservCount)
                    {
                        for (int i = 1; i <= stageCount; i++)
                        {
                            string strBib = i.ToString() + StringNormalizer(currentNumber.ToString(), bibNormalization);
                            int relayBib = int.Parse(strBib);
                            JObject newReserv = PersonTemplate.Reserv;
                            newReserv["group_id"] = groupId;
                            newReserv["bib"] = relayBib;
                            persons.Add(newReserv);
                            if (isDebug)
                                log += $"        Создан - {PPToString(newReserv)}\n";
                        }
                        currentNumber++;
                        createdReservCount++;
                    }
                    reservCreateLog = $" Создано команд резервов: {createdReservCount}.";
                }
                log += $"    Номера установлены с {numberOfGroup.StartBib} по {currentNumber - 1}.{reservCreateLog} {numberOfGroup}\n";
            }
            log += $"  Установка номеров завершена!\n";

            return log;
        }

        public static string SetDefaultNumbers(JToken jRace, string bibsSample, bool isDebug = false, bool isCreateReserv = false)
        {
            bool checkNumberExist = true;
            string log = "Установка номеров участникам групп...\n";
            JArray persons = PBPersons(jRace);
            JArray groups = PBGroups(jRace);

            List<NumbersOfGroup> numbersOfGroups = ParseBibsSample(bibsSample, ref log);
            if (numbersOfGroups == null)
                return log;

            List<int> personBibs = new();
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

                    if (isDebug)
                        log += $"      Установка номера. Номер до:{beforBib} Установлено: {PersonToString.BibName(person)}\n";
                    currentNumber++;
                }

                string reservCreateLog = string.Empty;
                if (isCreateReserv)
                {
                    int createdReservCount = 0;
                    if (isDebug)
                        log += $"      Создание резервов. максимальное кол-во {numberOfGroup.MaxReservCount} ...\n";
                    while (currentNumber <= numberOfGroup.EndBib && createdReservCount < numberOfGroup.MaxReservCount)
                    {
                        JObject newReserv = PersonTemplate.Reserv;
                        newReserv["group_id"] = groupId;
                        newReserv["bib"] = currentNumber;
                        persons.Add(newReserv);
                        if (isDebug)
                            log += $"        Создан - {PPToString(newReserv)}\n";
                        currentNumber++;
                        createdReservCount++;
                    }
                    reservCreateLog = $" Создано резервов: {createdReservCount}.";
                }

                log += $"    Номера установлены с {numberOfGroup.StartBib} по {currentNumber - 1}.{reservCreateLog} {numberOfGroup}\n";
            }
            log += $"  Установка номеров завершена!\n";

            return log;
        }

        internal static List<NumbersOfGroup> ParseBibsSample(string sample, ref string log)
        {
            log += $"  Парсинг списка номеров групп...\n";
            string[] groupSamples = sample.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            List<NumbersOfGroup> numbersOfGroups = new();
            
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
                    numbersOfGroup.NumberOfString = i;
                    log += $"    Парсинг удался: {numbersOfGroup}\n";
                    numbersOfGroups.Add(numbersOfGroup);
                }
            }
            return numbersOfGroups;
        }

        internal static NumbersOfGroup ParseGroupSample(string groupSample)
        {
            if (string.IsNullOrWhiteSpace(groupSample))
                return null;

            var parts = groupSample.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return null;

            string groupName = parts[0];
            string numbersRange = parts[1];
            string rawReservCount = parts.ElementAtOrDefault(2);


            // Попробуем разобрать диапазон номеров
            if (!TryParseNumbersRange(numbersRange, out int startBib, out int endBib))
                return null;

            _ = TryParseReservCount(rawReservCount, out int reservCount);

            return new NumbersOfGroup(groupName, startBib, endBib, reservCount: reservCount);
        }

        private static bool TryParseReservCount(string rawReservCount, out int reservCount)
        {
            reservCount = 100;
            if (string.IsNullOrWhiteSpace(rawReservCount))
                return false;
            string[] parts = rawReservCount.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return false;
            if (parts[0] == "r")
                return int.TryParse(parts[1], out reservCount);
            return false;
        }

        private static bool TryParseNumbersRange(string numbersRange, out int startBib, out int endBib)
        {
            const int defaultEndBib = 9999999;
            endBib = defaultEndBib;

            // Разделим диапазон на части
            var rangeParts = numbersRange.Split('-');

            // Попытка разобрать начальный номер
            if (!int.TryParse(rangeParts[0], out startBib))
                return false; // Ошибка разбора, возвращаем false

            // Если есть второй элемент, попытаемся разобрать его, иначе присвоим дефолтный конец диапазона
            if (rangeParts.Length > 1 && !int.TryParse(rangeParts[1], out endBib))
                endBib = defaultEndBib;

            return true;
        }


        static string StringNormalizer(string input, int minLength, char filler = '0')
        {
            if (input.Length >= minLength)
            {
                return input;
            }

            int zerosNeeded = minLength - input.Length;
            string zeros = new(filler, zerosNeeded);

            return zeros + input;
        }
    }


    internal class NumbersOfGroup
    {
        public int NumberOfString { get; set; }
        public int StartBib { get; set; }
        public int EndBib { get; set; }
        public int MaxReservCount { get; set; }
        public string GroupName { get; set; }
        public int NumbersCount => (EndBib - StartBib) + 1;

        public NumbersOfGroup(string group, int startBib = 0, int endBib = 0, int numberOfString = 0, int reservCount = 100)
        {
            GroupName = group;
            StartBib = startBib;
            EndBib = endBib;
            NumberOfString = numberOfString;
            MaxReservCount = reservCount;
        }

        public override string ToString()
        {
            return $"Строка: {NumberOfString+1} Имя: {GroupName} Максимум резервов:{MaxReservCount} Номера: {StartBib}-{EndBib}";
        }
    }

    internal class PersonRelayData
    {
        public string WorldCode { get => personWorldCode; }
        public int Stage { get => personStage; }
        public string ApplicationId { get => personApplicationId; }
        public JToken Person { get; set; }


        private string personWorldCode;
        private string personApplicationId;
        private string personApplicationIndex;
        private int personStage;

        public PersonRelayData(JToken person, out string log)
        {
            log = "";
            Person = person;
            personWorldCode = PPWorldCode(person);

            if (personWorldCode == null)
            {
                log += $"    Ошибка. Меджунородный код (world_code) не найден - {PersonToString.BibName(person)}. Пропущено.\n";
                return;
            }

            string[] personOrgeoRaw = personWorldCode.Split('-');
            if (personOrgeoRaw.Length != 2)
            {
                log += $"    Ошибка. Меджунородный код не содержит корректного OrgeoId {PersonToString.BibName(person)}. Пропущено.\n";
                return;
            }
            personApplicationId = personOrgeoRaw[0];
            personApplicationIndex = personOrgeoRaw[1];

            if (int.TryParse(personApplicationIndex, out int pStage))
            {
                personStage = pStage;
            }
        }

        public PersonRelayData(JToken person, int stage)
        {
            Person = person;
            personStage = stage;
        }

    }
}
