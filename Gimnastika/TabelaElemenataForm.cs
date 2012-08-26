using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Domain;
using Gimnastika.Dao;
using Gimnastika.Exceptions;
using Gimnastika.Win32;
using NHibernate.Context;
using Gimnastika.Data;
using NHibernate;

namespace Gimnastika
{
    public partial class TabelaElemenataForm : Form
    {
        private int brojVrsta;
        private SizeF elementSizePxl;
        private int tezineHeaderHeightPxl;
        private int grupaHeaderHeightPxl;
        private float zoom;
        private float xMargin = 10;
        private float yMargin = 10;

        private Color itemBorderColor;
        private Color itemBorderSelectedColor;
        private Color itemTextColor;

        private Color itemTextSelectedColor;
        public Color ItemTextSelectedColor
        {
            get { return itemTextSelectedColor; }
        }

        private Color tabelaBackColor;
        private Color headerBorderColor;
        private Color headerTezinaTextColor;
        private Color headerGrupaTextColor;

        private Pen itemBorderPen = null;
        public Pen ItemBorderPen
        {
            get {
                if (itemBorderPen == null)
                    itemBorderPen = new Pen(itemBorderColor);
                return itemBorderPen; 
            }
        }

        private Pen itemBorderSelectedPen = null;
        public Pen ItemBorderSelectedPen
        {
            get
            {
                if (itemBorderSelectedPen == null)
                    itemBorderSelectedPen = new Pen(itemBorderSelectedColor);
                return itemBorderSelectedPen;
            }
        }

        private Pen eraseBorderPen = null;
        public Pen EraseBorderPen
        {
            get
            {
                if (eraseBorderPen == null)
                    eraseBorderPen = new Pen(tabelaBackColor);
                return eraseBorderPen;
            }
        }

        private Brush itemTextBrush = null;
        public Brush ItemTextBrush
        {
            get {
                if (itemTextBrush == null)
                    itemTextBrush = new SolidBrush(itemTextColor);
                return itemTextBrush; 
            }
        }

        private Brush itemTextSelectedBrush = null;
        public Brush ItemTextSelectedBrush
        {
            get
            {
                if (itemTextSelectedBrush == null)
                    itemTextSelectedBrush = new SolidBrush(itemTextSelectedColor);
                return itemTextSelectedBrush;
            }
        }

        private Font itemFont;
        public Font ItemFont
        {
            get { return itemFont; }
        }

        private Font itemBoldFont;
        public Font ItemBoldFont
        {
            get { return itemBoldFont; }
        }

        private Pen headerBorderPen;
        public Pen HeaderBorderPen
        {
            get {
                if (headerBorderPen == null)
                    headerBorderPen = new Pen(headerBorderColor);
                return headerBorderPen; 
            }
        }

        private Brush headerTezinaTextBrush;
        public Brush HeaderTezinaTextBrush
        {
            get {
                if (headerTezinaTextBrush == null)
                    headerTezinaTextBrush = new SolidBrush(headerTezinaTextColor);
                return headerTezinaTextBrush; 
            }
        }

        private Font headerTezinaFont;
        public Font HeaderTezinaFont
        {
            get { return headerTezinaFont; }
        }

        private Brush headerGrupaTextBrush;
        public Brush HeaderGrupaTextBrush
        {
            get
            {
                if (headerGrupaTextBrush == null)
                    headerGrupaTextBrush = new SolidBrush(headerGrupaTextColor);
                return headerGrupaTextBrush;
            }
        }

        private Font headerGrupaFont;
        public Font HeaderGrupaFont
        {
            get { return headerGrupaFont; }
        }

        ElementTableItem[,] elementItems;
        private BindingListView<Element> elementi;

        ContextMenu contextMenuZoom;
        bool fitWidth = false;

        IList<Grupa> grupe;

        ElementTableItem clickedItem;
        ElementTableItem changingElement;
        ElementTableItem clipboard;
        TabelaElemenataFormRezimRada rezimRada;

        public enum TabelaElemenataFormRezimRada
        { 
            Edit, 
            Select 
        };

        private Dictionary<int, Element> izabrani = new Dictionary<int, Element>();

        public List<Element> IzabraniElementi
        {
            get
            {
                List<Element> result = new List<Element>();
                Dictionary<int, Element>.ValueCollection values =
                   izabrani.Values;
                foreach (Element e in values)
                    result.Add(e);
                return result;
            }
        }

