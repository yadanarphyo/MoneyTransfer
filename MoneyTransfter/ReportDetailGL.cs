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
    public partial class ReportDetailGL : Form
    {
        connect con = new connect();
        public ReportDetailGL()
        {
            InitializeComponent();
        }

        private void ReportDetailGL_Load(object sender, EventArgs e)
        {
            statuscb.Items.Add("Saved");
            statuscb.Items.Add("Deleted");
            statuscb.SelectedIndex = 0;

            SqlConnection conn2 = new SqlConnection(con.str);
            var select = @"Select AccountID, AccountName from Account";
            conn2.Open();
            var dataAdapter2 = new SqlDataAdapter(select, conn2);
            var commandBuilder = new SqlCommandBuilder(dataAdapter2);
            var ds = new DataSet();
            dataAdapter2.Fill(ds);
            account.DisplayMember = "AccountName";
            account.ValueMember = "AccountID";
            account.DataSource = ds.Tables[0];
        }

        private void all_CheckedChanged(object sender, EventArgs e)
        {
            if (all.Checked)
            {
                fromdate.Enabled = false;
                todate.Enabled = false;
            }
            else
            {
                fromdate.Enabled = true;
                todate.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cbid = account.SelectedValue.ToString();
            string from = fromdate.Value.ToString("yyyy-MM-dd");
            string to = todate.Value.ToString("yyyy-MM-dd");

            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            SqlConnection conn = new SqlConnection(con.str);
            var select = "";
            try
            {
                if (all.Checked)
                {
                    select = @"Select r.ID as ID, g.Date as [Date], g.Status as Status, r.Status As [Description],g.Amount as Amount, c.CashbookName as Cashbook, g.Detail as Detail
                                from Record r, GeneralLedger g, Cashbook c where g.C_ID=c.CashbookID and r.ID=g.glID and g.AccountID='" + cbid + "' and g.status='" + statuscb.SelectedItem.ToString()+"' order by g.Date";

                }
                else
                {
                    select = @"Select r.ID as ID, g.Date as [Date], g.Status as Status, r.Status As [Description],g.Amount as Amount, c.CashbookName as Cashbook, g.Detail as Detail
                                from Record r, GeneralLedger g, Cashbook c where g.C_ID=c.CashbookID and r.ID=g.glID and g.AccountID='" + cbid + "' and g.Date between '" + from + "' and '" + to + "' and g.status='" + statuscb.SelectedItem.ToString() + "' order by g.Date";
                }

                conn.Open();
                var dataAdapter = new SqlDataAdapter(select, conn);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet dt = new DataSet();
                dataAdapter.Fill(dt);
                dataGridView1.ReadOnly = true;
                dataGridView1.DataSource = dt.Tables[0];

                int sum = 0;
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    sum = sum + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                }

                total.Text = sum.ToString();
            }
            catch
            {
                MessageBox.Show("Error!");
            }


            conn.Close();
            from = "";
            to = "";
            select = "";
        }
    }
}
