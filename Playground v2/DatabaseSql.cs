﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground_v2
{
    class DatabaseSql
    {
        private string connectionString = null;
        private readonly SqlConnection conn;

        public DatabaseSql()
        {
            //get connectionString from the app.config and open the connection.
            connectionString = ConfigurationManager.ConnectionStrings["Wonderware"].ConnectionString;
            //connectionString = Encryption.DecryptStringAES(connectionString, Encryption.Secret);
            conn = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Open a database connection asynchronous
        /// </summary>
        public async Task<bool> databaseConnection()
        {
            // Start connection
            try
            {
                await conn.OpenAsync();
                return true;
            }
            // If connection failed then display the error
            catch (Exception e)
            {
                throw new Exception("Can't open database connection : " + e);
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

        /// <summary>
        /// method to return the connection
        /// </summary>
        /// <returns>connection</returns>
        public SqlConnection getConnection()
        {
            return conn;
        }

    }
}
