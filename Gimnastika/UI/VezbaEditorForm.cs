using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;

namespace Gimnastika.UI
{
    public partial class VezbaEditorForm : VezbaEditorBaseForm, IVezbaEditorView
    {
        public VezbaEditorForm() 
            : base()
        {
            InitializeComponent();
            initUI();
            presenter.initialize();
        }

        private void initUI()
        {
            setupGrid();
        }

        public VezbaEditorForm(int vezbaId)
            : base(vezbaId)
        {
            InitializeComponent();
            initUI();
            presenter.initialize();
        }

        
        private void setupGrid()
        {
            gridElementi.MultiSelect = false;
            gridElementi.AllowUserToAddRows = false;
            gridElementi.AllowUserToDeleteRows = false;
            gridElementi.AllowUserToResizeRows = false;
            gridElementi.AutoGenerateColumns = false;
            gridElementi.GridColor = Color.Black;
            gridElementi.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "RedBroj";
            column.Name = "RedBroj";
            column.HeaderText = "Redni broj";
            column.ReadOnly = true;
            column.Width = VezbaTabela.RED_BROJ_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "NazivElementa";
            column.Name = "NazivElementa";
            column.HeaderText = "Opis";
            column.ReadOnly = true;
            column.Width = VezbaTabela.NAZIV_ELEMENATA_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Tezina";
            column.Name = "Tezina";
            column.HeaderText = "Tezina";
            column.ReadOnly = true;
            column.Width = VezbaTabela.TEZINA_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "GrupaBroj";
            column.Name = "GrupaBroj";
            column.HeaderText = "Broj u tablicama";
            column.ReadOnly = true;
            column.Width = VezbaTabela.GRUPA_BROJ_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Vrednost";
            column.Name = "Vrednost";
            column.HeaderText = "Vrednost";
            column.ReadOnly = true;
            column.DefaultCellStyle.Format = "F2";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = VezbaTabela.VREDNOST_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "VezaSaPrethodnim";
            column.Name = "VezaSaPrethodnim";
            column.HeaderText = "Veza";
            column.ReadOnly = true;
            column.DefaultCellStyle.Format = "F2";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = VezbaTabela.VEZA_SA_PRETHODNIM_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Zahtev";
            column.Name = "Zahtev";
            column.HeaderText = "Zahtev";
            column.ReadOnly = false;
            column.DefaultCellStyle.Format = "F2";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = VezbaTabela.ZAHTEV_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Odbitak";
            column.Name = "Odbitak";
            column.HeaderText = "Odbitak";
            column.ReadOnly = false;
            column.DefaultCellStyle.Format = "F2";
            column.Width = VezbaTabela.ODBITAK_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Penalizacija";
            column.Name = "Penalizacija";
            column.HeaderText = "Penalizacija";
            column.ReadOnly = false;
            column.DefaultCellStyle.Format = "F2";
            column.Width = VezbaTabela.PENALIZACIJA_WIDTH;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridElementi.Columns.Add(column);

            addGridFooter();
        }

        private void addGridFooter()
        {
            disableAutomaticGridFooterUpdate();

            addEmptyRows(numEmptyRows);
            addUkupnoRow();
            addPocetnaOcenaRow();
            addIzvedbaRow();

            enableAutomaticGridFooterUpdate();
        }
        
        private void addEmptyRows(int numRows)
        {
            gridElementi.Rows.Add(numEmptyRows);
        }

        private void addUkupnoRow()
        {
            Font ukupnoCaptionFont = new Font("Arial", 9, FontStyle.Bold);

            int row = gridElementi.Rows.Add();
            gridElementi.Rows[row].DefaultCellStyle.BackColor = futerBackColor;
            addHMergedCell(row, "RedBroj", "GrupaBroj", "UKUPNO", StringAlignment.Far,
                StringAlignment.Center, true, ukupnoCaptionFont);
            gridElementi.Rows[row].Cells["VezaSaPrethodnim"].ReadOnly = true;
            gridElementi.Rows[row].Cells["Zahtev"].ReadOnly = true;
            gridElementi.Rows[row].Cells["Odbitak"].ReadOnly = true;
            gridElementi.Rows[row].Cells["Penalizacija"].ReadOnly = true;
        }

