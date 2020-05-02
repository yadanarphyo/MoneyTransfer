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
    public partial class EditTransfer : Form
    {
        connect con = new connect();
        public EditTransfer()
        {
            InitializeComponent();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            id.Clear();
            id.Enabled = true;            
            from.SelectedIndex = 0;
            to.SelectedIndex = 0;
            amount.Clear();
            remark.Clear();

        }

        private void id_KeyDown(object sender, KeyEventArgs e)
        {
            string tid = "";
            tid = id.Text;
            if (e.KeyCode == Keys.Enter)
            {
                SqlConnection conn = new SqlConnection(con.str);
                conn.Open();
                string query = @"Select [Date],From_CID,To_CID,Amount,Remark from Transfer where Trans_ID=@tid and Status=@status";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tid", tid);
                cmd.Parameters.AddWithValue("@status", "Saved");
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    id.Enabled = false;
                    while (dr.Read())
                    {
                        dateTimePicker1.Text = dr["Date"].ToString();
                        from.SelectedValue = dr["From_CID"].ToString();
                        to.SelectedValue = dr["To_CID"].ToString();
                        amount.Text = dr["Amount"].ToString();
                        remark.Text = dr["Remark"].ToString();
                    }
                    dr.Close();                    
                }
                else
                {
                    MessageBox.Show("This ID is not exist!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                conn.Close();
            }
        }

        private void EditTransfer_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today.Date;
            SqlConnection conn2 = new SqlConnection(con.str);
            id.Enabled = true;
            var select = @"Select CashbookID, CashbookName from Cashbook";
            conn2.Open();
            var dataAdapter2 = new SqlDataAdapter(select, conn2);
            var commandBuilder = new SqlCommandBuilder(dataAdapter2);
            var ds = new DataSet();
            dataAdapter2.Fill(ds);
            from.DisplayMember = "CashbookName";
            from.ValueMember = "CashbookID";
            from.DataSource = ds.Tables[0];

            var ds1 = new DataSet();
            dataAdapter2.Fill(ds1);
            to.DisplayMember = "CashbookName";
            to.ValueMember = "CashbookID";
            to.DataSource = ds1.Tables[0];
            conn2.Close();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            DialogResult dr= MessageBox.Show("Are you sure want to delete?", "Delete?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(con.str);
                try
                {
                    conn.Open();

                    //Update Remaining amount two cashbooks
                    SqlCommand command1 = new SqlCommand("SELECT Amount FROM [Transfer] WHERE Trans_ID=@id", conn);
                    command1.Parameters.AddWithValue("@id", id.Text);
                    int transferamount = Convert.ToInt32(command1.ExecuteScalar());

                    SqlCommand command = new SqlCommand("SELECT RemainingAmount FROM Cashbook WHERE CashbookID=@from", conn);
                    command.Parameters.AddWithValue("@from", from.SelectedValue);
                    int fromremain = Convert.ToInt32(command.ExecuteScalar());

                    string query3 = @"UPDATE Cashbook SET RemainingAmount =@remain1  WHERE CashbookID=@from";
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    int tosubstractamount = Convert.ToInt32(amount.Text);
                    int finalamount1 = fromremain + transferamount;
                    cmd3.Parameters.AddWithValue("@remain1", finalamount1);
                    cmd3.Parameters.AddWithValue("@from", from.SelectedValue);
                    cmd3.ExecuteNonQuery();


                    SqlCommand command2 = new SqlCommand("SELECT RemainingAmount FROM Cashbook WHERE CashbookID=@to", conn);
                    command2.Parameters.AddWithValue("@to", to.SelectedValue);
                    int toremain = Convert.ToInt32((Int32)command2.ExecuteScalar());

                    string query4 = @"UPDATE Cashbook SET RemainingAmount =@remain2  WHERE CashbookID=@to";
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    int tosumamount = Convert.ToInt32(amount.Text);
                    int finalamount2 = toremain - transferamount;
                    cmd4.Parameters.AddWithValue("@remain2", finalamount2);
                    cmd4.Parameters.AddWithValue("@to", to.SelectedValue);
                    cmd4.ExecuteNonQuery();

                    string query2 = @"UPDATE Transfer SET Status=@status WHERE Trans_ID=@id";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@status", "Deleted");
                    cmd2.Parameters.AddWithValue("@id", id.Text);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Delete transaction from Transfer and Record Table
                    //string deletetransfer = @"Delete from Transfer where Trans_ID=@id";
                    //SqlCommand cmd = new SqlCommand(deletetransfer, conn);
                    //cmd.Parameters.AddWithValue("@id", id.Text);
                    //cmd.ExecuteNonQuery();

                    //string deleterecord = @"Delete from Record where ID=@id";
                    //SqlCommand cmd1 = new SqlCommand(deleterecord, conn);
                    //cmd1.Parameters.AddWithValue("@id", id.Text);
                    //cmd1.ExecuteNonQuery();
                    

                }
                catch (Exception)
                {
                    MessageBox.Show("Something's wrong!", "Warning", MessageBoxButtons.OK);
                }
                finally
                {
                    conn.Close();
                    id.Clear();
                    id.Enabled = true;
                    dateTimePicker1.Value = DateTime.Today.Date;
                    amount.Clear();
                    remark.Clear();
                    from.SelectedIndex = 0;
                    to.SelectedIndex = 0;
                }
            }
            else
            {
                this.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Transfer t = new Transfer();
            t.Show();
            this.Close();
        }

        private void id_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
