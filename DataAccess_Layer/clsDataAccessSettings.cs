using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer
{
    internal static class clsDataAccessSettings
    {
        public static string ConnectionString { get; set; } = @"Server = MSI\SQLEXPRESS;Database = ContactsDB; Integrated Security = true;";
    }
}
