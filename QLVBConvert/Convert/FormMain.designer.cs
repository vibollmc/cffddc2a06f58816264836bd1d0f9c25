namespace Convert
{
    partial class FormMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSQLServerNguon = new System.Windows.Forms.TextBox();
            this.Convertbtn = new System.Windows.Forms.Button();
            this.progressBarChitiet = new System.Windows.Forms.ProgressBar();
            this.progressBarTong = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCSDLNguon = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSQLServerDich = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCSDLDich = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLoginNguon = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPassNguon = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLoginDich = new System.Windows.Forms.TextBox();
            this.txtPassDich = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lbTable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SQL Server Nguồn:";
            // 
            // txtSQLServerNguon
            // 
            this.txtSQLServerNguon.Enabled = false;
            this.txtSQLServerNguon.Location = new System.Drawing.Point(118, 49);
            this.txtSQLServerNguon.Name = "txtSQLServerNguon";
            this.txtSQLServerNguon.Size = new System.Drawing.Size(228, 20);
            this.txtSQLServerNguon.TabIndex = 1;
            // 
            // Convertbtn
            // 
            this.Convertbtn.Location = new System.Drawing.Point(508, 158);
            this.Convertbtn.Name = "Convertbtn";
            this.Convertbtn.Size = new System.Drawing.Size(102, 26);
            this.Convertbtn.TabIndex = 2;
            this.Convertbtn.Text = "Convert";
            this.Convertbtn.UseVisualStyleBackColor = true;
            this.Convertbtn.Click += new System.EventHandler(this.Convertbtn_Click);
            // 
            // progressBarChitiet
            // 
            this.progressBarChitiet.Location = new System.Drawing.Point(15, 207);
            this.progressBarChitiet.Name = "progressBarChitiet";
            this.progressBarChitiet.Size = new System.Drawing.Size(681, 23);
            this.progressBarChitiet.TabIndex = 3;
            // 
            // progressBarTong
            // 
            this.progressBarTong.Location = new System.Drawing.Point(15, 248);
            this.progressBarTong.Name = "progressBarTong";
            this.progressBarTong.Size = new System.Drawing.Size(681, 23);
            this.progressBarTong.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "CSDL Nguồn:";
            // 
            // txtCSDLNguon
            // 
            this.txtCSDLNguon.Enabled = false;
            this.txtCSDLNguon.Location = new System.Drawing.Point(118, 75);
            this.txtCSDLNguon.Name = "txtCSDLNguon";
            this.txtCSDLNguon.Size = new System.Drawing.Size(228, 20);
            this.txtCSDLNguon.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(232, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(259, 26);
            this.label3.TabIndex = 0;
            this.label3.Text = "CONVERT DATABASE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(368, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "SQL Server Đích:";
            // 
            // txtSQLServerDich
            // 
            this.txtSQLServerDich.Enabled = false;
            this.txtSQLServerDich.Location = new System.Drawing.Point(466, 49);
            this.txtSQLServerDich.Name = "txtSQLServerDich";
            this.txtSQLServerDich.Size = new System.Drawing.Size(230, 20);
            this.txtSQLServerDich.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(368, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "CSDL Đích:";
            // 
            // txtCSDLDich
            // 
            this.txtCSDLDich.Enabled = false;
            this.txtCSDLDich.Location = new System.Drawing.Point(466, 75);
            this.txtCSDLDich.Name = "txtCSDLDich";
            this.txtCSDLDich.Size = new System.Drawing.Size(230, 20);
            this.txtCSDLDich.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Login:";
            // 
            // txtLoginNguon
            // 
            this.txtLoginNguon.Enabled = false;
            this.txtLoginNguon.Location = new System.Drawing.Point(118, 101);
            this.txtLoginNguon.Name = "txtLoginNguon";
            this.txtLoginNguon.Size = new System.Drawing.Size(80, 20);
            this.txtLoginNguon.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(204, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Password:";
            // 
            // txtPassNguon
            // 
            this.txtPassNguon.Enabled = false;
            this.txtPassNguon.Location = new System.Drawing.Point(266, 101);
            this.txtPassNguon.Name = "txtPassNguon";
            this.txtPassNguon.PasswordChar = '*';
            this.txtPassNguon.Size = new System.Drawing.Size(80, 20);
            this.txtPassNguon.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(368, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Login:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(554, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Password:";
            // 
            // txtLoginDich
            // 
            this.txtLoginDich.Enabled = false;
            this.txtLoginDich.Location = new System.Drawing.Point(466, 104);
            this.txtLoginDich.Name = "txtLoginDich";
            this.txtLoginDich.Size = new System.Drawing.Size(82, 20);
            this.txtLoginDich.TabIndex = 1;
            // 
            // txtPassDich
            // 
            this.txtPassDich.Enabled = false;
            this.txtPassDich.Location = new System.Drawing.Point(616, 104);
            this.txtPassDich.Name = "txtPassDich";
            this.txtPassDich.PasswordChar = '*';
            this.txtPassDich.Size = new System.Drawing.Size(80, 20);
            this.txtPassDich.TabIndex = 1;
            // 
            // lbTable
            // 
            this.lbTable.AutoSize = true;
            this.lbTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbTable.Location = new System.Drawing.Point(29, 171);
            this.lbTable.Name = "lbTable";
            this.lbTable.Size = new System.Drawing.Size(84, 17);
            this.lbTable.TabIndex = 4;
            this.lbTable.Text = "Convert....";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 291);
            this.Controls.Add(this.lbTable);
            this.Controls.Add(this.progressBarTong);
            this.Controls.Add(this.progressBarChitiet);
            this.Controls.Add(this.Convertbtn);
            this.Controls.Add(this.txtCSDLDich);
            this.Controls.Add(this.txtPassDich);
            this.Controls.Add(this.txtLoginDich);
            this.Controls.Add(this.txtPassNguon);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtLoginNguon);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtCSDLNguon);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSQLServerDich);
            this.Controls.Add(this.txtSQLServerNguon);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.Text = "Convert QLVB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSQLServerNguon;
        private System.Windows.Forms.Button Convertbtn;
        private System.Windows.Forms.ProgressBar progressBarChitiet;
        private System.Windows.Forms.ProgressBar progressBarTong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCSDLNguon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSQLServerDich;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCSDLDich;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLoginNguon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPassNguon;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLoginDich;
        private System.Windows.Forms.TextBox txtPassDich;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lbTable;
    }
}

