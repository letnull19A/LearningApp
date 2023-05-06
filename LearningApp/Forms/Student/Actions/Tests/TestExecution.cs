using LearningApp.Entities;
using LearningApp.Entities.Stats;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Tests
{
    /// <summary>
    /// Класс отвечающий за прохождение теста
    /// </summary>
    public partial class TestExecution : Form
    {
        /// <summary>
        /// Константа оценки Отлично
        /// </summary>
        public const byte HightMark = 5;
        /// <summary>
        /// Константа оценки Хорошо
        /// </summary>
        public const byte GoodMark = 4;
        /// <summary>
        /// Константа оценки Удовлетворительно
        /// </summary>
        public const byte MiddleMark = 3;
        /// <summary>
        /// Константа оценки Неудовлетворительно
        /// </summary>
        public const byte FailMark = 2;

        // Поле с ID теста
        private string _id = string.Empty;
        // Поле с проходимым тестом
        private TestUnit _currentTest;
        // Поле с ответами тестов
        private TestUnit _originalTest;
        // Поле с текущим вопросом
        private int _currentQuestion = 0;
        // Поле с результатом правильных ответов
        private int result = 0;

        // Свойство с ID теста
        public string Id { get => _id; set => _id = value; }

        /// <summary>
        /// Конструктор класса TestExecution
        /// </summary>
        public TestExecution()
        {
            InitializeComponent();
            _currentTest = new TestUnit();
        }

        // Обработка события загрузки формы
        private void TestExecution_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(ApplicationContext.GetConnectionString()))
            {
                connection.Open();
                _currentTest = GetTestData(connection);
                connection.Close();

                connection.Open();
                _originalTest = GetTestData(connection);
                connection.Close();

                connection.Open();
                _currentTest = GetQuestions(connection, _currentTest);
                connection.Close();

                connection.Open();
                _originalTest = GetQuestions(connection, _originalTest);
                connection.Close();

                connection.Open();
                SetVariants(GetVariants(connection), _currentTest);
                connection.Close();

                connection.Open();
                SetVariants(GetVariants(connection), _originalTest);
                connection.Close();

                _currentTest = ResetVariants(_currentTest);
            }

            DisplayInformation();
        }

        // Метод для получения данных формы
        private TestUnit GetTestData(SqlConnection connection)
        {
            var result = new TestUnit();

            string sqlQuery = $"SELECT * FROM tests WHERE id = @id;";

            var query = new SqlCommand(sqlQuery, connection);
            query.Parameters.Add(new SqlParameter("@id", Id));

            var response = query.ExecuteReader();

            while (response.Read())
            {
                result.Id = Guid.Parse(response["id"].ToString());
                result.Theme = response["themeName"].ToString();
            }

            return result;
        }

        // Метод для получения вопросов
        private TestUnit GetQuestions(SqlConnection connection, TestUnit testUnit)
        {
            string sqlQuery = $"SELECT * FROM test_questions WHERE testId = @testId";

            var query = new SqlCommand(sqlQuery, connection);
            query.Parameters.Add(new SqlParameter("@testId", testUnit.Id));

            var response = query.ExecuteReader();

            testUnit.Questions = new List<TestQuestionUnit>();

            while (response.Read())
            {
                testUnit.Questions.Add(new TestQuestionUnit()
                {
                    Id = Guid.Parse(response["id"].ToString()),
                    Question = response["questionText"].ToString(),
                });
            }

            return testUnit;
        }

        // Метод для получения вариантов ответов
        private List<TestVariantUnit> GetVariants(SqlConnection connection)
        {
            string sqlQuery = "SELECT * FROM test_variants";

            var result = new List<TestVariantUnit>();

            var query = new SqlCommand(sqlQuery, connection);

            var response = query.ExecuteReader();

            while (response.Read())
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

        // Метод для установки вариантов ответов
        private void SetVariants(List<TestVariantUnit> variants, TestUnit testUnit)
        {
            foreach (var question in testUnit.Questions)
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

        // Обработчик собятия нажатия кнопки предыдущего теста
        private void buttonPrev_Click(object sender, EventArgs e)
        {
            SaveVariants();

            _currentQuestion =
                (_currentQuestion == 0) ? 0 : _currentQuestion - 1;

            DisplayInformation();
            ClearVariants();
            DisplayVariants();
        }

        // Обработчик события нажатия кнопки следующего теста
        private void buttonNext_Click(object sender, EventArgs e)
        {
            SaveVariants();

            _currentQuestion = _currentQuestion < _currentTest.Questions.Count - 1
                ? _currentQuestion + 1 : _currentQuestion;

            DisplayInformation();
            ClearVariants();
            DisplayVariants();
        }

        // Обработчик нажатия кнопки завершения теста
        private void button3_Click(object sender, EventArgs e)
        {
            CheckVariants();

            var stats = new TestStats();

            stats.MaxScores = _currentTest.Questions
                .Select(question => question.TestVariants)
                .Select(q => q.Any(i => i.IsRight == true))
                .Count();
            stats.ActualScores = result;
            stats.Percent = (float)result / (float)stats.MaxScores * 100f;
            stats.Mark = CalculateMark(stats.Percent);

            SaveResults(
                stats.Mark, 
                ApplicationContext.GetSession().Value.Id, 
                Guid.Parse(_id));

            var resultWindow = new TestResultPopup();
            resultWindow.Stats = stats;
            resultWindow.ShowDialog();

            result = 0;

            Hide();

            new TestList().Show();
        }

        // Метод сохраняющий результаты в БД
        private void SaveResults(int mark, int userId, Guid testId) 
        {
            using (var connection = new SqlConnection(ApplicationContext.GetConnectionString())) 
            {
                connection.Open();

                string sqlQuery = 
                    $"INSERT INTO results " +
                    $"(id, userId, testId, mark) " +
                    $"VALUES " +
                    $"('{Guid.NewGuid()}', '{userId}', '{testId}', '{mark}');";

                var query = new SqlCommand(sqlQuery, connection);

                var response = query.ExecuteNonQuery();

                connection.Close();
            }
        }

        // Метод подсчитывающий оценку по набранным балам
        private int CalculateMark(float percent)
        {
            if (percent == 100 && percent >= 95)
            {
                return HightMark;
            }

            if (percent < 95 && percent >= 75)
            {
                return GoodMark;
            }

            if (percent < 75 && percent >= 50)
            {
                return MiddleMark;
            }

            if (percent < 50)
            {
                return FailMark;
            }

            return 0;
        }

        // Метод для проверки вариантов
        private void CheckVariants()
        {
            for (int i = 0; i < _currentTest.Questions.Count; i++)
            {
                var currentOriginalQuestion = _originalTest.Questions[i];
                var currentQuestion = _currentTest.Questions[i];

                for (int j = 0; j < currentQuestion.TestVariants.Count; j++)
                {
                    if (currentQuestion.TestVariants[j].IsRight != currentOriginalQuestion.TestVariants[j].IsRight)
                    {
                        break;
                    }

                    if (j == currentOriginalQuestion.TestVariants.Count - 1)
                    {
                        result++;
                    }
                }
            }
        }

        // Метод для отображения информации о тесте
        private void DisplayInformation()
        {
            label1.Text = $"Вопрос №{_currentQuestion + 1}";

            label4.Text = _currentTest.Theme;

            label5.Text = _currentTest
                .Questions[_currentQuestion].Question;

            dataGridView1.Rows.Clear();

            dataGridView1.Columns[0].ReadOnly = true;

            _currentTest
                .Questions[_currentQuestion]
                .TestVariants
                .ForEach(variant =>
                {
                    dataGridView1.Rows.Add(variant.Answer, variant.IsRight);
                });
        }

        // Метод для очистки вариантов ответов
        private void ClearVariants()
        {
            dataGridView1.Rows.Clear();
        }

        // Метод для сохранения выбранных вариантов ответов у клиента
        private void SaveVariants()
        {
            _currentTest
                .Questions[_currentQuestion]
                .TestVariants = new List<TestVariantUnit>();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView1.Rows[i].Cells[1] as DataGridViewCheckBoxCell;

                _currentTest
                    .Questions[_currentQuestion]
                    .TestVariants
                    .Add(new TestVariantUnit()
                    {
                        Answer = dataGridView1.Rows[i].Cells[0].Value.ToString(),
                        IsRight = (bool)cell.Value == true
                    });
            }
        }

        // Метод очищающий варианты ответов в DataGridView
        private TestUnit ResetVariants(TestUnit test)
        {
            for (int i = 0; i < test.Questions.Count; i++)
            {
                for (int j = 0; j < test.Questions[i].TestVariants.Count; j++)
                {
                    test.Questions[i].TestVariants[j].IsRight = false;
                }
            }

            return test;
        }

        // Метод для отображения вариантов
        private void DisplayVariants()
        {
            foreach (var variant in _currentTest.Questions[_currentQuestion].TestVariants)
            {
                dataGridView1.Rows.Add(variant.Answer, variant.IsRight);
            }
        }

        // Обработчик события изменения состояния ячеек DataDridView
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[1].Index && e.RowIndex >= 0)
            {
                SaveVariants();
            }
        }
         
        // Обработсик события нажатия на ячейку DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[1].Value = false;
            }

            dataGridView1.CurrentRow.Cells[1].Value = true;
        }

        // Обработчик собятия закрытия формы
        private void TestExecution_FormClosed(object sender, FormClosedEventArgs e)
        {
            button3_Click(sender, e);
        }
    }
}