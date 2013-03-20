using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using Gimnastika.Domain;
using Gimnastika.Dao;
using Gimnastika.Exceptions;

namespace Gimnastika.UI
{
    public partial class MainForm : Form
    {
        // TODO: Proveri interfejs svih prozora (naziv, ivice, minimize box, ...)

        public MainForm()
        {
            InitializeComponent();
            Text = "Gimnastika";

            // Vidi napomenu kod metoda SystemEvents_SessionEnding
            //SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        private void mnPromenaElemenata_Click(object sender, EventArgs e)
        {
            try
            {
                ElementsForm f = new ElementsForm();
                f.ShowDialog();
            }
            catch (BusinessException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnPregledElemenata_Click(object sender, EventArgs e)
        {
            try
            {
                PregledElemenataForm f = new PregledElemenataForm();
                f.ShowDialog();
            }
            catch (BusinessException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnPromenaGimnasticara_Click(object sender, EventArgs e)
        {
            try
            {
                GimnasticariForm f = new GimnasticariForm();
                f.ShowDialog();
            }
            catch (BusinessException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnDatotekaKraj_Click(object sender, EventArgs e)
        {
            // TODO: Vidi zasto sam ovo stavio.
            throw new Exception();
            Close();
        }

        private void mnOpcijeOpcije_Click(object sender, EventArgs e)
        {
            OpcijeForm f = new OpcijeForm();
            if (f.ShowDialog() == DialogResult.OK)
            { 
            
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VezbaForm vezbaForm = getOpenVezbaForm();
            if (vezbaForm != null)
                vezbaForm.Close();
            e.Cancel = getOpenVezbaForm() != null;
        }

        public void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            // This event occurs when the user is trying to log off or shut down
            // the system.

            // If you are using SessionEnding in a Windows form to detect
            // a system logoff or reboot, there is no deterministic way to decide
            // whether the Closing event will fire before this event.
            // If you want to perform some special tasks before Closing is fired,
            // you need to ensure that SessionEnding fires before Closing. To do this,
            // you need to trap the WM_QUERYENDSESSION in the form by overriding the
            // WndProc function (see the example in Help)

            // Because this is a static event, you must detach your event handlers
            // when your application is disposed, or memory leaks will result.

            // NOTE: Ovaj event je bio potreban u NET 1.1 (da bi se proverilo da li
            // u otvorenim formama ima neceg sto bi trebalo snimiti), ali se u NET 2.0
            // moze koristiti event FormClosing (slucaj kada je e.CloseReason == 
            //  CloseReason.WindowsShutDown)

        }

        private void mnVezbeVezbe_Click(object sender, EventArgs e)
        {
            // Ako je vec otvoren, samo ga aktiviraj
            VezbaForm vezbaForm = getOpenVezbaForm();
            if (vezbaForm != null)
            {
                if (vezbaForm.WindowState == FormWindowState.Minimized)
                    vezbaForm.WindowState = FormWindowState.Normal;
                vezbaForm.Activate();
            }
            else
            {
                vezbaForm = new VezbaForm();
                vezbaForm.Show();
            }
        }

        private VezbaForm getOpenVezbaForm()
        {
            VezbaForm result = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.GetType() == typeof(VezbaForm))
                {
                    result = (VezbaForm)f;
                    break;
                }
            }
            return result;
        }

        private void mnPravilaOcenjivanja_Click(object sender, EventArgs e)
        {
            try
            {
                PravilaForm f = new PravilaForm();
                f.ShowDialog();
            }
            catch (BusinessException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnTabelaElemenata_Click(object sender, EventArgs e)
        {
            try
            {
                TabelaElemenataForm f =
                    new TabelaElemenataForm(
                        TabelaElemenataForm.TabelaElemenataFormRezimRada.Edit,
                        Sprava.Undefined);
                f.ShowDialog();
            }
            catch (BusinessException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnNaziviGrupa_Click(object sender, EventArgs e)
        {
            try
            {
                GrupeForm f = new GrupeForm();
                f.ShowDialog();
            }
            catch (BusinessException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

    }
}