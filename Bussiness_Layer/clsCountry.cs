using DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsCountry
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }

        public override string ToString()
        {
            return $"ID              : {ID}\n" +
                      $"Name       : {Name}\n" +
                      $"Code       : {Code}\n" +
                      $"Phone Code : {PhoneCode}";
        }

        private clsCountry(int iD, string name, string code, string phoneCode)
        {
            Mode = enMode.Update;
            ID = iD;
            Name = name;
            Code = code;
            PhoneCode = phoneCode;
        }

        public clsCountry()
        {
            Mode = enMode.AddNew;
            ID = -1;
            Name = "";
            
        }

        public static DataTable ListCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        public static clsCountry Find(int ID)
        {
            string Name = "", Code = "", PhoneCode = "";

            if (clsCountryData.FindCountryByID(ID, ref Name, ref Code, ref PhoneCode))
            {
                return new clsCountry(ID, Name, Code, PhoneCode);
            }
            return null;
        }
        
        public static clsCountry Find(string Name)
        {
            int ID = -1;
            string Code = "", PhoneCode = "";

            if (clsCountryData.FindCountryByName(Name, ref ID, ref Code, ref PhoneCode))
            {
                return new clsCountry(ID, Name, Code, PhoneCode);
            }
            return null;
        }

        private bool _AddNewContact()
        {
            ID = clsCountryData.AddNewCountry(Name, Code, PhoneCode);
            return ID != -1;
        }

        private bool _UpdateContact()
        {
            return clsCountryData.UpdateCountry(ID, Name, Code, PhoneCode);
        }

        public static bool DeleteCountry(int CountryID)
        {
            return clsCountryData.DeleteCountry(CountryID);
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

        public static bool IsCountryExist(int ID)
        {
            return clsCountryData.IsCountryExist(ID);
        }
        
        public static bool IsCountryExist(string Name)
        {
            return clsCountryData.IsCountryExist(Name);
        }
    }
}
