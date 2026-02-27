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
    public partial class Menu : UserControl {
        public Menu() {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e) {
            var category = Repo.db.Categories.ToList();

            comboBox1.Items.Add("All");

            foreach (var item in category) {
                comboBox1.Items.Add(item.Name);
            }

            comboBox1.SelectedIndex = 0;

            loadData();
        }

        void loadData() {

            if (comboBox1.SelectedIndex == 0) {
                if (textBox1.Text.Trim().Length == 0) {
                    var menu = Repo.db.MenuItems.ToList();

                    dataGridView1.DataSource = menu;

                    foreach (DataGridViewRow row in dataGridView1.Rows) {
                        var menuId = (int)row.Cells[0].Value;

                        var selMenu = Repo.db.MenuItems.Find(menuId);
                        var category = Repo.db.Categories.Find(selMenu.CategoryID).Name;

                        row.Cells[1].Value = category;
                    }
                } else {

                    var menu = Repo.db.MenuItems.AsEnumerable().Where(x => x.Name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();

                    dataGridView1.DataSource = menu;

                    foreach (DataGridViewRow row in dataGridView1.Rows) {
                        var menuId = (int)row.Cells[0].Value;

                        var selMenu = Repo.db.MenuItems.Find(menuId);
                        var category = Repo.db.Categories.Find(selMenu.CategoryID).Name;

                        row.Cells[1].Value = category;
                    }
                }
            } else {
                if (textBox1.Text.Trim().Length == 0) {

                    var selCat = comboBox1.SelectedItem;

                    if (selCat != null) {
                        var categoryId = Repo.db.Categories.AsEnumerable().Where(x => x.Name == selCat.ToString()).FirstOrDefault().CategoryID;

                        var menu = Repo.db.MenuItems.Where(x => x.CategoryID == categoryId).ToList();

                        dataGridView1.DataSource = menu;

                        foreach (DataGridViewRow row in dataGridView1.Rows) {
                            var menuId = (int)row.Cells[0].Value;

                            var selMenu = Repo.db.MenuItems.Find(menuId);
                            var category = Repo.db.Categories.Find(selMenu.CategoryID).Name;

                            row.Cells[1].Value = category;
                        }
                    }

                } else {
                    var selCat = comboBox1.SelectedItem;

                    if (selCat != null) {
                        var categoryId = Repo.db.Categories.AsEnumerable().Where(x => x.Name == selCat.ToString()).FirstOrDefault().CategoryID;

                        var menu = Repo.db.MenuItems.Where(x => x.Name.ToUpper().Contains(textBox1.Text.ToUpper()) && x.CategoryID == categoryId).ToList();

                        dataGridView1.DataSource = menu;

                        foreach (DataGridViewRow row in dataGridView1.Rows) {
                            var menuId = (int)row.Cells[0].Value;

                            var selMenu = Repo.db.MenuItems.Find(menuId);
                            var category = Repo.db.Categories.Find(selMenu.CategoryID).Name;

                            row.Cells[1].Value = category;
                        }
                    }

                }
            }
        }

        private void Menu_VisibleChanged(object sender, EventArgs e) {
            comboBox1.SelectedIndex = 0;
            textBox1.Clear();
            loadData();
        }

        private void button1_Click(object sender, EventArgs e) {
            loadData();
        }
    }
}
