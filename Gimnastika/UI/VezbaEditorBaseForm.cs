using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gimnastika.Domain;
using Gimnastika.Dao;
using Gimnastika.Exceptions;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Gimnastika.UI
{
    public partial class VezbaEditorBaseForm : Form
    {
        private Vezba vezba = null;
        protected bool selBodujeSe = false;
        private bool initialized;

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        public Vezba Vezba
        {
            get { return vezba; }
            set { vezba = value; }
        }

        protected readonly int numEmptyRows = VezbaTabela.NUM_EMPTY_ROWS;
        public int NumEmptyRows
        {
            get { return numEmptyRows; }
        }

        protected Color bodujeSeBackColor = Color.Lavender;
        protected Color bodujeSeForeColor = Color.Black;
        protected Color futerBackColor = SystemColors.Window;

        protected int clickedRow;
        protected int clickedColumn;

        public System.Windows.Forms.ToolStrip ToolStrip
        {
            get { return toolStrip1; }
        }

        protected VezbaEditorPresenter presenter;

        public VezbaEditorBaseForm()
        {
            InitializeComponent();
            initUI();

            presenter = new VezbaEditorPresenter(this, null);
        }

        public void setCaption(string caption)
        {
            Text = caption;
        }

        public VezbaEditorBaseForm(int id)
        {
            // TODO: Neka u dijalogu u kome se bira vezba
            // koja se otvara bude moguce da se izabere vise vezbi
            InitializeComponent();
            initUI();

            presenter = new VezbaEditorPresenter(this, id);
        }

        private void initUI()
        {

        }

        public void updateUI()
        {
            updateOsnovniPodaci();
            updateGrid();
        }

        private void updateOsnovniPodaci()
        {
            if (vezba.Gimnasticar != null)
                lblGimnasticarValue.Text = vezba.Gimnasticar.PrezimeIme;
            else
                lblGimnasticarValue.Text = "";
            lblSpravaValue.Text = vezba.Sprava.ToString();
            lblPraviloValue.Text = vezba.Pravilo.Naziv;
        }

        protected virtual void disableAutomaticGridFooterUpdate()
        {

        }

        protected virtual void enableAutomaticGridFooterUpdate()
        {

        }

        public bool save()
        {
            prekiniSelektovanjeBodovnih();
            return presenter.save();
        }

        public void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                default:
                    break;
            }
        }

        // TODO: Kod nove vezbe, izmeni da moze da se aktivira meni desnog tastera misa

        protected virtual void pocniSelektovanjeBodovanih()
        {
            selBodujeSe = true;
            this.Cursor = Cursors.Hand;
        }

        protected virtual void prekiniSelektovanjeBodovnih()
        {
            selBodujeSe = false;
            toolBtnObelezi.Checked = false;
            this.Cursor = Cursors.Default;
        }

        protected bool isFloatCell(int rowIndex, string columnName)
        {
            return isElementRow(rowIndex) && isFloatColumn(columnName)
            || isFooterRow(rowIndex) && isFloatFooterCell(rowIndex, columnName);
        }

        protected bool isElementRow(int rowIndex)
        {
            return (rowIndex >= 0 && rowIndex < vezba.Elementi.Count);
        }

        private bool isFooterRow(int rowIndex)
        {
            return (rowIndex >= firstFooterRowIndex()
                && rowIndex < firstFooterRowIndex() + 3);
        }

        private int firstFooterRowIndex()
        {
            return vezba.Elementi.Count + numEmptyRows;
        }

        private bool isFloatColumn(string columnName)
        {
            return columnName == "Vrednost" || columnName == "VezaSaPrethodnim"
                || columnName == "Zahtev" || columnName == "Odbitak"
                || columnName == "Penalizacija";
        }

        protected bool tryFormatFloatCell(object cellValue, string format,
            out string formattedValue)
        {
            float f;
            if (float.TryParse(cellValue.ToString(), out f))
            {
                char decimalSeparator = Opcije.Instance.DecimalSeparator;
                formattedValue = f.ToString(format).Replace('.', decimalSeparator);
                return true;
            }
            else
            {
                formattedValue = "";
                return false;
            }
        }

        private bool isFloatFooterCell(int rowIndex, string columnName)
        {
            if (!isFooterRow(rowIndex))
                return false;
            switch (rowIndex - firstFooterRowIndex())
            {
                case 0:
                    switch (columnName)
                    { 
                        case "Vrednost":
                        case "VezaSaPrethodnim":
                        case "Zahtev":
                        case "Odbitak":
                        case "Penalizacija":
                            return true;

                        default:
                            return false;
                    }

                case 1:
                    switch (columnName)
                    {
                        case "Vrednost":
                        case "Odbitak":
                        case "Penalizacija":
                            return true;

                        default:
                            return false;
                    }

                case 2:
                    switch (columnName)
                    {
                        case "Vrednost":
                        case "Odbitak":
                            return true;

                        default:
                            return false;
                    }

                default:
                    return false;
            }
        }

        private void VezbaEditorBaseForm_Click(object sender, EventArgs e)
        {
            // TODO: U dijalogu IzaberiElemente, prikazi izabrane u tool tipu

            // TODO: Uradi ovo i za sve ostale kontrole
            prekiniSelektovanjeBodovnih();
        }

        public bool brisiVezbu()
        {
            prekiniSelektovanjeBodovnih();
            return presenter.brisiVezbu();
        }

        private void VezbaEditorBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        /*    if (e.CloseReason == CloseReason.UserClosing
            || e.CloseReason == CloseReason.MdiFormClosing
            || e.CloseReason == CloseReason.WindowsShutDown
            || e.CloseReason == CloseReason.TaskManagerClosing)*/
                e.Cancel = !okToTrash();
        }

        protected bool okToTrash()
        {
            return presenter.okToTrash();
        }

        public bool Modified
        {
            get { return presenter.Modified; }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        public ElementVezbe SelectedElement
        {
            get
            {
                if (isElementRow(SelectedElementRowIndex))
                    return vezba.Elementi[SelectedElementRowIndex];
                else
                    return null;                     
            }
        }

        private void toolBtnMoveUp_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.moveElementUp();
        }

        private void toolBtnMoveDown_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.moveElementDown();
        }

        private void toolBtnObelezi_Click(object sender, EventArgs e)
        {
            if (toolBtnObelezi.Checked)
                pocniSelektovanjeBodovanih();
            else
                prekiniSelektovanjeBodovnih();
        }

        private void toolBtnBrisi_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.deleteElement();
        }

        private void toolBtnIzracunaj_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            izracunaj();
        }

        private void izracunaj()
        {
            updateGridFooter();
        }

        private void toolBtnAdd_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.dodajElementeIzTablice();
        }

        private void VezbaEditorBaseForm_Deactivate(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void lblGimnasticarCaption_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void lblGimnasticarValue_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void lblSpravaCaption_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void lblSpravaValue_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void lblPraviloCaption_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void lblPraviloValue_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
        }

        private void cntMenuGrid_Opening(object sender, CancelEventArgs e)
        {
            mnPonistiVezu.Visible = presenter.cmdPonistiVezuApplies(clickedRow + 1);
            mnVezaSaPrethodnim.Visible = presenter.cmdVezaSaPrethodnimApplies(clickedRow + 1);
        }

        private void mnPonistiVezu_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.ponistiVezu(clickedRow + 1);
        }

        private void mnVezaSaPrethodnim_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.kreirajVezuSaPrethodnim(clickedRow + 1);
        }

        private void mnDodajElemente_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.dodajElemente();
        }

        private void mnDodajElementeIzTablice_Click(object sender, EventArgs e)
        {
            prekiniSelektovanjeBodovnih();
            presenter.dodajElementeIzTablice();
        }

        public bool queryConfirmation(string message)
        {
            DialogResult value = MessageBox.Show(message, "Vezba", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return (value == DialogResult.Yes);
        }

        public void showMessage(string message)
        {
            MessageBox.Show(message, "Vezba", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void showError(string message)
        {
            MessageBox.Show(message, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected virtual void updateGrid()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void insertElementRow(ElementVezbe e)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void updateVezaColumn()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void updateGridFooter()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void updateRedBrojColumn()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void focusElementCell(int redBroj, string columnName)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        protected virtual int SelectedElementRowIndex
        {
            get
            {
                throw new Exception("The method or operation should be implemented in inherited class.");
            }
        }

        public virtual void ukloniElementGridRow(byte redBroj)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void updateElementRow(int redBroj, ElementVezbe element)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual int getSelectedRow()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual int getSelectedColumn()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void selectElementCell(int redBroj, int col)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual void markSelectedElementRow(bool bodujeSe)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual string getColumnName(int col)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public virtual object getElementCellValue(int redBroj, int col)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }
    }
}