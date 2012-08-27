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
using NHibernate;
using Gimnastika.Data;
using NHibernate.Context;
using Gimnastika.UI;

namespace Gimnastika
{
    public partial class PraviloForm : EntityDetailForm
    {
        private string oldNaziv;

        public PraviloForm(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new PraviloOceneVezbe();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO().FindById(id);
        }

        protected override void initUI()
        {
            base.initUI();
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

        protected override void saveOriginalData(DomainObject entity)
        {
            PraviloOceneVezbe p = (PraviloOceneVezbe)entity;
            oldNaziv = p.Naziv;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
            txtNazivPravila.Text = pravilo.Naziv;
            txtBrojBodovanih.Text = pravilo.BrojBodovanihElemenata.ToString();
            txtMaxIstaGrupa.Text = pravilo.MaxIstaGrupa.ToString();
            updateGridIzvedba();
        }

        private void updateGridIzvedba()
        {
            gridIzvedba.Rows.Clear();
            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
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
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            int dummyInt;
            if (txtNazivPravila.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Naziv", "Unesite naziv pravila.");
            }
            
            if (txtBrojBodovanih.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "BrojBodovanih", "Unesite broj elemenata koji se boduju.");
            }
            else if (!int.TryParse(txtBrojBodovanih.Text, out dummyInt))
            {
                notification.RegisterMessage(
                    "BrojBodovanih", "Nepravilan format za broj elemenata koji se boduju.");
            }
            
            if (txtMaxIstaGrupa.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "MaxIstaGrupa", "Unesite maksimalan broj elemenata iz iste grupe " +
                    "koji se boduju.");
            }
            else if (!int.TryParse(txtMaxIstaGrupa.Text, out dummyInt))
            {
                notification.RegisterMessage(
                    "MaxIstaGrupa", "Nepravilan format za maksimalan broj elemenata iz iste grupe koji se boduju.");
            }
            
            if (gridIzvedba.Rows.Count == 0)
            {
                notification.RegisterMessage(
                    "PocetneOceneIzvedbe", "Unesite pocetne ocene za izvedbu.");
            }
        }

        protected override void setFocus(string propertyName)
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

        protected override void updateEntityFromUI(DomainObject entity)
        {
            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
            pravilo.Naziv = txtNazivPravila.Text.Trim();
            pravilo.BrojBodovanihElemenata = int.Parse(txtBrojBodovanih.Text);
            pravilo.MaxIstaGrupa = int.Parse(txtMaxIstaGrupa.Text);
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
            Notification notification = new Notification();

            PraviloOceneVezbeDAO praviloOceneVezbeDAO = DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO();
            if (praviloOceneVezbeDAO.postojiPravilo(pravilo.Naziv))
            {
                notification.RegisterMessage("Naziv", "Pravilo sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO().MakePersistent((PraviloOceneVezbe)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
            Notification notification = new Notification();

            PraviloOceneVezbeDAO praviloOceneVezbeDAO = DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO();
            bool nazivChanged = (pravilo.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && praviloOceneVezbeDAO.postojiPravilo(pravilo.Naziv))
            {
                notification.RegisterMessage("Naziv", "Pravilo sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO().MakePersistent((PraviloOceneVezbe)entity);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            handleCancelClick();
        }

        private void btnDodajOcenu_Click(object sender, EventArgs e)
        {
            PocetnaOcenaForm f = new PocetnaOcenaForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
                    pravilo.dodajPocetnuOcenuIzvedbe((PocetnaOcenaIzvedbe)f.Entity);
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
                PraviloOceneVezbe pravilo = (PraviloOceneVezbe)entity;
                PocetnaOcenaIzvedbe ocena = pravilo.PocetneOceneIzvedbe[gridIzvedba.CurrentRow.Index];
                pravilo.ukloniPocetnuOcenuIzvedbe(ocena);
                updateGridIzvedba();
            }
        }

    }
}