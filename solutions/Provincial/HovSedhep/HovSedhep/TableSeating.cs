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
    public partial class TableSeating : UserControl {
        public TableSeating() {
            InitializeComponent();
        }

        private void TableSeating_Load(object sender, EventArgs e) {
            getData();
        }

        void getData() {

            var labels = new List<Label>() {
                label1, label2, label4, label3, label7, label8, label5, label6
            };

            var transaction = Repo.db.Transactions.Where(x => x.Status != "Completed" && x.Status != "Cancelled");

            foreach (var lbl in labels) {
                lbl.BackColor = Color.FromName("Control");
            }

            foreach (var tx in transaction) {
                var usedTable = labels[tx.TableID - 1];

                usedTable.BackColor = Color.Yellow;
            }
        }

        private void label1_Click(object sender, EventArgs e) {
            var labels = new List<Label>() {
                label1, label2, label4, label3, label7, label8, label5, label6
            };

            var table = (Label)sender;

            var tId = labels.IndexOf(table) + 1;

            if (table.BackColor == Color.Yellow) {
                Table_Detail td = new Table_Detail(tId);
                td.ShowDialog();
                getData();
            } else {
                Asign_Table td = new Asign_Table(tId);
                td.ShowDialog();
                getData();
            }
        }

        private void TableSeating_VisibleChanged(object sender, EventArgs e) {
            getData();
        }
    }
}
