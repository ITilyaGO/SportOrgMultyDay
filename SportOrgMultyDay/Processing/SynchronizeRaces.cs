using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

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
        public static string ToString(PersonCopies[] personCopies,bool newString = true)
        {
            string outs = "";
            foreach (PersonCopies person in personCopies)
            {
                outs += person;
                if (newString) outs += "\n";
            }
            return outs;
        }
        public JToken Person { get; set; }
        public int RaceIndex { get; set; }
    }

    public class SynchronizeRaces
    {
        public static string SynchronizeReservWithCurrentRace(JToken jBase,string reservSurname,string[] syncFields,bool ignoreChangeOtherDay = false,bool copyAdded = false)
        {
            int copyCount = 0;
            int currentRaceI = CurrentRaceID(jBase);
            string msgLog = $"Синхронизация...\nТекущий день: {currentRaceI+1}\n";
            JArray races = Races(jBase);
            JArray persons = Persons(CurrentRace(races, currentRaceI));

            for (int i = 0; i < persons.Count; i++)
            {
                int bib = PPBib(persons[i]);
                if (copyAdded)
                {
                    
                }
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

        public static string CreateNewPersons(JToken jBase)
        {
            int createCount = 0;
            int currentRaceI = CurrentRaceID(jBase);
            string msgLog = $"Создание новых дозаявленых...\nТекущий день: {currentRaceI + 1}\n";

            JArray races = Races(jBase);
            JArray persons = Persons(CurrentRace(races, currentRaceI));
            for (int i = 0; i < persons.Count; i++)
            {
                JToken person = persons[i];
                if (!PPIsPersonal(person)) continue;
                msgLog += $" {CreateNewPerson(races, new PersonCopies(person, currentRaceI))}\n";
                createCount++;
            }
            msgLog += $"Скопировано участников: {createCount}";
            return msgLog;
        }

        //public static string CombineAllPersonsFromDays(JToken jBase)
        //{
        //    int createCount = 0;
        //    string msgLog = "Сasdda...\n";
        //    int currentRaceI = (int)jBase["current_race"];
        //    msgLog += $"Текущий день: {currentRaceI + 1}\n";
        //    JArray races = (JArray)jBase["races"];

        //    for (int r = 0; r < races.Count; r++)
        //    {
        //        if (r == currentRaceI) continue;
                
        //        JArray persons = (JArray)race["persons"];
        //        JToken personClone = copyPerson.Person.DeepClone();
        //        personClone["is_personal"] = false;
        //        persons.Add(personClone);
        //        msglog += $"Д:{r + 1},";
        //    }

        //}
        public static string FindAddWithComment(JToken jBase, string findStr)
        {
            string ostr = "";
            JArray persons = PersonsCurRace(jBase);

            for (int i = 0; i < persons.Count; i++)
            {
                JToken person = persons[i];
                string comment = PPComment(person);
                if (comment.Contains(findStr))
                {
                    ostr += PPBib(person) + ",";
                }
            }

            return ostr;
        }

        public static string CreateNewPerson(JArray races,PersonCopies copyPerson)
        {
            string msglog = "";
            int bibCP = PPBib(copyPerson.Person);

            if (PersonExitsInRaceByBib(races, copyPerson))
            {
                PersonCopies[] pc = GetPersonTockensByBib(races, bibCP);
                return $"⚠Номер {bibCP} не уникален. {PersonCopies.ToString(pc)}";
            }

            msglog += "Создание - ";
            for (int r = 0; r < races.Count; r++)
            {
                if (r == copyPerson.RaceIndex) continue;
                JToken race = races[r];
                JArray persons = Persons(race);
                JToken personClone = copyPerson.Person.DeepClone();
                personClone["is_personal"] = false;
                persons.Add(personClone);
                msglog += $"Д:{r + 1},";
            }
            copyPerson.Person["is_personal"] = false;
            msglog += $"  {PersonCopies.ToString(GetPersonTockensByBib(races, bibCP),false)}";
            return msglog;
        }

        public static bool PersonExitsInRaceByBib(JArray races, PersonCopies copyPerson)
        {
            for (int r = 0; r < races.Count; r++)
            {
                if (r == copyPerson.RaceIndex) continue;
                JArray persons = Persons(races[r]);
                int findBib = PPBib(copyPerson.Person);
                for (int i = 0; i < persons.Count; i++)
                {
                    int bib = PPBib(persons[i]);
                    if (bib == findBib)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string CopyPersonsByNumberList(JToken jBase , int[] bibToCopy, string[] syncFields,string removePartFromComment = "")
        {
            int createCount = 0;
            string msgLog = "Синхронизация участников по номерам...\n";
            int currentRaceI = CurrentRaceID(jBase);
            msgLog += $"Текущий день: {currentRaceI + 1}\n";
            JArray races = Races(jBase);
            for (int i = 0; i < bibToCopy.Length; i++)
            {
                PersonCopies[] pc = GetPersonTockensByBib(races, bibToCopy[i]);
                msgLog += PersonCopiesCopy(pc, currentRaceI, syncFields);
            }
            return msgLog;
        }
        public static int ReservCountByPersonCopies(PersonCopies[] personCopies,string reservSurname)
        {
            int count = 0;
            for (int i = 0; i < personCopies.Length; i++)
            {
                if (personCopies[i].Person is null) continue;
                if (PPSurname(personCopies[i].Person) == reservSurname)
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
            if (personA is null || peosonB is null) return false;
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
            JArray persons = Persons(race);
            for (int i = 0; i < persons.Count; i++)
            {
                int pbib = PPBib(persons[i]); 
                if(pbib == bib)
                    return persons[i];
            }
            return null;
        }
        
    }
}
