using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay
{
    /// <summary>
    /// Неполные эстафетные заявки: участники команд, где не хватает этапов, и объединение одиночек в команду.
    /// Режим объединения: ПКМ по участнику добавляет его в нижнюю таблицу, Пробел сохраняет команду
    /// (этапы по порядку добавления), C - сброс, R - реверс порядка. Форма собрана кодом (без дизайнера).
    /// </summary>
    public class IncompleteTeamsForm : Form
    {
        private static readonly Color MergeColor = Color.FromArgb(255, 224, 178);

        private readonly JToken race;
        private readonly Action<string> onChanged;
        private readonly List<JToken> mergeList = new();
        private readonly Dictionary<JToken, Color> baseColors = new();
        private readonly Dictionary<JToken, int> teamSizes = new();

        private readonly ComboBox comboGroup = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        private readonly CheckBox checkOnlySingles = new() { Text = "Только одиночки (1 участник в заявке)", AutoSize = true, Padding = new Padding(8, 4, 0, 0) };
        private readonly CheckBox checkMergeMode = new() { Text = "Режим объединения", AutoSize = true, Padding = new Padding(8, 4, 8, 0) };
        private readonly Button buttonSave = new() { Text = "Сохранить команду (Пробел)", AutoSize = true, Enabled = false };
        private readonly Button buttonClear = new() { Text = "Сброс (C)", AutoSize = true, Enabled = false };
        private readonly Button buttonReverse = new() { Text = "Реверс порядка (R)", AutoSize = true, Enabled = false };
        private readonly Button buttonRefresh = new() { Text = "Обновить", AutoSize = true };
        private readonly Label labelInfo = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };
        private readonly Label labelMerge = new() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
        private readonly DataGridView grid = MakeGrid();
        private readonly DataGridView mergeGrid = MakeGrid();

        public IncompleteTeamsForm(JToken race, Action<string> onChanged = null)
        {
            this.race = race;
            this.onChanged = onChanged;
            Text = "Неполные эстафетные заявки";
            Size = new Size(1250, 800);
            StartPosition = FormStartPosition.CenterParent;

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            panel.Controls.AddRange(new Control[] { new Label { Text = "Группа:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) },
                comboGroup, checkOnlySingles, buttonRefresh, checkMergeMode, buttonSave, buttonClear, buttonReverse, labelInfo });

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            split.Panel1.Controls.Add(grid);
            split.Panel2.Controls.Add(mergeGrid);
            split.Panel2.Controls.Add(labelMerge);
            Controls.Add(split);
            Controls.Add(panel);
            Load += (_, _) => split.SplitterDistance = split.Height * 7 / 10;

            foreach (var (name, width) in new[] { ("Статус", 170), ("Группа", 110), ("Заявка", 80), ("Этап", 45), ("№", 55), ("Фамилия", 120), ("Имя", 100),
                ("Дата рожд.", 90), ("Клуб", 150), ("В заявке", 65), ("Этапов в группе", 90), ("Не хватает этапов", 120), ("Комментарий", 200), ("world_code", 100) })
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width });
            foreach (var (name, width) in new[] { ("Новый этап", 70), ("Группа", 110), ("Заявка сейчас", 90), ("№", 55), ("Фамилия", 120), ("Имя", 100),
                ("Дата рожд.", 90), ("Клуб", 150), ("world_code сейчас", 110), ("world_code после", 110), ("Комментарий", 200) })
                mergeGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width });

            comboGroup.Items.Add("Все");
            foreach (JToken g in IncompleteTeamsFinder.RelayGroups(race).OrderBy(g => PGName(g)))
                comboGroup.Items.Add(new GroupItem(g));
            comboGroup.SelectedIndex = 0;

            comboGroup.SelectedIndexChanged += (_, _) => Reload();
            checkOnlySingles.CheckedChanged += (_, _) => Reload();
            buttonRefresh.Click += (_, _) => Reload();
            checkMergeMode.CheckedChanged += (_, _) => MergeModeChanged();
            buttonSave.Click += (_, _) => SaveTeam();
            buttonClear.Click += (_, _) => ClearMerge();
            buttonReverse.Click += (_, _) => ReverseMerge();
            grid.CellMouseDown += GridCellMouseDown;

            UpdateMergeView();
            Reload();
        }

        private static DataGridView MakeGrid() => new()
        {
            Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
            ReadOnly = true, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = SystemColors.Window
        };

        private record GroupItem(JToken Group)
        {
            public override string ToString() => PGName(Group);
        }

        private bool MergeMode => checkMergeMode.Checked;

        // Горячие клавиши работают только в режиме объединения (чтобы Пробел не переключал чекбокс/кнопки)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (MergeMode && ActiveControl is not ComboBox)
            {
                switch (keyData)
                {
                    case Keys.Space: SaveTeam(); return true;
                    case Keys.C: ClearMerge(); return true;
                    case Keys.R: ReverseMerge(); return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MergeModeChanged()
        {
            buttonSave.Enabled = buttonClear.Enabled = buttonReverse.Enabled = MergeMode;
            if (MergeMode) grid.Focus();
            else mergeList.Clear();
            UpdateMergeView();
        }

        private void GridCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!MergeMode || e.Button != MouseButtons.Right || e.RowIndex < 0) return;
            if (grid.Rows[e.RowIndex].Tag is not JToken person) return;
            grid.ClearSelection();
            grid.Rows[e.RowIndex].Selected = true;

            if (mergeList.Contains(person)) return;
            if (mergeList.Count > 0 && PPGroupId(mergeList[0]) != PPGroupId(person))
            {
                MessageBox.Show("В одну команду можно объединить только участников одной группы.");
                return;
            }
            JToken group = FGById(PPGroupId(person), PBGroups(race));
            int legs = IncompleteTeamsFinder.GroupLegs(race, group);
            if (legs > 0 && mergeList.Count >= legs)
            {
                MessageBox.Show($"В группе {PGName(group)} всего этапов: {legs}, больше участников добавить нельзя.");
                return;
            }
            mergeList.Add(person);
            UpdateMergeView();
        }

        private void ClearMerge()
        {
            mergeList.Clear();
            UpdateMergeView();
        }

        private void ReverseMerge()
        {
            mergeList.Reverse();
            UpdateMergeView();
        }

        /// <summary>
        /// id заявки для новой команды: id первого добавленного, у кого он есть, если им больше никто в группе не пользуется;
        /// иначе новый (максимальный id в базе + 1).
        /// </summary>
        private string ChooseApplicationId()
        {
            string groupId = PPGroupId(mergeList[0]);
            string first = mergeList.Select(DuplicateFinder.ParseRelay).Select(m => m.ApplicationId).FirstOrDefault(a => !string.IsNullOrEmpty(a));
            JArray persons = PBPersons(race);
            if (first != null && !persons.Any(p => !mergeList.Contains(p) && PPGroupId(p) == groupId && DuplicateFinder.ParseRelay(p).ApplicationId == first))
                return first;
            long max = persons.Select(p => long.TryParse(DuplicateFinder.ParseRelay(p).ApplicationId, out long a) ? a : 0).DefaultIfEmpty(0).Max();
            return (max + 1).ToString();
        }

        private void UpdateMergeView()
        {
            mergeGrid.Rows.Clear();
            var orgNames = DictOIdOrgName(PBOrganizations(race));
            var groupNames = DictGIdGroupName(PBGroups(race));
            string appId = mergeList.Count > 0 ? ChooseApplicationId() : null;
            for (int i = 0; i < mergeList.Count; i++)
            {
                JToken p = mergeList[i];
                mergeGrid.Rows.Add(i + 1, groupNames.TryGetValue(PPGroupId(p) ?? "", out string g) ? g : "?", DuplicateFinder.ParseRelay(p).ApplicationId,
                    PPBib(p), PPSurname(p), PPName(p), DuplicateFinder.BirthDate(p),
                    orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "", PPWorldCode(p), $"{appId}-{i + 1}", PPComment(p));
            }
            labelMerge.Text = MergeMode
                ? $"Новая команда: участников {mergeList.Count}. ПКМ по участнику в таблице выше - добавить; Пробел - сохранить; C - сброс; R - реверс порядка."
                : "Включите \"Режим объединения\", чтобы собирать команду (ПКМ по участнику - добавить).";
            RefreshHighlight();
        }

        private void SaveTeam()
        {
            if (mergeList.Count == 0) return;
            string appId = ChooseApplicationId();
            string groupName = PGName(FGById(PPGroupId(mergeList[0]), PBGroups(race)));
            string log = $"Объединение команды в группе {groupName}, заявка {appId}:\n";
            for (int i = 0; i < mergeList.Count; i++)
            {
                JToken p = mergeList[i];
                string old = PPWorldCode(p);
                p["world_code"] = $"{appId}-{i + 1}";
                log += $"  Этап {i + 1}: {PersonToString.BibName(p)} world_code {old} -> {p["world_code"]}\n";
            }
            mergeList.Clear();
            onChanged?.Invoke(log);
            Reload();
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

            // Тех, кто пропал из списка (например, команда стала полной), из набора убираем
            var shown = members.Select(m => m.Person).ToHashSet();
            mergeList.RemoveAll(p => !shown.Contains(p));

            grid.Rows.Clear();
            baseColors.Clear();
            teamSizes.Clear();
            string prevApp = null;
            bool shade = false;
            foreach (IncompleteTeamMember m in members)
            {
                string appKey = PGId(m.Group) + "|" + m.ApplicationId;
                if (m.ApplicationId == string.Empty || appKey != prevApp) shade = !shade;
                prevApp = appKey;
                JToken p = m.Person;
                int i = grid.Rows.Add("", PGName(m.Group), m.ApplicationId, m.Stage > 0 ? m.Stage.ToString() : "", PPBib(p), PPSurname(p), PPName(p),
                    DuplicateFinder.BirthDate(p), orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "",
                    m.TeamSize, m.GroupLegs, string.Join(", ", m.MissingStages), PPComment(p), PPWorldCode(p));
                grid.Rows[i].Tag = p;
                teamSizes[p] = m.TeamSize;
                // Одиночки - белые, участники команд - голубой/сиреневый по чередованию заявок, граница между заявками - разделитель
                baseColors[p] = m.TeamSize == 1 ? Color.White : shade ? Color.FromArgb(222, 235, 250) : Color.FromArgb(235, 226, 247);
                bool lastOfTeam = i + 1 >= members.Count || PGId(members[i + 1].Group) + "|" + members[i + 1].ApplicationId != appKey || m.ApplicationId == string.Empty;
                if (lastOfTeam) grid.Rows[i].DividerHeight = 3;
            }
            int teams = members.Select(m => PGId(m.Group) + "|" + (m.ApplicationId == string.Empty ? PPId(m.Person) : m.ApplicationId)).Distinct().Count();
            labelInfo.Text = $"Участников: {members.Count}, неполных заявок: {teams}";
            UpdateMergeView();
        }

        /// <summary>Добавленные в команду подсвечиваются; цвет выделения - темнее цвета строки.</summary>
        private void RefreshHighlight()
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                var p = (JToken)row.Tag;
                int mergeIndex = mergeList.IndexOf(p);
                Color c = mergeIndex >= 0 ? MergeColor : baseColors.GetValueOrDefault(p, Color.White);
                int size = teamSizes.GetValueOrDefault(p, 1);
                row.Cells[0].Value = mergeIndex >= 0 ? $"-> новая команда, этап {mergeIndex + 1}"
                    : size > 1 ? $"в команде ({size} уч.)" : "одиночка";
                row.DefaultCellStyle.BackColor = c;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(c.R * 8 / 10, c.G * 8 / 10, c.B * 8 / 10);
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }
    }
}
