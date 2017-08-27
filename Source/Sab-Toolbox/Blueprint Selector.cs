using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sab_Toolbox
{
    public partial class Blueprint_Selector : Form
    {
        public Blueprint_Selector()
        {
            InitializeComponent();
        }

        private void willToFightButton_Click(object sender, EventArgs e)
        {
            WillToFight wtf1 = new WillToFight();
            wtf1.Show();
        }

        private void weaponsButton_Click(object sender, EventArgs e)
        {
            Weapon weapon1 = new Weapon();
            weapon1.Show();
        }

        private void vehiclesButton_Click(object sender, EventArgs e)
        {
            Vehicle vehicle1 = new Vehicle();
            vehicle1.Show();
        }
    }
}
