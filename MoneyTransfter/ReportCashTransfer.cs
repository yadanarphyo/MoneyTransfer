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
    public partial class ReportCashTransfer : Form
    {
        connect con = new connect();
        public ReportCashTransfer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con.str);
            try
            {
                string from = fromdate.Value.ToString("yyyy-MM-dd");
                string to = todate.Value.ToString("yyyy-MM-dd");
                string fcb = fromcb.SelectedValue.ToString();
                string tcb = tocb.SelectedValue.ToString();


                this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.dataGridView1.MultiSelect = false;
                var select = "";
                if (alldate.Checked == true && allcb.Checked == true && sttcheck.Checked == true)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                            from Transfer t Order By t.[Date]";
                }

                else if (alldate.Checked == true && allcb.Checked == true && sttcheck.Checked == false)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                            from Transfer t where t.Status='" + status.SelectedItem + "'  Order By t.[Date]";
                }

                else if (alldate.Checked == true && allcb.Checked == false && sttcheck.Checked == true)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                            from Transfer t where t.From_CID='" + fcb + "' and t.To_CID='" + tcb + "'  Order By t.[Date]";
                }

                else if (alldate.Checked == false && allcb.Checked == true && sttcheck.Checked == true)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                         from Transfer t Where Date between '" + from + "' and '" + to + "' Order By t.[Date]";
                }

                else if (alldate.Checked == false && allcb.Checked == false && sttcheck.Checked == true)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                         from Transfer t Where Date between '" + from + "' and '" + to + "' and t.From_CID='" + fcb + "' and t.To_CID='" + tcb + "' Order By t.[Date]";
                }

                else if (alldate.Checked == false && allcb.Checked == true && sttcheck.Checked == false)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                         from Transfer t Where Date between '" + from + "' and '" + to + "' and t.Status='" + status.SelectedItem + "' Order By t.[Date]";
                }

                else if (alldate.Checked == true && allcb.Checked == false && sttcheck.Checked == false)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                         from Transfer t Where t.From_CID='" + fcb + "' and t.To_CID='" + tcb + "' and t.Status='" + status.SelectedItem + "' Order By t.[Date]";
                }

                else if (alldate.Checked == false && allcb.Checked == false && sttcheck.Checked == false)
                {
                    select = @"Select t.Trans_ID as [ID], t.[Date] as [Date], t.FromCBName as [From], t.ToCBName as [To],t.Status as [Status], t.Amount as Amount 
                         from Transfer t Where Date between '" + from + "' and '" + to + "' and t.From_CID='" + fcb + "' and t.To_CID='" + tcb + "' and t.Status='" + status.SelectedItem + "' Order By t.[Date]";
                }

                else
                {
                    MessageBox.Show("There is no data. Please choose properly!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                conn.Open();
                var dataAdapter = new SqlDataAdapter(select, conn);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                var ds = new DataSet();
                dataAdapter.Fill(ds);
                dataGridView1.ReadOnly = true;
                dataGridView1.DataSource = ds.Tables[0];
                
                from = "";
                to = "";
                select = "";

                int sum = 0;
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    sum = sum + int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                }
                total.Text = sum.ToString();
            }
            catch
            {
                MessageBox.Show("Warning", "Something is wrong!", MessageBoxButtons.OK);
            }

            finally
            {
                conn.Close();
            }
        }

        private void all_CheckedChanged(object sender, EventArgs e)
        {
            if (alldate.Checked)
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

        private void ReportCashTransfer_Load(object sender, EventArgs e)
        {
            status.Items.Add("Saved");
            status.Items.Add("Deleted");
            status.SelectedIndex = 0;

            SqlConnection conn2 = new SqlConnection(con.str);
            conn2.Open();
            var select = @"Select CashbookID, CashbookName from Cashbook";
            var dataAdapter2 = new SqlDataAdapter(select, conn2);
            var commandBuilder = new SqlCommandBuilder(dataAdapter2);
            var ds = new DataSet();
            dataAdapter2.Fill(ds);
            fromcb.DisplayMember = "CashbookName";
            fromcb.ValueMember = "CashbookID";
            fromcb.DataSource = ds.Tables[0];

            var dataAdapter1 = new SqlDataAdapter(select, conn2);
            var commandBuilder1 = new SqlCommandBuilder(dataAdapter1);
            var ds1 = new DataSet();
            dataAdapter1.Fill(ds1);
            tocb.DisplayMember = "CashbookName";
            tocb.ValueMember = "CashbookID";
            tocb.DataSource = ds1.Tables[0];
            conn2.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
