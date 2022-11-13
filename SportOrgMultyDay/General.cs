using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportOrgMultyDay
{
    public partial class General : Form
    {
        public General()
        {
            InitializeComponent();
        }

        public Form numbers;
        public Form utils;

        private void General_Load(object sender, EventArgs e)
        {
            numbers = new Numbers((Utils)utils,this);
            utils = new Utils((Numbers)numbers,this);
            
            //utils.Show();
        }
        private void buttonOpenNumbers_Click(object sender, EventArgs e)
        {
            showNumbers();
           // this.WindowState = FormWindowState.Minimized;
        }

        public void showNumbers()
        {
            if (!numbers.Created)
                numbers = new Numbers((Utils)utils, this);
            numbers.Show();
            Hide();
        }

        public void ShowIfAllClosed(bool hideUtils = false, bool hideNumbers = false)
        {
            if ((!utils.Visible || hideUtils) && (!numbers.Visible || hideNumbers))
                Show();
        }

        private void buttonOpenUtils_Click(object sender, EventArgs e)
        {
            if (!utils.Created)
                utils = new Utils((Numbers)numbers,this);
            utils.Show();
            Hide();
            //this.WindowState = FormWindowState.Minimized;
        }

      
    }
}
