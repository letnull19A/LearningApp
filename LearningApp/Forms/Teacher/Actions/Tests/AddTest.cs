using LearningApp.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    /// <summary>
    /// Класс формы добавления тестов
    /// </summary>
    public partial class AddTest : Form
    {
        // Поле со строкой соединения
        private readonly string _connection;
        // Поле с текущим тестом
        private TestUnit _currentTest;
        // Поле с текущим тестом
        private int _currentTestNumber = 0;
        // Поле с флагом изменений
        private bool _isChanged = false;

        /// <summary>
        /// Конструктор класса AddTest
        /// </summary>
        public AddTest()
        {
            InitializeComponent();
            _currentTest = new TestUnit();
            _currentTest.Questions = new List<TestQuestionUnit>
            {
                new TestQuestionUnit()
            };
            _connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }

        // Обработчик нажатия на кнопку В список тестов
        private void button1_Click(object sender, EventArgs e)
        {
            if (_isChanged == true)
            {
                var ask = MessageBox.Show(
                    "У Вас есть несохранённые изменения, при выходе изменения будут утеряны",
                    "Подтветждение действий",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (ask == DialogResult.Cancel)
                    return;
            }

            Hide();

            new TestList().Show();
        }

        // Обработчик нажатия на кнопку Добавить вариант
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1
                .Rows
                .Add("Введите вариант ответа", false);
        }

        // Обреботчик нажатия на кнопку Удалить выбранное
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        // Обработчик события загрузки формы
        private void AddTest_Load(object sender, EventArgs e)
        {
            _currentTest.Theme = "Тема теста";
            DisplayTestInfo();
        }

        // Обреботчик события нажатия на кнопку Далее
        private void button2_Click(object sender, EventArgs e)
        {

            SaveVariants();

            _currentTestNumber++;

            DisplayTestInfo();

            if (_currentTest.Questions.Count < _currentTestNumber + 1)
            {
                _currentTest.Questions.Add(new TestQuestionUnit());
            }

            DisplayQuestion();

            ClearVariants();

            if (_currentTest.Questions[_currentTestNumber].TestVariants != null)
            {
                DisplayVariants();
            }

        }

        // Обработчик нажатия на кнопку Предыдущий
        private void button3_Click(object sender, EventArgs e)
        {
            SaveVariants();

            _currentTestNumber =
                (_currentTestNumber == 0) ? 0 : _currentTestNumber - 1;

            DisplayTestInfo();
            DisplayQuestion();
            ClearVariants();
            DisplayVariants();
        }

        // Метод для отображения информации о тесте
        private void DisplayTestInfo()
        {
            label1.Text = $"Вопрос №{_currentTestNumber + 1}";
            richTextBox2.Text = _currentTest.Theme;
        }

        // Метод для отображения вопросов 
        private void DisplayQuestion()
        {
            richTextBox1.Text = _currentTest.Questions[_currentTestNumber].Question;
        }

        // Обработчик события изменения поля с темой
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            _currentTest.Theme = richTextBox2.Text;
        }

        // Обработчик события изменения поля с вопросом
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            _currentTest.Questions[_currentTestNumber].Question = richTextBox1.Text;
        }

        // Метод для очистки вариантов ответов
        private void ClearVariants()
        {
            dataGridView1.Rows.Clear();
        }

        // Метод для отображения вариантов ответов
        private void DisplayVariants()
        {
            foreach (var variant in _currentTest.Questions[_currentTestNumber].TestVariants)
            {
                dataGridView1.Rows.Add(variant.Answer, variant.IsRight);
            }
        }

        // Метод для сохранения данных в объект
        private void SaveVariants()
        {
            _currentTest
                .Questions[_currentTestNumber]
                .TestVariants = new List<TestVariantUnit>();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                _currentTest
                    .Questions[_currentTestNumber]
                    .TestVariants
                    .Add(new TestVariantUnit()
                    {
                        Answer = dataGridView1.Rows[i].Cells[0].Value.ToString(),
                        IsRight = bool.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString())
                    });
            }
        }

        // Обреботчик нажатия на кнопку Сохранить изменения
        private void button6_Click(object sender, EventArgs e)
        {

            SaveVariants();

            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                var addTestQuery = new SqlCommand(MakeQuery());

                addTestQuery.Connection = connection;
                addTestQuery.ExecuteNonQuery();

                MessageBox.Show("Тест успешно добавлен!");

                connection.Close();
            }
        }

        // Метод создающий запрос в БД
        private string MakeQuery() 
        {
            return 
                CollectTestData() + 
                CollectTestQuestionsToQuery() + 
                CollectTestVariantsToQuery();
        }

        // Метод для запроса на вставки данных тестов
        private string CollectTestData() 
        {
            string result = $"INSERT INTO tests " +
                $"(id, themeName) " +
                $"VALUES " +
                $"('{_currentTest.Id}', '{_currentTest.Theme}');";
            return result;
        }

        // Метод для запроса на вставку вопросов теста
        private string CollectTestQuestionsToQuery()
        {
            string result = string.Empty;

            _currentTest.Questions.ForEach(question =>
            {
                var id = question.Id;
                var testId = _currentTest.Id;

                result += $"INSERT INTO test_questions " +
                $"(id, testId, questionText) " +
                $"VALUES " +
                $"('{id}', '{testId}', '{question.Question}');";
            });

            return result;
        }

        // Метод для запроса вставки вариантов ответов в БД
        private string CollectTestVariantsToQuery()
        {
            string result = string.Empty;

            var questions = _currentTest.Questions;

            questions.ForEach(question =>
            {
                question.TestVariants.ForEach(variant =>
                {
                    result += $"INSERT INTO test_variants " +
                    $"(id, testQuestionId, answer, isRight) " +
                    $"VALUES " +
                    $"('{variant.Id}', '{question.Id}', '{variant.Answer}', '{variant.IsRight}');";
                });
            });

            return result;
        }

        // Обработчик события нажатия на ячейку в DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[1].Value = false;
            }

            dataGridView1.CurrentRow.Cells[1].Value = true;
        }

        // Обработчик события закрытия формы
        private void AddTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
