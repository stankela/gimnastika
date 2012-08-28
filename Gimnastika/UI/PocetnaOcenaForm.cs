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
using Gimnastika.UI;

namespace Gimnastika.UI
{
    public partial class PocetnaOcenaForm : EntityDetailForm
    {
        public PocetnaOcenaForm()
        {
            InitializeComponent();
            // radi samo u add modu, a entity se ne snima
            initialize(null, false);
        }

        protected override DomainObject createNewEntity()
        {
            return new PocetnaOcenaIzvedbe();
        }

        protected override void initUI()
        {
            base.initUI();

            Text = "Pocetna ocena";
            lblNapomena.Text = "Napomena: Maksimalan broj elemenata moze da se izostavi. " +
                "Ukoliko se izostavi, smatra se da pocetna ocena vazi za broj elemenata od minimalnog " +
                "pa navise.";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            int dummyInt;
            float dummyFloat;

            if (txtMin.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "MinBrojElemenata", "Unesite vrednost za minimalan broj elemenata.");
            }
            else if (!int.TryParse(txtMin.Text, out dummyInt))
            {
                notification.RegisterMessage(
                    "MinBrojElemenata", "Nepravilna vrednost za minimalan broj elemenata.");
            }

            if (txtOcena.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "PocetnaOcena", "Unesite vrednost za ocenu.");
            }
            else if (!float.TryParse(txtOcena.Text.Replace(',', '.'), out dummyFloat))
            {
                notification.RegisterMessage(
                    "PocetnaOcena", "Nepravilna vrednost za pocetnu ocenu.");
            }

            if (txtMax.Text.Trim() != "")
            {
                if (!int.TryParse(txtMax.Text, out dummyInt))
                {
                    notification.RegisterMessage(
                        "MaxBrojElemenata", "Nepravilna vrednost za maksimalan broj elemenata.");
                }
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "MinBrojElemenata":
                    txtMin.Focus();
                    break;

                case "MaxBrojElemenata":
                    txtMax.Focus();
                    break;

                case "PocetnaOcena":
                    txtOcena.Focus();
                    break;

                default:
                    break;
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            PocetnaOcenaIzvedbe pocOcena = (PocetnaOcenaIzvedbe)entity;
            if (txtMax.Text.Trim() != "")
            {
                pocOcena.MinBrojElemenata = int.Parse(txtMin.Text);
                pocOcena.MaxBrojElemenata = int.Parse(txtMax.Text);
                pocOcena.PocetnaOcena = float.Parse(txtOcena.Text.Replace(',', '.'));
            }
            else
            {
                pocOcena.MinBrojElemenata = int.Parse(txtMin.Text);
                pocOcena.MaxBrojElemenata = PocetnaOcenaIzvedbe.MAX_LIMIT;
                pocOcena.PocetnaOcena = float.Parse(txtOcena.Text.Replace(',', '.'));
            }
        }
    }
}