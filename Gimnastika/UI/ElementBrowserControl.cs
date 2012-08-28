using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;

namespace Gimnastika.UI
{
    public partial class ElementBrowserControl : UserControl
    {
        BindingListView<Element> elementi;

        public ElementBrowserControl()
        {
            InitializeComponent();
            initUI();
        }

        public BindingListView<Element> Elementi
        {
            set
            {
                elementi = value;
                fillGridView();
                applyFilter();
            }
        }

        // NOTE: Ovo je prvobitno bilo realizovano kao svojstvo Sprava, ali je tada
        // svaki put kada bih stavio ElementBrowserControl na formu dizajner automatski
        // u Designer.cs fajlu forme inicijalizovao svojstvo Sprava na vrednost
        // Sprava.Undefined, sto je imalo za posledicu da je cmbSprava onemogucen.
        public void restrictSprava(Sprava value)
        {
            cmbSprava.SelectedIndex = value - Sprava.Undefined;
            cmbSprava.Enabled = false;
            applyFilter();
        }

        private void initUI()
        {
            cmbSprava.Items.Clear();
            cmbSprava.Items.AddRange(Resursi.SpravaNazivTableEx);
            cmbSprava.SelectedIndex = 0;

            cmbGrupa.Items.Clear();
            cmbGrupa.Items.AddRange(Resursi.GrupaNazivTableEx);
            cmbGrupa.SelectedIndex = 0;

            cmbTezina.DataSource = Resursi.TezinaNazivTableEx;
            cmbTezina.DisplayMember = "Naziv";
            cmbTezina.ValueMember = "Tezina";

            cmbSprava.SelectedIndex = 0;
            cmbGrupa.SelectedIndex = 0;
            cmbTezina.SelectedIndex = 0;

            rbtTablicni.Checked = true;

            gridViewElementi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridViewElementi.MultiSelect = false;
            gridViewElementi.AllowUserToAddRows = false;
            gridViewElementi.AllowUserToDeleteRows = false;
            gridViewElementi.AllowUserToResizeRows = false;
            gridViewElementi.ReadOnly = true;

            cmbSprava.SelectedIndexChanged += cmbSprava_SelectedIndexChanged;
            cmbGrupa.SelectedIndexChanged += cmbGrupa_SelectedIndexChanged;
            cmbTezina.SelectedIndexChanged += cmbTezina_SelectedIndexChanged;
            rbtTablicni.CheckedChanged += rbtTablicni_CheckedChanged;
        }

        private void fillGridView()
        {
            gridViewElementi.DataSource = elementi;
        }

        private void applyFilter()
        {
            IBindingListView listView = gridViewElementi.DataSource as IBindingListView;
            if (listView != null)
            {
                string filter = String.Format("IsTablicniElement = {0} ",
                    rbtTablicni.Checked);
                if (selectedSprava() != Sprava.Undefined)
                {
                    filter += String.Format(" AND Sprava = {0} ", selectedSprava());
                }
                if (rbtTablicni.Checked)
                {
                    if (selectedGrupa() != GrupaElementa.Undefined)
                    {
                        filter += String.Format(" AND Grupa = {0} ", selectedGrupa());
                    }
                    if (selectedTezina() != TezinaElementa.Undefined)
                    {
                        filter += String.Format(" AND Tezina = {0} ", selectedTezina());
                    }

                }
                if (filter != String.Empty)
                {
                    listView.Filter = filter;
                }
                else
                {
                    listView.RemoveFilter();
                }
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

        private TezinaElementa selectedTezina()
        {
            return (TezinaElementa)cmbTezina.SelectedValue;
        }



        /*
        // How to remove sort
        private void btnRemoveSort_Click(object sender, EventArgs e)
        {
            IBindingListView listView = gridViewElementi.DataSource as IBindingListView;
            if (listView != null)
            {
                listView.RemoveSort();
            }
        }

        // How to apply complex sort
        private void btnApplyComplexSort_Click(object sender, EventArgs e)
        {
            IBindingListView listView = gridViewElementi.DataSource as IBindingListView;
            if (listView != null)
            {
                ListSortDescription[] sortDescs = new ListSortDescription[3];
                PropertyDescriptorCollection propColl = TypeDescriptor.GetProperties(typeof(Element));
                sortDescs[0] = new ListSortDescription(propColl[2], ListSortDirection.Ascending);
                sortDescs[1] = new ListSortDescription(propColl[0], ListSortDirection.Ascending);
                sortDescs[2] = new ListSortDescription(propColl[1], ListSortDirection.Descending);
                ListSortDescriptionCollection coll = new ListSortDescriptionCollection(sortDescs);
                listView.ApplySort(coll);
            }
        }
        */

        private void cmbSprava_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedSprava() == Sprava.Parter && selectedGrupa() == GrupaElementa.V)
                cmbGrupa.SelectedIndex = 1;
            else
                applyFilter();
        }

        private void cmbGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            applyFilter();
        }

        private void cmbTezina_SelectedIndexChanged(object sender, EventArgs e)
        {
            applyFilter();
        }

        private void rbtTablicni_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTablicni.Checked)
            {
                cmbGrupa.Enabled = true;
                cmbTezina.Enabled = true;
            }
            else
            {
                cmbGrupa.Enabled = false;
                cmbTezina.Enabled = false;
            }
            applyFilter();
        }

        private void gridViewElementi_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridViewElementi.Columns[e.ColumnIndex].Name == "Tezina")
            {
                if (e.Value != null && ((TezinaElementa)e.Value == TezinaElementa.Undefined))
                    e.Value = "";
            }
        }

        public void selektuj(int position)
        {
            CurrencyManager currencyManager =
                (CurrencyManager)this.BindingContext[gridViewElementi.DataSource];
            currencyManager.Position = position;
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
    }
}
