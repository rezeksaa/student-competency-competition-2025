using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop_App.Fragments {
    public partial class ExhibitFragments : UserControl {
        public ExhibitFragments() {
            InitializeComponent();
        }

        private void ExhibitFragments_Load(object sender, EventArgs e) {
            loadData();
        }

        void loadData() {
            var exs = Repo.db.Exhibits.ToList();

            var ex = exs.Select(x => new {
                x.ID,
                x.Name,
                x.Artist,
                category = Repo.db.ExhibitCategories.Find(x.ExhibitCategoryID).Name,
                x.TimePeriod,
                tags = String.Join(", ", Repo.db.ExhibitTags.Where(z => z.ExhibitID == x.ID).ToList().Select(z => z.Tag)),
            }).ToList();

            dataGridView1.DataSource = ex;
        }

        private void button1_Click(object sender, EventArgs e) {
            AddExhibit a = new AddExhibit();
            a.ShowDialog(); 
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == dataGridView1.Columns["c"].Index) {
                if (MessageBox.Show("Are you sure want to delete this?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    var selId = (int)dataGridView1.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value;

                    var exhibit = Repo.db.Exhibits.Find(selId);

                    var eventExhibit = Repo.db.EventExhibits.Where(X => X.ExhibitID == selId).ToList();
                    var eventTags = Repo.db.ExhibitTags.Where(x => x.ExhibitID == selId).ToList();

                    foreach ( var tag in eventTags ) {
                        Repo.db.ExhibitTags.Remove(tag);
                    }

                    foreach ( var tag in eventExhibit ) {
                        Repo.db.EventExhibits.Remove(tag);
                    }

                    Repo.db.Exhibits.Remove(exhibit);
                    Repo.db.SaveChanges();
                    loadData();
                }
            } else if (e.ColumnIndex == dataGridView1.Columns["Column3"].Index) {
                var selId = (int)dataGridView1.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value;

                var exhibit = Repo.db.Exhibits.Find(selId);
                Repo.selExhibit = exhibit;

                AddExhibit a = new AddExhibit();
                a.ShowDialog();
                loadData();
            }
        }
    }
}
