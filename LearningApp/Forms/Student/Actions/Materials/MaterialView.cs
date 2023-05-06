using System;
using System.Configuration;
using System.Windows.Forms;

namespace LearningApp.Forms.Student.Actions.Materials
{
    /// <summary>
    /// Кдасс формы просмотра материалов
    /// </summary>
    public partial class MaterialView : Form
    {
        // Поле хранящий имя файла
        private string _fileName;
        // Поле хранящее путь к файлу PDF
        private readonly string _pdfPath;

        /// <summary>
        /// Свойство для поля _fileName
        /// </summary>
        public string FileName { get => _fileName; set => _fileName = value; }

        /// <summary>
        /// Конструктор класса MaterialView
        /// </summary>
        public MaterialView()
        {
            InitializeComponent();
            _pdfPath = ConfigurationManager.AppSettings["pdfDirectory"].ToString();
        }

        // Обработчик загрузки формы
        private void MaterialView_Load(object sender, EventArgs e)
        {
            var document = PdfiumViewer.PdfDocument.Load($"{_pdfPath}\\{_fileName}");
            pdfViewer1.Document = document;
        }

        // Обработчик нажатия кнопки в главное меню
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();

            new MaterialList().Show();
        }

        // Обработчик закрытия формы
        private void MaterialView_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
