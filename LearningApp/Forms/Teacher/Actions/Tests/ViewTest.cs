using LearningApp.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    public partial class ViewTest : Form
    {
        private string _id = string.Empty;
        private TestUnit _currentTest;
        private int _currentQuestion = 0;

        public string Id { get => _id; set => _id = value; }

        public ViewTest()
        {
            InitializeComponent();
            _currentTest = new TestUnit();
            _currentTest.Questions = new List<TestQuestionUnit>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new TestList();
            form.Show();

            Hide();
        }

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

        private void button7_Click(object sender, EventArgs e)
        {
            _currentQuestion =
                (_currentQuestion == 0) ? 0 : _currentQuestion - 1;

            DisplayInformation();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _currentQuestion = _currentQuestion < _currentTest.Questions.Count - 1
                ? _currentQuestion + 1 : _currentQuestion;

            DisplayInformation();
        }
    }
}
