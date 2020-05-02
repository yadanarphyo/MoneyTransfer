using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyTransfter
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void cashTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        CashbookRegistration c;
        private void cashbookRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (c == null)
            {
                c = new CashbookRegistration();
                c.MdiParent = this;
                c.FormClosed += new FormClosedEventHandler(c_FormClosed);
                c.Show();
            }
            else
                c.Activate();
        }

        private void c_FormClosed(object sender, FormClosedEventArgs e)
        {
            c = null;
            t = null;
            a = null;
            g = null;
            rct = null;
            gl = null;
            cl = null;
            dc = null;
            dgl = null;
            edittransfer = null;
            egl = null;

            //throw new NotImplementedException();
        }

        Transfer t;
        private void cashTransferToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (t == null)
            {
                t = new Transfer();
                t.MdiParent = this;
                t.FormClosed += new FormClosedEventHandler(c_FormClosed);
                t.Show();
            }
            else
                t.Activate();
        }

        AccountRegistration a;
        private void accountRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (a == null)
            {
                a = new AccountRegistration();
                a.MdiParent = this;
                a.FormClosed += new FormClosedEventHandler(c_FormClosed);
                a.Show();
            }
            else
                a.Activate();
        }

        GeneralLedger g;
        private void generalLedgerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (g == null)
            {
                g = new GeneralLedger();
                g.MdiParent = this;
                g.FormClosed += new FormClosedEventHandler(c_FormClosed);
                g.Show();
            }
            else
                g.Activate();
        }

        ReportCashTransfer rct;
        private void cashTransferToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (rct == null)
            {
                rct = new ReportCashTransfer();
                rct.MdiParent = this;
                rct.FormClosed += new FormClosedEventHandler(c_FormClosed);
                rct.Show();
            }
            else
                rct.Activate();
        }

        ReportGeneralLedger gl;
        private void generalLedgerToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gl == null)
            {
                gl = new ReportGeneralLedger();
                gl.MdiParent = this;
                gl.FormClosed += new FormClosedEventHandler(c_FormClosed);
                gl.Show();
            }
            else
                gl.Activate();
        }

        ReportCashbookList cl;
        private void cashbookListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cl == null)
            {
                cl = new ReportCashbookList();
                cl.MdiParent = this;
                cl.FormClosed += new FormClosedEventHandler(c_FormClosed);
                cl.Show();
            }
            else
                cl.Activate();
        }

        ReportDetailCashbook dc;
        private void cashTransferToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dc == null)
            {
                dc = new ReportDetailCashbook();
                dc.MdiParent = this;
                dc.FormClosed += new FormClosedEventHandler(c_FormClosed);
                dc.Show();
            }
            else
                dc.Activate();
        }

        ReportDetailGL dgl;
        private void detailGeneralLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgl == null)
            {
                dgl = new ReportDetailGL();
                dgl.MdiParent = this;
                dgl.FormClosed += new FormClosedEventHandler(c_FormClosed);
                dgl.Show();
            }
            else
                dgl.Activate();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        EditTransfer edittransfer;
        
        private void editTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (edittransfer == null)
            {
                edittransfer = new EditTransfer();
                edittransfer.MdiParent = this;
                edittransfer.FormClosed += new FormClosedEventHandler(c_FormClosed);
                edittransfer.Show();
            }
            else
                edittransfer.Activate();
        }

        editGL egl;
        private void editGeneralLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (egl == null)
            {
                egl = new editGL();
                egl.MdiParent = this;
                egl.FormClosed += new FormClosedEventHandler(c_FormClosed);
                egl.Show();
            }
            else
                egl.Activate();
        }
    }
}
