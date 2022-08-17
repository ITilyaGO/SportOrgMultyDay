using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParseBase
    {
        public static int CurrentRaceID(JToken jBase)
        {
            try
            {
                return (int)jBase["current_race"];
            }
            catch (Exception ex) { LogError("i273rv6tc", ex); }
            return -1;
        }
        public static JArray Races(JToken jBase)
        {
            try
            {
                return (JArray)jBase["races"];
            }
            catch (Exception ex) { LogError("s8d6vc3", ex); }
            return null;
        }
        public static JToken CurrentRace(JArray races,int id)
        {
            try
            {
                return races[id];
            }
            catch (Exception ex) { LogError("sa86dv3g", ex); }
            return null;
        }
        public static JToken CurrentRaceFromBase(JToken jBase)
        {
            try
            {
                JArray races = Races(jBase);
                return races[CurrentRaceID(jBase)];
            }
            catch (Exception ex) { LogError("fug237gb", ex);}
            return null;
        }
        public static JArray Persons(JToken race)
        {
            try
            {
                return (JArray)race["persons"];
            }
            catch (Exception ex) { LogError("976dvc2", ex); }
            return null;
        }
        public static JArray PersonsFromBase(JToken jBase)
        {
            try
            {
                JToken race = CurrentRaceFromBase(jBase);
                return (JArray)race["persons"];
            }
            catch (Exception ex) { LogError("976dvc2", ex); }
            return null;
        }
        public static JArray Groups(JToken race)
        {
            try
            {
                return (JArray)race["groups"];
            }
            catch (Exception ex) { LogError("d7gv33vad", ex); }
            return null;
        }
        public static JArray Organizations(JToken race)
        {
            try
            {
                return (JArray)race["organizations"];
            }
            catch (Exception ex) { LogError("oo86vsds8gc", ex); }
            return null;
        }
        public static JArray Results(JToken race)
        {
            try
            {
                return (JArray)race["results"];
            }
            catch (Exception ex) { LogError("7ugfi213djas", ex); }
            return null;
        }
        public static JArray ResultsFromBase(JToken jBase)
        {
            try
            {
                return (JArray)CurrentRaceFromBase(jBase)["results"];
            }
            catch (Exception ex) { LogError("i98g787bas", ex); }
            return null;
        }
        public static JToken Data(JToken race)
        {
            try
            {
                return race["data"];
            }
            catch (Exception ex) { LogError("l3iusv6clgf", ex); }
            return null;
        }
    }
}
