using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySqlConnector;
using System.Data.SqlClient;

namespace WF.coursework
{
    public partial class LoginForm : Form
    {
        public int is_admin;
        public int user_id;
        private SqlConnection sqlConnection = null;

        public LoginForm()
        {
            InitializeComponent();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD.coursework"].ConnectionString); sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD.coursework"].ConnectionString);
            sqlConnection.Open();
        }

        private void Enter_button_Click(object sender, EventArgs e)
        {
            if((loginBox.Text != "" || loginBox.Text != String.Empty) && (passBox.Text != "" || passBox.Text != String.Empty))
            {
                DataTable table = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand command = new SqlCommand("SELECT * FROM [log_pass] WHERE [login] = @lU AND [password] = @pU", sqlConnection);

                command.Parameters.AddWithValue("lU", loginBox.Text);
                command.Parameters.AddWithValue("pU", passBox.Text);

                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    //this.Hide();
                    //Window window = new Window();
                    //window.Show();

                    DataRow[] currentRows = table.Select(null, null, DataViewRowState.CurrentRows);

                    foreach (DataRow row in currentRows)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ToString() == "id_worker")
                                user_id = Convert.ToInt32(row[column]);
                            if (column.ToString() == "admin")
                                is_admin = Convert.ToInt32(row[column]);
                        }
                    }
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль");
                }

                //DataRow[] currentRows = table.Select(null, null, DataViewRowState.CurrentRows);
                //string tet = "";

                //foreach (DataColumn column in table.Columns)
                //    tet += ("\n{0}", column.ColumnName);

                //foreach (DataRow row in currentRows)
                //{
                //    foreach (DataColumn column in table.Columns)
                //        tet += ("\n{0}", row[column]);
                //    tet += ("\n" + row.RowState);
                //}
                //MessageBox.Show(tet);
            }
            else
            {
                MessageBox.Show("Введите логин и пароль");
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //админ
            loginBox.Text = "root";
            passBox.Text = "123qwe";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //не админ
            loginBox.Text = "adhjns";
            passBox.Text = "123";
        }

        private void Pic_close_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}
