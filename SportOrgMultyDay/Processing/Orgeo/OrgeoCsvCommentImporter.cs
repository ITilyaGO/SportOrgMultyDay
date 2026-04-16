using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.Orgeo
{
    public class OrgeoCsvCommentImporter
    {
        public string log;
        public List<OrgeoCsvParticipantRow> ParsedRows { get; private set; } = [];

        public OrgeoCsvCommentImporter(string csvContent)
        {
            ParseDataFromCsv(csvContent);
        }

        public void InsertToComments(JToken race, string csvContent)
        {
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            //Dictionary<string, JToken> groupMap = DictGNameGroup(groups);
            Dictionary<string, string> groupIdNames = DictGIdGroupName(groups);

            Dictionary<string, OrgeoCsvParticipantRow> commentMap = new();
            foreach (OrgeoCsvParticipantRow row in ParsedRows)
            {
                string groupName = row.Group;
                string year = row.BirthYear.HasValue ? row.BirthYear.Value.ToString() : string.Empty;
                string fullName = $"{row.LastName} {row.FirstName} {row.MiddleName}".Trim();
                string fingerprintKey = $"{fullName}_{groupName}_{year}";
                if (!commentMap.ContainsKey(fingerprintKey))
                    commentMap[fingerprintKey] = row;
                else
                    log += $"  Внимание: Дубликат для {fullName} (Группа: {groupName}, Год: {year}) - строка с комментарием '{row.Comment}'\n";
            }
            int updatedCount = 0;
            foreach (var person in persons)
            {
                string fullName = $"{PPSurname(person)} {PPName(person)} {PPMiddleName(person)}".Trim();
                string groupId = PPGroupId(person);
                string birthYear = PPYear(person).ToString();
                string groupName = groupIdNames.TryGetValue(groupId, out string name) ? name : string.Empty;
                string fingerprintKey = $"{fullName}_{groupName}_{birthYear}";

                if (commentMap.TryGetValue(fingerprintKey, out OrgeoCsvParticipantRow matchingRow))
                {
                    string newComment = matchingRow.Comment;
                    person["comment"] = newComment;
                    log += $"  Участник {PPGetFingerPrint(person)} - комментарий обновлен: {newComment}\n";
                    updatedCount++;
                }
            }
            log += $"  Готово. \n\n\n\nОбновлено комментариев: {updatedCount}. \n\n\n\nВсего участников: {persons.Count}. \n\n\n\nВсего строк в CSV: {ParsedRows.Count}\n";
        }

        public string ParseDataFromCsv(string csvContent)
        {
            log += "Импорт комментариев из CSV...\n";
            ParsedRows.Clear();

            if (string.IsNullOrWhiteSpace(csvContent))
            {
                log += "  CSV пустой.\n";
                return log;
            }

            string[] lines = csvContent
                .Split(["\r\n", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries);

            int lineNumber = 0;

            foreach (string rawLine in lines)
            {
                lineNumber++;
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    OrgeoCsvParticipantRow row = ParseLine(line);
                    ParsedRows.Add(row);

                    //log += $"Строка {lineNumber}: {row.FullName} ({row.Group}) - комментарий: {row.Comment}\n";
                }
                catch (Exception ex)
                {
                    log += $"  Ошибка в строке {lineNumber}: {ex.Message}\n";
                }
            }

            log += $"  Готово. Загружено строк: {ParsedRows.Count}\n";
            return log;
        }

        private OrgeoCsvParticipantRow ParseLine(string line)
        {
            string[] parts = line.Split(';');

            OrgeoCsvParticipantRow row = new()
            {
                RawColumns = parts.ToList(),
                Group = GetValue(parts, 0),
                FullName = GetValue(parts, 1),
                Team = GetValue(parts, 2),
                Representative = GetValue(parts, 3),
                ChipNumberRaw = GetValue(parts, 4),
                BirthYearRaw = GetValue(parts, 6),
                Comment = GetValue(parts, 8)
            };

            // --- Парсим ФИО ---
            ParseFio(row);

            if (int.TryParse(row.ChipNumberRaw, out int chipNumber))
                row.ChipNumber = chipNumber;

            if (int.TryParse(row.BirthYearRaw, out int birthYear))
                row.BirthYear = birthYear;

            return row;
        }

        private void ParseFio(OrgeoCsvParticipantRow row)
        {
            if (string.IsNullOrWhiteSpace(row.FullName))
                return;

            var parts = row.FullName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                row.LastName = parts[0];
            }
            else if (parts.Length == 2)
            {
                row.LastName = parts[0];
                row.FirstName = parts[1];
            }
            else if (parts.Length >= 3)
            {
                row.LastName = parts[0];
                row.FirstName = parts[1];
                row.MiddleName = parts[2];
            }
        }

        public string GetLog(bool clear = true)
        {
            string currentLog = log;
            if (clear)
                log = string.Empty;
            return currentLog;
        }

        private string GetValue(string[] parts, int index)
        {
            if (index < 0 || index >= parts.Length)
                return string.Empty;

            return parts[index].Trim();
        }
    }

    public class OrgeoCsvParticipantRow
    {
        public string Group { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;

        public string Team { get; set; } = string.Empty;
        public string Representative { get; set; } = string.Empty;

        public string ChipNumberRaw { get; set; } = string.Empty;
        public int? ChipNumber { get; set; }

        public string BirthYearRaw { get; set; } = string.Empty;
        public int? BirthYear { get; set; }

        public string Comment { get; set; } = string.Empty;

        public List<string> RawColumns { get; set; } = [];
    }
}