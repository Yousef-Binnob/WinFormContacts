﻿using Bussiness_Layer;
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
        public clsContact Contact { get; set; }
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
                Contact = new clsContact();
                return;
            }

            Contact = clsContact.Find(ID);
            if (Contact == null)
            {
                MessageBox.Show("Sorry!! Contact wit id = " + ID + " is not found");
            }

            txtFName.Text             = Contact.FirstName;
            txtLName.Text             = Contact.LastName;
            txtEmail.Text             = Contact.Email;
            txtAddress.Text           = Contact.Address;
            txtPhone.Text             = Contact.Phone;
            dtpBirthdate.Value        = Contact.DateOfBirth;
            cbCountries.SelectedIndex = cbCountries.FindString(clsCountry.Find(Contact.CountryID).Name);

            if (!string.IsNullOrWhiteSpace(Contact.ImagePath))
            {
                pbPicture.ImageLocation = Contact.ImagePath;
            }

            lblCaption.Text = "Update contact with ID = " + Contact.ID;
            txtID.Text = Contact.ID.ToString();
            if (!string.IsNullOrWhiteSpace(Contact.ImagePath))
            {
                llRemoveImage.Visible = true;
            }

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
            Contact.FirstName   = txtFName.Text;
            Contact.LastName    = txtLName.Text;
            Contact.Email       = txtEmail.Text;
            Contact.Address     = txtAddress.Text;
            Contact.Phone       = txtPhone.Text;
            Contact.DateOfBirth = dtpBirthdate.Value;
            Contact.CountryID   = clsCountry.Find(cbCountries.Text).ID;

            if (string.IsNullOrWhiteSpace(pbPicture.ImageLocation))
            {
                Contact.ImagePath = "";
            }
            else
            {
                Contact.ImagePath = pbPicture.ImageLocation;
            }
            if (!Contact.Save())
            {
                MessageBox.Show("Some thing went wrong!!!");
                return;
            }
            if (Mode == enMode.AddNew)
            {
                MessageBox.Show("Contact added successfully");
                lblCaption.Text = "Update contact with ID = " + Contact.ID;
                txtID.Text = Contact.ID.ToString();
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
                    pbPicture.ImageLocation = Contact.ImagePath = openDilog.FileName;
                    llRemoveImage.Visible = true;
                }
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPicture.ImageLocation = Contact.ImagePath = "";
            llRemoveImage.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
