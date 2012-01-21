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
    public partial class GimnasticariForm : Form
    {
        private BindingListView<Gimnasticar> gimnasticari = null;
        DatabaseException ex = null;

        public GimnasticariForm()
        {
            InitializeComponent();
            initUI();
            try
            {
                gimnasticari = new BindingListView<Gimnasticar>(new GimnasticarDAO().getAll());
                gridView.DataSource = gimnasticari;
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

            this.Text = "Gimnasticari";
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
            column.DataPropertyName = "Ime";
            column.Name = "Ime";
            column.HeaderText = "Ime";
            column.Width = 100;
            gridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Prezime";
            column.Name = "Prezime";
            column.HeaderText = "Prezime";
            column.Width = 100;
            gridView.Columns.Add(column);

        }

        private void GimnasticariForm_Shown(object sender, EventArgs e)
        {
            if (gimnasticari == null)
            {
                MessageBox.Show(ex.Message, "Greska");
                Close();
            }
        }

        private void btnNovi_Click(object sender, EventArgs e)
        {
            GimnasticarForm form = new GimnasticarForm(null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                gimnasticari.Add(form.Gimnasticar);
                //refreshGrid();

                CurrencyManager currencyManager =
                    (CurrencyManager)this.BindingContext[gridView.DataSource];
                currencyManager.Position = gimnasticari.Count - 1;
            }
        }

        private void refreshGrid()
        {
            CurrencyManager currencyManager =
                (CurrencyManager)this.BindingContext[gridView.DataSource];
            currencyManager.Refresh();
            gridView.Refresh();
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            if (gridView.Rows.Count > 0)
            {
                Gimnasticar gimnasticar = gimnasticari[gridView.CurrentRow.Index];
                GimnasticarForm form = new GimnasticarForm(gimnasticar);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    refreshGrid();
                }
            }
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            if (gridView.Rows.Count > 0)
            {
                Gimnasticar gimnasticar = gimnasticari[gridView.CurrentRow.Index];
                if (MessageBox.Show("Da li zelite da izbrisete gimnasticara '" +
                    gimnasticar.ToString() + "' ?", "Potvrda", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    try
                    {
                        // TODO: Ovde prvo treba pitati sta treba raditi sa vezbama
                        // za datog gimnasticara (da li ih brisati ili ih ostaviti
                        // ali bez gimnasticara)

                        bool delete = true;
                        List<Vezba> vezbe = new VezbaDAO().getAll();
                        foreach (Vezba v in vezbe)
                        {
                            if (v.Gimnasticar != null && v.Gimnasticar.Id == gimnasticar.Id)
                            {
                                delete = false;
                                break;
                            }
                        }

                        if (delete)
                        {
                            new GimnasticarDAO().delete(gimnasticar);
                            gimnasticari.RemoveAt(gridView.CurrentRow.Index);
                            //             refreshGrid();
                        }
                        else
                        {
                            MessageBox.Show("Nije dozvoljeno brisanje takmicara za koga postoje vezbe. Da biste izbrisali gimnasticara, prvo morate da izbrisete sve njegove vezbe.", "Poruka");
                        }
                    }
                    catch (Gimnastika.Exceptions.DatabaseException ex)
                    {
                        MessageBox.Show(ex.Message, "Greska");
                    }
                }
            }
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}