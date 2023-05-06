using System;
using System.Configuration;
using System.Windows.Forms;
using LearningApp.Forms.Public;
using LearningApp.Forms.Teacher.Actions;
using LearningApp.Forms.Teacher.Actions.Materials;
using LearningApp.Forms.Teacher.Actions.Reports;
using LearningApp.Forms.Teacher.Actions.Tests;

namespace LearningApp.Forms.Teacher
{
    /// <summary>
    /// Класс формы меню
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

        // Обработчик события при загрузке формы
        private void Menu_Load(object sender, EventArgs e)
        {
            var session = ApplicationContext.GetSession().Value;

            sessionLabel.Text = session.Name + " " + session.Surname +  "     Роль: " + session.RoleName;
        }

        // Обработчик события при нажатии на кнопку Выйти
        private void button5_Click(object sender, EventArgs e)
        {
            ApplicationContext.EndSession();

            var login = new Login();
            login.Show();

            Dispose();
        }

        // Обработчик события при нажатии на кнопку Студенты
        private void button1_Click(object sender, EventArgs e)
        {            
            var form = new StudentList();
            form.Show();

            Dispose();
        }

        // Обработчик события при нажатии на кнопку Справка
        private void button4_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(
                this,
                AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings.Get("helpFileName"),
                HelpNavigator.Topic);
        }

        // Обработчик события при нажатии на кнопку Тесты
        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();

            new TestList().Show();
        }

        // Обработчик события при нажатии на кнопку Результаты тестов
        private void button3_Click(object sender, EventArgs e)
        {
            Dispose();

            new MarkReport().Show();
        }

        // Обработчик события при нажатии на кнопку Материалы к тестам
        private void button6_Click(object sender, EventArgs e)
        {
            Dispose();

            new MaterialList().Show();
        }

        // Обработчик события закрытия формы
        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            button5_Click(sender, e);
        }
    }
}