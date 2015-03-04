using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground_v2
{
    public partial class Playground : Form
    {
        string connectionString = null;
        OdbcConnection conn;

        public Playground()
        {
            InitializeComponent();
            System.Threading.Thread databaseConnection = new System.Threading.Thread(connection);
        }

        //database connection thread
        private void connection(object obj)
        {
            connectionString = ConfigurationManager.ConnectionStrings["Aspen tech"].ConnectionString;
            conn = new OdbcConnection(connectionString);

            try
            {
                conn.Open();
                MessageBox.Show("Connection Open ! ");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }
    }
}
