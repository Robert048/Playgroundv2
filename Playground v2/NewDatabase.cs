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
        private string xmlPath = "";

        public NewDatabase(string xmlPath)
        {
            this.xmlPath = xmlPath;
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

                var key = label.Text.Substring(0, label.Text.Length - 1);
                var value = control.Text;

                if (key != "" && value != "")
                    if (!keyValues.ContainsKey(key))
                        keyValues.Add(key, value);

                i--;
            }

            string tabName = keyValues["Databasename (Tab name)"];
            string providerName = keyValues["Provider name"];

            StringBuilder connectionString = new StringBuilder();

            foreach (KeyValuePair<string, string> keyValuePair in keyValues)
            {
                if (keyValuePair.Key != tabName && keyValuePair.Key != providerName)
                    connectionString.Append(keyValuePair.Key + "=" + keyValuePair.Value + ";");
            }

            string xmlConfig = "<add name=\"Aspen tech\" connectionString=\"Dsn=IP21;trusted_connection=Yes;\" providerName=\"System.Data.Odbc\" />";
            System.Diagnostics.Debug.WriteLine("Tabname: " + tabName);
            System.Diagnostics.Debug.WriteLine("ProviderName: " + providerName);
            System.Diagnostics.Debug.WriteLine("XML: " + connectionString.ToString());
        }

        private void editXmlConfigFile()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlPath);

            XmlNodeList nodes = document.SelectNodes("configuration/connectionStrings/add");

            foreach (XmlNode node in nodes)
            {
                //string name = node.Attributes[""];
            }
        }
    }
}
