using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Exceptions;

namespace Gimnastika
{
    public partial class VezaForm : Form
    {
        float veza;
        public float Veza
        {
            get { return veza; }
        }

        public VezaForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                veza = float.Parse(txtVeza.Text.Replace(',', '.'));
                if (veza <= 0)
                    throw new InvalidPropertyException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Nekorektna vrednost za vezu.", "Greska");
                DialogResult = DialogResult.None;
            }
        }
    }
}