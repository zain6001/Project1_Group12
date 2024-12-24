using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCrypt;



using System;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
//using System.Windows;
using BCrypt.Net;

namespace VisualProject_Group12_WPF
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "SELECT PasswordHash, Role FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string storedHash = reader["PasswordHash"].ToString();
                    string role = reader["Role"].ToString();

                    if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        if (role == "Admin")
                        {

                            //CloseAllForms();
                            AdminForm a = new AdminForm();
                            a.Show();
                            this.Close();
                        } else if (role == "Manager") {
                            ManagerForm a = new ManagerForm();
                            a.Show();
                            this.Close();
                        } else if (role=="Staff") {
                            StaffForm a = new StaffForm();
                            a.Show();
                            this.Close();

                        } else
                            MessageBox.Show($"Login successful! Role: {role}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Invalid password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("User not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UsernameTextBox.Text="";
                PasswordBox.Password="";
                RoleComboBox.SelectedItem = null;
            }
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = ((ComboBoxItem)RoleComboBox.SelectedItem)?.Content.ToString();
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
            RoleComboBox.SelectedItem = null;


            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || role == null)
            {
                MessageBox.Show("All fields are required!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.Parameters.AddWithValue("@Role", role);

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Unique constraint error //This condition will true if userName already exists
                    {
                        MessageBox.Show("Username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
             
            }

        }
    }
}