        private void addHMergedCell(int row, string leftColumn, string rightColumn,
           string text, StringAlignment alignment, StringAlignment lineAlignment,
           bool readOnly, Font font)
        {
            int leftIndex = gridElementi.Columns[leftColumn].Index;
            int rightIndex = gridElementi.Columns[rightColumn].Index;
            for (int i = leftIndex; i <= rightIndex; i++)
            {
                HMergedCell cell = new HMergedCell(leftIndex, rightIndex,
                    alignment, lineAlignment);
                gridElementi.Rows[row].Cells[i] = cell;
                cell.ReadOnly = readOnly;
                if (font != null)
                    cell.Style.Font = font;
            }
            gridElementi.Rows[row].Cells[leftIndex].Value = text;
        }

        private void addPocetnaOcenaRow()
        {
            Font pocOcenaCaptionFont = new Font("Arial", 11, FontStyle.Bold);
            Font pocOcenaFont = new Font(gridElementi.DefaultCellStyle.Font,
                FontStyle.Bold);

            int row = gridElementi.Rows.Add();
            gridElementi.Rows[row].DefaultCellStyle.BackColor = futerBackColor;
            addHMergedCell(row, "RedBroj", "GrupaBroj", "POCETNA OCENA",
                StringAlignment.Far, StringAlignment.Center, true, pocOcenaCaptionFont);
            addHMergedCell(row, "Vrednost", "Zahtev", "", StringAlignment.Center,
                StringAlignment.Center, true, pocOcenaFont);
            addHMergedCell(row, "Odbitak", "Penalizacija", "", StringAlignment.Center,
                StringAlignment.Center, true, null);
        }

        private void addIzvedbaRow()
        {
            int row = gridElementi.Rows.Add();
            gridElementi.Rows[row].DefaultCellStyle.BackColor = futerBackColor;
            addIzvedbaCaptionCell(row);
            addHMergedCell(row, "Vrednost", "Zahtev", "", StringAlignment.Center,
                StringAlignment.Center, true, null);
            addHMergedCell(row, "Odbitak", "Penalizacija", "", StringAlignment.Center,
                StringAlignment.Center, true, null);
        }

        private void addIzvedbaCaptionCell(int row)
        {
            int leftIndex = gridElementi.Columns["RedBroj"].Index;
            int rightIndex = gridElementi.Columns["GrupaBroj"].Index;
            for (int i = leftIndex; i <= rightIndex; i++)
            {
                IzvedbaCaptionCell cell = new IzvedbaCaptionCell(leftIndex,
                    rightIndex, this);
                gridElementi.Rows[row].Cells[i] = cell;
                cell.ReadOnly = true;
                //               cell.Style.Font = new Font(gridElementi.DefaultCellStyle.Font, FontStyle.Regular);
                cell.Style.Font = new Font("Arial", 9);
            }
            gridElementi.Rows[row].Cells[leftIndex].Value = "IZVEDBA";
        }

        protected override void updateGrid()
        {
            clearRows();
            foreach (ElementVezbe e in Vezba.Elementi)
                insertElementRow(e);
            updateVezaColumn();
            addGridFooter();
            if (Vezba.Elementi.Count > 0)
            {
                updateGridFooter();
            }
        }

        private void clearRows()
        {
            gridElementi.Rows.Clear();
        }

        public override void insertElementRow(ElementVezbe e)
        {
            // ovo ne generise CellValueChanged event
            gridElementi.Rows.Insert(e.RedBroj - 1, getRowValues(e));
        }

        private object[] getRowValues(ElementVezbe e)
        {
            if (e.Tezina != TezinaElementa.Undefined)
                return new object[] { e.RedBroj, e.NazivElementa, e.Tezina,
                    e.GrupaBroj, e.Vrednost, e.VezaSaPrethodnim,
                    e.Zahtev, e.Odbitak, e.Penalizacija };
            else
                return new object[] { e.RedBroj, e.NazivElementa, null,
                    e.GrupaBroj, e.Vrednost, e.VezaSaPrethodnim,
                    e.Zahtev, e.Odbitak, e.Penalizacija };
        }
        
        public override void updateGridFooter()
        {
            disableAutomaticGridFooterUpdate();

            updateUkupnoRow();
            updatePocetnaOcenaRow();
            updateIzvedbaRow();

            enableAutomaticGridFooterUpdate();
            gridElementi.Refresh();
        }

