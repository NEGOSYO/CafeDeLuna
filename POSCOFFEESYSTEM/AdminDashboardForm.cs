using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class AdminDashboardForm : Form
    {
        private readonly Database db = new Database();

        public AdminDashboardForm()
        {
            InitializeComponent();

            // Wire button events
            Searchbtn2.Click += Searchbtn2_Click;
            Clearbtn.Click += Clearbtn_Click;
            Deletebtn.Click += Deletebtn_Click;

            // Ensure DataGridView allows full row select
            dataGridViewRecentTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewRecentTransactions.MultiSelect = false;

            // Note: The SelectionChanged event handler is no longer needed 
            // since product details are now loaded with the main transaction list.
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        // ==========================
        // LOAD DASHBOARD DATA (MODIFIED TO INCLUDE PRODUCTS)
        // ==========================
        private void LoadDashboardData(string searchFilter = "")
        {
            SqlConnection con = null;

            try
            {
                con = db.GetConnection();
                con.Open();

                // Total Users
                using (SqlCommand cmdUsers = new SqlCommand("SELECT COUNT(*) FROM Users", con))
                    lblTotalUsers.Text = cmdUsers.ExecuteScalar()?.ToString() ?? "0";

                // Low Stock
                using (SqlCommand cmdLowStock = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Quantity < 10", con))
                    lblLowStock.Text = cmdLowStock.ExecuteScalar()?.ToString() ?? "0";

                // Total Transactions
                // Uses COUNT(DISTINCT) because the JOIN will duplicate transaction rows
                using (SqlCommand cmdTransactions = new SqlCommand("SELECT COUNT(DISTINCT [TransactionID]) FROM Transactions", con))
                    lblTotalTransaction.Text = cmdTransactions.ExecuteScalar()?.ToString() ?? "0";

                // Total Sales (Remains based on Transactions table for overall total)
                using (SqlCommand cmdSales = new SqlCommand("SELECT SUM(TotalAmount) FROM Transactions", con))
                {
                    object result = cmdSales.ExecuteScalar();
                    decimal totalSales = (result != DBNull.Value && result != null) ? Convert.ToDecimal(result) : 0;
                    lblTotalSales.Text = $"₱{totalSales:N2}";
                }

                // 🚨 MODIFIED QUERY: Uses JOINs to combine transaction header with product details
                string queryRecent = @"
                    SELECT TOP 20
                        T.[TransactionID],
                        T.Customer,
                        T.[Date],
                        T.TotalAmount,
                        T.PaymentMethod,
                        P.ProductName,       -- ⬅️ NEW: Product Name
                        TD.Quantity,         -- ⬅️ NEW: Quantity Bought
                        TD.Price AS ItemPrice -- ⬅️ NEW: Price of the item in that transaction
                    FROM 
                        dbo.Transactions T
                    INNER JOIN
                        dbo.TransactionDetails TD ON T.[TransactionID] = TD.TransactionID
                    INNER JOIN
                        dbo.Products P ON TD.ProductID = P.ProductID
                ";

                if (!string.IsNullOrEmpty(searchFilter))
                {
                    queryRecent += @"
                        WHERE 
                            T.Customer LIKE @filter OR
                            T.PaymentMethod LIKE @filter OR
                            P.ProductName LIKE @filter OR -- Allows searching by product name
                            CAST(T.[TransactionID] AS NVARCHAR) LIKE @filter
                    ";
                }

                // Order by date and then by TransactionID to group all products for one transaction together
                queryRecent += " ORDER BY T.[Date] DESC, T.[TransactionID], P.ProductName";

                using (SqlCommand cmd = new SqlCommand(queryRecent, con))
                {
                    if (!string.IsNullOrEmpty(searchFilter))
                        cmd.Parameters.AddWithValue("@filter", $"%{searchFilter}%");

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewRecentTransactions.DataSource = dt;
                        dataGridViewRecentTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
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
        // SEARCH BUTTON
        // ==========================
        private void Searchbtn2_Click(object sender, EventArgs e)
        {
            string search = Searchbox.Text.Trim();
            LoadDashboardData(search);
        }

        // Live Search
        private void Searchbox_TextChanged(object sender, EventArgs e)
        {
            LoadDashboardData(Searchbox.Text.Trim());
        }

        // ==========================
        // CLEAR BUTTON
        // ==========================
        private void Clearbtn_Click(object sender, EventArgs e)
        {
            Searchbox.Text = "";
            LoadDashboardData();
        }

        // ==========================
        // DELETE BUTTON
        // ==========================
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewRecentTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a transaction to delete.");
                return;
            }

            // IMPORTANT: Since the DataGridView has multiple rows for one transaction,
            // we must ensure we get the ID from the selected row, which should be consistent.
            int id = Convert.ToInt32(dataGridViewRecentTransactions.SelectedRows[0].Cells["TransactionID"].Value);

            DialogResult confirm = MessageBox.Show(
                $"Delete Transaction ID: {id} and all associated details?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    con.Open();

                    // 🚨 MODIFIED: Delete line items first due to foreign key constraints
                    using (SqlCommand cmdDetail = new SqlCommand(
                        "DELETE FROM TransactionDetails WHERE TransactionID = @id", con))
                    {
                        cmdDetail.Parameters.AddWithValue("@id", id);
                        cmdDetail.ExecuteNonQuery();
                    }

                    // Now delete the master transaction record
                    using (SqlCommand cmdMaster = new SqlCommand(
                        "DELETE FROM Transactions WHERE TransactionID = @id", con))
                    {
                        cmdMaster.Parameters.AddWithValue("@id", id);
                        cmdMaster.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Transaction deleted successfully!", "Success");

                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting transaction:\n" + ex.Message,
                    "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}