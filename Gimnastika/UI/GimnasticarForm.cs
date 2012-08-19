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

namespace Gimnastika.UI
{
    public partial class GimnasticarForm : EntityDetailForm
    {
        private string oldIme;
        private string oldPrezime;

        public GimnasticarForm(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Gimnasticar();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetGimnasticarDAO().FindById(id);
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Novi gimnasticar";

            txtIme.Text = String.Empty;
            txtPrezime.Text = String.Empty;
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            Gimnasticar g = (Gimnasticar)entity;
            oldIme = g.Ime;
            oldPrezime = g.Prezime;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Gimnasticar g = (Gimnasticar)entity;
            this.Text = "Gimnasticar: " + g.ToString();
            txtIme.Text = g.Ime;
            txtPrezime.Text = g.Prezime;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            if (txtIme.Text.Trim() == String.Empty && txtPrezime.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Ime", "Ime ili prezime je obavezno.");
            }
        }

        protected override void setFocus(string propertyName)
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
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            Gimnasticar g = (Gimnasticar)entity;
            g.Ime = txtIme.Text.Trim();
            g.Prezime = txtPrezime.Text.Trim();
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Gimnasticar g = (Gimnasticar)entity;
            Notification notification = new Notification();

            GimnasticarDAO gimnasticarDAO = DAOFactoryFactory.DAOFactory.GetGimnasticarDAO();
            if (gimnasticarDAO.postojiGimnasticar(g.Ime, g.Prezime))
            {
                notification.RegisterMessage("Ime", "Gimnasticar sa datim imenom i prezimenom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetGimnasticarDAO().MakePersistent((Gimnasticar)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Gimnasticar g = (Gimnasticar)entity;
            Notification notification = new Notification();

            GimnasticarDAO gimnasticarDAO = DAOFactoryFactory.DAOFactory.GetGimnasticarDAO();
            bool imeChanged = (g.Ime.ToUpper() != oldIme.ToUpper()) ? true : false;
            bool prezimeChanged = (g.Prezime.ToUpper() != oldPrezime.ToUpper()) ? true : false;
            if ((imeChanged || prezimeChanged) && gimnasticarDAO.postojiGimnasticar(g.Ime, g.Prezime))
            {
                notification.RegisterMessage("Ime", "Gimnasticar sa datim imenom i prezimenom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetGimnasticarDAO().MakePersistent((Gimnasticar)entity);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            handleCancelClick();
        }
    }
}