        private void updateUkupnoRow()
        {
            DataGridViewRow rowUkupno =
                gridElementi.Rows[Vezba.Elementi.Count + numEmptyRows];
            rowUkupno.Cells["Vrednost"].Value = Vezba.getVrednostUkupno();
            rowUkupno.Cells["VezaSaPrethodnim"].Value = Vezba.getVezaUkupno();
            rowUkupno.Cells["Zahtev"].Value = Vezba.getZahtevUkupno();
            rowUkupno.Cells["Odbitak"].Value = Vezba.getOdbitakUkupno();
            rowUkupno.Cells["Penalizacija"].Value = Vezba.getPenalizacijaUkupno();
        }

        private void updatePocetnaOcenaRow()
        {
            DataGridViewRow rowPocetnaOcena =
                gridElementi.Rows[Vezba.Elementi.Count + numEmptyRows + 1];
            rowPocetnaOcena.Cells["Vrednost"].Value = Vezba.getPocetnaOcena();
            rowPocetnaOcena.Cells["Odbitak"].Value = Vezba.getOdbitakUkupno() +
                Vezba.getPenalizacijaUkupno();
        }

        private void updateIzvedbaRow()
        {
            DataGridViewRow rowIzvedba =
                gridElementi.Rows[Vezba.Elementi.Count + numEmptyRows + 2];
            rowIzvedba.Cells["Vrednost"].Value = Vezba.getIzvedba();
            rowIzvedba.Cells["Odbitak"].Value = Vezba.getOcena();
        }

        public override void updateVezaColumn()
        {
            disableAutomaticGridFooterUpdate();

            int vezaCol = gridElementi.Columns["VezaSaPrethodnim"].Index;
            List<int> veze = Vezba.getVeze();
            int row = 0;
            for (int i = 0; i < veze.Count / 2; i++)
            {
                int firstElement = veze[2 * i];
                int lastElement = veze[2 * i + 1];
                while (row < firstElement)
                    gridElementi.Rows[row++].Cells[vezaCol] = new DataGridViewTextBoxCell();
                for (int j = firstElement; j <= lastElement; j++)
                {
                    gridElementi.Rows[j].Cells[vezaCol] = new VMergedCell(
                        firstElement, lastElement, StringAlignment.Center,
                        StringAlignment.Center);
                }
                gridElementi.Rows[firstElement].Cells[vezaCol].Value =
                    Vezba.Elementi[lastElement].VezaSaPrethodnim;
                row = lastElement + 1;
            }
            while (row < Vezba.Elementi.Count)
                gridElementi.Rows[row++].Cells[vezaCol] = new DataGridViewTextBoxCell();

            enableAutomaticGridFooterUpdate();
        }

        public override void updateRedBrojColumn()
        {
            disableAutomaticGridFooterUpdate();
            for (int i = 0; i < Vezba.Elementi.Count; i++)
            {
                DataGridViewRow row = gridElementi.Rows[i];
                row.Cells["RedBroj"].Value = Vezba.Elementi[i].RedBroj;
            }
            enableAutomaticGridFooterUpdate();
        }

        public override void focusElementCell(int redBroj, string columnName)
        {
            gridElementi.Focus();
            gridElementi.CurrentCell = gridElementi.Rows[redBroj - 1].Cells[columnName];
        }

        protected override void pocniSelektovanjeBodovanih()
        {
            base.pocniSelektovanjeBodovanih();
            gridElementi.Cursor = Cursors.Hand;
        }

        protected override void prekiniSelektovanjeBodovnih()
        {
            base.prekiniSelektovanjeBodovnih();
            gridElementi.Cursor = Cursors.Default;
        }

        protected override int SelectedElementRowIndex
        {
            get
            {
                if (gridElementi.Rows.Count > 0)
                    return gridElementi.CurrentRow.Index;
                else
                    return -1;
            }
        }

        public override void ukloniElementGridRow(byte redBroj)
        {
            if (isElementRow(redBroj - 1))
                gridElementi.Rows.RemoveAt(redBroj - 1);
        }

        public override void updateElementRow(int redBroj, ElementVezbe element)
        {
            disableAutomaticGridFooterUpdate();
            gridElementi.Rows[redBroj - 1].SetValues(getRowValues(element));
            enableAutomaticGridFooterUpdate();
        }

