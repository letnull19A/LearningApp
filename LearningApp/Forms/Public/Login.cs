using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LearningApp.Forms.Teacher.Actions;
using Microsoft.Data.SqlClient;

namespace LearningApp.Forms.Public
{
    public partial class Login : Form
    {
        #if Release
        private readonly string _connection 
            = @"Server=(localdb)\mssqllocaldb;Database=RuLearningApp;Trusted_Connection=true";
        #endif
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            #if Release
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
            #endif
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var login = loginField.Text ?? string.Empty;
            var password = passwordField.Text ?? string.Empty;

            #if Release
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

                    Hide();

                    loginField.Text = string.Empty;
                    passwordField.Text = string.Empty;
            #endif        
                    var menu = new Teacher.Menu();
                    menu.Show();
                    
                    Hide();
            #if Release
                }

                reader.Close();
                connection.Close();
            }
            #endif
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var menu = new Student.Menu();
            menu.Show();
            
            Hide();
        }
    }
}