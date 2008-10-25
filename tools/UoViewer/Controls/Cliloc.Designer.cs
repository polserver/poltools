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

namespace Controls
{
    partial class Cliloc
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cliloc));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ClilocNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClilocText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stringEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.LangComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.GotoEntry = new System.Windows.Forms.ToolStripTextBox();
            this.GotoButton = new System.Windows.Forms.ToolStripButton();
            this.FindEntry = new System.Windows.Forms.ToolStripTextBox();
            this.FindButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.loadLabel = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stringEntryBindingSource)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClilocNumber,
            this.ClilocText});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(619, 299);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.onCell_dbClick);
            // 
            // ClilocNumber
            // 
            this.ClilocNumber.FillWeight = 20F;
            this.ClilocNumber.HeaderText = "Number";
            this.ClilocNumber.Name = "ClilocNumber";
            this.ClilocNumber.ReadOnly = true;
            // 
            // ClilocText
            // 
            this.ClilocText.FillWeight = 98.47716F;
            this.ClilocText.HeaderText = "Text";
            this.ClilocText.Name = "ClilocText";
            this.ClilocText.ReadOnly = true;
            // 
            // stringEntryBindingSource
            // 
            this.stringEntryBindingSource.DataSource = typeof(Ultima.StringEntry);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LangComboBox,
            this.toolStripSeparator1,
            this.GotoEntry,
            this.GotoButton,
            this.FindEntry,
            this.FindButton,
            this.toolStripSeparator2,
            this.loadLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(619, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // LangComboBox
            // 
            this.LangComboBox.Items.AddRange(new object[] {
            "English",
            "German"});
            this.LangComboBox.Name = "LangComboBox";
            this.LangComboBox.Size = new System.Drawing.Size(121, 25);
            this.LangComboBox.SelectedIndexChanged += new System.EventHandler(this.onLangChange);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // GotoEntry
            // 
            this.GotoEntry.MaxLength = 10;
            this.GotoEntry.Name = "GotoEntry";
            this.GotoEntry.Size = new System.Drawing.Size(100, 25);
            this.GotoEntry.Text = "Goto Nr..";
            // 
            // GotoButton
            // 
            this.GotoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GotoButton.Image = ((System.Drawing.Image)(resources.GetObject("GotoButton.Image")));
            this.GotoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GotoButton.Name = "GotoButton";
            this.GotoButton.Size = new System.Drawing.Size(34, 22);
            this.GotoButton.Text = "Goto";
            this.GotoButton.Click += new System.EventHandler(this.GotoNr);
            // 
            // FindEntry
            // 
            this.FindEntry.Name = "FindEntry";
            this.FindEntry.Size = new System.Drawing.Size(100, 25);
            this.FindEntry.Text = "Entry";
            // 
            // FindButton
            // 
            this.FindButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FindButton.Image = ((System.Drawing.Image)(resources.GetObject("FindButton.Image")));
            this.FindButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(31, 22);
            this.FindButton.Text = "Find";
            this.FindButton.Click += new System.EventHandler(this.FindEntryClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // loadLabel
            // 
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.Size = new System.Drawing.Size(60, 22);
            this.loadLabel.Text = "enu loaded";
            // 
            // Cliloc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "Cliloc";
            this.Size = new System.Drawing.Size(619, 324);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stringEntryBindingSource)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource stringEntryBindingSource;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox LangComboBox;
        private System.Windows.Forms.ToolStripLabel loadLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClilocNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClilocText;
        private System.Windows.Forms.ToolStripTextBox GotoEntry;
        private System.Windows.Forms.ToolStripButton GotoButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox FindEntry;
        private System.Windows.Forms.ToolStripButton FindButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
