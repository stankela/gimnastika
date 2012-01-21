using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Exceptions;
using Gimnastika.Entities;
using Gimnastika.Dao;
using System.IO;

namespace Gimnastika
{
    public partial class ElementForm : Form
    {
        private Element element = null;
        private Element original;
        private bool editMode;

        public Element Element
        {
            get { return element; }
        }

        private bool varijanta;
        private Element parent;
        bool persist;

        public ElementForm(Element element, Sprava sprava, bool varijanta,
            Element parent, bool persist)
        {
            InitializeComponent();

            this.varijanta = varijanta;
            this.parent = parent; // potrebno za varijante
            initUI();

            this.element = element;
            this.persist = persist;

            if (element == null)
            {
                editMode = false;
                this.element = new Element();
                if (sprava != Sprava.Undefined)
                    setComboSprava(sprava);
            }
            else
            {
                editMode = true;

                // TODO: Umesto objekta, konstruktoru dostavljati ID elementa
                // (ovo uraditi i u ostalim dijalozima). Ovo prvenstveno treba uraditi
                // zbog operacije restore koja se poziva kada se iz dijaloga izadje
                // pritiskom na Cancel. Ako objekat original nije pravilno kloniran,
                // tj. ako je izostavljen neki tip u pozivu naredbe Clone (a ovo se
                // lako moze desiti ako je klasa u medjuvremenu modifikovana
                // tako sto su dodate nove asocijacije), moze se
                // desiti da objekat koji restore generise nije jednak originalnom

                original = (Element)element.Clone(new TypeAsocijacijaPair[] { 
                        new TypeAsocijacijaPair(typeof(Video)), 
                        new TypeAsocijacijaPair(typeof(Slika)), 
                        new TypeAsocijacijaPair(typeof(Element), "varijante"),
                        new TypeAsocijacijaPair(typeof(Element), "parent") });
                updateUIFromEntity(element);
            }
            initHandlers();
        }

        private void initHandlers()
        {
            cmbSprava.SelectedIndexChanged += new EventHandler(cmbSprava_SelectedIndexChanged);
            cmbGrupa.SelectedIndexChanged += new EventHandler(cmbGrupa_SelectedIndexChanged);
            cmbTezina.SelectedIndexChanged += new EventHandler(cmbTezina_SelectedIndexChanged);
            chbTablicniElement.CheckedChanged += new EventHandler(chbTablicniElement_CheckedChanged);
        }

        public ElementForm(Element element, Sprava sprava, GrupaElementa grupa,
            int broj, TezinaElementa tezina)
        {
            InitializeComponent();

            this.varijanta = false;
            this.parent = null;
            initUI();

            this.element = element;
            this.persist = true;

            if (element == null)
            {
                editMode = false;
                this.element = new Element();
                setComboSprava(sprava);
                setComboGrupa(grupa);
                txtBroj.Text = broj.ToString();
                setComboTezina(tezina);
                
            }
            else
            {
                editMode = true;

                original = (Element)element.Clone(new TypeAsocijacijaPair[] { 
                        new TypeAsocijacijaPair(typeof(Video)), 
                        new TypeAsocijacijaPair(typeof(Slika)), 
                        new TypeAsocijacijaPair(typeof(Element), "varijante"),
                        new TypeAsocijacijaPair(typeof(Element), "parent") });
                updateUIFromEntity(element);
            }

            // TODO: Za rezim rada gde su comboi onemoguceni, probaj da ih zamenis
            // sa read-only text boxevima.
            cmbSprava.Enabled = false;
            chbTablicniElement.Enabled = false;
            cmbGrupa.Enabled = false;
            cmbTezina.Enabled = false;
            txtBroj.Enabled = false;

            initHandlers();
        }

        public ElementForm(Sprava sprava, GrupaElementa grupa)
        {
            InitializeComponent();

            this.varijanta = false;
            this.parent = null;
            initUI();

            this.element = null;
            this.persist = true;

            editMode = false;
            this.element = new Element();
            setComboSprava(sprava);
            setComboGrupa(grupa);

            cmbSprava.Enabled = false;
            chbTablicniElement.Enabled = false;
            cmbGrupa.Enabled = false;

            initHandlers();
        }

