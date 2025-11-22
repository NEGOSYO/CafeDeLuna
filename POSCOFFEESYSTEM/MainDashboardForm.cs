using System;
using System.Windows.Forms;

namespace POSCOFFEESYSTEM
{
    public partial class MainDashboardForm : Form
    {
        private Timer timer;
        private string currentUsername;
        private string currentRole;

        // Constructor after login
        public MainDashboardForm(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
            // 🚨 Re-adding the wiring call to ensure all buttons work
            WireButtonEvents();
        }

        // Default constructor (design time)
        public MainDashboardForm()
        {
            InitializeComponent();
            currentUsername = "DesignUser";
            currentRole = "Unknown";
            // 🚨 Re-adding the wiring call to ensure all buttons work
            WireButtonEvents();
        }

        private void MainDashboardForm_Load(object sender, EventArgs e)
        {
            // Welcome message
            lblWelcome.Text = $"Welcome, {currentUsername}";
            if (this.Controls.ContainsKey("lblRole"))
                lblRole.Text = $"Role: {currentRole}";

            // Live clock
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dddd, MMM dd yyyy hh:mm:ss tt";
            dateTimePicker1.Value = DateTime.Now;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        // =========================
        // Button Wiring
        // =========================
        private void WireButtonEvents()
        {
            // 🚨 Re-adding manual wiring for all buttons to ensure connectivity
            btnLogout.Click += btnLogout_Click;
            btnUserManagement.Click += btnUserManagement_Click;
            btnShop.Click += btnShop_Click;

            // Fix: Use correct inventory button name (assumed to be btnInventory for consistency)
            btnInventory.Click += InventoryButton_Click;

            btnAdminDashboard.Click += btnAdminDashboard_Click;
            btnStaffDashboard.Click += btnStaffDashboard_Click; // ⬅️ THIS LINE IS ESSENTIAL FOR THE FIX
        }

        // =========================
        // Button Handlers
        // =========================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timer?.Stop();
                timer?.Dispose();
                this.Hide();
                new LoginForm().Show();
            }
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            if (currentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                new UserManagementForm().ShowDialog();
            else
                MessageBox.Show("Access Denied. Only Admin can manage users.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            try
            {
                // Assuming 'Form1' is your POS/Shop Form
                new Form1(currentUsername, currentRole).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Shop: " + ex.Message);
            }
        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {
            new InventoryForm().Show();
        }

        private void btnAdminDashboard_Click(object sender, EventArgs e)
        {
            if (currentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    new AdminDashboardForm().ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening Admin Dashboard:\n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin can open Admin Dashboard.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStaffDashboard_Click(object sender, EventArgs e)
        {
            if (currentRole.Equals("Staff", StringComparison.OrdinalIgnoreCase) ||
                currentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    new StaffDashboardForm().ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening Staff Dashboard:\n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Access Denied. Only Staff/Admin can open Staff Dashboard.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblRole_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Current User Role: {currentRole}\nUsername: {currentUsername}",
                "User Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}