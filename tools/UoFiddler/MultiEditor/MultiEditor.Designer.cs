/***************************************************************************
 *
 * $Author: MuadDib & Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

namespace FiddlerPlugin
{
    partial class MultiEditor
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
            this.TC_MultiEditorToolbox = new System.Windows.Forms.TabControl();
            this.tileTab = new System.Windows.Forms.TabPage();
            this.designTab = new System.Windows.Forms.TabPage();
            this.importTab = new System.Windows.Forms.TabPage();
            this.TC_MultiEditorToolbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TC_MultiEditorToolbox
            // 
            this.TC_MultiEditorToolbox.Controls.Add(this.tileTab);
            this.TC_MultiEditorToolbox.Controls.Add(this.designTab);
            this.TC_MultiEditorToolbox.Controls.Add(this.importTab);
            this.TC_MultiEditorToolbox.Location = new System.Drawing.Point(4, 4);
            this.TC_MultiEditorToolbox.Name = "TC_MultiEditorToolbox";
            this.TC_MultiEditorToolbox.SelectedIndex = 0;
            this.TC_MultiEditorToolbox.Size = new System.Drawing.Size(171, 317);
            this.TC_MultiEditorToolbox.TabIndex = 0;
            // 
            // tileTab
            // 
            this.tileTab.BackColor = System.Drawing.SystemColors.Window;
            this.tileTab.Location = new System.Drawing.Point(4, 22);
            this.tileTab.Name = "tileTab";
            this.tileTab.Padding = new System.Windows.Forms.Padding(3);
            this.tileTab.Size = new System.Drawing.Size(163, 291);
            this.tileTab.TabIndex = 0;
            this.tileTab.Text = "Tiles";
            this.tileTab.UseVisualStyleBackColor = true;
            // 
            // designTab
            // 
            this.designTab.BackColor = System.Drawing.SystemColors.Window;
            this.designTab.Location = new System.Drawing.Point(4, 22);
            this.designTab.Name = "designTab";
            this.designTab.Padding = new System.Windows.Forms.Padding(3);
            this.designTab.Size = new System.Drawing.Size(163, 291);
            this.designTab.TabIndex = 1;
            this.designTab.Text = "Design";
            this.designTab.UseVisualStyleBackColor = true;
            // 
            // importTab
            // 
            this.importTab.BackColor = System.Drawing.SystemColors.Window;
            this.importTab.Location = new System.Drawing.Point(4, 22);
            this.importTab.Name = "importTab";
            this.importTab.Size = new System.Drawing.Size(163, 291);
            this.importTab.TabIndex = 2;
            this.importTab.Text = "Import";
            this.importTab.UseVisualStyleBackColor = true;
            // 
            // MultiEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TC_MultiEditorToolbox);
            this.Name = "MultiEditor";
            this.Size = new System.Drawing.Size(619, 324);
            this.TC_MultiEditorToolbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TC_MultiEditorToolbox;
        private System.Windows.Forms.TabPage tileTab;
        private System.Windows.Forms.TabPage designTab;
        private System.Windows.Forms.TabPage importTab;
    }
}
