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
    public partial class ReportCashbookList : Form
    {
        connect con = new connect();
        public ReportCashbookList()
        {
            InitializeComponent();
        }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private object AutoNumberedTable(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        private void ReportCashbookList_Load(object sender, EventArgs e)
        {
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            SqlConnection conn = new SqlConnection(con.str);
            var select = @"Select CashbookName as Name, RemainingAmount as [Remaining Amount] from Cashbook";
            conn.Open();
            var dataAdapter = new SqlDataAdapter(select, conn);
            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
            select = "";
        }
    }
}
