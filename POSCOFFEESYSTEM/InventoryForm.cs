using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class InventoryForm : Form
    {
        string connectionString = @"Server=DESKTOP-F6NJNVH\SQLEXPRESS;Database=CafeDB;Trusted_Connection=True;";

        public InventoryForm()
        {
            InitializeComponent();
            LoadProducts();
            LoadCategoryFilter();

            ProductList.CellClick += ProductList_CellClick;
            Addbtn.Click += Addbtn_Click;
            Editbtn.Click += Editbtn_Click;
            Deletebtn.Click += Deletebtn_Click;
            Searchbtn.Click += Searchbtn_Click;
            ctegoryFilter.SelectedIndexChanged += ctegoryFilter_SelectedIndexChanged;
        }

        // LOAD PRODUCTS
        private void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ProductList.DataSource = dt;
            }
        }

        // LOAD CATEGORIES
        private void LoadCategoryFilter()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Category FROM Products", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ctegoryFilter.Items.Clear();
                ctegoryFilter.Items.Add("All");

                foreach (DataRow row in dt.Rows)
                    ctegoryFilter.Items.Add(row["Category"].ToString());

                ctegoryFilter.SelectedIndex = 0;
            }
        }

        // ADD PRODUCT
        private void Addbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Products (ProductName, Category, Price, Quantity) VALUES (@n, @c, @p, @q)", con);

                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@c", txtCategory.Text);
                cmd.Parameters.AddWithValue("@p", decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("@q", int.Parse(txtQuantity.Text));

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Added!");
            LoadProducts();
            LoadCategoryFilter();
        }

        // CLICK ROW
        private void ProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtID.Text = ProductList.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();
                txtName.Text = ProductList.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                txtCategory.Text = ProductList.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                txtPrice.Text = ProductList.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                txtQuantity.Text = ProductList.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
            }
        }

        // UPDATE PRODUCT
        private void Editbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Products SET ProductName=@n, Category=@c, Price=@p, Quantity=@q WHERE ProductID=@id", con);

                if (!int.TryParse(txtID.Text, out int productId))
                {
                    MessageBox.Show("Please select a valid product to edit.");
                    return;
                }
                cmd.Parameters.AddWithValue("@id", productId);
                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@c", txtCategory.Text);
                cmd.Parameters.AddWithValue("@p", decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("@q", int.Parse(txtQuantity.Text));

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Updated!");
            LoadProducts();
        }

        // DELETE PRODUCT
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID=@id", con);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Deleted!");
            LoadProducts();
            LoadCategoryFilter();
        }

        // SEARCH
        private void Searchbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Products WHERE ProductName LIKE @s", con);

                da.SelectCommand.Parameters.AddWithValue("@s", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                ProductList.DataSource = dt;
            }
        }

        // CATEGORY FILTER
        private void ctegoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ctegoryFilter.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = selected == "All"
                    ? "SELECT * FROM Products"
                    : "SELECT * FROM Products WHERE Category=@c";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                if (selected != "All")
                    da.SelectCommand.Parameters.AddWithValue("@c", selected);

                DataTable dt = new DataTable();
                da.Fill(dt);
                ProductList.DataSource = dt;
            }
        }
    }
}
