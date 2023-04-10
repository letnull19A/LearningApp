using System;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions
{
    public partial class Tests : Form
    {
        public Tests()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            var menu = new Student.Menu();
            menu.Show();
        }
    }
}