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
            fillBox();

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
        
        public void fillBox()
        {
            listBoxDB1.Items.Add("yolo");
            listBoxDB1.Items.Add("rwer");
            listBoxDB1.Items.Add("sffd");
            listBoxDB1.Items.Add("swagger");
            listBoxDB1.Items.Add("dsf");
            listBoxDB1.Items.Add("fsdfsdf");
            listBoxDB1.Items.Add("\xzx");
            listBoxDB1.Items.Add("xcvxv");
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
            foreach (object itemChecked in listBoxDB1.CheckedItems)
            {
                dbObject temp = new dbObject(itemChecked.ToString());
                machines.Add(temp);
            }

            //machines op panel doen
            int y = -51;
            int x = 0;
            foreach (dbObject machine in machines)
            {
                y = y + 51;
                if(y >= pnlPlayground.Height)
                {
                    y = 0;
                    x = x + 101;
                }
                Label label = new Label();
                Panel panel = new Panel();

                panel.Controls.Add(label);
                panel.Size = new Size(100, 50);
                panel.BackColor = Color.Yellow;

                label.Text = machine.naam;
                label.AutoSize = true;
                label.Location = new Point((panel.Width / 2) - (label.Width / 2), 10);

                panel.Location = new Point(10 + x, 10 + y);

                pnlPlayground.Controls.Add(panel);

            }


            



            
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
