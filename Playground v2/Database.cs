using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground_v2
{
    class Database
    {
        string connectionString = null;
        //OdbcConnection conn;
        SqlConnection conn;

        public Database()   
        {
        //get connectionString from the app.config and open the connection.
        //connectionString = ConfigurationManager.ConnectionStrings["Aspen tech"].ConnectionString;
        //conn = new OdbcConnection(connectionString);
        connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        conn = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Open a database connection.
        /// </summary>
        public bool databaseConnection()
        {
            // Start connection
            try
            {
                conn.Open();
                return true;
            }

            // If connection failed then display the error
            catch (Exception e)
            {
                Console.WriteLine("Database Connection failed | " + e.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Close the database connection.
        /// </summary>
        public void closeConnection()
        {
            conn.Close();
        }

    }
}
