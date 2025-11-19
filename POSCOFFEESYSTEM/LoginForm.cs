using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class LoginForm : Form
    {
        private readonly Database db = new Database("CafeDB");

        public LoginForm()
        {
            InitializeComponent();
        }

        private void Loginbtn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    con.Open();

                    string query = @"
                        SELECT U.FullName, U.Username, R.RoleName 
                        FROM Users U
                        INNER JOIN Roles R ON U.RoleID = R.RoleID
                        WHERE U.Username = @username AND U.PasswordHash = @password AND U.Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = reader["FullName"].ToString();
                                string role = reader["RoleName"].ToString();

                                MessageBox.Show($"Welcome, {fullName}!", "Login Successful",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.Hide();
                                MainDashboardForm main = new MainDashboardForm(fullName, role);
                                main.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password, or account inactive.",
                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during login: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ForgotPassLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact the administrator to reset your password.",
                "Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SUlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Sign up is disabled. Please contact admin to create a new account.",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text = "admin";  // optional
            txtPassword.Text = "admin123"; // optional
            txtUsername.Focus();
        }
    }
}
