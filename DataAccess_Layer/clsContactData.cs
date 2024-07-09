using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess_Layer
{
    public static class clsContactData
    {
        public static DataTable GetAllContacts()
        {
            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"select * from Contacts";
            var cmd = new SqlCommand(Query, Connection);

            var AllContacts = new DataTable();
            try
            {
                Connection.Open();
                using (var Reader = cmd.ExecuteReader())
                {
                    AllContacts.Load(Reader);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Connection.Close();
            }
            return AllContacts;
        }

        public static bool FindContactByID(int ID, ref string firstName, ref string lastName
            , ref string email, ref string phone, ref string address, ref DateTime dateOfBirth, ref string imagePath, ref int countryID)
        {
            var isFound = false;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = "select * from Contacts where ContactID = @ContactID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@ContactID", ID);

            try
            {
                Connection.Open();

                using (var Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        isFound = true;

                        firstName = (string)Reader["FirstName"];
                        lastName = (string)Reader["LastName"];
                        email = (string)Reader["Email"];
                        phone = (string)Reader["Phone"];
                        address = (string)Reader["Address"];
                        dateOfBirth = (DateTime)Reader["DateOfBirth"];
                        countryID = (int)Reader["CountryID"];

                        if (Reader["ImagePath"] != DBNull.Value)
                            imagePath = (string)Reader["ImagePath"];
                        else
                            imagePath = "";

                    }
                }
            }
            catch (Exception)
            {

                isFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return isFound;
        }

        public static bool IsContactExist(int ID)
        {
            var isFound = false;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = "select Found = 1 from Contacts where ContactID = @ContactID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@ContactID", ID);

            try
            {
                Connection.Open();

                using (var Reader = cmd.ExecuteReader())
                {
                    isFound = Reader.HasRows;
                }
            }
            catch (Exception)
            {
                isFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return isFound;
        }

        public static int AddNewContact(string firstName, string lastName, string email,
            string phone, string address, DateTime dateOfBirth, string imagePath, int countryID)
        {
            int ContactID = -1;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"insert into Contacts values(@FirstName,
                                                      @LastName,
                                                      @Email,
                                                      @Phone,
                                                      @Address,
                                                      @DateOfBirth,
                                                      @CountryID,
                                                      @ImagePath); select SCOPE_IDENTITY();";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            cmd.Parameters.AddWithValue("@CountryID", countryID);

            if (String.IsNullOrWhiteSpace(imagePath))
                cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            else
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);

            try
            {
                Connection.Open();

                var Result = cmd.ExecuteScalar();
                if (Result != null)
                {
                    ContactID = int.Parse(Result.ToString());
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Connection.Close();
            }
            return ContactID;
        }


        public static bool UpdateContact(int ContactID, string firstName, string lastName, string email,
            string phone, string address, DateTime dateOfBirth, string imagePath, int countryID)
        {
            if (!IsContactExist(ContactID))
            {
                return false;
            }
            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"update Contacts set FirstName   = @FirstName,
                                              LastName    = @LastName,
                                              Email       = @Email,
                                              Phone       = @Phone,
                                              Address     = @Address,
                                              DateOfBirth = @DateOfBirth,
                                              CountryID   = @CountryID,
                                              ImagePath   = @ImagePath
                                                          where ContactID = @ContactID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@ContactID", ContactID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            cmd.Parameters.AddWithValue("@CountryID", countryID);

            if (String.IsNullOrWhiteSpace(imagePath))
                cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            else
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);

            int Result = 0;
            try
            {
                Connection.Open();

                Result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            finally
            {
                Connection.Close();
            }
            return Result != 0;
        }

        public static bool DeleteContact(int contactID)
        {

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"delete from Contacts where ContactID = @ContactID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@ContactID", contactID);

            int RowsAffected = default;

            try
            {
                Connection.Open();

                RowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
    }
}
