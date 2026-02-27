namespace Esemka_Vote {
    partial class Reason {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lbReason = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbReason
            // 
            this.lbReason.BackColor = System.Drawing.Color.White;
            this.lbReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbReason.Font = new System.Drawing.Font("Arial", 12F);
            this.lbReason.Location = new System.Drawing.Point(0, 0);
            this.lbReason.Name = "lbReason";
            this.lbReason.Size = new System.Drawing.Size(311, 90);
            this.lbReason.TabIndex = 0;
            this.lbReason.Text = "label1";
            // 
            // Reason
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbReason);
            this.Name = "Reason";
            this.Size = new System.Drawing.Size(311, 90);
            this.Load += new System.EventHandler(this.Reason_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbReason;
    }
}
