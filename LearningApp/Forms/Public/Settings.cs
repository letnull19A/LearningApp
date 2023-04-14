using System;
using System.Configuration;
using System.Windows.Forms;

namespace LearningApp.Forms.Public
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            textBox1.Text = connection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Remove("DefaultConnectionString");
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("DefaultConnectionString", textBox1.Text));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("DefaultConnectionString");

            MessageBox.Show("Настройки обновлены");
        }
    }
}
