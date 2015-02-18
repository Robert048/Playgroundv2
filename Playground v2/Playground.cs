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
    public partial class Playground : Form
    {
        public Playground()
        {
            InitializeComponent();
            System.Threading.Thread databaseConnection = new System.Threading.Thread(connection);
        }

        private void connection(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
