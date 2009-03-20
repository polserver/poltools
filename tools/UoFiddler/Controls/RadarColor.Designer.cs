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
    partial class RadarColor
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewItem = new System.Windows.Forms.TreeView();
            this.treeViewLand = new System.Windows.Forms.TreeView();
            this.pictureBoxArt = new System.Windows.Forms.PictureBox();
            this.pictureBoxColor = new System.Windows.Forms.PictureBox();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonMean = new System.Windows.Forms.Button();
            this.textBoxShortCol = new System.Windows.Forms.TextBox();
            this.numericUpDownR = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownG = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownB = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxMeanTo = new System.Windows.Forms.TextBox();
            this.textBoxMeanFrom = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewItem
            // 
            this.treeViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewItem.HideSelection = false;
            this.treeViewItem.Location = new System.Drawing.Point(3, 3);
            this.treeViewItem.Name = "treeViewItem";
            this.treeViewItem.Size = new System.Drawing.Size(191, 162);
            this.treeViewItem.TabIndex = 0;
            this.treeViewItem.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelectTreeViewitem);
            // 
            // treeViewLand
            // 
            this.treeViewLand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLand.HideSelection = false;
            this.treeViewLand.Location = new System.Drawing.Point(3, 3);
            this.treeViewLand.Name = "treeViewLand";
            this.treeViewLand.Size = new System.Drawing.Size(192, 162);
            this.treeViewLand.TabIndex = 0;
            this.treeViewLand.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelectTreeViewLand);
            // 
            // pictureBoxArt
            // 
            this.pictureBoxArt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxArt.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxArt.Name = "pictureBoxArt";
            this.pictureBoxArt.Size = new System.Drawing.Size(205, 126);
            this.pictureBoxArt.TabIndex = 0;
            this.pictureBoxArt.TabStop = false;
            // 
            // pictureBoxColor
            // 
            this.pictureBoxColor.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxColor.Name = "pictureBoxColor";
            this.pictureBoxColor.Size = new System.Drawing.Size(138, 101);
            this.pictureBoxColor.TabIndex = 0;
            this.pictureBoxColor.TabStop = false;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.textBoxMeanFrom);
            this.splitContainer5.Panel2.Controls.Add(this.textBoxMeanTo);
            this.splitContainer5.Panel2.Controls.Add(this.button3);
            this.splitContainer5.Panel2.Controls.Add(this.numericUpDownB);
            this.splitContainer5.Panel2.Controls.Add(this.numericUpDownG);
            this.splitContainer5.Panel2.Controls.Add(this.numericUpDownR);
            this.splitContainer5.Panel2.Controls.Add(this.textBoxShortCol);
            this.splitContainer5.Panel2.Controls.Add(this.button2);
            this.splitContainer5.Panel2.Controls.Add(this.button1);
            this.splitContainer5.Panel2.Controls.Add(this.buttonMean);
            this.splitContainer5.Panel2.Controls.Add(this.pictureBoxColor);
            this.splitContainer5.Size = new System.Drawing.Size(619, 324);
            this.splitContainer5.SplitterDistance = 205;
            this.splitContainer5.TabIndex = 1;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.pictureBoxArt);
            this.splitContainer6.Size = new System.Drawing.Size(205, 324);
            this.splitContainer6.SplitterDistance = 194;
            this.splitContainer6.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(205, 194);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.treeViewItem);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(197, 168);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Items";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.treeViewLand);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(198, 168);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Landtiles";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(83, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Save File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.onClickSaveFile);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Save Color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.onClickSaveColor);
            // 
            // buttonMean
            // 
            this.buttonMean.Location = new System.Drawing.Point(4, 111);
            this.buttonMean.Name = "buttonMean";
            this.buttonMean.Size = new System.Drawing.Size(75, 23);
            this.buttonMean.TabIndex = 1;
            this.buttonMean.Text = "Average Color";
            this.buttonMean.UseVisualStyleBackColor = true;
            this.buttonMean.Click += new System.EventHandler(this.OnClickMeanColor);
            // 
            // textBoxShortCol
            // 
            this.textBoxShortCol.Location = new System.Drawing.Point(189, 4);
            this.textBoxShortCol.Name = "textBoxShortCol";
            this.textBoxShortCol.Size = new System.Drawing.Size(100, 20);
            this.textBoxShortCol.TabIndex = 6;
            this.textBoxShortCol.TextChanged += new System.EventHandler(this.OnChangeShortText);
            // 
            // numericUpDownR
            // 
            this.numericUpDownR.Location = new System.Drawing.Point(189, 36);
            this.numericUpDownR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownR.Name = "numericUpDownR";
            this.numericUpDownR.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownR.TabIndex = 7;
            this.numericUpDownR.ValueChanged += new System.EventHandler(this.onChangeR);
            // 
            // numericUpDownG
            // 
            this.numericUpDownG.Location = new System.Drawing.Point(242, 36);
            this.numericUpDownG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownG.Name = "numericUpDownG";
            this.numericUpDownG.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownG.TabIndex = 8;
            this.numericUpDownG.ValueChanged += new System.EventHandler(this.OnChangeG);
            // 
            // numericUpDownB
            // 
            this.numericUpDownB.Location = new System.Drawing.Point(295, 36);
            this.numericUpDownB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownB.Name = "numericUpDownB";
            this.numericUpDownB.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownB.TabIndex = 9;
            this.numericUpDownB.ValueChanged += new System.EventHandler(this.OnChangeB);
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.Location = new System.Drawing.Point(189, 137);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Average Color from-to";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClickmeanColorFromTo);
            // 
            // textBoxMeanTo
            // 
            this.textBoxMeanTo.Location = new System.Drawing.Point(256, 114);
            this.textBoxMeanTo.Name = "textBoxMeanTo";
            this.textBoxMeanTo.Size = new System.Drawing.Size(52, 20);
            this.textBoxMeanTo.TabIndex = 12;
            // 
            // textBoxMeanFrom
            // 
            this.textBoxMeanFrom.Location = new System.Drawing.Point(189, 114);
            this.textBoxMeanFrom.Name = "textBoxMeanFrom";
            this.textBoxMeanFrom.Size = new System.Drawing.Size(52, 20);
            this.textBoxMeanFrom.TabIndex = 13;
            // 
            // RadarColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer5);
            this.Name = "RadarColor";
            this.Size = new System.Drawing.Size(619, 324);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).EndInit();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewItem;
        private System.Windows.Forms.TreeView treeViewLand;
        private System.Windows.Forms.PictureBox pictureBoxArt;
        private System.Windows.Forms.PictureBox pictureBoxColor;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button buttonMean;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDownB;
        private System.Windows.Forms.NumericUpDown numericUpDownG;
        private System.Windows.Forms.NumericUpDown numericUpDownR;
        private System.Windows.Forms.TextBox textBoxShortCol;
        private System.Windows.Forms.TextBox textBoxMeanFrom;
        private System.Windows.Forms.TextBox textBoxMeanTo;
        private System.Windows.Forms.Button button3;
    }
}
