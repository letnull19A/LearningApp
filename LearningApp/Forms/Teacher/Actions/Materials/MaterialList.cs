using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Materials
{
    /// <summary>
    /// Класс формы MatrialList
    /// </summary>
    public partial class MaterialList : Form
    {
        // Строка соединения с БД
        private readonly string _connection;

        /// <summary>
        /// Конструктор класа MaterialList
        /// </summary>
        public MaterialList()
        {
            InitializeComponent();
            _connection = ApplicationContext.GetConnectionString();
        }

        // Обработчик нажатия на кнопку в главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            new Menu().Show();
        }

        // Обработчик нажатия на кнопку Добавить
        private void button3_Click(object sender, EventArgs e)
        {
            new MaterialAdd().ShowDialog();
        }

        // Обработчик собятия загрузки формы
        private void MaterialList_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM materials ORDER BY materials.title";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];

                dataGridView1.Columns[0].HeaderText = "ID материала";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Файл";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[2].HeaderText = "Заголовок";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();
            }
        }

        // Обработчик нажатия кнопки Обновить
        private void button2_Click(object sender, EventArgs e)
        {
            MaterialList_Load(sender, e);
        }

        // Обработчик нажатия кнопки Удалить
        private void button4_Click(object sender, EventArgs e)
        {
            string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            if (string.IsNullOrEmpty(id))
                return;

            var result = MessageBox.Show(
                $"Иатериал с ID {id} будет безвозратно удалён!",
                "Удаление материала",
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
                $"DELETE FROM materials WHERE id = '{id}';";

            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                var command = new SqlCommand(query);
                command.Connection = connection;
                command.ExecuteNonQuery();

                connection.Close();
            }

            MessageBox.Show("Материал удалён!");

            MaterialList_Load(sender, e);
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        // Обработчик события закрытия формы
        private void MaterialList_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
