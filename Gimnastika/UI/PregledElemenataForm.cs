using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;
using Gimnastika.Dao;
using NHibernate.Context;
using Gimnastika.Data;
using NHibernate;

namespace Gimnastika.UI
{
    public partial class PregledElemenataForm : Form
    {
        private TabelaElemenata tabela;
        IList<Grupa> grupe;
        List<ElementTableItem> items;
        float zoom = 100f;
        private float xMargin = 0;
        private float yMargin = 0;
        bool fitWidth = true;
        int currentIndex = -1;
        SizeF elementSizePxl;

        public PregledElemenataForm()
        {
            InitializeComponent();
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    List<Element> sviElementi = new List<Element>(DAOFactoryFactory.DAOFactory.GetElementDAO().FindAll());
                    grupe = DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll();

                    Graphics g = CreateGraphics();
                    float elementSizeMM = Math.Min(210 / 4, 297 / 6);
                    elementSizePxl = (Size)Point.Round(
                        Utils.mmToPixel(g, new PointF(elementSizeMM, elementSizeMM)));

                    tabela = new TabelaElemenata(sviElementi, elementSizePxl);
                    currentIndex = -1;

                    //initUI();

                    g.Dispose();

                    //panelTabela.MouseWheel += new MouseEventHandler(panelTabela_MouseWheel);
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void filterItems(Sprava sprava, List<TezinaElementa> tezine, List<GrupaElementa> grupeElemenata)
        {
            items = new List<ElementTableItem>();
            foreach (GrupaElementa grupa in grupeElemenata)
            {
                foreach (ElementTableItem item in tabela.getElementItems(sprava, grupa))
                {
                    if (item.Element == null)
                        continue;
                    foreach (TezinaElementa tezina in tezine)
                    {
                        if (item.Element.Tezina == tezina)
                        {
                            item.Location = new PointF(0.0f, 0.0f);
                            items.Add(item);
                            break;
                        }
                    }
                }
            }
        }

        private void panelSlika_Paint(object sender, PaintEventArgs e)
        {
            float scale = zoom / 100;
            Graphics g = e.Graphics;
            //      g.TranslateTransform(xMargin, yMargin);
            g.ScaleTransform(scale, scale);

            PointF pt = panelSlika.AutoScrollPosition;  // vraca negativne koordinate
            pt = new PointF(pt.X + xMargin, pt.Y + yMargin);
            pt.X /= scale;
            pt.Y /= scale;

            if (currentIndex >= 0)
            {
                // centriraj item
                ElementTableItem currentItem = items[currentIndex];
                float x = (panelSlika.ClientRectangle.Width - currentItem.Size.Width * scale) / 2;
                float y = (panelSlika.ClientRectangle.Height - currentItem.Size.Height * scale) / 2;
                currentItem.Location = new PointF(x / scale, y / scale);

                currentItem.draw(g, pt);
            }
            else
            {
                // draw empty item
                float x = (panelSlika.ClientRectangle.Width - elementSizePxl.Width * scale) / 2;
                float y = (panelSlika.ClientRectangle.Height - elementSizePxl.Height * scale) / 2;
                PointF location = new PointF(x / scale, y / scale);

                RectangleF rect = new RectangleF(location, elementSizePxl);
                rect.Offset(pt.X, pt.Y);

                g.FillRectangle(tabela.ItemBackroundBrush, rect);
                g.DrawRectangle(tabela.ItemBorderPen, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        private void panelSlika_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                return;

            if (fitWidth)
                zumirajFitWidth();
            //else
              //  podesiKlizace();
        }

        private void zumirajFitWidth()
        {
            // zoom se dobija iz formule:
            // 2 * xMargin + getTabelaWidth() * zoom = panelTabela.Width

            if (currentIndex >= 0)
            {
                ElementTableItem currentItem = items[currentIndex];
                SizeF itemSize = currentItem.Size;
                float width = 2 * xMargin + itemSize.Width;
                float height = 2 * yMargin + itemSize.Height;
                float newZoom;
                if (panelSlika.Width / width < panelSlika.Height / height)
                    newZoom = (panelSlika.Width - 2 * xMargin) / itemSize.Width * 100;
                else
                    newZoom = (panelSlika.Height - 2 * yMargin) / itemSize.Height * 100;
                /*if (width / height >= panelSlika.Width / panelSlika.Height)
                {
                    // item ceo staje unutar prozora
                    newZoom = (panelSlika.Width - 2 * xMargin) / itemSize.Width * 100;
                }
                else
                {
                    // item nece stati po visini unutar prozora, pa mora da se
                    // uracuna i sirina vertikalnog klizaca (kada se koristi
                    // automatsko skrolovanje, klizaci se nalaze UNUTAR klijent
                    // oblasti)
                    newZoom = (panelSlika.Width - 2 * xMargin -
                        SystemInformation.VerticalScrollBarWidth) / itemSize.Width * 100;
                }*/
                zumiraj(newZoom);
                fitWidth = true;
            }
        }

        private void zumiraj(float newZoom)
        {
            // odredi neskalirane koordinate tacke koja se nalazi u sredini panelSlike
            float sredinaX = (panelSlika.Width / 2 - panelSlika.AutoScrollPosition.X
                - xMargin) / (zoom / 100);
            float sredinaY = (panelSlika.Height / 2 - panelSlika.AutoScrollPosition.Y
                - yMargin) / (zoom / 100);

            zoom = newZoom;
            //showZoomInTextBox();
            fitWidth = false;

            odrediVirtuelnuClientOblast();
            scrollToClientCenter(new PointF(sredinaX, sredinaY));

            panelSlika.Invalidate();
        }

        private void odrediVirtuelnuClientOblast()
        {
            /*SizeF size = tabela.getScaledTabelaSize(selectedSprava(), selectedGrupa(), zoom / 100);
            panelTabela.AutoScrollMinSize = new Size(
                (int)(size.Width + 2 * xMargin), (int)(size.Height + 2 * yMargin));
            podesiKlizace();*/
        }

        private void podesiKlizace()
        {
            /*SizeF zoomedElementSize = tabela.getScaledElementSizePxl(zoom / 100);
            panelTabela.HorizontalScroll.SmallChange =
                (int)(zoomedElementSize.Width / 3);
            panelTabela.HorizontalScroll.LargeChange = panelTabela.Width;
            panelTabela.VerticalScroll.SmallChange =
                (int)(zoomedElementSize.Height / 3);
            panelTabela.VerticalScroll.LargeChange = panelTabela.Height;*/
        }

        private void scrollToClientCenter(PointF nezumiranaTacka)
        {
            PointF zumiranaTacka = new PointF(nezumiranaTacka.X * zoom / 100,
                nezumiranaTacka.Y * zoom / 100);
            panelSlika.AutoScrollPosition = Point.Round(new PointF(
                zumiranaTacka.X + xMargin - panelSlika.Width / 2,
                zumiranaTacka.Y + yMargin - panelSlika.Height / 2));
        }

        private void PregledElemenataForm_Load(object sender, EventArgs e)
        {
            panelSlika_Resize(null, null);
            drawItem();
        }

        private void PregledElemenataForm_Shown(object sender, EventArgs e)
        {
            btnZatvori.Focus();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex = 0;
                drawItem();
            }
        }

        private void drawItem()
        {
            panelSlika.Invalidate();
            if (currentIndex >= 0)
                setItemNumberTextBox(currentIndex + 1);
        }

        private void setItemNumberTextBox(int num)
        {
            textBox1.Text = String.Format("{0} od {1}", num, items.Count);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                drawItem();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex < items.Count -1)
            {
                currentIndex++;
                drawItem();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (currentIndex < items.Count - 1)
            {
                currentIndex = items.Count - 1;
                drawItem();
            }
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int num = -1;
                try
                {
                    num = Int32.Parse(textBox1.Text);
                }
                catch (FormatException)
                {

                }

                if (num >= 1 && num <= items.Count)
                {
                    currentIndex = num - 1;
                    drawItem();
                }
                else
                {
                    setItemNumberTextBox(currentIndex + 1);
                }
            }
        }

        private void btnPrikazi_Click(object sender, EventArgs e)
        {
            if (selektujElementeControl1.selectedSprava() == Sprava.Undefined)
            {
                MessageDialogs.showMessage("Izaberite spravu.", this.Text);
                return;
            }
            else if (selektujElementeControl1.getSelektovaneTezine().Count == 0)
            {
                MessageDialogs.showMessage("Izaberite tezine.", this.Text);
                return;
            }
            else if (selektujElementeControl1.getSelektovaneGrupe().Count == 0)
            {
                MessageDialogs.showMessage("Izaberite grupe.", this.Text);
                return;
            }
            filterItems(selektujElementeControl1.selectedSprava(), selektujElementeControl1.getSelektovaneTezine(),
                selektujElementeControl1.getSelektovaneGrupe());
            if (items.Count == 0)
            {
                currentIndex = -1;
                MessageDialogs.showMessage("Ne postoje elementi za zadate kriterijume.", this.Text);
            }
            else
                currentIndex = 0;            
            drawItem();
        }

    }
}