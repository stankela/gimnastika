using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Gimnastika.Domain;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Gimnastika
{
    public class ElementTableItem
    {
        private Sprava sprava;
        public Sprava Sprava
        {
            get { return sprava; }
        }

        private GrupaElementa grupa;
        public GrupaElementa Grupa
        {
            get { return grupa; }
        }

        private int broj;
        public int Broj
        {
            get { return broj; }
        }

        private Element element;
        public Element Element
        {
            get { return element; }
        }

        private PointF location;
        public PointF Location
        {
            get { return location; }
            set { location = value; }
        }

        private SizeF size;
        public SizeF Size
        {
            get { return size; }
            set { size = value; }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        private Image Image
        {
            get
            {
                Slika s = Slika;
                if (s != null)
                    return s.Image;
                else
                    return null;
            }
        }

        private Image selectedImage;
        private Image SelectedImage
        {
            get
            {
                if (selectedImage == null)
                    selectedImage = createSelectedImage();
                return selectedImage;
            }
        }

        private Image createSelectedImage()
        {
            if (Image == null)
                return null;

            Bitmap result;
            if ((Image.PixelFormat & PixelFormat.Indexed) != 0)
                result = Image.Clone() as Bitmap;
            else
            {
                MemoryStream stream = new MemoryStream();
                Image.Save(stream, ImageFormat.Gif);
                result = new Bitmap(stream);
            }

            Color backColor = result.GetPixel(0, 0);
            ColorPalette palette = result.Palette;
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                if (palette.Entries[i] != backColor)
                    palette.Entries[i] = form.ItemTextSelectedColor;
            }
            result.Palette = palette;

            return result;
        }

        TabelaElemenataForm form;

        private bool cutted;
        public bool Cutted
        {
            get { return cutted; }
            set { cutted = value; }
        }

        public ElementTableItem(Sprava sprava, GrupaElementa grupa, int broj,
            Element element, PointF location, SizeF size, TabelaElemenataForm form)
        {
            this.sprava = sprava;
            this.grupa = grupa;
            this.broj = broj;
            this.element = element;
            this.location = location;
            this.size = size;
            selected = false;
            selectedImage = null;
            this.form = form;
            cutted = false;
        }

        public void draw(Graphics g, PointF autoScrollPosition)
        {
            Pen pen;
            Brush brush;
            if (selected)
            {
                pen = form.ItemBorderSelectedPen;
                brush = form.ItemTextSelectedBrush;
            }
            else
            {
                pen = form.ItemBorderPen;
                brush = form.ItemTextBrush;
            }
            Font f = form.ItemFont;
            Font fBold = form.ItemBoldFont;

            RectangleF rect = new RectangleF(location, size);
            rect.Offset(autoScrollPosition.X, autoScrollPosition.Y);

            string number = broj.ToString() + '.';

            if (cutted)
            {
                g.DrawRectangle(form.EraseBorderPen, rect.X, rect.Y, rect.Width, rect.Height);
                DashStyle style = pen.DashStyle;
                pen.DashStyle = DashStyle.Dot;
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                pen.DashStyle = style;
            }
            else
            {
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }
            g.DrawString(number, f, brush, (RectangleF)rect);

            if (element != null)
            {
                string naziv = element.Naziv;
                if (naziv != "")
                    naziv += '\n';
                naziv += element.EngleskiNaziv;
                string nazivGim = element.NazivPoGimnasticaru;

                float nazivHeigth = 0.0f;
                if (naziv != "")
                {
                    float xNaziv = rect.X + g.MeasureString(number + new String(' ', 3), f).Width;
                    nazivHeigth = g.MeasureString(
                        naziv, f, new SizeF(rect.Right - xNaziv, rect.Height)).Height;
                    RectangleF nazivRect = new RectangleF(xNaziv, rect.Y,
                        rect.Right - xNaziv, nazivHeigth);
                    g.DrawString(naziv, f, brush, nazivRect);
                }
                float nazivGimHeigth = 0.0f;
                if (nazivGim != "")
                {
                    nazivGim = "(" + nazivGim + ")";
                    nazivGimHeigth = g.MeasureString(nazivGim, fBold, rect.Size).Height;
                    nazivGimHeigth *= 2;
                    RectangleF nazivGimRect = new RectangleF(rect.X, rect.Y + nazivHeigth,
                        rect.Width, nazivGimHeigth);
                    StringFormat fmt = new StringFormat();
                    fmt.Alignment = StringAlignment.Center;
                    fmt.LineAlignment = StringAlignment.Center;
                    g.DrawString(nazivGim, fBold, brush, nazivGimRect, fmt);
                }

                if (Image != null)
                {
                    float textHeigth = nazivHeigth + nazivGimHeigth;
                    PointF imageTopLeft = new PointF(rect.Left, rect.Top + textHeigth);
                    SizeF imageSize = new SizeF(rect.Width, rect.Height - textHeigth);
                    RectangleF imageRect = new RectangleF(imageTopLeft, imageSize);
                    float hRedukcija =
                        imageRect.Width - imageRect.Width * Slika.ProcenatRedukcije / 100f;
                    float vRedukcija =
                        imageRect.Height - imageRect.Height * Slika.ProcenatRedukcije / 100f;
                    imageRect.Inflate(- hRedukcija / 2, - vRedukcija / 2);
                    imageRect.Inflate(-Math.Max(imageRect.Width / 50, 1), 
                        -Math.Max(imageRect.Height / 50, 1));
                    if (selected)
                        PictureBoxPlus.scaleImageIsotropically(g, SelectedImage, imageRect);
                    else
                        PictureBoxPlus.scaleImageIsotropically(g, Image, imageRect);
                }

                RectangleF vaRect = RectangleF.Empty;
                if (element.Varijante.Count > 0)
                {
                    string va = "VA";
                    SizeF vaSize = g.MeasureString(va, f);
                    PointF vaLoc = new PointF(rect.X, rect.Bottom - vaSize.Height);
                    vaRect = new RectangleF(vaLoc, vaSize);
                    g.DrawRectangle(pen, vaRect.X, vaRect.Y, vaRect.Width, vaRect.Height);
                    g.DrawString(va, f, brush, vaRect);
                }

                if (element.VideoKlipovi.Count > 0)
                {
                    string vi = "VI";
                    SizeF viSize = g.MeasureString(vi, f);
                    PointF viLoc;
                    if (vaRect != RectangleF.Empty)
                        viLoc = new PointF(vaRect.Right, vaRect.Top);
                    else
                        viLoc = new PointF(rect.X, rect.Bottom - viSize.Height);
                    RectangleF viRect = new RectangleF(viLoc, viSize);
                    g.DrawRectangle(pen, viRect.X, viRect.Y, viRect.Width, viRect.Height);
                    g.DrawString(vi, f, brush, viRect);
                }
            }
        }

        public Slika Slika
        {
            get
            {
                if (element == null)
                    return null;
                else
                    return element.getPodrazumevanaSlika();
            }
        }

        private Point pointToClient(Point p, Point autoScrollPosition)
        {
            return new Point(p.X + autoScrollPosition.X, p.Y + autoScrollPosition.Y);
        }

        public bool istiPolozaj(ElementTableItem other)
        {
            if (other == null)
                return false;
            return sprava == other.sprava && grupa == other.grupa && broj == other.broj;
        }
    }
}
