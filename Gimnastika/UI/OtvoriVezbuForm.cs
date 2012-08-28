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
    public partial class OtvoriVezbuForm : EntityListForm
    {
        private int vezbaId;
        public int VezbaId
        {
            get { return vezbaId; }
        }

        public OtvoriVezbuForm()
        {
            InitializeComponent();
            initialize(typeof(Vezba));
            //sort(PREZIME);
        }

        protected override DataGridView getDataGridView()
        {
            return gridView;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Otvori vezbu";
        }

        protected override void addGridColumns()
        {
            AddColumn("Naziv vezbe", "Naziv", 200);
            AddColumn("Sprava", "Sprava", 70);
            AddColumn("Gimnasticar", "Gimnasticar", 100);
            AddColumn("Pravila", "Pravilo", 100);
        }

        protected override List<object> loadEntities()
        {
            VezbaDAO vezbaDAO = DAOFactoryFactory.DAOFactory.GetVezbaDAO();
            return new List<Vezba>(vezbaDAO.FindAll()).ConvertAll<object>(
                delegate(Vezba v)
                {
                    return v;
                });
        }

        private void btnOtvori_Click(object sender, EventArgs e)
        {
            if (gridView.Rows.Count > 0)
            {
                Vezba vezba = (Vezba)entities[gridView.CurrentRow.Index];
                vezbaId = vezba.Id;
            }
        }
    }
}