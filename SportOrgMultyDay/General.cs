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

        Form numbers = new Numbers();
        Form utils = new Utils();
    

        private void buttonOpenNumbers_Click(object sender, EventArgs e)
        {
            if (!numbers.Created)
                numbers = new Numbers();
            numbers.Show();
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonOpenUtils_Click(object sender, EventArgs e)
        {
            if (!utils.Created)
                utils = new Utils();
            utils.Show();
            this.WindowState = FormWindowState.Minimized;
        }

        private void General_Load(object sender, EventArgs e)
        {
            utils.Show();
        }
    }
}
