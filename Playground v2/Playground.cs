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
        Database database;
        Thread databaseConnectionThread;
        Thread databaseOptionsThread;
        Thread newFormula;
        Thread databaseRefresh;

        //list with selected machines
        List<dbObject> machines;
        List<string> formulas;

        delegate void updateListBoxCallBack();


        public Playground()
        {
            InitializeComponent();
            database = new Database();

            //make a thread for the database connection
            databaseConnectionThread = new Thread(new ThreadStart(connection));
            databaseConnectionThread.Start();

            databaseRefresh = new Thread(new ThreadStart(databaseUpdate));
            databaseRefresh.Start();

            pnlPlayground.AutoScroll = true;
            pnlPlayground.HorizontalScroll.Enabled = true;
            pnlPlayground.HorizontalScroll.Visible = true;
            pnlPlayground.VerticalScroll.Visible = false;
        }

        private void databaseUpdate()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 3000;
            timer.Elapsed += tick;
            timer.Enabled = true;
        }

        private void tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            updateListBox();
            //playground updaten
            //fomules updaten
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
                string query = "select * from IP_PVDEF";
                OdbcDataAdapter dadapter = new OdbcDataAdapter();
                dadapter.SelectCommand = new OdbcCommand(query, database.getConnection());
                DataTable table = new DataTable();
                using (OdbcDataReader oReader = dadapter.SelectCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        //add items
                        listBoxDB1.Items.Add(oReader["NAME"].ToString());
                    }
                }

                // End the update process and force a repaint of the ListBox.
                listBoxDB1.EndUpdate();
            }

        }

        //database connection thread
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

        public void addFormulas(List<string> NewFormulas)
        {
            formulas = new List<string>();
            formulas = NewFormulas;

            pnlFormules.Invoke(new Action(() => pnlFormules.Controls.Clear()));

            //add formulas to playground
            int y = 0;
            int x = 0;

            Label txtFormula = new Label();
            txtFormula.Text = "Formule deel 1:";
            pnlFormules.Invoke(new Action(() => pnlFormules.Controls.Add(txtFormula)));



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
                MessageBox.Show("check");
            }
            else
            {
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
                listBoxDB1.BeginUpdate();
                listBoxDB1.Items.Clear();
                string searchText = searchBox.Text;
                OdbcCommand cmd = new OdbcCommand("SELECT NAME FROM IP_PVDEF WHERE IP_#_OF_TREND_VALUES = 2");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = database.getConnection();
                cmd.Parameters.Add("@naam", OdbcType.VarChar);
                cmd.Parameters.Add("@wildcard", OdbcType.Text).Value = "%";
                database.getConnection().Open();
                OdbcDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        listBoxDB1.Items.Add(dr[i].ToString());
                    }
                }
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
        /// click the oke button below listbox
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

        private void pnlResize(object sender, EventArgs e)
        {
            addMachines();
        }

        private void addMachines()
        {
            try
            {
                pnlPlayground.Controls.Clear();
                //add machines to playground
                int y = 0;
                int x = 0;
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

        private void clearPlaygroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlPlayground.Controls.Clear();
        }

        private void btnAddFormula_Click(object sender, EventArgs e)
        {
            newFormula = new Thread(new ThreadStart(newFormulaForm));
            newFormula.Start();
        }

        private void newFormulaForm()
        {
            NewFormula newFormula = new NewFormula(machines);
            Application.Run(newFormula);
        }

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
