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
    public partial class ReportGeneralLedger : Form
    {
        connect con = new connect();
        public ReportGeneralLedger()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string from = fromdate.Value.ToString("yyyy-MM-dd");
            string to = todate.Value.ToString("yyyy-MM-dd");

            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            SqlConnection conn = new SqlConnection(con.str);
            var select = "";
            if (all.Checked)
            {
                select = @"Select glID as [ID], [Date] as [Date], g.Status as Status,
                            c.CashbookName as Cashbook, a.Nature as Nature, g.Amount as Amount, g.Detail as Detail
                            from GeneralLedger g, Cashbook c, Account a
                            Where g.C_ID=c.CashbookID and a.AccountID=g.AccountID
                            Order By [Date]";

            }
            else
            {
                select = @"Select glID as [ID], [Date] as [Date], g.Status as Status, 
                        c.CashbookName as Cashbook, a.Nature as Nature, g.Amount as Amount, g.Detail as Detail
                        from GeneralLedger g, Cashbook c, Account a
                        Where g.C_ID=c.CashbookID and a.AccountID=g.AccountID
                        and Date between '" + from + "' and '" + to + "' Order By[Date]";
            }

            conn.Open();
            var dataAdapter = new SqlDataAdapter(select, conn);
            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
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

        private void ReportGeneralLedger_Load(object sender, EventArgs e)
        {

        }
    }
}
