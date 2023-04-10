using System;
using System.Data;
using System.Windows.Forms;
using LearningApp.Forms.Teacher.Actions.Students;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlDataAdapter = Microsoft.Data.SqlClient.SqlDataAdapter;

namespace LearningApp.Forms.Teacher.Actions
{
    public partial class StudentList : Form
    {
        private readonly string _connection
            = @"Server=(localdb)\mssqllocaldb;Database=RuLearningApp;Trusted_Connection=true";
        public StudentList()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.AllowUserToAddRows = false;
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
                    "SELECT users.id, users.name, users.surname, users.fatherName, users.login, groups.number FROM users JOIN studentAndGroup ON studentAndGroup.userId = users.id JOIN groups ON studentAndGroup.id = groups.id WHERE users.roleId = 2";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];
                dataGridView1.Columns[0].HeaderText = "ID студента";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
            Hide();

            var form = new StudentRegistration();
            form.Closed += (o, args) => { Show(); };
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadDataFromSQL();
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
                        string userDataUpdate = string.Empty;

                        //TODO: сделать апдейт записей пользователей отдельно от других записей

                        foreach (DataRow row in changes) 
                        {
                            userDataUpdate += $"UPDATE users SET users.name = {row[1]}, users.surname = {row[2]}, users.fatherName = {row[3]}, users.login = {row[4]} WHERE users.id = {row[0]}\n";
                        }

                        MessageBox.Show(userDataUpdate);

                        //string query =
                        //    "SELECT users.id, users.name, users.surname, users.fatherName, users.login, groups.number FROM users JOIN studentAndGroup ON studentAndGroup.userId = users.id JOIN groups ON studentAndGroup.id = groups.id WHERE users.roleId = 2";

                        //SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(query, connection);

                        //SqlCommandBuilder mcb = new SqlCommandBuilder(mySqlDataAdapter);
                        //mySqlDataAdapter.UpdateCommand = mcb.GetUpdateCommand();
                        //mySqlDataAdapter.Update(changes);
                        //((DataTable)dataGridView1.DataSource).AcceptChanges();

                        //MessageBox.Show("Cell Updated");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}