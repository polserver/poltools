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

namespace UoFiddler
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.checkBoxAltDesign = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseHash = new System.Windows.Forms.CheckBox();
            this.numericUpDownItemSizeHeight = new System.Windows.Forms.NumericUpDown();
            this.checkBoxItemClip = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownItemSizeWidth = new System.Windows.Forms.NumericUpDown();
            this.checkBoxCacheData = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxNewMapSize = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItemSizeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItemSizeWidth)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxAltDesign
            // 
            this.checkBoxAltDesign.AutoSize = true;
            this.checkBoxAltDesign.Location = new System.Drawing.Point(6, 19);
            this.checkBoxAltDesign.Name = "checkBoxAltDesign";
            this.checkBoxAltDesign.Size = new System.Drawing.Size(112, 17);
            this.checkBoxAltDesign.TabIndex = 0;
            this.checkBoxAltDesign.Text = "Alternative Design";
            this.toolTip1.SetToolTip(this.checkBoxAltDesign, "Alternative layout in item/landtile/texture tab?");
            this.checkBoxAltDesign.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxUseHash);
            this.groupBox1.Controls.Add(this.numericUpDownItemSizeHeight);
            this.groupBox1.Controls.Add(this.checkBoxItemClip);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownItemSizeWidth);
            this.groupBox1.Location = new System.Drawing.Point(14, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Item Tab";
            // 
            // checkBoxUseHash
            // 
            this.checkBoxUseHash.AutoSize = true;
            this.checkBoxUseHash.Location = new System.Drawing.Point(9, 71);
            this.checkBoxUseHash.Name = "checkBoxUseHash";
            this.checkBoxUseHash.Size = new System.Drawing.Size(86, 17);
            this.checkBoxUseHash.TabIndex = 4;
            this.checkBoxUseHash.Text = "Use Hashfile";
            this.toolTip1.SetToolTip(this.checkBoxUseHash, "Use Hashfile to speed up load?");
            this.checkBoxUseHash.UseVisualStyleBackColor = true;
            // 
            // numericUpDownItemSizeHeight
            // 
            this.numericUpDownItemSizeHeight.Location = new System.Drawing.Point(118, 19);
            this.numericUpDownItemSizeHeight.Name = "numericUpDownItemSizeHeight";
            this.numericUpDownItemSizeHeight.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownItemSizeHeight.TabIndex = 3;
            this.toolTip1.SetToolTip(this.numericUpDownItemSizeHeight, "Height");
            // 
            // checkBoxItemClip
            // 
            this.checkBoxItemClip.AutoSize = true;
            this.checkBoxItemClip.Location = new System.Drawing.Point(9, 47);
            this.checkBoxItemClip.Name = "checkBoxItemClip";
            this.checkBoxItemClip.Size = new System.Drawing.Size(66, 17);
            this.checkBoxItemClip.TabIndex = 2;
            this.checkBoxItemClip.Text = "Item Clip";
            this.toolTip1.SetToolTip(this.checkBoxItemClip, "ItemClip images in items tab shrinked or clipped");
            this.checkBoxItemClip.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Item Size";
            this.toolTip1.SetToolTip(this.label1, "ItemSize controls the size of images in items tab");
            // 
            // numericUpDownItemSizeWidth
            // 
            this.numericUpDownItemSizeWidth.Location = new System.Drawing.Point(62, 19);
            this.numericUpDownItemSizeWidth.Name = "numericUpDownItemSizeWidth";
            this.numericUpDownItemSizeWidth.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownItemSizeWidth.TabIndex = 0;
            this.toolTip1.SetToolTip(this.numericUpDownItemSizeWidth, "Width");
            // 
            // checkBoxCacheData
            // 
            this.checkBoxCacheData.AutoSize = true;
            this.checkBoxCacheData.Location = new System.Drawing.Point(6, 42);
            this.checkBoxCacheData.Name = "checkBoxCacheData";
            this.checkBoxCacheData.Size = new System.Drawing.Size(83, 17);
            this.checkBoxCacheData.TabIndex = 2;
            this.checkBoxCacheData.Text = "Cache Data";
            this.toolTip1.SetToolTip(this.checkBoxCacheData, "CacheData should mul entries be cached for faster load");
            this.checkBoxCacheData.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxNewMapSize);
            this.groupBox2.Controls.Add(this.checkBoxAltDesign);
            this.groupBox2.Controls.Add(this.checkBoxCacheData);
            this.groupBox2.Location = new System.Drawing.Point(14, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Misc";
            // 
            // checkBoxNewMapSize
            // 
            this.checkBoxNewMapSize.AutoSize = true;
            this.checkBoxNewMapSize.Location = new System.Drawing.Point(6, 65);
            this.checkBoxNewMapSize.Name = "checkBoxNewMapSize";
            this.checkBoxNewMapSize.Size = new System.Drawing.Size(95, 17);
            this.checkBoxNewMapSize.TabIndex = 3;
            this.checkBoxNewMapSize.Text = "New Map Size";
            this.toolTip1.SetToolTip(this.checkBoxNewMapSize, "NewMapSize Felucca/Trammel width 7168?");
            this.checkBoxNewMapSize.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(77, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClickApply);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 248);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItemSizeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItemSizeWidth)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxAltDesign;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownItemSizeWidth;
        private System.Windows.Forms.CheckBox checkBoxItemClip;
        private System.Windows.Forms.NumericUpDown numericUpDownItemSizeHeight;
        private System.Windows.Forms.CheckBox checkBoxCacheData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxUseHash;
        private System.Windows.Forms.CheckBox checkBoxNewMapSize;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}