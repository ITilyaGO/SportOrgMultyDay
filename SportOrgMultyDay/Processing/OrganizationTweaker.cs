using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Helpers;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    public static class OrganizationTweaker
    {
        public static string RenameOrganizations(OrganizationItemsController controller, JToken race)
        {
            string msgLog = "Переиминование коллективов...\n";

            if (controller == null)
            {
                msgLog += "  Список переиминования не загружен - переиминование остановлено";
                return msgLog;
            }

            JArray organizations = PBOrganizations(race);
            JArray persons = PBPersons(race);


            foreach (JObject org in organizations)
            {
                string orgName = POName(org);
                string orgId = POId(org);
                OrganizationItem orgItem = controller.GetOrgItem(orgName);
                if (orgItem == null)
                {
                    msgLog += $"    Не найдено: {orgName}\n";
                    continue;
                }
                string newName = orgItem.GetNewName();
                msgLog += $"  Переиминование: {orgName} => {newName}... ";
                org["name"] = newName;
                org["region"] = orgItem.City;

                FPAllByOrganization(orgId, persons)
                    .ForEach(person =>
                    {
                        person["world_code"] = $"{(orgItem.IsRemoveable ? "Y" : "N")}";
                    });

                msgLog += $"OK\n";
            }


            return msgLog;
        }

        //public static string RemovePersonsFromRemovableOrganizations(OrganizationItemsController controller, JToken race, bool remove = false)
        //{
        //    string msgLog = remove ? "Удаление участников" : "Установка ВК участникам" + " из отмечанных коллективов...\n";
            
        //    JArray organizations = PBOrganizations(race);


        //    return msgLog;
        //}
    }

    public class OrganizationItem
    {
        public string Name { get; set; }
        public string NewNameRaw { get; set; }
        [JsonIgnore]
        public string NewName { get => IsNewName() ? NewNameRaw : Name; set => NewNameRaw = value; }
        public string City { get; set; }
        public bool IsRemoveable { get; set; }
        public bool IsShowCity { get; set; }
        public OrganizationItem(string name, string newName, string city, bool isRemovable = false, bool isShowCity = true)
        {
            Name = name;
            NewNameRaw = newName;
            City = city;
            IsRemoveable = isRemovable;
            IsShowCity = isShowCity;
        }

        public string GetNewName()
        {
            return NewName + (IsShowCity ? $" г. {City}" : "");
        }

        public bool IsNewName()
        {
            return IsStringEmpty(NewNameRaw);
        }

        public bool IsCity()
        {
            return IsStringEmpty(City);
        }

        private bool IsStringEmpty(string str)
        {
            return str != null && str != string.Empty;
        }

        public override string ToString()
        {
            return $"{(IsRemoveable ? "Y" : "N")} [{Name}]" +
                $"{(IsNewName() ? $" >> [{NewName}]" : "")}" +
                $"{(IsShowCity ? $" г. {City}" : "")}";
        }
    }

    public class OrganizationItemsController
    {
        [JsonIgnore]
        public const string DefaultPath = "org_rename.json";
        public Dictionary<string, OrganizationItem> OrganizationItems { get; private set; } = new();
        public string Add(string fromName, string toName, string city, bool isRemovable, bool isShowCity)
        {
            OrganizationItem organizationItem = new(fromName, toName, city, isRemovable, isShowCity);
            if (OrganizationItems.ContainsKey(fromName))
                OrganizationItems[fromName] = organizationItem;
            else
                OrganizationItems.Add(fromName, organizationItem);

            return $"  Добавлено [{fromName}] => [{toName}] Удаление: {(isRemovable ? "Да" : "Нет")} Город: [{city}] {(isShowCity ? "показывается" : "скрыт")}";
        }

        public OrganizationItem GetOrgItem(string orgName)
        {
            if (OrganizationItems.TryGetValue(orgName, out OrganizationItem organizationItem))
                return organizationItem;
            else
                return null;
        }

        public void Save(out string msgLog, string path = DefaultPath)
        {
            string exportText = JsonConvert.SerializeObject(this);
            Backup.BackUp(path, "Replacnemt");
            File.WriteAllText(path, exportText);
            msgLog = $"Сохранено {OrganizationItems.Count} в [{path}]";
        }

        public static OrganizationItemsController Load(out string msgLog, string path = DefaultPath)
        {
            if(!File.Exists(path))
            {
                msgLog = $"Загрузка списка замен не удалась. Файл не найден {path}\n\n";
                return new();
            }
            string inputText = File.ReadAllText(path);
            OrganizationItemsController tempOrganizationTweakerController = JsonConvert.DeserializeObject<OrganizationItemsController>(inputText);
            if (tempOrganizationTweakerController == null)
            {
                msgLog = $"Загрузка списка замен не удалась \n\n {inputText}";
                return null;
            }
            msgLog = $"Загружено {tempOrganizationTweakerController.OrganizationItems.Count} пар из [{path}]";
            return tempOrganizationTweakerController;
        }
    }
}
