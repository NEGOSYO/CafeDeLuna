using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class UserManagementForm : Form
    {
        // NOTE: Update this to your actual SQL Server instance if different.
        // Common local SQL Server Express default: @".\SQLEXPRESS"
        // Example remote/server name: @"PRLXAJ\SQLEXPRESS" or @"SERVERNAME\INSTANCE"
        string connectionString = @"Data Source=DESKTOP-F6NJNVH\SQLEXPRESS;Initial Catalog=CafeDB;Integrated Security=True";

        public UserManagementForm()
        {
            InitializeComponent();
            LoadUsers();
            LoadRoles();
        }

        // ✅ Load Roles into ComboBox (Admin / Staff)
        private void LoadRoles()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT RoleID, RoleName FROM Roles", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Rolebox.DataSource = dt;
                    Rolebox.DisplayMember = "RoleName";
                    Rolebox.ValueMember = "RoleID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load roles: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Load Users into DataGridView
        private void LoadUsers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT u.UserID, u.FullName, u.Username, u.PasswordHash, r.RoleName " +
                        "FROM Users u JOIN Roles r ON u.RoleID = r.RoleID", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message + Environment.NewLine +
                                "Check your connection string and database server.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Add New User
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(addName.Text) ||
                string.IsNullOrWhiteSpace(AddUser.Text) ||
                string.IsNullOrWhiteSpace(Addpass.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Users (FullName, Username, PasswordHash, RoleID) VALUES (@FullName, @Username, @Password, @RoleID)", con);
                    cmd.Parameters.AddWithValue("@FullName", addName.Text);
                    cmd.Parameters.AddWithValue("@Username", AddUser.Text);
                    cmd.Parameters.AddWithValue("@Password", Addpass.Text);
                    cmd.Parameters.AddWithValue("@RoleID", Rolebox.SelectedValue);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Edit Existing User
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UserID"].Value);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Users SET FullName=@FullName, Username=@Username, PasswordHash=@Password, RoleID=@RoleID WHERE UserID=@UserID", con);
                    cmd.Parameters.AddWithValue("@FullName", addName.Text);
                    cmd.Parameters.AddWithValue("@Username", AddUser.Text);
                    cmd.Parameters.AddWithValue("@Password", Addpass.Text);
                    cmd.Parameters.AddWithValue("@RoleID", Rolebox.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Delete User
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UserID"].Value);

            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserID=@UserID", con);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("User deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ✅ Reset/Clear Fields
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            addName.Text = "";
            AddUser.Text = "";
            Addpass.Text = "";
            Rolebox.SelectedIndex = -1;
        }

        // ✅ Load selected row into input boxes
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                addName.Text = row.Cells["FullName"].Value.ToString();
                AddUser.Text = row.Cells["Username"].Value.ToString();
                Addpass.Text = row.Cells["PasswordHash"].Value.ToString();
                Rolebox.Text = row.Cells["RoleName"].Value.ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}



