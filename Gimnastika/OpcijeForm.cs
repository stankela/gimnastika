using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;
using Gimnastika.Dao;

namespace Gimnastika
{
    public partial class OpcijeForm : Form
    {
        List<PraviloOceneVezbe> pravila;

        public OpcijeForm()
        {
            InitializeComponent();

            Text = "Opcije";
            pravila = new PraviloOceneVezbeDAO().getAll();
            updateUI();
        }

        private void updateUI()
        {
            cmbPravilo.Items.AddRange(pravila.ToArray());
            selectPravilo(Opcije.Instance.PodrazumevanoPraviloID);
            txtVideo.Text = Opcije.Instance.PlayerFileName;
        }

        private void selectPravilo(int id)
        {
            foreach (PraviloOceneVezbe pravilo in pravila)
            {
                if (pravilo.Id == id)
                {
                    cmbPravilo.SelectedItem = pravilo;
                    return;
                }
            }
        }

        private void OpcijeForm_Shown(object sender, EventArgs e)
        {
            lblPravilo.Focus();
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = Application.ExecutablePath;
            openFileDlg.Filter = "exe (*.exe)|*.exe";
            openFileDlg.FilterIndex = 1;
            openFileDlg.RestoreDirectory = true;

            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                Opcije.Instance.PlayerFileName = openFileDlg.FileName;
                txtVideo.Text = Opcije.Instance.PlayerFileName;
            }
        }
    }
}