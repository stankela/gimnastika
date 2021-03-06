using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Report;
using System.Drawing.Printing;
using Gimnastika.Exceptions;
using System.Drawing.Drawing2D;
using Gimnastika.Domain;

namespace Gimnastika.UI
{
    public partial class PreviewDialog : Form
    {
        enum RezimRada { Normal, MyPreviewDraw, PageSizeTooSmall };

        private Izvestaj izvestaj;
        private int page;
        private int lastPageToPrint;
        private int previewPage;
        private RezimRada rezimRada;

        private Bitmap bitmap;
        private Graphics bitmapGraphics;
        private Graphics screenGraphics;

        private readonly string SMALL_PAPER_SIZE_MSG =
            "Strana stampaca je mala za izvestaj. Izaberite vecu stranu.";
        private readonly string NO_PRINTERS_MSG = "Ne postoje instalirani stampaci.";
        private readonly string PRINTING_FAILURE_MSG = "Neuspesno stampanje.";

        public PreviewDialog()
        {
            InitializeComponent();
            this.Size = new Size(Size.Width, 450);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            rezimRada = RezimRada.Normal;
            screenGraphics = CreateGraphics();

            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                rezimRada = RezimRada.MyPreviewDraw;

                float pageWidth = 210 / 25.4f;
                float pageHeight = 297 / 25.4f;
                Size s = pageToScreen(new SizeF(pageWidth, pageHeight));
                bitmap = new Bitmap(s.Width, s.Height);
                bitmap.SetResolution(screenGraphics.DpiX, screenGraphics.DpiY);

                bitmapGraphics = Graphics.FromImage(bitmap);
                pictureBox1.ClientSize = bitmap.Size;
            }
        }

        // TODO: Trebao bi neki parametar za page koordinate. Ovako se pretpostavlja
        // da su inchi.
        private Size pageToScreen(SizeF s)
        {
            return Size.Truncate(new SizeF(
                s.Width * screenGraphics.DpiX,
                s.Height * screenGraphics.DpiX));
        }

        private void setPageTextBox(int page)
        {
            textBox1.Text = String.Format("{0} od {1}", page, izvestaj.LastPageNum);
        }

        public void setIzvestaj(Izvestaj izvestaj)
        {
            this.izvestaj = izvestaj;
            setPreferredPrinterAndPaperSize(izvestaj);
            try
            {
                setupContent();
                previewPage = 1;
                setPageTextBox(previewPage);
            }
            catch (SmallPageSizeException)
            {
                rezimRada = RezimRada.PageSizeTooSmall;
            }
        }

        private void setPreferredPrinterAndPaperSize(Izvestaj izvestaj)
        {
            if (!String.IsNullOrEmpty(Opcije.Instance.PrinterName))
            {
                printDocument1.PrinterSettings.PrinterName = Opcije.Instance.PrinterName;
                if (!printDocument1.PrinterSettings.IsValid)
                    printDocument1.PrinterSettings = new PrinterSettings();
            }
            
            if (izvestaj.A4)
            {
                foreach (PaperSize size in printDocument1.PrinterSettings.PaperSizes)
                {
                    if (size.Kind == PaperKind.A4)
                    {
                        printDocument1.DefaultPageSettings.PaperSize = size;
                        break;
                    }
                }
            }
            if (izvestaj.Landscape)
                printDocument1.DefaultPageSettings.Landscape = true;

            if (izvestaj.Margins != null)
                printDocument1.DefaultPageSettings.Margins = izvestaj.Margins;
        }

