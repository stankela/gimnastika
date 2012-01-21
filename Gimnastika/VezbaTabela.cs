using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Gimnastika.Entities;

namespace Gimnastika
{
    public class VezbaTabela
    {
        float tableWidth, xRedBroj, xOpis, xTezina, xBroj, xVrednost, xVeza, xZahtev, xOdbitak,
            xPenal;
        PointF tableHeaderTopLeft;
        float tableHeaderHeight;
        float tableItemHeight;
        List<float> elementRowHeights = new List<float>();
        float footerTop;
        float ukupnoRowHeight;
        private float pocetnaOcenaRowHeight;
        private float izvedbaRowHeight;

        private int numEmptyRows;
        private Vezba vezba;

        Font gimnasticarFont;
        Font nazivVezbeFont;
        Font spravaFont;
        Font tableHeaderFont;
        Font tableItemFont;
        Pen headerPen;
        Pen itemPen;
        Pen footerPen;
        Font ukupnoCaptionFont;
        Font footerFont;
        Font pocetnaOcenaCaptionFont;
        Font footerFontBold;
        Brush obeleziBrush;
        Brush blackBrush;
        Brush selectBrush;

        private string[] colNames = { "RedBroj", "NazivElementa", "Tezina", "GrupaBroj",
                "Vrednost", "Zahtev", "VezaSaPrethodnim", "Odbitak", "Penalizacija"};

        public static readonly int RED_BROJ_WIDTH = 40;
        public static readonly int NAZIV_ELEMENATA_WIDTH = 310;
        public static readonly int TEZINA_WIDTH = 50;
        public static readonly int GRUPA_BROJ_WIDTH = 60;
        public static readonly int VREDNOST_WIDTH = 60;
        public static readonly int VEZA_SA_PRETHODNIM_WIDTH = 60;
        public static readonly int ZAHTEV_WIDTH = 60;
        public static readonly int ODBITAK_WIDTH = 60;
        public static readonly int PENALIZACIJA_WIDTH = 70;

        public static readonly int NUM_EMPTY_ROWS = 2;

        private int selectedElement = -1;
        private int selectedColumn = -1;

        public void selectElementCell(int redBroj, int col)
        {
            if (redBroj > 0 && redBroj <= vezba.Elementi.Count
            && col >= 0 && col < colNames.Length)
            {
                selectedElement = redBroj;
                selectedColumn = col;
            }
        }

        public VezbaTabela(PointF tableHeaderTopLeft, float tableWidth,
            Graphics g, int numEmptyRows, Vezba vezba)
            : this(tableHeaderTopLeft, tableWidth, RED_BROJ_WIDTH,
                NAZIV_ELEMENATA_WIDTH, TEZINA_WIDTH, GRUPA_BROJ_WIDTH, VREDNOST_WIDTH,
                VEZA_SA_PRETHODNIM_WIDTH, ZAHTEV_WIDTH, ODBITAK_WIDTH,
                PENALIZACIJA_WIDTH, g, numEmptyRows, vezba)
        {

        }

        public VezbaTabela(PointF tableHeaderTopLeft, float tableWidth,
            int redBrojWidth, int nazivElementaWidth, int tezinaWidth,
            int grupaBrojWidth, int vrednostWidth, int vezaSaPrethodnimWidth,
            int zahtevWidth, int odbitakWidth, int penalizacijaWidth,
            Graphics g, int numEmptyRows, Vezba vezba)
        {
            this.tableHeaderTopLeft = tableHeaderTopLeft;
            this.tableWidth = tableWidth;
            initXOffsets(tableWidth, redBrojWidth, nazivElementaWidth, tezinaWidth,
                grupaBrojWidth, vrednostWidth, vezaSaPrethodnimWidth, zahtevWidth,
                odbitakWidth, penalizacijaWidth);

            this.vezba = vezba;
            this.numEmptyRows = numEmptyRows;

            createFonts();

            tableHeaderHeight = 2 * tableHeaderFont.GetHeight(g) * 1.2f;
            tableItemHeight = tableHeaderFont.GetHeight(g) * 1.4f;
            calculateElementRowHeights(g);

            if (vezba.Elementi.Count > 0)
            {
                int lastElementRow = vezba.Elementi.Count - 1;
                footerTop = getElementRowTop(lastElementRow) +
                    getElementRowHeight(lastElementRow) + tableItemHeight * numEmptyRows;
            }
            else
            {
                footerTop = tableHeaderTopLeft.Y + tableHeaderHeight +
                    tableItemHeight * numEmptyRows;
            }
            ukupnoRowHeight = ukupnoCaptionFont.GetHeight(g) * 1.2f;
            pocetnaOcenaRowHeight = pocetnaOcenaCaptionFont.GetHeight(g) * 1.2f;
            izvedbaRowHeight = footerFontBold.GetHeight(g) * 1.2f;
        }

