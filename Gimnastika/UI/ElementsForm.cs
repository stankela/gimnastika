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
        private List<Element> elementi;

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

                    elementi = new List<Element>(DAOFactoryFactory.DAOFactory.GetElementDAO().FindAll());
                    elementBrowserControl1.setElementi(elementi);
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
            elementBrowserControl1.DataGridViewUserControl.DataGridView.MultiSelect = false;
            elementBrowserControl1.DataGridViewUserControl.DataGridView.AutoGenerateColumns = false;
            elementBrowserControl1.DataGridViewUserControl.DataGridView.GridColor = Color.Black;
            elementBrowserControl1.DataGridViewUserControl.DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            ElementForm form = new ElementForm(null, elementBrowserControl1.selectedSprava(),
                null, true);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            elementi.Add(form.Element);
            elementBrowserControl1.setElementi(elementi);
            elementBrowserControl1.selektuj(form.Element);
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            IList<Element> selItems = elementBrowserControl1.DataGridViewUserControl
                .getSelectedItems<Element>();
            if (selItems.Count != 1)
                return;

            Element element = selItems[0];
            int index = elementi.IndexOf(element);
            ElementForm form = new ElementForm(element.Id, element.Sprava,
                element.Parent, true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                elementi[index] = form.Element;
                elementBrowserControl1.setElementi(elementi);
                elementBrowserControl1.DataGridViewUserControl.refreshItems();
                elementBrowserControl1.selektuj(form.Element);
            }
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            IList<Element> selItems = elementBrowserControl1.DataGridViewUserControl
                .getSelectedItems<Element>();
            if (selItems.Count == 0)
                return;

            bool delete;
            if (selItems.Count == 1)
            {
                delete = MessageDialogs.queryConfirmation(
                    deleteConfirmationMessage(selItems[0]), this.Text);
            }
            else
            {
                delete = MessageDialogs.queryConfirmation(
                    deleteConfirmationMessage(), this.Text);
            }
            if (!delete)
                return;

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    foreach (Element element in selItems)
                    {
                        DAOFactoryFactory.DAOFactory.GetElementDAO().MakeTransient(element);
                    }
                    session.Transaction.Commit();
                    foreach (Element element in selItems)
                    {
                        elementi.Remove(element);
                    }
                    elementBrowserControl1.setElementi(elementi);
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private string deleteConfirmationMessage(Element e)
        {
            return String.Format("Da li zelite da izbrisete element \"{0}\"?", e);
        }

        private string deleteConfirmationMessage()
        {
            return String.Format("Da li zelite da izbrisete selektovane elemente?");
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}