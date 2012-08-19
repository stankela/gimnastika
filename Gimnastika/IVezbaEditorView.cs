using System;
using Gimnastika.Domain;

namespace Gimnastika
{
    interface IVezbaEditorView
    {
        bool brisiVezbu();
        void focusElementCell(int redBroj, string columnName);
        string getColumnName(int col);
        object getElementCellValue(int redBroj, int col);
        int getSelectedColumn();
        int getSelectedRow();
        bool Initialized { get; set; }
        void insertElementRow(ElementVezbe e);
        void markSelectedElementRow(bool bodujeSe);
        bool Modified { get; }
        int NumEmptyRows { get; }
        bool queryConfirmation(string message);
        bool save();
        ElementVezbe SelectedElement { get; }
        void selectElementCell(int redBroj, int col);
        void setCaption(string caption);
        void setFocus(string propertyName);
        void showError(string message);
        void showMessage(string message);
        void ukloniElementGridRow(byte redBroj);
        void updateElementRow(int redBroj, ElementVezbe element);
        void updateGridFooter();
        void updateRedBrojColumn();
        void updateUI();
        void updateVezaColumn();
        Vezba Vezba { get; set; }
        void startBatchUpdate();
        void endBatchUpdate();
    }
}
