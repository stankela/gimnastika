using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Entities;
using System.Windows.Forms;
using Gimnastika.Exceptions;
using Gimnastika.Dao;

namespace Gimnastika
{
    public class VezbaEditorPresenter
    {
        private IVezbaEditorView view;
        private Nullable<int> vezbaId;
        private Vezba vezba;
        private Vezba original;
        private bool modified;
        private bool existsInDatabase;

        public bool Modified
        {
            get { return modified; }
        }

        public VezbaEditorPresenter(VezbaEditorBaseForm form, Nullable<int> vezbaId)
        {
            this.view = form as IVezbaEditorView;
            this.vezbaId = vezbaId;
        }

        public void initialize()
        {
            if (vezbaId == null)
            {
                OsnovniPodaciVezbeForm f = new OsnovniPodaciVezbeForm();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    vezba = new Vezba();
                    vezba.Gimnasticar = f.Gimnasticar;
                    vezba.Sprava = f.Sprava;
                    vezba.Pravilo = f.Pravilo;
                    vezba.Naziv = f.Naziv;

                    view.Vezba = vezba;
                    view.updateUI();

                    existsInDatabase = false;
                    modified = false;
                    view.setCaption(getCaption());
                    view.Initialized = true;
                }
                else
                {
                    view.Initialized = false;
                }
            }
            else
            {
                vezba = new VezbaDAO().getById(vezbaId.Value);
                original = (Vezba)vezba.Clone(new TypeAsocijacijaPair[] { 
                    new TypeAsocijacijaPair(typeof(Vezba)), 
                    new TypeAsocijacijaPair(typeof(ElementVezbe)) });
                view.Vezba = vezba;
                view.updateUI();

                existsInDatabase = true;
                modified = false;
                view.setCaption(getCaption());
                if (vezba.Elementi.Count > 0)
                    view.selectElementCell(1, 0);

                view.Initialized = true;
                // TODO: Treba hvatati database izuzetke i postaviti initalized na false
            }
        }

        private string getCaption()
        {
            return "Vezbe" + " - " + fileTitle();
        }

        private string fileTitle()
        {
            // TODO:
            if (!existsInDatabase)
                return "Nova vezba";
            else
                return "Vezba";
            //return (strFileName != null && strFileName.Length > 0) ?
            //             Path.GetFileName(strFileName) : "Untitled";
        }

