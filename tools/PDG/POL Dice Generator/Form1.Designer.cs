namespace POL_Dice_Generator
{
    partial class MyForm
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
            this.TB_MinValue = new System.Windows.Forms.TextBox();
            this.TB_MaxValue = new System.Windows.Forms.TextBox();
            this.TB_Result = new System.Windows.Forms.TextBox();
            this.LBL_MinValue = new System.Windows.Forms.Label();
            this.LBL_MaxValue = new System.Windows.Forms.Label();
            this.LBL_Result = new System.Windows.Forms.Label();
            this.BTN_Create = new System.Windows.Forms.Button();
            this.BTN_Exit = new System.Windows.Forms.Button();
            this.BTN_Copy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TB_MinValue
            // 
            this.TB_MinValue.Location = new System.Drawing.Point(46, 21);
            this.TB_MinValue.Name = "TB_MinValue";
            this.TB_MinValue.Size = new System.Drawing.Size(100, 20);
            this.TB_MinValue.TabIndex = 0;
            // 
            // TB_MaxValue
            // 
            this.TB_MaxValue.Location = new System.Drawing.Point(46, 47);
            this.TB_MaxValue.Name = "TB_MaxValue";
            this.TB_MaxValue.Size = new System.Drawing.Size(100, 20);
            this.TB_MaxValue.TabIndex = 1;
            // 
            // TB_Result
            // 
            this.TB_Result.BackColor = System.Drawing.SystemColors.Window;
            this.TB_Result.Location = new System.Drawing.Point(46, 102);
            this.TB_Result.Name = "TB_Result";
            this.TB_Result.ReadOnly = true;
            this.TB_Result.Size = new System.Drawing.Size(100, 20);
            this.TB_Result.TabIndex = 2;
            // 
            // LBL_MinValue
            // 
            this.LBL_MinValue.AutoSize = true;
            this.LBL_MinValue.Location = new System.Drawing.Point(13, 24);
            this.LBL_MinValue.Name = "LBL_MinValue";
            this.LBL_MinValue.Size = new System.Drawing.Size(27, 13);
            this.LBL_MinValue.TabIndex = 3;
            this.LBL_MinValue.Text = "Min:";
            // 
            // LBL_MaxValue
            // 
            this.LBL_MaxValue.AutoSize = true;
            this.LBL_MaxValue.Location = new System.Drawing.Point(10, 50);
            this.LBL_MaxValue.Name = "LBL_MaxValue";
            this.LBL_MaxValue.Size = new System.Drawing.Size(30, 13);
            this.LBL_MaxValue.TabIndex = 4;
            this.LBL_MaxValue.Text = "Max:";
            // 
            // LBL_Result
            // 
            this.LBL_Result.AutoSize = true;
            this.LBL_Result.Location = new System.Drawing.Point(0, 105);
            this.LBL_Result.Name = "LBL_Result";
            this.LBL_Result.Size = new System.Drawing.Size(40, 13);
            this.LBL_Result.TabIndex = 5;
            this.LBL_Result.Text = "Result:";
            // 
            // BTN_Create
            // 
            this.BTN_Create.Location = new System.Drawing.Point(59, 73);
            this.BTN_Create.Name = "BTN_Create";
            this.BTN_Create.Size = new System.Drawing.Size(75, 23);
            this.BTN_Create.TabIndex = 6;
            this.BTN_Create.Text = "Create";
            this.BTN_Create.UseVisualStyleBackColor = true;
            this.BTN_Create.Click += new System.EventHandler(this.BTN_Create_Click);
            // 
            // BTN_Exit
            // 
            this.BTN_Exit.Location = new System.Drawing.Point(59, 162);
            this.BTN_Exit.Name = "BTN_Exit";
            this.BTN_Exit.Size = new System.Drawing.Size(75, 23);
            this.BTN_Exit.TabIndex = 7;
            this.BTN_Exit.Text = "Exit";
            this.BTN_Exit.UseVisualStyleBackColor = true;
            this.BTN_Exit.Click += new System.EventHandler(this.BTN_Exit_Click);
            // 
            // BTN_Copy
            // 
            this.BTN_Copy.Location = new System.Drawing.Point(59, 128);
            this.BTN_Copy.Name = "BTN_Copy";
            this.BTN_Copy.Size = new System.Drawing.Size(75, 23);
            this.BTN_Copy.TabIndex = 8;
            this.BTN_Copy.Text = "Copy";
            this.BTN_Copy.UseVisualStyleBackColor = true;
            this.BTN_Copy.Click += new System.EventHandler(this.BTN_Copy_Click);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 202);
            this.Controls.Add(this.BTN_Copy);
            this.Controls.Add(this.BTN_Exit);
            this.Controls.Add(this.BTN_Create);
            this.Controls.Add(this.LBL_Result);
            this.Controls.Add(this.LBL_MaxValue);
            this.Controls.Add(this.LBL_MinValue);
            this.Controls.Add(this.TB_Result);
            this.Controls.Add(this.TB_MaxValue);
            this.Controls.Add(this.TB_MinValue);
            this.Name = "MyForm";
            this.Text = "Dice Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_MinValue;
        private System.Windows.Forms.TextBox TB_MaxValue;
        private System.Windows.Forms.TextBox TB_Result;
        private System.Windows.Forms.Label LBL_MinValue;
        private System.Windows.Forms.Label LBL_MaxValue;
        private System.Windows.Forms.Label LBL_Result;
        private System.Windows.Forms.Button BTN_Create;
        private System.Windows.Forms.Button BTN_Exit;
        private System.Windows.Forms.Button BTN_Copy;
    }
}

