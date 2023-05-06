using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LearningApp.Forms.Public
{
    /// <summary>
    /// Класс Login отвечает за обработку событий формы Login
    /// </summary>
    public partial class Login : Form
    {
        private readonly string _connection;
        
        /// <summary>
        /// Конвтруктор класса Login
        /// </summary>
        public Login()
        {
            InitializeComponent();
            _connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }

        // Подписка на событие загрузки формы
        private void Login_Load(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connection);

            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка соединения с базой данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        // Обработчик нажатия на кнопку авторизации
        private async void button1_Click(object sender, EventArgs e)
        {
            var login = loginField.Text ?? string.Empty;
            var password = passwordField.Text ?? string.Empty;

            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                string query = "SELECT *, users.id AS user_id, users.name AS user_name, roles.id AS role_id, roles.name AS role_name FROM users JOIN roles ON roles.id = users.roleId WHERE login=@login AND password=@password";

                SqlCommand command = new SqlCommand(query, connection);

                SqlParameter loginParameter = new SqlParameter("@login", login);
                SqlParameter passwordParameter = new SqlParameter("@password", password);

                command.Parameters.Add(loginParameter);
                command.Parameters.Add(passwordParameter);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    ApplicationContext.StartSession(new Session
                    {
                        Id = Convert.ToInt32(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        Surname = reader["surname"].ToString(),
                        RoleId = Convert.ToInt32(reader["roleId"].ToString()),
                        RoleName = reader["role_name"].ToString()
                    });

                    ClearForm();

                    var role = ApplicationContext.GetSession().Value.RoleId;
                    Form menu = this;

                    switch (role)
                    {
                        case 1:
                            menu = new Teacher.Menu();
                            break;
                        case 2:
                            menu = new Student.Menu();
                            break;
                        default:
                            throw new Exception("Произошла неизвестная ошибка!");
                    }

                    menu?.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                }

                reader.Close();
                connection.Close();
            }
        }

        // Метод для очистки формы
        private void ClearForm()
        {
            loginField.Text = string.Empty;
            passwordField.Text = string.Empty;
        }

        // Метод для обработки нажатия по ToolStrip
        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        // Метод для обработки нажатия по ToolStrip
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(
                this,
                AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings.Get("helpFileName"),
                HelpNavigator.Topic);
        }

        // Обработчик события закрытия формы
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            new StartForm().Show();
        }

        // Обработчик события перехода в начальную форму
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            new StartForm().Show();
        }
    }
}