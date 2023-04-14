using LearningApp.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    public partial class AddTest : Form
    {
        private TestUnit _currentTest;
        private int _currentTestNumber = 0;

        public AddTest()
        {
            InitializeComponent();
            _currentTest = new TestUnit();
            _currentTest.Questions = new List<TestQuestionUnit>();
            _currentTest.Questions.Add(new TestQuestionUnit());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show(
                "У Вас есть несохранённые изменения, при выходе изменения будут утеряны",
                "Подтветждение действий",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (ask == DialogResult.Cancel)
                return;

            Hide();

            new TestList().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1
                .Rows
                .Add("Введите вариант ответа", false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void AddTest_Load(object sender, EventArgs e)
        {
            _currentTest.Theme = "Тема теста";
            DisplayTestInfo();
        }

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

        private void DisplayTestInfo()
        {
            label1.Text = $"Вопрос №{_currentTestNumber + 1}";
            richTextBox2.Text = _currentTest.Theme;
        }

        private void DisplayQuestion()
        {
            richTextBox1.Text = _currentTest.Questions[_currentTestNumber].Question;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            _currentTest.Theme = richTextBox2.Text;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            _currentTest.Questions[_currentTestNumber].Question = richTextBox1.Text;
        }

        private void ClearVariants()
        {
            dataGridView1.Rows.Clear();
        }

        private void DisplayVariants()
        {
            foreach (var variant in _currentTest.Questions[_currentTestNumber].TestVariants)
            {
                dataGridView1.Rows.Add(variant.Answer, variant.IsRight);
            }
        }

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
    }
}
