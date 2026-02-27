using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop_App {
    public partial class AddEvent : Form {
        public AddEvent() {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        List<Image> images = new List<Image>();
        List<string> imageNames = new List<string>();

        private void AddEvent_Load(object sender, EventArgs e) {

            if (Repo.selEvent != null) {
                textBox1.Text = Repo.selEvent.Title;
                textBox2.Text = Repo.selEvent.Initiator;
                textBox3.Text = Repo.selEvent.Description;
                textBox4.Text = Repo.selEvent.Location;
                comboBox1.SelectedValue = Repo.selEvent.EventCategoryID;

                var exhibitt = Repo.db.EventExhibits.Where(X => X.EventID == Repo.selEvent.ID).ToList();

                foreach (var exhib in exhibitt) {
                    var ex = Repo.db.Exhibits.Find(exhib.ExhibitID);
                    exhibits.Add(ex);
                }

                showData();
            }

            this.eventCategoryTableAdapter.Fill(this.mBCADataSet.EventCategory);

            var exhibit = Repo.db.Exhibits.Select(X => X.Name).ToList();

            var auto = new AutoCompleteStringCollection();

            foreach (var item in exhibit) {
                auto.Add(item);
            }

            textBox5.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox5.AutoCompleteCustomSource = auto;

        }

        private void button3_Click(object sender, EventArgs e) {
            if (exhibits.Count == 0) {
                MessageBox.Show("Add atleast one exhibit!");
            } else if (textBox1.Text.Trim().Length == 0 ||
                textBox2.Text.Trim().Length == 0 ||
                textBox3.Text.Trim().Length == 0 ||
                textBox4.Text.Trim().Length == 0 ||
                maskedTextBox1.Text.Length < 3 ||
                maskedTextBox2.Text.Length < 3
                ) {
                MessageBox.Show("Fill in all the data!");
            } else if (dateTimePicker1.Value.Date < DateTime.Now.Date) {
                MessageBox.Show("This date is already passed");
            } else {
                var hourStart = 0;
                var hourEnd = 0;
                var minuteStart = 0;
                var minuteEnd = 0;

                try {
                    hourStart = Convert.ToInt32(maskedTextBox1.Text.Split(':')[0]);
                    hourEnd = Convert.ToInt32(maskedTextBox2.Text.Split(':')[0]);
                    minuteStart = Convert.ToInt32(maskedTextBox1.Text.Split(':')[1]);
                    minuteEnd = Convert.ToInt32(maskedTextBox2.Text.Split(':')[1]);

                    if (hourStart > 24 || hourEnd > 24) {
                        MessageBox.Show("Invalid hour");
                    } else if (minuteStart > 59 || minuteEnd > 59) {
                        MessageBox.Show("Invalid minute");
                    } else if (images.Count == 0 && Repo.selEvent == null) {
                        MessageBox.Show("Atleast add one banner" );
                    } else {

                        var events = Repo.selEvent == null ? new Event() : Repo.selEvent;

                        var minuteAllStart = minuteStart + (hourStart * 60);
                        var minuteAllEnd = minuteEnd + (hourEnd * 60);

                        events.Title = textBox1.Text;
                        events.Description = textBox3.Text;
                        events.Initiator = textBox2.Text;
                        events.Price = numericUpDown1.Value;
                        events.Date = dateTimePicker1.Value.Date;
                        events.EventCategoryID = (int)comboBox1.SelectedValue;
                        events.StartTime = TimeSpan.FromMinutes(minuteAllStart);
                        events.EndTime = TimeSpan.FromMinutes(minuteAllEnd);
                        events.Location = textBox4.Text;

                        if (Repo.selEvent != null) {
                            var allex = Repo.db.EventExhibits.Where(X => X.EventID == Repo.selEvent.ID).ToList();

                            foreach (var a in allex) {
                                Repo.db.EventExhibits.Remove(a);
                            }
                        }

                        if (Repo.selEvent == null) Repo.db.Events.Add(events);
                        Repo.db.SaveChanges();

                        for (int i = 0; i < images.Count; i++) {
                            var img = new EventBanner {
                                EventID = events.ID,
                                Banner = imageNames[i],
                            };

                            Repo.db.EventBanners.Add(img);
                        }

                        foreach (var ex in exhibits) {
                            var exhibEvent = new EventExhibit {
                                EventID = events.ID,
                                ExhibitID = ex.ID,
                            };

                            Repo.db.EventExhibits.Add(exhibEvent);
                        }

                        Repo.db.SaveChanges();
                        MessageBox.Show("Success");
                        Repo.selEvent = null;
                        Close();

                    }

                } catch (Exception err) {
                    MessageBox.Show("Invalid time!");
                    MessageBox.Show(err.Message);
                }
            }
        }

        List<Exhibit> exhibits = new List<Exhibit>();

        private void button1_Click(object sender, EventArgs e) {
            var input = textBox5.Text;

            if (!Repo.db.Exhibits.Any(x => x.Name == input)) {
                MessageBox.Show("Exhibit not found!");
            } else {
                var exhibit = Repo.db.Exhibits.Where(x => x.Name == input).FirstOrDefault();

                exhibits.Add(exhibit);

                showData();
                textBox5.Clear();
            }

        }

        private void showData() {
            var ex = exhibits.Select(x => new {
                x.ID,
                x.Name,
                x.Artist,
                category = Repo.db.ExhibitCategories.Find(x.ExhibitCategoryID).Name,
            }).ToList();

            dataGridView2.DataSource = ex;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == dataGridView2.Columns["Column1"].Index) {
                var id = (int)dataGridView2.Rows[e.RowIndex].Cells["iDDataGridViewTextBoxColumn1"].Value;

                var delete = exhibits.Where((x) => x.ID == id).FirstOrDefault();
                exhibits.Remove(delete);
                showData();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog();

            ofd.Filter = ("Image File(*.jpg)|*.jpg");

            if (ofd.ShowDialog() == DialogResult.OK) {
                images.Add(Image.FromFile(ofd.FileName));
                var n = ofd.FileName.Split('\\');
                n.Reverse();
                imageNames.Add(n[0]);
                showImageData();
            }
        }

        int curImageIndex = 0;

        private void showImageData() {
            if (images.Count > 0) {
                pictureBox1.Image = images[curImageIndex];
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            if (images.Count > 0) {
                if (curImageIndex != images.Count - 1) {
                    curImageIndex++;
                    showImageData();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            if (images.Count > 0) {
                if (curImageIndex != 0) {
                    curImageIndex--;
                    showImageData();
                }
            }
        }
    }
}
