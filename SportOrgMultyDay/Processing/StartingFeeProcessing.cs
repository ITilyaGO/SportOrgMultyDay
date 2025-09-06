using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing
{
    public class StartingFeeProcessing
    {
        JToken jBase;
        JArray races;
        readonly string rawStartingFee;
        Dictionary<string,int> groupName_Price = new Dictionary<string, int>();
        public StartingFeeProcessing(JToken jBase, string rawStartingFee) 
        {
            this.rawStartingFee = rawStartingFee;
            this.jBase = jBase;
            races = PBRaces(jBase);
            ParseRawPrices();
        }

        public void SetStartingFeeFromGroupToPersons()
        {
            for (int i = 0; i < races.Count; i++)
            {
                JToken race = races[i];
                JArray persons = PBPersons(race);

                JArray groups = PBGroups(race);
                Dictionary<string, JToken> groupName_Group = [];
                Dictionary<string, JToken> gId_group = DictGIdGroup(groups);
                foreach (JToken person in persons)
                {
                    if (string.IsNullOrEmpty(PPWorldCode(person)))
                    {
                        if (PPWorldCode(person).Length != 0)
                            MessageBox.Show("Пробелма перезапись цены с сайта!");
                        string gId = PPGroupId(person);
                        JToken group = gId_group[gId];
                        person["world_code"] = PGPrice(group);
                    }
                }
            }
        }

        public void SetStartingFeeForGroups()
        {
            for (int i = 0; i < races.Count; i++)
            {
                JToken race = races[i];
                JArray groups = PBGroups(race);
                Dictionary<string, JToken> groupName_Group = [];
                foreach (JToken group in groups)
                {
                    string name = PGName(group);
                    group["price"] = groupName_Price[name];
                    //groupName_Group.Add(name, group);
                }
            }
        }

        private void ParseRawPrices()
        {
            groupName_Price.Clear();
            if (string.IsNullOrWhiteSpace(rawStartingFee)) return;

            string[] lines = rawStartingFee.Replace("\r", "").Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('=');
                if (parts.Length != 2) continue;

                string key = parts[0].Trim();
                string val = parts[1].Trim();

                int price;
                if (int.TryParse(val, out price))
                {
                    groupName_Price[key] = price;
                }
            }
        }


    }
}
