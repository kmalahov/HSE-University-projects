
namespace WF.coursework
{
    partial class AddData
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddData = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(24, 65);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddData
            // 
            this.btnAddData.Enabled = false;
            this.btnAddData.Location = new System.Drawing.Point(144, 65);
            this.btnAddData.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(100, 28);
            this.btnAddData.TabIndex = 1;
            this.btnAddData.Text = "Добавить";
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(50, 35);
            this.tbName.Margin = new System.Windows.Forms.Padding(4);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(168, 22);
            this.tbName.TabIndex = 2;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(-2, 11);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(277, 20);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Новое значение в поле: [Name]";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddData
            // 
            this.AcceptButton = this.btnAddData;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(270, 110);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.btnAddData);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddData";
            this.Load += new System.EventHandler(this.AddData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddData;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
    }
}