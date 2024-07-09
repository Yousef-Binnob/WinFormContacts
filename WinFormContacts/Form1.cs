using Bussiness_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormContacts
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _Refresh();
        }

        private void _Refresh()
        {
            dgvContacts.DataSource = clsContact.ListContacts();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var frmAddEdit = new frmAddEditContact(-1);
            frmAddEdit.ShowDialog();
            _Refresh();
        }

        private void updaeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAddEdit = new frmAddEditContact((int)dgvContacts.CurrentRow.Cells[0].Value);
            frmAddEdit.ShowDialog();
            _Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var id = (int)dgvContacts.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("are you sure you want delete contact with ID = " + id,"Delete contact",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (clsContact.DeleteContact(id))
                {
                    MessageBox.Show("Contact with ID = " + id + " is deleted successfully");
                    _Refresh();
                }
                else
                {
                    MessageBox.Show("Sorry, something went wrong!!");
                }
            }
        }
    }
}
