using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Gimnastika.Domain;

namespace Gimnastika.UI
{
    public class HMergedCell : DataGridViewTextBoxCell
    {
        private int leftColumn = 0;
        private int rightColumn = 0;
        StringAlignment alignment = StringAlignment.Center;
        StringAlignment lineAlignment = StringAlignment.Center;
        StringTrimming trimming = StringTrimming.EllipsisCharacter;

        /// <summary>
        /// Column Index of the left-most cell to be merged.
        /// This cell controls the merged text.
        /// </summary>
        public int LeftColumn
        {
            get { return leftColumn; }
            set { leftColumn = value; }
        }

        /// <summary>
        /// Column Index of the right-most cell to be merged
        /// </summary>
        public int RightColumn
        {
            get { return rightColumn; }
            set { rightColumn = value; }
        }

        public StringAlignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        public StringAlignment LineAlignment
        {
            get { return lineAlignment; }
            set { lineAlignment = value; }
        }

        public StringTrimming Trimming
        {
            get { return trimming; }
            set { trimming = value; }
        }

        public HMergedCell(int leftColumn, int rightColumn)
        {
            this.leftColumn = leftColumn;
            this.rightColumn = rightColumn;
        }

        public HMergedCell(int leftColumn, int rightColumn, StringAlignment alignment,
            StringAlignment lineAlignment) : this(leftColumn, rightColumn)
        {
            this.alignment = alignment;
            this.lineAlignment = lineAlignment;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds,
            Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
            object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            try
            {
                using (
                    Brush backColorBrush = new SolidBrush(cellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(this.DataGridView.GridColor))
                    {
                        // Draw the background
                        graphics.FillRectangle(backColorBrush, cellBounds);

                        // Only the right and bottom lines need to be drawn;
                        // DataGridView takes care of the others.

                        // Draw the separator for rows
                        graphics.DrawLine(gridLinePen,
                            cellBounds.Left, cellBounds.Bottom - 1,
                            cellBounds.Right, cellBounds.Bottom - 1);

                        // Draw the right vertical line for the cell
                        if (ColumnIndex == rightColumn)
                        {
                            graphics.DrawLine(gridLinePen,
                                cellBounds.Right - 1, cellBounds.Top,
                                cellBounds.Right - 1, cellBounds.Bottom);
                        }

                        // Determine the total width of the merged cell
                        int width = 0;
                        for (int i = leftColumn; i <= rightColumn; i++)
                        {
                            width += this.OwningRow.Cells[i].Size.Width;
                        }

                        // Determine the width before the current cell.
                        int widthLeft = 0;
                        for (int i = leftColumn; i < ColumnIndex; i++)
                        {
                            widthLeft += this.OwningRow.Cells[i].Size.Width;
                        }

                        // Draw the text
                        if (this.OwningRow.Cells[leftColumn].Value != null)
                        {
                            RectangleF rectDest = new RectangleF(cellBounds.Left - widthLeft,
                                cellBounds.Top, width, cellBounds.Height);

                            drawCellContent(graphics, rectDest, cellStyle);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        protected virtual void drawCellContent(Graphics g, RectangleF rectDest, 
            DataGridViewCellStyle cellStyle)
        {
            string text = this.OwningRow.Cells[leftColumn].FormattedValue.ToString();
            StringFormat sf = new StringFormat();
            sf.Alignment = alignment;
            sf.LineAlignment = lineAlignment;
            sf.Trimming = trimming;

            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
            g.DrawString(text, cellStyle.Font, foreColorBrush,
                       rectDest, sf);
        }
    }

    public class IzvedbaCaptionCell : HMergedCell
    {
        VezbaEditorBaseForm editorForm;

        public IzvedbaCaptionCell(int leftColumn, int rightColumn, VezbaEditorBaseForm editorForm)
            : base(leftColumn, rightColumn)
        {
            this.editorForm = editorForm;
        }

        protected override void drawCellContent(Graphics g, RectangleF rectDest,
          DataGridViewCellStyle cellStyle)
        {
            Font fontIzvedba = new Font(cellStyle.Font, FontStyle.Bold);
            VezbaTabela.drawIzvedbaCaptionCellContent(g, fontIzvedba,
                cellStyle.Font, editorForm.Vezba.Pravilo, rectDest,
                new SolidBrush(cellStyle.ForeColor));
        }
    }


}
