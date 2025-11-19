using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class StaffDashboardForm : Form
    {
        private readonly Database db = new Database();

        public StaffDashboardForm()
        {
            InitializeComponent();

            // Wire the Check button click
            Checkbtn.Click += Checkbtn_Click;
        }

        private void StaffDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        // ==========================
        // LOAD DASHBOARD DATA
        // ==========================
        private void LoadDashboardData()
        {
            SqlConnection con = null;
            try
            {
                con = db.GetConnection();
                con.Open();

                // ---------- TODAY'S SALES ----------
                using (SqlCommand cmdSales = new SqlCommand(
                    "SELECT ISNULL(SUM(TotalAmount), 0) FROM Transactions WHERE CAST(Date AS DATE) = CAST(GETDATE() AS DATE)", con))
                {
                    object result = cmdSales.ExecuteScalar();
                    decimal todaySales = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0;
                    lblTodaySales.Text = $"₱{todaySales:N2}";
                }

                // ---------- ORDERS IN PROCESS ----------
                using (SqlCommand cmdOrders = new SqlCommand(
                    "SELECT COUNT(*) FROM Transactions WHERE Status = 'Processing'", con))
                {
                    lblOrdersProcess.Text = cmdOrders.ExecuteScalar()?.ToString() ?? "0";
                }

                // ---------- LOW STOCK ITEMS ----------
                using (SqlCommand cmdLowStock = new SqlCommand(
                    "SELECT COUNT(*) FROM Products WHERE Quantity < 10", con))
                {
                    lblLowStock.Text = cmdLowStock.ExecuteScalar()?.ToString() ?? "0";
                }

                // ---------- SHOW RECENT TRANSACTIONS ----------
                string queryTransactions = @"
                    SELECT TOP 10
                        TransactionID,
                        Customer,
                        Date,
                        TotalAmount,
                        PaymentMethod,
                        Status
                    FROM Transactions
                    ORDER BY Date DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(queryTransactions, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    TransactionData.DataSource = dt;
                    TransactionData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    TransactionData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    TransactionData.MultiSelect = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        // ==========================
        // CHECK INVENTORY BUTTON
        // ==========================
        private void Checkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryForm invForm = new InventoryForm();
                invForm.ShowDialog(); // Opens the Inventory window
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Inventory:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}