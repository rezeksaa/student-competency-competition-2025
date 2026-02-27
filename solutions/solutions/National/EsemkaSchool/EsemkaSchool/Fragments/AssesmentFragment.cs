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
    public partial class AssesmentFragment : UserControl {
        public AssesmentFragment() {
            InitializeComponent();
        }

        private void AssesmentFragment_Load(object sender, EventArgs e) {
            button5.Enabled = false;
            button4.Enabled = false;

            var subject = Repo.db.TeachingAssignments.Where(x => x.TeacherID == Repo.logged.UserID).Select(x => x.Subject.SubjectName).ToList();
            subject = subject.Distinct().ToList();

            foreach (var item in subject) {
                comboBox1.Items.Add(item);
            }

            comboBox1.SelectedIndex = 0;

            var classes = Repo.db.TeachingAssignments.AsEnumerable().Where(x => x.TeacherID == Repo.logged.UserID).Select(x => Repo.db.Classes.Find(x.ClassID).ClassName).ToList();

            foreach (var c in classes) {
                comboBox2.Items.Add(c);
            }

            comboBox2.SelectedIndex = 0;

            loadData();
        }

        void loadData() {
            isEditing = false;
            textBox3.Enabled = false;
            numericUpDown2.Enabled = false;

            textBox2.Text = (Repo.db.AssessmentComponents.Count() + 1).ToString();
            var classId = Repo.db.Classes.AsEnumerable().Where(x => x.ClassName == comboBox2.SelectedItem.ToString()).FirstOrDefault().ClassID;
            var sbj = Repo.db.Subjects.AsEnumerable().FirstOrDefault(x => x.SubjectName == comboBox1.SelectedItem.ToString());

            var component = Repo.db.AssessmentComponents.AsEnumerable().Where(x => x.TeachingAssignment.SubjectID == sbj.SubjectID && 
            x.TeachingAssignment.ClassID == classId).ToList();

            var c = component.Select(x => new {
                id = x.ComponentID,
                name = x.ComponentName,
                weight = x.Weight,
            }).ToList();
            assignmentId = component.FirstOrDefault().TeachingAssignment.AssignmentID;

            dataGridView1.DataSource = c;

            decimal curWeight = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows) {
                curWeight += (decimal)row.Cells[2].Value;
            }

            numericUpDown1.Value = curWeight;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox2.Items.Count > 0) {
                loadData();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {

            if (comboBox2.Items.Count > 0) {
                loadData();
            }
        }

        private void AssesmentFragment_VisibleChanged(object sender, EventArgs e) {
            if (comboBox2.Items.Count > 0) {
                loadData();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            button5.Enabled = true;
            button4.Enabled = true;
            textBox3.Enabled = true;
            numericUpDown2.Enabled = true;
            textBox2.Text = (Repo.db.AssessmentComponents.Count() + 1).ToString();
            textBox3.Clear();
            numericUpDown2.Value = 1;
        }

        private void button4_Click(object sender, EventArgs e) {
            button5.Enabled = false;
            button4.Enabled = false;
            textBox2.Text = (Repo.db.AssessmentComponents.Count() + 1).ToString();
            textBox3.Clear();
            numericUpDown2.Value = 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

            textBox3.Enabled = false;
            numericUpDown2.Enabled = false;
            var classId = Repo.db.Classes.AsEnumerable().Where(x => x.ClassName == comboBox2.SelectedItem.ToString()).FirstOrDefault().ClassID;
            var sbj = Repo.db.Subjects.AsEnumerable().FirstOrDefault(x => x.SubjectName == comboBox1.SelectedItem.ToString());

            var component = Repo.db.AssessmentComponents.AsEnumerable().Where(x => x.TeachingAssignment.SubjectID == sbj.SubjectID &&
            x.TeachingAssignment.ClassID == classId).ToList();

            var c = component.Select(x => new {
                id = x.ComponentID,
                name = x.ComponentName,
                weight = x.Weight,
            }).Where(X => X.name.ToUpper().Contains(textBox1.Text.ToUpper())).ToList();

            assignmentId = component.FirstOrDefault().TeachingAssignment.AssignmentID;

            dataGridView1.DataSource = c;

            decimal curWeight = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows) {
                curWeight += (decimal)row.Cells[2].Value;
            }

            if (curWeight >= numericUpDown1.Minimum) {
                numericUpDown1.Value = curWeight;
            } else {
                numericUpDown1.Value = 1;
            }
        }

        int assignmentId = 0;

        private void button5_Click(object sender, EventArgs e) {
            if (textBox3.Text.Trim().Length == 0) {
                MessageBox.Show("fill in all the data!");
            } else {

                var weight = numericUpDown1.Value;

                var curWeight = numericUpDown2.Value;

                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    curWeight += (decimal)row.Cells[2].Value;
                }

                if (isEditing) {
                    var comp = Repo.db.AssessmentComponents.Find(selId);
                    curWeight -= (decimal)comp.Weight;
                }

                if (curWeight >= weight) {
                    MessageBox.Show("Too much weight!");
                } else {
                    if (!isEditing) {

                        var component = new AssessmentComponent {
                            AssignmentID = assignmentId,
                            ComponentName = textBox3.Text,
                            Weight = numericUpDown2.Value,
                        };

                        Repo.db.AssessmentComponents.Add(component);
                        Repo.db.SaveChanges();
                        textBox1.Clear();

                        MessageBox.Show("Success");
                        loadData();
                        button5.Enabled = false;
                        button4.Enabled = false;
                        textBox2.Text = (Repo.db.AssessmentComponents.Count() + 1).ToString();
                        textBox3.Clear();
                        numericUpDown2.Value = 1;
                    } else {

                        var comp = Repo.db.AssessmentComponents.Find(selId);

                        comp.ComponentName = textBox3.Text;
                        comp.Weight = numericUpDown2.Value;

                        Repo.db.SaveChanges();
                        textBox1.Clear();

                        MessageBox.Show("Success");
                        loadData();
                        button5.Enabled = false;
                        button4.Enabled = false;
                        textBox2.Text = (Repo.db.AssessmentComponents.Count() + 1).ToString();
                        textBox3.Clear();
                        numericUpDown2.Value = 1;

                    }
                }

            }
        }

        bool isEditing = false;
        int selId = 0;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            try {
                selId = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                var comp = Repo.db.AssessmentComponents.Find(selId);

                textBox2.Text = comp.ComponentID.ToString();
                textBox3.Text = comp.ComponentName.ToString();
                numericUpDown2.Value = (decimal)comp.Weight;

            } catch {
                selId = 0;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
            try {
                selId = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                var comp = Repo.db.AssessmentComponents.Find(selId);

                textBox2.Text = comp.ComponentID.ToString();
                textBox3.Text = comp.ComponentName.ToString();
                numericUpDown2.Value = (decimal)comp.Weight;

            } catch {
                selId = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e) {

            button5.Enabled = true;
            button4.Enabled = true;
            textBox3.Enabled = true;
            numericUpDown2.Enabled = true;
            isEditing = true;
        }

        private void button3_Click(object sender, EventArgs e) {
            if (selId !=0 ) {

                var comp = Repo.db.AssessmentComponents.Find(selId);

                Repo.db.AssessmentComponents.Remove(comp);
                Repo.db.SaveChanges();
                loadData();

            } else {
                MessageBox.Show("Select the component first!");
            }
        }
    }
}
