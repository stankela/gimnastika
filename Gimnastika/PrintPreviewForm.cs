using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

using Gimnastika.Exceptions;

namespace Gimnastika
{
    public partial class PrintPreviewForm : Form
    {
        private int totalPages;
        private int page;
        private int lastPageToPrint;
        private bool contentSetupDone;
        private bool preview;
        private bool myPreviewDraw;
        private bool previewA4;
        private int previewPage;
        private Graphics bitmapGraphics;

        private PrintDocument printDocument1;
        private PrintDialog printDialog1;
        private PageSetupDialog pageSetupDialog1;

        private Izvestaj izvestaj;
        
        public PrintPreviewForm()
        {
            InitializeComponent();

            printDocument1 = new PrintDocument();
            printDocument1.BeginPrint += new PrintEventHandler(this.printDocument1_BeginPrint);
            printDocument1.EndPrint += new PrintEventHandler(this.printDocument1_EndPrint);
            printDocument1.QueryPageSettings += new QueryPageSettingsEventHandler(this.printDocument1_QueryPageSettings);
            printDocument1.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);

            printDialog1 = new PrintDialog();
            printDialog1.Document = printDocument1;

            pageSetupDialog1 = new PageSetupDialog();
            pageSetupDialog1.MinMargins = new Margins(50, 50, 50, 50);
            pageSetupDialog1.ShowHelp = true;


            this.Size = new Size(Size.Width, 450);

            contentSetupDone = false;
            myPreviewDraw = true;
            previewPage = 1;
            textBox1.Text = Convert.ToString(previewPage);

            chbPreviewA4.Visible = false;
        }

        public void setIzvestaj(Izvestaj izv)
        {
            izvestaj = izv;
            izvestaj.setPreviewForm(this);
        }

        public int getTotalPages()
        {
            return totalPages;
        }

        public void setTotalPages(int totalPages)
        {
            this.totalPages = totalPages;
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            printDocument1.DocumentName = izvestaj.getDocumentName();
            printDocument1.PrinterSettings.FromPage = 1;
            printDocument1.PrinterSettings.ToPage = totalPages;
            printDocument1.PrinterSettings.MinimumPage = 1;
            printDocument1.PrinterSettings.MaximumPage = totalPages;
            printDialog1.AllowSomePages = true;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                if (printDialog1.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    page = printDocument1.PrinterSettings.FromPage;
                    lastPageToPrint = printDocument1.PrinterSettings.ToPage;
                }
                else
                {
                    page = 1;
                    lastPageToPrint = totalPages;
                }
                contentSetupDone = false;
                preview = false;
                try
                {
                    printDocument1.Print();
                }
                catch
                {
                    MessageBox.Show("Neuspesno stampanje.", "Greska");
                }
                contentSetupDone = false;
                drawPreviewPage();
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            izvestaj.BeginPrint(sender, e);
            //e.Cancel = true;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            if (preview && myPreviewDraw)
                g = bitmapGraphics;
            g.PageUnit = GraphicsUnit.Inch;

            using (Pen thinPen = new Pen(Color.Black, 0))
            {
                RectangleF pageBounds = GetRealPageBounds(e, preview);
                pageBounds = TranslateBounds(g, Rectangle.Truncate(pageBounds));
                RectangleF marginBounds = GetRealMarginBounds(e, preview);
                marginBounds = TranslateBounds(g, Rectangle.Truncate(marginBounds));
                if (preview && myPreviewDraw)
                {
                    pageBounds = g.VisibleClipBounds;

                    float leftMargin;
                    float topMargin;
                    float rightMargin;
                    float bottomMargin;
                    if (previewA4)
                    {
                        leftMargin = 1f;
                        topMargin = 1f;
                        rightMargin = 1f;
                        bottomMargin = 1f;
                    }
                    else
                    {
                        PageSettings pageSet = e.PageSettings;
                        leftMargin = pageSet.Margins.Left / 100f;
                        topMargin = pageSet.Margins.Top / 100f;
                        rightMargin = pageSet.Margins.Right / 100f;
                        bottomMargin = pageSet.Margins.Bottom / 100f;
                    }

                    marginBounds = new RectangleF(leftMargin, topMargin,
                            pageBounds.Width - (leftMargin + rightMargin),
                            pageBounds.Height - (topMargin + bottomMargin));
                }

                //		g.DrawRectangle(thinPen, pageBounds.X, pageBounds.Y, pageBounds.Width, pageBounds.Height);
                //		g.DrawRectangle(thinPen, marginBounds.X, marginBounds.Y, marginBounds.Width, marginBounds.Height);
                try
                {
                    drawPage(g, marginBounds, page);
                }
                catch (PageSizeToSmallException)
                {
                    MessageBox.Show("Strana stampaca je isuvise mala.", this.Text);
                    e.Cancel = true;
                }
            }
            ++page;
            e.HasMorePages = page <= lastPageToPrint;
        }

