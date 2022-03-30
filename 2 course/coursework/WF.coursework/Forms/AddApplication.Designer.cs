
namespace WF.coursework
{
    partial class AddApplication
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
            this.mcPeriodStart = new System.Windows.Forms.MonthCalendar();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.mcPeriodEnd = new System.Windows.Forms.MonthCalendar();
            this.lblVacationDuration = new System.Windows.Forms.Label();
            this.lblPeriodEnd1 = new System.Windows.Forms.Label();
            this.lblPeriodStart1 = new System.Windows.Forms.Label();
            this.cbClassifications = new System.Windows.Forms.ComboBox();
            this.lblClassifications = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.udDuration = new System.Windows.Forms.NumericUpDown();
            this.chbChange = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.udDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // mcPeriodStart
            // 
            this.mcPeriodStart.Location = new System.Drawing.Point(12, 63);
            this.mcPeriodStart.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.mcPeriodStart.MaxSelectionCount = 1;
            this.mcPeriodStart.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.mcPeriodStart.Name = "mcPeriodStart";
            this.mcPeriodStart.TabIndex = 13;
            this.mcPeriodStart.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.mcPeriodStart_DateSelected);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Enabled = false;
            this.btnConfirm.Location = new System.Drawing.Point(194, 304);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(164, 30);
            this.btnConfirm.TabIndex = 10;
            this.btnConfirm.Text = "Отправить заявку";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(164, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // mcPeriodEnd
            // 
            this.mcPeriodEnd.Location = new System.Drawing.Point(194, 63);
            this.mcPeriodEnd.Name = "mcPeriodEnd";
            this.mcPeriodEnd.TabIndex = 20;
            this.mcPeriodEnd.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.mcPeriodEnd_DateSelected);
            // 
            // lblVacationDuration
            // 
            this.lblVacationDuration.AutoSize = true;
            this.lblVacationDuration.Location = new System.Drawing.Point(12, 234);
            this.lblVacationDuration.Name = "lblVacationDuration";
            this.lblVacationDuration.Size = new System.Drawing.Size(80, 13);
            this.lblVacationDuration.TabIndex = 17;
            this.lblVacationDuration.Text = "Дней отпуска:";
            // 
            // lblPeriodEnd1
            // 
            this.lblPeriodEnd1.Location = new System.Drawing.Point(194, 41);
            this.lblPeriodEnd1.Name = "lblPeriodEnd1";
            this.lblPeriodEnd1.Size = new System.Drawing.Size(164, 13);
            this.lblPeriodEnd1.TabIndex = 16;
            this.lblPeriodEnd1.Text = "Конец периода";
            this.lblPeriodEnd1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPeriodStart1
            // 
            this.lblPeriodStart1.Location = new System.Drawing.Point(12, 41);
            this.lblPeriodStart1.Name = "lblPeriodStart1";
            this.lblPeriodStart1.Size = new System.Drawing.Size(164, 13);
            this.lblPeriodStart1.TabIndex = 15;
            this.lblPeriodStart1.Text = "Начало периода";
            this.lblPeriodStart1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbClassifications
            // 
            this.cbClassifications.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClassifications.FormattingEnabled = true;
            this.cbClassifications.Location = new System.Drawing.Point(114, 6);
            this.cbClassifications.Name = "cbClassifications";
            this.cbClassifications.Size = new System.Drawing.Size(244, 21);
            this.cbClassifications.TabIndex = 21;
            this.cbClassifications.SelectedIndexChanged += new System.EventHandler(this.cbClassifications_SelectedIndexChanged);
            // 
            // lblClassifications
            // 
            this.lblClassifications.AutoSize = true;
            this.lblClassifications.Location = new System.Drawing.Point(12, 9);
            this.lblClassifications.Name = "lblClassifications";
            this.lblClassifications.Size = new System.Drawing.Size(96, 13);
            this.lblClassifications.TabIndex = 22;
            this.lblClassifications.Text = "Причина отпуска:";
            // 
            // tbComment
            // 
            this.tbComment.Enabled = false;
            this.tbComment.Location = new System.Drawing.Point(12, 278);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(346, 20);
            this.tbComment.TabIndex = 23;
            this.tbComment.TextChanged += new System.EventHandler(this.tbComment_TextChanged);
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(12, 262);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(133, 13);
            this.lblComment.TabIndex = 24;
            this.lblComment.Text = "Комментарий к периоду:";
            // 
            // udDuration
            // 
            this.udDuration.Location = new System.Drawing.Point(98, 232);
            this.udDuration.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.udDuration.Name = "udDuration";
            this.udDuration.Size = new System.Drawing.Size(78, 20);
            this.udDuration.TabIndex = 25;
            this.udDuration.ValueChanged += new System.EventHandler(this.udDuration_ValueChanged);
            // 
            // chbChange
            // 
            this.chbChange.AutoSize = true;
            this.chbChange.Location = new System.Drawing.Point(278, 261);
            this.chbChange.Name = "chbChange";
            this.chbChange.Size = new System.Drawing.Size(77, 17);
            this.chbChange.TabIndex = 26;
            this.chbChange.Text = "Изменить";
            this.chbChange.UseVisualStyleBackColor = true;
            this.chbChange.CheckedChanged += new System.EventHandler(this.chbChange_CheckedChanged);
            // 
            // AddApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(370, 343);
            this.Controls.Add(this.chbChange);
            this.Controls.Add(this.udDuration);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.lblClassifications);
            this.Controls.Add(this.cbClassifications);
            this.Controls.Add(this.mcPeriodEnd);
            this.Controls.Add(this.lblVacationDuration);
            this.Controls.Add(this.lblPeriodEnd1);
            this.Controls.Add(this.lblPeriodStart1);
            this.Controls.Add(this.mcPeriodStart);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddApplication";
            this.Load += new System.EventHandler(this.AddApplication_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MonthCalendar mcPeriodStart;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MonthCalendar mcPeriodEnd;
        private System.Windows.Forms.Label lblVacationDuration;
        private System.Windows.Forms.Label lblPeriodEnd1;
        private System.Windows.Forms.Label lblPeriodStart1;
        private System.Windows.Forms.ComboBox cbClassifications;
        private System.Windows.Forms.Label lblClassifications;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.NumericUpDown udDuration;
        private System.Windows.Forms.CheckBox chbChange;
    }
}