        public TabelaElemenataForm(TabelaElemenataFormRezimRada rezimRada, Sprava sprava)
        {
            InitializeComponent();
            initUI();
            MinimumSize = SystemInformation.MinimizedWindowSize + 
                new Size(0, panel1.Height + panelHeader.Height + 100);

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    elementi = new BindingListView<Element>(
                        new List<Element>(DAOFactoryFactory.DAOFactory.GetElementDAO().FindAll()));
                    grupe = DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll();

                    float elementSizeMM = Math.Min(210 / 4, 297 / 4);
                    float tezineHeaderHeightMM = 7;
                    float grupaHeaderHeightMM = 5;

                    Graphics g = CreateGraphics();
                    elementSizePxl = (Size)Point.Round(
                        mmToPixel(g, new PointF(elementSizeMM, elementSizeMM)));
                    tezineHeaderHeightPxl = Point.Round(
                        mmToPixel(g, new PointF(0, tezineHeaderHeightMM))).Y;
                    grupaHeaderHeightPxl = Point.Round(
                        mmToPixel(g, new PointF(0, grupaHeaderHeightMM))).Y;
                    g.Dispose();

                    panelHeader.Height = tezineHeaderHeightPxl + grupaHeaderHeightPxl + 1;

                    this.rezimRada = rezimRada;
                    if (rezimRada == TabelaElemenataFormRezimRada.Select)
                    {
                        setSpravaCombo(sprava);
                        cmbSprava.Enabled = false;
                    }
                    else
                    {
                        btnOK.Enabled = false;
                        btnOK.Visible = false;
                    }

                    cmbSprava.SelectedIndexChanged += cmbSprava_SelectedIndexChanged;
                    cmbGrupa.SelectedIndexChanged += cmbGrupa_SelectedIndexChanged;

                    disableTrackBar();
                    promeniGrupu();
                    zumiraj(100);
                    panelTabela.MouseWheel += new MouseEventHandler(panelTabela_MouseWheel);
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        void panelTabela_MouseWheel(object sender, MouseEventArgs e)
        {
            disableTrackBar();
        }

        private void setSpravaCombo(Sprava sprava)
        {
            cmbSprava.SelectedIndex = sprava - Sprava.Parter;
        }

        private PointF mmToPixel(Graphics g, PointF mm)
        {
            PointF result = new PointF();
            result.X = mm.X * g.DpiX / 25.4f;
            result.Y = mm.Y * g.DpiY / 25.4f;
            return result;
        }

        private void TabelaElemenataForm_Shown(object sender, EventArgs e)
        {
            // Postavljanje fokusa na panel je potrebno da bi moglo da se skroluje
            // tockicem misa.

            // Mora ovde zato sto u konstruktoru postavljanje fokusa nema efekta.
            panelTabela.Focus();
        }

        private void initUI()
        {
            Text = "Tabela elemenata";

            tabelaBackColor = SystemColors.Window;
            itemBorderColor = SystemColors.WindowText;
            itemBorderSelectedColor = Color.Fuchsia;
            itemTextColor = SystemColors.WindowText;
            itemTextSelectedColor = Color.Fuchsia;
            itemFont = new Font("Arial", 8);
            itemBoldFont = new Font("Arial", 8, FontStyle.Bold);
            headerBorderColor = SystemColors.WindowText;
            headerTezinaTextColor = Color.Blue;
            headerTezinaFont = new Font("Arial", 14, FontStyle.Bold | FontStyle.Italic);
            headerGrupaTextColor = SystemColors.WindowText;
            headerGrupaFont = new Font("Arial", 10, FontStyle.Bold);

            cmbSprava.Items.Clear();
            cmbSprava.Items.AddRange(Resursi.SpravaNazivTable);
            cmbSprava.SelectedIndex = 0;

            cmbGrupa.Items.Clear();
            cmbGrupa.Items.AddRange(Resursi.GrupaNazivTable);
            cmbGrupa.SelectedIndex = 0;

            string[] zoomItems = { "10%", "25%", "50%", "75%", "100%", "125%",
                "150%", "200%", "250%", "400%", "-", "&Sirina prozora"};
            contextMenuZoom = new ContextMenu();
            EventHandler zoomHandler = new EventHandler(menuZoomOnClick);

            foreach (string zoomItem in zoomItems)
                contextMenuZoom.MenuItems.Add(zoomItem, zoomHandler);
            contextMenuZoom.Popup += new EventHandler(contextMenuZoom_Popup);

            this.KeyPreview = true; // the form will receive key events before the 
                                    // event is passed to the control that has focus.

            // NOTE: Kada se koristi automatsko skrolovanje, kontrola koja se skroluje
            // automatski obradjuje dogadjaje MouseWheel (ne mora da se pise handler)
            panelTabela.AutoScroll = true;
            panelTabela.BackColor = tabelaBackColor;

            panelHeader.BackColor = tabelaBackColor;
        }

        void promeniGrupu()
        {
            filterAndSortElements();
            createItems();
            odrediVirtuelnuClientOblast();

            panelTabela.AutoScrollPosition = Point.Empty;
            panelTabela.Invalidate();
            panelTabela.Focus();
        }

        private void filterAndSortElements()
        {
            string filter = String.Format("Sprava = {0} ", selectedSprava());
            filter += String.Format(" AND Grupa = {0} ", selectedGrupa());
            (elementi as IBindingListView).Filter = filter;

            PropertyDescriptor propDesc =
                TypeDescriptor.GetProperties(typeof(Element))["GrupaBroj"];
            (elementi as IBindingListView).ApplySort(propDesc, ListSortDirection.Ascending);
        }

        private void createItems()
        {
            if (elementi.Count > 0)
                brojVrsta = (elementi[elementi.Count - 1].Broj - 1) / 6 + 1;
            else
                brojVrsta = 1;
            
            int maxBroj = brojVrsta * 6;
            int elemIndex = 0;
            elementItems = new ElementTableItem[brojVrsta, 6];
            for (int broj = 1; broj <= maxBroj; broj++)
            {
                Element elem = null;
                if (elemIndex < elementi.Count && elementi[elemIndex].Broj == broj
                && !elementi[elemIndex].isVarijanta())
                {
                    elem = elementi[elemIndex++];
                    // preskoci varijante (ako ih slucajno ima)
                    while (elemIndex < elementi.Count
                    && elementi[elemIndex].isVarijanta())
                        elemIndex++;
                }
                createItem(broj, elem);
            }
        }

        private void odrediVirtuelnuClientOblast()
        {
            panelTabela.AutoScrollMinSize = new Size(
                (int)(6 * elementSizePxl.Width * zoom / 100 + 2 * xMargin),
                (int)(brojVrsta * elementSizePxl.Height * zoom / 100 + 2 * yMargin));
            podesiKlizace();
        }

        private void podesiKlizace()
        {
            panelTabela.HorizontalScroll.SmallChange =
                (int)(elementSizePxl.Width * zoom / 100 / 3);
            panelTabela.HorizontalScroll.LargeChange = panelTabela.Width;
            panelTabela.VerticalScroll.SmallChange =
                (int)(elementSizePxl.Height * zoom / 100 / 3);
            panelTabela.VerticalScroll.LargeChange = panelTabela.Height;
        }

        private void panelTabela_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                return;

            if (fitWidth)
                zumirajFitWidth();
            else
                podesiKlizace();
        }

