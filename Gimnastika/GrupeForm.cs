using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Domain;
using Gimnastika.Dao;
using Gimnastika.Exceptions;
using NHibernate;
using Gimnastika.Data;
using NHibernate.Context;

namespace Gimnastika.UI
{
    public partial class GrupeForm : Form
    {
        private Sprava sprava;
        private GrupaElementa grupaElementa;
        Grupa grupa;
        private bool editMode;
        IList<Grupa> grupe;
        private bool closedByOK;
        private bool closedByCancel;

        string oldNaziv;
        string oldEngNaziv;

        public GrupeForm()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    grupe = DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll();
                    initUI();
                    showEntityDetails();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void showEntityDetails()
        {
            grupa = findGrupa(sprava, grupaElementa);
            if (grupa == null)
            {
                grupa = new Grupa(sprava, grupaElementa, "", "");
                editMode = false;
            }
            else
            {
                editMode = true;
                oldNaziv = grupa.Naziv;
                oldEngNaziv = grupa.EngNaziv;
            }
            txtNaziv.Text = grupa.Naziv;
            txtEngNaziv.Text = grupa.EngNaziv;
        }

        private bool updateEntityFromUI()
        {
            try
            {
                grupa.Naziv = txtNaziv.Text.Trim();
                grupa.EngNaziv = txtEngNaziv.Text.Trim();
                if (!grupa.validate())
                    return false;

                GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
                if (editMode)
                {
                    if (grupa.Naziv != oldNaziv || grupa.EngNaziv != oldEngNaziv)
                    {
                        grupaDAO.MakePersistent(grupa);
                        NHibernateHelper.GetCurrentSession().Transaction.Commit();
                        oldNaziv = grupa.Naziv;
                        oldEngNaziv = grupa.EngNaziv;
                    }
                }
                else
                {
                    grupaDAO.MakePersistent(grupa);
                    NHibernateHelper.GetCurrentSession().Transaction.Commit();
                    grupe.Add(grupa);
                    editMode = true;
                    oldNaziv = grupa.Naziv;
                    oldEngNaziv = grupa.EngNaziv;
                }
                return true;
            }
            catch (InvalidPropertyException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
                setFocus(ex.InvalidProperty);
                return false;
            }
            catch (BusinessException ex)
            {
                if (ex.Notification != null)
                {
                    NotificationMessage msg = ex.Notification.FirstMessage;
                    MessageDialogs.showMessage(msg.Message, this.Text);
                    setFocus(msg.FieldName);
                }
                else
                {
                    MessageDialogs.showMessage(ex.Message, this.Text);
                }
                return false;
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
                return false;
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), this.Text);
                return false;
            }
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNaziv.Focus();
                    break;
                case "EngNaziv":
                    txtEngNaziv.Focus();
                    break;

                default:
                    break;
            }
        }
        private Grupa findGrupa(Sprava sprava, GrupaElementa grupaElementa)
        {
            foreach (Grupa g in grupe)
            {
                if (g.Sprava == sprava && g.GrupaElemenata == grupaElementa)
                    return g;
            }
            return null;
        }

        private void initUI()
        {
            Text = "Nazivi grupa";

            cmbSprava.Items.Clear();
            cmbSprava.Items.AddRange(Resursi.SpravaNazivTable);
            cmbSprava.SelectedIndex = 0;
            sprava = selectedSprava();

            cmbGrupa.Items.Clear();
            cmbGrupa.Items.AddRange(Resursi.GrupaNazivTable);
            cmbGrupa.SelectedIndex = 0;
            grupaElementa = selectedGrupa();

            txtNaziv.Text = "";
            txtEngNaziv.Text = "";

            cmbSprava.SelectedIndexChanged += new EventHandler(cmbSprava_SelectedIndexChanged);
            cmbGrupa.SelectedIndexChanged += new EventHandler(cmbGrupa_SelectedIndexChanged);
        }

        void cmbGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            onSelectedEntityChanged();
        }

        private void onSelectedEntityChanged()
        {
            bool updated = false;
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    updated = updateEntityFromUI();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }

            if (updated)
            {
                grupaElementa = selectedGrupa();
                sprava = selectedSprava();
                if (sprava == Sprava.Parter && selectedGrupa() == GrupaElementa.V)
                {
                    grupaElementa = GrupaElementa.I;
                    disableComboHandlers();
                    setGrupaCombo(grupaElementa);
                    enableComboHandlers();
                }
                showEntityDetails();
            }
            else
            {
                disableComboHandlers();
                setSpravaCombo(sprava);
                setGrupaCombo(grupaElementa);
                enableComboHandlers();
            }
        }

        private void setGrupaCombo(GrupaElementa grupaElementa)
        {
            cmbSprava.SelectedIndex = grupaElementa - GrupaElementa.I;
        }

        private void setSpravaCombo(Sprava sprava)
        {
            cmbSprava.SelectedIndex = sprava - Sprava.Parter;
        }

        private void enableComboHandlers()
        {
            cmbSprava.SelectedIndexChanged += new EventHandler(cmbSprava_SelectedIndexChanged);                
            cmbGrupa.SelectedIndexChanged += new EventHandler(cmbGrupa_SelectedIndexChanged);
        }

        private void disableComboHandlers()
        {
            cmbSprava.SelectedIndexChanged -= cmbSprava_SelectedIndexChanged;
            cmbGrupa.SelectedIndexChanged -= cmbGrupa_SelectedIndexChanged;
        }

        void cmbSprava_SelectedIndexChanged(object sender, EventArgs e)
        {
            onSelectedEntityChanged();
        }

        private void cmbGrupa_DropDown(object sender, EventArgs e)
        {
            GrupaElementa lastGrupa =
                ((GrupaNazivPair)cmbGrupa.Items[cmbGrupa.Items.Count - 1]).Grupa;
            if (selectedSprava() == Sprava.Parter)
            {
                if (lastGrupa == GrupaElementa.V)
                    cmbGrupa.Items.RemoveAt(cmbGrupa.Items.Count - 1);
            }
            else
            {
                if (lastGrupa == GrupaElementa.IV)
                    cmbGrupa.Items.Add(new GrupaNazivPair(GrupaElementa.V, "V"));
            }
        }

        public Sprava selectedSprava()
        {
            return ((SpravaNazivPair)cmbSprava.SelectedItem).Sprava;
        }

        private GrupaElementa selectedGrupa()
        {
            return ((GrupaNazivPair)cmbGrupa.SelectedItem).Grupa;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool updated = false;
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    updated = updateEntityFromUI();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }

            if (!updated)
                DialogResult = DialogResult.None;
            else
                closedByOK = true;
        }

        private void GrupeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closedByOK || closedByCancel)
                return;
            
            // zatvoreno pomocu X
            bool updated = false;
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    updated = updateEntityFromUI();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }

            e.Cancel = !updated;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            closedByCancel = true;
        }
    }
}