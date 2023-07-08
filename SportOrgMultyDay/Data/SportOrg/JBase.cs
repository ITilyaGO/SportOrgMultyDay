using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Data.SportOrg
{
    public class JBase
    {
        public JToken jBase { get; set; }
        public JBase(JToken jBase) => this.jBase = jBase;
        public int current_race
        {
            get
            {
                try
                {
                    return (int)jBase["current_race"];
                }
                catch (Exception ex) { LogError("i273rv6tc", ex); }
                return 0;
            }
        }
        //public JRaces Races
        //{
        //    get
        //    {
        //        try
        //        {
        //            return new JRaces((JArray)jBase["races"]);
        //        }
        //        catch (Exception ex) { LogError("s8d6vc3", ex); }
        //        return null;
        //    }
        //}

        public List<JRace> Races
        {
            get
            {
                try
                {
                    List<JRace> raceList = new List<JRace>();
                    jBase["races"].ToList().ForEach(race => raceList.Add(new JRace(race)));
                    return raceList;
                }
                catch (Exception ex) { LogError("s8d6vc3", ex); }
                return null;
            }
        }

        public JArray races
        {
            get
            {
                return (JArray)jBase["races"];
            }
        }
        //public static JToken selected_race(JToken jBase)
        //{
        //    try
        //    {
        //        JArray races = PBRaces(jBase);
        //        return races[CurrentRaceID(jBase)];
        //    }
        //    catch (Exception ex) { LogError("fug237gb", ex); }
        //    return null;
        //}
        //public static JArray PBPersons(JToken race)
        //{
        //    try
        //    {
        //        return (JArray)race["persons"];
        //    }
        //    catch (Exception ex) { LogError("976dvc2", ex); }
        //    return null;
        //}
        //public static JArray PBPersonsFromBase(JToken jBase)
        //{
        //    try
        //    {
        //        JToken race = PBCurrentRaceFromBase(jBase);
        //        return (JArray)race["persons"];
        //    }
        //    catch (Exception ex) { LogError("976dvc2", ex); }
        //    return null;
        //}
        //public static JArray PBGroups(JToken race)
        //{
        //    try
        //    {
        //        return (JArray)race["groups"];
        //    }
        //    catch (Exception ex) { LogError("d7gv33vad", ex); }
        //    return null;
        //}
        //public static JArray PBOrganizations(JToken race)
        //{
        //    try
        //    {
        //        return (JArray)race["organizations"];
        //    }
        //    catch (Exception ex) { LogError("oo86vsds8gc", ex); }
        //    return null;
        //}
        //public static JArray PBResults(JToken race)
        //{
        //    try
        //    {
        //        return (JArray)race["results"];
        //    }
        //    catch (Exception ex) { LogError("7ugfi213djas", ex); }
        //    return null;
        //}
        //public static JArray PBResultsFromBase(JToken jBase)
        //{
        //    try
        //    {
        //        return (JArray)PBCurrentRaceFromBase(jBase)["results"];
        //    }
        //    catch (Exception ex) { LogError("i98g787bas", ex); }
        //    return null;
        //}
        //public static JToken PBData(JToken race)
        //{
        //    try
        //    {
        //        return race["data"];
        //    }
        //    catch (Exception ex) { LogError("l3iusv6clgf", ex); }
        //    return null;
        //}
    }
}
