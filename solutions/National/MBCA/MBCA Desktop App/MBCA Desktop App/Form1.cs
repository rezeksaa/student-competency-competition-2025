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
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (textBox1.Text.Trim().Length == 0 ||
                textBox2.Text.Trim().Length == 0) {
                MessageBox.Show("Fill in all the data!");
            } else {
                Repo.logged = Repo.db.Users.Where(X => X.Username == textBox1.Text && X.Password == textBox2.Text).FirstOrDefault();

                if (Repo.logged == null) {
                    MessageBox.Show("Username or password is incorrect!");
                } else {
                    if(Repo.logged.IsActivated == 1) {
                        if (Repo.logged.RoleID == 2) {
                            MainForm m = new MainForm();
                            this.Hide();
                            m.Show();
                        } else {
                            MainFormVisitor m = new MainFormVisitor();
                            m.Show();
                            this.Hide();
                        }

                        textBox1.Clear();
                        textBox2.Clear();
                    } else {
                        MessageBox.Show("Usere is not activated");
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            Text = "Museum Bernis Ches Ainstin";
            Repo.form = this;
        }
    }
}
