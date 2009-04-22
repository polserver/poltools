/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

namespace FiddlerControls
{
    partial class MapReplace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapReplace));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxMap = new System.Windows.Forms.CheckBox();
            this.checkBoxStatics = new System.Windows.Forms.CheckBox();
            this.numericUpDownX1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownY1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownX2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownY2 = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.RemoveDupl = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY2)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(90, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(195, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Location = new System.Drawing.Point(291, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClickBrowse);
            // 
            // checkBoxMap
            // 
            this.checkBoxMap.AutoSize = true;
            this.checkBoxMap.Location = new System.Drawing.Point(58, 40);
            this.checkBoxMap.Name = "checkBoxMap";
            this.checkBoxMap.Size = new System.Drawing.Size(74, 17);
            this.checkBoxMap.TabIndex = 2;
            this.checkBoxMap.Text = "Copy Map";
            this.checkBoxMap.UseVisualStyleBackColor = true;
            // 
            // checkBoxStatics
            // 
            this.checkBoxStatics.AutoSize = true;
            this.checkBoxStatics.Location = new System.Drawing.Point(58, 64);
            this.checkBoxStatics.Name = "checkBoxStatics";
            this.checkBoxStatics.Size = new System.Drawing.Size(85, 17);
            this.checkBoxStatics.TabIndex = 3;
            this.checkBoxStatics.Text = "Copy Statics";
            this.checkBoxStatics.UseVisualStyleBackColor = true;
            // 
            // numericUpDownX1
            // 
            this.numericUpDownX1.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownX1.Location = new System.Drawing.Point(96, 91);
            this.numericUpDownX1.Name = "numericUpDownX1";
            this.numericUpDownX1.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownX1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "X1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Y1";
            // 
            // numericUpDownY1
            // 
            this.numericUpDownY1.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownY1.Location = new System.Drawing.Point(96, 117);
            this.numericUpDownY1.Name = "numericUpDownY1";
            this.numericUpDownY1.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownY1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "X2";
            // 
            // numericUpDownX2
            // 
            this.numericUpDownX2.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownX2.Location = new System.Drawing.Point(207, 91);
            this.numericUpDownX2.Name = "numericUpDownX2";
            this.numericUpDownX2.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownX2.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Y2";
            // 
            // numericUpDownY2
            // 
            this.numericUpDownY2.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownY2.Location = new System.Drawing.Point(207, 119);
            this.numericUpDownY2.Name = "numericUpDownY2";
            this.numericUpDownY2.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownY2.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(126, 158);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Replace";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClickCopy);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 195);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(327, 23);
            this.progressBar1.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Replace From";
            // 
            // RemoveDupl
            // 
            this.RemoveDupl.AutoSize = true;
            this.RemoveDupl.Location = new System.Drawing.Point(149, 64);
            this.RemoveDupl.Name = "RemoveDupl";
            this.RemoveDupl.Size = new System.Drawing.Size(119, 17);
            this.RemoveDupl.TabIndex = 17;
            this.RemoveDupl.Text = "Remove Duplicates";
            this.RemoveDupl.UseVisualStyleBackColor = true;
            // 
            // MapCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 218);
            this.Controls.Add(this.RemoveDupl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownY2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownX2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownY1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownX1);
            this.Controls.Add(this.checkBoxStatics);
            this.Controls.Add(this.checkBoxMap);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(335, 252);
            this.MinimumSize = new System.Drawing.Size(335, 252);
            this.Name = "MapCopy";
            this.Text = "MapReplace";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxMap;
        private System.Windows.Forms.CheckBox checkBoxStatics;
        private System.Windows.Forms.NumericUpDown numericUpDownX1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownY1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownX2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownY2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox RemoveDupl;
    }
}