using LearningApp.Entities.Stats;
using System;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Tests
{
    /// <summary>
    /// Класс отвечающий за отображение формы с результатами теста
    /// </summary>
    public partial class TestResultPopup : Form
    {
        // Поле отвечающий за данные о тестировании
        private TestStats _stats;

        /// <summary>
        /// 
        /// </summary>
        public TestResultPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Свойство для поля со статистикой теста
        /// </summary>
        public TestStats Stats 
        { 
            get => _stats; 
            set => _stats = value; 
        }

        // Обработчик загрузки формы результатов тестирования
        private void TestResultPopup_Load(object sender, EventArgs e)
        {
            resultLabel.Text = _stats.Mark.ToString();
            allScores.Text = _stats.MaxScores.ToString();
            actualScores.Text = _stats.ActualScores.ToString();
            percent.Text = $"{_stats.Percent}%";
        }

        // Обработчик нажатия кнопки закрыть
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
