using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop_App {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
            Repo.form.Show();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            label1.Text = Repo.logged.Username;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            Text = "Museum Bernis Ches Ainstin";

            var label = label2;

            var labels = new List<Label>() {
                label2,
                label3
            };

            var fragmeent = new List<UserControl> {
                eventsFragment1,
                exhibitFragments1
            };

            for (int i = 0; i < labels.Count; i++) {
                labels[i].BackColor = Color.White;
                fragmeent[i].Visible = false;
            }

            var index = labels.IndexOf(label);
            label.BackColor = Color.FromKnownColor(KnownColor.Control);
            fragmeent[index].Visible = true;
        }

        private void label2_Click(object sender, EventArgs e) {
            var label = (Label)sender;

            var labels = new List<Label>() {
                label2,
                label3
            };

            var fragmeent = new List<UserControl> {
                eventsFragment1,
                exhibitFragments1
            };

            for (int i = 0; i < labels.Count; i++) {
                labels[i].BackColor = Color.White;
                fragmeent[i].Visible = false;
            }

            var index = labels.IndexOf(label);
            label.BackColor = Color.FromKnownColor(KnownColor.Control);
            fragmeent[index].Visible = true;
        }
    }
}
