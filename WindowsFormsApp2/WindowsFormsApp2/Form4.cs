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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=DESKTOP-A2O2MQ6;Initial Catalog=finalproject;Integrated Security=True;");

            try
            {
                connect.Open();

                // Check if the SSN exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Guest WHERE SSN=@SSN", connect);
                checkCmd.Parameters.AddWithValue("@SSN", string.IsNullOrEmpty(textBox1.Text) ? DBNull.Value : (object)textBox1.Text);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("No matching SSN found. Please Register First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Form1 from3 = new Form1();
                    from3.Show();
                    Visible = false;
                    return; 
                }
                else
                {
                    Form2 from2 = new Form2();
                    from2.Show();
                    Visible = false;
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
    }
}