        private void panelTabela_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
      //      g.TranslateTransform(xMargin, yMargin);
            g.ScaleTransform(zoom / 100, zoom / 100);

            PointF pt = panelTabela.AutoScrollPosition;  // vraca negativne koordinate
            pt = new PointF(pt.X + xMargin, pt.Y + yMargin);
            pt.X /= zoom / 100;
            pt.Y /= zoom / 100;

            foreach (ElementTableItem item in elementItems)
            {
                item.draw(g, pt);
            }
            if (clipboard != null && clipboard.Sprava == selectedSprava()
            && clipboard.Grupa == selectedGrupa())
            {
                clipboard.draw(g, pt);
            }
            foreach (ElementTableItem item in elementItems)
            {
                if (item.Selected)
                    item.draw(g, pt);
            }
        }

        // TODO: Ne skroluje heder tabele kada se pritiskaju leva i desna strelica
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Point pt = panelTabela.AutoScrollPosition;

            pt.X = -pt.X;
            pt.Y = -pt.Y;

            SizeF zoomedElementSize = new SizeF(
                elementSizePxl.Width * zoom / 100, elementSizePxl.Height * zoom / 100);

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if ((e.Modifiers & Keys.Control) == Keys.Control)
                        pt.X += panelTabela.Width;
                    else
                        pt.X += (int)zoomedElementSize.Width;
                    break;

                case Keys.Left:
                    if ((e.Modifiers & Keys.Control) == Keys.Control)
                        pt.X -= panelTabela.Width;
                    else
                        pt.X -= (int)zoomedElementSize.Width;
                    break;

                case Keys.Down:
                    pt.Y += (int)zoomedElementSize.Height;
                    break;

                case Keys.Up:
                    pt.Y -= (int)zoomedElementSize.Height;
                    break;

                case Keys.PageDown:
                    pt.Y += (int)(zoomedElementSize.Height * 
                        (panelTabela.Height / (int)zoomedElementSize.Height));
                    break;

                case Keys.PageUp:
                    pt.Y -= (int)(zoomedElementSize.Height * 
                        (panelTabela.Height / (int)zoomedElementSize.Height));
                    break;

                case Keys.Home:
                    pt = Point.Empty;
                    break;

