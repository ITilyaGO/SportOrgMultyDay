using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportOrgMultyDay
{
    public partial class General : Form
    {
        //TODO: Удалить лишние формы, оставить только утилиты
        public General()
        {
            InitializeComponent();
            //this.Load += (sender, e) =>
            //{
            //    this.BeginInvoke((MethodInvoker)delegate
            //    {
            //        this.Hide();
            //    });
            //};
        }

        public Form numbers;
        public Form utils;

        private void General_Load(object sender, EventArgs e)
        {
            numbers = new Numbers((Utils)utils, this);
            utils = new Utils((Numbers)numbers, this);

            //utils.Show();
        }
        private void buttonOpenNumbers_Click(object sender, EventArgs e)
        {
            ShowNumbers();
            // this.WindowState = FormWindowState.Minimized;
        }

        public void ShowNumbers()
        {
            if (!numbers.Created)
                numbers = new Numbers((Utils)utils, this);
            numbers.Show();
            Hide();
        }
        public void ShowUtils()
        {
            if (!utils.Created)
                utils = new Utils((Numbers)numbers, this);
            utils.Show();
            Hide();

        }

        public void ShowIfAllClosed(bool hideUtils = false, bool hideNumbers = false)
        {
            if ((!utils.Visible || hideUtils) && (!numbers.Visible || hideNumbers))
                Show();
        }

        private void buttonOpenUtils_Click(object sender, EventArgs e)
        {
            ShowUtils();
            //this.WindowState = FormWindowState.Minimized;
        }

        private void General_Shown(object sender, EventArgs e)
        {
            ShowUtils();
        }
    }
}
