using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Entities;
using Gimnastika.Exceptions;
using Gimnastika.Dao;

namespace Gimnastika
{
    public partial class OtvoriVezbuForm : Form
    {
        private BindingListView<Vezba> vezbe = null;
        DatabaseException ex = null;

        private int vezbaId;

        public int VezbaId
        {
            get { return vezbaId; }
        }

        public OtvoriVezbuForm()
        {
            InitializeComponent();
            initUI();
            try
            {
                vezbe = new BindingListView<Vezba>(new VezbaDAO().getAll());
                gridView.DataSource = vezbe;
            }
            catch (DatabaseException ex)
            {
                this.ex = ex;
            }
        }

        private void initUI()
        {
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridView.MultiSelect = false;
            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;
            gridView.AllowUserToResizeRows = false;
            gridView.ReadOnly = true;

            this.Text = "Otvori vezbu";

            setupGrid();
        }

        private void setupGrid()
        {
            gridView.MultiSelect = false;
            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;
            gridView.AllowUserToResizeRows = false;
            gridView.AutoGenerateColumns = false;
            gridView.GridColor = Color.Black;
            gridView.ReadOnly = true;
            gridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Naziv";
            column.Name = "Naziv";
            column.HeaderText = "Naziv vezbe";
            column.Width = 200;
            gridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Sprava";
            column.Name = "Sprava";
            column.HeaderText = "Sprava";
            column.Width = 70;
            gridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Gimnasticar";
            column.Name = "Gimnasticar";
            column.HeaderText = "Gimnasticar";
            column.Width = 100;
            gridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Pravilo";
            column.Name = "Pravilo";
            column.HeaderText = "Pravila";
            column.Width = 100;
            gridView.Columns.Add(column);
        }

        private void VezbeForm_Shown(object sender, EventArgs e)
        {
            if (vezbe == null)
            {
                MessageBox.Show(ex.Message, "Greska");
                Close();
            }
        }

        private void btnOtvori_Click(object sender, EventArgs e)
        {
            if (gridView.Rows.Count > 0)
            {
                Vezba vezba = vezbe[gridView.CurrentRow.Index];
                vezbaId = vezba.Id;
            }
        }
    }
}