        // Get real page bounds based on printable area of the page
        static Rectangle GetRealPageBounds(PrintPageEventArgs e, bool preview)
        {
            Graphics g = e.Graphics;
            // Return in units of 1/100th of an inch
            if (preview)
                return e.PageBounds;

            // Translate to units of 1/100th of an inch
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

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, DeviceCapsIndex index);
        enum DeviceCapsIndex
        {
            PhysicalOffsetX = 112,
            PhysicalOffsetY = 113,
        }

        // Adjust MarginBounds rectangle when printing based
        // on the physical characteristics of the printer
        static Rectangle GetRealMarginBounds(PrintPageEventArgs e, bool preview)
        {
            if (preview)
                return e.MarginBounds;

            int cx = 0;
            int cy = 0;
            IntPtr hdc = e.Graphics.GetHdc();

            try
            {
                // Both of these come back as device units and are not
                // scaled to 1/100th of an inch
                cx = GetDeviceCaps(hdc, DeviceCapsIndex.PhysicalOffsetX);
                cy = GetDeviceCaps(hdc, DeviceCapsIndex.PhysicalOffsetY);
            }
            finally
            {
                e.Graphics.ReleaseHdc(hdc);
            }

            // Create the real margin bounds by scaling the offset
            // by the printer resolution and then rescaling it
            // back to 1/100th of an inch
            Rectangle marginBounds = e.MarginBounds;
            int dpiX = (int)e.Graphics.DpiX;
            int dpiY = (int)e.Graphics.DpiY;
            marginBounds.Offset(-cx * 100 / dpiX, -cy * 100 / dpiY);
            return marginBounds;
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            izvestaj.EndPrint(sender, e);
        }

        protected virtual void drawPage(Graphics g, RectangleF marginBounds, int pageNum)
        {
            float headerHeight = izvestaj.getHeaderHeight(g, marginBounds, pageNum);
            RectangleF headerBounds = new RectangleF(marginBounds.Location, new SizeF(marginBounds.Width, headerHeight));
            RectangleF contentBounds = new RectangleF(marginBounds.X,
                marginBounds.Y + headerHeight,
                marginBounds.Width,
                marginBounds.Height - headerHeight);

            if (!contentSetupDone)
            {
                izvestaj.setupContent(g, contentBounds);
                contentSetupDone = true;
                if (previewPage > totalPages)
                {
                    previewPage = totalPages;
                    textBox1.Text = Convert.ToString(previewPage);
                }
                if (pageNum > totalPages)
                    pageNum = totalPages;
            }
            izvestaj.drawHeader(g, headerBounds, pageNum);
            izvestaj.drawContent(g, contentBounds, pageNum);
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            izvestaj.QueryPageSettings(sender, e);
        }

        private void btnPageSetup_Click(object sender, System.EventArgs e)
        {
            pageSetupDialog1.Document = printDocument1;
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
            {
                contentSetupDone = false;
                drawPreviewPage();
            }
        }

