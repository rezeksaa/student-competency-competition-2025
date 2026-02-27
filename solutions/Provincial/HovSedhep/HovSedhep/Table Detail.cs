using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HovSedhep {
    public partial class Table_Detail : Form {
        int tId = 0;

        public Table_Detail(int tableId) {
            InitializeComponent();
            tId = tableId;
        }

        private void Table_Detail_Load(object sender, EventArgs e) {
            this.Text = "Table Seating Detail - " + Repo.db.RestaurantTables.Find(tId).Name;

            var transaction = Repo.db.Transactions.Where(x => x.TableID == tId && x.Status == "Ongoing").FirstOrDefault();
            var order = Repo.db.Orders.Where(x => x.TransactionID == transaction.TransactionID).FirstOrDefault();

            textBox1.Text = Repo.db.Employees.Find(order.EmployeeID).Name;
            textBox3.Text = transaction.CustomerName;
        }

        private void button1_Click(object sender, EventArgs e) {
            var transaction = Repo.db.Transactions.Where(x => x.TableID == tId && x.Status == "Ongoing").FirstOrDefault();

            transaction.Status = "Completed";

            Repo.db.SaveChanges();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            var transaction = Repo.db.Transactions.Where(x => x.TableID == tId && x.Status == "Ongoing").FirstOrDefault();

            transaction.Status = "Cancelled";

            Repo.db.SaveChanges();
            this.Close();
        }
    }
}
