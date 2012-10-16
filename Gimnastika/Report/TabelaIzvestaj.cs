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
	public class TabelaIzvestaj : Izvestaj
	{
        Font gimnasticarFont;
        Font nazivVezbeFont;
        Font spravaFont;
        Font datumFont;

        TabelaElemenata tabela;
        IList<TableItemBoundary> itemBoundaries;

        public TabelaIzvestaj(TabelaElemenata tabela)
		{
            this.tabela = tabela;
			DocumentName = "Tabela Elemenata";
            
            gimnasticarFont = new Font("Arial", 10, FontStyle.Bold);
            nazivVezbeFont = new Font("Arial", 12);
            spravaFont = new Font("Arial", 10, FontStyle.Bold);
            datumFont = new Font("Arial", 8);
        }

        protected override void doSetupContent(Graphics g)
		{
            itemBoundaries = tabela.getItemBoundaries();
            lastPageNum = tabela.getPageCount();
		}

        public override void drawContent(Graphics g, int pageNum)
        {
            TableItemBoundary itemBoundary = itemBoundaries[pageNum - 1];
            PointF pt = new PointF(0.0f, 0.0f);
            IList<ElementTableItem> items = tabela.getItems(itemBoundary.sprava, itemBoundary.grupa, itemBoundary.startBroj,
                itemBoundary.startBroj + 23);
            foreach (ElementTableItem item in items)
            {
                item.draw(g, pt);
            }
        }

        public override void drawHeader(Graphics g, int pageNum)
        {

        }
	}
}
