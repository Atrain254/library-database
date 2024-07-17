using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace aPROJECT
{
    public partial class Form1 : Form
    {
        DataTable table = new DataTable("table");
        int index;

        private string connectionString = "Data Source=LAPTOP-D804ODKC;Initial Catalog=LibraryDb2;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }
        private void RefreshDataGrid()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Bookss", con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.bookssTableAdapter.Fill(this.LibraryDb2DataSet1.bookss);
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            string title = txttitle.Text; // Assuming there's a TextBox named txtaitle
            string author = txtauthor.Text; // Assuming there's a TextBox named txtauthor
            string genre = txtgenre.Text;
            int YearPublished;
            if (!int.TryParse(txtyearpublished.Text, out YearPublished))
            {
                MessageBox.Show("Please enter a valid year.");
                return;
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Bookss (TITLE, AUTHOR,GENRE,YearPublished) VALUES (@TITLE, @AUTHOR,@GENRE,@YearPublished)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TITLE", title);
                    cmd.Parameters.AddWithValue("@AUTHOR", author);
                    cmd.Parameters.AddWithValue("@GENRE", genre);
                    cmd.Parameters.AddWithValue("@YearPublished", YearPublished);
                    cmd.ExecuteNonQuery();
                    RefreshDataGrid();
                }
            }
        }
            

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the ID from the selected row
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["iDDataGridViewTextBoxColumn"].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Bookss SET Title=@Title, Author=@Author, Genre=@Genre, YearPublished=@YearPublished WHERE ID=@iDDataGridViewTextBoxColumn";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Title", txttitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Author", txtauthor.Text.Trim());
                        cmd.Parameters.AddWithValue("@Genre", txtgenre.Text.Trim());
                        cmd.Parameters.AddWithValue("@YearPublished", txtyearpublished.Text.Trim());
                        cmd.Parameters.AddWithValue("@iDDataGridViewTextBoxColumn", id); // Set the ID parameter
                        
                        try
                        {
                            cmd.ExecuteNonQuery();
                            RefreshDataGrid();
                            MessageBox.Show("Record updated successfully!");
                           
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating record: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }
    
        private void btndelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["iDDataGridViewTextBoxColumn"].Value); // Assuming "ID" is the column name for your primary key

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string deleteQuery = "DELETE FROM Bookss WHERE ID = @iDDataGridViewTextBoxColumn";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@iDDataGridViewTextBoxColumn", id);
                       
                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record deleted successfully!");
                            RefreshDataGrid();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting record: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }


        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txttitle.Text = string.Empty;
            txtauthor.Text = string.Empty;
            txtgenre.Text = string.Empty;
            txtyearpublished.Text = string.Empty;

            // Optionally, you can clear DataGridView selection
            dataGridView1.ClearSelection();
        }
    }
}


