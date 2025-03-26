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
using SportOrgMultyDay.Processing.Parsing.Things;

namespace SportOrgMultyDay.Processing.SFR
{
    public static class SportOrgToSFR
    {
        public static SFRGeneral RaceToSFR(JToken race)
        {
            SFRGeneral sFRGeneral = new SFRGeneral();

            JToken data = PBData(race);
            sFRGeneral.event_name = PDTitle(data);
            string startDateTime = PDStartDatetime(data);
            startDateTime = startDateTime.Substring(0, startDateTime.IndexOf(' '));
            sFRGeneral.Parts.Add(1, new SFRPart(1) { Date = startDateTime, Name = $"Старт {startDateTime}"});

            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            JArray organizations = PBOrganizations(race);

            Dictionary<string, int> soToSfrPersons = [];
            Dictionary<string, int> soToSfrGroups = [];
            Dictionary<string, int> soToSfrOrganizations = [];

            for (int i = 0; i < groups.Count; i++)
            {
                JToken group = groups[i];
                string groupId = PGId(group);
                soToSfrGroups.Add(groupId, i);
                sFRGeneral.Groups.Add(i, GroupToSFR(i, group));
            }

            for (int i = 0; i < organizations.Count; i++)
            {
                JToken organization = organizations[i];
                string organizationId = POId(organization);
                soToSfrOrganizations.Add(organizationId, i);
                sFRGeneral.Teams.Add(i, TeamToSFR(i, organization));
            }

            for (int i = 0; i < persons.Count; i++)
            {
                JToken person = persons[i];
                string personId = PPId(person);
                string groupId = PPGroupId(person);
                string organizationId = PPOrganizationId(person);
                int groupIdSFR = soToSfrGroups[groupId];
                int organizationIdSFR = soToSfrOrganizations[organizationId];
                sFRGeneral.Persons.Add(i, PersonToSFR(i, groupIdSFR, organizationIdSFR, person, groups, organizations));
            }
            return sFRGeneral;
        }

        public static SFRPerson PersonToSFR(int id, int groupIdSFR, int organizationIdSFR, JToken person, JArray groups, JArray organizations)
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
