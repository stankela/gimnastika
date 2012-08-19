using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Domain;
using Gimnastika.Exceptions;
using Gimnastika.Dao;

namespace Gimnastika
{
    public partial class PraviloForm : Form
    {
        private PraviloOceneVezbe pravilo = null;
        private PraviloOceneVezbe original;
        private bool editMode;

        public PraviloOceneVezbe Pravilo
        {
            get { return pravilo; }
        }

        public PraviloForm(PraviloOceneVezbe pravilo)
        {
            InitializeComponent();
            initUI();

            this.pravilo = pravilo;
            if (pravilo == null)
            {
                editMode = false;
                this.pravilo = new PraviloOceneVezbe();
            }
            else
            {
                editMode = true;
                original = (PraviloOceneVezbe)pravilo.Clone();
                updateUIFromEntity(pravilo);
            }
        }

        private void initUI()
        {
            this.Text = "Pravila ocenjivanja";
            txtNazivPravila.Text = String.Empty;
            txtBrojBodovanih.Text = String.Empty;
            txtMaxIstaGrupa.Text = string.Empty;

            setupGrid();
        }

        private void setupGrid()
        {
            gridIzvedba.MultiSelect = false;
            gridIzvedba.AllowUserToAddRows = false;
            gridIzvedba.AllowUserToDeleteRows = false;
            gridIzvedba.AllowUserToResizeRows = false;
            gridIzvedba.AutoGenerateColumns = false;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.Name = "BrojElemenata";
            column.HeaderText = "Broj elemenata";
            column.Width = 110;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridIzvedba.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "PocetnaOcena";
            column.HeaderText = "Pocetna ocena";
            column.Width = 110;
            column.DefaultCellStyle.Format = "F2";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridIzvedba.Columns.Add(column);
        }

        private void updateUIFromEntity(PraviloOceneVezbe pravilo)
        {
            txtNazivPravila.Text = pravilo.Naziv;
            txtBrojBodovanih.Text = pravilo.BrojBodovanihElemenata.ToString();
            txtMaxIstaGrupa.Text = pravilo.MaxIstaGrupa.ToString();
            updateGridIzvedba();
        }

        private void updateGridIzvedba()
        {
            gridIzvedba.Rows.Clear();
            foreach (PocetnaOcenaIzvedbe ocena in pravilo.PocetneOceneIzvedbe)
            {
                // ovo ne generise CellValueChanged event
                gridIzvedba.Rows.Add(getRowValues(ocena));
            }
        }

        private object[] getRowValues(PocetnaOcenaIzvedbe ocena)
        {
            return new object[] { ocena.OpsegString, ocena.PocetnaOcena };
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!updatePraviloFromUI())
                {
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (editMode)
                    new PraviloOceneVezbeDAO().update(pravilo, original);
                else
                    new PraviloOceneVezbeDAO().insert(pravilo);
            }
            catch (DatabaseException)
            {
                string message;
                if (editMode)
                    message = "Neuspesna promena pravila u bazi.";
                else
                    message = "Neuspesan upis novog pravila u bazu.";
                MessageBox.Show(message, "Greska");
                discardChanges();
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception)
            {
                discardChanges();
                throw;
            }
        }

        private bool validateDialog()
        {
            return requiredFieldsValidation() && formatValidation();
        }

        private bool requiredFieldsValidation()
        {
            if (txtNazivPravila.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite naziv pravila.", "Greska");
                txtNazivPravila.Focus();
                return false;
            }
            if (txtBrojBodovanih.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite broj elemenata koji se boduju.", "Greska");
                txtBrojBodovanih.Focus();
                return false;
            }
            if (txtMaxIstaGrupa.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite maksimalan broj elemenata iz iste grupe " +
                    "koji se boduju.", "Greska");
                txtMaxIstaGrupa.Focus();
                return false;
            }
            if (gridIzvedba.Rows.Count == 0)
            {
                MessageBox.Show("Unesite pocetne ocene za izvedbu.", "Greska");
                gridIzvedba.Focus();
                return false;
            }
            return true;
        }

        private bool formatValidation()
        {
            try
            {
                int.Parse(txtBrojBodovanih.Text);
            }
            catch (FormatException e)
            {
                throw new InvalidFormatException(
                    "Unesite broj elemenata koji se boduju.", "BrojBodovanih", e);
            }
            try
            {
                int.Parse(txtMaxIstaGrupa.Text);
            }
            catch (FormatException e)
            {
                throw new InvalidFormatException(
                    "Unesite maksimalan broj elemenata iz iste grupe " +
                    "koji se boduju.", "MaxIstaGrupa", e);
            }
            return true;
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNazivPravila.Focus();
                    break;

                case "BrojBodovanih":
                    txtBrojBodovanih.Focus();
                    break;

                case "MaxIstaGrupa":
                    txtMaxIstaGrupa.Focus();
                    break;

                case "PocetneOceneIzvedbe":
                    gridIzvedba.Focus();
                    break;

                default:
                    break;
            }
        }

        private bool updatePraviloFromUI()
        {
            try
            {
                if (!validateDialog())
                    return false;
                doUpdatePraviloFromUI();
                if (editMode)
                    DatabaseConstraintsValidator.checkUpdate(pravilo, original);
                else
                    DatabaseConstraintsValidator.checkInsert(pravilo);
                return true;
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
            catch (DatabaseConstraintException ex)
            {
                MessageBox.Show(ex.ValidationErrors[0].Message, "Greska");
                setFocus(ex.ValidationErrors[0].InvalidProperties[0]);
                return false;
            }
            catch (DatabaseException)
            {
                string message;
                if (editMode)
                    message = "Neuspesna promena pravila u bazi.";
                else
                    message = "Neuspesan upis novog pravila u bazu.";
                MessageBox.Show(message, "Greska");
                return false;
            }
            catch (Exception)
            {
                // TODO: Ovde treba hvatati konkurentne izuzetke koji mogu da nastanu
                // usled promene u bazi izmedju trenutka kada proveravam constraints
                // i trenutka kada azuriram bazu.
                throw;
            }
        }

        private void doUpdatePraviloFromUI()
        {
            pravilo.Naziv = txtNazivPravila.Text.Trim();
            pravilo.BrojBodovanihElemenata = int.Parse(txtBrojBodovanih.Text);
            pravilo.MaxIstaGrupa = int.Parse(txtMaxIstaGrupa.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            discardChanges();
        }

        private void PraviloForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                discardChanges();
            }
        }

        private void discardChanges()
        {
            if (editMode)
                pravilo.restore(original);
        }

        private void btnDodajOcenu_Click(object sender, EventArgs e)
        {
            PocetnaOcenaForm f = new PocetnaOcenaForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pravilo.dodajPocetnuOcenuIzvedbe(f.PocOcena);
                    updateGridIzvedba();
                }
                catch (InvalidPropertyException ex)
                {
                    MessageBox.Show(ex.Message, "Greska");
                }
            }
        }

        private void btnIzbrisiOcenu_Click(object sender, EventArgs e)
        {
            if (gridIzvedba.Rows.Count > 0)
            {
                PocetnaOcenaIzvedbe ocena = pravilo.PocetneOceneIzvedbe[gridIzvedba.CurrentRow.Index];
                pravilo.ukloniPocetnuOcenuIzvedbe(ocena);
                updateGridIzvedba();
            }
        }

    }
}