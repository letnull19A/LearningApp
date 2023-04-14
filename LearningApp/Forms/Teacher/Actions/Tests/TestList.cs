using System;
using System.Windows.Forms;

namespace LearningApp.Forms.Teacher.Actions.Tests
{
    public partial class TestList : Form
    {
        public TestList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var menu = new Teacher.Menu();
            menu.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();

            new AddTest().Show();
        }
    }
}
