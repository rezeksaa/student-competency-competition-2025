using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop_App {
    public partial class MainFormVisitor : Form {
        public MainFormVisitor() {
            InitializeComponent();
        }

        private void MainFormVisitor_Load(object sender, EventArgs e) {
            label1.Text = Repo.logged.Username;
            Text = "Museum Bernis Ches Ainstin";
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
            Repo.form.Show();
        }
    }
}
