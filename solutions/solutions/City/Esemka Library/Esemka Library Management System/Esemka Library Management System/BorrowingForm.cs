using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Library_Management_System {
    public partial class BorrowingForm : Form {

        Member user = null;

        public BorrowingForm(Member users) {
            InitializeComponent();

            this.user = users;
        }

        private void BorrowingForm_Load(object sender, EventArgs e) {
            loadData();
        }

        EsemkaLibraryEntities db = new EsemkaLibraryEntities();

        void loadData() {
            this.bookTableAdapter.Fill(this.esemkaLibraryDataSet.Book);

            foreach (DataGridViewRow row in dataGridView1.Rows) {

                var id = (int)row.Cells[0].Value;
                var book = db.Books.Find(id);

                row.Cells[4].Value = book.publish_date != null ? Convert.ToDateTime(book.publish_date).ToString("dd MMMM yyyy") : String.Empty;

                var genre = db.BookGenres.Where(x => x.book_id == id).Select(x => x.Genre.name).ToList();

                row.Cells[2].Value = String.Join(", ", genre);

                if ((int)row.Cells[5].Value != 0) {
                    row.Cells[6].Value = "Borrow";
                    row.DefaultCellStyle.BackColor = Color.White;
                } else {
                    row.Cells[6].Value = String.Empty;
                    row.DefaultCellStyle.BackColor = Color.Red;
                }

            }
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            var input = tbSearch.Text.Trim().ToUpper();

            if (!String.IsNullOrEmpty(input)) {

                CurrencyManager cm = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                cm.SuspendBinding();

                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    if (!row.Cells[1].Value.ToString().ToUpper().Contains(input)) {
                        row.Visible = false;
                    } else {
                        row.Visible = true;
                    }
                }

                cm.ResumeBinding();
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(tbSearch.Text.Trim())) {
                CurrencyManager cm = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                cm.SuspendBinding();

                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    row.Visible = true;
                }

                cm.ResumeBinding();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.ColumnIndex == 6) {

                    Borrowing borrowing = new Borrowing();

                    var book = db.Books.Find((int)dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                    book.stock--;

                    borrowing.member_id = user.id;
                    borrowing.book_id = book.id;
                    borrowing.borrow_date = DateTime.Now;
                    borrowing.created_at = DateTime.Now;

                    db.Borrowings.Add(borrowing);
                    db.SaveChanges();

                    MessageBox.Show($"Success borrow \"{book.title}.\" \n Due date is 7 days from today.", "Notification");
                    this.Close();

                }
            } catch { }
        }
    }
}
