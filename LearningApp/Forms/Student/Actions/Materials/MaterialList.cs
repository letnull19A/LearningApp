using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using LearningApp.Forms.Student.Actions.Materials;

namespace LearningApp.Forms.Student.Actions
{
    /// <summary>
    /// Класс формы списка материалов
    /// </summary>
    public partial class MaterialList : Form
    {
        // Поле со строкой соединения в БД
        private readonly string _connection;

        /// <summary>
        /// Конструктор класса MaterialList
        /// </summary>
        public MaterialList()
        {
            InitializeComponent();
            _connection = ApplicationContext.GetConnectionString();
        }

        // Обработчик нажатия кнопки в главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var menu = new Student.Menu();
            menu.Show();
        }

        // Обработчик загрузки формы
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

        // Обработчик нажатия на ячейку в DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[3].Index && e.RowIndex >= 0)
            {
                var form = new MaterialView();

                form.FileName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                form.Show();

                Hide();
            }
        }

        // Обработчик закрытия формы
        private void MaterialList_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}