using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Gimnastika.Domain;
using System.Collections.Generic;
using Gimnastika.UI;

namespace Gimnastika.Report
{
	/// <summary>
	/// Summary description for PotvrdaIzvestaj.
	/// </summary>
	public class VezbaIzvestaj : Izvestaj
	{
        private VezbaEditorBaseForm vezbaEditor;

        RectangleF headerBounds;

        Font gimnasticarFont;
        Font nazivVezbeFont;
        Font spravaFont;
        Font datumFont;

        VezbaTabela tabela;

        public VezbaIzvestaj(VezbaEditorBaseForm vezbaEditor)
		{
            this.vezbaEditor = vezbaEditor;
			setDocumentName(vezbaEditor.Vezba.Naziv);
		}

		protected override void createFonts()
		{
			base.createFonts();
            gimnasticarFont = new Font("Arial", 10, FontStyle.Bold);
            nazivVezbeFont = new Font("Arial", 12);
            spravaFont = new Font("Arial", 10, FontStyle.Bold);
            datumFont = new Font("Arial", 8);
        }

		protected override void releaseFonts()
		{
			base.releaseFonts();
            gimnasticarFont.Dispose();
            nazivVezbeFont.Dispose();
            spravaFont.Dispose();
            datumFont.Dispose();
            // blackBrush.Dispose();  // daje gresku
		}
		
		public override void setupContent(Graphics g, RectangleF contentBounds)
		{
			getData();
            getPreviewForm().setTotalPages(1);
		}

        private void getData()
        {

        }

        public override void drawContent(Graphics g, RectangleF contentBounds, int pageNum)
        {
            DataGridView gridElementi = (vezbaEditor as VezbaEditorForm).getGridElementi();
            int redBrojWidth = gridElementi.Columns["RedBroj"].Width;
            int nazivElementaWidth = gridElementi.Columns["NazivElementa"].Width;
            int tezinaWidth = gridElementi.Columns["Tezina"].Width;
            int grupaBrojWidth = gridElementi.Columns["GrupaBroj"].Width;
            int vrednostWidth = gridElementi.Columns["Vrednost"].Width;
            int vezaSaPrethodnimWidth = gridElementi.Columns["VezaSaPrethodnim"].Width;
            int zahtevWidth = gridElementi.Columns["Zahtev"].Width;
            int odbitakWidth = gridElementi.Columns["Odbitak"].Width;
            int penalizacijaWidth = gridElementi.Columns["Penalizacija"].Width;

            tabela = new VezbaTabela(contentBounds.Location, contentBounds.Width,                 
                    redBrojWidth, nazivElementaWidth, tezinaWidth, grupaBrojWidth,
                    vrednostWidth, vezaSaPrethodnimWidth, zahtevWidth, odbitakWidth,
                    penalizacijaWidth, g, vezbaEditor.NumEmptyRows, vezbaEditor.Vezba);

            tabela.draw(g);
        }

        public override void drawHeader(Graphics g, RectangleF headerBounds, int pageNum)
		{
            this.headerBounds = headerBounds;
            PointF gimnasticarTopLeft = headerBounds.Location;

            float gimnasticarHeight = gimnasticarFont.GetHeight(g) * 1.5f;
            PointF nazivVezbeTopLeft = headerBounds.Location + new SizeF(0, gimnasticarHeight);

            drawGimnasticar(g);
            drawNazivVezbe(g, nazivVezbeTopLeft);
            drawSprava(g);
            drawDatum(g);
        }

        private void drawGimnasticar(Graphics g)
        {
            if (vezbaEditor.Vezba.Gimnasticar != null)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                g.DrawString(vezbaEditor.Vezba.Gimnasticar.ToString(),
                    gimnasticarFont, blackBrush,
                    headerBounds, sf);
            }
        }

        private void drawNazivVezbe(Graphics g, PointF topLeft)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            g.DrawString(vezbaEditor.Vezba.Naziv, nazivVezbeFont, blackBrush,
                topLeft, sf);
        }

        private void drawSprava(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Far;
            g.DrawString(Resursi.getImeSprave(vezbaEditor.Vezba.Sprava).ToUpper(),
                spravaFont, blackBrush,
                headerBounds, sf);
        }

        private void drawDatum(Graphics g)
        {
            DateTime now = DateTime.Now;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Near;
            g.DrawString(DateUtilities.serbianDateStr(now, '.'), datumFont,
                blackBrush, headerBounds, sf);
        }
	}
}