        private void setupContent()
        {
            Graphics g;
            RectangleF marginBounds;
            if (rezimRada == RezimRada.MyPreviewDraw)
            {
                g = bitmapGraphics;
                g.PageUnit = GraphicsUnit.Inch;
                g.PageScale = 1f;
                RectangleF pageBounds = g.VisibleClipBounds;

                float leftMargin = 1f;
                float topMargin = 1f;
                float rightMargin = 1f;
                float bottomMargin = 1f;

                marginBounds = new RectangleF(leftMargin, topMargin,
                    pageBounds.Width - (leftMargin + rightMargin),
                    pageBounds.Height - (topMargin + bottomMargin));
            }
            else
            {
                g = printDocument1.PrinterSettings.CreateMeasurementGraphics();
                PageSettings pageSettings = printDocument1.DefaultPageSettings;

                Margins margins = pageSettings.Margins;
                Rectangle mBounds = new Rectangle(
                    margins.Left, margins.Top,
                    pageSettings.Bounds.Width - (margins.Left + margins.Right),
                    pageSettings.Bounds.Height - (margins.Top + margins.Bottom));

                Rectangle pBounds = GetRealPageBounds(g);
                marginBounds = GetRealMarginBounds(mBounds, pageSettings);

                g.PageUnit = GraphicsUnit.Inch;
                g.PageScale = 1f;

                RectangleF pageBounds = TranslateBounds(g, pBounds);
                marginBounds = TranslateBounds(g, Rectangle.Truncate(marginBounds));
            }

            izvestaj.setupContent(g, marginBounds);
        }

        private void PreviewDialog_Load(object sender, System.EventArgs e)
        {
            if (rezimRada == RezimRada.PageSizeTooSmall)
            {
                MessageDialogs.showMessage(SMALL_PAPER_SIZE_MSG, this.Text);
                float pageWidth = 210 / 25.4f;
                float pageHeight = 297 / 25.4f;
                pictureBox1.ClientSize = pageToScreen(new SizeF(pageWidth, pageHeight));
            }
            else
                drawPreviewPage();
            this.WindowState = FormWindowState.Maximized;
            btnZatvori.Focus();
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            if (rezimRada == RezimRada.MyPreviewDraw)
            {
                MessageDialogs.showMessage(NO_PRINTERS_MSG, this.Text);
                return;
            }
            else if (rezimRada == RezimRada.PageSizeTooSmall)
            {
                MessageDialogs.showMessage(SMALL_PAPER_SIZE_MSG, this.Text);
                return;
            }

            printDocument1.DocumentName = izvestaj.DocumentName;
            printDocument1.PrinterSettings.FromPage = 1;
            printDocument1.PrinterSettings.ToPage = izvestaj.LastPageNum;
            printDocument1.PrinterSettings.MinimumPage = 1;
            printDocument1.PrinterSettings.MaximumPage = izvestaj.LastPageNum;
            printDialog1.AllowSomePages = true;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    setupContent();
                }
                catch (SmallPageSizeException)
                {
                    rezimRada = RezimRada.PageSizeTooSmall;
                    drawPreviewPage();
                    return;
                }


                if (printDialog1.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    page = printDocument1.PrinterSettings.FromPage;
                    lastPageToPrint = printDocument1.PrinterSettings.ToPage;
                }
                else
                {
                    page = 1;
                    lastPageToPrint = izvestaj.LastPageNum;
                }
                try
                {
                    printDocument1.Print();
                    Opcije.Instance.PrinterName = printDocument1.PrinterSettings.PrinterName;
                }
                catch (InvalidPrinterException)
                {
                    MessageBox.Show(PRINTING_FAILURE_MSG, this.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show(PRINTING_FAILURE_MSG, this.Text);
                }

                // za slucaj da je u PrintDialogu promenjenja velicina strane

                if (previewPage > izvestaj.LastPageNum)
                {
                    previewPage = izvestaj.LastPageNum;
                    setPageTextBox(previewPage);
                }
                drawPreviewPage();
            }
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            izvestaj.TimeOfPrint = DateTime.Now;
            izvestaj.BeginPrint(sender, e);
            //e.Cancel = true;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {

            //	RectangleF pageBounds = GetRealPageBounds(e, preview);
            //	RectangleF marginBounds = GetRealMarginBounds(e, preview);

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Inch;
            g.PageScale = 1f;

            //	pageBounds = TranslateBounds(g, Rectangle.Truncate(pageBounds));
            //	marginBounds = TranslateBounds(g, Rectangle.Truncate(marginBounds));


