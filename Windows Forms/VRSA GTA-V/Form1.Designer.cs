namespace VRSA_GTA_V
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnCleanSession = new System.Windows.Forms.Button();
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlControls.Controls.Add(this.btnCleanSession);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 684);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1262, 46);
            this.pnlControls.TabIndex = 0;
            // 
            // btnCleanSession
            // 
            this.btnCleanSession.Location = new System.Drawing.Point(12, 12);
            this.btnCleanSession.Name = "btnCleanSession";
            this.btnCleanSession.Size = new System.Drawing.Size(114, 23);
            this.btnCleanSession.TabIndex = 1;
            this.btnCleanSession.Text = "Clean Session";
            this.btnCleanSession.UseVisualStyleBackColor = true;
            this.btnCleanSession.Click += new System.EventHandler(this.BtnCleanSession_Click);
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBrowser.Location = new System.Drawing.Point(0, 0);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(1262, 684);
            this.pnlBrowser.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1262, 730);
            this.Controls.Add(this.pnlBrowser);
            this.Controls.Add(this.pnlControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "VRSA GTA-V";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.pnlControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnCleanSession;
        private System.Windows.Forms.Panel pnlBrowser;
    }
}

