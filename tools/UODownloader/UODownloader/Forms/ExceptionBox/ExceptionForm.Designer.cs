namespace UODownloader.ExceptionBox
{
    partial class ExceptionForm
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
			this.TB_ExceptionInformation = new System.Windows.Forms.TextBox();
			this.LBL_ExceptionInfo1 = new System.Windows.Forms.LinkLabel();
			this.BTN_CopyToClipBoard = new System.Windows.Forms.Button();
			this.LBL_ExceptionInfo2 = new System.Windows.Forms.Label();
			this.BTN_CloseExceptionForm = new System.Windows.Forms.Button();
			this.BTN_Quit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TB_ExceptionInformation
			// 
			this.TB_ExceptionInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(219)))));
			this.TB_ExceptionInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TB_ExceptionInformation.Location = new System.Drawing.Point(13, 53);
			this.TB_ExceptionInformation.Multiline = true;
			this.TB_ExceptionInformation.Name = "TB_ExceptionInformation";
			this.TB_ExceptionInformation.ReadOnly = true;
			this.TB_ExceptionInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TB_ExceptionInformation.Size = new System.Drawing.Size(640, 181);
			this.TB_ExceptionInformation.TabIndex = 0;
			// 
			// LBL_ExceptionInfo1
			// 
			this.LBL_ExceptionInfo1.AutoSize = true;
			this.LBL_ExceptionInfo1.LinkArea = new System.Windows.Forms.LinkArea(53, 52);
			this.LBL_ExceptionInfo1.Location = new System.Drawing.Point(263, 9);
			this.LBL_ExceptionInfo1.Name = "LBL_ExceptionInfo1";
			this.LBL_ExceptionInfo1.Size = new System.Drawing.Size(139, 17);
			this.LBL_ExceptionInfo1.TabIndex = 2;
			this.LBL_ExceptionInfo1.Text = "An Exception has occured!";
			this.LBL_ExceptionInfo1.UseCompatibleTextRendering = true;
			this.LBL_ExceptionInfo1.VisitedLinkColor = System.Drawing.Color.Blue;
			// 
			// BTN_CopyToClipBoard
			// 
			this.BTN_CopyToClipBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_CopyToClipBoard.Location = new System.Drawing.Point(13, 240);
			this.BTN_CopyToClipBoard.Name = "BTN_CopyToClipBoard";
			this.BTN_CopyToClipBoard.Size = new System.Drawing.Size(121, 23);
			this.BTN_CopyToClipBoard.TabIndex = 3;
			this.BTN_CopyToClipBoard.Text = "Copy To Clipboard";
			this.BTN_CopyToClipBoard.UseVisualStyleBackColor = false;
			this.BTN_CopyToClipBoard.Click += new System.EventHandler(this.BTN_CopyToClipBoard_Click);
			// 
			// LBL_ExceptionInfo2
			// 
			this.LBL_ExceptionInfo2.AutoSize = true;
			this.LBL_ExceptionInfo2.Location = new System.Drawing.Point(96, 30);
			this.LBL_ExceptionInfo2.Name = "LBL_ExceptionInfo2";
			this.LBL_ExceptionInfo2.Size = new System.Drawing.Size(473, 13);
			this.LBL_ExceptionInfo2.TabIndex = 4;
			this.LBL_ExceptionInfo2.Text = "Click the \"Copy To Clipboard\" button below, and paste the information in an email" +
				" to the developer.";
			// 
			// BTN_CloseExceptionForm
			// 
			this.BTN_CloseExceptionForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_CloseExceptionForm.Location = new System.Drawing.Point(578, 240);
			this.BTN_CloseExceptionForm.Name = "BTN_CloseExceptionForm";
			this.BTN_CloseExceptionForm.Size = new System.Drawing.Size(75, 23);
			this.BTN_CloseExceptionForm.TabIndex = 5;
			this.BTN_CloseExceptionForm.Text = "Continue";
			this.BTN_CloseExceptionForm.UseVisualStyleBackColor = false;
			this.BTN_CloseExceptionForm.Click += new System.EventHandler(this.BTN_CloseExceptionForm_Click);
			// 
			// BTN_Quit
			// 
			this.BTN_Quit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.BTN_Quit.Location = new System.Drawing.Point(272, 240);
			this.BTN_Quit.Name = "BTN_Quit";
			this.BTN_Quit.Size = new System.Drawing.Size(121, 23);
			this.BTN_Quit.TabIndex = 6;
			this.BTN_Quit.Text = "Quit Application";
			this.BTN_Quit.UseVisualStyleBackColor = false;
			this.BTN_Quit.Click += new System.EventHandler(this.button1_Click);
			// 
			// ExceptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(218)))), ((int)(((byte)(229)))));
			this.ClientSize = new System.Drawing.Size(665, 273);
			this.Controls.Add(this.BTN_Quit);
			this.Controls.Add(this.BTN_CloseExceptionForm);
			this.Controls.Add(this.LBL_ExceptionInfo2);
			this.Controls.Add(this.BTN_CopyToClipBoard);
			this.Controls.Add(this.LBL_ExceptionInfo1);
			this.Controls.Add(this.TB_ExceptionInformation);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExceptionForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Exception Occured!";
			this.Load += new System.EventHandler(this.ExceptionForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_ExceptionInformation;
        private System.Windows.Forms.LinkLabel LBL_ExceptionInfo1;
        private System.Windows.Forms.Button BTN_CopyToClipBoard;
        private System.Windows.Forms.Label LBL_ExceptionInfo2;
        private System.Windows.Forms.Button BTN_CloseExceptionForm;
		private System.Windows.Forms.Button BTN_Quit;
    }
}