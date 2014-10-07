namespace AuxSvcTestClient
{
    partial class AuxSvcTestClient
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
            this.auxRecvText = new System.Windows.Forms.TextBox();
            this.auxSendText = new System.Windows.Forms.TextBox();
            this.testAuxButton = new System.Windows.Forms.Button();
            this.serverHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // auxRecvText
            // 
            this.auxRecvText.BackColor = System.Drawing.SystemColors.Window;
            this.auxRecvText.Location = new System.Drawing.Point(12, 147);
            this.auxRecvText.Name = "auxRecvText";
            this.auxRecvText.ReadOnly = true;
            this.auxRecvText.Size = new System.Drawing.Size(585, 20);
            this.auxRecvText.TabIndex = 11;
            this.auxRecvText.Text = "Text recieved from Aux Svc";
            // 
            // auxSendText
            // 
            this.auxSendText.Location = new System.Drawing.Point(12, 121);
            this.auxSendText.Name = "auxSendText";
            this.auxSendText.Size = new System.Drawing.Size(585, 20);
            this.auxSendText.TabIndex = 10;
            this.auxSendText.Text = "Text to send to the Aux Svc";
            // 
            // testAuxButton
            // 
            this.testAuxButton.Location = new System.Drawing.Point(12, 173);
            this.testAuxButton.Name = "testAuxButton";
            this.testAuxButton.Size = new System.Drawing.Size(75, 23);
            this.testAuxButton.TabIndex = 9;
            this.testAuxButton.Text = "Aux Test";
            this.testAuxButton.UseVisualStyleBackColor = true;
            this.testAuxButton.Click += new System.EventHandler(this.testAuxButton_Click);
            // 
            // serverHost
            // 
            this.serverHost.Location = new System.Drawing.Point(12, 32);
            this.serverHost.Name = "serverHost";
            this.serverHost.Size = new System.Drawing.Size(231, 20);
            this.serverHost.TabIndex = 12;
            this.serverHost.Text = "localhost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Host Name or IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Port";
            // 
            // serverPort
            // 
            this.serverPort.Location = new System.Drawing.Point(12, 75);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(231, 20);
            this.serverPort.TabIndex = 13;
            this.serverPort.Text = "5666";
            // 
            // AuxSvcTestClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 208);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverPort);
            this.Controls.Add(this.serverHost);
            this.Controls.Add(this.auxRecvText);
            this.Controls.Add(this.auxSendText);
            this.Controls.Add(this.testAuxButton);
            this.Name = "AuxSvcTestClient";
            this.Text = "Aux Service Test Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox auxRecvText;
        private System.Windows.Forms.TextBox auxSendText;
        private System.Windows.Forms.Button testAuxButton;
        private System.Windows.Forms.TextBox serverHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPort;
    }
}

