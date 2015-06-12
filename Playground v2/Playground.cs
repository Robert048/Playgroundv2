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
        private readonly DatabaseOdbc _databaseOdbc;
        private readonly DatabaseSql _databaseSql;

        //threads
        private readonly Thread databaseConnectionOdbcThread;
        private readonly Thread databaseConnectionSqlThread;

        Thread databaseOptionsThread;
        Thread newFormula;
        Thread databaseRefresh;

        //list with selected machines
        private readonly List<dbObject> machines;
        List<string> formulas;

        //method for invoke required
        delegate void updateListBox1CallBack();

        private delegate void updateListBox2CallBack();

        public Playground()
        {
            
            InitializeComponent();

            _databaseOdbc = new DatabaseOdbc();
            _databaseSql = new DatabaseSql();

            machines = new List<dbObject>();

            //make a thread for the database connection
            databaseConnectionOdbcThread = new Thread(ConnectionOdbc)
            {
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };
            databaseConnectionOdbcThread.Start();

            databaseConnectionSqlThread = new Thread(ConnectionSql)
            {
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };
            databaseConnectionSqlThread.Start();

            //make a thread for the database refresh
            databaseRefresh = new Thread(databaseUpdate);
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

        private int tempCounter1 = 0;
        private int tempCounter2 = 0;

        private async void UpdateListBox1Async()
        {
            if (listBoxDB1.InvokeRequired)
            {
                var d = new updateListBox1CallBack(UpdateListBox1Async);
                Invoke(d, new object[] {});
            }
            else
            {
                listBoxDB1.BeginUpdate();
                progressBar1.Visible = true;

                const string query = "select * from IP_PVDEF";

                var adapter = new OdbcDataAdapter
                {
                    SelectCommand = new OdbcCommand(query, _databaseOdbc.getConnection())
                };

                using (var reader = await adapter.SelectCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tempCounter1++;
                        // just a test
                        // todo: remove
                        if (tempCounter1 >= 10)
                            break;

                        var name = reader["NAME"].ToString();

                        listBoxDB1.Invoke(new Action(() => { listBoxDB1.Items.Add(name); }));
                    }
                }

                listBoxDB1.EndUpdate();
                progressBar1.Visible = false;
            }
        }

        private async void UpdateListBox2Async()
        {
            if (listBoxDB2.InvokeRequired)
            {
                var d = new updateListBox2CallBack(UpdateListBox2Async);
                Invoke(d, new object[] { });
            }
            else
            {
                listBoxDB2.BeginUpdate();
                progressBar2.Visible = true;

                const string query = "select * from usr_vw_job_EMM";

                var adapter = new SqlDataAdapter(query, _databaseSql.getConnection());

                using (var reader = await adapter.SelectCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tempCounter2++;
                        // just a test
                        // todo: remove
                        if (tempCounter2 >= 10)
                            break;

                        var name = reader.GetValue(0).ToString();

                        listBoxDB2.Invoke(new Action(() => { listBoxDB2.Items.Add(name); }));
                    }
                }

                listBoxDB2.EndUpdate();
                progressBar2.Visible = false;
            }
        }

        /// <summary>
        /// database connection thread
        /// </summary>
        private async void ConnectionOdbc()
        {
            if (await _databaseOdbc.databaseConnection())
            {
                // update the listbox
                await Task.Factory.StartNew(UpdateListBox1Async).ContinueWith((t) =>
                {
                    //close database connection
                    _databaseOdbc.closeConnection();
                });
            }

            //close thread
            databaseConnectionOdbcThread.Abort();
        }

        /// <summary>
        /// database connection thread
        /// </summary>
        private async void ConnectionSql()
        {
            if (await _databaseSql.databaseConnection())
            {
                // update the listbox
                await Task.Factory.StartNew(UpdateListBox2Async).ContinueWith((t) =>
                {
                    //close database connection
                    // todo: fix connection close exception
                    //_databaseSql.closeConnection();
                });
            }

            //close thread
            databaseConnectionSqlThread.Abort();
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
                cmd.Connection = _databaseOdbc.getConnection();
                cmd.Parameters.Add("@naam", OdbcType.VarChar);
                cmd.Parameters.Add("@wildcard", OdbcType.Text).Value = "%";
                //open database connection
                _databaseOdbc.getConnection().Open();
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
                _databaseOdbc.getConnection().Close();
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
            var selectedBox = tabControl.SelectedIndex;

            machines.Clear();

                //get checked items and place them in the list
                foreach (object itemChecked in listBoxDB1.CheckedItems)
                {
                    dbObject temp = new dbObject(itemChecked.ToString());
                    machines.Add(temp);
                }

                foreach (object itemChecked in listBoxDB2.CheckedItems)
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
            var selectedBox = tabControl.SelectedIndex;

            if (selectedBox == 0)
            {
                foreach (int i in listBoxDB1.CheckedIndices)
                {
                    listBoxDB1.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
            else
            {
                foreach (int i in listBoxDB2.CheckedIndices)
                {
                    listBoxDB1.SetItemCheckState(i, CheckState.Unchecked);
                }
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
