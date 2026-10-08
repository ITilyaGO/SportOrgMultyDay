using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay
{
    /// <summary>
    /// Неполные эстафетные заявки: участники команд, где не хватает этапов, и объединение одиночек в команды.
    /// Слева список и сборка новой команды, справа буфер готовых команд: в базу ничего не пишется, пока не нажато "Сохранить".
    /// Режим объединения: ПКМ по участнику добавляет его в новую команду, Пробел отправляет команду в буфер
    /// (этапы по порядку добавления), C - сброс, R - реверс порядка, Del - вернуть выбранную команду из буфера.
    /// Форма собрана кодом (без дизайнера).
    /// </summary>
    public class IncompleteTeamsForm : Form
    {
        private static readonly Color MergeColor = Color.FromArgb(255, 224, 178);

        private readonly JToken race;
        private readonly Action<string> onChanged;
        private readonly List<JToken> mergeList = new();
        private readonly List<List<JToken>> buffer = new();
        private readonly Dictionary<JToken, Color> baseColors = new();
        private readonly Dictionary<JToken, int> teamSizes = new();

        private readonly ComboBox comboGroup = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        private readonly NumericUpDown numericMinLegs = new() { Minimum = 1, Maximum = 20, Value = IncompleteTeamsFinder.DefaultMinLegs, Width = 50 };
        private readonly CheckBox checkOnlySingles = new() { Text = "Только одиночки (1 участник в заявке)", AutoSize = true, Padding = new Padding(8, 4, 0, 0) };
        private readonly CheckBox checkMergeMode = new() { Text = "Режим объединения", AutoSize = true, Padding = new Padding(8, 4, 8, 0) };
        private readonly Button buttonSave = new() { Text = "В буфер (Пробел)", AutoSize = true, Enabled = false };
        private readonly Button buttonClear = new() { Text = "Сброс (C)", AutoSize = true, Enabled = false };
        private readonly Button buttonReverse = new() { Text = "Реверс порядка (R)", AutoSize = true, Enabled = false };
        private readonly Button buttonRefresh = new() { Text = "Обновить", AutoSize = true };
        private readonly ComboBox comboClubMode = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
        private readonly CheckBox checkRank = new() { Text = "Сильные вместе (разряд)", AutoSize = true, Checked = true, Padding = new Padding(8, 4, 0, 0) };
        private readonly CheckBox checkAge = new() { Text = "Близкие по возрасту", AutoSize = true, Checked = true, Padding = new Padding(8, 4, 0, 0) };
        private readonly CheckBox checkStages = new() { Text = "Учитывать заявленные этапы", AutoSize = true, Checked = true, Padding = new Padding(8, 4, 8, 0) };
        private readonly Button buttonAuto = new() { Text = "Авто-объединение в буфер", AutoSize = true };
        private readonly Button buttonApply = new() { Text = "Сохранить", AutoSize = true };
        private readonly Button buttonReturnTeam = new() { Text = "Вернуть команду (Del)", AutoSize = true };
        private readonly Button buttonClearBuffer = new() { Text = "Очистить буфер", AutoSize = true };
        private readonly Label labelInfo = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };
        private readonly Label labelMerge = new() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
        private readonly Label labelBuffer = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };
        private readonly DataGridView grid = MakeGrid();
        private readonly DataGridView mergeGrid = MakeGrid();
        private readonly DataGridView bufferGrid = MakeGrid();

        public IncompleteTeamsForm(JToken race, Action<string> onChanged = null)
        {
            this.race = race;
            this.onChanged = onChanged;
            Text = "Неполные эстафетные заявки";
            Size = new Size(1500, 850);
            StartPosition = FormStartPosition.CenterParent;

            comboClubMode.Items.AddRange(new object[] { "Только из одного клуба", "Сначала из одного клуба, затем остальные", "Без учета клуба" });
            comboClubMode.SelectedIndex = 1;

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            panel.Controls.AddRange(new Control[] { new Label { Text = "Группа:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) },
                comboGroup, new Label { Text = "Минимум этапов:", AutoSize = true, Padding = new Padding(8, 6, 0, 0) }, numericMinLegs, checkOnlySingles, buttonRefresh, checkMergeMode, buttonSave, buttonClear, buttonReverse, labelInfo });
            var autoPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            autoPanel.Controls.AddRange(new Control[] { new Label { Text = "Авто-объединение:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) },
                comboClubMode, checkRank, checkAge, checkStages, buttonAuto });

            var bufferButtons = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            bufferButtons.Controls.AddRange(new Control[] { buttonApply, buttonReturnTeam, buttonClearBuffer, labelBuffer });

            var leftSplit = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            leftSplit.Panel1.Controls.Add(grid);
            leftSplit.Panel2.Controls.Add(mergeGrid);
            leftSplit.Panel2.Controls.Add(labelMerge);

            var mainSplit = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical };
            mainSplit.Panel1.Controls.Add(leftSplit);
            mainSplit.Panel2.Controls.Add(bufferGrid);
            mainSplit.Panel2.Controls.Add(bufferButtons);

            Controls.Add(mainSplit);
            Controls.Add(autoPanel);
            Controls.Add(panel);
            Load += (_, _) =>
            {
                leftSplit.SplitterDistance = leftSplit.Height * 6 / 10;
                mainSplit.SplitterDistance = mainSplit.Width * 55 / 100;
            };

            foreach (var (name, width) in new[] { ("Статус", 170), ("Группа", 110), ("Заявка", 80), ("Этап", 45), ("№", 55), ("Фамилия", 120), ("Имя", 100),
                ("Дата рожд.", 90), ("Клуб", 150), ("В заявке", 65), ("Этапов в группе", 90), ("Не хватает этапов", 120), ("Разряд", 55), ("Комментарий", 200), ("world_code", 100) })
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width });
            foreach (var (name, width) in new[] { ("Новый этап", 70), ("Группа", 110), ("Заявка сейчас", 90), ("№", 55), ("Фамилия", 120), ("Имя", 100),
                ("Дата рожд.", 90), ("Клуб", 150), ("world_code сейчас", 110), ("world_code после", 110), ("Комментарий", 200) })
                mergeGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width });
            foreach (var (name, width) in new[] { ("Команда", 100), ("Этап", 45), ("Группа", 100), ("№", 50), ("Фамилия", 110), ("Имя", 90),
                ("Дата рожд.", 85), ("Клуб", 130), ("Разряд", 50), ("world_code сейчас", 100), ("world_code после", 100) })
                bufferGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width });

            comboGroup.Items.Add("Все");
            foreach (JToken g in IncompleteTeamsFinder.RelayGroups(race).OrderBy(g => PGName(g)))
                comboGroup.Items.Add(new GroupItem(g));
            comboGroup.SelectedIndex = 0;

            comboGroup.SelectedIndexChanged += (_, _) => Reload();
            checkOnlySingles.CheckedChanged += (_, _) => Reload();
            numericMinLegs.ValueChanged += (_, _) => Reload();
            buttonRefresh.Click += (_, _) => Reload();
            checkMergeMode.CheckedChanged += (_, _) => MergeModeChanged();
            buttonSave.Click += (_, _) => MoveToBuffer();
            buttonClear.Click += (_, _) => ClearMerge();
            buttonReverse.Click += (_, _) => ReverseMerge();
            buttonAuto.Click += (_, _) => AutoMerge();
            buttonApply.Click += (_, _) => ApplyBuffer();
            buttonReturnTeam.Click += (_, _) => ReturnSelectedTeam();
            buttonClearBuffer.Click += (_, _) => ClearBuffer();
            grid.CellMouseDown += GridCellMouseDown;
            bufferGrid.CellMouseDown += BufferCellMouseDown;
            FormClosing += (_, e) =>
            {
                if (buffer.Count > 0 && MessageBox.Show($"В буфере {buffer.Count} команд, они не сохранены и будут потеряны. Закрыть форму?", "Буфер",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    e.Cancel = true;
            };

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

        private int MinLegs => (int)numericMinLegs.Value;

        private bool MergeMode => checkMergeMode.Checked;

        private HashSet<JToken> BufferedPersons => buffer.SelectMany(t => t).ToHashSet();

        // Горячие клавиши работают только в режиме объединения (чтобы Пробел не переключал чекбокс/кнопки)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ActiveControl is not ComboBox)
            {
                if (keyData == Keys.Delete && ActiveControl == bufferGrid)
                {
                    ReturnSelectedTeam();
                    return true;
                }
                if (MergeMode)
                {
                    switch (keyData)
                    {
                        case Keys.Space: MoveToBuffer(); return true;
                        case Keys.C: ClearMerge(); return true;
                        case Keys.R: ReverseMerge(); return true;
                    }
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

        /// <summary>Проверяет ограничения и добавляет участника в новую команду.</summary>
        private bool TryAddToMerge(JToken person)
        {
            if (mergeList.Contains(person)) return false;
            if (mergeList.Count > 0 && PPGroupId(mergeList[0]) != PPGroupId(person))
            {
                MessageBox.Show("В одну команду можно объединить только участников одной группы.");
                return false;
            }
            JToken group = FGById(PPGroupId(person), PBGroups(race));
            int legs = IncompleteTeamsFinder.GroupLegs(race, group, MinLegs);
            if (legs > 0 && mergeList.Count >= legs)
            {
                MessageBox.Show($"В группе {PGName(group)} всего этапов: {legs}, больше участников добавить нельзя.");
                return false;
            }
            mergeList.Add(person);
            return true;
        }

        private void GridCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!MergeMode || e.Button != MouseButtons.Right || e.RowIndex < 0) return;
            if (grid.Rows[e.RowIndex].Tag is not JToken person) return;
            grid.ClearSelection();
            grid.Rows[e.RowIndex].Selected = true;
            if (TryAddToMerge(person))
                UpdateMergeView();
        }

        /// <summary>ПКМ по участнику буфера в режиме объединения: он идет в новую команду, остальные его команды возвращаются в список слева.</summary>
        private void BufferCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (bufferGrid.Rows[e.RowIndex].Tag is not JToken person) return;
            bufferGrid.ClearSelection();
            bufferGrid.Rows[e.RowIndex].Selected = true;
            if (!MergeMode || e.Button != MouseButtons.Right) return;

            List<JToken> team = buffer.First(t => t.Contains(person));
            if (!TryAddToMerge(person)) return;
            buffer.Remove(team);
            Reload();
        }

        private void ReturnSelectedTeam()
        {
            if (bufferGrid.CurrentRow?.Tag is not JToken person) return;
            buffer.RemoveAll(t => t.Contains(person));
            Reload();
        }

        private void ClearBuffer()
        {
            if (buffer.Count == 0) return;
            buffer.Clear();
            Reload();
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

        private void MoveToBuffer()
        {
            if (mergeList.Count == 0) return;
            buffer.Add(mergeList.ToList());
            mergeList.Clear();
            Reload();
        }

        /// <summary>
        /// id заявок для команд: id первого участника, у кого он есть, если им больше никто в группе не пользуется и он не занят другой командой;
        /// иначе новый (максимальный id в базе + 1).
        /// </summary>
        private List<string> PlanApplicationIds(List<List<JToken>> teams)
        {
            JArray persons = PBPersons(race);
            long max = persons.Select(p => long.TryParse(DuplicateFinder.ParseRelay(p).ApplicationId, out long a) ? a : 0).DefaultIfEmpty(0).Max();
            var taken = new HashSet<string>();
            var ids = new List<string>();
            foreach (List<JToken> team in teams)
            {
                string groupId = PPGroupId(team[0]);
                string first = team.Select(p => DuplicateFinder.ParseRelay(p).ApplicationId).FirstOrDefault(a => !string.IsNullOrEmpty(a));
                string id;
                if (first != null && !taken.Contains(first)
                    && !persons.Any(p => !team.Contains(p) && PPGroupId(p) == groupId && DuplicateFinder.ParseRelay(p).ApplicationId == first))
                    id = first;
                else
                    id = (++max).ToString();
                taken.Add(id);
                ids.Add(id);
            }
            return ids;
        }

        private void UpdateMergeView()
        {
            mergeGrid.Rows.Clear();
            var orgNames = DictOIdOrgName(PBOrganizations(race));
            var groupNames = DictGIdGroupName(PBGroups(race));
            string appId = null;
            if (mergeList.Count > 0)
                appId = PlanApplicationIds(buffer.Append(mergeList).ToList()).Last();
            for (int i = 0; i < mergeList.Count; i++)
            {
                JToken p = mergeList[i];
                mergeGrid.Rows.Add(i + 1, groupNames.TryGetValue(PPGroupId(p) ?? "", out string g) ? g : "?", DuplicateFinder.ParseRelay(p).ApplicationId,
                    PPBib(p), PPSurname(p), PPName(p), DuplicateFinder.BirthDate(p),
                    orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "", PPWorldCode(p), $"{appId}-{i + 1}", PPComment(p));
            }
            labelMerge.Text = MergeMode
                ? $"Новая команда: участников {mergeList.Count}. ПКМ по участнику (слева или в буфере) - добавить; Пробел - в буфер; C - сброс; R - реверс порядка."
                : "Включите \"Режим объединения\", чтобы собирать команду (ПКМ по участнику - добавить).";
            RefreshHighlight();
        }

        private void UpdateBufferView()
        {
            bufferGrid.Rows.Clear();
            var orgNames = DictOIdOrgName(PBOrganizations(race));
            var groupNames = DictGIdGroupName(PBGroups(race));
            List<string> ids = buffer.Count > 0 ? PlanApplicationIds(buffer) : new List<string>();
            for (int t = 0; t < buffer.Count; t++)
            {
                Color color = t % 2 == 0 ? Color.FromArgb(226, 240, 217) : Color.FromArgb(237, 245, 228);
                for (int i = 0; i < buffer[t].Count; i++)
                {
                    JToken p = buffer[t][i];
                    int r = bufferGrid.Rows.Add($"Команда {t + 1} (заявка {ids[t]})", i + 1, groupNames.TryGetValue(PPGroupId(p) ?? "", out string g) ? g : "?",
                        PPBib(p), PPSurname(p), PPName(p), DuplicateFinder.BirthDate(p),
                        orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "", PPQual(p), PPWorldCode(p), $"{ids[t]}-{i + 1}");
                    DataGridViewRow row = bufferGrid.Rows[r];
                    row.Tag = p;
                    SetRowColor(row, color);
                    if (i == buffer[t].Count - 1) row.DividerHeight = 3;
                }
            }
            labelBuffer.Text = $"Команд в буфере: {buffer.Count}, участников: {buffer.Sum(t => t.Count)}";
        }

        private void ApplyBuffer()
        {
            if (buffer.Count == 0) return;
            List<string> ids = PlanApplicationIds(buffer);
            string log = $"Сохранение буфера эстафетных команд ({buffer.Count}):\n";
            for (int t = 0; t < buffer.Count; t++)
            {
                string groupName = PGName(FGById(PPGroupId(buffer[t][0]), PBGroups(race)));
                log += $"  Группа {groupName}, заявка {ids[t]}:\n";
                for (int i = 0; i < buffer[t].Count; i++)
                {
                    JToken p = buffer[t][i];
                    string old = PPWorldCode(p);
                    p["world_code"] = $"{ids[t]}-{i + 1}";
                    log += $"    Этап {i + 1}: {PersonToString.BibName(p)} world_code {old} -> {p["world_code"]}\n";
                }
            }
            buffer.Clear();
            onChanged?.Invoke(log);
            Reload();
        }

        private void AutoMerge()
        {
            string groupId = comboGroup.SelectedItem is GroupItem gi ? PGId(gi.Group) : null;
            var options = new AutoTeamOptions
            {
                ClubMode = (AutoTeamClubMode)comboClubMode.SelectedIndex,
                UseRank = checkRank.Checked,
                UseAge = checkAge.Checked,
                UseDeclaredStages = checkStages.Checked
            };
            var buffered = BufferedPersons;
            var singles = IncompleteTeamsFinder.Find(race, groupId, MinLegs)
                .Where(m => m.TeamSize == 1 && !buffered.Contains(m.Person) && !mergeList.Contains(m.Person)).ToList();

            int teams = 0, left = 0;
            foreach (var byGroup in singles.GroupBy(m => PGId(m.Group)))
            {
                int legs = byGroup.First().GroupLegs;
                var built = AutoTeamBuilder.Build(byGroup.Select(m => m.Person), legs, options);
                buffer.AddRange(built);
                teams += built.Count;
                left += byGroup.Count() - built.Sum(t => t.Count);
            }
            Reload();
            MessageBox.Show($"Собрано команд: {teams}. Без команды осталось одиночек: {left}.\nРезультат в буфере справа, в базу ничего не записано - проверьте и нажмите \"Сохранить\".");
        }

        private void Reload()
        {
            string groupId = comboGroup.SelectedItem is GroupItem gi ? PGId(gi.Group) : null;
            var orgNames = DictOIdOrgName(PBOrganizations(race));
            var all = IncompleteTeamsFinder.Find(race, null, MinLegs);
            var alive = all.Select(m => m.Person).ToHashSet();

            // Пропавших из списка (например, ставших полными по другой причине) убираем из наборов
            mergeList.RemoveAll(p => !alive.Contains(p));
            foreach (var t in buffer) t.RemoveAll(p => !alive.Contains(p));
            buffer.RemoveAll(t => t.Count == 0);

            var buffered = BufferedPersons;
            var members = all.Where(m => (groupId == null || PGId(m.Group) == groupId) && !buffered.Contains(m.Person));
            if (checkOnlySingles.Checked)
                members = members.Where(m => m.TeamSize == 1);
            var list = members.OrderBy(m => PGName(m.Group))
                .ThenBy(m => long.TryParse(m.ApplicationId, out long a) ? a : long.MaxValue).ThenBy(m => m.ApplicationId).ThenBy(m => m.Stage).ToList();

            grid.Rows.Clear();
            baseColors.Clear();
            teamSizes.Clear();
            string prevApp = null;
            bool shade = false;
            for (int n = 0; n < list.Count; n++)
            {
                IncompleteTeamMember m = list[n];
                string appKey = PGId(m.Group) + "|" + m.ApplicationId;
                if (m.ApplicationId == string.Empty || appKey != prevApp) shade = !shade;
                prevApp = appKey;
                JToken p = m.Person;
                int i = grid.Rows.Add("", PGName(m.Group), m.ApplicationId, m.Stage > 0 ? m.Stage.ToString() : "", PPBib(p), PPSurname(p), PPName(p),
                    DuplicateFinder.BirthDate(p), orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "",
                    m.TeamSize, m.GroupLegs, string.Join(", ", m.MissingStages), PPQual(p), PPComment(p), PPWorldCode(p));
                grid.Rows[i].Tag = p;
                teamSizes[p] = m.TeamSize;
                // Одиночки - белые, участники команд - голубой/сиреневый по чередованию заявок, граница между заявками - разделитель
                baseColors[p] = m.TeamSize == 1 ? Color.White : shade ? Color.FromArgb(222, 235, 250) : Color.FromArgb(235, 226, 247);
                bool lastOfTeam = m.ApplicationId == string.Empty || n + 1 >= list.Count || PGId(list[n + 1].Group) + "|" + list[n + 1].ApplicationId != appKey;
                if (lastOfTeam) grid.Rows[i].DividerHeight = 3;
            }
            int teams = list.Select(m => PGId(m.Group) + "|" + (m.ApplicationId == string.Empty ? PPId(m.Person) : m.ApplicationId)).Distinct().Count();
            labelInfo.Text = $"Участников: {list.Count}, неполных заявок: {teams}";
            UpdateBufferView();
            UpdateMergeView();
        }

        private static void SetRowColor(DataGridViewRow row, Color c)
        {
            row.DefaultCellStyle.BackColor = c;
            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(c.R * 8 / 10, c.G * 8 / 10, c.B * 8 / 10);
            row.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        /// <summary>Добавленные в новую команду подсвечиваются и получают статус; цвет выделения - темнее цвета строки.</summary>
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
                SetRowColor(row, c);
            }
        }
    }
}