        protected void drawPreviewPage()
        {
            PageSettings pageSet = printDocument1.DefaultPageSettings;
            int pageWidth = pageSet.Bounds.Width;	// 0.01 inch
            int pageHeight = pageSet.Bounds.Height;
            if (pageWidth == 0 || pageHeight == 0)
            {
                // greska koja se javlja kada odmah nakon otvaranja PrintPreviewa
                // otvorim PageSetup i podesim stampac na HP a stranu na A5. Tada su
                // i pageWidth i pageHeight nula
                return;
            }
            if (previewA4)
            {
                pageWidth = (int)(210 / 25.4 * 100);
                pageHeight = (int)(297 / 25.4 * 100);
            }
            Graphics screenGraphics = CreateGraphics();
            float screenDpiX = screenGraphics.DpiX;
            float screenDpiY = screenGraphics.DpiY;
            screenGraphics.Dispose();

            float xRes = screenDpiX;
            float yRes = screenDpiY;
            Bitmap bitmap = new Bitmap((int)(pageWidth * xRes / 100),
                (int)(pageHeight * yRes / 100));
            bitmap.SetResolution(xRes, yRes);
            bitmapGraphics = Graphics.FromImage(bitmap);
            bitmapGraphics.Clear(Color.White);

            PrintController oldControler = printDocument1.PrintController;
            PreviewPrintController prevControler = new PreviewPrintController();
            printDocument1.PrintController = prevControler;
            page = previewPage;
            lastPageToPrint = previewPage;
            preview = true;
            printDocument1.Print();
            printDocument1.PrintController = oldControler;

            if (myPreviewDraw)
            {
                pictureBox1.Width = bitmap.Width;
                pictureBox1.Height = bitmap.Height;
                pictureBox1.Image = bitmap;
            }
            else
            {
                PreviewPageInfo pageInfo = prevControler.GetPreviewPageInfo()[0];
                pictureBox1.Width = (int)(pageInfo.PhysicalSize.Width * screenDpiX / 100);
                pictureBox1.Height = (int)(pageInfo.PhysicalSize.Height * screenDpiY / 100);
                pictureBox1.Image = pageInfo.Image; // Metafile
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            bitmapGraphics.Dispose();
        }

        private void btnFirst_Click(object sender, System.EventArgs e)
        {
            if (previewPage > 1)
            {
                previewPage = 1;
                drawPreviewPage();
            }
            textBox1.Text = Convert.ToString(previewPage); // izvukao sam naredbu iz
            // if strukture za slucaj kada je previewPage
            // 1, a u textBoxu se nalazi neki bezveze text
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            if (previewPage > 1)
            {
                previewPage--;
                drawPreviewPage();
            }
            textBox1.Text = Convert.ToString(previewPage);
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (previewPage < totalPages)
            {
                previewPage++;
                drawPreviewPage();
            }
            textBox1.Text = Convert.ToString(previewPage);
        }

        private void btnLast_Click(object sender, System.EventArgs e)
        {
            if (previewPage < totalPages)
            {
                previewPage = totalPages;
                drawPreviewPage();
            }
            textBox1.Text = Convert.ToString(previewPage);
        }

        private void textBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool revertToOrig = false;
                int num = previewPage;
                try
                {
                    num = Convert.ToInt32(textBox1.Text);
                }
                catch (FormatException)
                {
                    revertToOrig = true;
                }

                if (!revertToOrig)
                {
                    if (num != previewPage && num > 1 && num <= totalPages)
                    {
                        previewPage = num;
                        drawPreviewPage();
                    }
                    else
                        revertToOrig = true;
                }
                if (revertToOrig)
                    textBox1.Text = Convert.ToString(previewPage);
            }
        }

        private void textBox1_Leave(object sender, System.EventArgs e)
        {
            textBox1.Text = Convert.ToString(previewPage);
        }

        private void chbPreviewA4_CheckedChanged(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            if (chbPreviewA4.Checked)
            {
                previewA4 = true;
                btnPrint.Enabled = false;
                btnPageSetup.Enabled = false;
                contentSetupDone = false;
                drawPreviewPage();
            }
            else
            {
                previewA4 = false;
                btnPrint.Enabled = true;
                btnPageSetup.Enabled = true;
                contentSetupDone = false;
                drawPreviewPage();
            }
            Cursor.Hide();
            Cursor.Current = Cursors.Arrow;
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        public void printWithoutPreview()
        {
            izvestaj.init();

            printDocument1.DocumentName = izvestaj.getDocumentName();
            printDocument1.PrinterSettings.FromPage = 1;
            printDocument1.PrinterSettings.ToPage = 1;
            page = 1;
            lastPageToPrint = 1;
            contentSetupDone = false;
            preview = false;
            try
            {
                printDocument1.Print();
            }
            catch
            {
                MessageBox.Show("Neuspesno stampanje.", "Greska");
            }
        }

        private void PrintPreviewForm_Load(object sender, EventArgs e)
        {
            izvestaj.init();

            drawPreviewPage();
            this.WindowState = FormWindowState.Maximized;
        }

        private void PrintPreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            izvestaj.clearDataSet();
        }
    }
}