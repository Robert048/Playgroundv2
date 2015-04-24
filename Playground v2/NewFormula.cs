using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground_v2
{
    public partial class NewFormula : Form
    {

        List<dbObject> machines;
        List<string> formulaList = new List<string>();
        private int y;
        Dictionary<int, ComboBox> cbMachine = new Dictionary<int, ComboBox>();
        Dictionary<int, ComboBox> cbOperators = new Dictionary<int, ComboBox>();
        Dictionary<int,TextBox> txtValue = new Dictionary<int,TextBox>();
        Dictionary<int, ComboBox> cbAndOr = new Dictionary<int, ComboBox>();

        //ComboBox cbAndOr;
        int amount = 0;
        int index = 0;
        int lines = 0;


        public NewFormula(List<dbObject> machines)
        {
            InitializeComponent();
            y = 3;
            this.machines = machines;
            newLine();
        }

        private void fillComboBox(ComboBox cbMachineNew, ComboBox cbOperatorsNew, ComboBox cbAndOrNew)
        {
            try
            {
                cbMachineNew.Items.Clear();
                cbOperatorsNew.Items.Add(">");
                cbOperatorsNew.Items.Add("<");
                cbOperatorsNew.Items.Add("=");
                cbOperatorsNew.Items.Add(">=");
                cbOperatorsNew.Items.Add("<=");
                cbOperatorsNew.Items.Add("≠");

                foreach (dbObject machine in machines)
                {
                    cbMachineNew.Items.Add(machine.naam);

                }

                cbMachine.Add(index, cbMachineNew);
                cbOperators.Add(index, cbOperatorsNew);

                cbAndOrNew.Items.Add("And");
                cbAndOrNew.Items.Add("Or");

                cbAndOr.Add(index, cbAndOrNew);

            }
            catch(NullReferenceException)
            {
                MessageBox.Show("No machines available");
            }

            index++;

        }

        private void btnNewLine_Click(object sender, EventArgs e)
        {
            newLine();
            btnNewLine.Location = new Point(287, y - 30);
        }

        private void newLine()
        {
            ComboBox cbMachineNew = new ComboBox();
            ComboBox cbOperatorsNew = new ComboBox();
            TextBox txtValueNew = new TextBox();
            ComboBox cbAndOrNew = new ComboBox();

            cbMachineNew.Location = new Point(3, y);
            pnlNewFormula.Controls.Add(cbMachineNew);

            cbOperatorsNew.Location = new Point(130, y);
            cbOperatorsNew.Size = new Size(45, 21);
            pnlNewFormula.Controls.Add(cbOperatorsNew);
    

                txtValueNew.Location = new Point(181, y);
                txtValueNew.Size = new Size(100, 21);
                txtValue.Add(lines, txtValueNew);
                pnlNewFormula.Controls.Add(txtValueNew);
       

            if (y > 30)
            {
                cbAndOrNew.Location = new Point(287, y - 30);
                cbAndOrNew.Size = new Size(70, 21);
                pnlNewFormula.Controls.Add(cbAndOrNew);
            }

            fillComboBox(cbMachineNew, cbOperatorsNew, cbAndOrNew);
            y = y + 30;

            amount++;
            lines++;

            txtWarningMessage.Location = new Point(3, y);
            btnNewFormula.Location = new Point(286, y);
        }

        private void txtWarningMessageClick(object sender, EventArgs e)
        {
            txtWarningMessage.Text = "";
            txtWarningMessage.TextAlign = HorizontalAlignment.Left;
        }

        /// <summary>
        /// method for new formula button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewFormula_Click(object sender, EventArgs e)
        {
            int x = 0;
            string formula = " ";

            while (x < amount)
            {
                formula = cbAndOr[x].Text + "\n" + cbMachine[x].Text + "  " + cbOperators[x].Text + "  " + txtValue[x].Text;
                formulaList.Add(formula);
                x++;
            }
            
            //send formulaList to playground
            (System.Windows.Forms.Application.OpenForms["Playground"] as Playground).addFormulas(formulaList);
        }
    }
}
