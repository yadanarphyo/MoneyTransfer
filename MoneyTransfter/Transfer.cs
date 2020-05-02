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
    public partial class Transfer : Form
    {
        connect con = new connect();
        public string str;
        public String cid;
        public String prefix;
        public int num1;


        public Transfer()
        {
            InitializeComponent();
        }

        private void Transfer_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today.Date;
            amount.Text = "0";
            btnincrement_click();

            SqlConnection conn2 = new SqlConnection(con.str);
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

        private void btnok_Click(object sender, EventArgs e)
        {
            int Number = 0;
            SqlConnection conn = new SqlConnection(con.str);
            conn.Open();
            string query = @"INSERT INTO Transfer(Trans_ID,Date,From_CID,To_CID,Amount,Status,Remark,FromCBName,ToCBName) VALUES(@Trans_ID,@Date,@From_CID,@To_CID,@Amount,@Status,@Remark,@FromCBName,@ToCBName)";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                if (from.SelectedIndex == to.SelectedIndex)
                {
                    MessageBox.Show("'From Cashbook' and 'To Cashbook' cannot be same!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (!int.TryParse(amount.Text, out Number))
                {
                    MessageBox.Show("Amount must be numbers.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (amount.Text == "0")
                {
                    MessageBox.Show("Amount cannot be zero.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (Convert.ToInt32(amount.Text) < 0)
                {
                    MessageBox.Show("Amount cannot be less than zero.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {
                    cmd.Parameters.AddWithValue("@Trans_ID", id.Text);
                    cmd.Parameters.AddWithValue("@From_CID", from.SelectedValue);
                    cmd.Parameters.AddWithValue("@To_CID", to.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@Amount", amount.Text);
                    cmd.Parameters.AddWithValue("@Status", "Saved");
                    cmd.Parameters.AddWithValue("@Remark", remark.Text);
                    cmd.Parameters.AddWithValue("@FromCBName", from.Text);
                    cmd.Parameters.AddWithValue("@ToCBName", to.Text);
                    cmd.ExecuteNonQuery();

                    //Update Remaining amount two cashbooks
                    SqlCommand command = new SqlCommand("SELECT RemainingAmount FROM Cashbook WHERE CashbookID=@from", conn);
                    command.Parameters.AddWithValue("@from", from.SelectedValue);
                    int ramount1 = (Int32)command.ExecuteScalar();

                    string query3 = @"UPDATE Cashbook SET RemainingAmount =@remain1  WHERE CashbookID=@from";
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    int tosubstractamount = Convert.ToInt32(amount.Text);
                    int finalamount1 = ramount1 - tosubstractamount;
                    cmd3.Parameters.AddWithValue("@remain1", finalamount1);
                    cmd3.Parameters.AddWithValue("@from", from.SelectedValue);
                    cmd3.ExecuteNonQuery();


                    SqlCommand command1 = new SqlCommand("SELECT RemainingAmount FROM Cashbook WHERE CashbookID=@to", conn);
                    command1.Parameters.AddWithValue("@to", to.SelectedValue);
                    int ramount2 = (Int32)command1.ExecuteScalar();

                    string query4 = @"UPDATE Cashbook SET RemainingAmount =@remain2  WHERE CashbookID=@to";
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    int tosumamount = Convert.ToInt32(amount.Text);
                    int finalamount2 = ramount2 + tosumamount;
                    cmd4.Parameters.AddWithValue("@remain2", finalamount2);
                    cmd4.Parameters.AddWithValue("@to", to.SelectedValue);
                    cmd4.ExecuteNonQuery();

                    //Insert into record table For 'From Cashbook'
                    string query1 = @"INSERT INTO Record(ID,C_ID,amount,status,Remain,Date) VALUES(@ID,@CID,@amount,@status,@Remain,@Date)";
                    SqlCommand cmd1 = new SqlCommand(query1, conn);

                    cmd1.Parameters.AddWithValue("@ID", id.Text);
                    cmd1.Parameters.AddWithValue("@CID", from.SelectedValue);
                    cmd1.Parameters.AddWithValue("@amount", "-" + amount.Text);
                    cmd1.Parameters.AddWithValue("@status", "Transfer");
                    cmd1.Parameters.AddWithValue("@Remain", finalamount1);
                    cmd1.Parameters.AddWithValue("@Date", dateTimePicker1.Value.Date);
                    cmd1.ExecuteNonQuery();


                    //Insert into record table For 'To Cashbook'
                    string query2 = @"INSERT INTO Record(ID,C_ID,amount,status,Remain,Date) VALUES(@ID,@CID,@amount,@status,@Remain,@Date)";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);

                    cmd2.Parameters.AddWithValue("@ID", id.Text);
                    cmd2.Parameters.AddWithValue("@CID", to.SelectedValue);
                    cmd2.Parameters.AddWithValue("@amount", amount.Text);
                    cmd2.Parameters.AddWithValue("@status", "Receive");
                    cmd2.Parameters.AddWithValue("@Remain", finalamount2);
                    cmd2.Parameters.AddWithValue("@Date", dateTimePicker1.Value.Date);
                    cmd2.ExecuteNonQuery();


                    MessageBox.Show("Register Successfully!");
                    dateTimePicker1.Value = DateTime.Today.Date;
                    amount.Text = "0";
                    remark.Clear();
                    ramount1 = 0;
                    tosubstractamount = 0;
                    finalamount1 = 0;
                    ramount2 = 0;
                    tosumamount = 0;
                    finalamount2 = 0;
                    btnincrement_click();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Error");
            }
            finally
            {
                conn.Close();
            }


        }

        private void btnincrement_click()
        {
            SqlConnection conn = new SqlConnection(con.str);
            conn.Open();
            string sql_select = @"Select Max(Trans_ID) from Transfer";
            SqlCommand cmd_id = new SqlCommand(sql_select, conn);
            SqlDataReader dr = cmd_id.ExecuteReader();
            prefix = "T";
            while (dr.Read())
            {
                cid = dr[0].ToString();
            }

            if (cid == "")
            {
                str = prefix + "00001";
                id.Text = str;
            }
            else
            {
                num1 = Convert.ToInt32(cid.Substring(1, 5));

                if (num1 < 9)
                {
                    num1 += 1;
                    str = prefix + "0000" + num1;
                    id.Text = str;
                }

                else if (num1 >= 9 && num1 < 99)
                {
                    num1 += 1;
                    str = prefix + "000" + num1;
                    id.Text = str;
                }

                else if (num1 >= 99 && num1 < 999)
                {
                    num1 += 1;
                    str = prefix + "00" + num1;
                    id.Text = str;
                }

                else if (num1 >= 999 && num1 < 9999)
                {
                    num1 += 1;
                    str = prefix + "0" + num1;
                    id.Text = str;
                }

                else if (num1 >= 9999 && num1 < 99999)
                {
                    num1 += 1;
                    str = prefix + num1;
                    id.Text = str;
                }
                else
                {
                    MessageBox.Show("Your Transaction ID is full! Please contact IT Service.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

            conn.Close();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
