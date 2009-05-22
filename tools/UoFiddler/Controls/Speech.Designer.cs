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
    partial class Speech
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
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.IDEntry = new System.Windows.Forms.ToolStripTextBox();
            this.IDButton = new System.Windows.Forms.ToolStripButton();
            this.IDNextButton = new System.Windows.Forms.ToolStripButton();
            this.KeyWordEntry = new System.Windows.Forms.ToolStripTextBox();
            this.KeyWordButton = new System.Windows.Forms.ToolStripButton();
            this.KeyWordNextButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.IDEntry,
            this.IDButton,
            this.IDNextButton,
            this.KeyWordEntry,
            this.KeyWordButton,
            this.KeyWordNextButton,
            this.toolStripSeparator2,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(619, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // IDEntry
            // 
            this.IDEntry.MaxLength = 10;
            this.IDEntry.Name = "IDEntry";
            this.IDEntry.Size = new System.Drawing.Size(100, 25);
            this.IDEntry.Text = "Find ID..";
            // 
            // IDButton
            // 
            this.IDButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.IDButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IDButton.Name = "IDButton";
            this.IDButton.Size = new System.Drawing.Size(31, 22);
            this.IDButton.Text = "Find";
            this.IDButton.Click += new System.EventHandler(this.OnClickFindID);
            // 
            // IDNextButton
            // 
            this.IDNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.IDNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IDNextButton.Name = "IDNextButton";
            this.IDNextButton.Size = new System.Drawing.Size(57, 22);
            this.IDNextButton.Text = "Find Next";
            this.IDNextButton.Click += new System.EventHandler(this.OnClickNextID);
            // 
            // KeyWordEntry
            // 
            this.KeyWordEntry.Name = "KeyWordEntry";
            this.KeyWordEntry.Size = new System.Drawing.Size(100, 25);
            this.KeyWordEntry.Text = "KeyWord..";
            // 
            // KeyWordButton
            // 
            this.KeyWordButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.KeyWordButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.KeyWordButton.Name = "KeyWordButton";
            this.KeyWordButton.Size = new System.Drawing.Size(31, 22);
            this.KeyWordButton.Text = "Find";
            this.KeyWordButton.Click += new System.EventHandler(this.OnClickFindKeyWord);
            // 
            // KeyWordNextButton
            // 
            this.KeyWordNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.KeyWordNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.KeyWordNextButton.Name = "KeyWordNextButton";
            this.KeyWordNextButton.Size = new System.Drawing.Size(57, 22);
            this.KeyWordNextButton.Text = "Find Next";
            this.KeyWordNextButton.Click += new System.EventHandler(this.OnClickNextKeyWord);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton1.Text = "Save";
            this.toolStripButton1.Click += new System.EventHandler(this.OnClickSave);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(619, 299);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellValueChanged);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.onHeaderClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEntryToolStripMenuItem,
            this.deleteEntryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(141, 48);
            // 
            // addEntryToolStripMenuItem
            // 
            this.addEntryToolStripMenuItem.Name = "addEntryToolStripMenuItem";
            this.addEntryToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.addEntryToolStripMenuItem.Text = "Add Entry";
            this.addEntryToolStripMenuItem.Click += new System.EventHandler(this.OnAddEntry);
            // 
            // deleteEntryToolStripMenuItem
            // 
            this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
            this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.deleteEntryToolStripMenuItem.Text = "Delete Entry";
            this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.OnDeleteEntry);
            // 
            // Speech
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Speech";
            this.Size = new System.Drawing.Size(619, 324);
            this.Load += new System.EventHandler(this.OnLoad);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox IDEntry;
        private System.Windows.Forms.ToolStripButton IDButton;
        private System.Windows.Forms.ToolStripTextBox KeyWordEntry;
        private System.Windows.Forms.ToolStripButton KeyWordButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton IDNextButton;
        private System.Windows.Forms.ToolStripButton KeyWordNextButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
    }
}
