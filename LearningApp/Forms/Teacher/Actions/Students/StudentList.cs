using System;
using System.Data;
using System.Windows.Forms;
using LearningApp.Forms.Teacher.Actions.Students;
using Microsoft.Data.SqlClient;

namespace LearningApp.Forms.Teacher.Actions
{
    public partial class StudentList : Form
    {
        #if Release
        private readonly string _connection
            = @"Server=(localdb)\mssqllocaldb;Database=RuLearningApp;Trusted_Connection=true";
        #endif
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
            #if Release
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                string query =
                    "SELECT users.id, users.name, users.surname, users.fatherName, users.login, studentAndGroup.id, groups.number FROM users JOIN studentAndGroup ON studentAndGroup.userId = users.id JOIN groups ON studentAndGroup.id = groups.id WHERE users.roleId = 1";

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
                dataGridView1.Columns[3].HeaderText = "Логин";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].HeaderText = "Группа";
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();
            }
            #endif
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
    }
}