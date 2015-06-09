using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground_v2
{
    public partial class Playground : Form
    {
        //Database object
        Database database;

        //threads
        Thread databaseConnectionThread;
        Thread databaseOptionsThread;
        Thread newFormula;
        Thread databaseRefresh;

        //list with selected machines
        List<dbObject> machines;
        List<string> formulas;

        //method for invoke required
        delegate void updateListBoxCallBack();

        public Playground()
        {
            
            InitializeComponent();
            database = new Database();

            //make a thread for the database connection
            databaseConnectionThread = new Thread(new ThreadStart(connection));
            databaseConnectionThread.Start();
            //make a thread for the database refresh
            databaseRefresh = new Thread(new ThreadStart(databaseUpdate));
            databaseRefresh.Start();

            //add scrollbars to playground panel
            pnlPlayground.AutoScroll = true;
            pnlPlayground.HorizontalScroll.Enabled = true;
            pnlPlayground.HorizontalScroll.Visible = true;
            pnlPlayground.VerticalScroll.Visible = false;
        }

        /// <summary>
        /// method to update the database items
        /// </summary>
        private void databaseUpdate()
        {
            //TODO finishing
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 30000;
            //timer.Elapsed += tick;
            //timer.Enabled = true;
        }

        /// <summary>
        /// tick method for the timer in databaseUpdate()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //updateListBox();
            //TODO playground updaten
            //TODO fomules updaten
        }

        //fill the checkedlistbox with values from database
        private void updateListBox()
        {
            //check to fix thread problems
            if (this.listBoxDB1.InvokeRequired)
            {
                updateListBoxCallBack d = new updateListBoxCallBack(updateListBox);
                this.Invoke(d, new object[] { });
            }
            else
            {
                
                //fill checked list box
                listBoxDB1.BeginUpdate();
                //IP_PVDEF = table name on ASPEN TECH database
                string query = "select * from IP_PVDEF";
                OdbcDataAdapter dadapter = new OdbcDataAdapter();
                MessageBox.Show("1");
                dadapter.SelectCommand = new OdbcCommand(query, database.getConnection());
                MessageBox.Show("2");
                DataTable table = new DataTable();
                using (OdbcDataReader oReader = dadapter.SelectCommand.ExecuteReader())
                {
                    MessageBox.Show("3");
                    int i = 0;
                    while (oReader.Read())
                    {
                        //add items to checked listbox
                        i++;
                        listBoxDB1.Items.Add(oReader["NAME"].ToString());
                        if(i % 100 == 0)
                        {
                            MessageBox.Show(i + "");
                        }
                    }
                }

                // End the update process and force a repaint of the ListBox.
                listBoxDB1.EndUpdate();
            }
        }

        /// <summary>
        /// database connection thread
        /// </summary>
        private void connection()
        {
            if (database.databaseConnection())
            {
                //call update listbox method
                updateListBox();

                //close database connection
                database.closeConnection();
            }

            //close thread
            databaseConnectionThread.Abort();
        }

        /// <summary>
        /// Add formulas to the playground
        /// </summary>
        /// <param name="NewFormulas"></param>
        public void addFormulas(List<string> NewFormulas)
        {
            //make new list and fill it
            formulas = new List<string>();
            formulas = NewFormulas;

            pnlFormules.Invoke(new Action(() => pnlFormules.Controls.Clear()));

            //add formulas to playground
            int y = 0;
            int x = 0;

            //make label
            Label txtFormula = new Label();
            txtFormula.Text = "Formule deel 1:";
            pnlFormules.Invoke(new Action(() => pnlFormules.Controls.Add(txtFormula)));

            //loop through formulas List and displays them on panel
            foreach (object formula in formulas)
            {
                if (y >= pnlFormules.Height - 51)
                {
                    y = 0;
                    x = x + 101;
                }
                Label label = new Label();
                Panel panel = new Panel();

                panel.Controls.Add(label);
                panel.Size = new Size(366, 37);
                panel.BackColor = Color.LightSteelBlue;

                label.Text = formula.ToString();
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.MiddleCenter;
                
                label.AutoSize = true;
                label.Location = new Point((panel.Width / 2) - (label.Width / 2), 10);

                panel.Location = new Point(10 + x, 5 + y);

                pnlFormules.Invoke(new Action(() => pnlFormules.Controls.Add(panel)));

                y = y + 25;
            }

            Label txtFormulaEnd = new Label();
            txtFormulaEnd.Text = "Einde van formule";
            txtFormulaEnd.Location = new Point(0, y + 20);
            pnlFormules.Invoke(new Action(() => pnlFormules.Controls.Add(txtFormulaEnd)));

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
                //TODO something useful
                MessageBox.Show("check");
            }
            else
            {
                //TODO something useful
                MessageBox.Show("uncheck");
            }
        }

        /// <summary>
        /// search method for the textbox in first tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search(object sender, KeyEventArgs e)
        {
            try
            {
                //begin update for checked listbox
                listBoxDB1.BeginUpdate();
                listBoxDB1.Items.Clear();
                string searchText = searchBox.Text;
                //query to get the searched value
                //TODO query fixen
                OdbcCommand cmd = new OdbcCommand("SELECT NAME FROM IP_PVDEF WHERE IP_#_OF_TREND_VALUES = 2");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = database.getConnection();
                cmd.Parameters.Add("@naam", OdbcType.VarChar);
                cmd.Parameters.Add("@wildcard", OdbcType.Text).Value = "%";
                //open database connection
                database.getConnection().Open();
                OdbcDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        //add items to checked listbox
                        listBoxDB1.Items.Add(dr[i].ToString());
                    }
                }
                //end checked listbox update and close database connection
                listBoxDB1.EndUpdate();
                database.getConnection().Close();
            }

            catch (Exception ex)
            {
                searchBox.Text = "Database disabled.";
                searchBox.Enabled = false;
                MessageBox.Show(ex + "");
            }
        }

        /// <summary>
        /// click tihe oke button below lstbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //get checked items and place them in the list
            machines = new List<dbObject>();
            foreach (object itemChecked in listBoxDB1.CheckedItems)
            {
                dbObject temp = new dbObject(itemChecked.ToString());
                machines.Add(temp);
            }
            addMachines();
        }

        /// <summary>
        /// click on the clear button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (int i in listBoxDB1.CheckedIndices)
            {
                listBoxDB1.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        /// <summary>
        /// method the reorganize objects on playground if the size of the panel changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlResize(object sender, EventArgs e)
        {
            addMachines();
        }

        /// <summary>
        /// add machine objects to the playground panel
        /// </summary>
        private void addMachines()
        {
            try
            {
                pnlPlayground.Controls.Clear();
                //add machines to playground
                int y = 0;
                int x = 0;
                //loops through machines List and add them to the panel
                foreach (dbObject machine in machines)
                {
                    if (y >= pnlPlayground.Height - 51)
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
                    y = y + 51;
                }
            }
            catch (Exception)
            {
                // throw;
            }
        }

        /// <summary>
        /// View > clear playground
        /// clear the playground panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearPlaygroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlPlayground.Controls.Clear();
        }

        /// <summary>
        /// start a new thread if the add formula button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFormula_Click(object sender, EventArgs e)
        {
            newFormula = new Thread(new ThreadStart(newFormulaForm));
            newFormula.Start();
        }

        /// <summary>
        /// start the newFormula form
        /// </summary>
        private void newFormulaForm()
        {
            NewFormula newFormula = new NewFormula(machines);
            Application.Run(newFormula);
        }

        /// <summary>
        /// file > save
        /// saves the formulas to a .txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TextWriter tw = new StreamWriter("SavedFormulas.txt");

            foreach (string formula in formulas)
            {
                tw.WriteLine(formula);
            }

            tw.Close();
        }

        /// <summary>
        /// The file > open button
        /// opens the .txt file with formulas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open a file
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Search for the saved file";
            openFileDialog1.Filter = "saved files (*.txt)|*.txt";
            openFileDialog1.ShowDialog();

            String savedFilePath = openFileDialog1.FileName;
            List<string> tempList = new List<string>();

            //reading the file and adding the strings to the tempList
            using (StreamReader r = new StreamReader(savedFilePath))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    tempList.Add(line);
                }
            }

            //get naam from the tempList
            foreach (string naam in tempList)
            {
                string temp = naam;
                formulas.Add(temp);
            }

            //add formulas to playground
            addFormulas(formulas);
        }
    }
}
