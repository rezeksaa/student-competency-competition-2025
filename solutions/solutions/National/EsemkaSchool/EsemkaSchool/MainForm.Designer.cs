namespace EsemkaSchool {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dashboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assesmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.assesmentFragment1 = new EsemkaSchool.Fragments.AssesmentFragment();
            this.attendanceFragment1 = new EsemkaSchool.Fragments.AttendanceFragment();
            this.dashboardFragment1 = new EsemkaSchool.Fragments.DashboardFragment();
            this.scoreFragment1 = new EsemkaSchool.Fragments.ScoreFragment();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.meenuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // meenuToolStripMenuItem
            // 
            this.meenuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dashboardToolStripMenuItem,
            this.attendanceToolStripMenuItem,
            this.assesmentToolStripMenuItem,
            this.scoreToolStripMenuItem});
            this.meenuToolStripMenuItem.Name = "meenuToolStripMenuItem";
            this.meenuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.meenuToolStripMenuItem.Text = "Menu";
            this.meenuToolStripMenuItem.Click += new System.EventHandler(this.meenuToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dashboardToolStripMenuItem
            // 
            this.dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            this.dashboardToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.dashboardToolStripMenuItem.Text = "Dashboard";
            this.dashboardToolStripMenuItem.Click += new System.EventHandler(this.dashboardToolStripMenuItem_Click);
            // 
            // attendanceToolStripMenuItem
            // 
            this.attendanceToolStripMenuItem.Name = "attendanceToolStripMenuItem";
            this.attendanceToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.attendanceToolStripMenuItem.Text = "Attendance";
            this.attendanceToolStripMenuItem.Click += new System.EventHandler(this.attendanceToolStripMenuItem_Click);
            // 
            // assesmentToolStripMenuItem
            // 
            this.assesmentToolStripMenuItem.Name = "assesmentToolStripMenuItem";
            this.assesmentToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.assesmentToolStripMenuItem.Text = "Assesment Component";
            this.assesmentToolStripMenuItem.Click += new System.EventHandler(this.assesmentToolStripMenuItem_Click);
            // 
            // scoreToolStripMenuItem
            // 
            this.scoreToolStripMenuItem.Name = "scoreToolStripMenuItem";
            this.scoreToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.scoreToolStripMenuItem.Text = "Score";
            this.scoreToolStripMenuItem.Click += new System.EventHandler(this.scoreToolStripMenuItem_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.assesmentFragment1);
            this.flowLayoutPanel1.Controls.Add(this.attendanceFragment1);
            this.flowLayoutPanel1.Controls.Add(this.dashboardFragment1);
            this.flowLayoutPanel1.Controls.Add(this.scoreFragment1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1200, 695);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // assesmentFragment1
            // 
            this.assesmentFragment1.Location = new System.Drawing.Point(4, 4);
            this.assesmentFragment1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.assesmentFragment1.Name = "assesmentFragment1";
            this.assesmentFragment1.Size = new System.Drawing.Size(1200, 695);
            this.assesmentFragment1.TabIndex = 0;
            // 
            // attendanceFragment1
            // 
            this.attendanceFragment1.Location = new System.Drawing.Point(4, 707);
            this.attendanceFragment1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.attendanceFragment1.Name = "attendanceFragment1";
            this.attendanceFragment1.Size = new System.Drawing.Size(1200, 695);
            this.attendanceFragment1.TabIndex = 1;
            // 
            // dashboardFragment1
            // 
            this.dashboardFragment1.Location = new System.Drawing.Point(4, 1410);
            this.dashboardFragment1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dashboardFragment1.Name = "dashboardFragment1";
            this.dashboardFragment1.Size = new System.Drawing.Size(1200, 695);
            this.dashboardFragment1.TabIndex = 2;
            // 
            // scoreFragment1
            // 
            this.scoreFragment1.Location = new System.Drawing.Point(4, 2113);
            this.scoreFragment1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scoreFragment1.Name = "scoreFragment1";
            this.scoreFragment1.Size = new System.Drawing.Size(1200, 695);
            this.scoreFragment1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 719);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Esemka School";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dashboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attendanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assesmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scoreToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Fragments.AssesmentFragment assesmentFragment1;
        private Fragments.AttendanceFragment attendanceFragment1;
        private Fragments.DashboardFragment dashboardFragment1;
        private Fragments.ScoreFragment scoreFragment1;
    }
}