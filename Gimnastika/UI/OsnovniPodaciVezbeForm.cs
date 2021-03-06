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
    public partial class OsnovniPodaciVezbeForm : Form
    {
        List<PraviloOceneVezbe> pravila;
        private Gimnasticar gimnasticar;
        private Sprava sprava;
        private PraviloOceneVezbe pravilo;
        private string naziv;

        public string Naziv
        {
            get { return naziv; }
        }

        public Gimnasticar Gimnasticar
        {
            get { return gimnasticar; }
        }

        public Sprava Sprava
        {
            get { return sprava; }
        }

        public PraviloOceneVezbe Pravilo
        {
            get { return pravilo; }
        }

        public OsnovniPodaciVezbeForm()
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
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void initUI()
        {
            this.Text = "Osnovni podaci o vezbi";

            cmbGimnasticar.DataSource = DAOFactoryFactory.DAOFactory.GetGimnasticarDAO().FindAll();
            cmbGimnasticar.DisplayMember = "PrezimeIme";
            cmbGimnasticar.ValueMember = "Id";
            cmbGimnasticar.SelectedIndex = -1;
            cmbGimnasticar.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbSprava.DataSource = Resursi.SpravaNazivTable;
            cmbSprava.DisplayMember = "Naziv";
            cmbSprava.ValueMember = "Sprava";
            cmbSprava.SelectedIndex = -1;
            cmbSprava.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbPravila.DataSource = pravila;
            cmbPravila.DisplayMember = "Naziv";
            cmbPravila.ValueMember = "Id";
            cmbPravila.SelectedIndex = -1;
            cmbPravila.DropDownStyle = ComboBoxStyle.DropDownList;

            txtNaziv.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    if (!validateDialog())
                    {
                        DialogResult = DialogResult.None;
                        return;
                    }
                    if (cmbGimnasticar.SelectedIndex != -1)
                        gimnasticar = DAOFactoryFactory.DAOFactory.GetGimnasticarDAO()
                            .FindById(((Gimnasticar)cmbGimnasticar.SelectedItem).Id);
                    else
                        gimnasticar = null;
                    sprava = (Sprava)cmbSprava.SelectedValue;
                    pravilo = cmbPravila.SelectedItem as PraviloOceneVezbe;
                    naziv = txtNaziv.Text.Trim();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private bool validateDialog()
        {
            if (!requiredFieldsValidation())
                return false;

            if (txtNaziv.Text.Trim().Length > Vezba.NAZIV_MAX_LENGTH)
            {
                MessageBox.Show("Naziv vezbe moze da sadrzi maksimalno "
                    + Vezba.NAZIV_MAX_LENGTH + " znakova.", "Greska");
                txtNaziv.Focus();
                return false;
            }
            return true;
        }

        private bool requiredFieldsValidation()
        {
            if (cmbSprava.SelectedIndex == -1)
            {
                MessageBox.Show("Izaberite spravu.", "Greska");
                cmbSprava.Focus();
                return false;
            }
            if (cmbPravila.SelectedIndex == -1)
            {
                MessageBox.Show("Izaberite pravila ocenjivanja.", "Greska");
                cmbPravila.Focus();
                return false;
            }
            if (txtNaziv.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite naziv vezbe.", "Greska");
                txtNaziv.Focus();
                return false;
            }
            if (cmbGimnasticar.SelectedIndex == -1)
            {
                if (MessageBox.Show("Niste izabrali gimnasticara. Da li zelite da " +
                    "ostane tako?", "Potvrda", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                {
                    cmbGimnasticar.Focus();
                    return false;
                }
            }
            return true;
        }

        public void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNaziv.Focus();
                    break;

                default:
                    break;
            }
        }

    }
}