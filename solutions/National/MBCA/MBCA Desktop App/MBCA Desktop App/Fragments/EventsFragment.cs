using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Metadata.Edm;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop_App.Fragments {
    public partial class EventsFragment : UserControl {
        public EventsFragment() {
            InitializeComponent();
        }

        private void EventsFragment_Load(object sender, EventArgs e) {
            loadData();

        }

        void loadData() {

            var events = Repo.db.Events.AsEnumerable().Select(x => new {
                x.ID,
                x.Title,
                x.Description,
                x.Location,
                x.Initiator,
                Price = x.Price.ToString(CultureInfo.InvariantCulture) + "$",
                category = Repo.db.EventCategories.Find(x.EventCategoryID).Name,
                dateTime = x.Date.ToString("dd MM yyyy") + " " + x.StartTime.ToString(),

            }).ToList();

            dataGridView1.DataSource = events;
        }

        int selId = 0;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (dataGridView1.Columns["Column3"].Index == e.ColumnIndex) {
                selId = (int)dataGridView1.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value;

                var events = Repo.db.Events.Find(selId);
                Repo.selEvent = events;

                AddEvent a = new AddEvent();
                a.ShowDialog();
                loadData();
                dataGridView2.DataSource = null;
            } else if (dataGridView1.Columns["c"].Index == e.ColumnIndex) {

                if (MessageBox.Show("Are you sure want to delete this?", "Confirm", MessageBoxButtons.YesNo)== DialogResult.Yes) {

                    selId = (int)dataGridView1.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value;

                    var events = Repo.db.Events.Find(selId);

                    Repo.db.Events.Remove(events);

                    var banners = Repo.db.EventBanners.Where(X => X.EventID == events.ID).ToList();

                    foreach (var banner in banners) {
                        Repo.db.EventBanners.Remove(banner);
                    }

                    var allex = Repo.db.EventExhibits.Where(X => X.EventID == events.ID).ToList();

                    foreach (var a in allex) {
                        Repo.db.EventExhibits.Remove(a);
                    }

                    var tickets = Repo.db.Tickets.Where(x => x.EventID == events.ID).ToList();

                    foreach (var t in tickets) {
                        Repo.db.Tickets.Remove(t);
                    }

                    Repo.db.SaveChanges();
                    loadData();
                    dataGridView2.DataSource = null;
                }
            } else {
                try {
                    selId = (int)dataGridView1.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value;

                    var events = Repo.db.Events.Find(selId);

                    var exhibitsC = Repo.db.EventExhibits.Where(x => x.EventID == selId).Select(x => x.ExhibitID).ToList();

                    var exhibits = new List<Exhibit>();

                    foreach (var exhib in exhibitsC) {
                        var ex = Repo.db.Exhibits.Find(exhib);
                        exhibits.Add(ex);
                    }

                    var format = exhibits.Select(x => new {
                        x.ID,
                        x.Name,
                        x.Artist,
                        category = Repo.db.ExhibitCategories.Find(x.ExhibitCategoryID).Name,
                        x.TimePeriod,
                        tags = String.Join(", ", Repo.db.ExhibitTags.Where(z => z.ExhibitID == x.ID).ToList().Select(z => z.Tag)),
                    }).ToList();

                    dataGridView2.DataSource = format;
                } catch {

                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
            try {
                selId = (int)dataGridView1.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn"].Value;
                var events = Repo.db.Events.Find(selId);
                var exhibitsC = Repo.db.EventExhibits.Where(x => x.EventID == selId).Select(x => x.ExhibitID).ToList();
                var exhibits = new List<Exhibit>();

                foreach (var exhib in exhibitsC) {
                    var ex = Repo.db.Exhibits.Find(exhib);
                    exhibits.Add(ex);
                }

                var format = exhibits.Select(x => new {
                    x.ID,
                    x.Name,
                    x.Artist,
                    category = Repo.db.ExhibitCategories.Find(x.ExhibitCategoryID).Name,
                    x.TimePeriod,
                    tags = string.Join(", ", Repo.db.ExhibitTags.Where(z => z.ExhibitID == x.ID).ToList().Select(z => z.Tag)),
                }).ToList();

                dataGridView2.DataSource = format;
            } catch {

            }
        }

        private void button1_Click(object sender, EventArgs e) {
            AddEvent a = new AddEvent();
            a.ShowDialog();
            loadData();
            dataGridView2.DataSource = null;
        }
    }
}
