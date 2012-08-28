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

namespace Gimnastika.UI
{
    public partial class ElementsForm : Form
    {
        private BindingListView<Element> elementi = null;

        public ElementsForm()
        {
            InitializeComponent();
            initUI();

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    elementi = new BindingListView<Element>(
                        new List<Element>(DAOFactoryFactory.DAOFactory.GetElementDAO().FindAll()));
                    elementBrowserControl1.Elementi = elementi;
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void initUI()
        {
            this.Text = "Elementi";
            setupGrid();
        }

        private void setupGrid()
        {
            elementBrowserControl1.gridViewElementi.MultiSelect = false;
            elementBrowserControl1.gridViewElementi.AllowUserToAddRows = false;
            elementBrowserControl1.gridViewElementi.AllowUserToDeleteRows = false;
            elementBrowserControl1.gridViewElementi.AllowUserToResizeRows = false;
            elementBrowserControl1.gridViewElementi.AutoGenerateColumns = false;
            elementBrowserControl1.gridViewElementi.GridColor = Color.Black;
            elementBrowserControl1.gridViewElementi.ReadOnly = true;
            elementBrowserControl1.gridViewElementi.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "NazivString";
            column.Name = "Naziv";
            column.HeaderText = "Naziv";
            column.Width = 310;
            elementBrowserControl1.gridViewElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Sprava";
            column.Name = "Sprava";
            column.HeaderText = "Sprava";
            column.Width = 70;
            elementBrowserControl1.gridViewElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Tezina";
            column.Name = "Tezina";
            column.HeaderText = "Tezina";
            column.Width = 50;
            elementBrowserControl1.gridViewElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "GrupaBroj";
            column.Name = "GrupaBroj";
            column.HeaderText = "Broj u tablicama";
            column.Width = 60;
            elementBrowserControl1.gridViewElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Vrednost";
            column.Name = "Vrednost";
            column.HeaderText = "Vrednost";
            column.ReadOnly = true;
            column.DefaultCellStyle.Format = "F2";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 60;
            elementBrowserControl1.gridViewElementi.Columns.Add(column);
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            ElementForm form = new ElementForm(null, elementBrowserControl1.selectedSprava(),
                null, true);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            elementi.Add(form.Element);
            //refreshGrid();
            elementBrowserControl1.selektuj(elementi.Count - 1);
        }

        private void refreshGrid()
        {
            CurrencyManager currencyManager =
                (CurrencyManager)this.BindingContext[elementBrowserControl1.gridViewElementi.DataSource];
            currencyManager.Refresh();
            elementBrowserControl1.gridViewElementi.Refresh();
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            if (elementBrowserControl1.gridViewElementi.Rows.Count == 0)
                return;

            Element element = elementi[elementBrowserControl1.gridViewElementi.CurrentRow.Index];
            ElementForm form = new ElementForm(element.Id, element.Sprava,
                element.Parent, true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                elementi[elementBrowserControl1.gridViewElementi.CurrentRow.Index] = form.Element;
                refreshGrid();
            }
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            if (elementBrowserControl1.gridViewElementi.Rows.Count == 0)
                return;

            Element element = elementi[elementBrowserControl1.gridViewElementi.CurrentRow.Index];
            if (MessageBox.Show("Da li zelite da izbrisete element '" +
                element.ToString() + "' ?", "Potvrda", MessageBoxButtons.OKCancel,
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

                    DAOFactoryFactory.DAOFactory.GetElementDAO().MakeTransient(element);
                    session.Transaction.Commit();

                    elementi.RemoveAt(elementBrowserControl1.gridViewElementi.CurrentRow.Index);
                    //              refreshGrid();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}