using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");
            SqlTransaction transaction = null;

            try
            {
                connect.Open();
                transaction = connect.BeginTransaction();

                // Validate first name (letters only)
                if (!IsValidName(textBox1.Text))
                {
                    MessageBox.Show("Invalid first name. Please enter letters only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Stop execution if validation fails
                }

                // Validate last name (letters only)
                if (!IsValidName(textBox6.Text))
                {
                    MessageBox.Show("Invalid last name. Please enter letters only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Stop execution if validation fails
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO Guest VALUES (@Fname, @Lname, @email, @gender, @membership, @SSN, @reservation_ID)", connect, transaction);

                cmd.Parameters.AddWithValue("@Fname", string.IsNullOrEmpty(textBox1.Text) ? DBNull.Value : (object)textBox1.Text);
                cmd.Parameters.AddWithValue("@Lname", string.IsNullOrEmpty(textBox6.Text) ? DBNull.Value : (object)textBox6.Text);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(textBox5.Text) ? DBNull.Value : (object)textBox5.Text);
                cmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(textBox2.Text) ? DBNull.Value : (object)textBox2.Text);
                cmd.Parameters.AddWithValue("@membership", string.IsNullOrEmpty(textBox4.Text) ? DBNull.Value : (object)textBox4.Text);
                cmd.Parameters.AddWithValue("@reservation_ID", string.IsNullOrEmpty(textBox7.Text) ? DBNull.Value : (object)textBox7.Text);
                cmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("INSERT INTO Guest_phone VALUES (@SSN, @phonenum)", connect, transaction);

                cmd2.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
                cmd2.Parameters.AddWithValue("@phonenum", string.IsNullOrEmpty(textBox8.Text) ? DBNull.Value : (object)textBox8.Text);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("INSERT INTO GuestAddres VALUES (@SSN, @address)", connect, transaction);
                cmd3.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
                cmd3.Parameters.AddWithValue("@address", string.IsNullOrEmpty(textBox9.Text) ? DBNull.Value : (object)textBox9.Text);
                cmd3.ExecuteNonQuery();

                transaction.Commit(); // If all commands are successful, commit the transaction.

                MessageBox.Show("Successfully Registered!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form2 form2 = new Form2();
                form2.Show();
                Visible = false;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback(); // If an error occurs, rollback the transaction.

                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private bool IsValidName(string input)
        {
            // Use a regular expression to check if the input contains only letters
            return System.Text.RegularExpressions.Regex.IsMatch(input, "^[a-zA-Z]+$");
        }



        private void UpdateGuestInfo(SqlConnection connect)
        {
            // Initialize the SQL query string
            string updateQuery = "UPDATE Guest SET ";

            // Check each textbox and add corresponding update statements
            if (!string.IsNullOrEmpty(textBox1.Text))
                updateQuery += "Fname = @Fname, ";
            if (!string.IsNullOrEmpty(textBox6.Text))
                updateQuery += "Lname = @Lname, ";
            if (!string.IsNullOrEmpty(textBox5.Text))
                updateQuery += "email = @email, ";
            if (!string.IsNullOrEmpty(textBox2.Text))
                updateQuery += "gender = @gender, ";
            if (!string.IsNullOrEmpty(textBox4.Text))
                updateQuery += "membership = @membership, ";
            if (!string.IsNullOrEmpty(textBox7.Text))
                updateQuery += "reservation_ID = @reservation_ID, ";

            // Remove the trailing comma and add the WHERE clause
            updateQuery = updateQuery.TrimEnd(',', ' ') + " WHERE SSN = @SSN";

            // Create the SqlCommand with the dynamic query
            SqlCommand cmd = new SqlCommand(updateQuery, connect);

            // Add parameters based on the non-empty textboxes
            if (!string.IsNullOrEmpty(textBox1.Text))
                cmd.Parameters.AddWithValue("@Fname", textBox1.Text);
            if (!string.IsNullOrEmpty(textBox6.Text))
                cmd.Parameters.AddWithValue("@Lname", textBox6.Text);
            if (!string.IsNullOrEmpty(textBox5.Text))
                cmd.Parameters.AddWithValue("@email", textBox5.Text);
            if (!string.IsNullOrEmpty(textBox2.Text))
                cmd.Parameters.AddWithValue("@gender", textBox2.Text);
            if (!string.IsNullOrEmpty(textBox4.Text))
                cmd.Parameters.AddWithValue("@membership", textBox4.Text);
            if (!string.IsNullOrEmpty(textBox7.Text))
                cmd.Parameters.AddWithValue("@reservation_ID", textBox7.Text);

            // Add the common SSN parameter
            cmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);

            // Execute the update query
            cmd.ExecuteNonQuery();

            MessageBox.Show("Guest Information Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateGuestPhone(SqlConnection connect)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Guest_phone SET phonenum = @phonenum WHERE SSN = @SSN", connect);

            cmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
            cmd.Parameters.AddWithValue("@phonenum", string.IsNullOrEmpty(textBox8.Text) ? DBNull.Value : (object)int.Parse(textBox8.Text));

            cmd.ExecuteNonQuery();

            MessageBox.Show("Guest Phone Number Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateGuestAddress(SqlConnection connect)
        {
            SqlCommand cmd = new SqlCommand("UPDATE GuestAddres SET address = @address WHERE SSN = @SSN", connect);
            cmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(textBox9.Text) ? DBNull.Value : (object)textBox9.Text);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Guest Address Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateGuest(SqlConnection connect, string entityToEdit, string fname, string lname, string email, string gender, string membership, string reservationID, int? phoneNum, string address, string ssn)
        {
            try
            {
                connect.Open();

                // Update Guest Information
                string updateInfoQuery = "UPDATE Guest SET ";
                updateInfoQuery += !string.IsNullOrEmpty(fname) && (entityToEdit == "Fname") ? "Fname = @Fname, " : "";
                updateInfoQuery += !string.IsNullOrEmpty(lname) && (entityToEdit == "Lname") ? "Lname = @Lname, " : "";
                updateInfoQuery += !string.IsNullOrEmpty(email) && (entityToEdit == "email") ? "email = @email, " : "";
                updateInfoQuery += !string.IsNullOrEmpty(gender) && (entityToEdit == "gender") ? "gender = @gender, " : "";
                updateInfoQuery += !string.IsNullOrEmpty(membership) && (entityToEdit == "membership") ? "membership = @membership, " : "";
                updateInfoQuery += !string.IsNullOrEmpty(reservationID) && (entityToEdit == "reservation_ID") ? "reservation_ID = @reservation_ID, " : "";
                updateInfoQuery = updateInfoQuery.TrimEnd(',', ' ') + " WHERE SSN = @SSN";

                SqlCommand cmdInfo = new SqlCommand(updateInfoQuery, connect);
                cmdInfo.Parameters.AddWithValue("@Fname", string.IsNullOrEmpty(fname) ? DBNull.Value : (object)fname);
                cmdInfo.Parameters.AddWithValue("@Lname", string.IsNullOrEmpty(lname) ? DBNull.Value : (object)lname);
                cmdInfo.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? DBNull.Value : (object)email);
                cmdInfo.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(gender) ? DBNull.Value : (object)gender);
                cmdInfo.Parameters.AddWithValue("@membership", string.IsNullOrEmpty(membership) ? DBNull.Value : (object)membership);
                cmdInfo.Parameters.AddWithValue("@reservation_ID", string.IsNullOrEmpty(reservationID) ? DBNull.Value : (object)reservationID);
                cmdInfo.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(ssn) ? DBNull.Value : (object)ssn);
                cmdInfo.ExecuteNonQuery();

                MessageBox.Show($"{entityToEdit} Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Update Guest Phone
                if (phoneNum.HasValue && entityToEdit == "phonenum")
                {
                    SqlCommand cmdPhone = new SqlCommand("UPDATE Guest_phone SET phonenum = @phonenum WHERE SSN = @SSN", connect);
                    cmdPhone.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(ssn) ? DBNull.Value : (object)ssn);
                    cmdPhone.Parameters.AddWithValue("@phonenum", phoneNum.Value);
                    cmdPhone.ExecuteNonQuery();

                    MessageBox.Show("Guest Phone Number Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update Guest Address
                if (!string.IsNullOrEmpty(address) && entityToEdit == "address")
                {
                    SqlCommand cmdAddress = new SqlCommand("UPDATE GuestAddres SET address = @address WHERE SSN = @SSN", connect);
                    cmdAddress.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(ssn) ? DBNull.Value : (object)ssn);
                    cmdAddress.Parameters.AddWithValue("@address", address);
                    cmdAddress.ExecuteNonQuery();

                    MessageBox.Show("Guest Address Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");

          
        }



        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");

            try
            {
                connect.Open();

                // Check if the SSN exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Guest WHERE SSN=@SSN", connect);
                checkCmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("No matching SSN found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if SSN does not exist
                }

                // Prompt the user for confirmation
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Proceed with deletion
                    SqlCommand deleteCmd = new SqlCommand("DELETE FROM Guest_phone WHERE SSN=@SSN; DELETE FROM GuestAddres WHERE SSN=@SSN; DELETE FROM Guest WHERE SSN=@SSN", connect);
                    deleteCmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);

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





        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");
            connect.Open();
            SqlCommand cmd = new SqlCommand("select * from Guest ",connect);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);  
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource=dt ;
            cmd.ExecuteNonQuery();


            SqlCommand cmd2 = new SqlCommand("select * from GuestAddres ", connect);
            SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            dataGridView2.DataSource=dt2;
            cmd2.ExecuteNonQuery();

            SqlCommand cmd3 = new SqlCommand("select * from Guest_phone ", connect);
            SqlDataAdapter adapter3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            adapter3.Fill(dt3);
            dataGridView3.DataSource = dt3;
            cmd3.ExecuteNonQuery();

            connect.Close();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");
            try
            {
                connect.Open();

                // Initialize the SQL query string
                string updateQuery = "UPDATE Guest SET ";

                // Check each textbox and add corresponding update statements
                if (!string.IsNullOrEmpty(textBox1.Text))
                    updateQuery += "Fname = @Fname, ";
                if (!string.IsNullOrEmpty(textBox6.Text))
                    updateQuery += "Lname = @Lname, ";
                if (!string.IsNullOrEmpty(textBox5.Text))
                    updateQuery += "email = @email, ";
                if (!string.IsNullOrEmpty(textBox2.Text))
                    updateQuery += "gender = @gender, ";
                if (!string.IsNullOrEmpty(textBox4.Text))
                    updateQuery += "membership = @membership, ";
                if (!string.IsNullOrEmpty(textBox7.Text))
                    updateQuery += "reservation_ID = @reservation_ID, ";

                // Remove the trailing comma and add the WHERE clause
                updateQuery = updateQuery.TrimEnd(',', ' ') + " WHERE SSN = @SSN";

                // Create the SqlCommand with the dynamic query
                SqlCommand cmd = new SqlCommand(updateQuery, connect);

                // Add parameters based on the non-empty textboxes
                if (!string.IsNullOrEmpty(textBox1.Text))
                    cmd.Parameters.AddWithValue("@Fname", textBox1.Text);
                if (!string.IsNullOrEmpty(textBox6.Text))
                    cmd.Parameters.AddWithValue("@Lname", textBox6.Text);
                if (!string.IsNullOrEmpty(textBox5.Text))
                    cmd.Parameters.AddWithValue("@email", textBox5.Text);
                if (!string.IsNullOrEmpty(textBox2.Text))
                    cmd.Parameters.AddWithValue("@gender", textBox2.Text);
                if (!string.IsNullOrEmpty(textBox4.Text))
                    cmd.Parameters.AddWithValue("@membership", textBox4.Text);
                if (!string.IsNullOrEmpty(textBox7.Text))
                    cmd.Parameters.AddWithValue("@reservation_ID", textBox7.Text);

                // Add the common SSN parameter
                cmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);

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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;"))
                {
                    connect.Open(); // Open the connection

                    using (SqlCommand cmd2 = new SqlCommand("UPDATE Guest_phone SET phonenum = @phonenum WHERE SSN = @SSN", connect))
                    {
                        // Use Parameters to prevent SQL injection
                        cmd2.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
                        cmd2.Parameters.Add("@phonenum", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(textBox8.Text) ? DBNull.Value : (object)textBox8.Text;

                        // Execute the query
                        int rowsAffected = cmd2.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Inform the user that the update was successful
                            MessageBox.Show("Update successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // No rows were affected, meaning the record with the specified SSN was not found
                            MessageBox.Show("No matching record found. Update unsuccessful.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display, or both)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;"))
                {
                    connect.Open(); // Open the connection

                    using (SqlCommand cmd3 = new SqlCommand("UPDATE GuestAddres SET address = @address WHERE SSN = @SSN", connect))
                    {
                        // Use Parameters to prevent SQL injection
                        cmd3.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox3.Text) ? DBNull.Value : (object)textBox3.Text);
                        cmd3.Parameters.AddWithValue("@address", string.IsNullOrEmpty(textBox9.Text) ? DBNull.Value : (object)textBox9.Text);

                        // Execute the query
                        int rowsAffected = cmd3.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Inform the user that the update was successful
                            MessageBox.Show("Update successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // No rows were affected, meaning the record with the specified SSN was not found
                            MessageBox.Show("No matching record found. Update unsuccessful.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display, or both)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
    


