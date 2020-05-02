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
    public partial class GeneralLedger : Form
    {

        connect con = new connect();
        public string str;
        public String cid;
        public String prefix;
        public int num1;
        public string nature;
        public string status;
        public int finalamount2;
        public GeneralLedger()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void save_Click(object sender, EventArgs e)
        {
            
            int Number = 0;
            SqlConnection conn = new SqlConnection(con.str);
            conn.Open();
            string query = @"INSERT INTO GeneralLedger(AccountID,Amount,C_ID,glID,Date,Status,Detail) VALUES(@AccountID,@Amount,@C_ID,@glID,@Date,@status,@detail)";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                if (!int.TryParse(amount.Text, out Number))
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
                    cmd.Parameters.AddWithValue("@AccountID", name.SelectedValue);
                    cmd.Parameters.AddWithValue("@Amount", amount.Text);
                    cmd.Parameters.AddWithValue("@C_ID", cashbook.SelectedValue);
                    cmd.Parameters.AddWithValue("@glID", id.Text);
                    cmd.Parameters.AddWithValue("@Date", date.Value.Date);
                    cmd.Parameters.AddWithValue("@status","Saved");
                    cmd.Parameters.AddWithValue("@detail", detail.Text);
                    cmd.ExecuteNonQuery();


                    //update remaining amount
                    SqlCommand command1 = new SqlCommand("SELECT Nature FROM Account WHERE AccountID=@accountid", conn);
                    command1.Parameters.AddWithValue("@accountid", name.SelectedValue);
                    SqlDataReader objDataReader = command1.ExecuteReader();


                    while (objDataReader.Read())
                    {
                        nature = objDataReader.GetString(0);
                    }

                    string status = "";
                    if (nature == "Income")
                    {
                        status = "+";
                    }
                    else if (nature == "Expense")
                    {
                        status = "-";
                    }
                    objDataReader.Close();

                    string final = "";
                    SqlCommand command2 = new SqlCommand("SELECT RemainingAmount FROM Cashbook WHERE CashbookID=@cashbookid", conn);
                    command2.Parameters.AddWithValue("@cashbookid", cashbook.SelectedValue);
                    int ramount2 = (Int32)command2.ExecuteScalar();
                    int inputamount = Convert.ToInt32(amount.Text);
                    if (status == "+")
                    {
                        finalamount2 = ramount2 + inputamount;
                        final = amount.Text;
                        
                    }
                    else if (status == "-")
                    {
                        finalamount2 = ramount2 - inputamount;
                        final = "-" + amount.Text;
                    }

                    string query4 = @"UPDATE Cashbook SET RemainingAmount =@remain2  WHERE CashbookID=@cashbookid";
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                                        
                    
                    cmd4.Parameters.AddWithValue("@remain2", finalamount2);
                    cmd4.Parameters.AddWithValue("@cashbookid", cashbook.SelectedValue);
                    cmd4.ExecuteNonQuery();


                    //Insert into record table                    
                                        
                    string query1 = @"INSERT INTO Record(ID,C_ID,amount,status,Remain,Date) VALUES(@ID,@CID,@amount,@status,@Remain,@Date)";
                    SqlCommand cmd1 = new SqlCommand(query1, conn);

                    cmd1.Parameters.AddWithValue("@ID", id.Text);
                    cmd1.Parameters.AddWithValue("@CID", cashbook.SelectedValue);
                    cmd1.Parameters.AddWithValue("@amount",final);
                    cmd1.Parameters.AddWithValue("@status", nature);
                    cmd1.Parameters.AddWithValue("@Remain", finalamount2);
                    cmd1.Parameters.AddWithValue("@Date", date.Value.Date);
                    cmd1.ExecuteNonQuery();

                    MessageBox.Show("Register Successfully!");
                    amount.Text = "0";
                    detail.Text = "";
                    name.SelectedIndex = 0;
                    cashbook.SelectedIndex = 0;
                    nature = "";
                    status = "";
                    finalamount2 = 0;
                    final = "";
                    btnincrement_click();
                }
            }
            //catch (SqlException)
            //{
            //    MessageBox.Show("Error");
            //}
            finally
            {
                conn.Close();
            }
        }

        private void btnincrement_click()
        {
            SqlConnection conn = new SqlConnection(con.str);
            conn.Open();
            string sql_select = @"Select Max(glID) from GeneralLedger";
            SqlCommand cmd_id = new SqlCommand(sql_select, conn);
            SqlDataReader dr = cmd_id.ExecuteReader();
            prefix = "GL";
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
                num1 = Convert.ToInt32(cid.Substring(2, 5));

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
                    MessageBox.Show("Your ID is full! Please contact IT Service.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

            conn.Close();
        }

        private void GeneralLedger_Load(object sender, EventArgs e)
        {
            amount.Text = "0";
            btnincrement_click();

            SqlConnection conn2 = new SqlConnection(con.str);
            conn2.Open();
            var select = @"Select CashbookID, CashbookName from Cashbook";            
            var dataAdapter2 = new SqlDataAdapter(select, conn2);
            var commandBuilder = new SqlCommandBuilder(dataAdapter2);
            var ds = new DataSet();
            dataAdapter2.Fill(ds);
            cashbook.DisplayMember = "CashbookName";
            cashbook.ValueMember = "CashbookID";
            cashbook.DataSource = ds.Tables[0];
            conn2.Close();

            var select3 = @"Select AccountID, AccountName from Account";
            conn2.Open();
            var dataAdapter3 = new SqlDataAdapter(select3, conn2);
            var commandBuilder3 = new SqlCommandBuilder(dataAdapter3);
            var ds3 = new DataSet();
            dataAdapter3.Fill(ds3);
            name.DisplayMember = "AccountName";
            name.ValueMember = "AccountID";
            name.DataSource = ds3.Tables[0];
            conn2.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void date_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
