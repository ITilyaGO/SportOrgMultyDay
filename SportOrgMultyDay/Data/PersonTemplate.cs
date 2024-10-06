using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data
{
    public static class PersonTemplate
    {
        public static JObject Reserv
        {
            get
            {
                return new()
                {
                    ["bib"] = 0,
                    ["birth_date"] = null,
                    ["card_number"] = 0,
                    ["comment"] = "",
                    ["group_id"] = null,
                    ["id"] = Guid.NewGuid().ToString(),
                    ["is_out_of_competition"] = false,
                    ["is_paid"] = false,
                    ["is_personal"] = false,
                    ["is_rented_card"] = false,
                    ["name"] = "",
                    ["national_code"] = 0,
                    ["object"] = "Person",
                    ["organization_id"] = null,
                    ["qual"] = 0,
                    ["sex"] = 0,
                    ["start_group"] = 0,
                    ["start_time"] = 0,
                    ["surname"] = "_Резерв",
                    ["world_code"] = "",
                    ["year"] = 0
                };
            }
        }

        public static JObject ReservWithData(string group_id = null)
        {
            JObject person = Reserv;
            person["group_id"] = group_id;
            return person;
        }
    }
}
