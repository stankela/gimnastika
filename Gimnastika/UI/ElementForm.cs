using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika.Dao;
using System.IO;
using NHibernate;
using Gimnastika.Data;
using NHibernate.Context;
using System.Globalization;

namespace Gimnastika.UI
{
    public partial class ElementForm : Form
    {
        private Element element;
        public Element Element
        {
            get { return element; }
        }

        private bool varijanta;
        private Element parent;

        private bool editMode;
        bool persistEntity;
        private bool closedByOK;
        private bool closedByCancel;

        private Sprava oldSprava;
        private string oldNaziv;
        private string oldEngleskiNaziv;
        private string oldNazivPoGimnasticaru;
        private GrupaElementa oldGrupa;
        private TezinaElementa oldTezina;
        private short oldBroj;
        private byte oldPodBroj;

        public ElementForm(Nullable<int> elementId, Sprava sprava,
            Element parent, bool persistEntity)
        {
            InitializeComponent();

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    this.parent = parent;
                    this.varijanta = parent != null;
                    initUI(sprava == Sprava.Preskok);
                    this.persistEntity = persistEntity;

                    if (elementId == null)
                    {
                        editMode = false;
                        element = createNewEntity();
                        if (sprava != Sprava.Undefined)
                            setComboSprava(sprava);
                    }
                    else
                    {
                        editMode = true;
                        element = getEntityById(elementId.Value);
                        saveOldData(element);
                        updateUIFromEntity(element);
                    }

                    // TODO: Kada je sprava Undefined, moze da se desi da je combo sprava omogucen a textbox Vrednost
                    // onemogucen. Ako se kasnije izabere Preskok za spravu, textbox Vrednost bi trebalo omoguciti.
                    // Ova situacija moze da se desi u 2 od 4 konstruktora za ElementForm (u ostala 2 je cmbSprava
                    // uvek onemogucen).
                    cmbSprava.Enabled = sprava == Sprava.Undefined;
                    initHandlers();
                }
            }
            finally
            {
                // TODO: Dodaj sve catch blokove koji fale (i ovde i u programu za Clanove)
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        // konstruktor za situaciju kada se menja varijanta koja jos nije snimljena u bazu
        public ElementForm(Element varijanta)
        {
            InitializeComponent();

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    parent = varijanta.Parent;
                    this.varijanta = true;
                    initUI(varijanta.Sprava == Sprava.Preskok);
                    persistEntity = false;

                    editMode = true;
                    element = varijanta;
                    saveOldData(element);
                    updateUIFromEntity(element);

                    cmbSprava.Enabled = varijanta.Sprava == Sprava.Undefined;
                    initHandlers();
                }
            }
            finally
            {
                // TODO: Dodaj sve catch blokove koji fale (i ovde i u programu za Clanove)
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private Element createNewEntity()
        {
            return new Element();
        }

        private Element getEntityById(int id)
        {
            Element result = DAOFactoryFactory.DAOFactory.GetElementDAO().FindById(id);
            foreach (Element e in result.Varijante)
            {
                NHibernateUtil.Initialize(e.Slike);
                NHibernateUtil.Initialize(e.VideoKlipovi);
                NHibernateUtil.Initialize(e.Varijante);
            }
            return result;
        }

        private void saveOldData(Element element)
        {
            oldSprava = element.Sprava;
            oldNaziv = element.Naziv;
            oldEngleskiNaziv = element.EngleskiNaziv;
            oldNazivPoGimnasticaru = element.NazivPoGimnasticaru;
            oldGrupa = element.Grupa;
            oldTezina = element.Tezina;
            oldBroj = element.Broj;
            oldPodBroj = element.PodBroj;
        }

        private void initHandlers()
        {
            cmbSprava.SelectedIndexChanged += new EventHandler(cmbSprava_SelectedIndexChanged);
            cmbGrupa.SelectedIndexChanged += new EventHandler(cmbGrupa_SelectedIndexChanged);
            cmbTezina.SelectedIndexChanged += new EventHandler(cmbTezina_SelectedIndexChanged);
            chbTablicniElement.CheckedChanged += new EventHandler(chbTablicniElement_CheckedChanged);
        }

        public ElementForm(Nullable<int> elementId, Sprava sprava, GrupaElementa grupa,
            int broj, TezinaElementa tezina)
        {
            InitializeComponent();

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    this.varijanta = false;
                    this.parent = null;
                    initUI(sprava == Sprava.Preskok);
                    this.persistEntity = true;

                    if (elementId == null)
                    {
                        editMode = false;
                        element = createNewEntity();
                        setComboSprava(sprava);
                        setComboGrupa(grupa);
                        txtBroj.Text = broj.ToString();
                        setComboTezina(tezina);

                    }
                    else
                    {
                        editMode = true;
                        element = getEntityById(elementId.Value);
                        saveOldData(element);
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
            }
            finally
            {
                // TODO: Dodaj sve catch blokove koji fale (i ovde i u programu za Clanove)
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        public ElementForm(Sprava sprava, GrupaElementa grupa)
        {
            InitializeComponent();

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    this.varijanta = false;
                    this.parent = null;
                    initUI(sprava == Sprava.Preskok);

                    this.element = null;
                    this.persistEntity = true;

                    editMode = false;
                    this.element = new Element();
                    setComboSprava(sprava);
                    setComboGrupa(grupa);

                    cmbSprava.Enabled = false;
                    chbTablicniElement.Enabled = false;
                    cmbGrupa.Enabled = false;

                    initHandlers();
                }
            }
            finally
            {
                // TODO: Dodaj sve catch blokove koji fale (i ovde i u programu za Clanove)
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
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
            element.changeSprava(selectedSprava());
        }

        void cmbGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            element.changeGrupa(selectedGrupa());
        }

        void cmbTezina_SelectedIndexChanged(object sender, EventArgs e)
        {
            element.changeTezina(selectedTezina());
        }

        // inicijalizuj UI za novi element (koji moze da bude i nova varijanta)
        private void initUI(bool enableVrednost)
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
            txtVrednostPreskoka.Text = String.Empty;

            txtVrednostPreskoka.Enabled = enableVrednost;

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
                if (txtVrednostPreskoka.Enabled)
                {
                    if (element.Vrednost != null)
                        txtVrednostPreskoka.Text = element.VrednostPreskoka.ToString();
                    else
                        txtVrednostPreskoka.Text = string.Empty;
                }
            }
            else
            {
                chbTablicniElement.Checked = false;
                cmbGrupa.Enabled = false;
                cmbTezina.Enabled = false;
                txtBroj.Enabled = false;
                txtVrednostPreskoka.Enabled = false;
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
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    Notification notification = new Notification();
                    requiredFieldsAndFormatValidation(notification);
                    if (!notification.IsValid())
                        throw new BusinessException(notification);

                    if (editMode)
                        update();
                    else
                        add();
                    if (persistEntity)
                        session.Transaction.Commit();
                    closedByOK = true;
                }
            }
            catch (InvalidPropertyException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
                setFocus(ex.InvalidProperty);
                this.DialogResult = DialogResult.None;
            }
            catch (BusinessException ex)
            {
                if (ex.Notification != null)
                {
                    NotificationMessage msg = ex.Notification.FirstMessage;
                    MessageDialogs.showMessage(msg.Message, this.Text);
                    setFocus(msg.FieldName);
                }
                else
                {
                    MessageDialogs.showMessage(ex.Message, this.Text);
                }
                this.DialogResult = DialogResult.None;
            }
            catch (InfrastructureException ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(ex.Message, this.Text);
                this.DialogResult = DialogResult.Cancel;
                closedByCancel = true;
            }
            catch (Exception ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), this.Text);
                this.DialogResult = DialogResult.Cancel;
                closedByCancel = true;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
  
        }

        private void discardChanges()
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            discardChanges();
            closedByCancel = true;
        }

        private void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Sprava":
                    cmbSprava.Focus();
                    break;
                case "Naziv":
                    txtNaziv.Focus();
                    break;
                case "EngleskiNaziv":
                    txtEngNaziv.Focus();
                    break;
                case "NazivPoGimnasticaru":
                    txtNazivGim.Focus();
                    break;
                case "Grupa":
                    cmbGrupa.Focus();
                    break;
                case "Tezina":
                    cmbTezina.Focus();
                    break;
                case "Broj":
                    txtBroj.Focus();
                    break;
                case "VrednostPreskoka":
                    txtVrednostPreskoka.Focus();
                    break;
                case "Varijante":
                    lstVarijante.Focus();
                    break;

                default:
                    break;
            }
        }

        private void requiredFieldsAndFormatValidation(Notification notification)
        {
            if (cmbSprava.SelectedIndex == -1)
            {
                notification.RegisterMessage(
                    "Sprava", "Izaberite spravu.");
            }
            if (txtNaziv.Text.Trim() == String.Empty
            && txtEngNaziv.Text.Trim() == String.Empty
            && txtNazivGim.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Naziv", "Unesite naziv elementa.");
            }
            if (chbTablicniElement.Checked)
            {
                if (cmbGrupa.SelectedIndex == -1)
                {
                    notification.RegisterMessage(
                        "Grupa", "Izaberite grupu.");
                }
                if (cmbTezina.SelectedIndex == -1)
                {
                    notification.RegisterMessage(
                       "Tezina", "Izaberite tezinu.");
                }
                if (txtBroj.Text.Trim() == String.Empty)
                {
                    notification.RegisterMessage(
                       "Broj", "Unesite broj elementa.");
                }
                if (txtVrednostPreskoka.Text.Trim() != String.Empty && !isFloat(txtVrednostPreskoka.Text))
                {
                    notification.RegisterMessage(
                        "VrednostPreskoka", "Nepravilan format za vrednost.");
                }
            }
        }

        private bool isFloat(string s)
        {
            // NOTE: NumberStyles.Float sprecava situaciju da se umesto zareza unese
            // tacka (koja bi se tumacila kao celobrojni separator za grupe)
            NumberStyles numStyles = NumberStyles.Float & ~NumberStyles.AllowExponent;

            float dummy;
            return float.TryParse(s, numStyles, null, out dummy);
        }

        private void add()
        {
            updateEntityFromUI(element);
            validateEntity(element);
            checkBusinessRulesOnAdd(element);
            if (persistEntity)
                insertEntity(element);
        }

        private void updateEntityFromUI(Element e)
        {
            e.Naziv = txtNaziv.Text.Trim();
            e.EngleskiNaziv = txtEngNaziv.Text.Trim();
            e.NazivPoGimnasticaru = txtNazivGim.Text.Trim();
            e.changeSprava(selectedSprava());
            if (!chbTablicniElement.Checked)
            {
                e.ponistiPolozajUTablici();
            }
            else
            {
                setPolozajUTabliciFromUI();
            }
            if (txtVrednostPreskoka.Enabled)
            {
                if (txtVrednostPreskoka.Text.Trim() != String.Empty)
                    element.VrednostPreskoka = float.Parse(txtVrednostPreskoka.Text);
                else
                    element.VrednostPreskoka = null;
            }
            if (!editMode && varijanta)
                parent.dodajVarijantu(e);
        }

        private void validateEntity(Element e)
        {
            Notification notification = new Notification();
            e.validate(notification);
            if (!notification.IsValid())
                throw new BusinessException(notification);
        }

        private void checkBusinessRulesOnAdd(Element e)
        {
            // TODO: Nek bude moguce da dva tablicna elementa iste sprave, grupe,
            // tezine i broja, a razlicitog podbroja imaju isti naziv (promeni ovo i
            // kod checkUpdate)

            Notification notification = new Notification();
            ElementDAO elementDAO = DAOFactoryFactory.DAOFactory.GetElementDAO();
        
            if (e.Naziv != ""
            && elementDAO.postojiElement(e.Sprava, e.Naziv))
            {
                notification.RegisterMessage("Naziv", "Element sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
            if (e.EngleskiNaziv != ""
            && elementDAO.postojiElementEng(e.Sprava, e.EngleskiNaziv))
            {
                notification.RegisterMessage("EngleskiNaziv", "Element sa datim engleskim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
            if (e.NazivPoGimnasticaru != ""
            && elementDAO.postojiElementGim(e.Sprava, e.NazivPoGimnasticaru))
            {
                notification.RegisterMessage("NazivPoGimnasticaru", "Element sa datim nazivom po gimnasticaru vec postoji.");
                throw new BusinessException(notification);
            }
            if (e.IsTablicniElement)
            {
                if (elementDAO.postojiElement(e.Sprava, e.Grupa,
                    e.Broj, e.PodBroj))
                {
                    notification.RegisterMessage("Grupa", "Vec postoji element sa datim brojem za datu spravu i grupu.");
                    throw new BusinessException(notification);
                }
            }
        }

        private void insertEntity(Element e)
        {
            DAOFactoryFactory.DAOFactory.GetElementDAO().MakePersistent(e);
        }

        private void update()
        {
            updateEntityFromUI(element);
            validateEntity(element);
            checkBusinessRulesOnUpdate(element);
            if (persistEntity)
                updateEntity(element);
        }

        private void checkBusinessRulesOnUpdate(Element e)
        {
            Notification notification = new Notification();
            ElementDAO elementDAO = DAOFactoryFactory.DAOFactory.GetElementDAO();

            if (e.Sprava != oldSprava || e.Naziv != oldNaziv)
            {
                if (e.Naziv != "" && elementDAO.postojiElement(e.Sprava, e.Naziv))
                {
                    notification.RegisterMessage("Naziv", "Element sa datim nazivom vec postoji.");
                    throw new BusinessException(notification);
                }
            }
            if (e.Sprava != oldSprava || e.EngleskiNaziv != oldEngleskiNaziv)
            {
                if (e.EngleskiNaziv != ""
                && elementDAO.postojiElementEng(e.Sprava, e.EngleskiNaziv))
                {
                    notification.RegisterMessage("EngleskiNaziv", "Element sa datim engleskim nazivom vec postoji.");
                    throw new BusinessException(notification);
                }
            }
            if (e.Sprava != oldSprava || e.NazivPoGimnasticaru != oldNazivPoGimnasticaru)
            {
                if (e.NazivPoGimnasticaru != ""
                && elementDAO.postojiElementGim(e.Sprava, e.NazivPoGimnasticaru))
                {
                    notification.RegisterMessage("NazivPoGimnasticaru", "Element sa datim nazivom po gimnasticaru vec postoji.");
                    throw new BusinessException(notification);
                }
            }
            if (e.IsTablicniElement)
            {
                if (e.Sprava != oldSprava
                || e.Grupa != oldGrupa
                || e.Tezina != oldTezina
                || e.Broj != oldBroj
                || e.PodBroj != oldPodBroj)
                {
                    if (elementDAO.postojiElement(e.Sprava, e.Grupa,
                        e.Broj, e.PodBroj))
                    {
                        notification.RegisterMessage("Grupa", "Vec postoji element sa datim brojem za datu spravu i grupu.");
                        throw new BusinessException(notification);
                    }
                }
            }
        }

        private void updateEntity(Element e)
        {
            DAOFactoryFactory.DAOFactory.GetElementDAO().MakePersistent(e);
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
            // TODO: Vidi sta treba da se radi sa txtVrednostPreskoka u ovom metodu.
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
                element, false);
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
            if (lstVarijante.SelectedItem == null)
                return;

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
            ElementForm form = new ElementForm(varijanta);
            if (form.ShowDialog() == DialogResult.OK)
            {
                updateVarijanteUI();
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

        private bool isDirty()
        {
            // TODO
            return true;
        }

        private void ElementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closedByOK && !closedByCancel)
            {
                // zatvoreno pomocu X
                if (isDirty())
                {
                    bool canClose = MessageBox.Show(
                        "Izmene koje ste uneli nece biti sacuvane?", "Klub",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) ==
                        DialogResult.OK;
                    e.Cancel = !canClose;
                }
            }
        }

        private void ElementForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (e.CloseReason == CloseReason.UserClosing)
            if (!closedByOK && !closedByCancel)
            {
                discardChanges();
            }
        }

    }
}