using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;
using System.ComponentModel;
using System.Drawing;

namespace Gimnastika.UI
{
    public class TabelaElemenata
    {
        public ElementTableItem[,] getElementItems(Sprava s, GrupaElementa g)
        {
            return itemsMap[getElementiKey(s, g)];
        }

        // kljuc je sprava * (Grupa.Max + 1) + grupa
        private Dictionary<int, List<Element>> elementiMap = new Dictionary<int, List<Element>>();
        private Dictionary<int, ElementTableItem[,]> itemsMap = new Dictionary<int, ElementTableItem[,]>();

        // TODO: Neka elementItems bude 1-dimenzionalan niz

        private TabelaElemenataForm form;

        private SizeF elementSizePxl;
        public SizeF getElementSizePxl()
        { 
            return elementSizePxl;
        }

        private Color itemBorderColor;
        private Color itemBorderSelectedColor;
        private Color itemTextColor;

        private Color itemTextSelectedColor;
        public Color ItemTextSelectedColor
        {
            get { return itemTextSelectedColor; }
        }

        private Color tabelaBackColor;
        public Color TabelaBackColor
        {
            get { return tabelaBackColor; }
        }

        private Color headerBorderColor;
        private Color headerTezinaTextColor;
        private Color headerGrupaTextColor;