                case Keys.End:
                    pt.Y = 1000000;
                    break;
            }
            panelTabela.AutoScrollPosition = pt;
        }

        private Sprava selectedSprava()
        {
            return ((SpravaNazivPair)cmbSprava.SelectedItem).Sprava;
        }

        private GrupaElementa selectedGrupa()
        {
            return ((GrupaNazivPair)cmbGrupa.SelectedItem).Grupa;
        }

        private void cmbSprava_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearClipboard();
            if (selectedSprava() == Sprava.Parter && selectedGrupa() == GrupaElementa.V)
                cmbGrupa.SelectedIndex = 0;
            else
                promeniGrupu();
            panelHeader.Invalidate();
        }

        private void cmbGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            promeniGrupu();
            panelHeader.Invalidate();
        }

        private void cmbZoom_DropDownClosed(object sender, EventArgs e)
        {
            panelTabela.Focus();
        }

        private void cmbSprava_DropDownClosed(object sender, EventArgs e)
        {
            // Ovo je zbog situacija kada se kombo samo otvori i zatvori, pri cemu
            // se nije birala stavka iz liste. Tada kombo ostaje u fokusu (bez obzira
            // na to da li mu je svojstvo TabStop podeseno na false), pa je potrebno
            // skloniti fokus, kako okretanje tockica misa ne bi promenilo aktivnu 
            // stavku.
            panelTabela.Focus();
        }

        private void cmbGrupa_DropDownClosed(object sender, EventArgs e)
        {
            panelTabela.Focus();
        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            drawHeader(e.Graphics);
        }

        private void drawHeader(Graphics g)
        {
            drawTezineHeader(g);
            drawGrupaHeader(g);
        }

        private void drawTezineHeader(Graphics g)
        {
            string[] tezine = { "A = 0,10", "B = 0,20", "C = 0,30", "D = 0,40", 
                "E = 0,50", "F = 0,60 (G = 0,70)"};
            for (int i = 0; i < 6; i++)
            {
                ElementTableItem item = elementItems[0, i];
                RectangleF tezinaRect = new RectangleF(
                    xMargin + item.Location.X * zoom / 100, 0,
                    item.Size.Width * zoom / 100, tezineHeaderHeightPxl);
                drawTezinaHeader(tezine[i], tezinaRect, g);
            }

        }

        private void drawTezinaHeader(string text, RectangleF rect, Graphics g)
        {
            Pen pen = HeaderBorderPen;
            Brush brush = HeaderTezinaTextBrush;
            Font f = HeaderTezinaFont;

            Point autoScrollPosition = panelTabela.AutoScrollPosition;
            rect.Offset(autoScrollPosition.X, 0);
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            StringFormat fmt = new StringFormat();
            fmt.Alignment = fmt.LineAlignment = StringAlignment.Center;
            g.DrawString(text, f, brush, rect, fmt);
        }

        private float getTabelaWidth()
        {
            return 6 * elementSizePxl.Width;
        }

        private float getTabelaHeight()
        {
            return brojVrsta * elementSizePxl.Width;
        }

        private void drawGrupaHeader(Graphics g)
        {
            Pen pen = HeaderBorderPen;
            Brush brush = HeaderGrupaTextBrush;
            Font f = HeaderGrupaFont;
            
            Point autoScrollPosition = panelTabela.AutoScrollPosition;
            RectangleF rect = new RectangleF(
                new PointF(xMargin, tezineHeaderHeightPxl),
                new SizeF(getTabelaWidth() * zoom / 100, grupaHeaderHeightPxl));
            rect.Offset(autoScrollPosition.X, 0);
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "GRUPA ELEMENATA " + selectedGrupa() + ": ";
            Grupa grupa = findGrupa(selectedSprava(), selectedGrupa());
            if (grupa != null)
            {
                string naziv = "" + grupa.Naziv;
                if (naziv != "")
                    naziv += " - ";
                naziv += grupa.EngNaziv;
                text += naziv;
            }
            StringFormat fmt = new StringFormat();
            fmt.LineAlignment = StringAlignment.Center;
            g.DrawString(text, f, brush, rect, fmt);
        }

        private Grupa findGrupa(Sprava sprava, GrupaElementa grupaElementa)
        {
            if (grupe == null)
                return null;
            foreach (Grupa g in grupe)
            {
                if (g.Sprava == sprava && g.GrupaElemenata == grupaElementa)
                    return g;
            }
            return null;
        }

        private void panelTabela_Scroll(object sender, ScrollEventArgs e)
        {
            disableTrackBar();
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                RECT rect;
                rect.left = 0;
                rect.top = 0;
                rect.right = panelHeader.Width;
                rect.bottom = panelHeader.Height;

                int scroll = e.NewValue - e.OldValue;
                NativeMethods.ScrollWindow(panelHeader.Handle, -scroll, 0, 
                    ref rect, ref rect);
                panelHeader.Update();
            }
        }

        private void btnZoomDropDown_Click(object sender, EventArgs e)
        {
            disableTrackBar();
            contextMenuZoom.Show(txtZoom, new Point(0, txtZoom.Height));
        }

        private void menuZoomOnClick(object sender, EventArgs e)
        {
            string item = (sender as MenuItem).Text.Replace("&", "");
            if (item.IndexOf('%') != -1)
            {
                float newZoom = float.Parse(item.Replace("%", ""));
                zumiraj(newZoom);
            }
            else if (item == "Sirina prozora")
            {
                zumirajFitWidth();
            }
            panelTabela.Focus();
        }

        private void txtZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            disableTrackBar();
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                // '\r' je 13, 'n' je 10
                float newZoom;
                if (float.TryParse(txtZoom.Text.Replace("%", "").Replace(',', '.'),
                    out newZoom) && newZoom >= 10)
                {

                    // TODO: Neka CurrentCulture za program bude srpski. Ispitaj sta treba
                    // da se promeni na bazi da bi i ona bila srpska

                    zumiraj(newZoom);
                    panelTabela.Focus();
                }
                else
                {
                    showZoomInTextBox();
                }
            }
        }

        private void zumiraj(float newZoom)
        {
            // odredi neskalirane koordinate tacke tabele koja se nalazi u sredini
            // panelTabele
            float sredinaX = (panelTabela.Width / 2 - panelTabela.AutoScrollPosition.X
                - xMargin) / (zoom / 100);
            float sredinaY = (panelTabela.Height / 2 - panelTabela.AutoScrollPosition.Y
                - yMargin) / (zoom / 100);

            zoom = newZoom;
            showZoomInTextBox();
            fitWidth = false;

            odrediVirtuelnuClientOblast();
            scrollToClientCenter(new PointF(sredinaX, sredinaY));

            panelTabela.Invalidate();
            panelHeader.Invalidate();
        }

        private void scrollToClientCenter(PointF nezumiranaTacka)
        {
            PointF zumiranaTacka = new PointF(nezumiranaTacka.X * zoom / 100,
                nezumiranaTacka.Y * zoom / 100);
            panelTabela.AutoScrollPosition = Point.Round(new PointF(
                zumiranaTacka.X + xMargin - panelTabela.Width / 2,
                zumiranaTacka.Y + yMargin - panelTabela.Height / 2));
        }

        private void showZoomInTextBox()
        {
            txtZoom.Text = ((int)zoom).ToString() + '%';
        }

        private void zumirajFitWidth()
        {
            // zoom se dobija iz formule:
            // 2 * xMargin + getTabelaWidth() * zoom = panelTabela.Width

            float width = 2 * xMargin + getTabelaWidth();
            float height = 2 * yMargin + getTabelaHeight();
            float newZoom;
            if (width / height >= panelTabela.Width / panelTabela.Height)
            {
                // tabela cela staje unutar prozora
                newZoom = (panelTabela.Width - 2 * xMargin) / getTabelaWidth() * 100;
            }
            else
            {
                // tabela nece stati po visini unutar prozora, pa mora da se
                // uracuna i sirina vertikalnog klizaca (kada se koristi
                // automatsko skrolovanje, klizaci se nalaze UNUTAR klijent
                // oblasti)
                newZoom = (panelTabela.Width - 2 * xMargin -
                    SystemInformation.VerticalScrollBarWidth) / getTabelaWidth() * 100;
            }
            zumiraj(newZoom);
            fitWidth = true;
        }

        private void txtZoom_Leave(object sender, EventArgs e)
        {
            showZoomInTextBox();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            disableTrackBar();
            panelTabela.Focus();
        }

        private void panelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            disableTrackBar();
            panelTabela.Focus();
        }

        private void panelTabela_MouseDown(object sender, MouseEventArgs e)
        {
            disableTrackBar();
            panelTabela.Focus();
        }

        void contextMenuZoom_Popup(object sender, EventArgs e)
        {
            foreach (MenuItem mi in contextMenuZoom.MenuItems)
            { 
                string text = mi.Text.Replace("&", "");
                mi.Checked = false;
                if (text == txtZoom.Text)
                    mi.Checked = true;
                if (text.IndexOf("irina prozora") != -1 && fitWidth == true)
                    // izostavio sam S iz Sirina da bi kod radio i kad uvedem srpska
                    // slova
                    mi.Checked = true;
            }
        }

        private void cmbGrupa_DropDown(object sender, EventArgs e)
        {
            disableTrackBar();
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
                    cmbGrupa.Items.Add(new GrupaNazivPair (GrupaElementa.V, "V" ));
            }

        }

        private PointF clientToTable(PointF pt)
        {
            float scale = zoom / 100;
            return new PointF((pt.X - panelTabela.AutoScrollPosition.X - xMargin) / scale,
                (pt.Y - panelTabela.AutoScrollPosition.Y - yMargin) / scale);
        }

        private ElementTableItem getItem(PointF tablePt)
        {
            if (tablePt.X < 0 || tablePt.Y < 0)
                return null;

            int j = (int)Math.Floor((double)tablePt.X / elementSizePxl.Width);
            int i = (int)Math.Floor((double)tablePt.Y / elementSizePxl.Height);

            if (i >= brojVrsta || j >= 6)
                return null;

            return elementItems[i, j];
        }

        private void panelTabela_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            clickedItem = getItem(clientToTable(new PointF(e.X, e.Y)));
            if (clickedItem == null)
                return;

            bool showDodajElement = true;
            bool showPromeniElement = true;
            bool showPromeniVelicinuSlike = true;
            bool showBrisiElement = true;
            bool showCut = true;
            bool showPaste = true;
            bool showIzaberiElement = true;
            bool showIzaberiVarijantu = true;
            bool showSeparator1 = true;
            bool showSeparator2 = true;

            if (rezimRada == TabelaElemenataFormRezimRada.Select)
            {
                showDodajElement = false;
                showPromeniElement = false;
                showPromeniVelicinuSlike = false;
                showBrisiElement = false;
                showCut = false;
                showPaste = false;
                showIzaberiElement = clickedItem.Element != null;

                if (clickedItem.Element != null
                && !clickedItem.Element.isVarijanta()
                && clickedItem.Element.Varijante.Count > 0)
                {
                    showIzaberiVarijantu = true;
                    mnIzaberiVarijantu.DropDownItems.Clear();
                    foreach (Element elem in clickedItem.Element.Varijante)
                    {
                        ToolStripMenuItem item =
                            new ToolStripMenuItem(elem.VarijantaString);
                        item.Tag = elem;
                        item.Checked = izabrani.ContainsKey(elem.Id);
                        item.Click += mnIzaberiVarijantu_Click;
                        mnIzaberiVarijantu.DropDownItems.Add(item);
                    }
                }
                else
                    showIzaberiVarijantu = false;
            }
            else
            {
                showIzaberiElement = false;
                showIzaberiVarijantu = false;
                if (clickedItem.Element != null)
                {
                    showDodajElement = false;
                    showPromeniElement = true;
                    showPromeniVelicinuSlike = clickedItem.Element.Slike.Count > 0;
                    showBrisiElement = true;
                    showCut = true;
                    showPaste = false;                    
                }
                else
                {
                    showDodajElement = true;
                    showPromeniElement = false;
                    showPromeniVelicinuSlike = false;
                    showBrisiElement = false;
                    showCut = false;
                    showPaste = clipboard != null
                        && clipboard.Sprava == clickedItem.Sprava;
                }
            }

            bool sector1Visible = showDodajElement || showPromeniElement
                || showPromeniVelicinuSlike || showBrisiElement;
            bool sector2Visible = showCut || showPaste;
            bool sector3Visible = showIzaberiElement || showIzaberiVarijantu;

            showSeparator1 = sector1Visible && (sector2Visible || sector3Visible);
            showSeparator2 = sector3Visible && (sector1Visible || sector2Visible);
            if (showSeparator1 && showSeparator2 && !sector3Visible)
            {
                showSeparator2 = false;
            }

            mnDodajElement.Visible = showDodajElement;
            mnPromeniElement.Visible = showPromeniElement;
            mnPromeniVelicinuSlike.Visible = showPromeniVelicinuSlike;
            mnBrisiElement.Visible = showBrisiElement;
            mnCut.Visible = showCut;
            mnPaste.Visible = showPaste;
            mnIzaberiElement.Visible = showIzaberiElement;
            mnIzaberiVarijantu.Visible = showIzaberiVarijantu;
            mnSeparator1.Visible = showSeparator1;
            mnSeparator2.Visible = showSeparator2;

            Element element = null;
            if (clickedItem != null)
                element = clickedItem.Element;
            mnIzaberiElement.Checked = element != null
                && izabrani.ContainsKey(element.Id);

            contextMenuTabela.Show(panelTabela, e.Location);
        }

        private void mnDodajElement_Click(object sender, EventArgs e)
        {
            if (clickedItem == null || clickedItem.Element != null)
                return;

            clearClipboard();
            ElementForm form = new ElementForm(null, selectedSprava(), selectedGrupa(),
                clickedItem.Broj, Element.getTezina(clickedItem.Broj));
            if (form.ShowDialog() == DialogResult.OK)
            {
                Element element = form.Element;
                elementi.Add(element);
                filterAndSortElements();
                createItem(element.Broj, element);

                panelTabela.Invalidate();
                panelTabela.Focus();
            }
        }

        private void mnPromeniElement_Click(object sender, EventArgs e)
        {
            if (clickedItem == null || clickedItem.Element == null)
                return;

            clearClipboard();
            Element element = clickedItem.Element;
            ElementForm form = new ElementForm(element.Id, element.Sprava, element.Grupa,
                element.Broj, element.Tezina);
            if (form.ShowDialog() == DialogResult.OK)
            {
                elementi[elementi.IndexOf(element)] = form.Element;
                clickedItem.Element = form.Element;
                panelTabela.Invalidate();
                panelTabela.Focus();
            }
        }

        private void mnPromeniVelicinuSlike_Click(object sender, EventArgs e)
        {
            if (clickedItem == null || clickedItem.Element == null
            || clickedItem.Slika == null)
                return;

            changingElement = clickedItem;
            enableTrackBar();

        }

        private void disableTrackBar()
        {
            if (changingElement == null)
                return;

            // snimi promenu velicine
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    DAOFactoryFactory.DAOFactory.GetElementDAO().MakePersistent(changingElement.Element);
                    session.Transaction.Commit();
                    
                    lblVelicinaSlike.Enabled = false;
                    trackBar1.Enabled = false;
                    changingElement = null;
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void enableTrackBar()
        {
            if (changingElement == null)
                return;

            lblVelicinaSlike.Enabled = true;
            trackBar1.Enabled = true;

            // The Maximum property sets the value of the track bar when
            // the slider is all the way to the right.
            trackBar1.Maximum = 100;

            trackBar1.Minimum = 50;

            // The TickFrequency property establishes how many positions
            // are between each tick-mark.
            trackBar1.TickFrequency = 5;

            // The LargeChange property sets how many positions to move
            // if the bar is clicked on either side of the slider.
            trackBar1.LargeChange = 5;

            // The SmallChange property sets how many positions to move
            // if the keyboard arrows are used to move the slider.
            trackBar1.SmallChange = 1;

            trackBar1.Value = changingElement.Slika.ProcenatRedukcije;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (changingElement == null)
                return;

            changingElement.Slika.ProcenatRedukcije = (byte)trackBar1.Value;

            PointF pt = panelTabela.AutoScrollPosition;  // vraca negativne koordinate
            pt = new PointF(pt.X + xMargin, pt.Y + yMargin);
            pt.X /= zoom / 100;
            pt.Y /= zoom / 100;

            RectangleF rect = new RectangleF(changingElement.Location, changingElement.Size);
            rect.Offset(pt.X, pt.Y);

            panelTabela.Invalidate(Rectangle.Round(rect));
        }

        private void mnBrisiElement_Click(object sender, EventArgs e)
        {
            if (clickedItem == null || clickedItem.Element == null)
                return;

            clearClipboard();
            Element element = clickedItem.Element;
            if (MessageBox.Show("Da li zelite da izbrisete element '" +
                element.ToString() + "' ?", "Potvrda", MessageBoxButtons.OKCancel,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }
            
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    
                    int broj = element.Broj;
                    DAOFactoryFactory.DAOFactory.GetElementDAO().MakeTransient(element);
                    session.Transaction.Commit();

                    elementi.Remove(element);
                    //  filterAndSortElements();
                    createItem(broj, null);

                    panelTabela.Invalidate();
                    panelTabela.Focus();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void createItem(int broj, Element element)
        {
            // TODO: Neka elementItems bude 1-dimenzionalan niz

            if (broj > brojVrsta * 6)
            {
                int oldBrojVrsta = brojVrsta;
                brojVrsta = (broj - 1) / 6 + 1;
                ElementTableItem[,] newElementItems = new ElementTableItem[brojVrsta, 6];
                copyElementItems(newElementItems, elementItems, oldBrojVrsta);
                elementItems = newElementItems;

                int from = oldBrojVrsta * 6 + 1;
                int to = brojVrsta * 6;
                for (int k = from; k <= to; k++)
                    createItem(k, null);
            }

            int i = getRowIndex(broj);
            int j = getColumnIndex(broj);
            ElementTableItem item = new ElementTableItem(selectedSprava(), selectedGrupa(), 
                broj, element,
                new PointF(j * elementSizePxl.Width, i * elementSizePxl.Height),
                elementSizePxl, this);
            elementItems[i, j] = item;
            if (element != null && izabrani.ContainsKey(element.Id))
                item.Selected = true;
        }

        private int getRowIndex(int broj)
        {
            return (broj - 1) / 6;
        }

        private int getColumnIndex(int broj)
        {
            return (broj - 1) % 6;
        }

        private void copyElementItems(ElementTableItem[,] newElementItems, 
            ElementTableItem[,] elementItems, int brojVrsta)
        {
            for (int i = 0; i < brojVrsta; i++)
                for (int j = 0; j < 6; j++)
                    newElementItems[i, j] = elementItems[i, j];
        }

        private void btnNoviElement_Click(object sender, EventArgs e)
        {
            disableTrackBar();
            clearClipboard();
            ElementForm form = new ElementForm(selectedSprava(), selectedGrupa());
            if (form.ShowDialog() == DialogResult.OK)
            {
                Element element = form.Element;
                bool extend = element.Broj > brojVrsta * 6;
                elementi.Add(element);
                filterAndSortElements();
                createItem(element.Broj, element); // prosiruje tabelu ako je potrebno

                if (extend)
                    odrediVirtuelnuClientOblast();
                scrollItemToClientCenter(element.Broj);

                panelTabela.Invalidate();
            }
            panelTabela.Focus();
        }

        private void scrollItemToClientCenter(int broj)
        {
            ElementTableItem item = getItem(broj);
            if (item != null)
            {
                PointF center = item.Location + 
                    new SizeF(item.Size.Width / 2, item.Size.Height / 2);
                scrollToClientCenter(center);
            }
        }

        ElementTableItem getItem(int broj)
        {
            if (broj < 1 || broj > brojVrsta * 6)
                return null;
            return elementItems[getRowIndex(broj), getColumnIndex(broj)];
        }

        private void mnCut_Click(object sender, EventArgs e)
        {
            if (clickedItem != null && clickedItem.Element != null)
            {
                clearClipboard();
                clipboard = clickedItem;
                clipboard.Cutted = true;

                panelTabela.Invalidate();
                panelTabela.Focus();
            }
        }

        private void mnPaste_Click(object sender, EventArgs e)
        {
            if (clickedItem != null && clickedItem.Element == null
            && clipboard != null && clipboard.Sprava == clickedItem.Sprava)
            {
                paste(clipboard, clickedItem);
                clearClipboard();

                panelTabela.Invalidate();
                panelTabela.Focus();
            }
        }

        private void paste(ElementTableItem from, ElementTableItem to)
        {
            if (to != null && to.Element == null
            && from != null && from.Element != null && from.Sprava == to.Sprava)
            {
                // TODO: Proveri da li moze nekako bez originala

                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (session.BeginTransaction())
                    {
                        CurrentSessionContext.Bind(session);

                        Element element = from.Element;
                        Element original = (Element)element.Clone(new TypeAsocijacijaPair[] { 
                            new TypeAsocijacijaPair(typeof(Video)), 
                            new TypeAsocijacijaPair(typeof(Slika)), 
                            new TypeAsocijacijaPair(typeof(Element), "varijante"),
                            new TypeAsocijacijaPair(typeof(Element), "parent") });
                        element.promeniGrupuBroj(to.Grupa, to.Broj);
                        DAOFactoryFactory.DAOFactory.GetElementDAO().MakePersistent(element);
                        session.Transaction.Commit();

                        filterAndSortElements();
                        createItem(element.Broj, element);
                        if (from.Grupa == selectedGrupa())
                            createItem(from.Broj, null);
                    }
                }
                finally
                {
                    CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                }
            }
        }

        private void mnIzaberiElement_Click(object sender, EventArgs e)
        {
            if (clickedItem == null || clickedItem.Element == null)
                return;

            Element elem = clickedItem.Element;
            if (!izabrani.ContainsKey(elem.Id))
            {
                izabrani.Add(elem.Id, elem);
                clickedItem.Selected = true;
            }
            else
            {
                izabrani.Remove(elem.Id);
                clickedItem.Selected = izabranElementIliVarijanta(elem);
            }

            panelTabela.Invalidate();
            panelTabela.Focus();
        }

        private void mnIzaberiVarijantu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mnItem = sender as ToolStripMenuItem;
            Element elem = mnItem.Tag as Element;
            if (!izabrani.ContainsKey(elem.Id))
            {
                izabrani.Add(elem.Id, elem);
                clickedItem.Selected = true;
            }
            else
            {
                izabrani.Remove(elem.Id);
                clickedItem.Selected = izabranElementIliVarijanta(elem.Parent);
            }

            panelTabela.Invalidate();
            panelTabela.Focus();
        }

        private bool izabranElementIliVarijanta(Element element)
        {
            if (izabrani.ContainsKey(element.Id))
                return true;
            foreach (Element e in element.Varijante)
            {
                if (izabrani.ContainsKey(e.Id))
                    return true;
            }
            return false;
        }

        private void clearClipboard()
        {
            if (clipboard != null)
            {
                clipboard.Cutted = false;
                clipboard = null;

                panelTabela.Invalidate();
            }
        }

        private void trackBar1_MouseLeave(object sender, EventArgs e)
        {
            panelTabela.Focus();
        }

        private void TabelaElemenataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            disableTrackBar();
        }

        private void cmbSprava_DropDown(object sender, EventArgs e)
        {
            disableTrackBar();
        }
    }
}