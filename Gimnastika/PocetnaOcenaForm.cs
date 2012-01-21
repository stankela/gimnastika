using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Entities;
using Gimnastika.Dao;
using Gimnastika.Exceptions;

namespace Gimnastika
{
    public partial class PocetnaOcenaForm : Form
    {
        PocetnaOcenaIzvedbe pocOcena;
        public PocetnaOcenaIzvedbe PocOcena
        {
            get { return pocOcena; }
        }

        public PocetnaOcenaForm()
        {
            InitializeComponent();

            Text = "Pocetna ocena";
            lblNapomena.Text = "Napomena: Maksimalan broj elemenata moze da se izostavi. " +
                "Ukoliko se izostavi, smatra se da pocetna ocena vazi za broj elemenata od minimalnog " +
                "pa navise.";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!updateOcenaFromUI())
            {
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private bool updateOcenaFromUI()
        {
            try
            {
                if (!validateDialog())
                    return false;
                doUpdateOcenaFromUI();
                return pocOcena.validate();
            }
            catch (InvalidPropertyException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                setFocus(ex.InvalidProperty);
                return false;
            }
            catch (InvalidFormatException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                setFocus(ex.InvalidProperty);
                return false;
            }
        }

        private void doUpdateOcenaFromUI()
        {
            if (txtMax.Text.Trim() != "")
                pocOcena = new PocetnaOcenaIzvedbe(int.Parse(txtMin.Text), 
                    int.Parse(txtMax.Text), float.Parse(txtOcena.Text.Replace(',', '.')));
            else
                pocOcena = new PocetnaOcenaIzvedbe(int.Parse(txtMin.Text),
                    float.Parse(txtOcena.Text.Replace(',', '.')));
        }

        private bool validateDialog()
        {
            return requiredFieldsValidation() && formatValidation();
        }

        private bool requiredFieldsValidation()
        {
            if (txtMin.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite vrednost za minimalan broj elemenata.", "Greska");
                txtMin.Focus();
                return false;
            }
            if (txtOcena.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite vrednost za ocenu.", "Greska");
                txtOcena.Focus();
                return false;
            }
            return true;
        }

        private bool formatValidation()
        {
            try
            {
                int.Parse(txtMin.Text);
            }
            catch (FormatException e)
            {
                throw new InvalidFormatException(
                    "Nepravilna vrednost za minimalan broj elemenata.", "MinBrojElemenata", e);
            }
            if (txtMax.Text.Trim() != "")
            {
                try
                {
                    int.Parse(txtMax.Text);
                }
                catch (FormatException e)
                {
                    throw new InvalidFormatException(
                        "Nepravilna vrednost za maksimalan broj elemenata.", "MaxBrojElemenata", e);
                }
            }
            try
            {
                float.Parse(txtOcena.Text.Replace(',', '.'));
            }
            catch (FormatException e)
            {
                throw new InvalidFormatException(
                    "Nepravilna vrednost za pocetnu ocenu.", "PocetnaOcena", e);
            }
            return true;
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "MinBrojElemenata":
                    txtMin.Focus();
                    break;

                case "MaxBrojElemenata":
                    txtMax.Focus();
                    break;

                case "PocetnaOcena":
                    txtOcena.Focus();
                    break;

                default:
                    break;
            }
        }

    }
}