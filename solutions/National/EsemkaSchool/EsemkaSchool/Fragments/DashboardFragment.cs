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
    public partial class DashboardFragment : UserControl {
        public DashboardFragment() {
            InitializeComponent();
        }

        public class scores {
            public string name { get; set; }
            public string subject { get; set; }
            public string className { get; set; }
            public decimal score { get; set; }

        }

        private void DashboardFragment_Load(object sender, EventArgs e) {
            loadData();
        }

        void loadData() {

            var guruClass = Repo.db.Classes.Where(x => x.HomeroomTeacherID == Repo.logged.UserID).ToList();
            var clas = Repo.db.Classes.Where(x => x.HomeroomTeacherID == Repo.logged.UserID).FirstOrDefault();
            
            if (clas != null) {
                var student = Repo.db.Users.Where(x => x.ClassID == clas.ClassID && x.Role == "Siswa").ToList();

                var absence = student.Select(x => new {
                    count = Repo.db.Attendances.Where(z => z.User1.UserID == x.UserID).Select(z => z.Status != "Hadir" ? 1 : 0).ToList().Sum(),
                    name = x.FullName,
                }).OrderByDescending(x => x.count).Take(10).ToList();

                dataGridView2.DataSource = absence;
            }

            var subject = Repo.db.TeachingAssignments.Where(x => x.TeacherID == Repo.logged.UserID).Select(x => x.Subject.SubjectName).ToList();

            var score = Repo.db.StudentScores.AsEnumerable().Where(x =>subject.Contains(x.AssessmentComponent.TeachingAssignment.Subject.SubjectName)).Select(x => new {
                name = x.User.FullName,
                subject = x.AssessmentComponent.TeachingAssignment.Subject.SubjectName,
                className = x.User.ClassID.ToString(),
                score = x.Score
            }).ToList();

            var data = new List<scores>();
            var counts = new List<int>();

            foreach (var s in score) {
                if (data.Any(X => X.name == s.name && X.subject == s.subject)) {
                    var d = data.Where(X => X.name == s.name && X.subject == s.subject).FirstOrDefault();
                    var index = data.IndexOf(d);

                    data[index].score += (decimal)s.score;
                    counts[index] += 1;
                } else {
                    var d = new scores {
                        name = s.name,
                        subject = s.subject,
                        className = s.className,
                        score = (decimal)s.score
                    };

                    data.Add(d);
                    counts.Add(1);
                }
            }

            for (int i = 0; i < data.Count; i++) {
                var d = data[i];
                var count = counts[i];

                d.score /= count;
                d.score = Math.Round(d.score, 2);

                var classId = d.className;
                var cl = Repo.db.Classes.Find(Convert.ToInt32(classId));
                d.className  =cl.ClassName;
            }

            data = data.OrderBy(x => x.score).Take(10).ToList();

            dataGridView1.DataSource = data;
        }

        private void DashboardFragment_VisibleChanged(object sender, EventArgs e) {
            loadData();
        }
    }
}
