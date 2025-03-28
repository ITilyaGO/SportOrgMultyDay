using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportOrgMultyDay.Data.SFR;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using static SportOrgMultyDay.Processing.Logger;
using SportOrgMultyDay.Processing.Parsing.Things;

namespace SportOrgMultyDay.Processing.SFR
{
    public static class SportOrgToSFR
    {
        public static SFRGeneral RaceToSFR(JToken race, out string log)
        {
            log = "Конвертация SFRx...\n";
            SFRGeneral sFRGeneral = new();
            try
            {
                log += "  Сбор информации о базе...";
                JToken data = PBData(race);
                sFRGeneral.event_name = PDTitle(data);
                string startDateTime = PDStartDate(data);
                sFRGeneral.Parts.Add(1, new SFRPart(1) { Date = startDateTime, Name = $"Старт {startDateTime}" });
                log += "  OK\n";

                log += "  Получение групп, участников, команд...";
                JArray persons = PBPersons(race);
                JArray groups = PBGroups(race);
                JArray organizations = PBOrganizations(race);
                log += "  OK\n";

                Dictionary<string, int> soToSfrPersons = [];
                Dictionary<string, int> soToSfrGroups = [];
                Dictionary<string, int> soToSfrOrganizations = [];

                log += "  Конвертация групп...";
                for (int i = 0; i < groups.Count; i++)
                {
                    JToken group = groups[i];
                    string groupId = PGId(group);
                    soToSfrGroups.Add(groupId, i);
                    sFRGeneral.Groups.Add(i, GroupToSFR(i, group));
                }
                log += "  OK\n";

                log += "  Конвертация команд...";
                for (int i = 0; i < organizations.Count; i++)
                {
                    JToken organization = organizations[i];
                    string organizationId = POId(organization);
                    soToSfrOrganizations.Add(organizationId, i);
                    sFRGeneral.Teams.Add(i, TeamToSFR(i, organization));
                }
                log += "  OK\n";

                log += "  Конвертация участников...";
                for (int i = 0; i < persons.Count; i++)
                {
                    JToken person = persons[i];
                    string personId = PPId(person);
                    string groupId = PPGroupId(person);
                    string organizationId = PPOrganizationId(person);
                    int groupIdSFR = soToSfrGroups[groupId];
                    int organizationIdSFR = soToSfrOrganizations[organizationId];
                    SFRPerson sFRPerson = PersonToSFR(i, groupIdSFR, organizationIdSFR, person, groups, organizations);
                    if (sFRPerson == null)
                    {
                        log += $"\n  Ошибка конвертации участника: {PPToString(person)}\n";
                        continue;
                    }
                    sFRGeneral.Persons.Add(i, sFRPerson);
                }
                log += "  OK\n";
            }
            catch (Exception ex) { log += $"  Ошибка\n{ex.Message}"; LogError("wef23fds", ex); }
            log += "Конвертация завершена!\n\n";

            return sFRGeneral;
        }

        public static SFRPerson PersonToSFR(int id, int groupIdSFR, int organizationIdSFR, JToken person, JArray groups, JArray organizations)
        {
            try
            {
                SFRPerson sfrPerson = new(id)
                {
                    Bib = PPBib(person),
                    GroupId = groupIdSFR,
                    Surname = PPSurname(person),
                    Name = PPName(person),
                    OrganizationId = organizationIdSFR,
                    Year = PPYear(person),
                    Qualification = PPQual(person),
                    Comment = PPComment(person),
                    StartTime = StartTimeToString(PPStartTime(person))
                };
                return sfrPerson;
            }
            catch (Exception ex) { LogError("d2gsdf24tfwef", ex); }
            return null;
        }

        private static SFRGroup GroupToSFR(int id, JToken group) => new(id)
        {
            Name = PGName(group)
        };

        private static SFRTeam TeamToSFR(int id, JToken team) => new(id)
        {
            Name = POName(team)
        };
    }
}
