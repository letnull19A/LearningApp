using System;
using System.Configuration;
using System.Windows.Forms;
using LearningApp.Forms.Public;
using LearningApp.Forms.Student.Actions;
using LearningApp.Forms.Student.Actions.Dictation;
using LearningApp.Forms.Student.Actions.Reports;
using LearningApp.Forms.Student.Actions.Videos;

namespace LearningApp.Forms.Student
{
    /// <summary>
    /// Класс отвечающий за отображение меню
    /// </summary>
    public partial class Menu : Form
    {
        /// <summary>
        /// Конструктор класса Menu
        /// </summary>
        public Menu()
        {
            InitializeComponent();
        }

        // Обработчик нажатия кнопки Выхода из аккаунта
        private void button5_Click(object sender, EventArgs e)
        {
            Hide();

            ApplicationContext.EndSession();

            var login = new Login();
            login.Show();
        }

        // Обработчик нажатия кнопки Теоритический материал
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var materials = new MaterialList();
            materials.Show();
        }

        // Обработчик нажатия кнопки Тестирований
        private void button3_Click(object sender, EventArgs e)
        {
            Hide();

            var tests = new Actions.TestList();
            tests.Show();
        }

        // Обработчик нажатия кнопки Справки
        private void button4_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(
                this,
                AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings.Get("helpFileName"),
                HelpNavigator.Topic);
        }

        // Обработчик нажатия кнопки
        private void Menu_Load(object sender, EventArgs e)
        {
            var session = ApplicationContext.GetSession().Value;

            sessionLabel.Text = session.Name + " " + session.Surname + "     Роль: " + session.RoleName;
        }

        // Обработчик нажатия кнопки Результаты тестов
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();

            var results = new MarkReport();
            results.Show();
        }

        // Обработчик выхода из формы
        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            button5_Click(sender, e);
        }

        // Обработчик нажатия кнопки Видеоматериалы
        private void button6_Click(object sender, EventArgs e)
        {
            Hide();

            new VideoList().Show();
        }

        // Обработчик нажатия на кнопку Диктанты
        private void button7_Click(object sender, EventArgs e)
        {
            Hide();

            new DictationList().Show();
        }
    }
}