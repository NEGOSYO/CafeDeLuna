using System;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    partial class AdminDashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminDashboardForm));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupboxTotalSalesToday = new System.Windows.Forms.GroupBox();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxTotalTransactions = new System.Windows.Forms.GroupBox();
            this.lblTotalTransaction = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxLowStock = new System.Windows.Forms.GroupBox();
            this.lblLowStock = new System.Windows.Forms.Label();
            this.groupBoxTotalUsers = new System.Windows.Forms.GroupBox();
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridViewRecentTransactions = new System.Windows.Forms.DataGridView();
            this.Searchbtn2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.Searchbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Deletebtn = new System.Windows.Forms.Button();
            this.Clearbtn = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupboxTotalSalesToday.SuspendLayout();
            this.groupBoxTotalTransactions.SuspendLayout();
            this.groupBoxLowStock.SuspendLayout();
            this.groupBoxTotalUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecentTransactions)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(147)))), ((int)(((byte)(125)))));
            this.panelHeader.Controls.Add(this.label2);
            this.panelHeader.Controls.Add(this.pictureBox1);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1003, 85);
            this.panelHeader.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(410, 55);
            this.label2.TabIndex = 2;
            this.label2.Text = "Admin Dashboard";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(635, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(695, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Admin Name";
            // 
            // groupboxTotalSalesToday
            // 
            this.groupboxTotalSalesToday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(231)))), ((int)(((byte)(200)))));
            this.groupboxTotalSalesToday.Controls.Add(this.lblTotalSales);
            this.groupboxTotalSalesToday.Controls.Add(this.label3);
            this.groupboxTotalSalesToday.Location = new System.Drawing.Point(35, 91);
            this.groupboxTotalSalesToday.Name = "groupboxTotalSalesToday";
            this.groupboxTotalSalesToday.Size = new System.Drawing.Size(200, 104);
            this.groupboxTotalSalesToday.TabIndex = 1;
            this.groupboxTotalSalesToday.TabStop = false;
            // 
            // lblTotalSales
            // 
            this.lblTotalSales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(227)))), ((int)(((byte)(213)))));
            this.lblTotalSales.Location = new System.Drawing.Point(39, 49);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(110, 23);
            this.lblTotalSales.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(39, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Total Sales Today";
            // 
            // groupBoxTotalTransactions
            // 
            this.groupBoxTotalTransactions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(231)))), ((int)(((byte)(200)))));
            this.groupBoxTotalTransactions.Controls.Add(this.lblTotalTransaction);
            this.groupBoxTotalTransactions.Controls.Add(this.label4);
            this.groupBoxTotalTransactions.Location = new System.Drawing.Point(261, 91);
            this.groupBoxTotalTransactions.Name = "groupBoxTotalTransactions";
            this.groupBoxTotalTransactions.Size = new System.Drawing.Size(200, 104);
            this.groupBoxTotalTransactions.TabIndex = 2;
            this.groupBoxTotalTransactions.TabStop = false;
            // 
            // lblTotalTransaction
            // 
            this.lblTotalTransaction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(227)))), ((int)(((byte)(213)))));
            this.lblTotalTransaction.Location = new System.Drawing.Point(42, 49);
            this.lblTotalTransaction.Name = "lblTotalTransaction";
            this.lblTotalTransaction.Size = new System.Drawing.Size(110, 23);
            this.lblTotalTransaction.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(39, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Total Transactions";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(39, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Low Stock Items";
            // 
            // groupBoxLowStock
            // 
            this.groupBoxLowStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(231)))), ((int)(((byte)(200)))));
            this.groupBoxLowStock.Controls.Add(this.lblLowStock);
            this.groupBoxLowStock.Controls.Add(this.label5);
            this.groupBoxLowStock.Location = new System.Drawing.Point(489, 91);
            this.groupBoxLowStock.Name = "groupBoxLowStock";
            this.groupBoxLowStock.Size = new System.Drawing.Size(200, 104);
            this.groupBoxLowStock.TabIndex = 3;
            this.groupBoxLowStock.TabStop = false;
            // 
            // lblLowStock
            // 
            this.lblLowStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(227)))), ((int)(((byte)(213)))));
            this.lblLowStock.Location = new System.Drawing.Point(39, 49);
            this.lblLowStock.Name = "lblLowStock";
            this.lblLowStock.Size = new System.Drawing.Size(110, 23);
            this.lblLowStock.TabIndex = 2;
            // 
            // groupBoxTotalUsers
            // 
            this.groupBoxTotalUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(231)))), ((int)(((byte)(200)))));
            this.groupBoxTotalUsers.Controls.Add(this.lblTotalUsers);
            this.groupBoxTotalUsers.Controls.Add(this.label6);
            this.groupBoxTotalUsers.Location = new System.Drawing.Point(729, 91);
            this.groupBoxTotalUsers.Name = "groupBoxTotalUsers";
            this.groupBoxTotalUsers.Size = new System.Drawing.Size(158, 104);
            this.groupBoxTotalUsers.TabIndex = 4;
            this.groupBoxTotalUsers.TabStop = false;
            // 
            // lblTotalUsers
            // 
            this.lblTotalUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(227)))), ((int)(((byte)(213)))));
            this.lblTotalUsers.Location = new System.Drawing.Point(22, 49);
            this.lblTotalUsers.Name = "lblTotalUsers";
            this.lblTotalUsers.Size = new System.Drawing.Size(110, 23);
            this.lblTotalUsers.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(36, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Total Users";
            // 
            // dataGridViewRecentTransactions
            // 
            this.dataGridViewRecentTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRecentTransactions.Location = new System.Drawing.Point(12, 240);
            this.dataGridViewRecentTransactions.Name = "dataGridViewRecentTransactions";
            this.dataGridViewRecentTransactions.Size = new System.Drawing.Size(664, 342);
            this.dataGridViewRecentTransactions.TabIndex = 7;
            // 
            // Searchbtn2
            // 
            this.Searchbtn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(147)))), ((int)(((byte)(125)))));
            this.Searchbtn2.Location = new System.Drawing.Point(729, 302);
            this.Searchbtn2.Name = "Searchbtn2";
            this.Searchbtn2.Size = new System.Drawing.Size(144, 44);
            this.Searchbtn2.TabIndex = 10;
            this.Searchbtn2.Text = "Search Transaction";
            this.Searchbtn2.UseVisualStyleBackColor = false;
            this.Searchbtn2.Click += new System.EventHandler(this.Searchbtn2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "Recent Transactions:";
            // 
            // Searchbox
            // 
            this.Searchbox.Location = new System.Drawing.Point(698, 259);
            this.Searchbox.Name = "Searchbox";
            this.Searchbox.Size = new System.Drawing.Size(190, 20);
            this.Searchbox.TabIndex = 13;
            this.Searchbox.TextChanged += new System.EventHandler(this.Searchbox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(682, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Search";
            // 
            // Deletebtn
            // 
            this.Deletebtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(147)))), ((int)(((byte)(125)))));
            this.Deletebtn.Location = new System.Drawing.Point(729, 352);
            this.Deletebtn.Name = "Deletebtn";
            this.Deletebtn.Size = new System.Drawing.Size(144, 44);
            this.Deletebtn.TabIndex = 15;
            this.Deletebtn.Text = "Delete Transaction";
            this.Deletebtn.UseVisualStyleBackColor = false;
            // 
            // Clearbtn
            // 
            this.Clearbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(147)))), ((int)(((byte)(125)))));
            this.Clearbtn.Location = new System.Drawing.Point(729, 402);
            this.Clearbtn.Name = "Clearbtn";
            this.Clearbtn.Size = new System.Drawing.Size(144, 44);
            this.Clearbtn.TabIndex = 16;
            this.Clearbtn.Text = "Clear Transaction";
            this.Clearbtn.UseVisualStyleBackColor = false;
            // 
            // AdminDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(207)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(920, 647);
            this.Controls.Add(this.Clearbtn);
            this.Controls.Add(this.Deletebtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Searchbox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Searchbtn2);
            this.Controls.Add(this.dataGridViewRecentTransactions);
            this.Controls.Add(this.groupBoxTotalUsers);
            this.Controls.Add(this.groupBoxLowStock);
            this.Controls.Add(this.groupBoxTotalTransactions);
            this.Controls.Add(this.groupboxTotalSalesToday);
            this.Controls.Add(this.panelHeader);
            this.Name = "AdminDashboardForm";
            this.Text = "AdminDashboardForm";
            this.Load += new System.EventHandler(this.AdminDashboardForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupboxTotalSalesToday.ResumeLayout(false);
            this.groupboxTotalSalesToday.PerformLayout();
            this.groupBoxTotalTransactions.ResumeLayout(false);
            this.groupBoxTotalTransactions.PerformLayout();
            this.groupBoxLowStock.ResumeLayout(false);
            this.groupBoxLowStock.PerformLayout();
            this.groupBoxTotalUsers.ResumeLayout(false);
            this.groupBoxTotalUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecentTransactions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupboxTotalSalesToday;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxTotalTransactions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBoxLowStock;
        private System.Windows.Forms.GroupBox groupBoxTotalUsers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridViewRecentTransactions;
        private System.Windows.Forms.Button Searchbtn2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label lblTotalTransaction;
        private System.Windows.Forms.Label lblLowStock;
        private System.Windows.Forms.Label lblTotalUsers;
        private TextBox Searchbox;
        private Label label7;
        private Button Deletebtn;
        private Button Clearbtn;
    }
}