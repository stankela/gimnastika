using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;
using System.Windows.Forms;
using Gimnastika.Exceptions;
using Gimnastika.Dao;
using NHibernate;
using NHibernate.Context;
using Gimnastika.Data;
using Gimnastika.UI;

namespace Gimnastika
{
    public class VezbaEditorPresenter
    {
        private IVezbaEditorView view;
        private Nullable<int> vezbaId;
        private Vezba vezba;
        private bool modified;
        private bool existsInDatabase;

        Sprava oldSprava;
        string oldNaziv;
        Gimnasticar oldGimnasticar;

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
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
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
                        vezba = DAOFactoryFactory.DAOFactory.GetVezbaDAO().FindById(vezbaId.Value);
                        vezba.sortirajElementeByRedBroj();
                        saveOrigData(vezba);
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
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void saveOrigData(Vezba v)
        {
            oldSprava = v.Sprava;
            oldNaziv = v.Naziv;
            oldGimnasticar = v.Gimnasticar;
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
                if (vezba.moveElementDown(redBroj))
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
                if (vezba.moveElementUp(redBroj))
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
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    Notification notification = new Notification();
                    requiredFieldsAndFormatValidation(notification);
                    if (!notification.IsValid())
                        throw new BusinessException(notification);

                    if (existsInDatabase)
                    {
                        update();
                    }
                    else
                    {
                        add();
                        existsInDatabase = true;
                    }
                    session.Transaction.Commit();
                    
                    modified = false;
                    saveOrigData(vezba);
                    return true;

                }
            }
            catch (InvalidPropertyException ex)
            {
                MessageDialogs.showMessage(ex.Message, "Vezba");
                return false;
            }
            catch (BusinessException ex)
            {
                if (ex.Notification != null)
                {
                    NotificationMessage msg = ex.Notification.FirstMessage;
                    MessageDialogs.showMessage(msg.Message, "Vezba");
                }
                else
                {
                    MessageDialogs.showMessage(ex.Message, "Vezba");
                }
                return false;
            }
            catch (InfrastructureException ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(ex.Message, "Vezba");
                return false;
            }
            catch (Exception ex)
                {
                //discardChanges();
                MessageDialogs.showMessage(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), "Vezba");
                return false;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void requiredFieldsAndFormatValidation(Notification notification)
        {
            // validate Naziv
            if (vezba.Naziv.Length == 0)
            {
                notification.RegisterMessage(
                    "Naziv", "Naziv vezbe ne sme da bude prazan.");
            }
            if (vezba.Naziv.Length > Vezba.NAZIV_MAX_LENGTH)
            {
                notification.RegisterMessage(
                    "Naziv", "Naziv vezbe moze da sadrzi maksimalno "
                    + Vezba.NAZIV_MAX_LENGTH + " znakova.");
            }
        }

        private void add()
        {
            //updateEntityFromUI(entity);
            validateEntity(vezba);
            checkBusinessRulesOnAdd(vezba);
            //if (persistEntity)
                insertEntity(vezba);
        }

        private void update()
        {
            //updateEntityFromUI(entity);
            validateEntity(vezba);
            checkBusinessRulesOnUpdate(vezba);
            //if (persistEntity)
                updateEntity(vezba);
        }

        private void validateEntity(DomainObject entity)
        {
            Notification notification = new Notification();
            entity.validate(notification);
            if (!notification.IsValid())
                throw new BusinessException(notification);
        }

        // TODO: Izgleda da ne radi otvaranje menija desnim klikom misem (dodavanje elementa) kada je vezba prazna
        
        private void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Vezba v = (Vezba)entity;
            Notification notification = new Notification();

            VezbaDAO vezbaDAO = DAOFactoryFactory.DAOFactory.GetVezbaDAO();
            if (vezbaDAO.postojiVezba(v.Sprava, v.Naziv, v.Gimnasticar))
            {
                notification.RegisterMessage("Naziv", "Vezba sa datim nazivom i za datu spravu vec postoji " +
                    "za datog gimnasticara.");
                throw new BusinessException(notification);
            }
        }

        private void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Vezba v = (Vezba)entity;
            Notification notification = new Notification();

            VezbaDAO vezbaDAO = DAOFactoryFactory.DAOFactory.GetVezbaDAO();
            bool spravaChanged = (v.Sprava != oldSprava) ? true : false;
            bool nazivChanged = (v.Naziv != oldNaziv) ? true : false;
            bool gimnasticarChanged = v.Gimnasticar == null && oldGimnasticar != null
                || v.Gimnasticar != null && oldGimnasticar == null
                || v.Gimnasticar != null && oldGimnasticar != null
                    && v.Gimnasticar.Id != oldGimnasticar.Id;
            if (nazivChanged || spravaChanged || gimnasticarChanged)
            {
                if (vezbaDAO.postojiVezba(v.Sprava, v.Naziv, v.Gimnasticar))
                {
                    notification.RegisterMessage("Naziv", "Vezba sa datim nazivom i za datu spravu vec postoji " +
                        "za datog gimnasticara.");
                    throw new BusinessException(notification);
                }
            }
        }

        private void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetVezbaDAO().MakePersistent((Vezba)entity);
        }

        private void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetVezbaDAO().MakePersistent((Vezba)entity);
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
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    DAOFactoryFactory.DAOFactory.GetVezbaDAO().MakeTransient(vezba);
                    existsInDatabase = false;
                    modified = false;
                    session.Transaction.Commit();
                    return true;
                }
            }
            catch (InvalidPropertyException ex)
            {
                MessageDialogs.showMessage(ex.Message, "Vezba");
                return false;
            }
            catch (BusinessException ex)
            {
                if (ex.Notification != null)
                {
                    NotificationMessage msg = ex.Notification.FirstMessage;
                    MessageDialogs.showMessage(msg.Message, "Vezba");
                }
                else
                {
                    MessageDialogs.showMessage(ex.Message, "Vezba");
                }
                return false;
            }
            catch (InfrastructureException ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(ex.Message, "Vezba");
                return false;
            }
            catch (Exception ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), "Vezba");
                return false;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
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
