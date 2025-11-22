using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class InventoryForm : Form
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=CafeDB;Trusted_Connection=True;";

        public InventoryForm()
        {
            InitializeComponent();
            LoadProducts();
            LoadCategoryCombo();
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
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT P.ProductID, P.ProductName, P.Price, P.Quantity,
                             C.CategoryID, C.CategoryName
                      FROM Products P
                      LEFT JOIN Categories C ON P.CategoryID = C.CategoryID",
                    con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                ProductList.DataSource = dt;
            }
        }

        // LOAD CATEGORIES INTO COMBOBOX
        private void LoadCategoryCombo()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Categories", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCategory.DataSource = dt;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
            }
        }

        // LOAD CATEGORIES FOR FILTER DROPDOWN
        private void LoadCategoryFilter()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Categories", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ctegoryFilter.Items.Clear();
                ctegoryFilter.Items.Add("All");

                foreach (DataRow row in dt.Rows)
                    ctegoryFilter.Items.Add(row["CategoryName"].ToString());

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
                    "INSERT INTO Products (ProductName, CategoryID, Price, Quantity, DateAdded) VALUES (@n, @cid, @p, @q, @d)", con);

                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@cid", cmbCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@p", decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("@q", int.Parse(txtQuantity.Text));
                cmd.Parameters.AddWithValue("@d", DateTime.Now);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Added!");
            LoadProducts();
        }

        // CLICK ROW
        private void ProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtID.Text = ProductList.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();
                txtName.Text = ProductList.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                txtPrice.Text = ProductList.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                txtQuantity.Text = ProductList.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();

                // Set ComboBox using CategoryID
                cmbCategory.SelectedValue = ProductList.Rows[e.RowIndex].Cells["CategoryID"].Value;
            }
        }

        // UPDATE PRODUCT
        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID.Text, out int productId))
            {
                MessageBox.Show("Please select a product first!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Products 
                      SET ProductName=@n, CategoryID=@cid, Price=@p, Quantity=@q 
                      WHERE ProductID=@id", con);

                cmd.Parameters.AddWithValue("@id", productId);
                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@cid", cmbCategory.SelectedValue);
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
            if (!int.TryParse(txtID.Text, out int productId))
            {
                MessageBox.Show("Select a product first!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID=@id", con);
                cmd.Parameters.AddWithValue("@id", productId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Deleted!");
            LoadProducts();
        }

        // SEARCH BY PRODUCT NAME
        private void Searchbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT P.ProductID, P.ProductName, P.Price, P.Quantity,
                             C.CategoryID, C.CategoryName
                      FROM Products P
                      JOIN Categories C ON P.CategoryID = C.CategoryID
                      WHERE P.ProductName LIKE @s", con);

                da.SelectCommand.Parameters.AddWithValue("@s", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                ProductList.DataSource = dt;
            }
        }

        // FILTER BY CATEGORY
        private void ctegoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ctegoryFilter.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = selected == "All"
                    ? @"SELECT P.ProductID, P.ProductName, P.Price, P.Quantity,
                               C.CategoryID, C.CategoryName
                        FROM Products P
                        JOIN Categories C ON P.CategoryID = C.CategoryID"
                    : @"SELECT P.ProductID, P.ProductName, P.Price, P.Quantity,
                               C.CategoryID, C.CategoryName
                        FROM Products P
                        JOIN Categories C ON P.CategoryID = C.CategoryID
                        WHERE C.CategoryName=@c";

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
