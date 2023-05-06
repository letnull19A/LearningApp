using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Dictation
{
    // Класс формы обзора диктанта
    public partial class DictationView : Form
    {
        // Поле отвечающее за аудиофайл к диктанту
        private string _dictationTitle;

        // Поле отвечающее за текст диктанта
        private string _dictationAudioFile;

        // Поле отвечающее за заголовок диктанта
        private string _dictationText;

        // Конструктор класса DictationView
        public DictationView()
        {
            InitializeComponent();
        }
        
        // Свойство отвечающее за аудиофайл к диктанту
        public string DictationAudioFile { get => _dictationAudioFile; set => _dictationAudioFile = value; }
        
        // Свойство отвечающее за текст диктанта
        public string DictationText { get => _dictationText; set => _dictationText = value; }
        
        // Свойство отвечающее за заголовок диктанта
        public string DictationTitle { get => _dictationTitle; set => _dictationTitle = value; }

        // Обработчик события нажатия на кнопку
        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start($"{Application.StartupPath}\\{_dictationAudioFile}");
        }

        // Обработчик события нажатия на кнопку
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_dictationText);
        }

        // Обработчик события нажатия на кнопку
        private void button3_Click(object sender, EventArgs e)
        {
            Hide();

            new DictationList().Show();
        }

        // Обработчик события закрытия формы
        private void DictationView_FormClosed(object sender, FormClosedEventArgs e)
        {
            button3_Click(sender, e);
        }

        // Обработчик события
        private void DictationView_Load(object sender, EventArgs e)
        {
            label1.Text = DictationTitle;
        }
    }
}
