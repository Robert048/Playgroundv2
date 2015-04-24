using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Playground_v2
{
    public partial class NewDatabase : Form
    {
        // temp config path
        private string xmlConfigPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewDatabase"/> class.
        /// </summary>
        /// <param name="xmlPath">The XML config path.</param>
        public NewDatabase(string xmlPath)
        {
            this.xmlConfigPath = xmlPath;
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the NewDatabase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void NewDatabase_Load(object sender, EventArgs e)
        {
            // set dropdownlist style for all dropdown elements
            // set the selected index on 0
            foreach (var control in Controls)
            {
                if (control is ComboBox)
                {
                    (control as ComboBox).SelectedIndex = 0;
                    (control as ComboBox).DropDownStyle = ComboBoxStyle.DropDownList;
                }
            }
        }

        /// <summary>
        /// Counts all textBoxes.
        /// </summary>
        /// <param name="textboxCount">The textbox count as reference.</param>
        /// <param name="controls">The controls.</param>
        private void CountBoxes(ref int textboxCount, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is TextBox)
                    textboxCount++;
            }
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            // temp storage for key values (label + textbox)
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            // count of all elements
            int i = 0;
            // get the element count, reference to i
            CountBoxes(ref i, this.Controls);

            // loop through all controls
            foreach (var control in this.Controls.OfType<TextBox>())
            {
                // get the label
                var label = this.Controls["key" + i];

                // get the text of the label as the key
                var key = label.Text;

                // check if it contains ':', cut it away
                if (key.Contains(':'))
                    key = label.Text.Substring(0, label.Text.Length - 1);

                // get the value text
                var value = control.Text;

                // check if length > 0 and than add to keyValues
                if (key != "" && value != "")
                    if (!keyValues.ContainsKey(key))
                        keyValues.Add(key, value);

                // decrement local variable i
                i--;
            }

            try
            {
                // get the value of the tabname and the provider name
                string tabName = keyValues["Databasename (Tab name)"];
                string providerName = keyValues["Provider name"];

                // check if tabname and providername are empty
                if (String.IsNullOrEmpty(tabName) || String.IsNullOrEmpty(providerName))
                {
                    MessageBox.Show("You need to enter a TabName and Providername");
                    return; // don't move on
                }

                // Check keyvalues
                loopKeyValuePairs(keyValues, tabName, providerName);
            }
            catch
            {
                MessageBox.Show("You need to enter a TabName and Providername");
                return;
            }
        }

        /// <summary>
        /// Loop through all keyvalue pairs and add them to the connectionstring
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="providerName">Name of the provider.</param>
        private void loopKeyValuePairs(Dictionary<string, string> keyValues, string tabName, string providerName)
        {
            // the string for connectionString
            StringBuilder connectionString = new StringBuilder();

            // loop through all keyvalues
            foreach (KeyValuePair<string, string> keyValuePair in keyValues)
            {
                // delete tab name and providername from connection string
                if (keyValuePair.Key != "Databasename (Tab name)" && keyValuePair.Key != "Provider name")
                    connectionString.Append(keyValuePair.Key + "=" + keyValuePair.Value + ";");
            }

            // edit the current xml file
            editXmlConfigFile(tabName, providerName, connectionString.ToString());
        }

        /// <summary>
        /// Edits the XML configuration file.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="connectionString">The connection string.</param>
        private void editXmlConfigFile(string dbName, string providerName, string connectionString)
        {
            // open the xml document with temp config path
            XmlDocument document = new XmlDocument();
            document.Load(xmlConfigPath);

            // select the connectionstrings node
            XmlNode node = document.SelectSingleNode("configuration/connectionStrings");

            // add a new node
            XmlNode newNode = document.CreateNode(XmlNodeType.Element, "add", "");

            // Create attribute name
            XmlAttribute nameAttr = document.CreateAttribute("name");
            nameAttr.Value = dbName;
            newNode.Attributes.Append(nameAttr);

            // Create attribute connectionString
            XmlAttribute connectionStringAttr = document.CreateAttribute("connectionString");
            connectionStringAttr.Value = connectionString;
            newNode.Attributes.Append(connectionStringAttr);

            // Create attribute providerName
            XmlAttribute providerNameAttr = document.CreateAttribute("providerName");
            providerNameAttr.Value = providerName;
            newNode.Attributes.Append(providerNameAttr);

            // append the new child node to the root node
            node.AppendChild(newNode.Clone());

            // save xml config file
            document.Save(xmlConfigPath);

            // Close the configuration form
            this.Close();
        }
    }
}
