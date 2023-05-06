using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Reports
{
    /// <summary>
    /// Класс отвечающий за отображение отчёта об успеваемости
    /// </summary>
    public partial class MarkReport : Form
    {
        // поле строки соединения для БД
        private readonly string _connection;

        /// <summary>
        /// Конструктор класса MarkReport
        /// </summary>
        public MarkReport()
        {
            InitializeComponent();
            _connection = ApplicationContext.GetConnectionString();
        }

        // Обработчик загрузки формы
        private void MarkReport_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                string sqlQuery = "SELECT " +
                    "results.id, " +
                    "results.mark, " +
                    "tests.themeName " +
                    "FROM results " +
                    "JOIN users ON users.id = results.userId " +
                    "JOIN tests ON tests.id = results.testId " +
                    $"WHERE users.id = '{ApplicationContext.GetSession().Value.Id}';";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];

                dataGridView1.Columns[0].HeaderText = "ID записи";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Оценка";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[2].HeaderText = "Тема теста";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                connection.Close();
            }
        }

        // Обработчик нажатия на кнопку в главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            new Menu().Show();
        }

        // Обработчик закрытия формы
        private void MarkReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
