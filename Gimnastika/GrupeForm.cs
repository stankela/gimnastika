using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Entities;
using Gimnastika.Dao;
using Gimnastika.Exceptions;

namespace Gimnastika
{
    public partial class GrupeForm : Form
    {
        private Sprava sprava;
        private GrupaElementa grupaElementa;
        bool editMode;
        Grupa grupa;
        Grupa original;

        List<Grupa> grupe;

        public GrupeForm()
        {
            InitializeComponent();
            initUI();

            grupe = new GrupaDAO().getAll();
            showGrupaDetails();

        }

        private void showGrupaDetails()
        {
            grupa = findGrupa(sprava, grupaElementa);
            if (grupa == null)
            {
                grupa = new Grupa(sprava, grupaElementa, "", "");
                original = null;
                editMode = false;
            }
            else
            {
                editMode = true;
                original = (Grupa)grupa.Copy();
            }
            txtNaziv.Text = grupa.Naziv;
            txtEngNaziv.Text = grupa.EngNaziv;
        }

        private bool updateGrupaFromUI()
        {
            try
            {
                grupa.Naziv = txtNaziv.Text.Trim();
                grupa.EngNaziv = txtEngNaziv.Text.Trim();
                if (!grupa.validate())
                    return false;
                if (editMode)
                {
                    if (grupa.Naziv != original.Naziv || grupa.EngNaziv != original.EngNaziv)
                        new GrupaDAO().update(grupa, null);
                }
                else
                {
                    new GrupaDAO().insert(grupa);
                    grupe.Add(grupa);
                    editMode = true;
                    original = (Grupa)grupa.Copy();
                }
                return true;
            }
            catch (InvalidPropertyException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                setFocus(ex.InvalidProperty);
                return false;
            }
            catch (DatabaseException)
            {
                string message;
                if (editMode)
                    message = "Neuspesna promena grupe u bazi.";
                else
                    message = "Neuspesan upis nove grupe u bazu.";
                MessageBox.Show(message, "Greska");
                return false;
            }
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNaziv.Focus();
                    break;
                case "EngNaziv":
                    txtEngNaziv.Focus();
                    break;

                default:
                    break;
            }
        }
        private Grupa findGrupa(Sprava sprava, GrupaElementa grupaElementa)
        {
            foreach (Grupa g in grupe)
            {
                if (g.Sprava == sprava && g.GrupaElemenata == grupaElementa)
                    return g;
            }
            return null;
        }

        private void initUI()
        {
            Text = "Nazivi grupa";

            cmbSprava.Items.Clear();
            cmbSprava.Items.AddRange(Resursi.SpravaNazivTable);
            cmbSprava.SelectedIndex = 0;
            sprava = selectedSprava();

            cmbGrupa.Items.Clear();
            cmbGrupa.Items.AddRange(Resursi.GrupaNazivTable);
            cmbGrupa.SelectedIndex = 0;
            grupaElementa = selectedGrupa();

            txtNaziv.Text = "";
            txtEngNaziv.Text = "";

            cmbSprava.SelectedIndexChanged += new EventHandler(cmbSprava_SelectedIndexChanged);
            cmbGrupa.SelectedIndexChanged += new EventHandler(cmbGrupa_SelectedIndexChanged);
        }

        void cmbGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateGrupaFromUI())
            {
                grupaElementa = selectedGrupa();
                showGrupaDetails();
            }
            else
            {
                disableComboHandlers();
                setSpravaCombo(sprava);
                setGrupaCombo(grupaElementa);
                enableComboHandlers();
            }
        }

        private void setGrupaCombo(GrupaElementa grupaElementa)
        {
            cmbSprava.SelectedIndex = grupaElementa - GrupaElementa.I;
        }

        private void setSpravaCombo(Sprava sprava)
        {
            cmbSprava.SelectedIndex = sprava - Sprava.Parter;
        }

        private void enableComboHandlers()
        {
            cmbSprava.SelectedIndexChanged += new EventHandler(cmbSprava_SelectedIndexChanged);                
            cmbGrupa.SelectedIndexChanged += new EventHandler(cmbGrupa_SelectedIndexChanged);
        }

        private void disableComboHandlers()
        {
            cmbSprava.SelectedIndexChanged -= cmbSprava_SelectedIndexChanged;
            cmbGrupa.SelectedIndexChanged -= cmbGrupa_SelectedIndexChanged;
        }

        void cmbSprava_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateGrupaFromUI())
            {
                sprava = selectedSprava();
                if (sprava == Sprava.Parter && selectedGrupa() == GrupaElementa.V)
                {
                    grupaElementa = GrupaElementa.I;
                    disableComboHandlers();
                    setGrupaCombo(grupaElementa);
                    enableComboHandlers();
                }
                showGrupaDetails();
            }
            else
            {
                disableComboHandlers();
                setSpravaCombo(sprava);
                setGrupaCombo(grupaElementa);
                enableComboHandlers();
            }
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

        public Sprava selectedSprava()
        {
            return ((SpravaNazivPair)cmbSprava.SelectedItem).Sprava;
        }

        private GrupaElementa selectedGrupa()
        {
            return ((GrupaNazivPair)cmbGrupa.SelectedItem).Grupa;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!updateGrupaFromUI())
                DialogResult = DialogResult.None;
        }
    }
}