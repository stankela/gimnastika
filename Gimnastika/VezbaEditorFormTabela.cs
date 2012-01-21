using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Entities;

namespace Gimnastika
{
    public partial class VezbaEditorFormTabela : VezbaEditorBaseForm, IVezbaEditorView
    {
        VezbaTabela vezbaTabela;
        bool batchUpdate;
        int selectedElementRowIndex = -1;

        public VezbaEditorFormTabela()
        {
            InitializeComponent();
            initUI();
            presenter.initialize();
        }

        public VezbaEditorFormTabela(int vezbaId)
            : base(vezbaId)
        {
            InitializeComponent();
            initUI();
            presenter.initialize();
        }

        private void initUI()
        {

        }

        private void createTabela()
        {
            Graphics g = panelTabela.CreateGraphics();
            vezbaTabela = new VezbaTabela(Point.Empty, panelTabela.Width, g,
                VezbaTabela.NUM_EMPTY_ROWS, Vezba);
            g.Dispose();
        }

        protected override void updateGrid()
        {
            if (vezbaTabela == null)
                createTabela();
            panelTabela.Invalidate();
        }

        public void startBatchUpdate()
        {
            batchUpdate = true;   
        }

        public void endBatchUpdate()
        {
            batchUpdate = false;
            updateGrid();
        }

        public override void insertElementRow(ElementVezbe e)
        {
            vezbaTabela = null;
            if (!batchUpdate)
                updateGrid();
        }

        public override void updateRedBrojColumn()
        {
            if (!batchUpdate)
                updateGrid();
        }

        public override void updateVezaColumn()
        {
            if (!batchUpdate)
                updateGrid();
        }

        public override void selectElementCell(int redBroj, int col)
        {
            vezbaTabela.selectElementCell(redBroj, col);
        }

        public override int getSelectedColumn()
        {
            return 0;
        }

        public override void updateGridFooter()
        {
            if (!batchUpdate)
                updateGrid();
        }

        public override void focusElementCell(int redBroj, string columnName)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        protected override int SelectedElementRowIndex
        {
            get
            {
                return selectedElementRowIndex;
            }
        }

        public override void ukloniElementGridRow(byte redBroj)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public override void updateElementRow(int redBroj, ElementVezbe element)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public override int getSelectedRow()
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public override void markSelectedElementRow(bool bodujeSe)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public override string getColumnName(int col)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        public override object getElementCellValue(int redBroj, int col)
        {
            throw new Exception("The method or operation should be implemented in inherited class.");
        }

        private void panelTabela_Paint(object sender, PaintEventArgs e)
        {
            vezbaTabela.draw(e.Graphics);
        }
    }
}