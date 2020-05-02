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
    public partial class CashbookRegistration : Form
    {
        connect con = new connect();
        public string str;
        public String cid;
        public String prefix;
        public int num1;

        public CashbookRegistration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Number = 0;

            SqlConnection conn = new SqlConnection(con.str);
            conn.Open();
            string query = @"INSERT INTO Cashbook(CashbookID,CashbookName,OpeningAmount,OpeningDate,RemainingAmount) VALUES(@CashbookID,@CashbookName,@OpeningAmount,@OpeningDate,@RemainingAmount)";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                if (name.Text == "")
                {
                    MessageBox.Show("Please Enter Cashbook Name.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (openingamount.Text=="")
                {
                    MessageBox.Show("Please Enter Opening Amount.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if(!int.TryParse(openingamount.Text, out Number))
                {
                    MessageBox.Show("Amount must be numbers.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (Convert.ToInt32(openingamount.Text)<0)
                {
                    MessageBox.Show("Amount cannot be smaller than zero.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {
                    cmd.Parameters.AddWithValue("@CashbookID", id.Text);
                    cmd.Parameters.AddWithValue("@CashbookName", name.Text);
                    cmd.Parameters.AddWithValue("@OpeningAmount", openingamount.Text);
                    cmd.Parameters.AddWithValue("@OpeningDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@RemainingAmount", openingamount.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Register Successfully!");
                    name.Text = "";
                    openingamount.Text = "0";
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
            string sql_select = @"Select Max(CashbookID) from Cashbook";
            SqlCommand cmd_id = new SqlCommand(sql_select, conn);
            SqlDataReader dr = cmd_id.ExecuteReader();
            prefix = "CB";
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
                    MessageBox.Show("Your Cashbook ID is full! Please contact IT Service.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

            conn.Close();
        }

        private void CashbookRegistration_Load(object sender, EventArgs e)
        {
            openingamount.Text = "0";
            btnincrement_click();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
