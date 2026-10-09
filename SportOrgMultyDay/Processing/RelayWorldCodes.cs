using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;

namespace SportOrgMultyDay.Processing
{
    /// <summary>
    /// Обратная операция к SetRelayNumbers: по эстафетным номерам (этап + номер команды) восстанавливает
    /// world_code вида "ИдЗаявки-НомерЭтапа", объединяя участников одной команды в одну заявку.
    /// </summary>
    public class RelayWorldCodes
    {
        public const int NewApplicationStartId = 9000001;
        private const int RelayGroupType = 3;

        public static string SetFromBibs(JToken jRace, bool isDebug = false, int bibNormalization = 3)
        {
            string log = "Расстановка world_code эстафетных групп по номерам...\n";
            JArray persons = PBPersons(jRace);
            JArray groups = PBGroups(jRace);
            int divisor = (int)Math.Pow(10, bibNormalization);

            // Участники, которые будут обработаны: группа типа 3, номер содержит этап и номер команды
            HashSet<JToken> processed = new();
            List<(JToken group, List<IGrouping<int, JToken>> teams)> groupTeams = new();
            foreach (JToken group in groups)
            {
                if (PGType(group) != RelayGroupType)
                    continue;
                string groupId = PGId(group);
                List<JToken> groupPersons = FPAllByGroup(groupId, persons);
                if (groupPersons == null)
                {
                    log += $"  Ошибка при поиске участников группы {PGName(group)} ID:{groupId}\n";
                    continue;
                }

                List<JToken> withBib = new();
                foreach (JToken person in groupPersons)
                {
                    int bib = PPBib(person);
                    if (bib < divisor)
                    {
                        log += $"  Пропущен (нет эстафетного номера): {PersonToString.BibName(person)}\n";
                        continue;
                    }
                    withBib.Add(person);
                    processed.Add(person);
                }
                var teams = withBib.GroupBy(p => PPBib(p) % divisor).OrderBy(g => g.Key).ToList();
                groupTeams.Add((group, teams));
            }

            // Заявки, которыми пользуются необработанные участники, трогать нельзя
            HashSet<string> reservedApps = new();
            // Все известные заявки - чтобы новый номер не пересекся ни с чем
            HashSet<string> usedApps = new();
            foreach (JToken person in persons)
            {
                string app = new PersonRelayData(person, out _).ApplicationId;
                if (app == null) continue;
                usedApps.Add(app);
                if (!processed.Contains(person))
                    reservedApps.Add(app);
            }

            int nextNewApp = NewApplicationStartId;
            HashSet<string> claimedApps = new();
            int changed = 0, unchanged = 0, newApps = 0;

            foreach (var (group, teams) in groupTeams)
            {
                string groupName = PGName(group);
                foreach (IGrouping<int, JToken> team in teams)
                {
                    // Заявка команды - самая частая среди участников, у кого этап в world_code уже совпадает с номером
                    string app = team
                        .Select(p => new { Rd = new PersonRelayData(p, out _), Stage = PPBib(p) / divisor })
                        .Where(x => x.Rd.ApplicationId != null && x.Rd.Stage == x.Stage)
                        .GroupBy(x => x.Rd.ApplicationId)
                        .OrderByDescending(g => g.Count())
                        .Select(g => g.Key)
                        .FirstOrDefault(a => !reservedApps.Contains(a) && !claimedApps.Contains(a));

                    if (app == null)
                    {
                        while (usedApps.Contains(nextNewApp.ToString()))
                            nextNewApp++;
                        app = nextNewApp.ToString();
                        usedApps.Add(app);
                        newApps++;
                        if (isDebug)
                            log += $"  {groupName} команда {team.Key}: новая заявка {app}\n";
                    }
                    claimedApps.Add(app);

                    foreach (JToken person in team)
                    {
                        int stage = PPBib(person) / divisor;
                        PersonRelayData rd = new(person, out _);
                        if (rd.ApplicationId == app && rd.Stage == stage)
                        {
                            unchanged++;
                            continue;
                        }
                        string before = PPWorldCode(person);
                        string after = $"{app}-{stage:D4}";
                        person["world_code"] = after;
                        changed++;
                        if (isDebug)
                            log += $"    {PersonToString.BibName(person)}: world_code [{before}] -> [{after}]\n";
                    }
                }
            }

            log += $"  Готово. Изменено: {changed}, без изменений: {unchanged}, создано новых заявок: {newApps}.\n";
            return log;
        }
    }
}
