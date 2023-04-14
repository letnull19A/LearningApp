using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LearningApp.Forms.Public
{
    public partial class Login : Form
    {
        private readonly string _connection;
        public Login()
        {
            InitializeComponent();
            _connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }

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
                    menu.FormClosed += (a, c) => Application.Exit();
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

        private void ClearForm()
        {
            loginField.Text = string.Empty;
            passwordField.Text = string.Empty;
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Guide().ShowDialog();
        }
    }
}