        private void initXOffsets(float tableWidth, int redBrojWidth,
            int nazivElementaWidth, int tezinaWidth, int grupaBrojWidth,
            int vrednostWidth, int vezaSaPrethodnimWidth, int zahtevWidth, 
            int odbitakWidth, int penalizacijaWidth)
        {
            int gridWidth = redBrojWidth + nazivElementaWidth + tezinaWidth + grupaBrojWidth +
                vrednostWidth + vezaSaPrethodnimWidth + zahtevWidth + odbitakWidth +
                penalizacijaWidth;

            xRedBroj = tableHeaderTopLeft.X;
            xOpis = xRedBroj + tableWidth * redBrojWidth / gridWidth;
            xTezina = xOpis + tableWidth * nazivElementaWidth / gridWidth;
            xBroj = xTezina + tableWidth * tezinaWidth / gridWidth;
            xVrednost = xBroj + tableWidth * vrednostWidth / gridWidth;
            xVeza = xVrednost + tableWidth * vezaSaPrethodnimWidth / gridWidth;
            xZahtev = xVeza + tableWidth * zahtevWidth / gridWidth;
            xOdbitak = xZahtev + tableWidth * odbitakWidth / gridWidth;
            xPenal = xOdbitak + tableWidth * penalizacijaWidth / gridWidth;
        }

        private void createFonts()
        {
            gimnasticarFont = new Font("Arial", 10, FontStyle.Bold);
            nazivVezbeFont = new Font("Arial", 12);
            spravaFont = new Font("Arial", 10, FontStyle.Bold);
            tableHeaderFont = new Font("Arial", 8);
            tableItemFont = new Font("Arial", 8);
            ukupnoCaptionFont = new Font("Arial", 9, FontStyle.Bold);
            footerFont = new Font("Arial", 8);
            pocetnaOcenaCaptionFont = new Font("Arial", 11, FontStyle.Bold);
            footerFontBold = new Font("Arial", 8, FontStyle.Bold);

            headerPen = new Pen(Color.Black, 1 / 72f * 1f);
            itemPen = new Pen(Color.Black, 1 / 72f * 0.5f);
            footerPen = new Pen(Color.Black, 1 / 72f * 1f);
            obeleziBrush = new SolidBrush(Color.LightGray);
            selectBrush = new SolidBrush(Color.Blue);
            blackBrush = new SolidBrush(Color.Black);
        }

        public void draw(Graphics g)
        {
            drawTableHeader(g);
            drawTableContent(g);
            drawTableFooter(g);
        }

        private void calculateElementRowHeights(Graphics g)
        {
            elementRowHeights.Clear();
            for (int i = 0; i < vezba.Elementi.Count; i++)
            {
                string nazivElementa = vezba.Elementi[i].NazivElementa;
                float height = g.MeasureString(nazivElementa, tableItemFont,
                    new SizeF(xTezina - xOpis, xTezina - xOpis)).Height; //y koordinata 
                // je izabrana da bude dovoljno velika
                float unitHeight = g.MeasureString("A", tableItemFont).Height;
                elementRowHeights.Add(
                    (float)Math.Round(height / unitHeight) * tableItemHeight);
            }
        }

        private float getElementRowTop(int i)
        {
            float result = getTableItemsTopLeft().Y;
            for (int j = 0; j < i; j++)
                result += elementRowHeights[j];
            return result;
        }

        private float getElementRowHeight(int i)
        {
            return elementRowHeights[i];
        }

        private PointF getTableItemsTopLeft()
        {
            return tableHeaderTopLeft + new SizeF(0, tableHeaderHeight);
        }

        private void drawTableHeader(Graphics g)
        {
            foreach (string column in colNames)
                drawHeaderCell(g, column);
        }

