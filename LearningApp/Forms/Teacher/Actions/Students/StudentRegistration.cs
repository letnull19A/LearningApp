using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LearningApp.Forms.Teacher.Actions.Students
{
    public partial class StudentRegistration : Form
    {
        #if release
        private readonly string _connection
            = @"Server=(localdb)\mssqllocaldb;Database=RuLearningApp;Trusted_Connection=true";
        #endif
        public StudentRegistration()
        {
            InitializeComponent();
        }

        private async void StudentRegistration_Load(object sender, EventArgs e)
        {
            #if release
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
            #endif
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int roleId = 1;
            string name = userName.Text;
            string surname = userSurname.Text;
            string fatherName = userFatherName.Text;
            string login = userLogin.Text;
            string password = userPassword.Text;
            string confirm = confirmPassword.Text;
            string groupName = userGroup.Text;

            #if release
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

                connection.Close();
            }
            #endif
        }
    }
}