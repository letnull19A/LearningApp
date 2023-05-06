using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Dictation
{
    // Класс отвечающий за список диктантов
    public partial class DictationList : Form
    {
        // Поле со списком диктантов
        private List<DictationApp> _dictations = new List<DictationApp>();

        // Конструктор класса DictationList
        public DictationList()
        {
            InitializeComponent();

            _dictations.Add(new DictationApp() 
            {
                Theme = "Тотальный диктант 2022",
                TextFile = "d1.txt",
                AudioFile = "Тотальный диктант 2022.wav"
            });

            _dictations.Add(new DictationApp() 
            {
                Theme = "Тотальный диктант 2023",
                TextFile = "d2.txt",
                AudioFile = "Тотальный диктант 2023.wav"
            });

            _dictations.Add(new DictationApp() 
            {
                Theme = "Диктант Осенняя пора",
                TextFile = "d3.txt",
                AudioFile = "Диктант Осенняя пора.wav"
            });

            _dictations.Add(new DictationApp() 
            {
                Theme = "Диктант Рябина",
                TextFile = "d4.txt",
                AudioFile = "Диктант Рябина.wav"
            });
        }

        // Обработчик нажатия на кнопку В главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            new Menu().Show();
        }

        // Обраьртчик события закрытия формы
        private void DictationList_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
        
        // Обработчик нажатия на кнопку открытия диктанта
        private void button2_Click(object sender, EventArgs e)
        {
            SetValuesByIndex(0);
        }
        
        // Обработчик нажатия на кнопку открытия диктанта
        private void button3_Click(object sender, EventArgs e)
        {
            SetValuesByIndex(1);
        }
        
        // Обработчик нажатия на кнопку открытия диктанта
        private void button4_Click(object sender, EventArgs e)
        {
            SetValuesByIndex(2);
        }
        
        // Обработчик нажатия на кнопку открытия диктанта
        private void button5_Click(object sender, EventArgs e)
        {
            SetValuesByIndex(3);
        }

        // Метод отвечающий за установку данных по индексу в массиве
        private void SetValuesByIndex(int index) 
        {
            Hide();

            var text = GetTextFromIndex(index);
            SetValuesForDictation(_dictations[index].Theme, text, _dictations[index].AudioFile)
                .Show();
        }

        // Метод отвечающий за получение текста из файла по его названию
        private string GetTextFromIndex(int index)
        {
            var text = File.ReadAllText($"{Application.StartupPath}\\{_dictations[index].TextFile}");

            return text;
        }

        // Метод отвечающий за инициализацию формы
        private DictationView SetValuesForDictation(string title, string text, string audio) 
        {
            var view = new DictationView();
            view.DictationTitle = title;
            view.DictationText = text;
            view.DictationAudioFile = audio;
            
            return view;
        }
    }
}

// структура для хранения данных и диктанте
struct DictationApp
{
    public string Theme { get; set; }
    public string TextFile { get; set; }
    public string AudioFile { get; set; }
}
