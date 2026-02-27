using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsemkaSchool {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void meenuToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void MainForm_Load(object sender, EventArgs e) {
            hide();
        }

        void hide() {
            foreach (Control c in flowLayoutPanel1.Controls) {
                c.Visible = false;
            }
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e) {
            hide();

            dashboardFragment1.Visible = true;
            Text = "Dashboard";
        }

        private void attendanceToolStripMenuItem_Click(object sender, EventArgs e) {

            hide();

            attendanceFragment1.Visible = true;
            Text = "Attendance";
        }

        private void assesmentToolStripMenuItem_Click(object sender, EventArgs e) {

            hide();

            assesmentFragment1.Visible = true;
            Text = "Assesment";
        }

        private void scoreToolStripMenuItem_Click(object sender, EventArgs e) {

            hide();

            scoreFragment1.Visible = true;
            Text = "Score";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
            Repo.loginForm.Show();
        }
    }
}
