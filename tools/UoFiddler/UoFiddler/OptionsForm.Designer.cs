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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.map0Nametext = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.map1Nametext = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.map2Nametext = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.map3Nametext = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.map4Nametext = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmdtext = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.argstext = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItemSizeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItemSizeWidth)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.button1.Location = new System.Drawing.Point(189, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClickApply);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.argstext);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cmdtext);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.map4Nametext);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.map3Nametext);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.map2Nametext);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.map1Nametext);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.map0Nametext);
            this.groupBox3.Location = new System.Drawing.Point(220, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 206);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Map";
            // 
            // map0Nametext
            // 
            this.map0Nametext.Location = new System.Drawing.Point(76, 17);
            this.map0Nametext.Name = "map0Nametext";
            this.map0Nametext.Size = new System.Drawing.Size(100, 20);
            this.map0Nametext.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "map0 Name";
            this.toolTip1.SetToolTip(this.label2, "Definies the map name");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "map1 Name";
            this.toolTip1.SetToolTip(this.label3, "Definies the map name");
            // 
            // map1Nametext
            // 
            this.map1Nametext.Location = new System.Drawing.Point(76, 43);
            this.map1Nametext.Name = "map1Nametext";
            this.map1Nametext.Size = new System.Drawing.Size(100, 20);
            this.map1Nametext.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "map2 Name";
            this.toolTip1.SetToolTip(this.label4, "Definies the map name");
            // 
            // map2Nametext
            // 
            this.map2Nametext.Location = new System.Drawing.Point(76, 69);
            this.map2Nametext.Name = "map2Nametext";
            this.map2Nametext.Size = new System.Drawing.Size(100, 20);
            this.map2Nametext.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "map3 Name";
            this.toolTip1.SetToolTip(this.label5, "Definies the map name");
            // 
            // map3Nametext
            // 
            this.map3Nametext.Location = new System.Drawing.Point(76, 95);
            this.map3Nametext.Name = "map3Nametext";
            this.map3Nametext.Size = new System.Drawing.Size(100, 20);
            this.map3Nametext.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "map4 Name";
            this.toolTip1.SetToolTip(this.label6, "Definies the map name");
            // 
            // map4Nametext
            // 
            this.map4Nametext.Location = new System.Drawing.Point(76, 120);
            this.map4Nametext.Name = "map4Nametext";
            this.map4Nametext.Size = new System.Drawing.Size(100, 20);
            this.map4Nametext.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Cmd";
            this.toolTip1.SetToolTip(this.label7, "Definies the cmd to send Client to loc");
            // 
            // cmdtext
            // 
            this.cmdtext.Location = new System.Drawing.Point(76, 151);
            this.cmdtext.Name = "cmdtext";
            this.cmdtext.Size = new System.Drawing.Size(100, 20);
            this.cmdtext.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 180);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Args";
            this.toolTip1.SetToolTip(this.label8, "{1} = x, {2} = y, {3} = z, {4} = mapid, {5} = mapname");
            // 
            // argstext
            // 
            this.argstext.Location = new System.Drawing.Point(76, 177);
            this.argstext.Name = "argstext";
            this.argstext.Size = new System.Drawing.Size(100, 20);
            this.argstext.TabIndex = 12;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 248);
            this.Controls.Add(this.groupBox3);
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
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox map4Nametext;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox map3Nametext;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox map2Nametext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox map1Nametext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox map0Nametext;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox argstext;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox cmdtext;

    }
}