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
    public partial class frmAddEditContact : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        private clsContact _Contact { get; set; }
        public frmAddEditContact(int id)
        {
            InitializeComponent();
            ID = id;

            if (ID == -1)
                Mode = enMode.AddNew;
            else
                Mode = enMode.Update;

        }
        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void _LoadData()
        {
            _LoadCountries();
            if (Mode == enMode.AddNew)
            {
                lblCaption.Text = "Add new contact";
                cbCountries.SelectedIndex = 0;
                _Contact = new clsContact();
                return;
            }

            _Contact = clsContact.Find(ID);
            if (_Contact == null)
            {
                MessageBox.Show("Sorry!! Contact wit id = " + ID + " is not found");
            }

            txtID.Text                = _Contact.ID.ToString();
            txtFName.Text             = _Contact.FirstName;
            txtLName.Text             = _Contact.LastName;
            txtEmail.Text             = _Contact.Email;
            txtAddress.Text           = _Contact.Address;
            txtPhone.Text             = _Contact.Phone;
            dtpBirthdate.Value        = _Contact.DateOfBirth;
            cbCountries.SelectedIndex = cbCountries.FindString(clsCountry.Find(_Contact.CountryID).Name);

            if (!string.IsNullOrWhiteSpace(_Contact.ImagePath))
            {
                pbPicture.Load(_Contact.ImagePath);
            }

            lblCaption.Text = "Update contact with ID = " + _Contact.ID;
            
            llRemoveImage.Visible = !string.IsNullOrWhiteSpace(_Contact.ImagePath);
            

        }

        private void _LoadCountries()
        {
            var Countries = clsCountry.ListCountries();
            foreach (DataRow country in Countries.Rows)
            {
                cbCountries.Items.Add(country["CountryName"]);
            }
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Contact.FirstName   = txtFName.Text;
            _Contact.LastName    = txtLName.Text;
            _Contact.Email       = txtEmail.Text;
            _Contact.Address     = txtAddress.Text;
            _Contact.Phone       = txtPhone.Text;
            _Contact.DateOfBirth = dtpBirthdate.Value;
            _Contact.CountryID   = clsCountry.Find(cbCountries.Text).ID;

            if (string.IsNullOrWhiteSpace(pbPicture.ImageLocation))
            {
                _Contact.ImagePath = "";
            }
            else
            {
                _Contact.ImagePath = pbPicture.ImageLocation;
            }
            if (!_Contact.Save())
            {
                MessageBox.Show("Some thing went wrong!!!");
                return;
            }
            if (Mode == enMode.AddNew)
            {
                MessageBox.Show("Contact added successfully");
                lblCaption.Text = "Update contact with ID = " + _Contact.ID;
                txtID.Text = _Contact.ID.ToString();
                Mode = enMode.Update;
            }
            else
            {
                MessageBox.Show("Contact Updated successfully");

            }
        }

        private void llAddImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var openDilog = new OpenFileDialog())
            {
                if (openDilog.ShowDialog() == DialogResult.OK)
                {
                    pbPicture.ImageLocation = _Contact.ImagePath = openDilog.FileName;
                    llRemoveImage.Visible = true;
                }
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPicture.ImageLocation = _Contact.ImagePath = "";
            llRemoveImage.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
