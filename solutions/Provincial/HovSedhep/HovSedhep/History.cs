using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HovSedhep {
    public partial class History : UserControl {
        public History() {
            InitializeComponent();
        }

        private void History_Load(object sender, EventArgs e) {
            loadData();
        }

        void def() {
            try {

                var id = dataGridView1.Rows[0].Cells[0].Value;

                var transaction = Repo.db.Transactions.Find(id);
                var order = Repo.db.Orders.Where(x => x.TransactionID == transaction.TransactionID).ToList();

                dataGridView2.DataSource = order;

                foreach (DataGridViewRow row in dataGridView2.Rows) {
                    var o = order[row.Index];

                    row.Cells[3].Value = Repo.db.Employees.Find(o.EmployeeID).Name;
                    row.Cells[2].Value = Convert.ToDateTime(o.OrderTime).ToString("HH:mm:ss");

                    var od = Repo.db.OrderDetails.Where(x => x.OrderID == o.OrderID).Count();

                    row.Cells[4].Value = od;
                }

                dataGridView3.DataSource = new List<OrderDetail>();

                def2();
            } catch {

                dataGridView1.DataSource = new List<Transaction>();
                dataGridView2.DataSource = new List<Order>();
                dataGridView3.DataSource = new List<OrderDetail>();
            
            }
        }

        void def2() {
            var id = dataGridView2.Rows[0].Cells[1].Value;

            var order = Repo.db.Orders.Find(id);

            var orderDetails = Repo.db.OrderDetails.Where(x => x.OrderID == order.OrderID).ToList();

            dataGridView3.DataSource = orderDetails;

            foreach (DataGridViewRow row in dataGridView3.Rows) {
                var od = orderDetails[row.Index];

                row.Cells[2].Value = Repo.db.MenuItems.Find(od.MenuItemID).Name;
            }
        }


        void loadData() {
            if (textBox1.Text.Trim().Length == 0) {

                var transaction = Repo.db.Transactions.AsEnumerable().Where(x => Convert.ToDateTime(x.TransactionDate).Date == dateTimePicker1.Value.Date).ToList();

                dataGridView1.DataSource = transaction;

                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    var id = (int)row.Cells[0].Value;

                    var tr = Repo.db.Transactions.Find(id);

                    row.Cells[1].Value = Repo.db.RestaurantTables.Find(tr.TableID).Name;
                    row.Cells[3].Value = Convert.ToDateTime(tr.TransactionDate).ToString("dd MMMM yyyy");

                    var o = Repo.db.Orders.Where(x => x.TransactionID == tr.TransactionID).ToList();

                    if (o.Count > 0) {
                        var thisTotal = 0m;

                        foreach (var order in o) {

                            thisTotal += Repo.db.OrderDetails.AsEnumerable().Where(x => x.OrderID == order.OrderID).Select(x => x.Price * x.Quantity).ToList().Sum();

                        }

                        row.Cells[4].Value = "Rp" + thisTotal.ToString(CultureInfo.InvariantCulture);
                    } else {
                        row.Cells[4].Value = "Rp0";
                    }
                }
            } else{

                var transaction = Repo.db.Transactions.AsEnumerable().Where(x => Convert.ToDateTime(x.TransactionDate).Date == dateTimePicker1.Value.Date && Repo.db.RestaurantTables.Find(x.TableID).Name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();

                dataGridView1.DataSource = transaction;

                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    var id = (int)row.Cells[0].Value;

                    var tr = Repo.db.Transactions.Find(id);

                    row.Cells[1].Value = Repo.db.RestaurantTables.Find(tr.TableID).Name;
                    row.Cells[3].Value = Convert.ToDateTime(tr.TransactionDate).ToString("dd MMMM yyyy");

                    var o = Repo.db.Orders.Where(x => x.TransactionID == tr.TransactionID).ToList();

                    if (o.Count > 0) {
                        var thisTotal = 0m;

                        foreach (var order in o) {

                            thisTotal += Repo.db.OrderDetails.AsEnumerable().Where(x => x.OrderID == order.OrderID).Select(x => x.Price * x.Quantity).ToList().Sum();

                        }

                        row.Cells[4].Value = "Rp" + thisTotal.ToString(CultureInfo.InvariantCulture);
                    } else {
                        row.Cells[4].Value = "Rp0";
                    }
                }
            }

            def();
        }

        private void History_VisibleChanged(object sender, EventArgs e) {
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            loadData();
        }

        private void button1_Click(object sender, EventArgs e) {
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                var id = dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                var transaction = Repo.db.Transactions.Find(id);
                var order = Repo.db.Orders.Where(x => x.TransactionID == transaction.TransactionID).ToList();

                dataGridView2.DataSource = order;

                foreach (DataGridViewRow row in dataGridView2.Rows) {
                    var o = order[row.Index];

                    row.Cells[3].Value = Repo.db.Employees.Find(o.EmployeeID).Name;
                    row.Cells[2].Value = Convert.ToDateTime(o.OrderTime).ToString("HH:mm:ss");

                    var od = Repo.db.OrderDetails.Where(x => x.OrderID == o.OrderID).Count();

                    row.Cells[4].Value = od;
                }

                dataGridView3.DataSource = new List<OrderDetail>();

                def3(order[0].OrderID);
            } catch { }
        }

        void def3(int id) {
            var order = Repo.db.Orders.Find(id);

            var orderDetails = Repo.db.OrderDetails.Where(x => x.OrderID == order.OrderID).ToList();
                
            dataGridView3.DataSource = orderDetails;

            foreach (DataGridViewRow row in dataGridView3.Rows) {
                var od = orderDetails[row.Index];

                row.Cells[2].Value = Repo.db.MenuItems.Find(od.MenuItemID).Name;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                var id = dataGridView2.Rows[e.RowIndex].Cells[1].Value;

                var order = Repo.db.Orders.Find(id);

                var orderDetails = Repo.db.OrderDetails.Where(x => x.OrderID == order.OrderID).ToList();

                dataGridView3.DataSource = orderDetails;

                foreach (DataGridViewRow row in dataGridView3.Rows) {
                    var od = orderDetails[row.Index];

                    row.Cells[2].Value = Repo.db.MenuItems.Find(od.MenuItemID).Name;
                }
            } catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
            try {
                var id = dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                var transaction = Repo.db.Transactions.Find(id);
                var order = Repo.db.Orders.Where(x => x.TransactionID == transaction.TransactionID).ToList();

                dataGridView2.DataSource = order;

                foreach (DataGridViewRow row in dataGridView2.Rows) {
                    var o = order[row.Index];

                    row.Cells[3].Value = Repo.db.Employees.Find(o.EmployeeID).Name;
                    row.Cells[2].Value = Convert.ToDateTime(o.OrderTime).ToString("HH:mm:ss");

                    var od = Repo.db.OrderDetails.Where(x => x.OrderID == o.OrderID).Count();

                    row.Cells[4].Value = od;
                }

                def3(order[0].OrderID);
            } catch { }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) {
            try {
                var id = dataGridView2.Rows[e.RowIndex].Cells[1].Value;

                var order = Repo.db.Orders.Find(id);

                var orderDetails = Repo.db.OrderDetails.Where(x => x.OrderID == order.OrderID).ToList();

                dataGridView3.DataSource = orderDetails;

                foreach (DataGridViewRow row in dataGridView3.Rows) {
                    var od = orderDetails[row.Index];

                    row.Cells[2].Value = Repo.db.MenuItems.Find(od.MenuItemID).Name;
                }
            } catch { }
        }
    }
}
