using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using LearningApp.Forms.Teacher.Actions.Students;
using Microsoft.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlDataAdapter = Microsoft.Data.SqlClient.SqlDataAdapter;

namespace LearningApp.Forms.Teacher.Actions
{
    public partial class StudentList : Form
    {
        private readonly string _connection;

        public StudentList()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.AllowUserToAddRows = false;

            _connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var menu = new Teacher.Menu();
            menu.Show();
        }

        private void LoadDataFromSQL()
        {
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                string query =
                    "SELECT " +
                    "users.id, " +
                    "users.name, " +
                    "users.surname, " +
                    "users.fatherName, " +
                    "users.login, " +
                    "groups.number " +
                    "FROM users " +
                    "LEFT JOIN studentAndGroup ON studentAndGroup.userId = users.id " +
                    "LEFT JOIN groups ON groups.id = studentAndGroup.groupId " +
                    "WHERE users.roleId = 2";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];
                dataGridView1.Columns[0].HeaderText = "ID студента";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].ReadOnly = true;

                dataGridView1.Columns[1].HeaderText = "Имя";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[2].HeaderText = "Фамилия";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[3].HeaderText = "Отчество";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[4].HeaderText = "Логин";
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[5].HeaderText = "Группа";
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();
            }
        }

        private void StudentList_Load(object sender, EventArgs e)
        {
            LoadDataFromSQL();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new StudentRegistration();
            form.FormClosed += (a, c) => Application.Exit();
            form.Show();

            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadDataFromSQL();
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                try
                {
                    var changes = ((DataTable)dataGridView1.DataSource).GetChanges().Rows;

                    if (changes != null)
                    {
                        string query = string.Empty; 

                        foreach (DataRow row in changes) 
                        {
                            query += $"UPDATE users SET name = '{row[1]}', surname = '{row[2]}', fatherName = '{row[3]}', login = '{row[4]}' WHERE id = '{row[0]}';\n";
                            query += $"UPDATE studentAndGroup SET groupId=(SELECT id FROM groups WHERE number = '{row[5]}') WHERE userId = '{row[0]}';\n";
                        }

                        if (connection == null)
                            throw new NullReferenceException("Соединение с БД отсутствует или нет ссылки на экземпляр соединения!");

                        connection.Open();

                        var command = new SqlCommand(query);
                        command.Connection = connection;
                        command.ExecuteNonQuery();

                        connection.Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string userId = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            if (string.IsNullOrEmpty(userId))
                return;

            var result = MessageBox.Show(
                $"Пользователь с ID {userId} будет безвозратно удалён!", 
                "Удаление пользователя", 
                MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) 
            {
                MessageBox.Show(
                    "Удаление прервано!", 
                    "Внимание", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string query =
                $"DELETE FROM studentAndGroup WHERE userId = {userId};" +
                $"DELETE FROM users WHERE users.id = {userId};";

            using (var connection = new SqlConnection(_connection)) 
            {
                connection.Open();
                
                var command = new SqlCommand(query);
                command.Connection = connection;
                command.ExecuteNonQuery();
                
                connection.Close();
            }

            MessageBox.Show("Пользователь удалён!");

            LoadDataFromSQL();
            dataGridView1.Update();
            dataGridView1.Refresh();

        }
    }
}