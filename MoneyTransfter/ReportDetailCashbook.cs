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
    public partial class ReportDetailCashbook : Form
    {
        connect con = new connect();
        public ReportDetailCashbook()
        {
            InitializeComponent();
        }

        private void ReportDetailCashbook_Load(object sender, EventArgs e)
        {
            statuscb.Items.Add("Saved");
            statuscb.Items.Add("Deleted");
            statuscb.SelectedIndex = 0;

            SqlConnection conn2 = new SqlConnection(con.str);
            var select = @"Select CashbookID, CashbookName from Cashbook";
            conn2.Open();
            var dataAdapter2 = new SqlDataAdapter(select, conn2);
            var commandBuilder = new SqlCommandBuilder(dataAdapter2);
            var ds = new DataSet();
            dataAdapter2.Fill(ds);
            cashbook.DisplayMember = "CashbookName";
            cashbook.ValueMember = "CashbookID";
            cashbook.DataSource = ds.Tables[0];
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cbid = cashbook.SelectedValue.ToString();
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

                    select = @"Select r.ID as ID, t.Date as [Date], 'Cash Transfer' As [Transaction Name], r.Status As [Description],r.amount as Amount, t.Status as Status
                             from Record r, Transfer t where r.ID=t.Trans_ID and  r.C_ID='" + cbid + "' and  t.Status='" + statuscb.SelectedItem.ToString() +
                            "' UNION Select r.ID as ID, g.Date as [Date], 'General Ledger' As [Transaction Name], r.Status As [Description],r.amount as Amount, g.Status as Status from Record r, GeneralLedger g where r.ID=g.glID and  r.C_ID='" + cbid +"' and g.status = '"+statuscb.SelectedItem.ToString()+"'";

                }
                else
                {

                    select = @"Select r.ID as ID, t.Date as [Date], 'Cash Transfer' As [Transaction Name], r.Status As [Description],r.amount as Amount, t.Status as Status
                                from Record r, Transfer t where r.ID=t.Trans_ID and  r.C_ID='" + cbid + "' and r.Date between '"+ from + "' and '" + to + "' and t.status='"+statuscb.SelectedItem.ToString()+"' UNION Select r.ID as ID, g.Date as [Date], 'General Ledger' As [Transaction Name], r.Status As [Description],r.amount as Amount, g.Status as Status from Record r, GeneralLedger g where r.ID=g.glID and r.C_ID= '" + cbid + "' and r.Date between '"+from+"' and '"+to+"' and g.status='"+statuscb.SelectedItem.ToString()+"'";
                }
                conn.Open();


                string sql_select = @"Select [RemainingAmount] from Cashbook where CashbookID='" + cbid + "'";
                SqlCommand cmd_id = new SqlCommand(sql_select, conn);
                SqlDataReader dr = cmd_id.ExecuteReader();
                dr.Read();
                remain.Text = dr[0].ToString();
                dr.Close();

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

            finally
            {
                conn.Close();
            }
            from = "";
            to = "";
            select = "";
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

        private void statuscb_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }

        private void total_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
