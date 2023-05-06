using LearningApp.Forms.Student.Actions.Tests;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions
{
    /// <summary>
    /// Класс отвечающий за отображения списка с тестами
    /// </summary>
    public partial class TestList : Form
    {
        /// <summary>
        /// Конструктор класса TestList
        /// </summary>
        public TestList()
        {
            InitializeComponent();
        }

        // Обрабтчик нажатия на кнопку выхода главного меню
        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();

            var menu = new Student.Menu();
            menu.Show();
        }

        // Обработчик события загрузки формы
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
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Тема теста";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[2].HeaderText = "Количество вопросов";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                var viewButton = new DataGridViewButtonColumn
                {
                    Text = "Пройти",
                    HeaderText = "Действие",
                    UseColumnTextForButtonValue = true
                };

                dataGridView1.Columns.Add(viewButton);
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();

            }
        }

        // обработчик нажатия на начейку DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[3].Index && e.RowIndex >= 0)
            {
                var form = new TestExecution();
                form.Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                form.Show();

                Hide();
            }
        }

        // Обработчик события загрузки формы
        private void TestList_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}