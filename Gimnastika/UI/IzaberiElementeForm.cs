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
    public partial class IzaberiElementeForm : Form
    {
        private List<Element> elementi;
        private List<Element> izabrani = new List<Element>();

        public List<Element> IzabraniElementi
        {
            get { return izabrani; }
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

                    elementi = new List<Element>(DAOFactoryFactory.DAOFactory.GetElementDAO().FindAll());
                    elementBrowserControl1.setElementi(elementi);
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

            setupGrid();
        }

        private void setupGrid()
        {
            elementBrowserControl1.DataGridViewUserControl.DataGridView.MultiSelect = true;
            elementBrowserControl1.DataGridViewUserControl.DataGridView.AutoGenerateColumns = false;
            elementBrowserControl1.DataGridViewUserControl.DataGridView.GridColor = Color.Black;
            elementBrowserControl1.DataGridViewUserControl.DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            ElementForm form = new ElementForm(null, elementBrowserControl1.selectedSprava(),
                null, true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Element.Sprava == elementBrowserControl1.selectedSprava())
                {
                    // TODO: Razmisli sta bi trebalo raditi kada element nije u skladu sa filterom

                    elementi.Add(form.Element);
                    elementBrowserControl1.setElementi(elementi);
                    elementBrowserControl1.selektuj(form.Element);
                }
                else
                {
                    MessageBox.Show("Mozete da dodate element samo za spravu " +
                        "za koju birate elemente.", "Greska");
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (elementBrowserControl1.DataGridViewUserControl.DataGridView.SelectedRows.Count == 0)
            {
                DialogResult = DialogResult.None;
                return;
            }
            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in elementBrowserControl1.DataGridViewUserControl.DataGridView.SelectedRows)
            {
                selectedRows.Add(row);
            }
            // NOTE: DataGridView.SelectedRows sadrzi selektovane vrste po obrnutom
            // vremenskom redosledu selekcije (zadnja vremenski selektovana vrsta
            // nalazi se na prvom mestu). Potrebno ih je sortirati po broju vrste.
            selectedRows.Sort(
                delegate(DataGridViewRow r1, DataGridViewRow r2)
                {
                    return r1.Index.CompareTo(r2.Index);
                }
            );

            Sprava sprava = elementBrowserControl1.selectedSprava();
            foreach (DataGridViewRow row in selectedRows)
            {
                Element elem = (Element)row.DataBoundItem;
                if (elem.Sprava == sprava)
                    izabrani.Add(elem);
            }
        }
   }
}