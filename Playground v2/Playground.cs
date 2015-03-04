using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground_v2
{
    public partial class Playground : Form
    {
        Database database;
        Thread databaseConnectionThread;

        public Playground()
        {
            InitializeComponent();
            database = new Database();

            //make a thread for the database connection
            databaseConnectionThread = new Thread(connection);
        }

        //database connection thread
        private void connection(object obj)
        {
            if(database.databaseConnection())
            {
                //TODO


                //close database connection
                database.closeConnection();
            }
            
            //close thread
            databaseConnectionThread.Abort();
        }
    }
}
