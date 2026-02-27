using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HovSedhep {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            change(button1);
        }

        void change(Button btn) {
            List<Button> btns = new List<Button>{ button1, button2, button3 };
            UserControl[] u = { tableSeating1, menu1, history1 };

            var index = btns.IndexOf(btn);
            
            foreach (Control c in flowLayoutPanel1.Controls) {
                c.Visible = false;
            }

            foreach (Control c in panel1.Controls) {
                if (c is Button button) {
                    button.BackColor = Color.FromName("Control");
                    button.ForeColor = Color.Black;
                }
            }

            btn.BackColor = Color.Blue;
            btn.ForeColor = Color.White;

            u[index].Visible = true;
        }

        private void button1_Click(object sender, EventArgs e) {
            change((Button)sender);
        }
    }
}
