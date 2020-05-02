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

namespace MoneyTransfter
{
    public partial class editGL : Form
    {
        connect con = new connect();
        public editGL()
        {
            InitializeComponent();
        }

        private void id_KeyDown(object sender, KeyEventArgs e)
        {
            string gid = "";
            gid = id.Text;
            if (e.KeyCode == Keys.Enter)
            {
                SqlConnection conn = new SqlConnection(con.str);
                
                conn.Open();
                string query = @"Select [Date],AccountID,Amount,C_ID from GeneralLedger where glID=@gid and Status=@status";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@gid", gid);
                cmd.Parameters.AddWithValue("@status", "Saved");
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    id.Enabled = false;
                    while (dr.Read())
                    {
                        date.Text = dr["Date"].ToString();
                        name.SelectedValue = dr["AccountID"].ToString();
                        amount.Text = dr["Amount"].ToString();
                        cashbook.SelectedValue = dr["C_ID"].ToString();
                    }
                    dr.Close();
                    
                }
                else
                {
                    MessageBox.Show("This ID is not exist!","Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                conn.Close();
            }
        }

        private void editGL_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con.str);
            id.Enabled = true;
            var select = @"Select CashbookID, CashbookName from Cashbook";
            conn.Open();
            var dataAdapter2 = new SqlDataAdapter(select, conn);
            var commandBuilder = new SqlCommandBuilder(dataAdapter2);
            var ds = new DataSet();
            dataAdapter2.Fill(ds);
            cashbook.DisplayMember = "CashbookName";
            cashbook.ValueMember = "CashbookID";
            cashbook.DataSource = ds.Tables[0];

            var select1 = @"Select AccountID, AccountName from Account";
            var dataAdapter3 = new SqlDataAdapter(select1, conn);
            var commandBuilder1 = new SqlCommandBuilder(dataAdapter3);
            var ds1 = new DataSet();
            dataAdapter3.Fill(ds1);
            name.DisplayMember = "AccountName";
            name.ValueMember = "AccountID";
            name.DataSource = ds1.Tables[0];
        }

        private void save_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con.str);
            try
            {
                DialogResult drs= MessageBox.Show("Are you sure want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (drs == DialogResult.Yes)
                {
                    conn.Open();
                    SqlCommand command1 = new SqlCommand("SELECT Nature FROM Account WHERE AccountID=@accountid", conn);
                    command1.Parameters.AddWithValue("@accountid", name.SelectedValue);
                    string nature = Convert.ToString(command1.ExecuteScalar());

                    SqlCommand command = new SqlCommand("SELECT RemainingAmount FROM Cashbook WHERE CashbookID=@from", conn);
                    command.Parameters.AddWithValue("@from", cashbook.SelectedValue);
                    int remain = Convert.ToInt32(command.ExecuteScalar());
                    int deleteamount = Convert.ToInt32(amount.Text);
                    int result = 0;
                    if (nature == "Income")
                    {
                        result = remain - deleteamount;
                    }
                    else
                    {
                        result = remain + deleteamount;
                    }

                    string query3 = @"UPDATE Cashbook SET RemainingAmount=@remain1 WHERE CashbookID=@from";
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    cmd3.Parameters.AddWithValue("@remain1", result);
                    cmd3.Parameters.AddWithValue("@from", cashbook.SelectedValue);
                    cmd3.ExecuteNonQuery();

                    string query2 = @"UPDATE GeneralLedger SET Status=@status WHERE glID=@glid";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@status", "Deleted");
                    cmd2.Parameters.AddWithValue("@glid", id.Text);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    

                    //string query5 = @"Delete Record WHERE ID=@id";
                    //SqlCommand cmd5 = new SqlCommand(query5, conn);
                    //cmd5.Parameters.AddWithValue("@id", id.Text);
                    //cmd5.ExecuteNonQuery();

                    //string query4 = @"Delete GeneralLedger WHERE glID=@id";
                    //SqlCommand cmd4 = new SqlCommand(query4, conn);
                    //cmd4.Parameters.AddWithValue("@id", id.Text);
                    //cmd4.ExecuteNonQuery();


                }

            }
            catch (Exception)
            {
                MessageBox.Show("Something is wrong!", "Error Warning");
            }
            finally
            {
                conn.Close();
                id.Clear();
                id.Enabled = true;
                date.Value = DateTime.Today.Date;
                name.SelectedIndex = 0;
                cashbook.SelectedIndex = 0;
                amount.Clear();
                remark.Clear();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            id.Enabled = true;
            amount.Clear();
            remark.Clear();
            name.SelectedIndex = 0;
            cashbook.SelectedIndex = 0;
        }

        private void id_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
 