using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Materials
{
    /// <summary>
    /// Класс формы MaterialAdd
    /// </summary>
    public partial class MaterialAdd : Form
    {
        // Поле со строкой соединения к БД
        private readonly string _connection;
        // Поле с директорией к хранилицу pdf файлов
        private readonly string _pdfPath;

        /// <summary>
        /// Конструктор класса MaterialAdd
        /// </summary>
        public MaterialAdd()
        {
            InitializeComponent();
            _connection = ApplicationContext.GetConnectionString();
            _pdfPath = ConfigurationManager.AppSettings["pdfDirectory"].ToString();
        }

        // Обработчик собятия при нажатии на кнопку Добавить
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Файл PDF|*.pdf";

            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                label4.Text = openFileDialog1.FileName;
            }
        }

        // Обработка нажатии на кнопку Отмена
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        // Обработка нажатия на кнопку Добавить
        private void button1_Click(object sender, EventArgs e)
        {
            string newFileName = $"{Guid.NewGuid()}_{openFileDialog1.SafeFileName}";

            using (var connection = new SqlConnection(_connection)) 
            {
                connection.Open();

                string sqlQuery = 
                    "INSERT INTO materials " +
                    "(id, fileName, title) " +
                    "VALUES " +
                    $"('{Guid.NewGuid()}', '{newFileName}', '{textBox1.Text}');";

                var query = new SqlCommand(sqlQuery, connection);
                query.ExecuteNonQuery();

                connection.Close();
            }

            File.Copy(openFileDialog1.FileName, $"{_pdfPath}\\{newFileName}");

            Hide();
        }
    }
}