        private void drawHeaderCell(Graphics g, string column)
        {
            RectangleF rect = getHeaderCellRect(g, column);
            g.DrawRectangle(headerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = getHeaderCellText(column);
            StringFormat sf = getHeaderStringFormat(column);
            g.DrawString(text, tableHeaderFont, blackBrush, rect, sf);
        }

        private RectangleF getHeaderCellRect(Graphics g, string column)
        {
            float y = tableHeaderTopLeft.Y;
            switch (column)
            {
                case "RedBroj":
                    return new RectangleF(xRedBroj, y, xOpis - xRedBroj, tableHeaderHeight);

                case "NazivElementa":
                    return new RectangleF(xOpis, y, xTezina - xOpis, tableHeaderHeight);

                case "Tezina":
                    return new RectangleF(xTezina, y, xBroj - xTezina, tableHeaderHeight);

                case "GrupaBroj":
                    return new RectangleF(xBroj, y, xVrednost - xBroj, tableHeaderHeight);

                case "Vrednost":
                    return new RectangleF(xVrednost, y, xVeza - xVrednost, tableHeaderHeight);

                case "VezaSaPrethodnim":
                    return new RectangleF(xVeza, y, xZahtev - xVeza, tableHeaderHeight);

                case "Zahtev":
                    return new RectangleF(xZahtev, y, xOdbitak - xZahtev, tableHeaderHeight);

                case "Odbitak":
                    return new RectangleF(xOdbitak, y, xPenal - xOdbitak, tableHeaderHeight);

                case "Penalizacija":
                    return new RectangleF(xPenal, y, xRedBroj + tableWidth - xPenal, tableHeaderHeight);

                default:
                    return new RectangleF();

            }
        }

        private string getHeaderCellText(string column)
        {
            switch (column)
            {
                case "RedBroj":
                    return "Red.\nbroj";

                case "NazivElementa":
                    return "Opis";

                case "GrupaBroj":
                    return "Broj u\ntabl.";

                case "VezaSaPrethodnim":
                    return "Veza";

                case "Penalizacija":
                    return "Penal.";

                case "Tezina":
                case "Vrednost":
                case "Zahtev":
                case "Odbitak":
                    return column;

                default:
                    return "";

            }
        }

        private StringFormat getHeaderStringFormat(string column)
        {
            StringFormat result = new StringFormat();
            result.Alignment = StringAlignment.Center;
            result.LineAlignment = StringAlignment.Center;
            return result;
        }

        private void drawTableContent(Graphics g)
        {
            for (int i = 0; i < vezba.Elementi.Count; i++)
                drawItemRow(g, i);

            drawVezaItemColumn(g);

            for (int i = 0; i < numEmptyRows; i++)
                drawEmptyRow(vezba.Elementi.Count + i, g);
        }

        private void drawItemRow(Graphics g, int row)
        {
            if (vezba.Elementi[row].BodujeSe)
                obeleziVrstu(g, row);
            if (selectedElement == row + 1)
                drawSelectedCell(g);

            foreach (string column in colNames)
            {
                if (column != "VezaSaPrethodnim")
                    drawItemCell(g, row, column);
            }
        }

        private void drawSelectedCell(Graphics g)
        {
            if (selectedElement != -1 && selectedColumn != -1)
            {
                int row = selectedElement - 1;
                float rowHeight = getElementRowHeight(row);
                float y = getElementRowTop(row);
                RectangleF rect = new RectangleF(xRedBroj, y, xOpis - xRedBroj,
                    rowHeight);
                g.FillRectangle(selectBrush, rect);
            }
        }

        private void obeleziVrstu(Graphics g, int row)
        {
            float rowHeight = getElementRowHeight(row);
            float y = getElementRowTop(row);
            RectangleF rect = new RectangleF(xTezina, y, xRedBroj + tableWidth - xTezina,
                rowHeight);
            g.FillRectangle(obeleziBrush, rect);
        }

        private void drawItemCell(Graphics g, int row, string column)
        {
            RectangleF rect = getItemCellRect(g, row, column);
            g.DrawRectangle(itemPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = getItemCellText(row, column);
            StringFormat sf = getItemStringFormat(column);
            if (column == "NazivElementa")
                rect.X += rect.Width / 50;  // // odmakni tekst od ivice
            else if (column == "GrupaBroj")
                rect.X += rect.Width / 20;
            g.DrawString(text, tableItemFont, blackBrush, rect, sf);
        }

        private RectangleF getItemCellRect(Graphics g, int row, string column)
        {
            float rowHeight;
            float y;
            if (row < vezba.Elementi.Count)
            {
                rowHeight = getElementRowHeight(row);
                y = getElementRowTop(row);
            }
            else
            {
                rowHeight = tableItemHeight;
                int lastElementRow = vezba.Elementi.Count - 1;
                if (vezba.Elementi.Count > 0)
                {
                    y = getElementRowTop(lastElementRow)
                        + getElementRowHeight(lastElementRow) +
                        (row - vezba.Elementi.Count) * tableItemHeight;
                }
                else
                {
                    y = tableHeaderTopLeft.Y + tableHeaderHeight +
                        row * tableItemHeight;
                }

            }
            switch (column)
            {
                case "RedBroj":
                    return new RectangleF(xRedBroj, y, xOpis - xRedBroj, rowHeight);

                case "NazivElementa":
                    return new RectangleF(xOpis, y, xTezina - xOpis, rowHeight);

                case "Tezina":
                    return new RectangleF(xTezina, y, xBroj - xTezina, rowHeight);

                case "GrupaBroj":
                    return new RectangleF(xBroj, y, xVrednost - xBroj, rowHeight);

                case "Vrednost":
                    return new RectangleF(xVrednost, y, xVeza - xVrednost, rowHeight);

                case "VezaSaPrethodnim":
                    return new RectangleF(xVeza, y, xZahtev - xVeza, rowHeight);

                case "Zahtev":
                    return new RectangleF(xZahtev, y, xOdbitak - xZahtev, rowHeight);

                case "Odbitak":
                    return new RectangleF(xOdbitak, y, xPenal - xOdbitak, rowHeight);

                case "Penalizacija":
                    return new RectangleF(xPenal, y, xRedBroj + tableWidth - xPenal, rowHeight);

                default:
                    return new RectangleF();

            }
        }

        private string getItemCellText(int rowIndex, string column)
        {
            ElementVezbe element = vezba.Elementi[rowIndex];

            switch (column)
            {
                case "RedBroj":
                    return element.RedBroj.ToString();

                case "NazivElementa":
                    return element.NazivElementa;

                case "Tezina":
                    if (element.Tezina != TezinaElementa.Undefined)
                        return element.Tezina.ToString();
                    else
                        return "";

                case "GrupaBroj":
                    return element.GrupaBroj;

                case "Vrednost":
                    if (element.Vrednost != null)
                        return element.Vrednost.Value.ToString(getColumnFormatString("Vrednost"));
                    else
                        return "";

                case "VezaSaPrethodnim":
                    // poseban se obradjuje
                    return "";

                case "Zahtev":
                    if (element.Zahtev != null)
                        return element.Zahtev.Value.ToString(getColumnFormatString("Zahtev"));
                    else
                        return "";

                case "Odbitak":
                    if (element.Odbitak != null)
                        return element.Odbitak.Value.ToString(getColumnFormatString("Odbitak"));
                    else
                        return "";

                case "Penalizacija":
                    if (element.Penalizacija != null)
                        return element.Penalizacija.Value.ToString(getColumnFormatString("Penalizacija"));
                    else
                        return "";

                default:
                    return "";
            }
        }

        private string getColumnFormatString(string column)
        {
            switch (column)
            {
                case "Vrednost":
                case "VezaSaPrethodnim":
                case "Zahtev":
                case "Odbitak":
                case "Penalizacija":
                    return "F2";

                case "RedBroj":
                case "NazivElementa":
                case "Tezina":
                case "GrupaBroj":
                    return "";

                default:
                    return "";
            }
        }

        private StringFormat getItemStringFormat(string column)
        {
            StringFormat result = new StringFormat();
            switch (column)
            {
                case "RedBroj":
                case "Tezina":
                case "Vrednost":
                case "VezaSaPrethodnim":
                case "Zahtev":
                case "Odbitak":
                case "Penalizacija":
                    result.Alignment = StringAlignment.Center;
                    result.LineAlignment = StringAlignment.Center;
                    break;

                case "NazivElementa":
                case "GrupaBroj":
                    result.Alignment = StringAlignment.Near;
                    result.LineAlignment = StringAlignment.Center;
                    break;

                default:
                    break;
            }
            return result;
        }

        private void drawVezaItemColumn(Graphics g)
        {
            for (int row = 0; row < vezba.Elementi.Count; row++)
            {
                if (!vezba.isDeoVeze(row + 1))
                {
                    RectangleF rect = getItemCellRect(g, row, "VezaSaPrethodnim");
                    g.DrawRectangle(itemPen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
            List<int> veze = vezba.getVeze();
            for (int i = 0; i < veze.Count / 2; i++)
            {
                int firstElement = veze[2 * i];
                int lastElement = veze[2 * i + 1];
                float veza = vezba.Elementi[lastElement].VezaSaPrethodnim.Value;

                RectangleF firstRect = getItemCellRect(g, firstElement, "VezaSaPrethodnim");
                RectangleF lastRect = getItemCellRect(g, lastElement, "VezaSaPrethodnim");
                RectangleF rect = new RectangleF(firstRect.Location,
                    new SizeF(firstRect.Width, lastRect.Bottom - firstRect.Top));
                g.DrawRectangle(itemPen, rect.X, rect.Y, rect.Width, rect.Height);

                string text = veza.ToString(getColumnFormatString("VezaSaPrethodnim"));
                StringFormat sf = getItemStringFormat("VezaSaPrethodnim");
                g.DrawString(text, tableItemFont, blackBrush, rect, sf);
            }

        }

        private void drawEmptyRow(int row, Graphics g)
        {
            foreach (string column in colNames)
            {
                RectangleF rect = getItemCellRect(g, row, column);
                g.DrawRectangle(itemPen, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        private void drawTableFooter(Graphics g)
        {
            drawUkupnoRow(g);
            drawPocetnaOcenaRow(g);
            drawIzvedbaRow(g);
        }

        private void drawUkupnoRow(Graphics g)
        {
            drawUkupnoCaptionCell(g);
            drawUkupnoVrednostCell(g);
            drawUkupnoVezaCell(g);
            drawUkupnoZahtevCell(g);
            drawUkupnoOdbitakCell(g);
            drawUkupnoPenalCell(g);
        }

        private void drawUkupnoCaptionCell(Graphics g)
        {
            float y = footerTop;
            RectangleF rect = new RectangleF(xRedBroj, y, xVrednost - xRedBroj,
                ukupnoRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "UKUPNO";
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(text, ukupnoCaptionFont, blackBrush, rect, sf);
        }

        private void drawUkupnoVrednostCell(Graphics g)
        {
            float y = footerTop;
            RectangleF rect = new RectangleF(xVrednost, y, xVeza - xVrednost,
                ukupnoRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getVrednostUkupno().ToString(getColumnFormatString("Vrednost"));
            StringFormat sf = getItemStringFormat("Vrednost");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawUkupnoVezaCell(Graphics g)
        {
            float y = footerTop;
            RectangleF rect = new RectangleF(xVeza, y, xZahtev - xVeza,
                ukupnoRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getVezaUkupno().ToString(getColumnFormatString("VezaSaPrethodnim"));
            StringFormat sf = getItemStringFormat("VezaSaPrethodnim");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawUkupnoZahtevCell(Graphics g)
        {
            float y = footerTop;
            RectangleF rect = new RectangleF(xZahtev, y, xOdbitak - xZahtev,
                ukupnoRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getZahtevUkupno().ToString(getColumnFormatString("Zahtev"));
            StringFormat sf = getItemStringFormat("Zahtev");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawUkupnoOdbitakCell(Graphics g)
        {
            float y = footerTop;
            RectangleF rect = new RectangleF(xOdbitak, y, xPenal - xOdbitak,
                ukupnoRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getOdbitakUkupno().ToString(getColumnFormatString("Odbitak"));
            StringFormat sf = getItemStringFormat("Odbitak");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawUkupnoPenalCell(Graphics g)
        {
            float y = footerTop;
            RectangleF rect = new RectangleF(xPenal, y, xRedBroj + tableWidth - xPenal,
                ukupnoRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getPenalizacijaUkupno().ToString(getColumnFormatString("Penalizacija"));
            StringFormat sf = getItemStringFormat("Penalizacija");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawPocetnaOcenaRow(Graphics g)
        {
            drawPocetnaOcenaCaptionCell(g);
            drawPocetnaOcenaCell(g);
            drawOdbitakPenalizacijaUkupnoCell(g);
        }

        private void drawPocetnaOcenaCaptionCell(Graphics g)
        {
            float y = footerTop + ukupnoRowHeight;
            RectangleF rect = new RectangleF(xRedBroj, y, xVrednost - xRedBroj,
                pocetnaOcenaRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "POCETNA OCENA";
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(text, pocetnaOcenaCaptionFont, blackBrush, rect, sf);
        }

        private void drawPocetnaOcenaCell(Graphics g)
        {
            float y = footerTop + ukupnoRowHeight;
            RectangleF rect = new RectangleF(xVrednost, y, xOdbitak - xVrednost,
                pocetnaOcenaRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getPocetnaOcena().ToString(getColumnFormatString("Vrednost"));
            StringFormat sf = getItemStringFormat("Vrednost");
            g.DrawString(text, footerFontBold, blackBrush, rect, sf);
        }

        private void drawOdbitakPenalizacijaUkupnoCell(Graphics g)
        {
            float y = footerTop + ukupnoRowHeight;
            RectangleF rect = new RectangleF(xOdbitak, y, xRedBroj + tableWidth - xOdbitak,
                pocetnaOcenaRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getOdbitakPenalizacija().ToString(getColumnFormatString("Odbitak"));
            StringFormat sf = getItemStringFormat("Odbitak");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawIzvedbaRow(Graphics g)
        {
            drawIzvedbaCaptionCell(g);
            drawIzvedbaCell(g);
            drawOcenaCell(g);
        }

        private void drawIzvedbaCaptionCell(Graphics g)
        {
            float y = footerTop + ukupnoRowHeight + pocetnaOcenaRowHeight;
            RectangleF rect = new RectangleF(xRedBroj, y, xVrednost - xRedBroj,
                izvedbaRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            drawIzvedbaCaptionCellContent(g, footerFontBold, footerFont,
                vezba.Pravilo, rect, blackBrush);
        }

        public static void drawIzvedbaCaptionCellContent(Graphics g, Font fontIzvedba,
            Font fontOpseg, PraviloOceneVezbe pravilo, RectangleF rect, Brush brush)
        {
            string izvedba = "IZVEDBA";
            SizeF izvedbaSize = g.MeasureString(izvedba, fontIzvedba);
            List<string> opsezi = new List<string>();
            List<SizeF> sizes = new List<SizeF>();
            pravilo.sortirajPocetneOceneIzvedbe();
            float opseziWidth = 0;
            foreach (PocetnaOcenaIzvedbe po in pravilo.PocetneOceneIzvedbe)
            {
                if (po.MinBrojElemenata > 0)
                {
                    string opsegString = po.OpsegOcenaString;
                    opsezi.Add(opsegString);
                    SizeF size = g.MeasureString(opsegString, fontOpseg);
                    sizes.Add(size);
                    opseziWidth += size.Width;
                }
            }
            float opseziMaxWidth = rect.Width - izvedbaSize.Width;
            float opseziMargin = (opseziMaxWidth - opseziWidth) / (opsezi.Count + 1);
            SizeF marginSize = new SizeF(opseziMargin, 0);
            PointF topLeft = rect.Location + new SizeF(izvedbaSize.Width, 0) + marginSize;
            List<PointF> opseziTopLeft = new List<PointF>();
            for (int i = 0; i < sizes.Count; i++)
            {
                opseziTopLeft.Add(topLeft);
                topLeft += new SizeF(sizes[i].Width, 0) + marginSize;
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(izvedba, fontIzvedba, brush, rect, sf);

            for (int i = 0; i < opsezi.Count; i++)
            {
                string opseg = opsezi[i];
                RectangleF r = new RectangleF(opseziTopLeft[i], new SizeF(sizes[i].Width, rect.Height));
                g.DrawString(opseg, fontOpseg, brush, r, sf);
            }
        }

        private void drawIzvedbaCell(Graphics g)
        {
            float y = footerTop + ukupnoRowHeight + pocetnaOcenaRowHeight;
            RectangleF rect = new RectangleF(xVrednost, y, xOdbitak - xVrednost,
                izvedbaRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getIzvedba().ToString(getColumnFormatString("Vrednost"));
            StringFormat sf = getItemStringFormat("Vrednost");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

        private void drawOcenaCell(Graphics g)
        {
            float y = footerTop + ukupnoRowHeight + pocetnaOcenaRowHeight;
            RectangleF rect = new RectangleF(xOdbitak, y, xRedBroj + tableWidth - xOdbitak,
                izvedbaRowHeight);
            g.DrawRectangle(footerPen, rect.X, rect.Y, rect.Width, rect.Height);

            string text = "";
            if (vezba.Elementi.Count > 0)
                text = vezba.getOcena().ToString(getColumnFormatString("Odbitak"));
            StringFormat sf = getItemStringFormat("Odbitak");
            g.DrawString(text, footerFont, blackBrush, rect, sf);
        }

    }
}
