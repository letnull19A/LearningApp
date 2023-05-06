using System;
using System.Windows.Forms;
using LearningApp.Forms.Public;

namespace LearningApp
{
    /// <summary>
    /// Класс программы
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Точка входа приложения
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartForm());
        }
    }
}