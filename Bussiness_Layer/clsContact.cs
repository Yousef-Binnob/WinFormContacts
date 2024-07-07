using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess_Layer;

namespace Bussiness_Layer
{
    public class clsContact
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public DateTime DateOfBirth { set; get; }

        public string ImagePath { set; get; }

        public int CountryID { set; get; }

        public override string ToString()
        {
            return $"ID              : {ID}\n"+
                      $"First Name      : {FirstName}\n"+
                      $"Last Name       : {LastName}\n" +
                      $"Email           : {Email}\n" +
                      $"Phone           : {Phone}\n" +
                      $"Address         : {Address}\n" +
                      $"Date Of Birth   : {DateOfBirth.ToString("dd/mm/yyyy")}\n" +
                      $"Country ID      : {CountryID}";
        }

        private clsContact(int iD, string firstName, string lastName, string email, string phone, string address, DateTime dateOfBirth, string imagePath, int countryID)
        {
            Mode = enMode.Update;
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            DateOfBirth = dateOfBirth;
            ImagePath = imagePath;
            CountryID = countryID;
        }

        public clsContact()
        {
            Mode = enMode.AddNew;
            ID = -1;
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Address = "";
            DateOfBirth = DateTime.Now;
            ImagePath = "";
            CountryID = -1;
        }

        public static DataTable ListContacts()
        {
            return clsContactData.GetAllContacts();
        }

        public static clsContact Find(int ID)
        {
            string firstName ="", lastName = "", email = "", phone = "", address = "", imagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            int countryID = -1;

            if (clsContactData.FindContactByID(ID, ref firstName, ref lastName, ref email,
                ref phone, ref address, ref dateOfBirth, ref imagePath, ref countryID)) 
            {
                return new clsContact(ID, firstName, lastName, email,
                    phone, address, dateOfBirth, imagePath, countryID);
            }
            return null;
        }
        
        public static bool IsContactExist(int ID)
        {
            return clsContactData.IsContactExist(ID);
        }

        private bool _AddNewContact()
        {
            ID = clsContactData.AddNewContact(FirstName,LastName,Email,Phone,Address,DateOfBirth,ImagePath,CountryID);
            return ID != -1;
        }

        private bool _UpdateContact()
        {
            return clsContactData.UpdateContact(ID, FirstName, LastName, Email, Phone, Address, DateOfBirth, ImagePath, CountryID);
        }

        public static bool DeleteContact(int ContactID)
        {
            return clsContactData.DeleteContact(ContactID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewContact())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateContact();
                default:
                    return false;
            }
        }
    }
}
