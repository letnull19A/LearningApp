using System;
using System.Windows.Forms;
using LearningApp.Forms.Public;
using LearningApp.Forms.Teacher.Actions;
using LearningApp.Forms.Teacher.Actions.Tests;

namespace LearningApp.Forms.Teacher
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            var session = ApplicationContext.GetSession().Value;

            sessionLabel.Text = session.Name + " " + session.Surname +  " Роль: " + session.RoleName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ApplicationContext.EndSession();

            var login = new Login();
            login.FormClosed += (a, c) => Application.Exit();
            login.Show();

            Hide();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            var form = new StudentList();
            form.FormClosed += (a, c) => Application.Exit();
            form.Show();

            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Guide().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();

            new TestList().Show();
        }
    }
}