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
        private Dictionary<string, string> databaseConnections;
        private string xmlConfigPath = "";

        public DatabaseOptions()
        {
            InitializeComponent();
            setUpTapControl();

        }

        private void setUpTapControl()
        {
            
            tabControl1.Dock = DockStyle.Fill;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.Controls.Clear();

            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Search for the configuration file";
            openFileDialog1.Filter = "xml config files (*.config)|*.config";
            DialogResult dialogResult = STAShowDialog(openFileDialog1);

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                databaseConnections = new Dictionary<string, string>();

                xmlConfigPath = openFileDialog1.FileName;

                createDatabaseTabs();
            }

        }

        private void createDatabaseTabs()
        {
            var doc = new XmlDocument();
            doc.Load(xmlConfigPath);

            foreach (XmlNode node in doc.SelectNodes("//add"))
            {
                string connectionName = node.Attributes["name"].Value;
                string connectionString = node.Attributes["connectionString"].Value;
                string providerName = node.Attributes["providerName"].Value;

                if (!databaseConnections.ContainsKey(connectionName))
                {
                    databaseConnections.Add(connectionName, connectionString);
                }

                TabPage tabPage = new TabPage();
                tabPage.Name = connectionName;
                tabPage.Text = connectionName;
                tabPage.Text = providerName;

                string[] connectionStringItems = connectionString.Split(';');

                int currentX = 120;
                int currentY = 0;

                foreach (string item in connectionStringItems)
                {
                    string key = "";
                    string value = "";

                    try
                    {
                        key = item.Split('=')[0];
                        value = item.Split('=')[1];
                    }
                    catch
                    {
                        key = "";
                        value = "";
                    }

                    if (key.Length > 0)
                    {
                        Label label1 = new Label();
                        label1.Text = key;
                        label1.Location = new Point(0, currentY);
                        tabPage.Controls.Add(label1);
                    }

                    if (value.Length > 0)
                    {
                        TextBox textBox1 = new TextBox();
                        Size size = TextRenderer.MeasureText(item, textBox1.Font);
                        textBox1.Width = size.Width;
                        textBox1.Text = value;
                        textBox1.Location = new Point(currentX, currentY);
                        tabPage.Controls.Add(textBox1);
                    }

                    currentX += 0;
                    currentY += 30;

                }

                tabControl1.Controls.Add(tabPage);
            }
        }

        private DialogResult STAShowDialog(FileDialog dialog)
        {
            DialogState state = new DialogState();
            state.dialog = dialog;
            System.Threading.Thread t = new System.Threading.Thread(state.ThreadProcShowDialog);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            return state.result;
        }

        private void DatabaseOptions_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDatabase newDatabase = new NewDatabase(xmlConfigPath);
            newDatabase.Show();
        }

    }

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
