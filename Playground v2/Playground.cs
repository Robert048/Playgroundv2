using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

        List<dbObject> machines;

        delegate void updateListBoxCallBack(String text);

        public Playground()
        {
            InitializeComponent();
            database = new Database();

            //make a thread for the database connection
            databaseConnectionThread = new Thread(new ThreadStart(connection));
            databaseConnectionThread.Start();
        }

        public class dbObject
        {
            public string naam { get; set; }

            public dbObject(string naam)
            {
                this.naam = naam;
            }
        }
        
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
                string query = "Select * from Table_1";
                SqlCommand command = new SqlCommand(query, database.getConnection());
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        //add items
                        listBoxDB1.Items.Add(oReader["id"].ToString()+ " - " + oReader["naam"].ToString());
                    }
                } 

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

        private void search(object sender, KeyEventArgs e)
        {
            listBoxDB1.BeginUpdate();
            listBoxDB1.Items.Clear();
            string searchText = searchBox.Text;
            SqlCommand cmd = new SqlCommand("SELECT naam FROM Table_1 WHERE naam LIKE (@wildcard)+(@naam)+(@wildcard)");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = database.getConnection();
            cmd.Parameters.AddWithValue("@naam", searchText);
            cmd.Parameters.AddWithValue("@wildcard", "%");
            database.getConnection().Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {   
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    listBoxDB1.Items.Add(dr[i].ToString());
                }
            }
            listBoxDB1.EndUpdate();
            database.getConnection().Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            machines = new List<dbObject>();
            foreach (int i in listBoxDB1.CheckedIndices)
            {
                dbObject temp = new dbObject(listBoxDB1.Text);
                machines.Add(temp);
            }

            //machines op panel doen
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (int i in listBoxDB1.CheckedIndices)
            {
                listBoxDB1.SetItemCheckState(i, CheckState.Unchecked);
            }
        }
    }
}
