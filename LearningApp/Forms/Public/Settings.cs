using System;
using System.Configuration;
using System.Windows.Forms;

namespace LearningApp.Forms.Public
{
    /// <summary>
    /// Класс формы с настройками
    /// </summary>
    public partial class Settings : Form
    {
        /// <summary>
        /// Конструктор класса Settings
        /// </summary>
        public Settings()
        {
            InitializeComponent();
        }

        // Обработчик загрузки формы
        private void Settings_Load(object sender, EventArgs e)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            textBox1.Text = connection;

            var folder = ConfigurationManager.AppSettings["pdfDirectory"];
            textBox2.Text = folder;
        }

        // Обработчик нажатия кнопки сохранить
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateConnectionString();
            UpdateDefaultFolder();

            MessageBox.Show("Настройки обновлены, рекомендуется перезапустить программу");
        }

        // Метод для обновления настроек строки соединения
        private void UpdateConnectionString() 
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.ConnectionStrings.ConnectionStrings.Remove("DefaultConnectionString");
            config.ConnectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("DefaultConnectionString", textBox1.Text));
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("DefaultConnectionString");
        }

        // Метод для обновления настроек выбранной папки
        private void UpdateDefaultFolder()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("pdfDirectory");
            config.AppSettings.Settings.Add("pdfDirectory", folderBrowserDialog1.SelectedPath);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection(config.AppSettings.Settings["pdfDirectory"].Value);
        }

        // Обработчик нажатия кнопки с выбором папки
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) 
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
