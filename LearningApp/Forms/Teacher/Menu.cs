using System;
using System.Data;
using System.Windows.Forms;
using LearningApp.Forms.Public;
using LearningApp.Forms.Teacher.Actions;
using Microsoft.Data.SqlClient;

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
            Hide();

            var login = new Login();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            
            var form = new StudentList();
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Guide().Show();
        }
    }
}