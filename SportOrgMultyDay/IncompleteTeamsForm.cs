using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay
{
    /// <summary>Неполные эстафетные заявки: участники команд, где не хватает этапов. Форма собрана кодом (без дизайнера).</summary>
    public class IncompleteTeamsForm : Form
    {
        private readonly JToken race;
        private readonly ComboBox comboGroup = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        private readonly CheckBox checkOnlySingles = new() { Text = "Только одиночки (1 участник в заявке)", AutoSize = true, Padding = new Padding(8, 4, 0, 0) };
        private readonly Button buttonRefresh = new() { Text = "Обновить", AutoSize = true };
        private readonly Label labelInfo = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };
        private readonly DataGridView grid = new()
        {
            Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
            ReadOnly = true, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = SystemColors.Window
        };

        public IncompleteTeamsForm(JToken race)
        {
            this.race = race;
            Text = "Неполные эстафетные заявки";
            Size = new Size(1250, 700);
            StartPosition = FormStartPosition.CenterParent;

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            panel.Controls.AddRange(new Control[] { new Label { Text = "Группа:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) },
                comboGroup, checkOnlySingles, buttonRefresh, labelInfo });
            Controls.Add(grid);
            Controls.Add(panel);

            foreach (var (name, width) in new[] { ("Группа", 110), ("Заявка", 80), ("Этап", 45), ("№", 55), ("Фамилия", 120), ("Имя", 100),
                ("Дата рожд.", 90), ("Клуб", 150), ("В заявке", 65), ("Этапов в группе", 90), ("Не хватает этапов", 120), ("Комментарий", 200), ("world_code", 100) })
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width });

            comboGroup.Items.Add("Все");
            foreach (JToken g in IncompleteTeamsFinder.RelayGroups(race).OrderBy(g => PGName(g)))
                comboGroup.Items.Add(new GroupItem(g));
            comboGroup.SelectedIndex = 0;

            comboGroup.SelectedIndexChanged += (_, _) => Reload();
            checkOnlySingles.CheckedChanged += (_, _) => Reload();
            buttonRefresh.Click += (_, _) => Reload();
            Reload();
        }

        private record GroupItem(JToken Group)
        {
            public override string ToString() => PGName(Group);
        }

        private void Reload()
        {
            string groupId = comboGroup.SelectedItem is GroupItem gi ? PGId(gi.Group) : null;
            var orgNames = DictOIdOrgName(PBOrganizations(race));
            var members = IncompleteTeamsFinder.Find(race, groupId);
            if (checkOnlySingles.Checked)
                members = members.Where(m => m.TeamSize == 1).ToList();
            members = members.OrderBy(m => PGName(m.Group))
                .ThenBy(m => long.TryParse(m.ApplicationId, out long a) ? a : long.MaxValue).ThenBy(m => m.ApplicationId).ThenBy(m => m.Stage).ToList();

            grid.Rows.Clear();
            string prevApp = null;
            bool shade = false;
            foreach (IncompleteTeamMember m in members)
            {
                string appKey = PGId(m.Group) + "|" + m.ApplicationId;
                if (m.ApplicationId == string.Empty || appKey != prevApp) shade = !shade;
                prevApp = appKey;
                JToken p = m.Person;
                int i = grid.Rows.Add(PGName(m.Group), m.ApplicationId, m.Stage > 0 ? m.Stage.ToString() : "", PPBib(p), PPSurname(p), PPName(p),
                    DuplicateFinder.BirthDate(p), orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "",
                    m.TeamSize, m.GroupLegs, string.Join(", ", m.MissingStages), PPComment(p), PPWorldCode(p));
                grid.Rows[i].Tag = p;
                if (shade) grid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
            }
            int teams = members.Select(m => PGId(m.Group) + "|" + (m.ApplicationId == string.Empty ? PPId(m.Person) : m.ApplicationId)).Distinct().Count();
            labelInfo.Text = $"Участников: {members.Count}, неполных заявок: {teams}";
        }
    }
}
