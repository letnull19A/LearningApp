using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Reports
{
    /// <summary>
    /// Класс формы MarkReport
    /// </summary>
    public partial class MarkReport : Form
    {
        //
        private readonly string _connection;

        ///
        ///
        ///
        public MarkReport()
        {
            InitializeComponent();
            _connection = ApplicationContext.GetConnectionString();
        }

        // Обработчик нажатия на кнопку В главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            new Menu().Show();
        }

        // Обработчик собятия загрузки формы
        private void MarkReport_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                string sqlQuery = "SELECT " +
                    "results.id, " +
                    "users.name, " +
                    "users.surname, " +
                    "users.fatherName, " +
                    "results.mark, " +
                    "tests.themeName " +
                    "FROM results " +
                    "JOIN users ON users.id = results.userId " +
                    "JOIN tests ON tests.id = results.testId " +
                    "WHERE users.roleId = 2;";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];

                dataGridView1.Columns[0].HeaderText = "ID записи";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Имя";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[2].HeaderText = "Фамилия";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[3].HeaderText = "Отчество";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[4].HeaderText = "Оценка";
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[5].HeaderText = "Тема теста";
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                connection.Close();
            }
        }

        // Обреботчик собятия закрытия формы
        private void MarkReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
