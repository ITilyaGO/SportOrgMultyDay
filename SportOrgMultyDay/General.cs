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

        Form numbers;
        Form utils;

        private void General_Load(object sender, EventArgs e)
        {
            numbers = new Numbers((Utils)utils);
            utils = new Utils((Numbers)numbers);
            
            utils.Show();
        }
        private void buttonOpenNumbers_Click(object sender, EventArgs e)
        {
            if (!numbers.Created)
                numbers = new Numbers((Utils)utils);
            numbers.Show();
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonOpenUtils_Click(object sender, EventArgs e)
        {
            if (!utils.Created)
                utils = new Utils((Numbers)numbers);
            utils.Show();
            this.WindowState = FormWindowState.Minimized;
        }

      
    }
}
