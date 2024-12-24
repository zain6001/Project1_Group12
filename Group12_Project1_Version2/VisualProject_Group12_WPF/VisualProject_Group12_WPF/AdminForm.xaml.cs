using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VisualProject_Group12_WPF;
using Microsoft.Data.SqlClient;
using BCrypt;

namespace VisualProject_Group12_WPF
{
    public partial class AdminForm : Window
    {
        public AdminForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Load Users
            usersDataGrid.ItemsSource = GetDataTable("SELECT * FROM Users").DefaultView;

            // Load Products
            productsDataGrid.ItemsSource = GetDataTable("SELECT * FROM Products").DefaultView;

            // Load Suppliers
            suppliersDataGrid.ItemsSource = GetDataTable("SELECT * FROM Suppliers").DefaultView;

        }

        private DataTable GetDataTable(string query)
        {
            using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
            string role = ((ComboBoxItem)roleComboBox.SelectedItem)?.Content.ToString();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            if (username == null && password == null && role == null)
            {
                MessageBox.Show("All Fields are required","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
                
            ExecuteQuery($"INSERT INTO Users (Username, PasswordHash, Role) VALUES ('{username}', '{hashedPassword}', '{role}')");
            MessageBox.Show("User added successfully.");
            LoadData();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            string name = productNameTextBox.Text;
            string sku = skuTextBox.Text;
            string category = categoryTextBox.Text;
            int quantity = int.Parse(quantityTextBox.Text);
            decimal unitPrice = decimal.Parse(unitPriceTextBox.Text);
            //&& quantity==null && unitPrice==null
            if (name == null && sku == null && category == null )
            {
                MessageBox.Show("All Fields are required", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ExecuteQuery($"INSERT INTO Products (Name, SKU, Category, Quantity, UnitPrice) VALUES ('{name}', '{sku}', '{category}', {quantity}, {unitPrice})");
            MessageBox.Show("Product added successfully.");
            LoadData();
        }

        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
           
            string supplierName = addSupplierNameTextBox.Text;
            string contactName = addContactNameTextBox.Text;
            string phone = addPhoneTextBox.Text;
            string email = addEmailTextBox.Text;

            if (supplierName == null && contactName == null && phone == null && email==null)
            {
                MessageBox.Show("All Fields are required", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ExecuteQuery($"INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email) VALUES ('{supplierName}', '{contactName}', '{phone}', '{email}')");
            MessageBox.Show("Supplier added successfully.");
            LoadData();
        }
        // Update Supplier
        private void UpdateSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values from the textboxes
            string currentSupplierId = CurrentSupplierIdTextBox.Text;
            string updatedSupplierName = updateSupplierNameTextBox.Text;
            string contactName = updateContactNameTextBox.Text;
            string phone = updatePhoneTextBox.Text;
            string email = updateEmailTextBox.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(currentSupplierId) ||
                string.IsNullOrWhiteSpace(updatedSupplierName) ||
                string.IsNullOrWhiteSpace(contactName) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please fill out all fields before updating.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // SQL query to update supplier data
            string query = @"UPDATE Suppliers 
                     SET SupplierName = @UpdatedSupplierName, 
                         ContactName = @ContactName, 
                         Phone = @Phone, 
                         Email = @Email 
                     WHERE SupplierId = @CurrentSupplierId";

            // Perform database operation
            try
            {
                using (SqlConnection con = new SqlConnection(DataBaseHelper.ConnectionString))
                {
                    con.Open(); // Open connection

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@CurrentSupplierId", currentSupplierId);
                        cmd.Parameters.AddWithValue("@UpdatedSupplierName", updatedSupplierName);
                        cmd.Parameters.AddWithValue("@ContactName", contactName);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Email", email);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Display success or failure message
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Supplier updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData(); // Refresh the DataGrid or list of suppliers
                        }
                        else
                        {
                            MessageBox.Show("No supplier found with the given ID.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        // Delete Supplier
        private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            var supplierId = deleteUserIdTextBox.Text;
            string query = "DELETE FROM Suppliers WHERE SupplierId = @supplierId";
            using (SqlConnection con = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@SupplierId", deleteUserIdTextBox.Text);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Supplier deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No supplier found with the given name.");
                        }
                    }
                    LoadData(); // Refresh DataGrid
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the new form (e.g., a logout confirmation form or a login form)
            MainWindow newForm = new MainWindow();

            // Show the new form
            newForm.Show();

            // Optionally close the current window
            this.Close();
        }



        private void UpdateUserButton_Click( object sender, RoutedEventArgs e)
        {
            // Capture user inputs
            string currentUsername = currentName.Text;
            string newUsername = updateName.Text;
            string newPassword = updatePass.Password;
            newPassword=BCrypt.Net.BCrypt.HashPassword(newPassword);
            string newRole = updateCombo.Text;

            // Input validation
            if (string.IsNullOrWhiteSpace(currentUsername))
            {
                MessageBox.Show("Please enter the current username.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
                {
                    connection.Open();

                    // Build the UPDATE query
                    string query = "UPDATE Users " +
                                   "SET Username = @NewUsername, PasswordHash = @NewPassword, Role = @NewRole " +
                                   "WHERE Username = @CurrentUsername";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@CurrentUsername", currentUsername);
                        command.Parameters.AddWithValue("@NewUsername", string.IsNullOrWhiteSpace(newUsername) ? currentUsername : newUsername);
                        command.Parameters.AddWithValue("@NewPassword", string.IsNullOrWhiteSpace(newPassword) ? (object)DBNull.Value : newPassword);
                        command.Parameters.AddWithValue("@NewRole", string.IsNullOrWhiteSpace(newRole) ? (object)DBNull.Value : newRole);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("User not found. Please check the current username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadData();
        

    }
        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = deleteUserName.Text; // Assuming deleteUserName is a TextBox control in your form.
            if (string.IsNullOrWhiteSpace(userId))
            {
                MessageBox.Show("Please enter a userId to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
                {
                    connection.Open();

                    // Define the query with a parameter
                    string query2 = "Delete from AuditLogs where UserId=@UserId";
                    
                    string query = "DELETE FROM Users WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query2, connection);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Assign the parameter value
                        command.Parameters.AddWithValue("@UserId", userId);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                           MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                // Reload the data to reflect changes
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            int productId;
            string name = updateProductNameTextBox.Text;
            int quantity;
            decimal unitPrice;

            if (!int.TryParse(updateProductIdTextBox.Text, out productId) ||
                string.IsNullOrWhiteSpace(name) || !int.TryParse(updateQuantityTextBox.Text, out quantity) ||
                !decimal.TryParse(updateUnitPriceTextBox.Text, out unitPrice))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Products SET Name = @Name, Quantity = @Quantity, UnitPrice = @UnitPrice WHERE ProductID = @ProductID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Product updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            int productId;
            if (!int.TryParse(deleteProductIdTextBox.Text, out productId))
            {
                MessageBox.Show("Please enter a valid Product ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
    