            // NOTE: Za preview se koristi setup izvestaja izracunat za printer, sto 
            // znaci da ce prikaz biti malo pomeren ulevo

            drawPage(g, page);
            e.HasMorePages = ++page <= lastPageToPrint;
        }

        // If the Graphics object is using a nondefault PageUnit, then 
        // VisibleClipBounds will be in different units than PageBounds (which is 
        // always in units of 100 dpi). To handle these variables, it's useful to 
        // have a helper method to return the "real" page bounds in a consistent unit
        // of measure
        static Rectangle GetRealPageBounds(PrintPageEventArgs e, bool preview)
        {
            if (preview)
            {
                // Return in units of 1/100th of an inch
                return e.PageBounds;
            }
            else
            {
                // Translate to units of 1/100th of an inch
                return GetRealPageBounds(e.Graphics);
            }
        }

        // Get real printer page bounds in units of 1/100th of an inch
        static Rectangle GetRealPageBounds(Graphics g)
        {
            RectangleF vpb = g.VisibleClipBounds;
            PointF[] bottomRight = { new PointF(vpb.Size.Width, vpb.Size.Height) };
            g.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, bottomRight);
            float dpiX = g.DpiX;
            float dpiY = g.DpiY;
            return new Rectangle(0, 0, (int)(bottomRight[0].X * 100 / dpiX), (int)(bottomRight[0].Y * 100 / dpiY));
        }

        // Translate from units of 1/100th of an inch to page units
        static RectangleF TranslateBounds(Graphics g, Rectangle bounds)
        {
            float dpiX = g.DpiX;
            float dpiY = g.DpiY;
            PointF[] pts = new PointF[2];
            // Translate from units of 1/100th of an inch to device units
            pts[0] = new PointF(bounds.X * dpiX / 100, bounds.Y * dpiY / 100);
            pts[1] = new PointF(bounds.Width * dpiX / 100, bounds.Height * dpiX / 100);
            // Translate from device units to page units
            g.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, pts);
            return new RectangleF(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);
        }

        // Adjust MarginBounds rectangle when printing based
        // on the physical characteristics of the printer
        static Rectangle GetRealMarginBounds(PrintPageEventArgs e, bool preview)
        {
            if (preview)
                return e.MarginBounds;
            else
                return GetRealMarginBounds(e.MarginBounds, e.PageSettings);
        }

        // Adjust MarginBounds rectangle when printing based
        // on the physical characteristics of the printer
        static Rectangle GetRealMarginBounds(Rectangle marginBounds,
            PageSettings pageset)
        {
            // Get printer's offsets
            float cx = pageset.HardMarginX; // in 0.01 inch
            float cy = pageset.HardMarginY;

            // Create the real margin bounds
            marginBounds.Offset(-(int)cx, -(int)cy);
            return marginBounds;
        }
        
        private void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            izvestaj.EndPrint(sender, e);
        }

        private void drawPage(Graphics g, int pageNum)
        {
            izvestaj.drawPage(g, pageNum);
        }

        private void printDocument1_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            izvestaj.QueryPageSettings(sender, e);
        }

        private void btnPageSetup_Click(object sender, System.EventArgs e)
        {
            if (rezimRada == RezimRada.MyPreviewDraw)
            {
                MessageDialogs.showMessage(NO_PRINTERS_MSG, this.Text);
                return;
            }

            pageSetupDialog1.Document = printDocument1;
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    setupContent();
                }
                catch (SmallPageSizeException)
                {
                    rezimRada = RezimRada.PageSizeTooSmall;
                    drawPreviewPage();
                    return;
                }

                if (previewPage > izvestaj.LastPageNum)
                {
                    previewPage = izvestaj.LastPageNum;
                    setPageTextBox(previewPage);
                }
                drawPreviewPage();
            }
        }

        private void drawPreviewPage()
        {
            if (rezimRada == RezimRada.MyPreviewDraw)
            {
                izvestaj.TimeOfPrint = DateTime.Now;
                bitmapGraphics.Clear(Color.White);
                drawPage(bitmapGraphics, previewPage);
                pictureBox1.Image = bitmap;
                return;
            }
            else if (rezimRada == RezimRada.PageSizeTooSmall)
            {
                bitmapGraphics.Clear(Color.White);
                pictureBox1.Image = bitmap;
                return;
            }

            PrintController oldControler = printDocument1.PrintController;
            PreviewPrintController prevControler = new PreviewPrintController();
            printDocument1.PrintController = prevControler;
            page = previewPage;
            lastPageToPrint = previewPage;
            printDocument1.Print();
            printDocument1.PrintController = oldControler;

            PreviewPageInfo pageInfo = prevControler.GetPreviewPageInfo()[0];
            // PhysicalSize is in 0.01 inch
            pictureBox1.ClientSize = pageToScreen(new SizeF(
                pageInfo.PhysicalSize.Width / 100f,
                pageInfo.PhysicalSize.Height / 100f));
            pictureBox1.Image = pageInfo.Image; // Metafile
        }

        private void btnFirst_Click(object sender, System.EventArgs e)
        {
            if (rezimRada == RezimRada.PageSizeTooSmall)
                return;

            if (previewPage > 1)
            {
                previewPage = 1;
                drawPreviewPage();
                setPageTextBox(previewPage);
            }
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            if (rezimRada == RezimRada.PageSizeTooSmall)
                return;

            if (previewPage > 1)
            {
                previewPage--;
                drawPreviewPage();
                setPageTextBox(previewPage);
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            // TODO: Kada se predje na novu stranicu, prikazi vrh strane. Takodje
            // implementiraj skrolovanje tockicem i lepsi prikaz strane (sa senkama)
            if (rezimRada == RezimRada.PageSizeTooSmall)
                return;

            if (previewPage < izvestaj.LastPageNum)
            {
                previewPage++;
                drawPreviewPage();
                setPageTextBox(previewPage);
            }
        }

        private void btnLast_Click(object sender, System.EventArgs e)
        {
            if (rezimRada == RezimRada.PageSizeTooSmall)
                return;

            if (previewPage < izvestaj.LastPageNum)
            {
                previewPage = izvestaj.LastPageNum;
                drawPreviewPage();
                setPageTextBox(previewPage);
            }
        }

        private void textBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (rezimRada == RezimRada.PageSizeTooSmall)
                return;

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

                if (num >= 1 && num <= izvestaj.LastPageNum)
                {
                    previewPage = num;
                    drawPreviewPage();
                }
                else
                {
                    setPageTextBox(previewPage);
                }
            }
        }

        private void textBox1_Leave(object sender, System.EventArgs e)
        {
            setPageTextBox(previewPage);
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        public void printWithoutPreview(Izvestaj izvestaj)
        {
            if (rezimRada == RezimRada.MyPreviewDraw)
            {
                MessageDialogs.showMessage(NO_PRINTERS_MSG, this.Text);
                return;
            }

            setIzvestaj(izvestaj);
            if (rezimRada == RezimRada.PageSizeTooSmall)
            {
                MessageDialogs.showMessage(SMALL_PAPER_SIZE_MSG, this.Text);
                return;
            }

            printDocument1.DocumentName = izvestaj.DocumentName;
            page = 1;
            lastPageToPrint = izvestaj.LastPageNum;
            try
            {
                printDocument1.Print();
            }
            catch (InvalidPrinterException)
            {
                MessageBox.Show(PRINTING_FAILURE_MSG, this.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(PRINTING_FAILURE_MSG, this.Text);
            }
        }

        private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            removeFocusFromTextBox();
        }

        private void removeFocusFromTextBox()
        {
            panel1.Focus();
        }

        private void panel2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            removeFocusFromTextBox();
        }

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            removeFocusFromTextBox();
        }

        private void PreviewDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            screenGraphics.Dispose();
            if (bitmap != null)
                bitmap.Dispose();
            // TODO: Ovde bi mogao da ubacis i Dispose fontova (ako se odlucis za to)
        }
    }
}