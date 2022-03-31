using Microsoft.VisualBasic;
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
using System.Data.SqlClient;
using WF.coursework;
using System.Data.OleDb;

namespace WF.coursework
{
    public partial class Window : Form
    {
        public SqlConnection sqlConnection = null;

        public static int user_id;
        public static string user_name = String.Empty;
        public static string user_surname = String.Empty;
        public static int is_admin;

        public static int convert_id_department;

        public static int Vacation_balance;

        public Window()
        {
            var currentYear = DateTime.Now.Year;
            var maxYear = DateTime.Now.AddYears(10).Year;

            LoginForm login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                is_admin = login.is_admin;
                user_id = login.user_id;
            }

            InitializeComponent();
            //MessageBox.Show($"{user_id}\n{is_admin}");

            if (is_admin == 0)
            {
                tabControl.TabPages.Remove(tpDepartments);
            }

            lblBalance.Text = $"Баланс на {currentYear} год: {9999}";
            //Добавь в таблицу с юзерами баланс отпуска и напиши запрос на получение баланса

            dtpAppPeriodStart.Value = DateTime.Now;
            dtpAppPeriodEnd.Value = DateTime.Now;

            mcPeriodStart.MinDate = DateTime.Parse($"01.01.{DateTime.Now.Year}");
            mcPeriodEnd.MinDate = DateTime.Parse($"01.01.{DateTime.Now.Year}");

            mcPeriodStart.MaxDate = DateTime.Parse($"31.12.{maxYear}");
            mcPeriodEnd.MaxDate = DateTime.Parse($"31.12.{maxYear}");

            numudAppDuration.Maximum = (DateTime.Parse($"31.12.{maxYear}") - DateTime.Parse($"01.01.{DateTime.Now.Year}")).Days + 1;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD.coursework"].ConnectionString);
            sqlConnection.Open();

            Update_applicationForVacations();
            Update_dgvApplication();

            Gantt_chart_year_load();

            Update_departments();
            Update_posts();
            Update_gender();

            Update_applications();
            //загугли как вносить данные из sql в datagrid view         

            //добавить такое на стр387
            SqlCommand command = new SqlCommand( $"SELECT * FROM [Workers] WHERE id_worker = {user_id}", sqlConnection);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user_surname = reader.GetString(1);
                        user_name = reader.GetString(2);

