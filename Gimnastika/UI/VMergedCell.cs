using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Gimnastika.UI
{
    public class VMergedCell : DataGridViewTextBoxCell
    {
        private int firstRow = 0;
        private int lastRow = 0;
        StringAlignment alignment = StringAlignment.Center;
        StringAlignment lineAlignment = StringAlignment.Center;
        StringTrimming trimming = StringTrimming.EllipsisCharacter;

        public int FirstRow
        {
            get { return firstRow; }
            set { firstRow = value; }
        }

        public int LastRow
        {
            get { return lastRow; }
            set { lastRow = value; }
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

        public VMergedCell(int firstRow, int lastRow)
        {
            this.firstRow = firstRow;
            this.lastRow = lastRow;
        }

        public VMergedCell(int firstRow, int lastRow, StringAlignment alignment,
            StringAlignment lineAlignment)
            : this(firstRow, lastRow)
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
                    Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor),
                    backColorBrush = new SolidBrush(cellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(this.DataGridView.GridColor))
                    {
                        // Draw the background
                        graphics.FillRectangle(backColorBrush, cellBounds);

                        // Only the right and bottom lines need to be drawn;
                        // DataGridView takes care of the others.

                        // Draw the right vertical line
                        graphics.DrawLine(gridLinePen,
                            cellBounds.Right - 1, cellBounds.Top,
                            cellBounds.Right - 1, cellBounds.Bottom);

                        // Draw the bottom line
                        if (this.RowIndex == lastRow)
                        {
                            graphics.DrawLine(gridLinePen,
                                cellBounds.Left, cellBounds.Bottom - 1,
                                cellBounds.Right, cellBounds.Bottom - 1);
                        }

                        DataGridView grid = this.DataGridView;
                        int col = this.OwningColumn.Index;

                        // Determine the total heigth of the merged cell
                        int height = 0;
                        for (int i = firstRow; i <= lastRow; i++)
                        {

                            height += grid.Rows[i].Cells[col].Size.Height;
                        }

                        // Determine the heigth before the current cell.
                        int heightPrev = 0;
                        for (int i = firstRow; i < this.RowIndex; i++)
                        {
                            heightPrev += grid.Rows[i].Cells[col].Size.Height;
                        }

                        // Draw the text
                        if (grid.Rows[firstRow].Cells[col].Value != null)
                        {
                            string text = grid.Rows[firstRow].Cells[col].FormattedValue.ToString();
                            StringFormat sf = new StringFormat();
                            sf.Alignment = alignment;
                            sf.LineAlignment = lineAlignment;
                            sf.Trimming = trimming;

                            RectangleF rectDest = new RectangleF(cellBounds.Left,
                                cellBounds.Top - heightPrev, cellBounds.Width, height);
                            graphics.DrawString(text, cellStyle.Font, foreColorBrush,
                                rectDest, sf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }
    }
}
