using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Playground_v2
{
    public partial class DatabaseOptions : Form
    {
        // stores all database connections from the config file
        private Dictionary<string, string> databaseConnections;
        // stores the current xml config path
        private string xmlConfigPath = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseOptions"/> class.
        /// </summary>
        public DatabaseOptions()
        {
            InitializeComponent();
            SetUpTabControl();
        }

        /// <summary>
        /// Tabcontrol setup
        /// </summary>
        private void SetUpTabControl()
        {
            tabControl1.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Menu click: "Open"
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open the config file
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Search for the configuration file";
            openFileDialog1.Filter = "config files (*.config)|*.config";
            DialogResult dialogResult = STAShowDialog(openFileDialog1);

            // set the configpath
            // create new tabs
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // create new cache for all database options
                databaseConnections = new Dictionary<string, string>();
                // set new config file path
                xmlConfigPath = openFileDialog1.FileName;
                // create database option tabs
                CreateDatabaseTabs();
            }

        }

        /// <summary>
        /// Creating new tabs for the tab control
        /// </summary>
        private void CreateDatabaseTabs()
        {
            // remove all old tabs
            tabControl1.TabPages.Clear();

            // open the config xml file
            var doc = new XmlDocument();
            doc.Load(xmlConfigPath);

            // loop through "add" nodes
            foreach (XmlNode node in doc.SelectNodes("//add"))
            {
                // check if node object is null
                if (node == null) continue;

                // get the connectionname, connectionstring and providername attributes
                var connectionName = node.Attributes["name"].Value;
                var connectionString = node.Attributes["connectionString"].Value;
                var providerName = node.Attributes["providerName"].Value;

                // add to databaseconnection dictionary
                if (!databaseConnections.ContainsKey(connectionName))
                {
                    // add the connection to cache
                    databaseConnections.Add(connectionName, connectionString);
                }

                // Create a new TabPage with connectionname
                TabPage tabPage = new TabPage
                {
                    Name = connectionName,
                    Text = connectionName
                };

                // Split all connectionstring items by ";"
                string[] connectionStringItems = connectionString.Split(';');

                // For placing objects in the form
                int currentX = 120;
                int currentY = 0;

                // Loop through all connectionstring items
                foreach (string item in connectionStringItems)
                {
                    var key = "";
                    var value = "";

                    // try to split the key an value
                    try
                    {
                        // get the key (value [0] in front of "=")
                        key = item.Split('=')[0];
                        // get the value (value [1] after "=")
                        value = item.Split('=')[1];
                    }
                    catch
                    {
                        // exception: set to null
                        key = "";
                        value = "";
                    }

                    // create new label with the key as text
                    if (key.Length > 0)
                    {
                        // create new label object and set location and text
                        var label1 = new Label
                        {
                            Text = key,
                            Location = new Point(0, currentY)
                        };

                        // add the label to the tabpage
                        tabPage.Controls.Add(label1);
                    }

                    // Create new TextBox with value as text
                    if (value.Length > 0)
                    {
                        var textBox1 = new TextBox();
                        var size = TextRenderer.MeasureText(item, textBox1.Font);
                        textBox1.Width = size.Width;
                        textBox1.Text = value;
                        textBox1.Location = new Point(currentX, currentY);
                        // add the textbox to the tabpage
                        tabPage.Controls.Add(textBox1);
                    }

                    // set new x and y values
                    currentX += 0;
                    currentY += 30;
                }

                // add the tabpage to tabcontrol
                tabControl1.Controls.Add(tabPage);
            }
        }

        /// <summary>
        /// Show STA Dialog. Multithreaded ShowDialog process
        /// </summary>
        /// <returns>The DialogState result as DialogResult object</returns>
        private DialogResult STAShowDialog(FileDialog dialog)
        {
            // create new dialog state instance
            var state = new DialogState
            {
                dialog = dialog
            };

            // set threading for file open dialog
            System.Threading.Thread t = new System.Threading.Thread(state.ThreadProcShowDialog);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            
            // return the result
            return state.result;
        }

        /// <summary>
        /// Open NewDatabase form
        /// Check if XML file is already open
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmlConfigPath == "")
            {
                MessageBox.Show("Please open the file first.");
                return;
            }

            NewDatabase newDatabase = new NewDatabase(xmlConfigPath);
            newDatabase.FormClosed += newDatabase_FormClosed;
            newDatabase.Show();
        }

        /// <summary>
        /// Detect if NewDatabase form is closed.
        /// Recreate the tabs in this form
        /// </summary>
        private void newDatabase_FormClosed(object sender, FormClosedEventArgs e)
        {
            CreateDatabaseTabs();
        }

    }

    /// <summary>
    /// Multithread file dialog open as STAThread
    /// </summary>
    public class DialogState
    {
        public DialogResult result;
        public FileDialog dialog;

        public void ThreadProcShowDialog()
        {
            result = dialog.ShowDialog();
        }
    }
}
