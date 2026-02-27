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
    public partial class ScoreFragment : UserControl {
        public ScoreFragment() {
            InitializeComponent();
        }

        private void ScoreFragment_Load(object sender, EventArgs e) {

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

            //loadData();
        }

        public class s {
            public string name { get; set; }
            public int id { get; set; }
            public List<string> component { get; set; }
            public List<decimal> score { get; set; }
            public decimal finalScore { get; set; }
        }

        void loadData() {
            var classId = Repo.db.Classes.AsEnumerable().Where(x => x.ClassName == comboBox2.SelectedItem.ToString()).FirstOrDefault().ClassID;
            var sbj = Repo.db.Subjects.AsEnumerable().FirstOrDefault(x => x.SubjectName == comboBox1.SelectedItem.ToString());

            var component = Repo.db.AssessmentComponents.AsEnumerable().Where(x => x.TeachingAssignment.SubjectID == sbj.SubjectID &&
            x.TeachingAssignment.ClassID == classId).ToList();

            var student = Repo.db.Users.Where(x => x.ClassID == classId).ToList();
            var datas = new List<s>();

            for ( var i = 0; i < student.Count; i++ ) {
                var stu = student[i];

                var st = new s {
                    id = stu.UserID,
                    name = stu.FullName,
                    component = new List<string>() { },
                    score = new List<decimal>()
                };

                foreach (var c in component) {
                    st.component.Add(c.ComponentName);
                    var score = stu.StudentScores.Where(x => x.ComponentID == c.ComponentID).FirstOrDefault().Score;

                    st.score.Add((decimal)score);
                }

                var all = st.score.Sum();
                var final = all / st.score.Count;

                st.finalScore = final;

                datas.Add(st);
            }


        }
    }
}
