using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    /// <summary>
    /// Класс отвечающий за форму со списком тестов
    /// </summary>
    public partial class TestList : Form
    {
        // строка соединения с БД
        private readonly string _connection;

        /// <summary>
        /// Конструктор класса TestList
        /// </summary>
        public TestList()
        {
            InitializeComponent();
            _connection = ApplicationContext.GetConnectionString();
        }

        // Обработчик события при нажатии кнопки Назад
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var menu = new Teacher.Menu();
            menu.Show();
        }

        // Обработчик события при нажатии кнопки Добавить
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();

            new AddTest().Show();
        }

        // Обработчик события загрузки формы
        private void TestList_Load(object sender, EventArgs e)
        {
            LoadDataFromSQL();
        }

        // Метод загружающий в форму данные тестов в DataGridView
        private void LoadDataFromSQL() 
        {
            using (var connection = new SqlConnection(_connection)) 
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
                    Text = "Смотреть",
                    HeaderText = "Действие",
                    UseColumnTextForButtonValue = true
                };

                dataGridView1.Columns.Add(viewButton);
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();
            }
        }

        // Обработчик события при нажатии на ячейку DataGridView
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

        // Обработчик закрытия формы
        private void TestList_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }

        // Обработчик нажатия на кнопку Удалить
        private void button4_Click(object sender, EventArgs e)
        {
            string testId = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            if (string.IsNullOrEmpty(testId))
                return;

            var result = MessageBox.Show(
                $"Тест с ID {testId} будет безвозратно удалён!",
                "Удаление теста",
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
                $"DELETE FROM tests WHERE tests.id = '{testId}';";

            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                var command = new SqlCommand(query);
                command.Connection = connection;
                command.ExecuteNonQuery();

                connection.Close();
            }

            MessageBox.Show("Тест удалён!");

            LoadDataFromSQL();
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        // Событие при нажатии на кнопку обновить
        private void button3_Click(object sender, EventArgs e)
        {
            LoadDataFromSQL();
        }
    }
}
