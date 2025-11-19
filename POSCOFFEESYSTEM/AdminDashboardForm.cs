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
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        // ==========================
        // LOAD DASHBOARD DATA
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
                using (SqlCommand cmdTransactions = new SqlCommand("SELECT COUNT(*) FROM Transactions", con))
                    lblTotalTransaction.Text = cmdTransactions.ExecuteScalar()?.ToString() ?? "0";

                // Total Sales
                using (SqlCommand cmdSales = new SqlCommand("SELECT SUM(TotalAmount) FROM Transactions", con))
                {
                    object result = cmdSales.ExecuteScalar();
                    decimal totalSales = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0;
                    lblTotalSales.Text = $"₱{totalSales:N2}";
                }

                // Recent Transactions with Search Filter
                string queryRecent = @"
                    SELECT TOP 10
                        TransactionID,
                        Customer,
                        Date,
                        TotalAmount,
                        PaymentMethod
                    FROM Transactions
                ";

                if (!string.IsNullOrEmpty(searchFilter))
                {
                    queryRecent += @"
                        WHERE 
                            Customer LIKE @filter OR
                            PaymentMethod LIKE @filter OR
                            CAST(TransactionID AS NVARCHAR) LIKE @filter
                    ";
                }

                queryRecent += " ORDER BY Date DESC";

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

            int id = Convert.ToInt32(dataGridViewRecentTransactions.SelectedRows[0].Cells["TransactionID"].Value);

            DialogResult confirm = MessageBox.Show(
                $"Delete Transaction ID: {id}?",
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
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Transactions WHERE TransactionID = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
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
