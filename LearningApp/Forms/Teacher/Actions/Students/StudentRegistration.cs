using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LearningApp.Forms.Teacher.Actions.Students
{
    public partial class StudentRegistration : Form
    {
        private readonly string _connection;
        public StudentRegistration()
        {
            InitializeComponent();
            _connection = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }

        private void StudentRegistration_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                
                string query = "SELECT groups.number FROM groups";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                userGroup.DisplayMember = "number";
                userGroup.ValueMember = "id";
                userGroup.DataSource = dataSet.Tables[0];
                
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var list = new StudentList();
            list.FormClosed += (a, c) => Application.Exit();
            list.Show();

            Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int roleId = 2;
            string name = userName.Text;
            string surname = userSurname.Text;
            string fatherName = userFatherName.Text;
            string login = userLogin.Text;
            string password = userPassword.Text;
            string confirm = confirmPassword.Text;
            string groupName = userGroup.Text;

            if (!password.Equals(confirm)) 
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            using(SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                string sqlQueryAddingStudent = "INSERT INTO users " +
                                               "(roleId, name, surname, fatherName, login, password) " +
                                               "VALUES " +
                                               "(@roleId, @name, @surname, @fatherName, @login, @password);" +
                                               "INSERT INTO studentAndGroup (userId, groupId) " +
                                               "VALUES " +
                                               "((SELECT users.id FROM users WHERE users.login = @login), (SELECT groups.id FROM groups WHERE groups.number = @groupNumber))";
                
                SqlCommand query = new SqlCommand(sqlQueryAddingStudent, connection);
                
                SqlParameter roleIdParameter = new SqlParameter("@roleId", roleId);
                SqlParameter nameParameter = new SqlParameter("@name", name);
                SqlParameter surnameParameter = new SqlParameter("@surname", surname);
                SqlParameter fatherNameParameter = new SqlParameter("@fatherName", fatherName);
                SqlParameter loginParameter = new SqlParameter("@login", login);
                SqlParameter passwordParameter = new SqlParameter("@password", password);
                SqlParameter groupIdParameter = new SqlParameter("@groupNumber", groupName);

                query.Parameters.Add(roleIdParameter);
                query.Parameters.Add(nameParameter);
                query.Parameters.Add(surnameParameter);
                query.Parameters.Add(fatherNameParameter);
                query.Parameters.Add(loginParameter);
                query.Parameters.Add(passwordParameter);
                query.Parameters.Add(groupIdParameter);

                query.ExecuteNonQuery();

                MessageBox.Show("Студент добавлен в базу данных!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();

                connection.Close();
            }
        }

        private void ClearForm() 
        {
            userName.Text = string.Empty;
            userSurname.Text = string.Empty;
            userFatherName.Text = string.Empty;
            userLogin.Text = string.Empty;
            userPassword.Text = string.Empty;
            confirmPassword.Text = string.Empty;
            userGroup.Text = string.Empty;
        }
    }
}