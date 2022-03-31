using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF.coursework
{
    public partial class AddData : Form
    {
        public String var = String.Empty;
        public String callback = String.Empty;

        public AddData()
        {
            InitializeComponent();
        }

        private void AddData_Load(object sender, EventArgs e)
        {
            lblName.Text = $"Новое значение в поле: {var}";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            Close();
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Вы уверены в добавлении нового значения: {tbName.Text}\nВ поле: {var}", $"Добавление нового {var}", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if(result == DialogResult.Yes)
            {
                callback = tbName.Text;
                Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            //сделай запрос на проверку в базе данных
            string empty_check = tbName.Text.Replace(" ", "");
            if (empty_check == "" || empty_check == String.Empty || tbName.Text == "" || tbName.Text == String.Empty)                
                btnAddData.Enabled = false;
            else
                btnAddData.Enabled = true;
        }
    }
}