        public void deleteElement()
        {
            ElementVezbe selectedElement = view.SelectedElement;
            if (selectedElement != null)
            {
                string message = string.Format("Da li zelite da izbrisete " +
                    "element \"{0}\"?", selectedElement.NazivElementa);
                bool choice = view.queryConfirmation(message);
                if (choice)
                {
                    try
                    {
                        vezba.UkloniElement(selectedElement);
                        view.ukloniElementGridRow(selectedElement.RedBroj);
                        view.updateRedBrojColumn();
                        view.updateGridFooter();
                        modified = true;
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Problem prilikom pokusaja brisanja " +
                            "elementa \"{0}\". \n{1}", selectedElement.NazivElementa, ex.Message);
                        view.showMessage(message);
                    }
                }
            }
        }

        public void moveElementUp()
        {
            ElementVezbe selectedElement = view.SelectedElement;
            if (selectedElement != null)
            {
                byte redBroj = selectedElement.RedBroj;
                if (vezba.moveElementUp(redBroj))
                {
                    view.updateElementRow(redBroj - 1, vezba.Elementi[redBroj - 2]);
                    view.updateElementRow(redBroj, vezba.Elementi[redBroj - 1]);
                    view.selectElementCell(redBroj - 1, view.getSelectedColumn());
                    modified = true;
                }
            }
        }

        public void moveElementDown()
        {
            ElementVezbe selectedElement = view.SelectedElement;
            if (selectedElement != null)
            {
                byte redBroj = selectedElement.RedBroj;
                if (vezba.moveElementDown(redBroj))
                {
                    view.updateElementRow(redBroj, vezba.Elementi[redBroj - 1]);
                    view.updateElementRow(redBroj + 1, vezba.Elementi[redBroj]);
                    view.selectElementCell(redBroj + 1, view.getSelectedColumn());
                    modified = true;
                }
            }
        }

        public void selektovanjeBodovanihCellClick()
        {
            // TODO: U rezimu selektovanja vezbi koje se boduju, trebalo bi da se
            // sakrije trenutno selektovana celija, i da ne bude moguce menjanje celija
            // U normalnom rezimu, kada se selektuje celija u read onlu koloni, boja bi
            // trebala da bude kao i boja vrste (ili bela ili boja bodovanog
            // elementa) - dakle ne bi trebala da se vidi selekcija
            ElementVezbe selectedElement = view.SelectedElement;
            if (selectedElement != null)
            {
                if (!selectedElement.BodujeSe)
                {
                    // TODO: Mozda bi trebalo prikazati poruku ukoliko uslov
                    // if naredbe nije ispunjen
                    if (vezba.canSelektujElement(selectedElement.RedBroj))
                    {
                        selectedElement.BodujeSe = true;
                        view.markSelectedElementRow(true);
                        view.updateGridFooter();
                        modified = true;
                    }
                }
                else
                {
                    selectedElement.BodujeSe = false;
                    view.markSelectedElementRow(false);
                    view.updateGridFooter();
                    modified = true;
                }
            }
        }

        public void dodajElementeIzTablice()
        {
            TabelaElemenataForm f = new TabelaElemenataForm(
                TabelaElemenataForm.TabelaElemenataFormRezimRada.Select, vezba.Sprava);
            if (f.ShowDialog() == DialogResult.OK && f.IzabraniElementi.Count > 0)
            {
                dodajElemente(f.IzabraniElementi);
            }
        }

        private void dodajElemente(List<Element> elementi)
        {
            bool dodajNaTrenutnuPoziciju = true;
            int index = vezba.Elementi.Count;
            if (dodajNaTrenutnuPoziciju)
            {
                ElementVezbe selectedElement = view.SelectedElement;
                if (selectedElement != null)
                    index = selectedElement.RedBroj - 1;
            }

            view.startBatchUpdate();
            foreach (Element elem in elementi)
            {
                ElementVezbe ev = new ElementVezbe(elem.Naziv, elem.EngleskiNaziv,
                    elem.IsTablicniElement, elem.Grupa, elem.Tezina, elem.Broj,
                    elem.PodBroj);
                vezba.DodajElement(index++, ev);
                view.insertElementRow(ev);
            }
            if (dodajNaTrenutnuPoziciju)
                view.updateRedBrojColumn();

            // selektuj zadnji dodat element
            view.selectElementCell(index, view.getSelectedColumn());
            view.endBatchUpdate();

            modified = true;
        }

        public void dodajElemente()
        {
            IzaberiElementeForm f = new IzaberiElementeForm(vezba.Sprava);
            if (f.ShowDialog() == DialogResult.OK && f.IzabraniElementi.Count > 0)
            {
                dodajElemente(f.IzabraniElementi);
            }
        }

        public void elementCellChanged(int redBroj, int col)
        {
            try
            {
                modified = true;
                updateElementFromCell(redBroj, col);
                view.updateGridFooter();
            }
            catch (GridException ex)
            {
                view.showError(ex.Message);
                view.focusElementCell(ex.RowIndex, ex.ColumnName);
            }
            catch (InvalidPropertyException ex)
            {
                view.showError(ex.Message);
                view.setFocus(ex.InvalidProperty);
            }
        }

        private void updateElementFromCell(int redBroj, int col)
        {
            ElementVezbe element = vezba.Elementi[redBroj - 1];
            string columnName = "";
            string errMsg = "";
            object cellValue = view.getElementCellValue(redBroj, col);
            try
            {
                columnName = view.getColumnName(col);
                switch (columnName)
                {
                    case "Zahtev":
                        errMsg = "Vrednost za zahtev mora da bude broj.";
                        element.Zahtev = parseFloatCell(cellValue);
                        break;

                    case "Odbitak":
                        errMsg = "Vrednost za odbitak mora da bude broj.";
                        element.Odbitak = parseFloatCell(cellValue);
                        break;

                    case "Penalizacija":
                        errMsg = "Vrednost za penalizaciju mora da bude broj.";
                        element.Penalizacija = parseFloatCell(cellValue);
                        break;

                    default:
                        break;
                }
            }
            catch (FormatException)
            {
                throw new GridException(errMsg, redBroj, columnName);
            }
            catch (InvalidPropertyException ex)
            {
                throw new GridException(ex.Message, redBroj, columnName, ex);
            }
        }

        Nullable<float> parseFloatCell(object cellValue)
        {
            Nullable<float> result = null;
            if (cellValue != null)
            {
                char decimalSeparator = Opcije.Instance.DecimalSeparator;
                result = float.Parse(cellValue.ToString().Replace(decimalSeparator, '.'));
            }
            return result;
        }


        public void ponistiVezu(int redBroj)
        {
            if (vezba.isDeoVeze(redBroj))
            {
                if (vezba.raskiniVezu(redBroj))
                {
                    modified = true;
                    view.updateVezaColumn();
                    view.updateGridFooter();
                }
            }
        }

        public void kreirajVezuSaPrethodnim(int redBroj)
        {
            if (vezba.Elementi[redBroj - 1].VezaSaPrethodnim == null
            && vezba.canCreateVezaSaPrethodnim(redBroj))
            {
                VezaForm f = new VezaForm();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    float veza = f.Veza;
                    if (vezba.kreirajVezuSaPrethodnim(redBroj, veza))
                    {
                        modified = true;
                        view.updateVezaColumn();
                        view.updateGridFooter();
                    }
                }
            }
        }

        public bool cmdPonistiVezuApplies(int redBroj)
        {
            if (vezba.isDeoVeze(redBroj))
                return true;
            else
                return false;
        }

        public bool cmdVezaSaPrethodnimApplies(int redBroj)
        {
            if (vezba.canCreateVezaSaPrethodnim(redBroj)
            && vezba.Elementi[redBroj - 1].VezaSaPrethodnim == null)
                return true;
            else
                return false;
        }

        public bool save()
        {
            try
            {
                if (!validateVezba())
                    return false;
                if (existsInDatabase)
                {
                    new VezbaDAO().update(vezba, original);
                }
                else
                {
                    new VezbaDAO().insert(vezba);
                    existsInDatabase = true;
                }
                modified = false;
                original = (Vezba)vezba.Clone(new TypeAsocijacijaPair[] { 
                        new TypeAsocijacijaPair(typeof(Vezba)), 
                        new TypeAsocijacijaPair(typeof(ElementVezbe)) });
                return true;
            }
            catch (DatabaseException)
            {
                string message;
                if (existsInDatabase)
                {
                    message = "Neuspesna promena vezbe u bazi.";
                    //message = string.Format(
                    //"Neuspesna promena vezbe u bazi. \n{0}", ex.InnerException.Message);
                }
                else
                {
                    message = "Neuspesan upis nove vezbe u bazu.";
                    //message = string.Format(
                    //"Neuspesan upis nove vezbe u bazu. \n{0}", ex.InnerException.Message);
                }
                MessageBox.Show(message, "Greska");
                return false;
            }
        }

        private bool validateVezba()
        {
            try
            {
                if (!vezba.validate())
                    return false;
                if (existsInDatabase)
                    DatabaseConstraintsValidator.checkUpdate(vezba, original);
                else
                    DatabaseConstraintsValidator.checkInsert(vezba);
                return true;
            }
            catch (InvalidPropertyException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                view.setFocus(ex.InvalidProperty);
                return false;
            }
            catch (DatabaseConstraintException ex)
            {
                MessageBox.Show(ex.ValidationErrors[0].Message, "Greska");
                view.setFocus(ex.ValidationErrors[0].InvalidProperties[0]);
                return false;
            }
            catch (DatabaseException)
            {
                string message;
                if (existsInDatabase)
                    // TODO: Ovde mozda trebaju drugacije poruke
                    message = "Neuspesna promena vezbe u bazi.";
                else
                    message = "Neuspesan upis nove vezbe u bazu.";
                MessageBox.Show(message, "Greska");
                return false;
            }
        }

        public bool brisiVezbu()
        {
            if (MessageBox.Show("Da li zelite da izbrisete vezbu?", "Potvrda",
                MessageBoxButtons.OKCancel, MessageBoxIcon.None,
                MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return false;
            }
            if (!existsInDatabase)
                return true;
            try
            {
                new VezbaDAO().delete(vezba);
                existsInDatabase = false;
                modified = false;
                return true;
            }
            catch (DatabaseException ex)
            {
                MessageBox.Show(ex.Message, "Greska");
                return false;
            }
        }

        public bool okToTrash()
        {
            if (!Modified)
                return true;

            // TODO: Navedi neku identifikaciju vezbe (npr. naziv, gimnasticar i sprava)
            DialogResult dr = MessageBox.Show("Vezba je promenjena. Da li zelite da " +
                "sacuvate izmene?", "Potvrda", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation);
            switch (dr)
            {
                case DialogResult.Yes:
                    return save();

                case DialogResult.No:
                    return true;

                case DialogResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}
