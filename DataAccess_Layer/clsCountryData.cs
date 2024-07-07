using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer
{
    public class clsCountryData
    {
        public static DataTable GetAllCountries()
        {
            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"select * from Countries";
            var cmd = new SqlCommand(Query, Connection);

            var AllCountries = new DataTable();
            try
            {
                Connection.Open();
                using (var Reader = cmd.ExecuteReader())
                {
                    AllCountries.Load(Reader);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Connection.Close();
            }
            return AllCountries;
        }
        public static bool FindCountryByID(int ID, ref string Name, ref string Code, ref string PhoneCode)
        {
            var isFound = false;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = "select * from Countries where CountryID = @CountryID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                Connection.Open();

                using (var Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        isFound = true;

                        Name = (string)Reader["CountryName"];
                        if (Reader["Code"] != DBNull.Value)
                        {
                            Code = (string)Reader["Code"];
                        }
                        else
                        {
                            Code = "";
                        }

                        if (Reader["PhoneCode"] != DBNull.Value)
                        {
                            PhoneCode = (string)Reader["PhoneCode"];
                        }
                        else
                        {
                            PhoneCode = "";
                        }
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

        public static bool FindCountryByName(string Name, ref int ID, ref string Code, ref string PhoneCode)
        {
            var isFound = false;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = "select * from Countries where CountryName = @CountryName";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@CountryName", Name);

            try
            {
                Connection.Open();

                using (var Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        isFound = true;

                        ID = (int)Reader["CountryID"];

                        if (Reader["Code"] != DBNull.Value)
                        {
                            Code = (string)Reader["Code"];
                        }
                        else
                        {
                            Code = "";
                        }

                        if (Reader["PhoneCode"] != DBNull.Value)
                        {
                            PhoneCode = (string)Reader["PhoneCode"];
                        }
                        else
                        {
                            PhoneCode = "";
                        }
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


        public static int AddNewCountry(string Name, string Code, string PhoneCode)
        {
            int CountryID = -1;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"insert into Countries values(@Name, @Code, @PhoneCode); select SCOPE_IDENTITY();";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@Name", Name);

            if (string.IsNullOrWhiteSpace(Code))
            {
                cmd.Parameters.AddWithValue("@Code", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Code", Code);
            }

            if (string.IsNullOrWhiteSpace(PhoneCode))
            {
                cmd.Parameters.AddWithValue("@PhoneCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PhoneCode", PhoneCode);
            }


            try
            {
                Connection.Open();

                var Result = cmd.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    CountryID = insertedID;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Connection.Close();
            }
            return CountryID;
        }

        public static bool UpdateCountry(int CountryID, string CountryName, string Code, string PhoneCode)
        {
            
            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"update Countries set CountryName = @CountryName,
                                               Code        = @Code,
                                               PhoneCode   = @PhoneCode
                                                          where CountryID = @CountryID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            if (string.IsNullOrWhiteSpace(Code))
            {
                cmd.Parameters.AddWithValue("@Code", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Code", Code);
            }

            if (string.IsNullOrWhiteSpace(PhoneCode))
            {
                cmd.Parameters.AddWithValue("@PhoneCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PhoneCode", PhoneCode);
            }


            int Result = 0;
            try
            {
                Connection.Open();

                Result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Connection.Close();
            }
            return Result > 0;
        }

        public static bool DeleteCountry(int CountryID)
        {

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = @"delete from Countries where CountryID = @CountryID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);

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

        public static bool IsCountryExist(int ID)
        {
            var isFound = false;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = "select Found = 1 from Countries where CountryID = @CountryID";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@CountryID", ID);

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

        public static bool IsCountryExist(string Name)
        {
            var isFound = false;

            var Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            var Query = "select Found = 1 from Countries where CountryName = @CountryName";

            var cmd = new SqlCommand(Query, Connection);
            cmd.Parameters.AddWithValue("@CountryName", Name);

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
    }
}
