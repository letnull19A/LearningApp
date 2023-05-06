using System;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Videos
{
    /// <summary>
    /// Класс отвечающий за вывод видеоматериалов
    /// </summary>
    public partial class VideoList : Form
    {
        /// <summary>
        /// Конструктор класса VideoList
        /// </summary>
        public VideoList()
        {
            InitializeComponent();
        }

        // Обработчик нажатия на кнопку В главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            new Menu().Show();
        }

        // Открытие видеоматериала по кнопке
        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\video1.mp4");
        }

        // Открытие видеоматериала по кнопке
        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\video2.mp4");
        }

        // Открытие видеоматериала по кнопке
        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\video3.mp4");
        }

        // Открытие видеоматериала по кнопке
        private void button6_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\video4.mp4");
        }

        // Открытие видеоматериала по кнопке
        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\video5.mp4");
        }
    }
}
