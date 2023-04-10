using System;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions
{
    public partial class Materials : Form
    {
        public Materials()
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