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
        ArrayList formulaList = new ArrayList();
        private int y;
        //ComboBox cbMachine;
        //ComboBox cbOperators;
        int lines = 0;
        Dictionary<int, ComboBox> cbMachine = new Dictionary<int, ComboBox>();
        Dictionary<int, ComboBox> cbOperators = new Dictionary<int, ComboBox>();
        Dictionary<int,TextBox> txtValue = new Dictionary<int,TextBox>();
        ComboBox cbAndOr;
        int amount = 0;
        int index = 0;


        public NewFormula(List<dbObject> machines)
        {
            InitializeComponent();
            y = 3;
            this.machines = machines;
            newLine();
        }

        private void fillComboBox(ComboBox cbMachineNew, ComboBox cbOperatorsNew)
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




                //cbOperators.Add(0, ">");
                //cbOperators.Add(1, "<");
                //cbOperators.Add(2, "=");
                //cbOperators.Add(3, ">=");
                //cbOperators.Add(4, "<=");
                //cbOperators.Add(5, "≠");
                


                cbAndOr.Items.Add("And");
                cbAndOr.Items.Add("Or");
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
            cbAndOr = new ComboBox();

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
                cbAndOr.Location = new Point(287, y - 30);
                cbAndOr.Size = new Size(70, 21);
                pnlNewFormula.Controls.Add(cbAndOr);
            }

            fillComboBox(cbMachineNew, cbOperatorsNew);
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

        private void btnNewFormula_Click(object sender, EventArgs e)
        {
            int x = 0;
            string formula = " ";

            var formulaList = new ArrayList();

           while(x < amount)
           {
         
      
               formula = cbMachine[x].Text + "  " + cbOperators[x].Text + "  " + txtValue[x].Text;
               formulaList.Add(formula);
               x++;
           }

          foreach(object f in formulaList)
          {
              MessageBox.Show(f.ToString());
          }
        }
    }
}
