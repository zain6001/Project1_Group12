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

namespace VisualProject_Group12_WPF
{
    public partial class ManagerForm : Window
    {
        

        public ManagerForm()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {

      
            ProductsDataGrid.ItemsSource = GetDataTable("SELECT * FROM Products").DefaultView;

            
            SalesPersonsDataGrid.ItemsSource = GetDataTable("SELECT * FROM Suppliers").DefaultView;

            OrderssDataGrid.ItemsSource = GetDataTable("SELECT * FROM SalesOrders").DefaultView;

            StockMovementsDataGrid.ItemsSource = GetDataTable("SELECT * FROM StockMovements").DefaultView;
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
        private void ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }


     

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
    }
}

