using System;
using System.Collections.Generic;
using System.Text;

namespace Gimnastika.Exceptions
{
    [Serializable]
    class GridException : Exception
    {
        private int rowIndex;
        private string columnName;

        public string ColumnName
        {
            get { return columnName; }
        }

        public int RowIndex
        {
            get { return rowIndex; }
        }

        public GridException()
        { 
        
        }

        public GridException(string message, int rowIndex, string columnName)
            : base(message)
        {
            this.rowIndex = rowIndex;
            this.columnName = columnName;
        }

        public GridException(string message, int rowIndex, string columnName,
            Exception inner)
            : base(message, inner)
        {
            this.rowIndex = rowIndex;
            this.columnName = columnName;
        }

    }
}
