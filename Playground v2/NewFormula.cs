using System;
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
        private int y;
        ComboBox cbMachine;
        ComboBox cbOperators;
        TextBox txtValue;
        ComboBox cbAndOr;

        public NewFormula(List<dbObject> machines)
        {
            InitializeComponent();
            y = 3;
            this.machines = machines;
            newLine();
        }

        private void fillComboBox()
        {
            try
            {
                cbMachine.Items.Clear();
                foreach (dbObject machine in machines)
                {
                    cbMachine.Items.Add(machine.naam);
                }
                cbOperators.Items.Add(">");
                cbOperators.Items.Add("<");
                cbOperators.Items.Add("=");
                cbOperators.Items.Add(">=");
                cbOperators.Items.Add("<=");
                cbOperators.Items.Add("≠");

                cbAndOr.Items.Add("And");
                cbAndOr.Items.Add("Or");
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("No machines available");
            }

        }

        private void btnNewLine_Click(object sender, EventArgs e)
        {
            newLine();
            btnNewLine.Location = new Point(287, y - 30);
        }

        private void newLine()
        {
            cbMachine = new ComboBox();
            cbOperators = new ComboBox();
            txtValue = new TextBox();
            cbAndOr = new ComboBox();

            cbMachine.Location = new Point(3, y);
            pnlNewFormula.Controls.Add(cbMachine);

            cbOperators.Location = new Point(130, y);
            cbOperators.Size = new Size(45, 21);
            pnlNewFormula.Controls.Add(cbOperators);

            txtValue.Location = new Point(181, y);
            txtValue.Size = new Size(100, 21);
            pnlNewFormula.Controls.Add(txtValue);

            if (y > 30)
            {
                cbAndOr.Location = new Point(287, y - 30);
                cbAndOr.Size = new Size(70, 21);
                pnlNewFormula.Controls.Add(cbAndOr);
            }

            fillComboBox();
            y = y + 30;

            txtWarningMessage.Location = new Point(3, y);
            btnNewFormula.Location = new Point(286, y);
        }

        private void txtWarningMessageClick(object sender, EventArgs e)
        {
            txtWarningMessage.Text = "";
            txtWarningMessage.TextAlign = HorizontalAlignment.Left;
        }

        private void btnNewFormula_Click(object sender, EventArgs e)
        {

        }
    }
}
