using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WF.coursework
{
    public partial class AddApplication : Form
    {
        public SqlConnection sqlConnection = null;
        public static string classification = String.Empty;

        public string period_start = String.Empty;
        public string period_end = String.Empty;
        public int duration = 0;
        public int id_classification = 0;

        public AddApplication()
        {
            var maxYear = DateTime.Now.AddYears(10).Year;

            InitializeComponent();

            mcPeriodStart.MinDate = DateTime.Now;
            mcPeriodEnd.MinDate = DateTime.Now;

            mcPeriodStart.MaxDate = DateTime.Parse($"31.12.{maxYear}");
            mcPeriodEnd.MaxDate = DateTime.Parse($"31.12.{maxYear}");

            period_start = DateTime.Now.ToString("dd.MM.yyyy");

            udDuration.Maximum = (DateTime.Parse($"31.12.{maxYear}") - DateTime.Now).Days + 1;
        }

        private void AddApplication_Load(object sender, EventArgs e)
        {
            cbClassifications.Items.Clear();

            try
            {
                string seletQuery = "SELECT * FROM [Classification_vacation]";
                SqlCommand DPTcmd = new SqlCommand(seletQuery, sqlConnection);
                SqlDataReader reader = DPTcmd.ExecuteReader();
                while (reader.Read())
                {
                    cbClassifications.Items.Add(reader.GetString(2));
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Update_text()
        {
            DateTime temp_date_start;
            DateTime temp_date_end;

            if (period_start != "" || period_start != String.Empty)
            {
                if(duration != 0)
                {
                    temp_date_start = DateTime.Parse(period_start).AddDays(duration);
                    mcPeriodEnd.SelectionStart = temp_date_start;
                    mcPeriodEnd.SelectionEnd = temp_date_start;
                    period_end = temp_date_start.ToString("dd.MM.yyyy");

                    tbComment.Text = $"{duration} дней отпуска с {period_start} по {period_end} по причине {classification}(а)";
                    btnConfirm.Enabled = true;
                }
                else if((period_end != "" || period_end != String.Empty) && period_end != period_start)
                {
                    temp_date_start = DateTime.Parse(period_start);
                    temp_date_end = DateTime.Parse(period_end);
                    duration = (temp_date_end - temp_date_start).Days;

                    tbComment.Text = $"{duration} дней отпуска с {period_start} по {period_end} по причине {classification}(а)";
                    btnConfirm.Enabled = true;
                }
                else
                {
                    btnConfirm.Enabled = false;
                }
            }
            else
            {
                btnConfirm.Enabled = false;
            }
        }

        private void mcPeriodStart_DateSelected(object sender, DateRangeEventArgs e)
        {
            period_start = mcPeriodStart.SelectionStart.ToString("dd.MM.yyyy");
            Update_text();
        }

        private void mcPeriodEnd_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime temp_date;

            period_end = mcPeriodEnd.SelectionStart.ToString("dd.MM.yyyy");
            if(period_start == "" || period_start == String.Empty)
            {
                temp_date = DateTime.Now;
            }
            else
            {
                temp_date = DateTime.Parse(period_start);
            }
            duration = (mcPeriodEnd.SelectionStart - temp_date).Days;
            udDuration.Value = duration;

            Update_text();
        }

        private void udDuration_ValueChanged(object sender, EventArgs e)
        {
            DateTime temp_date;

            duration = Convert.ToInt32(udDuration.Value);

            if(duration == 0)
            {
                tbComment.Clear();
                mcPeriodEnd.SelectionStart = DateTime.Now;
                mcPeriodEnd.SelectionEnd = DateTime.Now;

                period_end = DateTime.Now.ToString("dd.MM.yyyy");
                btnConfirm.Enabled = false;
            }
            else
            {
                if(period_start == "" || period_start == String.Empty)
                {
                    temp_date = DateTime.Now;
                }
                else
                {
                    temp_date = DateTime.Parse(period_start);
                }
                mcPeriodEnd.SelectionStart = temp_date.AddDays(duration);
                mcPeriodEnd.SelectionEnd = temp_date.AddDays(duration);

                period_end = mcPeriodEnd.SelectionStart.ToString("dd.MM.yyyy");

                Update_text();
            }
        }

        private void cbClassifications_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code_classification = String.Empty;
            int period_vacation = 0;
            classification = cbClassifications.Text;

            SqlCommand command = new SqlCommand($"SELECT * FROM [Classification_vacation] WHERE name_classification LIKE N'{classification}'", sqlConnection);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_classification = reader.GetInt32(0);
                        code_classification = reader.GetString(1);
                        period_vacation = reader.GetInt32(3);
                    }
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            if(code_classification == "")
            {
                mcPeriodEnd.Enabled = false;
                udDuration.Enabled = false;
                
                // блокировать изменения длительности, если фиксированный отпуск
            }
            else
            {
                mcPeriodEnd.Enabled = true;
                udDuration.Enabled = true;
            }
            Update_text();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(id_classification == 0)
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM [Classification_vacation] WHERE name_classification LIKE N'{classification}'", sqlConnection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id_classification = reader.GetInt32(0);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            
            Close();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chbChange_CheckedChanged(object sender, EventArgs e)
        {
            if(chbChange.CheckState == CheckState.Checked)
            {
                tbComment.Enabled = true;
            }
            else
            {
                tbComment.Enabled = false;
            }
        }

        private void tbComment_TextChanged(object sender, EventArgs e)
        {
            string text = tbComment.Text.Replace(" ", "");
            if(text == "" || text == String.Empty)
            {
                btnConfirm.Enabled = false;
            }
            else
            {
                btnConfirm.Enabled = true;
            }
        }
    }
}
