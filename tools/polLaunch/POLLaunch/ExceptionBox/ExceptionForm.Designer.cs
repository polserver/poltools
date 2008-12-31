namespace POLLaunch.ExceptionBox
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
            this.LLBL_POLForumsLink = new System.Windows.Forms.LinkLabel();
            this.BTN_CopyToClipBoard = new System.Windows.Forms.Button();
            this.LBL_ExceptionInfo2 = new System.Windows.Forms.Label();
            this.BTN_CloseExceptionForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TB_ExceptionInformation
            // 
            this.TB_ExceptionInformation.BackColor = System.Drawing.SystemColors.Window;
            this.TB_ExceptionInformation.Location = new System.Drawing.Point(13, 53);
            this.TB_ExceptionInformation.Multiline = true;
            this.TB_ExceptionInformation.Name = "TB_ExceptionInformation";
            this.TB_ExceptionInformation.ReadOnly = true;
            this.TB_ExceptionInformation.Size = new System.Drawing.Size(640, 181);
            this.TB_ExceptionInformation.TabIndex = 0;
            // 
            // LLBL_POLForumsLink
            // 
            this.LLBL_POLForumsLink.AutoSize = true;
            this.LLBL_POLForumsLink.LinkArea = new System.Windows.Forms.LinkArea(53, 52);
            this.LLBL_POLForumsLink.Location = new System.Drawing.Point(18, 9);
            this.LLBL_POLForumsLink.Name = "LLBL_POLForumsLink";
            this.LLBL_POLForumsLink.Size = new System.Drawing.Size(628, 17);
            this.LLBL_POLForumsLink.TabIndex = 2;
            this.LLBL_POLForumsLink.TabStop = true;
            this.LLBL_POLForumsLink.Text = "An Exception Has Occured In POL Launch. Please Go To http://forums.polserver.com/" +
                "viewtopic.php?f=1&t=2492. Click on the";
            this.LLBL_POLForumsLink.UseCompatibleTextRendering = true;
            this.LLBL_POLForumsLink.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // BTN_CopyToClipBoard
            // 
            this.BTN_CopyToClipBoard.Location = new System.Drawing.Point(13, 240);
            this.BTN_CopyToClipBoard.Name = "BTN_CopyToClipBoard";
            this.BTN_CopyToClipBoard.Size = new System.Drawing.Size(121, 23);
            this.BTN_CopyToClipBoard.TabIndex = 3;
            this.BTN_CopyToClipBoard.Text = "Copy To Clipboard";
            this.BTN_CopyToClipBoard.UseVisualStyleBackColor = true;
            this.BTN_CopyToClipBoard.Click += new System.EventHandler(this.BTN_CopyToClipBoard_Click);
            // 
            // LBL_ExceptionInfo2
            // 
            this.LBL_ExceptionInfo2.AutoSize = true;
            this.LBL_ExceptionInfo2.Location = new System.Drawing.Point(117, 30);
            this.LBL_ExceptionInfo2.Name = "LBL_ExceptionInfo2";
            this.LBL_ExceptionInfo2.Size = new System.Drawing.Size(431, 13);
            this.LBL_ExceptionInfo2.TabIndex = 4;
            this.LBL_ExceptionInfo2.Text = "Copy To Clipboard button below, and paste the information about the Crash in a ne" +
                "w Post.";
            // 
            // BTN_CloseExceptionForm
            // 
            this.BTN_CloseExceptionForm.Location = new System.Drawing.Point(578, 240);
            this.BTN_CloseExceptionForm.Name = "BTN_CloseExceptionForm";
            this.BTN_CloseExceptionForm.Size = new System.Drawing.Size(75, 23);
            this.BTN_CloseExceptionForm.TabIndex = 5;
            this.BTN_CloseExceptionForm.Text = "Close";
            this.BTN_CloseExceptionForm.UseVisualStyleBackColor = true;
            this.BTN_CloseExceptionForm.Click += new System.EventHandler(this.BTN_CloseExceptionForm_Click);
            // 
            // ExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 273);
            this.Controls.Add(this.BTN_CloseExceptionForm);
            this.Controls.Add(this.LBL_ExceptionInfo2);
            this.Controls.Add(this.BTN_CopyToClipBoard);
            this.Controls.Add(this.LLBL_POLForumsLink);
            this.Controls.Add(this.TB_ExceptionInformation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exception Occured!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_ExceptionInformation;
        private System.Windows.Forms.LinkLabel LLBL_POLForumsLink;
        private System.Windows.Forms.Button BTN_CopyToClipBoard;
        private System.Windows.Forms.Label LBL_ExceptionInfo2;
        private System.Windows.Forms.Button BTN_CloseExceptionForm;
    }
}