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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyForm));
            this.TB_MinValue = new System.Windows.Forms.TextBox();
            this.TB_MaxValue = new System.Windows.Forms.TextBox();
            this.TB_Result = new System.Windows.Forms.TextBox();
            this.LBL_MinValue = new System.Windows.Forms.Label();
            this.LBL_MaxValue = new System.Windows.Forms.Label();
            this.LBL_Result = new System.Windows.Forms.Label();
            this.BTN_Create = new System.Windows.Forms.Button();
            this.BTN_Exit = new System.Windows.Forms.Button();
            this.BTN_Copy = new System.Windows.Forms.Button();
            this.chkUniform = new System.Windows.Forms.CheckBox();
            this.chkAutocopy = new System.Windows.Forms.CheckBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkOnTop = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BTN_Options = new System.Windows.Forms.Button();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // TB_MinValue
            // 
            this.TB_MinValue.Location = new System.Drawing.Point(55, 4);
            this.TB_MinValue.Name = "TB_MinValue";
            this.TB_MinValue.Size = new System.Drawing.Size(100, 20);
            this.TB_MinValue.TabIndex = 0;
            this.TB_MinValue.TextChanged += new System.EventHandler(this.TB_MaxMinValue_TextChanged);
            // 
            // TB_MaxValue
            // 
            this.TB_MaxValue.Location = new System.Drawing.Point(55, 30);
            this.TB_MaxValue.Name = "TB_MaxValue";
            this.TB_MaxValue.Size = new System.Drawing.Size(100, 20);
            this.TB_MaxValue.TabIndex = 1;
            this.TB_MaxValue.TextChanged += new System.EventHandler(this.TB_MaxMinValue_TextChanged);
            // 
            // TB_Result
            // 
            this.TB_Result.BackColor = System.Drawing.SystemColors.Window;
            this.TB_Result.Location = new System.Drawing.Point(55, 59);
            this.TB_Result.Name = "TB_Result";
            this.TB_Result.ReadOnly = true;
            this.TB_Result.Size = new System.Drawing.Size(100, 20);
            this.TB_Result.TabIndex = 2;
            // 
            // LBL_MinValue
            // 
            this.LBL_MinValue.AutoSize = true;
            this.LBL_MinValue.Location = new System.Drawing.Point(25, 7);
            this.LBL_MinValue.Name = "LBL_MinValue";
            this.LBL_MinValue.Size = new System.Drawing.Size(27, 13);
            this.LBL_MinValue.TabIndex = 3;
            this.LBL_MinValue.Text = "Min:";
            // 
            // LBL_MaxValue
            // 
            this.LBL_MaxValue.AutoSize = true;
            this.LBL_MaxValue.Location = new System.Drawing.Point(22, 33);
            this.LBL_MaxValue.Name = "LBL_MaxValue";
            this.LBL_MaxValue.Size = new System.Drawing.Size(30, 13);
            this.LBL_MaxValue.TabIndex = 4;
            this.LBL_MaxValue.Text = "Max:";
            // 
            // LBL_Result
            // 
            this.LBL_Result.AutoSize = true;
            this.LBL_Result.Location = new System.Drawing.Point(17, 62);
            this.LBL_Result.Name = "LBL_Result";
            this.LBL_Result.Size = new System.Drawing.Size(32, 13);
            this.LBL_Result.TabIndex = 5;
            this.LBL_Result.Text = "Dice:";
            // 
            // BTN_Create
            // 
            this.BTN_Create.Location = new System.Drawing.Point(80, 84);
            this.BTN_Create.Name = "BTN_Create";
            this.BTN_Create.Size = new System.Drawing.Size(75, 23);
            this.BTN_Create.TabIndex = 6;
            this.BTN_Create.Text = "Create";
            this.toolTip1.SetToolTip(this.BTN_Create, "Update the result.");
            this.BTN_Create.UseVisualStyleBackColor = true;
            this.BTN_Create.Click += new System.EventHandler(this.BTN_Create_Click);
            // 
            // BTN_Exit
            // 
            this.BTN_Exit.Location = new System.Drawing.Point(80, 113);
            this.BTN_Exit.Name = "BTN_Exit";
            this.BTN_Exit.Size = new System.Drawing.Size(75, 23);
            this.BTN_Exit.TabIndex = 7;
            this.BTN_Exit.Text = "Exit";
            this.BTN_Exit.UseVisualStyleBackColor = true;
            this.BTN_Exit.Click += new System.EventHandler(this.BTN_Exit_Click);
            // 
            // BTN_Copy
            // 
            this.BTN_Copy.Location = new System.Drawing.Point(3, 84);
            this.BTN_Copy.Name = "BTN_Copy";
            this.BTN_Copy.Size = new System.Drawing.Size(75, 23);
            this.BTN_Copy.TabIndex = 8;
            this.BTN_Copy.Text = "Copy";
            this.toolTip1.SetToolTip(this.BTN_Copy, "Copy the result to the clipboard.");
            this.BTN_Copy.UseVisualStyleBackColor = true;
            this.BTN_Copy.Click += new System.EventHandler(this.BTN_Copy_Click);
            // 
            // chkUniform
            // 
            this.chkUniform.AutoSize = true;
            this.chkUniform.Location = new System.Drawing.Point(89, 19);
            this.chkUniform.Name = "chkUniform";
            this.chkUniform.Size = new System.Drawing.Size(62, 17);
            this.chkUniform.TabIndex = 9;
            this.chkUniform.Text = "Uniform";
            this.toolTip1.SetToolTip(this.chkUniform, resources.GetString("chkUniform.ToolTip"));
            this.chkUniform.UseVisualStyleBackColor = true;
            this.chkUniform.CheckedChanged += new System.EventHandler(this.chkUniform_CheckedChanged);
            // 
            // chkAutocopy
            // 
            this.chkAutocopy.AutoSize = true;
            this.chkAutocopy.Checked = true;
            this.chkAutocopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutocopy.Location = new System.Drawing.Point(4, 39);
            this.chkAutocopy.Name = "chkAutocopy";
            this.chkAutocopy.Size = new System.Drawing.Size(72, 17);
            this.chkAutocopy.TabIndex = 10;
            this.chkAutocopy.Text = "AutoCopy";
            this.toolTip1.SetToolTip(this.chkAutocopy, "When enabled, auto-copies the result to the clipboard.");
            this.chkAutocopy.UseVisualStyleBackColor = true;
            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Checked = true;
            this.chkUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdate.Location = new System.Drawing.Point(4, 19);
            this.chkUpdate.Name = "chkUpdate";
            this.chkUpdate.Size = new System.Drawing.Size(83, 17);
            this.chkUpdate.TabIndex = 11;
            this.chkUpdate.Text = "AutoUpdate";
            this.toolTip1.SetToolTip(this.chkUpdate, "When enabled, changing min/max will update dice string.");
            this.chkUpdate.UseVisualStyleBackColor = true;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkOnTop);
            this.grpOptions.Controls.Add(this.chkUniform);
            this.grpOptions.Controls.Add(this.chkUpdate);
            this.grpOptions.Controls.Add(this.chkAutocopy);
            this.grpOptions.Location = new System.Drawing.Point(3, 136);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(152, 62);
            this.grpOptions.TabIndex = 12;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // chkOnTop
            // 
            this.chkOnTop.AutoSize = true;
            this.chkOnTop.Location = new System.Drawing.Point(89, 37);
            this.chkOnTop.Name = "chkOnTop";
            this.chkOnTop.Size = new System.Drawing.Size(62, 17);
            this.chkOnTop.TabIndex = 12;
            this.chkOnTop.Text = "On-Top";
            this.toolTip1.SetToolTip(this.chkOnTop, "Always on Top");
            this.chkOnTop.UseVisualStyleBackColor = true;
            this.chkOnTop.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // BTN_Options
            // 
            this.BTN_Options.Location = new System.Drawing.Point(3, 113);
            this.BTN_Options.Name = "BTN_Options";
            this.BTN_Options.Size = new System.Drawing.Size(75, 23);
            this.BTN_Options.TabIndex = 13;
            this.BTN_Options.Text = "Options <<";
            this.BTN_Options.UseVisualStyleBackColor = true;
            this.BTN_Options.Click += new System.EventHandler(this.button1_Click);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(158, 202);
            this.Controls.Add(this.BTN_Options);
            this.Controls.Add(this.grpOptions);
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
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
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
        private System.Windows.Forms.CheckBox chkUniform;
        private System.Windows.Forms.CheckBox chkAutocopy;
        private System.Windows.Forms.CheckBox chkUpdate;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox chkOnTop;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BTN_Options;
    }
}

