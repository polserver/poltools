using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace POLLaunch
{
    /// <summary>
    /// Summary description for AboutForm.
    /// </summary>
    public class AboutForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel WebLink;
        private System.Windows.Forms.LinkLabel EmailLink;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AboutForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.WebLink = new System.Windows.Forms.LinkLabel();
			this.EmailLink = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(208, 48);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// WebLink
			// 
			this.WebLink.AutoSize = true;
			this.WebLink.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.WebLink.Location = new System.Drawing.Point(5, 128);
			this.WebLink.Name = "WebLink";
			this.WebLink.Size = new System.Drawing.Size(145, 16);
			this.WebLink.TabIndex = 1;
			this.WebLink.TabStop = true;
			this.WebLink.Text = "http://www.polserver.com";
			this.WebLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.WebLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelClicked);
			// 
			// EmailLink
			// 
			this.EmailLink.AutoSize = true;
			this.EmailLink.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EmailLink.Location = new System.Drawing.Point(156, 128);
			this.EmailLink.Name = "EmailLink";
			this.EmailLink.Size = new System.Drawing.Size(134, 16);
			this.EmailLink.TabIndex = 2;
			this.EmailLink.TabStop = true;
			this.EmailLink.Text = "POLTeam@polserver.com";
			this.EmailLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.EmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(83, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(117, 18);
			this.label1.TabIndex = 3;
			this.label1.Text = "POL Launcher Tool";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AboutForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.ClientSize = new System.Drawing.Size(290, 150);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.EmailLink);
			this.Controls.Add(this.WebLink);
			this.Controls.Add(this.pictureBox1);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.AboutForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion

        private void AboutForm_Load(object sender, EventArgs e)
        {
        }

        private void LinkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel clicked = (LinkLabel)sender;
            if (sender == EmailLink)
                System.Diagnostics.Process.Start("mailto:" + this.EmailLink.Text.ToString());
            else
                System.Diagnostics.Process.Start(clicked.Text.ToString());
        }
    }
}
