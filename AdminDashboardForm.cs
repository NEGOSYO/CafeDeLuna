using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class AdminDashboardForm : Form
    {
        Database db = new Database(); // Uses your existing Database.cs connection class

        public AdminDashboardForm()
        {
            InitializeComponent();
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(db.connectionString))
                {
                    con.Open();

                    // 🔹 1. Total Users
                    SqlCommand cmdUsers = new SqlCommand("SELECT COUNT(*) FROM Users", con);
                    int totalUsers = (int)cmdUsers.ExecuteScalar();
                    lblTotalUsers.Text = totalUsers.ToString();

                    // 🔹 2. Low Stock Items (edit Stock < 10 if you want a different threshold)
                    SqlCommand cmdLowStock = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Stock < 10", con);
                    int lowStock = (int)cmdLowStock.ExecuteScalar();
                    lblLowStock.Text = lowStock.ToString();

                    // 🔹 3. Total Transactions
                    SqlCommand cmdTransactions = new SqlCommand("SELECT COUNT(*) FROM Transactions", con);
                    int totalTransactions = (int)cmdTransactions.ExecuteScalar();
                    lblTotalTransaction.Text = totalTransactions.ToString();

                    // 🔹 4. Total Sales
                    SqlCommand cmdSales = new SqlCommand("SELECT SUM(TotalAmount) FROM Transactions", con);
                    object result = cmdSales.ExecuteScalar();
                    lblTotalSales.Text = result != DBNull.Value ? $"₱{Convert.ToDecimal(result):N2}" : "₱0.00";

                    // 🔹 5. Load 10 Recent Transactions into DataGridView
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT TOP 10 TransactionID, Date, TotalAmount FROM Transactions ORDER BY Date DESC", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewRecentTransactions.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Optional: add manual refresh button
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}
