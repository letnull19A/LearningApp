using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    public partial class TestList : Form
    {
        public TestList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var menu = new Teacher.Menu();
            menu.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();

            new AddTest().Show();
        }

        private void TestList_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(ApplicationContext.GetConnectionString())) 
            {
                
                connection.Open();

                string query =
                    "SELECT tests.id, tests.themeName, COUNT(test_questions.testId) AS countOfQuestions " +
                    "FROM tests " +
                    "LEFT JOIN test_questions ON test_questions.testId = tests.id " +
                    "GROUP BY tests.id, tests.themeName;";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];

                dataGridView1.Columns[0].HeaderText = "ID теста";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[1].HeaderText = "Тема теста";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[2].HeaderText = "Количество вопросов";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                var viewButton = new DataGridViewButtonColumn
                {
                    Text = "Смотреть",
                    HeaderText = "Действие",
                    UseColumnTextForButtonValue = true
                };

                dataGridView1.Columns.Add(viewButton);
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[3].Index && e.RowIndex >= 0) 
            {
                var form = new ViewTest();
                form.Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                form.Show();

                Hide();
            }
        }
    }
}
