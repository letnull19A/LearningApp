using LearningApp.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    /// <summary>
    /// Класс отвечающия за форму ViewTest
    /// </summary>
    public partial class ViewTest : Form
    {
        // Поле с идентификатором теста
        private string _id = string.Empty;
        // Поле со строкой соединения
        private readonly string _connection;
        // Поле с текущим тестом
        private TestUnit _currentTest;
        // Поле с текущим вопросом
        private int _currentQuestion = 0;

        /// <summary>
        /// Свойство для поля _id
        /// </summary>
        public string Id { get => _id; set => _id = value; }

        /// <summary>
        /// Конструктор класса ViewTest
        /// </summary>
        public ViewTest()
        {
            InitializeComponent();
            _currentTest = new TestUnit();
            _currentTest.Questions = new List<TestQuestionUnit>();
            _connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }
        
        #region Events

        // Обработчик нажатия на кнопку Назад
        private void button1_Click(object sender, EventArgs e)
        {
            var form = new TestList();
            form.Show();

            Hide();
        }

        // Обработчик нажатия на кнопку Предыдущий вопрос
        private void button7_Click(object sender, EventArgs e)
        {
            SaveVariants();

            _currentQuestion =
                (_currentQuestion == 0) ? 0 : _currentQuestion - 1;

            DisplayInformation();
        }

        // Обработчик события нажатия на ячейку DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[1].Value = false;
            }

            dataGridView1.CurrentRow.Cells[1].Value = true;

            SaveVariants();
        }

        // Обработчик нажатия на кнопку Следующий вопрос
        private void button8_Click(object sender, EventArgs e)
        {
            SaveVariants();

            _currentQuestion = _currentQuestion < _currentTest.Questions.Count - 1
                ? _currentQuestion + 1 : _currentQuestion;

            DisplayInformation();
        }

        // Обработчик события на кнопку Обновить тест
        private void button3_Click(object sender, EventArgs e)
        {
            SaveVariants();

            Clipboard.SetData(DataFormats.Text, (Object)CollectTestVariantsToQuery());

            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();

                var addTestQuery = new SqlCommand(MakeQuery());

                addTestQuery.Connection = connection;
                addTestQuery.ExecuteNonQuery();

                MessageBox.Show("Тест успешно обновлён!");

                connection.Close();
            }
        }

        // Обработчик события изменения текста
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            _currentTest.Questions[_currentQuestion].Question = richTextBox1.Text;
        }

        // Обработчик события изменения текста
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            _currentTest.Theme = richTextBox2.Text;
        }

        // Обработка события загрузки формы
        private void ViewTest_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(ApplicationContext.GetConnectionString()))
            {
                connection.Open();
                GetTestData(connection);
                connection.Close();

                connection.Open();
                GetQuestions(connection);
                connection.Close();

                connection.Open();
                SetVariants(GetVariants(connection));
                connection.Close();

            }

            DisplayInformation();
        }
        #endregion

        #region Function

        // Метод для получения данных теста у загрузка в текущий тест
        private void GetTestData(SqlConnection connection)
        {
            string sqlQuery = $"SELECT * FROM tests WHERE id = @id;";

            var query = new SqlCommand(sqlQuery, connection);
            query.Parameters.Add(new SqlParameter("@id", Id));

            var response = query.ExecuteReader();

            while (response.Read())
            {
                _currentTest.Id = Guid.Parse(response["id"].ToString());
                _currentTest.Theme = response["themeName"].ToString();
            }
        }

        // Метод для получения вопросов теста и загрузка в текущий тест
        private void GetQuestions(SqlConnection connection) 
        {
            string sqlQuery = $"SELECT * FROM test_questions WHERE testId = @testId";

            var query = new SqlCommand(sqlQuery, connection);
            query.Parameters.Add(new SqlParameter("@testId", _currentTest.Id));
            
            var response = query.ExecuteReader();

            while (response.Read()) 
            {
                _currentTest.Questions.Add(new TestQuestionUnit()
                {
                    Id = Guid.Parse(response["id"].ToString()),
                    Question = response["questionText"].ToString(),
                });
            }

        }

        // Метод для получения вариантов теста
        private List<TestVariantUnit> GetVariants(SqlConnection connection) 
        {
            string sqlQuery = "SELECT * FROM test_variants";

            var result = new List<TestVariantUnit>();

            var query = new SqlCommand(sqlQuery, connection);

            var response = query.ExecuteReader();

            while(response.Read()) 
            {
                result.Add(new TestVariantUnit() 
                {
                    Id = Guid.Parse(response["id"].ToString()),
                    QuestionId = Guid.Parse(response["testQuestionId"].ToString()),
                    Answer = response["answer"].ToString(),
                    IsRight = bool.Parse(response["isRight"].ToString()),
                });
            }

            return result;
        }

        // Метод для установки значений для вариантов
        private void SetVariants(List<TestVariantUnit> variants) 
        {
            foreach (var question in _currentTest.Questions) 
            {
                foreach (var variant in variants) 
                {
                    if (variant.QuestionId == question.Id) 
                    {
                        if (question.TestVariants == null)
                            question.TestVariants = new List<TestVariantUnit>();

                        question
                            .TestVariants
                            .Add(variant);
                    }
                }
            }
        }

        // Метод для отображеня данных о тесте
        private void DisplayInformation()
        {
            label1.Text = $"Вопрос №{_currentQuestion + 1}";

            richTextBox2.Text = _currentTest.Theme;

            richTextBox1.Text = _currentTest
                .Questions[_currentQuestion].Question;

            dataGridView1.Rows.Clear();

            _currentTest
                .Questions[_currentQuestion]
                .TestVariants
                .ForEach(variant => 
                {
                    dataGridView1.Rows.Add(variant.Answer, variant.IsRight);
                });
        }

        // Метод для сохранения вариантов теста
        private void SaveVariants()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++) 
            {
                var question = _currentTest
                    .Questions[_currentQuestion];

                question.TestVariants[i].Answer = dataGridView1.Rows[i].Cells[0].Value.ToString();
                question.TestVariants[i].IsRight = bool.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }
        }

        // Метод для генерации запроса в БД
        private string MakeQuery() 
        {
            return
                CollectTestData() +
                CollectTestQuestionsToQuery() +
                CollectTestVariantsToQuery();
        }

        // Метод собирающий данные о тесте и генерирующий запрос в БД
        private string CollectTestData()
        {
            string result = $"UPDATE tests SET " +
                $"themeName='{_currentTest.Theme}' " +
                $"WHERE id='{_currentTest.Id}'; ";
            return result;
        }

        // Метод сборки запроса в БД для обновлени вопросов
        private string CollectTestQuestionsToQuery()
        {
            string result = string.Empty;

            _currentTest.Questions.ForEach(question =>
            {
                var id = question.Id;
                var testId = _currentTest.Id;

                result += $"UPDATE test_questions SET " +
                $"testId='{testId}', " +
                $"questionText='{question.Question}' " +
                $"WHERE id='{id}'; ";
            });

            return result;
        }

        // Метод собирающий запрос для обновления вариантов ответов
        private string CollectTestVariantsToQuery()
        {
            string result = string.Empty;

            var questions = _currentTest.Questions;

            questions.ForEach(question =>
            {
                question.TestVariants.ForEach(variant =>
                {
                    result += $"UPDATE test_variants SET " +
                    $"test_variants.answer='{variant.Answer}', " +
                    $"test_variants.isRight='{variant.IsRight}' " +
                    $"WHERE " +
                    $"test_variants.id='{variant.Id}'; ";
                });
            });

            return result;
        }
        #endregion

        // Обработчик события закрытия формы
        private void ViewTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
