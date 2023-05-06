using System;
using System.Windows.Forms;

namespace LearningApp.Forms.Public
{
    /// <summary>
    /// Класс стартовой формы
    /// </summary>
    public partial class StartForm : Form
    {
        /// <summary>
        /// Конструтор класса StartForm
        /// </summary>
        public StartForm()
        {
            InitializeComponent();
        }

        // Обработчик нажатия на кнопку начать работу
        private void button1_Click(object sender, EventArgs e)
        {
            var currentForm = new Login();
            
            Hide();
            currentForm.Show();
        }

        // Обработчик нажатия на кнопку завершить работу
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Обработчик закрытия формы
        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            Application.ExitThread();
        }
    }
}
