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

        delegate void updateListBoxCallBack(String text);
        private void updateListBox(string text)
        {
            if(this.listBoxDB1.InvokeRequired)
            {
                updateListBoxCallBack d = new updateListBoxCallBack(updateListBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                //fill checked list box
                listBoxDB1.BeginUpdate();

                // TODO add items
                listBoxDB1.Items.Add("bla");

                // End the update process and force a repaint of the ListBox.
                listBoxDB1.EndUpdate();
            }
            
        }
        //database connection thread
        private void connection()
        {
            if(database.databaseConnection())
            {
                updateListBox("test");


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

        /// <summary>
        /// check for gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridToolStripMenuItem.Checked)
            {
                MessageBox.Show("check");
            }
            else
            {
                MessageBox.Show("uncheck");
            }
        }
    }
}
