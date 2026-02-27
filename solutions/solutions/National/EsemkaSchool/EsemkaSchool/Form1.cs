using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsemkaSchool {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (textBox1.Text.Trim().Length == 0 || textBox2.Text.Trim().Length == 0) {
                MessageBox.Show("Fill in all the data!");
            } else {
                try {
                    var mail = new MailAddress(textBox1.Text);

                    Repo.logged = Repo.db.Users.Where(x => x.Email == textBox1.Text && x.PasswordHash == textBox2.Text).FirstOrDefault();

                    if (Repo.logged == null) {
                        MessageBox.Show("Email or password is incorrect!");
                    } else if (Repo.logged.Role != "Guru") {
                        MessageBox.Show("Only guru can login to the application");
                    } else {
                        MainForm m = new MainForm();
                        this.Hide();
                        m.Show();

                        textBox1.Clear();
                        textBox2.Clear();
                    }

                } catch {
                    MessageBox.Show("Email is not in valid format!");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            Repo.loginForm = this;
        }
    }
}
