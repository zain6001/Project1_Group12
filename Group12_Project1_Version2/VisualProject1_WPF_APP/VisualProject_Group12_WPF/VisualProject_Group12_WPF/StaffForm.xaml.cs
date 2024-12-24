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
using Microsoft.Data.SqlClient;
using VisualProject_Group12_WPF;

namespace VisualProject_Group12_WPF
{
    public partial class StaffForm : Window
    {

        public StaffForm()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            // Load Users
            ProductsDataGrid.ItemsSource = GetDataTable("SELECT * FROM Products").DefaultView;

            // Load Products
            stockMovementsDataGrid.ItemsSource = GetDataTable("SELECT * FROM StockMovements").DefaultView;

            // Load Suppliers
            salesOrdersDataGrid.ItemsSource = GetDataTable("SELECT * FROM SalesOrders").DefaultView;


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
        private void RecordMovementButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values
            int productID;
            string movementType = (movementTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            int quantity;
            string description = movementDescriptionTextBox.Text;

            // Validate inputs
            if (!int.TryParse(productIDTextBox.Text, out productID) || productID <= 0)
            {
                MessageBox.Show("Please enter a valid Product ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(movementQuantityTextBox.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(movementType))
            {
                MessageBox.Show("Please select a valid Movement Type.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // SQL queries for checking existence, inserting, and updating
            string checkProductQuery = "SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID";
            string checkStockMovementQuery = "SELECT COUNT(*) FROM StockMovements WHERE ProductID = @ProductID";
            string insertQuery = @"
        INSERT INTO StockMovements (ProductID, MovementType, Quantity, Description, MovementDate) 
        VALUES (@ProductID, @MovementType, @Quantity, @Description, GETDATE())";
            string updateQuery = @"
        UPDATE StockMovements
        SET MovementType = @MovementType, Quantity = @Quantity, Description = @Description, MovementDate = GETDATE()
        WHERE ProductID = @ProductID";

            // Initialize the connection and command objects
            using (var connection = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                // Check if the product exists in the Products table
                using (var productCommand = new SqlCommand(checkProductQuery, connection))
                {
                    productCommand.Parameters.AddWithValue("@ProductID", productID);
                    int productExists = (int)productCommand.ExecuteScalar();

                    if (productExists == 0) // If the product doesn't exist
                    {
                        MessageBox.Show("This product does not exist in the Products table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Check if the ProductID exists in the StockMovements table
                using (var stockMovementCommand = new SqlCommand(checkStockMovementQuery, connection))
                {
                    stockMovementCommand.Parameters.AddWithValue("@ProductID", productID);
                    int stockMovementExists = (int)stockMovementCommand.ExecuteScalar();

                    if (stockMovementExists > 0) // Update if the record exists
                    {
                        using (var updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@ProductID", productID);
                            updateCommand.Parameters.AddWithValue("@MovementType", movementType);
                            updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                            updateCommand.Parameters.AddWithValue("@Description", description);
                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show("Stock movement updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else // Insert if the record does not exist
                    {
                        using (var insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@ProductID", productID);
                            insertCommand.Parameters.AddWithValue("@MovementType", movementType);
                            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                            insertCommand.Parameters.AddWithValue("@Description", description);
                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Stock movement recorded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }

            // Optionally refresh the DataGrid to reflect the latest data (if needed)
            LoadData();
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

        private void AddOrderStatusButton_Click(object sender, RoutedEventArgs e)
        {
                // Check if any required field is empty
                if (string.IsNullOrEmpty(CustomerNameTextBox.Text) ||
                    StatusComboBox.SelectedItem == null ||
                    string.IsNullOrEmpty(TotalAmountTextBox.Text) ||
                    string.IsNullOrEmpty(OrderDescriptionTextBox.Text))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Exit if required fields are missing
                }

                // Try to parse TotalAmount as a decimal
                decimal totalAmount;
                if (!decimal.TryParse(TotalAmountTextBox.Text, out totalAmount))
                {
                    MessageBox.Show("Please enter a valid Total Amount.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string insertQuery = @"
                INSERT INTO SalesOrders (CustomerName, OrderDate, Status, TotalAmount, Order_Description)
                VALUES (@CustomerName, @OrderDate, @Status, @TotalAmount, @OrderDescription)";

                using (SqlConnection conn = new SqlConnection(DataBaseHelper.ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            // Set parameters from the user inputs
                            cmd.Parameters.AddWithValue("@CustomerName", CustomerNameTextBox.Text);
                            cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now); // Current date and time
                            cmd.Parameters.AddWithValue("@Status", (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                            cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            cmd.Parameters.AddWithValue("@OrderDescription", OrderDescriptionTextBox.Text);

                            // Execute the query
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Sales Order Added Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Optionally, you can reload the data grid or handle other UI updates.
                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                LoadData();
            
        
    }

        private void updateSalesOrder2(object sender, RoutedEventArgs e)
        {
            // Validate that all fields are filled
            if (string.IsNullOrEmpty(UpdateIdTextBox.Text) ||
                UpdateStatusComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(UpdateTotalAmountTextBox.Text) ||
                string.IsNullOrEmpty(UpdateOrderDescriptionTextBox.Text))
            {
                MessageBox.Show("Please fill in all the fields before updating.", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate that the ID is a valid integer
            int salesOrderId;
            if (!int.TryParse(UpdateIdTextBox.Text, out salesOrderId))
            {
                MessageBox.Show("Please enter a valid numeric ID.", "Invalid ID", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate that the Total Amount is a valid decimal
            decimal totalAmount;
            if (!decimal.TryParse(UpdateTotalAmountTextBox.Text, out totalAmount))
            {
                MessageBox.Show("Please enter a valid Total Amount.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Prepare the database connection and query 
            string checkQuery = "SELECT COUNT(*) FROM SalesOrders WHERE SalesOrderID = @SalesOrderID";
            string updateQuery = @"
        UPDATE SalesOrders
        SET Status = @Status, TotalAmount = @TotalAmount, Order_Description = @OrderDescription
        WHERE SalesOrderID = @SalesOrderID";

            using (SqlConnection conn = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Check if the ID exists in the database
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            MessageBox.Show($"No Sales Order found with ID {salesOrderId}.", "ID Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }

                    // Proceed with the update if the ID exists
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                        updateCmd.Parameters.AddWithValue("@Status", (UpdateStatusComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                        updateCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        updateCmd.Parameters.AddWithValue("@OrderDescription", UpdateOrderDescriptionTextBox.Text);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Sales Order updated successfully.", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Optionally, reload the data grid or handle other UI updates.
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the Sales Order. Please try again.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

 
    }
}
