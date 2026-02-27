using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HovSedhep {
    public partial class Asign_Table : Form {
        int tableId = 0;

        public Asign_Table(int tId) {
            InitializeComponent();

            tableId = tId;
        }

        private void Asign_Table_Load(object sender, EventArgs e) {

            var waiter = Repo.db.Employees.Where(x => x.Role == "Waitress").Select(x => x.Name).ToList();

            foreach (var item in waiter) {
                comboBox1.Items.Add(item);
            }

            comboBox1.SelectedIndex = 0;

            var table = Repo.db.RestaurantTables.Find(tableId);
            this.Text = "Assign Table - " + table.Name;

            numericUpDown1.Maximum = table.Capacity;
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(textBox3.Text.Trim())) {
                MessageBox.Show("Please fill in all the data!");
            } else {
                var table = Repo.db.RestaurantTables.Find(tableId);
                var changed = false;

                if ((int)numericUpDown1.Value <= 2 && table.Capacity != 2) {
                    var changeTable = Repo.db.RestaurantTables.AsEnumerable().Where(x => x.Capacity == 2 && (Repo.db.Transactions.Any(z => z.TableID == x.TableID && z.Status == "Ongoing") == false)).FirstOrDefault();

                    if (changeTable != null) {
                        tableId = changeTable.TableID;
                    } else {
                        changeTable = Repo.db.RestaurantTables.AsEnumerable().Where(x => x.Capacity == 4 && Repo.db.Transactions.Any(z => z.TableID == x.TableID && z.Status == "Ongoing") == false).FirstOrDefault();
                        
                        if (changeTable != null) {
                            tableId = changeTable.TableID;
                            changed = true;
                        }

                    }
                } else if (!changed &&(int)numericUpDown1.Value <= 4 && table.Capacity != 4 && table.Capacity != 2) {
                    var changeTable = Repo.db.RestaurantTables.AsEnumerable().Where(x => x.Capacity == 4 && Repo.db.Transactions.Any(z => z.TableID == x.TableID && z.Status == "Ongoing") == false).FirstOrDefault();

                    if (changeTable != null) {
                        tableId = changeTable.TableID;
                    }
                }

                var transaction = new Transaction() {
                    TableID = tableId,
                    CustomerName = textBox3.Text,
                    TransactionDate = DateTime.Now,
                    Status = "Ongoing"
                };

                Repo.db.Transactions.Add(transaction);

                var employeeId = Repo.db.Employees.AsEnumerable().Where(X => X.Name == comboBox1.SelectedItem.ToString()).FirstOrDefault().EmployeeID;

                var order = new Order() {
                    TransactionID = transaction.TransactionID,
                    EmployeeID = employeeId,
                    OrderTime = DateTime.Now,
                };

                Repo.db.Orders.Add(order);
                Repo.db.SaveChanges();
                this.Close();
            }
        }
    }
}
