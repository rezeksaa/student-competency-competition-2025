using Esemka_Vote.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Vote {
    public partial class ReportForm : Form {
        public ReportForm() {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        EsemkaVoteEntities db = null;
        List<VotingHeader> headers = null;

        private void ReportForm_Load(object sender, EventArgs e) {
            db = new EsemkaVoteEntities();
            this.votingHeaderTableAdapter.Fill(this.esemkaVoteDataSet.VotingHeader);

            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;

            headers = db.VotingHeaders.ToList();

            foreach (var header in headers) {
                if (header.EndDate <= DateTime.Now) {
                    comboBox1.Items.Add(header.Name);
                }
            }

            comboBox1.SelectedIndex = 0;
            change();
        }

        void change() {

            if (comboBox1.SelectedIndex != -1) {
                var voteHeaderId = headers[comboBox1.SelectedIndex].Id;

                var voteHeader = db.VotingHeaders.Find(voteHeaderId);

                lbTitle.Text = voteHeader.Name;
                lbDesc.Text = voteHeader.Description;

                var candidates = db.VotingCandidates.Where(x => x.VotingHeaderId == voteHeaderId).ToList();
                List<int> divisionId = new List<int>();
                List<int> count = new List<int>();

                int qtyVote = 0;

                var reasons = new List<String>();
                var votePerDivision = new List<int>();

                foreach (var candidate in candidates) {

                    var voted = db.VotingDetails.Where(x => x.VotedCandidateId == candidate.Id).ToList();

                    qtyVote += voted.Count;

                    count.Add(voted.Count);
                }


                VotingCandidate winnerCandidate = null;
                var winnerVoteCount = 0;

                for (int i = 0; i < count.Count; i++) {

                    if (count[i] > winnerVoteCount) {
                        winnerVoteCount = count[i];
                        winnerCandidate = candidates[i];
                    }
                }

                var votes = db.VotingDetails.Where(x => x.VotedCandidateId == winnerCandidate.Id).ToList();

                foreach (var vote in votes) {
                    reasons.Add(vote.Reason);

                    var divisionIdFromEmployee = vote.Employee.DivisionId;

                    if (divisionId.Contains(divisionIdFromEmployee)) {
                        votePerDivision[divisionId.IndexOf(divisionIdFromEmployee)] += 1;
                    } else {
                        divisionId.Add(divisionIdFromEmployee);
                        votePerDivision.Add(1);

                    }
                }

                var winner = db.Employees.Find(winnerCandidate.EmployeeId);

                lbQty.Text = $"({winnerVoteCount}/{qtyVote})";
                double percentage = Convert.ToDouble(winnerVoteCount) / Convert.ToDouble(qtyVote) * 100;

                percentage = Math.Round(percentage, 2);

                lbPercentage.Text = percentage + "%";

                String[] name = { "Alexander.jpg", "Benjamin.jpg", "Emily.jpg", "Ethan.jpg", "Michael.jpg", "Sophia.jpg", "William.jpg" };
                Image[] image = { Resources.Alexander, Resources.Benjamin, Resources.Emily, Resources.Ethan, Resources.Michael, Resources.Sophia, Resources.William };

                var iName = winner.Name;
                lbName.Text = iName;

                for (var i = 0; i < name.Length; i++) {
                    if (name[i] == winner.Photo) {
                        pictureBox1.Image = image[i];
                        break;
                    } else {
                        pictureBox1.Image = null;
                    }
                }

                dataGridView1.Rows.Clear();

                if (divisionId.Count > 0) {
                    dataGridView1.Rows.Add(divisionId.Count);
                }

                var allDivisionCount = votePerDivision.Sum();

                foreach (DataGridViewRow row in dataGridView1.Rows) {

                    row.Cells[0].Value = db.Divisions.Find(divisionId[row.Index]).Name;
                    row.Cells[1].Value = votePerDivision[row.Index];

                    var percent = Convert.ToDouble(votePerDivision[row.Index]) / Convert.ToDouble(allDivisionCount) * 100;
                    percent = Math.Round(percent, 2);

                    row.Cells[2].Value = percent + " %";

                }

                dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);

                flowLayoutPanel1.Controls.Clear();

                foreach (var reason in reasons) {
                    if (reason != null) {

                        Reason reasonControl = new Reason(reason);

                        reasonControl.BorderStyle = BorderStyle.Fixed3D;

                        flowLayoutPanel1.Controls.Add(reasonControl);
                    }
                }
            }
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            change();
        }
    }
}
