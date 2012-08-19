using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;

namespace Gimnastika
{
    public partial class SlikeForm : Form
    {
        Element element;
        List<Slika> originalSlike = new List<Slika>();

        public SlikeForm(Element element)
        {
            InitializeComponent();
            initUI();

            this.element = element;
            foreach (Slika s in element.Slike)
                originalSlike.Add((Slika)s.Copy());

            refreshLstSlike();
            if (element.Slike.Count > 0)
            {
                Slika slika = element.getPodrazumevanaSlika();
                if (slika != null)
                    selectSlika(slika);
                else
                    lstSlike.SelectedIndex = 0;
            }
        }

        private void selectSlika(Slika slika)
        {
            for (int i = 0; i < lstSlike.Items.Count; i++)
            {
                Slika s = (Slika)lstSlike.Items[i];
                if (s == slika)
                {
                    lstSlike.SelectedIndex = i;
                    return;
                }
            }
        }

        private void initUI()
        {
            pictureBoxSlika.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlika.NoDistort = true;

            lstSlike.DisplayMember = "RelFileNamePath";
            lstSlike.Items.Clear();
        }

        private void refreshLstSlike()
        {
            lstSlike.Items.Clear();
            foreach (Slika s in element.Slike)
                lstSlike.Items.Add(s);
        }

        private void btnDodajSliku_Click(object sender, EventArgs e)
        {
            string relFileNamePath = ElementForm.getAppRelativeFileNamePathFromUser();
            if (relFileNamePath != null)
            {
                if (findSlika(relFileNamePath) != null)
                {
                    MessageBox.Show(
                        "Slika: " + relFileNamePath + " vec postoji za dati element.", "Greska");
                    return;
                }
                else
                {
                    Slika s = new Slika(relFileNamePath, false, 100);
                    element.dodajSliku(s);
                    refreshLstSlike();
                    selectSlika(s);
                }
            }
        }

        private Slika findSlika(string relFileNamePath)
        {
            Slika result = null;
            foreach (Slika s in element.Slike)
            {
                if (s.RelFileNamePath == relFileNamePath)
                {
                    result = s;
                    break;
                }
            }
            return result;
        }

        private void btnBrisiSliku_Click(object sender, EventArgs e)
        {
            if (lstSlike.SelectedItem != null)
            {
                element.ukloniSliku((Slika)lstSlike.SelectedItem);
                refreshLstSlike();
                if (element.Slike.Count > 0)
                {
                    if (element.getPodrazumevanaSlika() != null)
                        selectSlika(element.getPodrazumevanaSlika());
                    else
                        selectSlika(element.Slike[0]);
                }
                else
                {
                    pictureBoxSlika.Image = null;
                    chbPodrazumevana.Checked = false;
                }
            }
        }

        private void chbPodrazumevana_CheckedChanged(object sender, EventArgs e)
        {
            if (lstSlike.SelectedItem != null)
            {
                element.getPodrazumevanaSlika().Podrazumevana = false;
                ((Slika)lstSlike.SelectedItem).Podrazumevana = chbPodrazumevana.Checked;
            }
        }

        private void lstSlike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSlike.SelectedIndex != -1)
            {
                showDetails();            
            }
        }

        private void showDetails()
        {
            disableCheckBoxHandler(); //zato sto se sada samo prikazuje stanje. Handler
                        // treba da se poziva samo kada se klikne misem na check box
                        // (ili se pritisne spacebar)
            if (lstSlike.SelectedIndex != -1)
            {
                Slika slika = element.Slike[lstSlike.SelectedIndex];
                pictureBoxSlika.Image = slika.Image;
                chbPodrazumevana.Checked = slika.Podrazumevana;
            }
            else
            {
                pictureBoxSlika.Image = null;
                chbPodrazumevana.Checked = false;
            }
            enableCheckBoxHandler();
        }

        private void disableCheckBoxHandler()
        {
            chbPodrazumevana.CheckedChanged -= chbPodrazumevana_CheckedChanged;
        }

        private void enableCheckBoxHandler()
        {
            chbPodrazumevana.CheckedChanged += chbPodrazumevana_CheckedChanged;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            discardChanges();
        }

        private void discardChanges()
        {
            element.ukloniSlike();
            foreach (Slika s in originalSlike)
                element.dodajSliku((Slika)s.Copy());
        }

        private void SlikeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // kada se dijalog zatvara pritiskom na OK ili Cancel, CloseReason je None
            if (e.CloseReason == CloseReason.UserClosing)
            {
                discardChanges();
            }
        }

    }
}