                        user_surname = user_surname.Replace(" ", "");
                        user_name = user_name.Replace(" ", "");
                        MainMenu.Text = $"{user_name} {user_surname}";
                    }
                    reader.Close();
                }                
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        #region Взаимодействие с окном

        #region Движение окна
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }
        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        #region Альтернативное закрытие
        private void Pic_close_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void Pic_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void TSbtnExit_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        #endregion

        #endregion

        #region Мои отпуска
        private void btnAddVacation_Click(object sender, EventArgs e)
        {
            int id_classification = 0;
            string period_start = string.Empty;
            string period_end = string.Empty;
            int duration = 0;
            AddApplication new_vacation = new AddApplication();
            new_vacation.sqlConnection = sqlConnection;

            if (new_vacation.ShowDialog() == DialogResult.OK)
            {
                id_classification = new_vacation.id_classification;
                period_start = new_vacation.period_start;
                period_end = new_vacation.period_end;
                duration = new_vacation.duration;
            }
            //запрос на добавление нового отпуска
            SqlCommand command = new SqlCommand($"INSERT INTO [Application_for_vacation] (date_begin_vacation, vacation_count) " +
                $"VALUES (@date_begin_vacation, @vacation_count)", sqlConnection);

            
            DateTime parsed_date = DateTime.Parse(period_start);


            
            command.Parameters.AddWithValue("vacation_count", duration);
            command.Parameters.AddWithValue("date_begin_vacation", $"{parsed_date.Month}/{parsed_date.Day}/{parsed_date.Year}");
            command.ExecuteNonQuery();
            MessageBox.Show(period_start, duration.ToString());
            //command.Parameters.AddWithValue("id_worker", cbVacation_count_reeal.Text);
            //command.Parameters.AddWithValue("id_status_application", tbID_classification_vacation.Text);
            //command.Parameters.AddWithValue("id_classification_vacation", tbID_classification_vacation.Text);
            //, id_worker, id_status_application, id_classification_vacation
            //@id_worker, @id_status_application, id_classification_vacation
        }

        private void Update_applicationForVacations()
        {
            cbApplications.Items.Clear();

            try
            {
                SqlCommand DPTcmd = new SqlCommand($"SELECT * FROM [Application_for_vacation] WHERE id_worker = {user_id}", sqlConnection);
                SqlDataReader reader = DPTcmd.ExecuteReader();
                while (reader.Read())
                {
                    cbApplications.Items.Add(reader.GetDateTime(1).ToString());
                }
                //if(cbDepartment.SelectedItem != null)
                //{
                //    MessageBox.Show(cbDepartment.Text);
                //}
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"Update my applications\nSQL Application for vacation\n{ex.Message}"); }
        }

        private void cbApplications_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnChangeVacation_Click(object sender, EventArgs e)
        {
            //запрос на добавление новой истории по отпуску
            //SqlCommand command = new SqlCommand($"UPDATE [Vacations] (id_application, id_worker, date_vacation_real, vacation_count_real, id_classification_vacation) " +
            //    $"VALUES (@id_application, @id_worker, @date_vacation_real, @vacation_count_real, @id_classification_vacation)", sqlConnection);

            //string date = dtpdate_vacation.Value.ToString("dd.MM.yyyy");
            //DateTime parsed_date = DateTime.Parse(date);


            //command.Parameters.AddWithValue("id_application", tbID_application.Text);
            //command.Parameters.AddWithValue("id_worker", tbID_worker.Text);
            //command.Parameters.AddWithValue("date_vacation_real", $"{parsed_date.Month}/{parsed_date.Day}/{parsed_date.Year}");
            //command.Parameters.AddWithValue("vacation_count_real", cbVacation_count_reeal.Text);
            //command.Parameters.AddWithValue("id_classification_vacation", tbID_classification_vacation.Text);
        }
        #endregion

        #region Проекты
        GanttChart ganttChart_year;
        GanttChart ganttChart_mounth;

        private void Gantt_chart_year_load()
        {
            panelMonth.Visible = false;
            panelYear.Visible = true;

            panelMonth.Dock = DockStyle.Right;
            panelYear.Dock = DockStyle.Fill;

            ganttChart_year = new GanttChart();
            ganttChart_year.AllowChange = false;
            ganttChart_year.Dock = DockStyle.Fill;
            ganttChart_year.FromDate = new DateTime(2015, 1, 1, 0, 0, 0);
            ganttChart_year.ToDate = new DateTime(2015, 12, 31, 0, 0, 0);
            panelYear.Controls.Add(ganttChart_year);

            ganttChart_year.MouseMove += new MouseEventHandler(ganttChart_year.GanttChart_MouseMove);
            ganttChart_year.MouseDragged += new MouseEventHandler(ganttChart_year.GanttChart_MouseDragged);
            ganttChart_year.MouseLeave += new EventHandler(ganttChart_year.GanttChart_MouseLeave);
            //ganttChart3.ToolTip.Draw += new DrawToolTipEventHandler(ganttChart3.ToolTipText_Draw);
            //ganttChart3.ToolTip.Popup += new PopupEventHandler(ganttChart3.ToolTipText_Popup);
            //ganttChart_year.ContextMenuStrip = ContextMenuGanttChart1;


            List<BarInformation> lst3 = new List<BarInformation>();

            lst3.Add(new BarInformation("Row 1", new DateTime(2015, 1, 1), new DateTime(2015, 5, 1), Color.Gray, Color.LightGray, 0));
            lst3.Add(new BarInformation("Row 2", new DateTime(2015, 1, 1), new DateTime(2015, 7, 1), Color.Gray, Color.LightGray, 1));
            lst3.Add(new BarInformation("Row 3", new DateTime(2015, 5, 1), new DateTime(2015, 8, 1), Color.Gray, Color.LightGray, 2));
            lst3.Add(new BarInformation("Row 2", new DateTime(2015, 10, 1), new DateTime(2015, 12, 1), Color.Gray, Color.LightGray, 3));
            lst3.Add(new BarInformation("Row 2", new DateTime(2015, 10, 1), new DateTime(2015, 12, 1), Color.Gray, Color.LightGray, 3));
            lst3.Add(new BarInformation("Row 2", new DateTime(2015, 10, 1), new DateTime(2015, 12, 1), Color.Gray, Color.LightGray, 3));
            lst3.Add(new BarInformation("Row 1", new DateTime(2015, 8, 1), new DateTime(2016, 01, 1), Color.Gray, Color.LightGray, 4));

            foreach (BarInformation bar in lst3)
            {
                ganttChart_year.AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index);
            }
        }

        private void Gantt_chart_mounth_load()
        {
            panelMonth.Visible = true;
            panelYear.Visible = false;

            panelYear.Dock = DockStyle.Right;
            panelMonth.Dock = DockStyle.Fill;

            ganttChart_mounth = new GanttChart();
            ganttChart_mounth.AllowChange = false;
            ganttChart_mounth.Dock = DockStyle.Fill;
            ganttChart_mounth.FromDate = new DateTime(2015, 12, 12, 0, 0, 0);
            ganttChart_mounth.ToDate = new DateTime(2015, 12, 24, 0, 0, 0);
            panelMonth.Controls.Add(ganttChart_mounth);

            ganttChart_mounth.MouseMove += new MouseEventHandler(ganttChart_mounth.GanttChart_MouseMove);
            ganttChart_mounth.MouseMove += new MouseEventHandler(GanttChartMounth_MouseMove);
            ganttChart_mounth.MouseDragged += new MouseEventHandler(ganttChart_mounth.GanttChart_MouseDragged);
            ganttChart_mounth.MouseLeave += new EventHandler(ganttChart_mounth.GanttChart_MouseLeave);
            //ganttChart1.ContextMenuStrip = ContextMenuGanttChart1;

            List<BarInformation> lst1 = new List<BarInformation>();

            lst1.Add(new BarInformation("Row 1", new DateTime(2015, 12, 12), new DateTime(2015, 12, 16), Color.Aqua, Color.Khaki, 0));
            lst1.Add(new BarInformation("Row 2", new DateTime(2015, 12, 13), new DateTime(2015, 12, 20), Color.AliceBlue, Color.Khaki, 1));
            lst1.Add(new BarInformation("Row 3", new DateTime(2015, 12, 14), new DateTime(2015, 12, 24), Color.Violet, Color.Khaki, 2));
            lst1.Add(new BarInformation("Row 2", new DateTime(2015, 12, 21), new DateTime(2015, 12, 22, 12, 0, 0), Color.Yellow, Color.Khaki, 1));
            lst1.Add(new BarInformation("Row 1", new DateTime(2015, 12, 17), new DateTime(2015, 12, 24), Color.LawnGreen, Color.Khaki, 0));

            foreach (BarInformation bar in lst1)
            {
                ganttChart_mounth.AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index);
            }
        }

        private void GanttChartMounth_MouseMove(Object sender, MouseEventArgs e)
        {
            List<string> toolTipText = new List<string>();

            if (ganttChart_mounth.MouseOverRowText.Length > 0)
            {
                BarInformation val = (BarInformation)ganttChart_mounth.MouseOverRowValue;
                toolTipText.Add("[b]Date:");
                toolTipText.Add("From ");
                toolTipText.Add(val.FromTime.ToLongDateString() + " - " + val.FromTime.ToString("HH:mm"));
                toolTipText.Add("To ");
                toolTipText.Add(val.ToTime.ToLongDateString() + " - " + val.ToTime.ToString("HH:mm"));
            }
            else
            {
                toolTipText.Add("");
            }

            ganttChart_mounth.ToolTipTextTitle = ganttChart_mounth.MouseOverRowText;
            ganttChart_mounth.ToolTipText = toolTipText;
        }

        private void btnChangeGantt_Click(object sender, EventArgs e)
        {
            if (btnChangeGantt.Text == "Месяц")
            {
                btnChangeGantt.Text = "Год";
                Gantt_chart_mounth_load();
            }   
            else if (btnChangeGantt.Text == "Год")
            {
                btnChangeGantt.Text = "Месяц";
                Gantt_chart_year_load();
            }    
        }
        #endregion

        #region Подразделения
        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_workers();
            //делаем запрос и проверку как с логином
            //command.Parameters.AddWithValue("что то", cbDepartment.Text);
            //for(int i, i <= table.Rows.Count, i++)
            //if (cbDepartment.Text == название в таблице)
            //lbUsers.Items.Add([название в таблице].имя\фамилия\таб номер);
        }

        private void Update_workers()
        {
            lbUsers.Items.Clear();

            SqlCommand command = new SqlCommand($"SELECT * FROM [Department] WHERE department LIKE N'{cbDepartment.Text}'", sqlConnection);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        convert_id_department = reader.GetInt32(0);
                    }
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            SqlCommand WRKcmd = new SqlCommand($"SELECT * FROM [Workers] WHERE id_department = {convert_id_department}", sqlConnection);
            SqlDataReader dataReader = WRKcmd.ExecuteReader();
            if (dataReader.HasRows)
            {
                lbUsers.BeginUpdate();
                while (dataReader.Read())
                {
                    lbUsers.Items.Add($"{dataReader.GetString(1).Replace(" ", "")} {dataReader.GetString(2).Replace(" ", "")} [{dataReader.GetInt32(0)}]");
                }
                lbUsers.EndUpdate();
            }
            dataReader.Close();
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_worker_fields();
            btnDeleteUser.Enabled = true;

            int id_worker = 0;
            int id_department = 0;
            int id_post = 0;
            int id_gender = 0;

            string tabWorker_department = String.Empty;
            string tabWorker_post = String.Empty;
            string tabWorker_gender = String.Empty;
            int tabWorker_admin = 0;
            int tabWorker_manager = 0;

            if (lbUsers.SelectedItem != null || lbUsers.SelectedIndex >= 0)
                id_worker = Convert.ToInt32(lbUsers.SelectedItem.ToString().Replace("[", "").Replace("]", "").Split(' ')[2]);
            else
                return;

            lblUserID.Text = $"ID: {id_worker}";

            SqlCommand command = new SqlCommand($"SELECT * FROM [Workers] WHERE id_worker = {id_worker}", sqlConnection);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tbSurname.Text = reader.GetString(1);
                        tbName.Text = reader.GetString(2);
                        if (reader.IsDBNull(4) == true)
                            tbTabNum.Text = "0";
                        else
                            tbTabNum.Text = reader.GetString(4);

                        id_department = reader.GetInt32(10);
                        id_post = reader.GetInt32(5);

                        if (reader.IsDBNull(6) == true)
                            tbMail.Text = "0";
                        else
                            tbMail.Text = reader.GetString(6);

                        if (reader.IsDBNull(7) == true)
                            tbPhone.Text = "0";
                        else
                            tbPhone.Text = reader.GetInt32(7).ToString();

                        dtpDateHired.Value = reader.GetDateTime(8);
                        id_gender = reader.GetInt32(9);

                        //нет поля менеджер
                    }
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Workers {ex.Message}"); }

            SqlCommand login_command = new SqlCommand($"SELECT * FROM [log_pass] WHERE id_worker = {id_worker}", sqlConnection);
            try
            {
                SqlDataReader login_reader = login_command.ExecuteReader();
                if (login_reader.HasRows)
                {
                    while (login_reader.Read())
                    {
                        tbLogin.Text = login_reader.GetString(1);
                        tbPassword.Text = login_reader.GetString(2);
                        tabWorker_admin = login_reader.GetInt32(4);
                    }
                }
                login_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Login {ex.Message}"); }

            SqlCommand department_command = new SqlCommand($"SELECT * FROM [Department] WHERE id_department = {id_department}", sqlConnection);
            try
            {
                SqlDataReader department_reader = department_command.ExecuteReader();
                if (department_reader.HasRows)
                {
                    while (department_reader.Read())
                    {
                        tabWorker_department = department_reader.GetString(1);
                    }
                }
                department_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Department {ex.Message}"); }

            SqlCommand post_command = new SqlCommand($"SELECT * FROM [Posts] WHERE id_post = {id_post}", sqlConnection);
            try
            {
                SqlDataReader post_reader = post_command.ExecuteReader();
                if (post_reader.HasRows)
                {
                    while (post_reader.Read())
                    {
                        tabWorker_post = post_reader.GetString(1);
                    }
                }
                post_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Posts {ex.Message}"); }

            SqlCommand gender_command = new SqlCommand($"SELECT * FROM [gender] WHERE id_gender = {id_gender}", sqlConnection);
            try
            {
                SqlDataReader gender_reader = gender_command.ExecuteReader();
                if (gender_reader.HasRows)
                {
                    while (gender_reader.Read())
                    {
                        tabWorker_gender = gender_reader.GetString(1);
                    }
                }
                gender_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Gender {ex.Message}"); }

            //tabWorker_departament = tabWorker_departament.Replace(" ", "");
            //tabWorker_post = tabWorker_post.Replace(" ", "");
            //tabWorker_gender = tabWorker_gender.Replace(" ", "");

            if (!string.IsNullOrEmpty(tabWorker_department))
            {
                int index = cbUserDepartment.FindStringExact(tabWorker_department);
                if (index != ListBox.NoMatches)
                    cbUserDepartment.SelectedIndex = index;
                else
                    MessageBox.Show("Департамент не найден");
            }
            else
                return;

            if (!string.IsNullOrEmpty(tabWorker_post))
            {
                int index = cbPost.FindStringExact(tabWorker_post);
                if (index != ListBox.NoMatches)
                    cbPost.SelectedIndex = index;
                else
                    MessageBox.Show("Должность не найдена");
            }
            else
                return;
            if (!string.IsNullOrEmpty(tabWorker_gender))
            {
                int index = cbGender.FindStringExact(tabWorker_gender);
                if (index != ListBox.NoMatches)
                    cbGender.SelectedIndex = index;
                else
                    MessageBox.Show("Пол не найден");
            }
            else
                return;

            if (tabWorker_admin == 1)
                chbAdmin.Checked = true;
            else
                chbAdmin.Checked = false;


            //запрос на получение чела по таб номеру из лист бокса
            //потом добавляем все его данные из таблицы в комбобоксы
        }

        #region Департаменты
        private void Update_departments()
        {
            cbDepartment.Items.Clear();
            cbUserDepartment.Items.Clear();

            try
            {
                string seletQuery = "SELECT * FROM [Department]";
                SqlCommand DPTcmd = new SqlCommand(seletQuery, sqlConnection);
                SqlDataReader reader = DPTcmd.ExecuteReader();
                while (reader.Read())
                {
                    cbDepartment.Items.Add(reader.GetString(1));
                    cbUserDepartment.Items.Add(reader.GetString(1));
                }
                //if(cbDepartment.SelectedItem != null)
                //{
                //    MessageBox.Show(cbDepartment.Text);
                //}
                reader.Close();
            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAddDepartment_Click(object sender, EventArgs e)
        {
            string name = "[Department]";
            string new_department = String.Empty;

            AddData add_data = new AddData();
            add_data.var = "[Департаменты]";
            if (add_data.ShowDialog() == DialogResult.OK)
            {
                new_department = add_data.callback;
                if (new_department != "" || new_department != String.Empty)
                {
                    cbUserDepartment.Text = new_department;
                    SqlCommand command = new SqlCommand($"INSERT INTO {name} (department) " + $"VALUES (@department)", sqlConnection);

                    command.Parameters.AddWithValue("department", new_department);
                    MessageBox.Show($"Добавлено {command.ExecuteNonQuery()} значений в таблицу {name}",
                        $"Добавление в базу данных",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    //запрос на добавление нового депратамента  
                }
                else
                    return;
            }
            Update_departments();
        }

        private void btnDeleteDepartment_Click(object sender, EventArgs e)
        {
            string to_delete = String.Empty;

            DialogResult result = MessageBox.Show($"Вы уверены в удалении значения: {cbUserDepartment.Text}\nИз таблицы: [Департаменты]", $"Удаление из [Департаменты]", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                SqlCommand command = new SqlCommand($"DELETE FROM Department WHERE department LIKE N'{cbUserDepartment.Text}'", sqlConnection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            btnDeleteDepartment.Enabled = false;

            Update_departments();
        }

        private void cbUserDepartment_TextChanged(object sender, EventArgs e)
        {
            string empty_check = cbUserDepartment.Text.Replace(" ", "");
            if (empty_check == "" || empty_check == String.Empty || cbUserDepartment.Text == "" || cbUserDepartment.Text == String.Empty)
                btnDeleteDepartment.Enabled = false;
            else
                btnDeleteDepartment.Enabled = true;
        }
        #endregion

        #region Должности
        private void Update_posts()
        {
            cbPost.Items.Clear();

            try
            {
                string selectQuery= "SELECT * FROM [Posts]";
                SqlCommand PSTcmd = new SqlCommand(selectQuery, sqlConnection);
                SqlDataReader reader = PSTcmd.ExecuteReader();
                while (reader.Read())
                {
                    cbPost.Items.Add(reader.GetString(1));
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAddPost_Click(object sender, EventArgs e)
        {
            string new_post = String.Empty;

            AddData add_data = new AddData();
            add_data.var = "[Должности]";
            if (add_data.ShowDialog() == DialogResult.OK)
            {
                new_post = add_data.callback;
                if (new_post != "" || new_post != String.Empty)
                {
                    cbUserDepartment.Text = new_post;
                    SqlCommand command = new SqlCommand($"INSERT INTO [Posts] (post) " + $"VALUES (@post )", sqlConnection);
                    command.Parameters.AddWithValue("post", new_post);
                    MessageBox.Show($"Добавлено {command.ExecuteNonQuery()} значений в таблицу [Posts]",
                        $"Добавление в базу данных",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Update_posts();
                    //сделть запрос на добавление новой должности
                    //и на удаление
                    //все то же самое что наверху
                }
                else
                    return;
            }
            
        }

        private void btnDeletePost_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Вы уверены в удалении значения: {cbPost.Text}\nИз таблицы: [Должности]", $"Удаление из [Должности]", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                SqlCommand command = new SqlCommand($"DELETE FROM Posts WHERE post LIKE N'{cbPost.Text}'", sqlConnection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
            btnDeletePost.Enabled = false;
            Update_posts();
        }        

        private void cbPost_TextChanged(object sender, EventArgs e)
        {
            string empty_check = cbPost.Text.Replace(" ", "");
            if (empty_check == "" || empty_check == String.Empty || cbPost.Text == "" || cbPost.Text == String.Empty)
                btnDeletePost.Enabled = false;
            else
                btnDeletePost.Enabled = true;
        }
        #endregion
        
        private void btnNewUser_Click(object sender, EventArgs e)
        {
            Clear_worker_fields();
            if (btnNewUser.Text == "Новый работник")
            {
                btnNewUser.Text = "Отменить";
                tbSurname.Enabled = true;
                tbName.Enabled = true;
                tbTabNum.Enabled = true;
                cbUserDepartment.Enabled = true;
                cbPost.Enabled = true;
                tbPhone.Enabled = true;
                dtpDateHired.Enabled = true;
                cbGender.Enabled = true;
                tbLogin.Enabled = true;
                tbPassword.Enabled = true;
                chbAdmin.Enabled = true;
                chbManadger.Enabled = true;

                btnAddUser.Enabled = true;
                btnDeleteUser.Enabled = false;
            }
            else if (btnNewUser.Text == "Отменить")
            {
                btnNewUser.Text = "Новый работник";
                tbSurname.Enabled = false;
                tbName.Enabled = false;
                tbTabNum.Enabled = false;
                cbUserDepartment.Enabled = false;
                cbPost.Enabled = false;
                tbPhone.Enabled = false;
                dtpDateHired.Enabled = false;
                cbGender.Enabled = false;
                tbLogin.Enabled = false;
                tbPassword.Enabled = false;
                chbAdmin.Enabled = false;
                chbManadger.Enabled = false;

                btnAddUser.Enabled = false;
            }
        }

        private void btnChangeInfo_Click(object sender, EventArgs e)
        {
            if (btnChangeInfo.Text == "Изменить")
            {
                btnChangeInfo.Text = "Отменить";
                chbAdmin.Enabled = true;
                chbManadger.Enabled = true;
                tbSurname.Enabled = true;
                tbName.Enabled = true;
                tbTabNum.Enabled = true;
                cbUserDepartment.Enabled = true;
                cbPost.Enabled = true;
                tbMail.Enabled = true;
                tbPhone.Enabled = true;
                dtpDateHired.Enabled = true;
                cbGender.Enabled = true;
                tbPassword.Enabled = true;

                btnChangeUserInfo.Enabled = true;
                btnDeleteUser.Enabled = false;
            }
            else if (btnChangeInfo.Text == "Отменить")
            {
                btnChangeInfo.Text = "Изменить";
                chbAdmin.Enabled = false;
                chbManadger.Enabled = false;
                tbSurname.Enabled = false;
                tbName.Enabled = false;
                tbTabNum.Enabled = false;
                cbUserDepartment.Enabled = false;
                cbPost.Enabled = false;
                tbMail.Enabled = false;
                tbPhone.Enabled = false;
                dtpDateHired.Enabled = false;
                cbGender.Enabled = false;
                tbPassword.Enabled = false;

                btnChangeUserInfo.Enabled = false;
            }
        }

        private void Clear_worker_fields()
        {
            lblUserID.Text = "ID: 0";
            tbSurname.Clear();
            tbName.Clear();
            tbTabNum.Clear();
            tbMail.Clear();
            tbPhone.Clear();
            dtpDateHired.Value = DateTime.Now;
            tbLogin.Clear();
            tbPassword.Clear();
            chbAdmin.Checked = false;
            chbManadger.Checked = false;
            chbAdmin.Enabled = false;
            chbManadger.Enabled = false;
            tbSurname.Enabled = false;
            tbName.Enabled = false;
            tbTabNum.Enabled = false;
            cbUserDepartment.Enabled = false;
            cbPost.Enabled = false;
            tbMail.Enabled = false;
            tbPhone.Enabled = false;
            dtpDateHired.Enabled = false;
            cbGender.Enabled = false;
            tbPassword.Enabled = false;

            btnChangeInfo.Text = "Изменить";
            btnAddUser.Enabled = false;
            btnChangeUserInfo.Enabled = false;
            btnDeleteUser.Enabled = false;
        }

        private void Update_gender()
        {
            cbGender.Items.Clear();

            try
            {
                SqlCommand GNDcmd = new SqlCommand("SELECT * FROM [Gender]", sqlConnection);
                SqlDataReader reader = GNDcmd.ExecuteReader();
                while (reader.Read())
                {
                    cbGender.Items.Add(reader.GetString(1));
                }reader.Close();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }            
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string name = "[Workers]";
            SqlCommand command = new SqlCommand($"INSERT INTO {name} (surname, name, service_number, phone, date_hiring,) " +//id_post
                $"VALUES (@surname, @name, @service_number, @phone, @date_hiring)", sqlConnection);//@id_post

            string date = dtpDateHired.Value.ToString("dd.MM.yyyy");
            DateTime parsed_date = DateTime.Parse(date);

            //написать запрос на получение id департамента и id должности от названия

            command.Parameters.AddWithValue("surname", tbSurname.Text);
            command.Parameters.AddWithValue("name", tbName.Text);
            command.Parameters.AddWithValue("service_number", tbTabNum.Text);
            //command.Parameters.AddWithValue("id_post", cbPost.Text);
            command.Parameters.AddWithValue("phone", tbPhone.Text);
            command.Parameters.AddWithValue("date_hiring", $"{parsed_date.Month}/{parsed_date.Day}/{parsed_date.Year}");
            MessageBox.Show($"Добавлено {command.ExecuteNonQuery()} значений в таблицу {name}", 
                $"Добавление в базу данных", 
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            //Update_workers();
        }

        private void btnChangeUserInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Заявки
        public static DateTime tabAppl_period_start = DateTime.Now;
        public static DateTime tabAppl_period_end = DateTime.Now;
        public static string tabAppl_classification = String.Empty;
        public static int tabAppl_duration;
        public static bool print_order = false;

        private void Update_applications()
        {
            lbApplications.Items.Clear();
            SqlCommand command = new SqlCommand($"SELECT * FROM [Application_for_vacation] WHERE id_status_application = {1}", sqlConnection);
            
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                lbApplications.BeginUpdate();
                while (reader.Read())
                {
                    lbApplications.Items.Add($"№ {reader.GetInt32(0)}");
                }
                lbApplications.EndUpdate();
            }
            reader.Close();            
        }

        private void btnUpdateApplications_Click(object sender, EventArgs e)
        {
            Clear_application_fields();
            Update_applications();
        }

        private void lbApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_application_fields();
            btnAppReject.Enabled = true;
            btnAppAccept.Enabled = true;

            int id_application;
            int id_worker = 0;
            int id_calssification = 0;
            string code_calssification = String.Empty;
            int period_vacation = 0;

            string appl_worker_name = String.Empty;
            string appl_worker_surname = String.Empty;
            string appl_worker_gender = String.Empty;
            string appl_worker_tabnum = String.Empty;
            int id_department = 0;
            int id_post = 0;
            int id_gender = 0;

            if (lbApplications.SelectedItem != null || lbApplications.SelectedIndex >= 0)
                id_application = Convert.ToInt32((lbApplications.SelectedItem.ToString()).Replace("№ ", ""));
            else
                return;
            
            lblApplicationID.Text = $"ID: {id_application}";
            
            SqlCommand command = new SqlCommand($"SELECT * FROM [Application_for_vacation] WHERE id_application = {id_application}", sqlConnection);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tabAppl_period_start = reader.GetDateTime(1);
                        tabAppl_duration = reader.GetInt32(2);
                        id_worker = reader.GetInt32(3);
                        id_calssification = reader.GetInt32(5);
                    }
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Application {ex.Message}"); }

            SqlCommand worker_command = new SqlCommand($"SELECT * FROM [Workers] WHERE id_worker = {id_worker}", sqlConnection);
            try
            {
                SqlDataReader worker_reader = worker_command.ExecuteReader();
                if (worker_reader.HasRows)
                {
                    while (worker_reader.Read())
                    {
                        appl_worker_name = worker_reader.GetString(2);
                        appl_worker_surname = worker_reader.GetString(1);
                        appl_worker_tabnum = worker_reader.GetString(4);
                        id_department = worker_reader.GetInt32(10);
                        id_post = worker_reader.GetInt32(5);
                        id_gender = worker_reader.GetInt32(9);
                    }
                }
                worker_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Workers {ex.Message}"); }

            SqlCommand department_command = new SqlCommand($"SELECT * FROM [Department] WHERE id_department = {id_department}", sqlConnection);
            try
            {
                SqlDataReader department_reader = department_command.ExecuteReader();
                if (department_reader.HasRows)
                {
                    while (department_reader.Read())
                    {
                        tbAppDepartment.Text = department_reader.GetString(1);
                    }
                }
                department_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Department {ex.Message}"); }

            SqlCommand post_command = new SqlCommand($"SELECT * FROM [Posts] WHERE id_post = {id_post}", sqlConnection);
            try
            {
                SqlDataReader post_reader = post_command.ExecuteReader();
                if (post_reader.HasRows)
                {
                    while (post_reader.Read())
                    {
                        tbAppPost.Text = post_reader.GetString(1);
                    }
                }
                post_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Posts {ex.Message}"); }

            SqlCommand classification_command = new SqlCommand($"SELECT * FROM [Classification_vacation] WHERE id_classification_vacation = {id_calssification} or id_classification_vacation IS NULL", sqlConnection);
            try
            {
                SqlDataReader classification_reader = classification_command.ExecuteReader();
                if (classification_reader.HasRows)
                {
                    while (classification_reader.Read())
                    {   
                        code_calssification = classification_reader.GetString(1);
                        tabAppl_classification = classification_reader.GetString(2);
                        if (classification_reader.IsDBNull(3) == true)
                            period_vacation = 0;
                        else
                            period_vacation = classification_reader.GetInt32(3);
                    }
                }
                classification_reader.Close();
            }
            catch (Exception ex) { MessageBox.Show($"SQL Classification {ex.Message}"); }

            if (id_gender == 1)
                appl_worker_gender = "М";
            else if (id_gender == 2)
                appl_worker_gender = "Ж";

            tbAppReason.Text = tabAppl_classification;
            lblWorkerInfo.Text = $"{appl_worker_name.Replace(" ", "")} {appl_worker_surname.Replace(" ", "")} ({appl_worker_gender.Replace(" ", "")}) Табельный номер: {appl_worker_tabnum}";

            dtpAppPeriodStart.Value = tabAppl_period_start;
            mcPeriodStart.SelectionStart = tabAppl_period_start;
            mcPeriodStart.SelectionEnd = tabAppl_period_start;

            numudAppDuration.Value = tabAppl_duration;

            tabAppl_period_end = tabAppl_period_start.AddDays(tabAppl_duration);
            dtpAppPeriodEnd.Value = tabAppl_period_end;
            mcPeriodStart.SelectionStart = tabAppl_period_start;
            mcPeriodStart.SelectionEnd = tabAppl_period_start;

            Update_text();
        }

        private void btnAppChange_Click(object sender, EventArgs e)
        {
            if(btnAppChange.Text == "Изменить")
            {
                btnAppChange.Text = "Отменить";
                dtpAppPeriodStart.Enabled = true;
                dtpAppPeriodEnd.Enabled = true;
                numudAppDuration.Enabled = true;
                btnAppAccept.Enabled = false;
                btnAppReject.Enabled = false;
                btnAppUpdate.Enabled = true;
            }
            else if(btnAppChange.Text == "Отменить")
            {
                btnAppChange.Text = "Изменить";
                dtpAppPeriodStart.Enabled = false;
                dtpAppPeriodEnd.Enabled = false;
                numudAppDuration.Enabled = false;
                btnAppReject.Enabled = true;
                btnAppUpdate.Enabled = false;
            }    
        }

        private void btnAppReject_Click(object sender, EventArgs e)
        {
            Clear_application_fields();
            Update_applications();
        }

        private void btnAppUpdate_Click(object sender, EventArgs e)
        {
            Clear_application_fields();
            Update_applications();
        }

        private void btnAppAccept_Click(object sender, EventArgs e)
        {
            Clear_application_fields();
            Update_applications();
        }

        private void dtpAppPeriodStart_ValueChanged(object sender, EventArgs e)
        {
            tabAppl_period_start = dtpAppPeriodStart.Value;
            mcPeriodStart.SelectionStart = tabAppl_period_start;
            mcPeriodStart.SelectionEnd = tabAppl_period_start;
            Update_text();
        }

        private void dtpAppPeriodEnd_ValueChanged(object sender, EventArgs e)
        {
            DateTime temp_date;

            string period_start = tabAppl_period_start.ToString("dd.MM.yyyy");

            tabAppl_period_end = dtpAppPeriodEnd.Value;
            mcPeriodEnd.SelectionStart = tabAppl_period_end;
            mcPeriodEnd.SelectionEnd = tabAppl_period_end;
            if (period_start == "" || period_start == String.Empty)
            {
                temp_date = DateTime.Now;
            }
            else
            {
                temp_date = DateTime.Parse(period_start);
            }
            tabAppl_duration = (mcPeriodEnd.SelectionStart - temp_date).Days;
            numudAppDuration.Value = tabAppl_duration;

            Update_text();
        }

        private void numudAppDuration_ValueChanged(object sender, EventArgs e)
        {
            DateTime temp_date;
            string period_start = tabAppl_period_start.ToString();

            tabAppl_duration = Convert.ToInt32(numudAppDuration.Value);

            if (tabAppl_duration == 0)
            {
                tbComment.Clear();
                mcPeriodEnd.SelectionStart = DateTime.Now;
                mcPeriodEnd.SelectionEnd = DateTime.Now;

                btnAppUpdate.Enabled = false;
            }
            else
            {
                if (period_start == "" || period_start == String.Empty)
                {
                    temp_date = DateTime.Now;
                }
                else
                {
                    temp_date = DateTime.Parse(period_start);
                }
                mcPeriodEnd.SelectionStart = temp_date.AddDays(tabAppl_duration);
                mcPeriodEnd.SelectionEnd = temp_date.AddDays(tabAppl_duration);

                tabAppl_period_end = mcPeriodEnd.SelectionStart;

                Update_text();
            }
        }

        private void btnShowCalendar_Click(object sender, EventArgs e)
        {

        }

        private void Update_text()
        {
            DateTime temp_date_start;
            DateTime temp_date_end;

            string period_start = tabAppl_period_start.ToString("dd.MM.yyyy");
            string period_end = tabAppl_period_end.ToString("dd.MM.yyyy");

            if (period_start != "" || period_start != String.Empty)
            {
                if (tabAppl_duration != 0)
                {
                    temp_date_start = DateTime.Parse(period_start).AddDays(tabAppl_duration);
                    mcPeriodEnd.SelectionStart = temp_date_start;
                    mcPeriodEnd.SelectionEnd = temp_date_start;
                    tabAppl_period_end = temp_date_start;

                    tbComment.Text = $"{tabAppl_duration} дней отпуска с {period_start} по {period_end} по причине {tabAppl_classification}(а)";
                    }
                else if ((period_end != "" || period_end != String.Empty) && period_end != period_start)
                {
                    temp_date_start = DateTime.Parse(period_start);
                    temp_date_end = tabAppl_period_end;
                    tabAppl_duration = (temp_date_end - temp_date_start).Days;

                    tbComment.Text = $"{tabAppl_duration} дней отпуска с {period_start} по {period_end} по причине {tabAppl_classification}(а)";
                }
            }
            else
            {
                btnAppUpdate.Enabled = false;
            }
        }

        private void tbComment_TextChanged(object sender, EventArgs e)
        {
            string text = tbComment.Text.Replace(" ", "");
            if (text == "" || text == String.Empty)
            {
                btnAppAccept.Enabled = false;
            }
            else
            {
                btnAppAccept.Enabled = true;
            }
        }

        private void chbChange_CheckedChanged(object sender, EventArgs e)
        {
            if (chbChange.CheckState == CheckState.Checked)
            {
                tbComment.Enabled = true;
            }
            else
            {
                tbComment.Enabled = false;
            }
        }

        private void chbPrintOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (chbChange.CheckState == CheckState.Checked)
            {
                print_order = true;
            }
            else
            {
                print_order = false;
            }
        }

        private void Clear_application_fields()
        {
            lblApplicationID.Text = "ID: 0";
            lblWorkerInfo.Text = "Фамилия Имя (пол) Табельный номер: 0";
            tbAppDepartment.Clear();
            tbAppPost.Clear();
            dtpAppPeriodStart.Value = DateTime.Now;
            dtpAppPeriodEnd.Value = DateTime.Now;
            mcPeriodStart.SelectionStart = DateTime.Now;
            mcPeriodStart.SelectionEnd = DateTime.Now;
            mcPeriodEnd.SelectionStart = DateTime.Now;
            mcPeriodEnd.SelectionEnd = DateTime.Now;
            numudAppDuration.Value = 0;
            tbAppReason.Clear();
            tbComment.Clear();
            chbChange.Checked = false;
            chbPrintOrder.Checked = false;

            btnAppChange.Text = "Изменить";
            dtpAppPeriodStart.Enabled = false;
            dtpAppPeriodEnd.Enabled = false;
            numudAppDuration.Enabled = false;
            btnAppReject.Enabled = false;
            btnAppUpdate.Enabled = false;
            btnAppAccept.Enabled = false;
        }
        #endregion

        private void btn_test_Click(object sender, EventArgs e)
        {
            tbSurname.Text = "Иванов";
            tbName.Text = "Иван";
            tbTabNum.Text = "Иванович";
            cbPost.Text = "1";
            cbPost.Text = "1";
            tbPhone.Text = "1234";
            dtpDateHired.Value = new DateTime(2000, 01, 01, 0, 0, 0);
            cbGender.Text = "Мужской";
        }

        private void Update_dgvApplication()
        {
            //try
            //{
            //    SqlCommand command = new SqlCommand("SELECT * FROM [Application_for_vacation] ORDER BY id_application", sqlConnection);
            //    SqlDataReader reader = command.ExecuteReader();
            //    //List<string[]> data = new List<string[]>();
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            //data.Add(new string[6]);

            //            //data[data.Count - 1][0] = reader[0].ToString();
            //            //dgwApplication.Rows.Add()
            //            MessageBox.Show(reader.GetString(1));
            //        }
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