        private Pen itemBorderPen = null;
        public Pen ItemBorderPen
        {
            get
            {
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
            get
            {
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
            get
            {
                if (headerBorderPen == null)
                    headerBorderPen = new Pen(headerBorderColor);
                return headerBorderPen;
            }
        }

        private Brush headerTezinaTextBrush;
        public Brush HeaderTezinaTextBrush
        {
            get
            {
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

        public TabelaElemenata(List<Element> elementi, SizeF elementSizePxl, TabelaElemenataForm form)
        {
            this.elementSizePxl = elementSizePxl;
            this.form = form;
            fillElementiMap(elementi);
            fillItemsMap();

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

        }

        private void fillElementiMap(List<Element> elementi)
        {
            Sprava[] sprave = new Sprava[] { Sprava.Parter, Sprava.Konj, Sprava.Karike, Sprava.Preskok, Sprava.Razboj,
                Sprava.Vratilo };
            GrupaElementa[] grupe = new GrupaElementa[] { GrupaElementa.I, GrupaElementa.II, GrupaElementa.III,
                GrupaElementa.IV, GrupaElementa.V };
            foreach (Sprava s in sprave)
            {
                foreach (GrupaElementa g in grupe)
                {
                    elementiMap[getElementiKey(s, g)] = getSortedElements(s, g, elementi);                
                }
            }
        }

        private void fillItemsMap()
        {
            Sprava[] sprave = new Sprava[] { Sprava.Parter, Sprava.Konj, Sprava.Karike, Sprava.Preskok, Sprava.Razboj,
                Sprava.Vratilo };
            GrupaElementa[] grupe = new GrupaElementa[] { GrupaElementa.I, GrupaElementa.II, GrupaElementa.III,
                GrupaElementa.IV, GrupaElementa.V };
            foreach (Sprava s in sprave)
            {
                foreach (GrupaElementa g in grupe)
                {
                    createItems(s, g);
                }
            }
        }

        private int getElementiKey(Sprava sprava, GrupaElementa grupa)
        {
            return (int)sprava * ((int)GrupaElementa.Max + 1) + (int)grupa;
        }

        public float getTabelaWidth()
        {
            return 6 * elementSizePxl.Width;
        }

        public int getBrojVrsta(Sprava s, GrupaElementa g)
        {
            return itemsMap[getElementiKey(s, g)].Length / 6;
        }

        public float getTabelaHeight(Sprava s, GrupaElementa g)
        {
            return getBrojVrsta(s, g) * elementSizePxl.Width;
        }

        public void promeniSpravuGrupu(Sprava sprava, GrupaElementa grupa)
        {

        }

        private void createItems(Sprava sprava, GrupaElementa grupa)
        {
            List<Element> elementi = elementiMap[getElementiKey(sprava, grupa)];
            int brojVrsta;
            if (elementi.Count > 0)
                brojVrsta = (elementi[elementi.Count - 1].Broj - 1) / 6 + 1;
            else
                brojVrsta = 1;
            itemsMap[getElementiKey(sprava, grupa)] = new ElementTableItem[brojVrsta, 6];

            int maxBroj = brojVrsta * 6;
            int elemIndex = 0;
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
                createItem(broj, elem, sprava, grupa);
            }
        }

        private List<Element> getSortedElements(Sprava sprava, GrupaElementa grupa, List<Element> elementi)
        {
            List<Element> result = new List<Element>();
            foreach (Element e in elementi)
            {
                if (sprava != Sprava.Undefined)
                {
                    if (e.Sprava != sprava)
                        continue;
                }
                if (grupa != GrupaElementa.Undefined)
                {
                    if (e.Grupa != grupa)
                        continue;
                }
                result.Add(e);
            }

            PropertyDescriptor propDesc =
                TypeDescriptor.GetProperties(typeof(Element))["GrupaBroj"];
            result.Sort(new SortComparer<Element>(propDesc, ListSortDirection.Ascending));
            return result;
        }

        // TODO: Verovatno bi trebalo da se umesto liste elementi (za elemente za trenutnu spravu i grupu) kreira mapa
        // koja bi bila ideksirana po spravi i grupi. Tako bi se izbegla situacija da se dodaje element za jednu spravu i
        // grupu, a lista elementi sadrzi elemente za drugu spravu i  grupu.

        public void addElement(Element e)
        {
            insertElement(e);
            createItem(e.Broj, e, e.Sprava, e.Grupa);
        }

        private void insertElement(Element e)
        {
            List<Element> el = elementiMap[getElementiKey(e.Sprava, e.Grupa)];
            int i = 0;
            // TODO: Obrati paznju i na varijante
            while (i < el.Count && e.Broj >= el[i].Broj)
                i++;
            if (i < el.Count)
                el.Insert(i, e);
            else
                el.Add(e);
        }

        public void createItem(int broj, Element element, Sprava sprava, GrupaElementa grupa)
        {
            ElementTableItem[,] items = itemsMap[getElementiKey(sprava, grupa)];

            int oldBrojVrsta = getBrojVrsta(sprava, grupa);
            if (broj > oldBrojVrsta * 6)
            {
                int newBrojVrsta = (broj - 1) / 6 + 1;
                ElementTableItem[,] newElementItems = new ElementTableItem[newBrojVrsta, 6];
                copyElementItems(newElementItems, items, oldBrojVrsta);
                itemsMap[getElementiKey(sprava, grupa)] = newElementItems;
                items = newElementItems;

                int from = oldBrojVrsta * 6 + 1;
                int to = newBrojVrsta * 6;
                for (int k = from; k <= to; k++)
                    createItem(k, null, sprava, grupa);
            }

            int i = getRowIndex(broj);
            int j = getColumnIndex(broj);
            ElementTableItem item = new ElementTableItem(sprava, grupa,
                broj, element,
                new PointF(j * elementSizePxl.Width, i * elementSizePxl.Height),
                elementSizePxl, this);
            items[i, j] = item;
            if (element != null && form.izabrani.ContainsKey(element.Id))
                item.Selected = true;
        }

        private void copyElementItems(ElementTableItem[,] newElementItems,
            ElementTableItem[,] elementItems, int brojVrsta)
        {
            for (int i = 0; i < brojVrsta; i++)
                for (int j = 0; j < 6; j++)
                    newElementItems[i, j] = elementItems[i, j];
        }

        public ElementTableItem getItem(int broj, Sprava s, GrupaElementa g)
        {
            if (broj < 1 || broj > getBrojVrsta(s, g) * 6)
                return null;

            ElementTableItem[,] items = itemsMap[getElementiKey(s, g)];
            return items[getRowIndex(broj), getColumnIndex(broj)];
        }

        private int getRowIndex(int broj)
        {
            return (broj - 1) / 6;
        }

        private int getColumnIndex(int broj)
        {
            return (broj - 1) % 6;
        }

        public ElementTableItem getItem(PointF tablePt, Sprava s, GrupaElementa g)
        {
            if (tablePt.X < 0 || tablePt.Y < 0)
                return null;

            int j = (int)Math.Floor((double)tablePt.X / elementSizePxl.Width);
            int i = (int)Math.Floor((double)tablePt.Y / elementSizePxl.Height);

            if (i >= getBrojVrsta(s, g) || j >= 6)
                return null;

            ElementTableItem[,] items = itemsMap[getElementiKey(s, g)];
            return items[i, j];
        }

        public void promeniElement(Element oldElem, Element newElem)
        {
            if (oldElem.Sprava != newElem.Sprava || oldElem.Grupa != newElem.Grupa)
            {
                throw new Exception("Greska u programu.");
            }
            // TODO: Proveri da li ovo radi. Pretpostavljam da treba da se implementira Equals i GetHashCode da bi radilo.
            List<Element> el = elementiMap[getElementiKey(oldElem.Sprava, oldElem.Grupa)];
            el.Remove(oldElem);
            insertElement(newElem);
        }

        public void removeElement(Element element)
        {
            List<Element> el = elementiMap[getElementiKey(element.Sprava, element.Grupa)];
            el.Remove(element);
            createItem(element.Broj, null, element.Sprava, element.Grupa);            
        }
    }
}
