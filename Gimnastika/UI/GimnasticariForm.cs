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
    public partial class GimnasticariForm : EntityListForm
    {
        private const string IME = "Ime";
        private const string PREZIME = "Prezime";

        public GimnasticariForm()
        {
            InitializeComponent();
            initialize(typeof(Gimnasticar));
            sort(PREZIME);
        }

        protected override DataGridView getDataGridView()
        {
            return gridView;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Gimnasticari";
        }

        protected override void addGridColumns()
        {
            AddColumn("Ime", IME, 100);
            AddColumn("Prezime", PREZIME, 100);
        }

        protected override List<object> loadEntities()
        {
            GimnasticarDAO gimnasticarDAO = DAOFactoryFactory.DAOFactory.GetGimnasticarDAO();
            return new List<Gimnasticar>(gimnasticarDAO.FindAll()).ConvertAll<object>(
                delegate(Gimnasticar g)
                {
                    return g;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new GimnasticarForm(entityId);
        }

        private void btnNovi_Click(object sender, EventArgs e)
        {
            addCommand();
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            editCommand();
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            deleteCommand();
        }

        protected override string deleteConfirmationMessage(DomainObject entity)
        {
            return String.Format("Da li zelite da izbrisete gimnasticara \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            Gimnasticar g = (Gimnasticar)entity;
            VezbaDAO vezbaDao = DAOFactoryFactory.DAOFactory.GetVezbaDAO();

            // TODO: Ovde prvo treba pitati sta treba raditi sa vezbama
            // za datog gimnasticara (da li ih brisati ili ih ostaviti
            // ali bez gimnasticara)
            
            if (vezbaDao.existsVezbaGimnasticar(g))
            {
                string msg = "Gimnasticara '{0}' nije moguce izbrisati zato sto postoje " +
                    "vezbe za datog gimnasticara. \nMorate najpre da izbrisete vezbe " +
                    "da biste mogli da izbrisete gimnsticara.";
                MessageDialogs.showMessage(String.Format(msg, g), this.Text);
                return false;
            }
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            GimnasticarDAO gimnasticarDAO = DAOFactoryFactory.DAOFactory.GetGimnasticarDAO();
            gimnasticarDAO.MakeTransient((Gimnasticar)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja gimnasticara.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}