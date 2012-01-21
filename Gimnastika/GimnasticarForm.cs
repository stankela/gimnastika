using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Entities;
using Gimnastika.Exceptions;
using Gimnastika.Dao;

namespace Gimnastika
{
    public partial class GimnasticarForm : Form
    {
        private Gimnasticar gimnasticar = null;
        private Gimnasticar original;
        private bool editMode;

        public Gimnasticar Gimnasticar
        {
            get { return gimnasticar; }
        }

        public GimnasticarForm(Gimnasticar gimnasticar)
        {
            InitializeComponent();
            initUI();

            this.gimnasticar = gimnasticar;
            if (gimnasticar == null)
            {
                editMode = false;
            }
            else
            {
                editMode = true;
                original = (Gimnasticar)gimnasticar.Copy();
                updateUIFromEntity(gimnasticar);
            }
        }

        private void initUI()
        {
            this.Text = "Novi gimnasticar";
            txtIme.Text = String.Empty;
            txtPrezime.Text = String.Empty;
        }

        private void updateUIFromEntity(Gimnasticar gimnasticar)
        {
            this.Text = "Gimnasticar: " + gimnasticar.ToString();
            txtIme.Text = gimnasticar.Ime;
            txtPrezime.Text = gimnasticar.Prezime;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!validateDialog())
            {
                this.DialogResult = DialogResult.None;
                return;
            }
            try
            {
                if (editMode)
                {
                    UpdateGimnasticar();
                    new GimnasticarDAO().update(gimnasticar, original);
                }
                else
                {
                    CreateGimnasticar();
                    new GimnasticarDAO().insert(gimnasticar);
                }
            }
            catch (InvalidPropertyException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                setFocus(ex.InvalidProperty);
                this.DialogResult = DialogResult.None;
                return;
            }
            catch (DatabaseConstraintException ex)
            {
                MessageBox.Show(ex.ValidationErrors[0].Message, "Greska");
                setFocus(ex.ValidationErrors[0].InvalidProperties[0]);
                this.DialogResult = DialogResult.None;
                return;
            }
            catch (DatabaseException)
            {
                string message;
                if (editMode)
                {
                    message = "Neuspesna promena gimnasticara u bazi.";
                    //message = string.Format(
                    //"Neuspesna promena gimnasticara u bazi. \n{0}", ex.InnerException.Message);
                }
                else
                {
                    message = "Neuspesan upis novog gimnasticara u bazu.";
                    //message = string.Format(
                    //"Neuspesan upis novog gimnasticara u bazu. \n{0}", ex.InnerException.Message);
                }
                MessageBox.Show(message, "Greska");
                discardChanges();
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception)
            {
                discardChanges();
                throw;
            }
        }

        private void discardChanges()
        {
            if (editMode)
                gimnasticar.restore(original);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            discardChanges();
        }

        private bool validateDialog()
        {
            return requiredFieldsValidation();
        }

        private bool requiredFieldsValidation()
        {
            if (txtIme.Text.Trim() == String.Empty
             && txtPrezime.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite ime ili prezime gimnasticara.", "Greska");
                txtIme.Focus();
                return false;
            }
            return true;
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Ime":
                    txtIme.Focus();
                    break;
                case "Prezime":
                    txtPrezime.Focus();
                    break;

                default:
                    break;
            }
        }

        private void CreateGimnasticar()
        {
            gimnasticar = new Gimnasticar();
            UpdateEntityFromUI(gimnasticar);
            DatabaseConstraintsValidator.checkInsert(gimnasticar);
        }

        private void UpdateGimnasticar()
        {
            UpdateEntityFromUI(gimnasticar);
            DatabaseConstraintsValidator.checkUpdate(gimnasticar, original);
        }

        private void UpdateEntityFromUI(Gimnasticar gimnasticar)
        {
            gimnasticar.Ime = txtIme.Text.Trim();
            gimnasticar.Prezime = txtPrezime.Text.Trim();
        }

        private void GimnasticarForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                discardChanges();
            }
        }
    }
}