        private void ElementForm_Shown(object sender, EventArgs e)
        {
            lblSprava.Focus();
        }

        public void setComboSprava(Sprava sprava)
        {
            cmbSprava.SelectedIndex = sprava - Sprava.Parter;
        }

        public void setComboGrupa(GrupaElementa grupa)
        {
            cmbGrupa.SelectedIndex = grupa - GrupaElementa.I;
        }

        public void setComboTezina(TezinaElementa tezina)
        {
            cmbTezina.SelectedIndex = tezina - TezinaElementa.A;
        }

        public Sprava selectedSprava()
        {
            if (cmbSprava.SelectedIndex != -1)
                return ((SpravaNazivPair)cmbSprava.SelectedItem).Sprava;
            else
                return Sprava.Undefined;
        }

        private GrupaElementa selectedGrupa()
        {
            if (cmbGrupa.SelectedIndex != -1)
                return ((GrupaNazivPair)cmbGrupa.SelectedItem).Grupa;
            else
                return GrupaElementa.Undefined;
        }

        private TezinaElementa selectedTezina()
        {
            if (cmbTezina.SelectedIndex != -1)
                return ((TezinaNazivPair)cmbTezina.SelectedItem).Tezina;
            else
                return TezinaElementa.Undefined;
        }

        void cmbSprava_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedSprava() == Sprava.Parter && selectedGrupa() == GrupaElementa.V)
                setComboGrupa(GrupaElementa.I);
            element.Sprava = selectedSprava();
        }

        void cmbGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            element.Grupa = selectedGrupa();
        }

        void cmbTezina_SelectedIndexChanged(object sender, EventArgs e)
        {
            element.Tezina = selectedTezina();
        }

        // inicijalizuj UI za novi element (koji moze da bude i nova varijanta)
        private void initUI()
        {
            cmbSprava.Items.Clear();
            cmbSprava.Items.AddRange(Resursi.SpravaNazivTable);

            cmbGrupa.Items.Clear();
            cmbGrupa.Items.AddRange(Resursi.GrupaNazivTable);

            cmbTezina.DataSource = Resursi.TezinaNazivTable;
            cmbTezina.DisplayMember = "Naziv";
            cmbTezina.ValueMember = "Tezina";

            this.Text = "Novi element";
            txtNaziv.Text = String.Empty;
            txtEngNaziv.Text = string.Empty;
            txtNazivGim.Text = String.Empty;
            cmbSprava.SelectedIndex = -1;
            chbTablicniElement.Checked = true;
            cmbGrupa.SelectedIndex = -1;
            cmbTezina.SelectedIndex = -1;
            txtBroj.Text = String.Empty;

            lstVideo.DisplayMember = "RelFileNamePath";
            lstVideo.Items.Clear();

            lstVarijante.DisplayMember = "VarijantaString";
            lstVarijante.Items.Clear();

            if (varijanta)
            {
                lblVarijante.Visible = false;
                lstVarijante.Visible = false;
                btnDodajVarijantu.Visible = false;
                btnPromeniVarijantu.Visible = false;
                btnBrisiVarijantu.Visible = false;

                int oldHeight = groupBoxElement.Height;
                groupBoxElement.Height = lblVarijante.Top - 10;
                int delta = oldHeight - groupBoxElement.Height;
                btnOK.Top -= delta;
                btnCancel.Top -= delta;
                this.Height -= delta;

                this.Text = "Varijanta elementa " + parent.ToString();
                setComboSprava(parent.Sprava);
                setComboGrupa(parent.Grupa);
                setComboTezina(parent.Tezina);
                txtBroj.Text = parent.Broj.ToString() + ",";
            }

            pictureBoxSlika.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlika.NoDistort = true;
        }

        // azuriraj UI za postojeci element (koji moze da bude i varijanta)
        private void updateUIFromEntity(Element element)
        {
            this.Text = "Element: " + element.ToString();
            txtNaziv.Text = element.Naziv;
            txtEngNaziv.Text = element.EngleskiNaziv;
            txtNazivGim.Text = element.NazivPoGimnasticaru;
            setComboSprava(element.Sprava);
            if (element.IsTablicniElement)
            {
                chbTablicniElement.Checked = true;
                setComboGrupa(element.Grupa);
                setComboTezina(element.Tezina);
                txtBroj.Text = element.BrojPodBroj;
            }
            else
            {
                chbTablicniElement.Checked = false;
                cmbGrupa.Enabled = false;
                cmbTezina.Enabled = false;
                txtBroj.Enabled = false;
            }
            updateVideoUI();
            updateSlikeUI();
            updateVarijanteUI();
        }

        private void updateVideoUI()
        {
            lstVideo.Items.Clear();
            foreach (Video v in element.VideoKlipovi)
                lstVideo.Items.Add(v);
        }

        private void updateSlikeUI()
        {
            Slika slika = element.getPodrazumevanaSlika();
            if (slika != null)
                pictureBoxSlika.Image = slika.Image;
            else
                pictureBoxSlika.Image = null;
        }

        private void updateVarijanteUI()
        {
            lstVarijante.Items.Clear();
            foreach (Element e in element.Varijante)
                lstVarijante.Items.Add(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!updateElementFromUI())
                {
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (persist)
                {
                    if (editMode)
                        new ElementDAO().update(element, original);
                    else
                        new ElementDAO().insert(element);
                }
            }
            catch (DatabaseException)
            {
                string message;
                if (editMode)
                {
                    message = "Neuspesna promena elementa u bazi.";
                    //message = string.Format(
                    //"Neuspesna promena elementa u bazi. \n{0}", ex.InnerException.Message);
                }
                else
                {
                    message = "Neuspesan upis novog elementa u bazu.";
                    //message = string.Format(
                    //"Neuspesan upis novog elementa u bazu. \n{0}", ex.InnerException.Message);
                }
                MessageBox.Show(message, "Greska");
                discardChanges();
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception)
            {
                discardChanges();
                throw;
            }
        }

        private void discardChanges()
        {
            if (editMode)
                element.restore(original);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            discardChanges();
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNaziv.Focus();
                    break;
                case "EngleskiNaziv":
                    txtEngNaziv.Focus();
                    break;
                case "NazivPoGimnasticaru":
                    txtNazivGim.Focus();
                    break;
                case "Broj":
                    txtBroj.Focus();
                    break;
                case "Varijante":
                    lstVarijante.Focus();
                    break;

                default:
                    break;
            }
        }

        private bool validateDialog()
        {
            return requiredFieldsValidation();
        }

        private bool requiredFieldsValidation()
        {
            if (cmbSprava.SelectedIndex == -1)
            {
                MessageBox.Show("Izaberite spravu.", "Greska");
                cmbSprava.Focus();
                return false;
            }
            if (txtNaziv.Text.Trim() == String.Empty
            && txtEngNaziv.Text.Trim() == String.Empty
            && txtNazivGim.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Unesite naziv elementa.", "Greska");
                txtNaziv.Focus();
                return false;
            }
            if (chbTablicniElement.Checked)
            {
                if (cmbGrupa.SelectedIndex == -1)
                {
                    MessageBox.Show("Izaberite grupu.", "Greska");
                    cmbGrupa.Focus();
                    return false;
                }
                if (cmbTezina.SelectedIndex == -1)
                {
                    MessageBox.Show("Izaberite tezinu.", "Greska");
                    cmbTezina.Focus();
                    return false;
                }
                if (txtBroj.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Unesite broj elementa.", "Greska");
                    txtBroj.Focus();
                    return false;
                }
            }
            return true;
        }

        private bool updateElementFromUI()
        {
            if (!validateDialog())
                return false;
            try
            {
                doUpdateElementFromUI();
                if (!element.validate())
                    return false;
                if (persist)
                {
                    if (editMode)
                        DatabaseConstraintsValidator.checkUpdate(element, original);
                    else
                        DatabaseConstraintsValidator.checkInsert(element);
                }
                return true;
            }
            catch (InvalidPropertyException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                setFocus(ex.InvalidProperty);
                return false;
            }
            catch (DatabaseConstraintException ex)
            {
                MessageBox.Show(ex.ValidationErrors[0].Message, "Greska");
                setFocus(ex.ValidationErrors[0].InvalidProperties[0]);
                return false;
            }
            catch (DatabaseException)
            {
                string message;
                if (editMode)
                    message = "Neuspesna promena elementa u bazi.";
                else
                    message = "Neuspesan upis novog elementa u bazu.";
                MessageBox.Show(message, "Greska");
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void doUpdateElementFromUI()
        {
            element.Naziv = txtNaziv.Text.Trim();
            element.EngleskiNaziv = txtEngNaziv.Text.Trim();
            element.NazivPoGimnasticaru = txtNazivGim.Text.Trim();
            element.Sprava = selectedSprava();
            if (!chbTablicniElement.Checked)
            {
                element.ponistiPolozajUTablici();
            }
            else
            {
                setPolozajUTabliciFromUI();
            }
            if (!editMode && varijanta)
                element.Parent = parent;
        }

        private void setPolozajUTabliciFromUI()
        {
            GrupaElementa grupa = selectedGrupa();
            TezinaElementa tezina = selectedTezina();
            string broj = txtBroj.Text;
            element.setPolozajUTablici(grupa, tezina, broj);
        }

        private void chbTablicniElement_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTablicniElement.Checked)
            {
                cmbGrupa.Enabled = true;
                cmbTezina.Enabled = true;
                txtBroj.Enabled = true;

                if (!Element.isValidBrojPodBroj(txtBroj.Text))
                {
                    if (varijanta)
                        txtBroj.Text = "1,1";
                    else
                        txtBroj.Text = "1";
                }
                setPolozajUTabliciFromUI();
            }
            else
            {
                cmbGrupa.Enabled = false;
                cmbTezina.Enabled = false;
                txtBroj.Enabled = false;
                element.ponistiPolozajUTablici();
            }
        }

        private void ElementForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                discardChanges();
            }
        }

        private void btnDodajVideo_Click(object sender, EventArgs e)
        {
            string relFileNamePath = getAppRelativeFileNamePathFromUser();
            if (relFileNamePath != null)
            {
                if (findVideo(relFileNamePath) != null)
                {
                    MessageBox.Show(
                        "Video: " + relFileNamePath + " vec postoji za dati element.", "Greska");
                    return;
                }
                else
                {
                    Video v = new Video(relFileNamePath);
                    element.dodajVideo(v);
                    updateVideoUI();
                }
            }
        }

        public static string getAppRelativeFileNamePathFromUser()
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = Application.ExecutablePath;
            openFileDlg.Filter = "All files (*.*)|*.*";
            openFileDlg.FilterIndex = 1;
            openFileDlg.RestoreDirectory = true;

            DialogResult dlgResult = DialogResult.None;
            while (true)
            {
                dlgResult = openFileDlg.ShowDialog();
                if (dlgResult != DialogResult.OK || isAppRelative(openFileDlg.FileName))
                    break;
                MessageBox.Show("Datoteka mora da se nalazi u nekom od " +
                    "poddirektorijuma glavnog direktorijuma aplikacije.", "Greska");
            }
            if (dlgResult != DialogResult.OK)
                return null;

            string appDir = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
            return openFileDlg.FileName.Replace(appDir, "");
        }

        private static bool isAppRelative(string fullName)
        {
            string dir = Path.GetDirectoryName(fullName);
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            return dir.IndexOf(appDir) != -1;
        }

        private Video findVideo(string relFileNamePath)
        {
            Video result = null;
            foreach (Video v in element.VideoKlipovi)
            {
                if (v.RelFileNamePath == relFileNamePath)
                {
                    result = v;
                    break;
                }
            }
            return result;
        }

        private void btnBrisiVideo_Click(object sender, EventArgs e)
        {
            if (lstVideo.SelectedItem != null)
            {
                element.ukloniVideo((Video)lstVideo.SelectedItem);
                updateVideoUI();
            }
        }

        private void btnPrikaziVideo_Click(object sender, EventArgs e)
        {
            Video video = null;
            if (lstVideo.SelectedItem != null)
                video = (Video)lstVideo.SelectedItem;
            else if (lstVideo.Items.Count == 1)
                video = (Video)lstVideo.Items[0];

            if (video != null)
            {
                try
                {
                    video.play();
                }
                catch (VideoException ex)
                {
                    MessageBox.Show(ex.Message, "Greska");
                }
            }
        }

        private void btnDodajVarijantu_Click(object sender, EventArgs e)
        {
            if (!chbTablicniElement.Checked)
                element.ponistiPolozajUTablici();
            else
            {
                if (cmbSprava.SelectedIndex == -1)
                {
                    MessageBox.Show("Izaberite spravu.", "Greska");
                    cmbSprava.Focus();
                    return;
                }
                else if (cmbGrupa.SelectedIndex == -1)
                {
                    MessageBox.Show("Izaberite grupu.", "Greska");
                    cmbGrupa.Focus();
                    return;
                }
                else if (cmbTezina.SelectedIndex == -1)
                {
                    MessageBox.Show("Izaberite tezinu.", "Greska");
                    cmbTezina.Focus();
                    return;
                }
                else if (!Element.isValidBrojPodBroj(txtBroj.Text))
                {
                    MessageBox.Show("Unesite ispravan broj.", "Greska");
                    txtBroj.Focus();
                    return;
                }
                else
                {
                    setPolozajUTabliciFromUI();
                }
            }
            ElementForm form = new ElementForm(null, selectedSprava(),
                true, element, false);
            if (form.ShowDialog() == DialogResult.OK)
            {
                element.dodajVarijantu(form.Element);
                updateVarijanteUI();
            }
        }

        private void btnBrisiVarijantu_Click(object sender, EventArgs e)
        {
            if (lstVarijante.SelectedItem != null)
            {
                Element varijanta = (Element)lstVarijante.SelectedItem;
                if (MessageBox.Show("Da li zelite da izbrisete varijantu '" +
                    varijanta.VarijantaString + "' ?", "Potvrda", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    element.ukloniVarijantu(varijanta);
                    updateVarijanteUI();
                }
            }
        }

        private void btnPromeniVarijantu_Click(object sender, EventArgs e)
        {
            if (lstVarijante.SelectedItem != null)
            {
                if (!chbTablicniElement.Checked)
                    element.ponistiPolozajUTablici();
                else
                {
                    if (Element.isValidBrojPodBroj(txtBroj.Text))
                        setPolozajUTabliciFromUI();
                    else
                    {
                        MessageBox.Show("Unesite ispravan broj.", "Greska");
                        txtBroj.Focus();
                        return;
                    }
                }
                Element varijanta = (Element)lstVarijante.SelectedItem;
                ElementForm form = new ElementForm(varijanta, varijanta.Sprava, true,
                    element, false);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    updateVarijanteUI();
                }
            }
        }

        private void txtBroj_Leave(object sender, EventArgs e)
        {
            if (Element.isValidBrojPodBroj(txtBroj.Text))
            {
                element.BrojPodBroj = txtBroj.Text;
                cmbTezina.SelectedIndex =
                    Element.getTezina(element.Broj) - TezinaElementa.A;
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

        private void btnSlike_Click(object sender, EventArgs e)
        {
            SlikeForm f = new SlikeForm(element);
            if (f.ShowDialog() == DialogResult.OK)
            {
                updateSlikeUI();
            }
        }
    }
}