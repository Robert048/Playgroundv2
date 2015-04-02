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
        private string xmlConfigPath;

        public NewDatabase(string xmlPath)
        {
            this.xmlConfigPath = xmlPath;

            InitializeComponent();
        }

        private void NewDatabase_Load(object sender, EventArgs e)
        {
            key3.SelectedIndex = 0;
            key3.DropDownStyle = ComboBoxStyle.DropDownList;

            key4.SelectedIndex = 0;
            key4.DropDownStyle = ComboBoxStyle.DropDownList;

            key9.SelectedIndex = 0;
            key9.DropDownStyle = ComboBoxStyle.DropDownList;

            key13.SelectedIndex = 0;
            key13.DropDownStyle = ComboBoxStyle.DropDownList;

            key16.SelectedIndex = 0;
            key16.DropDownStyle = ComboBoxStyle.DropDownList;

            key19.SelectedIndex = 0;
            key19.DropDownStyle = ComboBoxStyle.DropDownList;

            key21.SelectedIndex = 0;
            key21.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            int i = 32;

            foreach (var control in this.Controls.OfType<TextBox>())
            {

                var label = this.Controls["key" + i];

                var key = label.Text;

                if (key.Contains(':'))
                    key = label.Text.Substring(0, label.Text.Length - 1);

                var value = control.Text;

                if (key != "" && value != "")
                    if (!keyValues.ContainsKey(key))
                        keyValues.Add(key, value);

                i--;
            }

            try
            {
                string tabName = keyValues["Databasename (Tab name)"];
                string providerName = keyValues["Provider name"];

                // check if tabname and providername are empty
                if (String.IsNullOrEmpty(tabName) || String.IsNullOrEmpty(providerName))
                {
                    MessageBox.Show("You need to enter a TabName and Providername");
                    return;
                }

                loopKeyValuePairs(keyValues, tabName, providerName);
            }
            catch
            {
                MessageBox.Show("You need to enter a TabName and Providername");
                return;
            }
        }

        private void loopKeyValuePairs(Dictionary<string, string> keyValues, string tabName, string providerName)
        {
            StringBuilder connectionString = new StringBuilder();

            foreach (KeyValuePair<string, string> keyValuePair in keyValues)
            {
                if (keyValuePair.Key != "Databasename (Tab name)" && keyValuePair.Key != "Provider name")
                    connectionString.Append(keyValuePair.Key + "=" + keyValuePair.Value + ";");
            }

            editXmlConfigFile(tabName, providerName, connectionString.ToString());
        }

        private void editXmlConfigFile(string dbName, string providerName, string connectionString)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlConfigPath);

            XmlNode node = document.SelectSingleNode("configuration/connectionStrings");

            XmlNode newNode = document.CreateNode(XmlNodeType.Element, "add", "");

            XmlAttribute nameAttr = document.CreateAttribute("name");
            nameAttr.Value = dbName;
            newNode.Attributes.Append(nameAttr);

            XmlAttribute connectionStringAttr = document.CreateAttribute("connectionString");
            connectionStringAttr.Value = connectionString;
            newNode.Attributes.Append(connectionStringAttr);

            XmlAttribute providerNameAttr = document.CreateAttribute("providerName");
            providerNameAttr.Value = providerName;
            newNode.Attributes.Append(providerNameAttr);

            node.AppendChild(newNode.Clone());

            document.Save(xmlConfigPath);

            this.Close();
        }
    }
}
