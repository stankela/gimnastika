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
        List<Element> elementi;

        public DataGridViewUserControl DataGridViewUserControl
        {
            get { return dataGridViewUserControl1; }
        }

        public ElementBrowserControl()
        {
            InitializeComponent();
            initUI();
        }

        public void setElementi(List<Element> elementi)
        {
            this.elementi = elementi;
            dataGridViewUserControl1.setItems<Element>(elementi);
            applyFilter();
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

            dataGridViewUserControl1.GridColumnHeaderMouseClick +=
                new EventHandler<GridColumnHeaderMouseClickEventArgs>(DataGridViewUserControl_GridColumnHeaderMouseClick);
            GridColumnsInitializer.initElement(dataGridViewUserControl1);
            dataGridViewUserControl1.DataGridView.MultiSelect = false;
            dataGridViewUserControl1.DataGridView.AllowUserToResizeRows = false;

            cmbSprava.SelectedIndexChanged += cmbSprava_SelectedIndexChanged;
            cmbGrupa.SelectedIndexChanged += cmbGrupa_SelectedIndexChanged;
            cmbTezina.SelectedIndexChanged += cmbTezina_SelectedIndexChanged;
            rbtTablicni.CheckedChanged += rbtTablicni_CheckedChanged;
        }

        private void DataGridViewUserControl_GridColumnHeaderMouseClick(object sender,
            GridColumnHeaderMouseClickEventArgs e)
        {
            DataGridViewUserControl dgwuc = sender as DataGridViewUserControl;
            if (dgwuc != null)
                dgwuc.onColumnHeaderMouseClick<Element>(e.DataGridViewCellMouseEventArgs);
        }

        private void applyFilter()
        {
            List<Element> filteredElementi = new List<Element>();
            foreach (Element e in elementi)
            {
                if (e.IsTablicniElement != rbtTablicni.Checked)
                    continue;
                if (selectedSprava() != Sprava.Undefined)
                {
                    if (e.Sprava != selectedSprava())
                        continue;
                }
                if (rbtTablicni.Checked)
                {
                    if (selectedGrupa() != GrupaElementa.Undefined)
                    {
                        if (e.Grupa != selectedGrupa())
                            continue;
                    }
                    if (selectedTezina() != TezinaElementa.Undefined)
                    {
                        if (e.Tezina != selectedTezina())
                            continue;
                    }
                }
                filteredElementi.Add(e);
            }
            dataGridViewUserControl1.setItems<Element>(filteredElementi);

            /*IBindingListView listView = gridViewElementi.DataSource as IBindingListView;
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
                listView.Filter = filter;
            }*/
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

        public void selektuj(Element e)
        {
            // TODO: U komentaru unutart metoda setSelectedItem stoji da Element mora da implementira Equals da bi ovaj
            // metod dobro radio. Proveri ovo.
            dataGridViewUserControl1.setSelectedItem<Element>(e);
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
