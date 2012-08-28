using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;
using Gimnastika.Dao;
using NHibernate;
using Gimnastika.Data;
using NHibernate.Context;

namespace Gimnastika.UI
{
    public partial class PravilaForm : Form
    {
        List<PraviloOceneVezbe> pravila;

        public PravilaForm()
        {
            InitializeComponent();
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    pravila = new List<PraviloOceneVezbe>(DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO().FindAll());
                    initUI();

                    cmbPravila.SelectedIndexChanged += cmbPravila_SelectedIndexChanged;
                    updatePravilaDetails();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void initUI()
        {
            cmbPravila.DataSource = pravila;
            cmbPravila.DisplayMember = "Naziv";
            cmbPravila.ValueMember = "Id";
            cmbPravila.SelectedIndex = -1;
            cmbPravila.DropDownStyle = ComboBoxStyle.DropDownList;
            if (pravila.Count > 0)
            {
                // TODO: Ovde treba selektovati podrazumevano pravilo
                cmbPravila.SelectedIndex = 0;
            }

            this.Text = "Pravila";

            txtBrojBodovanih.ReadOnly = true;
            txtBrojBodovanih.TabStop = false;
            txtMaxIstaGrupa.ReadOnly = true;
            txtMaxIstaGrupa.TabStop = false;

            setupGrid();
        }

        private void setupGrid()
        {
            gridIzvedba.MultiSelect = false;
            gridIzvedba.AllowUserToAddRows = false;
            gridIzvedba.AllowUserToDeleteRows = false;
            gridIzvedba.AllowUserToResizeRows = false;
            gridIzvedba.AutoGenerateColumns = false;
            gridIzvedba.ReadOnly = true;

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

        private void updatePravilaDetails()
        {
            if (cmbPravila.SelectedIndex != -1)
            {
                PraviloOceneVezbe pravilo = (PraviloOceneVezbe)cmbPravila.SelectedItem;
                txtBrojBodovanih.Text = pravilo.BrojBodovanihElemenata.ToString();
                txtMaxIstaGrupa.Text = pravilo.MaxIstaGrupa.ToString();
                updateGridIzvedba();
            }
        }

        private void updateGridIzvedba()
        {
            if (cmbPravila.SelectedIndex != -1)
            {
                PraviloOceneVezbe pravilo = (PraviloOceneVezbe)cmbPravila.SelectedItem;
                gridIzvedba.Rows.Clear();
                pravilo.sortirajPocetneOceneIzvedbe();
                foreach (PocetnaOcenaIzvedbe ocena in pravilo.PocetneOceneIzvedbe)
                {
                    // ovo ne generise CellValueChanged event
                    gridIzvedba.Rows.Add(getRowValues(ocena));
                }
            }
        }

        private object[] getRowValues(PocetnaOcenaIzvedbe ocena)
        {
            return new object[] { ocena.OpsegString, ocena.PocetnaOcena };
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            PraviloForm f = new PraviloForm(null);
            if (f.ShowDialog() == DialogResult.OK)
            {
                pravila.Add((PraviloOceneVezbe)f.Entity);
                refreshCombo();
            }
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            if (cmbPravila.SelectedIndex != -1)
            {
                PraviloOceneVezbe pravilo = (PraviloOceneVezbe)cmbPravila.SelectedItem;
                PraviloForm f = new PraviloForm(pravilo.Id);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    pravila[pravila.IndexOf(pravilo)] = (PraviloOceneVezbe)f.Entity;
                    refreshCombo();
                    updatePravilaDetails();
                }
            }
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            if (cmbPravila.SelectedIndex == -1)
                return;

            if (cmbPravila.Items.Count == 1)
            {
                MessageBox.Show("Selektovana pravila su jedina postojeca pravila i nije ih moguce izbrisati. " +
                    "Ukoliko zelite da izbrisete data pravila, najpre dodajte nova pravila.", "Poruka", MessageBoxButtons.OK,
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                return;
            }
            
            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)cmbPravila.SelectedItem;
            if (MessageBox.Show("Da li zelite da izbrisete pravila '" +
                pravilo.Naziv + "' ?", "Potvrda", MessageBoxButtons.OKCancel,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    // TODO: Ovde prvo treba pitati sta treba raditi sa vezbama
                    // za dato pravilo (da li ih brisati ili ne). Ako je odgovor
                    // ne, ne treba brisati ni pravilo)

                    bool delete = true;
                    IList<Vezba> vezbe = DAOFactoryFactory.DAOFactory.GetVezbaDAO().FindAll();
                    foreach (Vezba v in vezbe)
                    {
                        if (v.Pravilo != null && v.Pravilo.Id == pravilo.Id)
                        {
                            delete = false;
                            break;
                        }
                    }

                    if (delete)
                    {
                        foreach (Vezba v in vezbe)
                        {
                            DAOFactoryFactory.DAOFactory.GetVezbaDAO().Evict(v);
                        }
                        DAOFactoryFactory.DAOFactory.GetPraviloOceneVezbeDAO().MakeTransient(pravilo);
                        session.Transaction.Commit();

                        pravila.Remove(pravilo);
                        refreshCombo();
                        updatePravilaDetails();
                    }
                    else
                    {
                        MessageBox.Show("Nije dozvoljeno brisanje pravila za koja postoje vezbe. Da biste izbrisali pravila, prvo morate da izbrisete sve vezbe za data pravila.", "Poruka");
                    }
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void refreshCombo()
        {
            CurrencyManager currencyManager =
                (CurrencyManager)this.BindingContext[cmbPravila.DataSource];
            currencyManager.Refresh();
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbPravila_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePravilaDetails();
        }
    }
}