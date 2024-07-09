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
    }
}
