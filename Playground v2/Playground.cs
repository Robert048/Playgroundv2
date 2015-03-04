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
        Thread databaseOptionsThread;

        public Playground()
        {
            InitializeComponent();
            database = new Database();

            //make a thread for the database connection
            databaseConnectionThread = new Thread(new ThreadStart(connection));
            databaseConnectionThread.Start();
        }

        //database connection thread
        private void connection()
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

        /// <summary>
        /// Edit > update database
        /// Makes a new thread to open a form for database settings
        /// </summary>
        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            databaseOptionsThread = new Thread(new ThreadStart(openForm));
            databaseOptionsThread.Start();
        }
        
        /// <summary>
        /// Opens the form for the database settings
        /// </summary>
        private void openForm()
        {
            DatabaseOptions databaseOptions = new DatabaseOptions();
            Application.Run(databaseOptions);
        }
    }
}