        public override int getSelectedRow()
        {
            return gridElementi.CurrentCell.RowIndex;
        }

        public override int getSelectedColumn()
        {
            return gridElementi.CurrentCell.ColumnIndex;
        }

        public override void selectElementCell(int redBroj, int col)
        {
            if (redBroj >= 1 && redBroj <= Vezba.Elementi.Count)
                gridElementi.CurrentCell = gridElementi.Rows[redBroj - 1].Cells[col];
        }

        public override void markSelectedElementRow(bool bodujeSe)
        {
            if (isElementRow(SelectedElementRowIndex))
            {
                DataGridViewRow row = gridElementi.Rows[SelectedElementRowIndex];
                if (bodujeSe)
                {
                    row.DefaultCellStyle.BackColor = bodujeSeBackColor;
                    row.DefaultCellStyle.ForeColor = bodujeSeForeColor;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = gridElementi.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = gridElementi.DefaultCellStyle.ForeColor;
                }
            }
        }

        public override string getColumnName(int col)
        {
            return gridElementi.Columns[col].Name;
        }

        public override object getElementCellValue(int redBroj, int col)
        {
            return gridElementi.Rows[redBroj - 1].Cells[col].Value;
        }

        private void gridElementi_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // TODO: Ispitaj kako moze da se optimizuje formatiranje
            if (isElementRow(e.RowIndex))
            {
                ElementVezbe elem = Vezba.Elementi[e.RowIndex];
                DataGridViewRow row = gridElementi.Rows[e.RowIndex];
                if (elem.BodujeSe)
                {
                    row.DefaultCellStyle.BackColor = bodujeSeBackColor;
                    row.DefaultCellStyle.ForeColor = bodujeSeForeColor;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = gridElementi.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = gridElementi.DefaultCellStyle.ForeColor;
                }
            }

            string columnName = gridElementi.Columns[e.ColumnIndex].Name;
            if (isFloatCell(e.RowIndex, columnName))
            {
                if (e.Value != null)
                {
                    string format = gridElementi.Columns[e.ColumnIndex].
                        DefaultCellStyle.Format;
                    string formattedValue;
                    if (tryFormatFloatCell(e.Value, format, out formattedValue))
                    {
                        e.Value = formattedValue;
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void gridElementi_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string columnName = gridElementi.Columns[e.ColumnIndex].Name;
            if (isFloatCell(e.RowIndex, columnName))
            {
                if (e.Value != null)
                {
                    try
                    {
                        char decimalSeparator = Opcije.Instance.DecimalSeparator;
                        e.Value = float.Parse(e.Value.ToString().Replace(decimalSeparator, '.'));
                        // Set the ParsingApplied property to show the event is handled
                        e.ParsingApplied = true;

                    }
                    catch (FormatException)
                    {
                        // Set to false in case another CellParsing handler wants to 
                        // try to parse this DataGridViewCellParsingEventArgs instance.
                        e.ParsingApplied = false;
                    }
                }
            }
        }

        private void gridElementi_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (gridElementi.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell)
                {
                    clickedRow = gridElementi.HitTest(e.X, e.Y).RowIndex;
                    clickedColumn = gridElementi.HitTest(e.X, e.Y).ColumnIndex;
                    if (clickedRow < Vezba.Elementi.Count)
                        cntMenuGrid.Show(gridElementi, e.Location);
                }
            }
        }

        private void gridElementi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (selBodujeSe)
                presenter.selektovanjeBodovanihCellClick();
        }

        private void gridElementi_Leave(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        // dogadja se kada korisnik promeni vrednost celije, ili kada se u programu
        // svojstvu Cells[].Value dodeli vrednost
        protected void gridElementi_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isElementRow(e.RowIndex))
                presenter.elementCellChanged(e.RowIndex + 1, e.ColumnIndex);
        }

        protected override void disableAutomaticGridFooterUpdate()
        {
            gridElementi.CellValueChanged -= gridElementi_CellValueChanged;
        }

        protected override void enableAutomaticGridFooterUpdate()
        {
            gridElementi.CellValueChanged += gridElementi_CellValueChanged;
        }

        public DataGridView getGridElementi()
        {
            return gridElementi;
        }

        public void startBatchUpdate()
        { 
        
        }

        public void endBatchUpdate()
        { 
        
        }
    }
}