using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POSCOFFEESYSTEM
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, decimal> menuItems = new Dictionary<string, decimal>
        {
            {"Midnight Mocha", 200m},
            {"Caramel Cloud", 200m},
            {"Forest Latte", 175m},
            {"Fireside Cappuccino", 150m},
            {"Morning Bliss", 110m},
            {"Nutty Warmth", 175m},
            {"Mocha Loca", 190m},
            {"Cinnamon Hug", 190m},
            {"Amber Latte", 175m}
        };

        private string currentUser = "Admin";
        private string currentRole = "Staff";

        private List<(string item, decimal price, int qty)> cart = new List<(string, decimal, int)>();

        private string connectionString = @"Server=DESKTOP-F6NJNVH\SQLEXPRESS;Database=CafeDB;Trusted_Connection=True;";

        public Form1()
        {
            InitializeComponent();
            InitializeFormDefaults();
            WireEvents();
        }

        public Form1(string username, string role) : this()
        {
            currentUser = username;
            currentRole = role;
        }

        private void InitializeFormDefaults()
        {
            lblTotal.Text = "₱0.00";
            change.Text = "₱0.00";
            generatereceipt.Enabled = false;
        }

        private void WireEvents()
        {
            Paybtn.Click += (s, e) => CalculateChange();
            calculate.Click += (s, e) => CalculateChange();
            generatereceipt.Click += (s, e) => GenerateReceipt();
            btnclear.Click += (s, e) => ClearOrder();

            addtocart1.Click += (s, e) => AddItemToCart("Midnight Mocha", menuItems["Midnight Mocha"], (int)numericUpDown1.Value, numericUpDown1);
            addtocart2.Click += (s, e) => AddItemToCart("Caramel Cloud", menuItems["Caramel Cloud"], (int)numericUpDown2.Value, numericUpDown2);
            addtocart3.Click += (s, e) => AddItemToCart("Forest Latte", menuItems["Forest Latte"], (int)numericUpDown3.Value, numericUpDown3);
            addtocart4.Click += (s, e) => AddItemToCart("Fireside Cappuccino", menuItems["Fireside Cappuccino"], (int)numericUpDown4.Value, numericUpDown4);
            addtocart5.Click += (s, e) => AddItemToCart("Morning Bliss", menuItems["Morning Bliss"], (int)numericUpDown5.Value, numericUpDown5);
            addtocart6.Click += (s, e) => AddItemToCart("Nutty Warmth", menuItems["Nutty Warmth"], (int)numericUpDown6.Value, numericUpDown6);
            addtocart7.Click += (s, e) => AddItemToCart("Mocha Loca", menuItems["Mocha Loca"], (int)numericUpDown7.Value, numericUpDown7);
            addtocart8.Click += (s, e) => AddItemToCart("Cinnamon Hug", menuItems["Cinnamon Hug"], (int)numericUpDown8.Value, numericUpDown8);
            addtocart9.Click += (s, e) => AddItemToCart("Amber Latte", menuItems["Amber Latte"], (int)numericUpDown9.Value, numericUpDown9);
        }

        private int GetStock(string productName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Quantity FROM Products WHERE ProductName=@name", con);
                cmd.Parameters.AddWithValue("@name", productName);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void DeductStock(string productName, int qty)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Products SET Quantity = Quantity - @qty WHERE ProductName=@name", con);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@name", productName);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddItemToCart(string itemName, decimal price, int qty, NumericUpDown numeric)
        {
            if (qty <= 0)
            {
                MessageBox.Show("Select quantity > 0");
                return;
            }

            int stock = GetStock(itemName);

            if (stock <= 0)
            {
                MessageBox.Show($"{itemName} is OUT OF STOCK");
                numeric.Value = 0;
                return;
            }

            if (qty > stock)
            {
                MessageBox.Show($"Not enough stock! Available: {stock}");
                numeric.Value = 0;
                return;
            }

            cart.Add((itemName, price, qty));
            decimal subtotal = price * qty;

            richTextBoxOrderSummary.AppendText($"{qty}x {itemName} @ ₱{price:0.00} = ₱{subtotal:0.00}\n");
            UpdateTotalDisplay();
            numeric.Value = 0;
        }

        private void UpdateTotalDisplay()
        {
            decimal total = cart.Sum(i => i.price * i.qty);
            lblTotal.Text = $"₱{total:0.00}";
        }

        private void CalculateChange()
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Add items first");
                return;
            }

            if (!decimal.TryParse(lblTotal.Text.Replace("₱", ""), out decimal total))
            {
                MessageBox.Show("Invalid total");
                return;
            }

            if (!decimal.TryParse(txtAmountPaid.Text, out decimal paid))
            {
                MessageBox.Show("Enter valid payment amount");
                return;
            }

            decimal changeAmount = paid - total;

            if (changeAmount < 0)
            {
                MessageBox.Show($"Insufficient payment! Need ₱{Math.Abs(changeAmount):0.00}");
                change.Text = "₱0.00";
                generatereceipt.Enabled = false;
                return;
            }

            // Deduct stock immediately after successful payment
            foreach (var item in cart)
                DeductStock(item.item, item.qty);

            change.Text = $"₱{changeAmount:0.00}";
            generatereceipt.Enabled = true;

            MessageBox.Show("Payment successful! Stock updated.");
        }

        private void GenerateReceipt()
        {
            if (cart.Count == 0 || string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Enter customer name and add items first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalAmount = cart.Sum(i => i.price * i.qty);
            string customerName = txtCustomerName.Text.Trim();
            string paymentMethod = "Cash"; // or use a dropdown if you have multiple methods

            // Insert into Transactions table WITHOUT UserID
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string insertQuery = @"
                INSERT INTO Transactions
                (Date, TotalAmount, PaymentMethod, Customer, Status)
                VALUES
                (@date, @total, @payment, @customer, @status)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@total", totalAmount);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@customer", string.IsNullOrEmpty(customerName) ? DBNull.Value : (object)customerName);
                        cmd.Parameters.AddWithValue("@status", "Completed");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save receipt to desktop
            try
            {
                StringBuilder receipt = new StringBuilder();
                receipt.AppendLine("===== Cafe de Luna Receipt =====");
                receipt.AppendLine($"Customer: {customerName}");
                receipt.AppendLine($"Staff: {currentUser}");
                receipt.AppendLine($"Date: {DateTime.Now}");
                receipt.AppendLine("--------------------------------");
                receipt.Append(richTextBoxOrderSummary.Text);
                receipt.AppendLine("--------------------------------");
                receipt.AppendLine($"Total: ₱{totalAmount:0.00}");
                receipt.AppendLine($"Amount Paid: ₱{txtAmountPaid.Text}");
                receipt.AppendLine($"Change: {change.Text}");
                receipt.AppendLine("================================");
                receipt.AppendLine("Thank you for your purchase!");

                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                File.WriteAllText(Path.Combine(desktop, fileName), receipt.ToString());

                MessageBox.Show($"Receipt saved to Desktop as: {fileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearOrder()
        {
            cart.Clear();
            richTextBoxOrderSummary.Clear();
            lblTotal.Text = "₱0.00";
            change.Text = "₱0.00";
            txtAmountPaid.Clear();
            txtCustomerName.Clear();

            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            numericUpDown9.Value = 0;

            generatereceipt.Enabled = false;
        }
    }
}
