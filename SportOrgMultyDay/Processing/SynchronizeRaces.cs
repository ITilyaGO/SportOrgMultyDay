using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing
{
    public class PersonCopies
    {
        public PersonCopies(JToken person, int raceIndex)
        {
            Person = person;
            RaceIndex = raceIndex;
        }
        public override string ToString()
        {
            return $"День:{RaceIndex+1} [{PersonToString.BibName(Person)}]";
        }
        public static string ToString(PersonCopies[] personCopies)
        {
            string outs = "";
            foreach (PersonCopies person in personCopies)
            {
                outs += $"{person}\n";
            }
            return outs;
        }
        public JToken Person { get; set; }
        public int RaceIndex { get; set; }
    }

    public class SynchronizeRaces
    {
        public static string SynchronizeReservWithCurrentRace(JToken jBase,string reservSurname,string[] syncFields,bool ignoreChangeOtherDay = false)
        {
            int copyCount = 0;
            string msgLog = "Синхронизация...\n";
            int currentRaceI = (int)jBase["current_race"];
            msgLog += $"Текущий день: {currentRaceI+1}\n";
            JArray races = (JArray)jBase["races"];
            JToken curRace = races[currentRaceI];
            JArray persons = (JArray)curRace["persons"];

            for (int i = 0; i < persons.Count; i++)
            {
                JToken person = persons[i];
                int bib = (int)person["bib"];
                PersonCopies[] personCopies = GetPersonTockensByBib(races, bib);
                int reservCount = ReservCountByPersonCopies(personCopies, reservSurname);
                if (reservCount == races.Count || reservCount == 0) continue;
                else if (reservCount == races.Count - 1)
                {
                    int difPers = DifferentPersons(personCopies, currentRaceI);
                    if (difPers == 0)
                    {
                        msgLog += PersonCopiesCopy(personCopies, currentRaceI, syncFields);
                        copyCount++;
                        continue;
                    }
                    msgLog += $"\n⚠Номер {bib} уже изменен в другом дне\n{PersonCopies.ToString(personCopies)}";
                    if (ignoreChangeOtherDay)
                    {
                        msgLog += PersonCopiesCopy(personCopies, currentRaceI, syncFields);
                        copyCount++;
                    }
                }
                else
                {
                    msgLog += $"\n⚠Найдено резервов:{reservCount} проверьте номер {bib} в разных днях\n{PersonCopies.ToString(personCopies)}\n";
                }

            }
            msgLog += $"Скопировано участников: {copyCount}";
            return msgLog;
        }

        public static int ReservCountByPersonCopies(PersonCopies[] personCopies,string reservSurname)
        {
            int count = 0;
            for (int i = 0; i < personCopies.Length; i++)
            {
                if (personCopies[i].Person is null) continue;
                if ((string)personCopies[i].Person["surname"] == reservSurname)
                {
                    count++;
                }
            }
            return count;
        }


        public static int DifferentPersons(PersonCopies[] personCopies,int skipIndex = -1)
        {
            int count = 0;
            for (int a = 0; a < personCopies.Length-1; a++)
            {
                for (int b = a + 1; b < personCopies.Length; b++)
                {
                    if(a==skipIndex||b==skipIndex) continue;
                    JToken personA = personCopies[a].Person;
                    JToken personB = personCopies[b].Person;
                    if (DifferentPerson(personA, personB)) count++;
                }
            }
            return count;
        }

        public static bool DifferentPerson(JToken personA,JToken peosonB)
        {
            string[] fields = { "surname", "name", "year" };
            foreach (string field in fields)
            {
                string a = (string)personA[field];
                string b = (string)peosonB[field];

                if (a!=b) return true;

            }    
            return false;
        }

        private static string PersonCopiesCopy(PersonCopies[] personCopies,int indexFromCopy,string[] syncFields)
        {
            JToken person = personCopies[indexFromCopy].Person;
            string msgLog = $"  Копирование - Из:{PersonToString.BibNameComment(person)} В:";
            for (int i = 0; i < personCopies.Length; i++)
            {
                if (indexFromCopy == i) continue;
                if (personCopies[i].Person == null)
                {
                    msgLog += $"⚠Участник в {i + 1} дне не найден {PersonToString.BibNameComment(person)}";
                    continue;
                }
                msgLog += $"[Д:{personCopies[i].RaceIndex+1} { PersonToString.BibName(personCopies[i].Person)}]";
                msgLog += CopyPerson(person, personCopies[i].Person,syncFields);
                ///if pc.index != p[r]

            }
            msgLog += $"\n";


            return msgLog;


        }

        private static string CopyPerson(JToken from,JToken to, string[] syncFields)
        {
            string msgLog = "";
            try
            {
                foreach (string field in syncFields)
                {
                    to[field] = from[field];
                }
                msgLog += $"";
            }
            catch (Exception ex)
            {
                msgLog = ex.Message;
            }
            return msgLog;
        }
        private static PersonCopies[] GetPersonTockensByBib(JArray races, int bib)
        {
            PersonCopies[] pCopy = new PersonCopies[races.Count];
            for (int i = 0; i < races.Count; i++)
            {
                var race = races[i];
                JToken person = GetPersonByBib(race,bib);
                pCopy[i] = new(person, i);

            }
            return pCopy;
        }

        private static JToken GetPersonByBib(JToken race, int bib)
        {
            var persons = race["persons"];
            for (int i = 0; i < persons.Count(); i++)
            {
                int pbib = (int)persons[i]["bib"]; 
                if(pbib == bib)
                    return persons[i];
            }
            return null;
        }
        
    }
}
