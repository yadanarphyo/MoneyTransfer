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
    public partial class AccountRegistration : Form
    {
        connect con = new connect();
        public string str;
        public String cid;
        public String prefix;
        public int num1;

        public AccountRegistration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con.str);
            conn.Open();
            string query = @"INSERT INTO Account(AccountID,AccountName,Nature,Remark) VALUES(@AccountID,@AccountName,@Nature,@Remark)";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                if (name.Text == "")
                {
                    MessageBox.Show("Please Enter Account Name.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (nature.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select account nature.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }           

                else
                {
                    cmd.Parameters.AddWithValue("@AccountID", id.Text);
                    cmd.Parameters.AddWithValue("@AccountName", name.Text);
                    cmd.Parameters.AddWithValue("@Nature", nature.SelectedItem);
                    cmd.Parameters.AddWithValue("@Remark", remark.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Register Successfully!");
                    name.Text = "";
                    nature.SelectedIndex = 0;
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
            string sql_select = @"Select Max(AccountID) from Account";
            SqlCommand cmd_id = new SqlCommand(sql_select, conn);
            SqlDataReader dr = cmd_id.ExecuteReader();
            prefix = "A";
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
                    MessageBox.Show("Your Acccount ID is full! Please contact IT Service.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

            conn.Close();
        }

        private void GeneralLedger_Load(object sender, EventArgs e)
        {
            btnincrement_click();
            
            nature.SelectedItem = nature.Items.Add("---Select---");
            nature.Items.Add("Income");
            nature.Items.Add("Expense");
            nature.SelectedIndex = 0;
        }

        private void nature_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
