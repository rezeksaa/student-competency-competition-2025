using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MBCA_Desktop_App {
    public partial class AddExhibit : Form {
        public AddExhibit() {
            InitializeComponent();
        }

        private void AddExhibit_Load(object sender, EventArgs e) {
            this.exhibitCategoryTableAdapter.Fill(this.mBCADataSet.ExhibitCategory);

            if (Repo.selExhibit != null) {
                textBox1.Text = Repo.selExhibit.Name;
                textBox4.Text = Repo.selExhibit.Artist;
                textBox2.Text = Repo.selExhibit.TimePeriod;
                comboBox1.SelectedValue = Repo.selExhibit.ExhibitCategoryID;

                var tagsss = Repo.db.ExhibitTags.Where(x => x.ExhibitID == Repo.selExhibit.ID).ToList();

                foreach (var tag in tagsss) {
                    tags.Add(tag.Tag);
                }

                showData();

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            var input = textBox2.Text;

            var filtered = "";

            foreach (char c in input) {
                if (char.IsDigit(c)) filtered += c;
            }

            textBox2.Text = filtered;
            textBox2.SelectionStart = filtered.Length;
        }

        List<string> tags = new List<string>();
        private void button1_Click(object sender, EventArgs e) {
            if (textBox5.Text.Length > 0) {
                var t = textBox5.Text;

                tags.Add(t);
                textBox5.Clear();
                showData();
            } else {
                MessageBox.Show("Add the name");
            }
        }

        private void showData() {
            dataGridView2.Rows.Clear();

            if (tags.Count > 0) {
                dataGridView2.Rows.Add(tags.Count);
            }

            foreach (DataGridViewRow row in dataGridView2.Rows) {
                row.Cells["Column2"].Value = tags[row.Index];
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == dataGridView2.Columns["Column1"].Index) {
                var t = tags.Where(x => x == dataGridView2.Rows[e.RowIndex].Cells["Column2"].Value.ToString()).FirstOrDefault();

                tags.Remove(t);
                showData();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            Close();
        }

        System.Drawing.Image images = null;
        string imageNames = "";

        private void pictureBox1_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog();

            ofd.Filter = ("Image File(*.jpg)|*.jpg");

            if (ofd.ShowDialog() == DialogResult.OK) {
                images = System.Drawing.Image.FromFile(ofd.FileName);
                var n = ofd.FileName.Split('\\');
                n.Reverse();
                imageNames = n[0];
                showImageData();
            }
        }

        private void showImageData() {
            if (images != null) {
                pictureBox1.Image = images;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if (textBox1.Text.Length == 0 ||
                textBox2.Text.Length == 0 ||
                textBox4.Text.Length == 0) {
                MessageBox.Show("Fill in all the data!");
            } else if (tags.Count == 0) {
                MessageBox.Show("Add a tag!");
            } else if (images == null && Repo.selExhibit == null) {
                MessageBox.Show("Add an image!");
            } else if (Convert.ToInt32(textBox2.Text) > DateTime.Now.Year) {
                MessageBox.Show("Time period cannot be greater than this year");
            } else {
                var ex = Repo.selExhibit == null ? new Exhibit() : Repo.selExhibit;

                ex.Name = textBox1.Text;
                ex.Artist = textBox4.Text;
                ex.TimePeriod = textBox2.Text;
                ex.ExhibitCategoryID = (int)comboBox1.SelectedValue;
                ex.Image = imageNames;

                if (Repo.selExhibit != null) {

                    var tagsss = Repo.db.ExhibitTags.Where(x => x.ExhibitID == Repo.selExhibit.ID).ToList();

                    foreach (var tagss in tagsss) {
                        Repo.db.ExhibitTags.Remove(tagss);
                    }

                }

                if (Repo.selExhibit == null) Repo.db.Exhibits.Add(ex);
                Repo.db.SaveChanges();

                foreach (var t in tags) {
                    var ttt = new ExhibitTag {
                        ExhibitID = ex.ID,
                        Tag = t,
                    };

                    Repo.db.ExhibitTags.Add(ttt);
                }

                Repo.db.SaveChanges();

                MessageBox.Show("Success");
                Close();
            }
        }
    }
}
