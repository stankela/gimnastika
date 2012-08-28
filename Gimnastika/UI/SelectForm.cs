using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;

namespace Gimnastika.UI
{
    public partial class SelectForm : Form
    {
        public SelectForm()
        {
            InitializeComponent();
            initUI();
        }

        private void initUI()
        {
            Text = "Pregled elemenata";
            SelektujElementeControl parterSelektujElementeControl =
                this.selektujElementeControl1;

            TabPage[] tabPages = { tabPageKonj, tabPageKarike, tabPagePreskok,
                tabPageRazboj, tabPageVratilo};
            foreach (TabPage tabPage in tabPages)
            {
                SelektujElementeControl newSelektujElementeControl =
                    new SelektujElementeControl();
                newSelektujElementeControl.Location =
                    parterSelektujElementeControl.Location;
                newSelektujElementeControl.Size =
                    parterSelektujElementeControl.Size;
                newSelektujElementeControl.TabIndex =
                    parterSelektujElementeControl.TabIndex;
                tabPage.Controls.Add(newSelektujElementeControl);
            }
        }

        private void btnResetuj_Click(object sender, EventArgs e)
        {
            getActiveSelektujElementeControl().resetuj();
        }

        private SelektujElementeControl getActiveSelektujElementeControl()
        {
            SelektujElementeControl result = null;
            TabPage activeTab = tabControl1.SelectedTab;
            foreach (Control c in activeTab.Controls)
            {
                if (c.GetType() == typeof(SelektujElementeControl))
                {
                    result = (SelektujElementeControl)c;
                    break;
                }
            }
            return result;
        }

        private void btnPrikazi_Click(object sender, EventArgs e)
        {
            List<TezinaElementa> selektovaneTezine =
                getActiveSelektujElementeControl().getSelektovaneTezine();
            List<GrupaElementa> selektovaneGrupe =
                getActiveSelektujElementeControl().getSelektovaneGrupe();
        }
    }
}