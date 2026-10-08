using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay
{
    /// <summary>
    /// Поиск дубликатов заявок. Сверху - люди, найденные несколько раз; снизу - их дубли
    /// (для эстафетных групп - все этапы заявок дублей). Дубли помечаются на удаление, помеченные просматриваются и удаляются.
    /// Форма собрана кодом (без дизайнера).
    /// </summary>
    public class DuplicatesForm : Form
    {
        private static readonly Color DoneColor = Color.FromArgb(198, 239, 206);
        private static readonly Color AllMarkedColor = Color.FromArgb(255, 199, 206);
        private static readonly Color DifferentGroupsColor = Color.FromArgb(204, 229, 255);
        private static readonly Color DupColor = Color.FromArgb(255, 249, 219);

        private readonly JToken race;
        private readonly Action<string> onChanged;
        private readonly HashSet<JToken> marked = new();

        private List<DuplicateSet> sets = new();
        private Dictionary<string, string> groupNames = new();
        private Dictionary<string, string> orgNames = new();
        private bool loadingBottom;

        private readonly DataGridView topGrid = MakeGrid();
        private readonly DataGridView bottomGrid = MakeGrid();
        private readonly Button buttonKeepSelected = new() { Text = "Оставить выбранного (пометить остальных дублей)", AutoSize = true };
        private readonly Button buttonUnmarkSet = new() { Text = "Снять пометки с набора", AutoSize = true };
        private readonly Button buttonMarkApplication = new() { Text = "Пометить всю заявку строки", AutoSize = true };
        private readonly CheckBox checkIgnoreDifferentGroups = new() { Text = "Игнорировать дубликаты в разных группах", AutoSize = true, Padding = new Padding(0, 4, 8, 0) };
        private readonly Button buttonAutoMark = new() { Text = "Авто-пометка", AutoSize = true };
        private readonly Button buttonReview = new() { Text = "Помеченные...", AutoSize = true };
        private readonly Button buttonRefresh = new() { Text = "Обновить", AutoSize = true };
        private readonly Label labelInfo = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };

        public DuplicatesForm(JToken race, Action<string> onChanged)
        {
            this.race = race;
            this.onChanged = onChanged;

            Text = "Поиск дубликатов заявок";
            Size = new Size(1250, 800);
            StartPosition = FormStartPosition.CenterParent;

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            buttons.Controls.AddRange(new Control[] { checkIgnoreDifferentGroups, buttonAutoMark, buttonKeepSelected, buttonUnmarkSet, buttonMarkApplication, buttonReview, buttonRefresh, labelInfo });

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            split.Panel1.Controls.Add(topGrid);
            split.Panel2.Controls.Add(bottomGrid);
            Controls.Add(split);
            Controls.Add(buttons);
            Load += (_, _) => split.SplitterDistance = split.Height / 2;

            AddTextColumns(topGrid, ("Участник", 220), ("Дата рожд.", 90), ("Дублей", 60), ("Помечено", 70), ("Группы", 320), ("Эстафета", 70));

            var markColumn = new DataGridViewCheckBoxColumn { HeaderText = "Удалить", Width = 60 };
            bottomGrid.Columns.Add(markColumn);
            AddTextColumns(bottomGrid, ("Дубль", 50), ("Группа", 110), ("Заявка", 80), ("Этап", 45), ("№", 55), ("Фамилия", 120), ("Имя", 100),
                ("Дата рожд.", 90), ("Клуб", 150), ("Карта", 75), ("Комментарий", 200), ("world_code", 100));
            bottomGrid.ReadOnly = false;
            foreach (DataGridViewColumn c in bottomGrid.Columns)
                c.ReadOnly = c != markColumn;

            topGrid.SelectionChanged += (_, _) => LoadBottom();
            bottomGrid.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (bottomGrid.IsCurrentCellDirty) bottomGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            bottomGrid.CellValueChanged += BottomCellValueChanged;

            buttonAutoMark.Click += (_, _) => AutoMark();
            checkIgnoreDifferentGroups.CheckedChanged += (_, _) => RefreshAfterMarkChange();
            buttonKeepSelected.Click += (_, _) => KeepSelected();
            buttonUnmarkSet.Click += (_, _) => UnmarkSet();
            buttonMarkApplication.Click += (_, _) => MarkApplication();
            buttonReview.Click += (_, _) => Review();
            buttonRefresh.Click += (_, _) => Reload();

            Reload();
        }

        private static DataGridView MakeGrid() => new()
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            MultiSelect = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = SystemColors.Window
        };

        private static void AddTextColumns(DataGridView grid, params (string name, int width)[] cols)
        {
            foreach (var (name, width) in cols)
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = name, Width = width, SortMode = DataGridViewColumnSortMode.NotSortable });
        }

        private DuplicateSet SelectedSet =>
            topGrid.CurrentRow?.Tag as DuplicateSet;

        private int UnmarkedCount(DuplicateSet set) => set.Persons.Count(p => !marked.Contains(p));

        /// <summary>Полная перестройка: заново ищет дубли, пометки сохраняются для тех, кто остался в базе.</summary>
        private void Reload()
        {
            groupNames = DictGIdGroupName(PBGroups(race));
            orgNames = DictOIdOrgName(PBOrganizations(race));
            sets = DuplicateFinder.Find(race);
            string selectedKey = SelectedSet?.Key;

            var alive = PBPersons(race).ToHashSet();
            marked.RemoveWhere(p => !alive.Contains(p));

            topGrid.Rows.Clear();
            foreach (DuplicateSet set in sets)
            {
                JToken first = set.Persons[0];
                int i = topGrid.Rows.Add(set.Title, DuplicateFinder.BirthDate(first), set.Persons.Count, 0,
                    string.Join(", ", set.Persons.Select(GroupName).Distinct()), set.HasRelay ? "да" : "");
                topGrid.Rows[i].Tag = set;
            }
            for (int i = 0; i < topGrid.Rows.Count; i++)
                RefreshTopRow(topGrid.Rows[i]);

            if (selectedKey != null)
            {
                foreach (DataGridViewRow row in topGrid.Rows)
                    if (((DuplicateSet)row.Tag).Key == selectedKey) { topGrid.CurrentCell = row.Cells[0]; break; }
            }
            LoadBottom();
            UpdateInfo();
        }

        private string GroupName(JToken person) =>
            groupNames.TryGetValue(PPGroupId(person) ?? "", out string n) ? n : "?";

        private void RefreshTopRow(DataGridViewRow row)
        {
            var set = (DuplicateSet)row.Tag;
            row.Cells[3].Value = set.Persons.Count(marked.Contains);
            bool differentGroups = set.Persons.Select(p => PPGroupId(p)).Distinct().Count() > 1;
            Color color = IsDone(set) ? DoneColor
                : UnmarkedCount(set) == 0 ? AllMarkedColor
                : differentGroups ? DifferentGroupsColor
                : Color.White;
            ApplyRowColor(row, color);
        }

        /// <summary>Цвет строки и цвет выделения того же оттенка (темнее), чтобы при выборе строки был виден ее цвет.</summary>
        private static void ApplyRowColor(DataGridViewRow row, Color color)
        {
            row.DefaultCellStyle.BackColor = color;
            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(color.R * 8 / 10, color.G * 8 / 10, color.B * 8 / 10);
            row.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        /// <summary>
        /// Набор готов (зеленый): остался ровно один непомеченный - тот, кого оставляем.
        /// При включенном игноре групп - ровно один непомеченный в каждой группе набора.
        /// </summary>
        private bool IsDone(DuplicateSet set)
        {
            if (!checkIgnoreDifferentGroups.Checked)
                return UnmarkedCount(set) == 1;
            return set.Persons.GroupBy(p => PPGroupId(p)).All(g => g.Count(p => !marked.Contains(p)) == 1);
        }

        private void UpdateInfo()
        {
            int done = sets.Count(IsDone);
            labelInfo.Text = $"Наборов: {sets.Count}, готово (зеленые): {done}, помечено на удаление: {marked.Count}";
            buttonReview.Text = $"Помеченные ({marked.Count})...";
        }

        private void LoadBottom()
        {
            loadingBottom = true;
            bottomGrid.Rows.Clear();
            DuplicateSet set = SelectedSet;
            if (set != null)
            {
                JArray persons = PBPersons(race);
                var shown = new HashSet<JToken>();
                var dups = set.Persons.ToHashSet();
                foreach (JToken dup in set.Persons)
                {
                    bool relay = DuplicateFinder.IsRelayGroup(race, PPGroupId(dup));
                    var members = relay ? DuplicateFinder.RelayTeam(dup, persons).Select(m => m.Person) : new[] { dup };
                    foreach (JToken p in members)
                    {
                        if (!shown.Add(p)) continue;
                        RelayMember rm = DuplicateFinder.ParseRelay(p);
                        bool isDup = dups.Contains(p);
                        int i = bottomGrid.Rows.Add(marked.Contains(p), isDup ? "●" : "", GroupName(p),
                            relay ? rm.ApplicationId : "", relay && rm.Stage > 0 ? rm.Stage.ToString() : "",
                            PPBib(p), PPSurname(p), PPName(p), DuplicateFinder.BirthDate(p),
                            orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "",
                            PPCardNumber(p) > 0 ? PPCardNumber(p).ToString() : "", PPComment(p), PPWorldCode(p));
                        DataGridViewRow row = bottomGrid.Rows[i];
                        row.Tag = p;
                        ApplyRowColor(row, isDup ? DupColor : Color.White);
                        if (isDup)
                        {
                            row.DefaultCellStyle.Font = new Font(bottomGrid.Font, FontStyle.Bold);
                        }
                    }
                }
            }
            loadingBottom = false;
        }

        private void BottomCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (loadingBottom || e.RowIndex < 0 || e.ColumnIndex != 0) return;
            var person = (JToken)bottomGrid.Rows[e.RowIndex].Tag;
            if ((bool)(bottomGrid.Rows[e.RowIndex].Cells[0].Value ?? false)) marked.Add(person);
            else marked.Remove(person);
            RefreshAfterMarkChange();
        }

        private void RefreshAfterMarkChange()
        {
            foreach (DataGridViewRow row in topGrid.Rows) RefreshTopRow(row);
            UpdateInfo();
        }

        private void SetMark(JToken person, bool value)
        {
            if (value) marked.Add(person); else marked.Remove(person);
            foreach (DataGridViewRow row in bottomGrid.Rows)
                if (ReferenceEquals(row.Tag, person)) row.Cells[0].Value = value;
        }

        private void AutoMark()
        {
            if (sets.Count == 0) return;
            if (marked.Count > 0 && MessageBox.Show("Текущие пометки в обрабатываемых наборах будут заменены. Продолжить?", "Авто-пометка",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            HashSet<JToken> auto = DuplicateFinder.AutoMark(sets, race, checkIgnoreDifferentGroups.Checked);
            // Пометки участников найденных наборов перезаписываем результатом алгоритма
            marked.ExceptWith(sets.SelectMany(s => s.Persons));
            marked.UnionWith(auto);
            LoadBottom();
            RefreshAfterMarkChange();
            MessageBox.Show($"Обработано наборов: {sets.Count}, помечено на удаление: {auto.Count}.");
        }

        private void KeepSelected()
        {
            DuplicateSet set = SelectedSet;
            if (set == null || bottomGrid.CurrentRow?.Tag is not JToken keep || !set.Persons.Contains(keep))
            {
                MessageBox.Show("Выберите в нижней таблице строку дубля, которого нужно оставить.");
                return;
            }
            loadingBottom = true;
            foreach (JToken p in set.Persons) SetMark(p, !ReferenceEquals(p, keep));
            SetMark(keep, false);
            loadingBottom = false;
            RefreshAfterMarkChange();
        }

        private void UnmarkSet()
        {
            DuplicateSet set = SelectedSet;
            if (set == null) return;
            loadingBottom = true;
            foreach (DataGridViewRow row in bottomGrid.Rows)
                SetMark((JToken)row.Tag, false);
            loadingBottom = false;
            RefreshAfterMarkChange();
        }

        private void MarkApplication()
        {
            if (bottomGrid.CurrentRow?.Tag is not JToken current) return;
            RelayMember cur = DuplicateFinder.ParseRelay(current);
            if (string.IsNullOrEmpty(cur.ApplicationId))
            {
                MessageBox.Show("У выбранного участника нет id заявки в world_code (формат id-этап).");
                return;
            }
            loadingBottom = true;
            foreach (DataGridViewRow row in bottomGrid.Rows)
            {
                var p = (JToken)row.Tag;
                if (PPGroupId(p) == PPGroupId(current) && DuplicateFinder.ParseRelay(p).ApplicationId == cur.ApplicationId)
                    SetMark(p, true);
            }
            loadingBottom = false;
            RefreshAfterMarkChange();
        }

        private void Review()
        {
            using var dlg = new MarkedReviewForm(marked, race, groupNames, orgNames, sets);
            dlg.ShowDialog(this);
            if (dlg.Deleted > 0)
            {
                onChanged?.Invoke($"Удалено дубликатов по форме поиска: {dlg.Deleted}");
                marked.Clear();
            }
            else
            {
                foreach (JToken p in dlg.Unmarked) marked.Remove(p);
            }
            Reload();
        }

        /// <summary>Просмотр помеченных на удаление и само удаление.</summary>
        private class MarkedReviewForm : Form
        {
            private readonly HashSet<JToken> marked;
            private readonly JToken race;
            private readonly List<DuplicateSet> sets;
            private readonly DataGridView grid = MakeGrid();
            public int Deleted { get; private set; }
            public List<JToken> Unmarked { get; } = new();

            public MarkedReviewForm(HashSet<JToken> marked, JToken race, Dictionary<string, string> groupNames,
                Dictionary<string, string> orgNames, List<DuplicateSet> sets)
            {
                this.marked = marked;
                this.race = race;
                this.sets = sets;
                Text = "Помеченные на удаление";
                Size = new Size(1100, 600);
                StartPosition = FormStartPosition.CenterParent;

                AddTextColumns(grid, ("Группа", 110), ("Заявка", 80), ("Этап", 45), ("№", 55), ("Фамилия", 130), ("Имя", 110),
                    ("Дата рожд.", 90), ("Клуб", 150), ("Комментарий", 220), ("world_code", 100));
                foreach (JToken p in marked.OrderBy(p => PPSurname(p)).ThenBy(p => PPName(p)))
                {
                    RelayMember rm = DuplicateFinder.ParseRelay(p);
                    int i = grid.Rows.Add(groupNames.TryGetValue(PPGroupId(p) ?? "", out string g) ? g : "?", rm.ApplicationId,
                        rm.Stage > 0 ? rm.Stage.ToString() : "", PPBib(p), PPSurname(p), PPName(p), DuplicateFinder.BirthDate(p),
                        orgNames.TryGetValue(PPOrganizationId(p) ?? "", out string org) ? org : "", PPComment(p), PPWorldCode(p));
                    grid.Rows[i].Tag = p;
                }
                grid.MultiSelect = true;

                var buttonUnmark = new Button { Text = "Снять пометку с выбранных", AutoSize = true };
                var buttonDelete = new Button { Text = "Удалить все помеченные", AutoSize = true };
                var buttonClose = new Button { Text = "Закрыть", AutoSize = true };
                var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(4) };
                panel.Controls.AddRange(new Control[] { buttonUnmark, buttonDelete, buttonClose });
                Controls.Add(grid);
                Controls.Add(panel);

                buttonClose.Click += (_, _) => Close();
                buttonUnmark.Click += (_, _) =>
                {
                    foreach (DataGridViewRow row in grid.SelectedRows.Cast<DataGridViewRow>().ToList())
                    {
                        Unmarked.Add((JToken)row.Tag);
                        this.marked.Remove((JToken)row.Tag);
                        grid.Rows.Remove(row);
                    }
                };
                buttonDelete.Click += (_, _) => Delete();
            }

            private void Delete()
            {
                if (marked.Count == 0) return;
                int noKeeper = sets.Count(s => s.Persons.Any(marked.Contains) && s.Persons.All(marked.Contains));
                string msg = $"Удалить участников: {marked.Count}?\nРезультаты участников не изменяются.";
                if (noKeeper > 0)
                    msg += $"\n\nВНИМАНИЕ: у {noKeeper} наборов будут удалены ВСЕ дубли (никого не останется).";
                if (MessageBox.Show(msg, "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                Deleted = DuplicateFinder.Remove(marked);
                Close();
            }
        }
    }
}
