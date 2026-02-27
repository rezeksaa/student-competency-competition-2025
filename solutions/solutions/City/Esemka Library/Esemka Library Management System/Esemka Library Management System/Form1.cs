using Esemka_Library_Management_System.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Library_Management_System {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        EsemkaLibraryEntities db = new EsemkaLibraryEntities();
        List<Borrowing> borrowing = new List<Borrowing>();

        private void Form1_Load(object sender, EventArgs e) {
            lbTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (a, b) => {
                lbTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            };
            timer.Start();

            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();

            var members = db.Members.Select(x => x.name).ToList();

            foreach (var member in members) {
                auto.Add(member);
            }

            tbName.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbName.AutoCompleteCustomSource = auto;

            refr();
        }

        void refr() {
            borrowing.Clear();

            dataGridView1.Rows.Clear();

            user = null;
            btnBorrowing.Enabled = false;
        }

        void loadData() {

            dataGridView1.Rows.Clear();

            if (borrowing.Count > 0) {
                dataGridView1.Rows.Add(borrowing.Count);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows) {
                var borrow = borrowing[row.Index];

                var book = db.Books.Find(borrow.book_id);

                row.Cells[0].Value = borrow.borrow_date;

                row.Cells[1].Value = book.title;
                row.Cells[2].Value = borrow.borrow_date.ToString("dd MMMM yyyy");

                var borrowDate = borrow.borrow_date;
                var dueDate = borrowDate.AddDays(7);

                row.Cells[3].Value = dueDate.ToString("dd MMMM yyyy");

                var dueDays = DateTime.Now - dueDate;

                if (dueDays.Days >= 0) {
                    row.Cells[4].Value = dueDays.Days;
                } else {
                    row.Cells[4].Value = 0;
                }

                if (dueDays.Days > 0) {
                    row.DefaultCellStyle.BackColor = Color.Red;
                } else if (dueDate.Date == DateTime.Now.Date) {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                } else {
                    row.DefaultCellStyle.BackColor = Color.White;
                }

            }

            if (borrowing.Count >= 3) {
                btnBorrowing.Enabled = false;
            } else {
                btnBorrowing.Enabled = true;
            }

        }

        Member user = null;

        private void button1_Click(object sender, EventArgs e) {

            if (String.IsNullOrEmpty(tbName.Text.Trim())) {

                MessageBox.Show("Fill the input box first!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            } else {

                user = db.Members.Where(x => x.name == tbName.Text).FirstOrDefault();

                if (user != null) {

                    borrowing = db.Borrowings.Where(x => x.member_id == user.id && x.return_date == null).ToList();

                    loadData();

                } else {
                    MessageBox.Show("Member with name " + tbName.Text + " is not found!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refr();
                }
            }

        }

        private void btnBorrowing_Click(object sender, EventArgs e) {
            if (user != null) {
                BorrowingForm form = new BorrowingForm(user);
                this.Hide();
                form.ShowDialog();
                this.Show();
                borrowing = db.Borrowings.Where(x => x.member_id == user.id && x.return_date == null).ToList();
                loadData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.ColumnIndex == 5) {
                    var bookTitle = dataGridView1.Rows[e.RowIndex].Cells[1].Value;

                    var borrowingData = borrowing[e.RowIndex];

                    var book = db.Books.Find(borrowingData.book_id);
                    book.stock++;

                    var borrowDate = borrowingData.borrow_date;
                    var dueDate = borrowDate.AddDays(7);
                    var dueDays = DateTime.Now - dueDate;

                    borrowingData.return_date = DateTime.Now.Date;

                    if (dueDays.Days > 0) {
                        var fine = 2000 * dueDays.Days;
                        borrowingData.fine = fine;

                        MessageBox.Show($"Success return \"{bookTitle}.\" \n Member needs to pay fine: {fine.ToString("N0")} IDR.", "Notification");
                    } else {
                        borrowingData.fine = null;

                        MessageBox.Show($"Success return \"{bookTitle}.\"", "Notification");
                    }

                    db.SaveChanges();

                    borrowing = db.Borrowings.Where(x => x.member_id == user.id && x.return_date == null).ToList();
                    loadData();

                }
            } catch { }
        }

        private void tbName_TextChanged(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(tbName.Text)) refr();
        }
    }
}
