using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int GenerateReservationId()
            {
                Random random = new Random();
                int reservationId = random.Next(10000000, 99999999);
                return reservationId;
            }

            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");

            try
            {
                connect.Open();

                DateTime checkinDate;
                DateTime checkoutDate;

                if (!DateTime.TryParse(textBox1.Text, out checkinDate) || !DateTime.TryParse(textBox2.Text, out checkoutDate))
                {
                    MessageBox.Show("Invalid date format. Please enter valid dates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the checkin date is in the future
                if (checkinDate < DateTime.Now)
                {
                    MessageBox.Show("Check-in date must be in the future.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the checkout date is after the checkin date
                if (checkoutDate <= checkinDate)
                {
                    MessageBox.Show("Checkout date must be after the check-in date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use correct parameter names in the SQL query
                SqlCommand cmd = new SqlCommand("INSERT INTO Reservation VALUES (@checkin, @checkout, @Reservation_ID, @room_category, @receptionist_ID, @Branch_ID)", connect);

                cmd.Parameters.AddWithValue("@checkin", checkinDate);
                cmd.Parameters.AddWithValue("@checkout", checkoutDate);
                cmd.Parameters.AddWithValue("@room_category", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);

                int reservationId = GenerateReservationId();
                cmd.Parameters.AddWithValue("@Reservation_ID", reservationId);
                cmd.Parameters.AddWithValue("@Branch_ID", 1);
                cmd.Parameters.AddWithValue("@receptionist_ID",7);
                // Add other parameters as needed

                cmd.ExecuteNonQuery();

                // Display reservation ID in a message box
                MessageBox.Show("Successfully Registered! Reservation ID: " + reservationId, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");

            try
            {
                connect.Open();

                // Initialize the SQL query string for update
                string updateQuery = "UPDATE Reservation SET ";

                // Check each textbox and add corresponding update statements
                if (!string.IsNullOrEmpty(textBox1.Text))
                    updateQuery += "checkin = @checkin, ";
                if (!string.IsNullOrEmpty(textBox2.Text))
                    updateQuery += "checkout = @checkout, ";
                if (!string.IsNullOrEmpty(textBox3.Text))
                    updateQuery += "room_category = @room_category, ";

                // Remove the trailing comma and add the WHERE clause
                updateQuery = updateQuery.TrimEnd(',', ' ') + " WHERE Reservation_ID = @Reservation_ID";

                // Create the SqlCommand with the dynamic query
                SqlCommand cmd = new SqlCommand(updateQuery, connect);

                // Add parameters based on the non-empty textboxes
                if (DateTime.TryParse(textBox1.Text, out DateTime checkinDate))
                    cmd.Parameters.AddWithValue("@checkin", checkinDate);
                if (DateTime.TryParse(textBox2.Text, out DateTime checkoutDate))
                    cmd.Parameters.AddWithValue("@checkout", checkoutDate);
                if (!string.IsNullOrEmpty(textBox3.Text))
                    cmd.Parameters.AddWithValue("@room_category", textBox3.Text);

                // Add the common Reservation_ID parameter
                cmd.Parameters.AddWithValue("@Reservation_Id", string.IsNullOrEmpty(textBox5.Text) ? DBNull.Value : (object)textBox5.Text);

                // Execute the update query
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");
            connect.Open();
            SqlCommand cmd = new SqlCommand("select * from Reservation ", connect);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");
            try
            {
                connect.Open();

                // Check if the SSN exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Reservation WHERE Reservation_Id=@Reservation_Id", connect);
                checkCmd.Parameters.AddWithValue("@Reservation_Id", string.IsNullOrEmpty(textBox5.Text) ? DBNull.Value : (object)textBox5.Text);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("No matching Reservation Id found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                // Prompt the user for confirmation
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Proceed with deletion
                    SqlCommand deleteCmd = new SqlCommand("Delete FROM Reservation WHERE Reservation_Id=@Reservation_Id", connect);
                    deleteCmd.Parameters.AddWithValue("@Reservation_Id", string.IsNullOrEmpty(textBox5.Text) ? DBNull.Value : (object)textBox5.Text);

                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show("Successfully Deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox5.Text, out _))
            {
                MessageBox.Show("Please enter a valid Reservation_Id (numeric value).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;"))
            {
                connect.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation WHERE Reservation_Id=@Reservation_Id", connect))
                {
                    cmd.Parameters.AddWithValue("@Reservation_Id", textBox5.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Reservation_Id not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            dataGridView1.DataSource = dt;
                        }
                    }
                }

                connect.Close();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 from3 = new Form1();
            from3.Show();
            Visible = false;
        }
    }
}
