using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;
using System.Windows.Forms;

namespace Gimnastika.UI
{
    public class GridColumnsInitializer
    {
        public static void initElement(DataGridViewUserControl dgw)
        {
            dgw.AddColumn("Naziv", "NazivString", 310);
            dgw.AddColumn("Sprava", "Sprava", 70);
            dgw.AddColumn("Tezina", "Tezina", 50);
            dgw.AddColumn("Broj u tablicama", "GrupaBroj", 60);
            DataGridViewColumn col = dgw.AddColumn("Vrednost", "Vrednost", 60, "{0:F2}");
            col.ReadOnly = true;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}
