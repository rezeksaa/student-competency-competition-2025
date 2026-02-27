using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsemkaSchool.Fragments {
    public partial class AttendanceFragment : UserControl {
        public AttendanceFragment() {
            InitializeComponent();
        }

        private void AttendanceFragment_Load(object sender, EventArgs e) {
            var classes = Repo.db.TeachingAssignments.AsEnumerable().Where(x => x.TeacherID == Repo.logged.UserID).Select(x => Repo.db.Classes.Find(x.ClassID).ClassName).ToList();

            foreach (var c in classes) {
                comboBox1.Items.Add(c);
            }

            comboBox1.SelectedIndex = 0;

            getData();
        }

        void getData() {
            try {

                var classId = Repo.db.Classes.AsEnumerable().Where(x => x.ClassName == comboBox1.SelectedItem.ToString()).FirstOrDefault().ClassID;
                var attendance = Repo.db.Attendances.AsEnumerable().Where(x => x.User1.ClassID == classId
                && Convert.ToDateTime(x.Date).Date == dateTimePicker1.Value.Date).Select(x => new {
                    name = x.User1.FullName,
                    status = x.Status,
                }).ToList();

                dataGridView1.DataSource = attendance;
            } catch {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            getData();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {
            getData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            try {
                var input = textBox1.Text.ToUpper();
                var classId = Repo.db.Classes.AsEnumerable().Where(x => x.ClassName == comboBox1.SelectedItem.ToString()).FirstOrDefault().ClassID;

                var attendance = Repo.db.Attendances.AsEnumerable().Where(x => x.User1.FullName.ToUpper().Contains(input) && x.User1.ClassID == classId
                && Convert.ToDateTime(x.Date).Date == dateTimePicker1.Value.Date).Select(x => new {
                    name = x.User1.FullName,
                    status = x.Status,
                }).ToList();

                dataGridView1.DataSource = attendance;
            } catch {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
        }
    }
}
