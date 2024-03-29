﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data
{
    public class ComboBoxItemId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ComboBoxItemId(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
