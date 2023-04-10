using System;
using System.Windows.Forms;
using LearningApp.Forms.Public;

namespace LearningApp.Forms.Teacher
{
    public partial class StudentMenu : Form
    {
        public StudentMenu()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hide();

            var guide = new Guide();
            guide.Show();
        }
    }
}