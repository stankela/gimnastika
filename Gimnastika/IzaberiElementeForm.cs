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

namespace Gimnastika
{
    public partial class IzaberiElementeForm : Form
    {
        private BindingListView<Element> elementi;
        private Dictionary<int, Element> izabrani = new Dictionary<int,Element>();

        Color izabraniBackColor = SystemColors.Highlight;
        Color izabraniForeColor = SystemColors.Window;

        public List<Element> IzabraniElementi
        {
            get
            {
                List<Element> result = new List<Element>();
                Dictionary<int, Element>.ValueCollection values =
                   izabrani.Values;
                Sprava sprava = elementBrowserControl1.selectedSprava();
                foreach (Element e in values)
                {
                    if (e.Sprava == sprava)
                        result.Add(e);
                }
                return result;
            }
        }

        public IzaberiElementeForm(Sprava sprava)
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
                    if (sprava != Sprava.Undefined)
                        elementBrowserControl1.restrictSprava(sprava);
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void initUI()
        {
            this.Text = "Izbor elemenata";

            elementBrowserControl1.gridViewElementi.CellClick +=
                new DataGridViewCellEventHandler(gridViewElementi_CellClick);
            elementBrowserControl1.gridViewElementi.CellFormatting +=
                new DataGridViewCellFormattingEventHandler(gridViewElementi_CellFormatting);
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

        private void gridViewElementi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = (DataGridView)sender;
                IBindingListView listView = (IBindingListView)grid.DataSource;
                Element elem = (Element)listView[e.RowIndex];
                DataGridViewRow row = grid.Rows[e.RowIndex];
                try
                {
                    izabrani.Add(elem.Id, elem);
                    row.DefaultCellStyle.BackColor = izabraniBackColor;
                    row.DefaultCellStyle.ForeColor = izabraniForeColor;
                }
                catch (ArgumentException)
                {
                    // elem vec postoji
                    izabrani.Remove(elem.Id);
                    row.DefaultCellStyle.BackColor = grid.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = grid.DefaultCellStyle.ForeColor;
                }
            }
        }

        private void gridViewElementi_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = (DataGridView)sender;
                IBindingListView listView = (IBindingListView)grid.DataSource;
                Element elem = (Element)listView[e.RowIndex];
                DataGridViewRow row = grid.Rows[e.RowIndex];
                if (row.Selected)
                {
                    if (izabrani.ContainsKey(elem.Id))
                    {
                        e.CellStyle.SelectionBackColor = izabraniBackColor;
                        e.CellStyle.SelectionForeColor = izabraniForeColor;
                    }
                    else
                    {
                        e.CellStyle.SelectionBackColor = grid.DefaultCellStyle.BackColor;
                        e.CellStyle.SelectionForeColor = grid.DefaultCellStyle.ForeColor;
                    }
                }
                else
                {
                    if (izabrani.ContainsKey(elem.Id))
                    {
                        e.CellStyle.BackColor = izabraniBackColor;
                        e.CellStyle.ForeColor = izabraniForeColor;
                    }
                    else
                    {
                        e.CellStyle.BackColor = grid.DefaultCellStyle.BackColor;
                        e.CellStyle.ForeColor = grid.DefaultCellStyle.ForeColor;
                    }
                }
            }
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            ElementForm form = new ElementForm(null, elementBrowserControl1.selectedSprava(),
                null, true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Element.Sprava == elementBrowserControl1.selectedSprava())
                {
                    elementi.Add(form.Element);
                    //refreshGrid();

                    /*    
                    // Oznaci novi element
                    izabrani.Add(form.Element.Id, form.Element);
                    DataGridViewRow row = elementBrowserControl1.gridViewElementi.Rows[elementi.Count - 1];
                    row.DefaultCellStyle.BackColor = izabraniBackColor;
                    row.DefaultCellStyle.ForeColor = izabraniForeColor;
                    */

                    elementBrowserControl1.selektuj(elementi.Count - 1);
                }
                else
                {
                    MessageBox.Show("Mozete da dodate element samo za spravu " +
                        "za koju birate elemente.", "Greska");
                }
            }
        }
   }
}