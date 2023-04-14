using System;
using System.Windows.Forms;
using LearningApp.Forms.Public;
using LearningApp.Forms.Student.Actions;

namespace LearningApp.Forms.Student
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Hide();

            var login = new Login();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var materials = new Materials();
            materials.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();

            var tests = new Actions.Tests();
            tests.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Guide().Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            var session = ApplicationContext.GetSession().Value;

            sessionLabel.Text = session.Name + " " + session.Surname + " Роль: " + session.RoleName;
        }
    }
}