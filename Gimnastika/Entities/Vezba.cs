using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;
using System.Data.SqlServerCe;
using System.Data;

namespace Gimnastika.Entities
{
    public class Vezba : DomainObject
    {
        private Sprava sprava;
        private string naziv;
        private Gimnasticar gimnasticar;
        private List<ElementVezbe> elementi = new List<ElementVezbe>();
        private Nullable<float> odbitak;
        private Nullable<float> penalizacija;
        private PraviloOceneVezbe pravilo;

        public static readonly int NAZIV_MAX_LENGTH = 128;

        public Vezba()
        {

        }

        public Vezba(Sprava sprava, string naziv, Nullable<float> odbitak,
            Nullable<float> penalizacija)
        {
            this.sprava = sprava;
            this.naziv = naziv;
            this.odbitak = odbitak;
            this.penalizacija = penalizacija;
            this.gimnasticar = null;
            this.pravilo = null;
        }

        public Vezba(Sprava sprava, string naziv, Gimnasticar gimnasticar,
            Nullable<float> odbitak, Nullable<float> penalizacija, PraviloOceneVezbe pravilo)
        {
            this.sprava = sprava;
            this.naziv = naziv;
            this.gimnasticar = gimnasticar;
            this.odbitak = odbitak;
            this.penalizacija = penalizacija;
            this.pravilo = pravilo;
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            Vezba vezba = (Vezba)domainObject;
            naziv = vezba.naziv;
            sprava = vezba.sprava;
            odbitak = vezba.odbitak;
            penalizacija = vezba.penalizacija;
            gimnasticar = vezba.gimnasticar;
            if (gimnasticar != null && shouldClone(new TypeAsocijacijaPair(typeof(Gimnasticar))))
                gimnasticar = (Gimnasticar)gimnasticar.Clone();
            pravilo = vezba.pravilo;
            if (pravilo != null && shouldClone(new TypeAsocijacijaPair(typeof(PraviloOceneVezbe))))
                pravilo = (PraviloOceneVezbe)pravilo.Clone();
            if (shouldClone(new TypeAsocijacijaPair(typeof(ElementVezbe))))
            {
                foreach (ElementVezbe ev in vezba.elementi)
                    elementi.Add((ElementVezbe)ev.Clone());
            }
            else
            {
                foreach (ElementVezbe ev in vezba.elementi)
                    elementi.Add(ev);
            }
        }

        public void restore(Vezba original)
        {
            // TODO: Kod restore operacija bi trebalo kao parametar da se prosledjuju
            // i tipovi, da se zna koliko duboko treba vrsiti kloniranje
            sprava = original.sprava;
            naziv = original.naziv;
            odbitak = original.odbitak;
            penalizacija = original.penalizacija;
            gimnasticar = original.gimnasticar;
            pravilo = original.pravilo;
            UkloniElemente();
            foreach (ElementVezbe e in original.elementi)
            {
                DodajElement((ElementVezbe)e.Copy());
            }
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public Sprava Sprava
        {
            get { return sprava; }
            set { sprava = value; }
        }

        public Gimnasticar Gimnasticar
        {
            get { return gimnasticar; }
            set { gimnasticar = value; }
        }

        public PraviloOceneVezbe Pravilo
        {
            get { return pravilo; }
            set { pravilo = value; }
        }

        public Nullable<float> Odbitak
        {
            // TODO: Polja odbitak i penalizacija bi trebalo izbaciti
            get { return odbitak = getOdbitakUkupno(); }
            //set { odbitak = value; }
        }

        public Nullable<float> Penalizacija
        {
            get { return penalizacija = getPenalizacijaUkupno(); }
            //set { penalizacija = value; }
        }

        public IList<ElementVezbe> Elementi
        {
            get { return elementi.AsReadOnly(); }
        }

        public void DodajElement(ElementVezbe element)
        {
            if (element == null)
                return;
            azurirajVezuPremaVezbi(element);
            elementi.Add(element);  // uspostavlja vezu prema elementu
            renumberElements();
        }

        private void azurirajVezuPremaVezbi(ElementVezbe element)
        {
            // kada je potrebno da se najpre raskine postojeca asocijacija elementa
            // i vezbe
            if (element.Vezba != null)
                element.Vezba.UkloniElement(element);

            element.setVezba(this);
        }

        public void DodajElement(int index, ElementVezbe element)
        {
            if (index < 0 || index > elementi.Count)
                return;
            if (element == null)
                return;
            azurirajVezuPremaVezbi(element);
            elementi.Insert(index, element);
            renumberElements();
        }

        public void UkloniElement(ElementVezbe element)
        {
            if (element == null)
                return;
            if (elementi.Remove(element))
            {
                element.setVezba(null);
                renumberElements();
            }
        }

        public void UkloniElemente()
        {
            foreach (ElementVezbe e in elementi)
            {
                e.setVezba(null);
            }
            elementi.Clear();
        }

        private void renumberElements()
        {
            for (int i = 0; i < elementi.Count; i++)
            {
                elementi[i].RedBroj = (byte)(i + 1);
            }
        }

        public float getVrednostUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Vrednost != null)
                    result += e.Vrednost.Value;
            }
            return result;
        }

        public float getZahtevUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Zahtev != null)
                    result += e.Zahtev.Value;
            }
            return result;
        }

        public float getVezaUkupno()
        {
            float result = 0;
            List<int> veze = getVeze();
            for (int i = 0; i < veze.Count / 2; i++)
            {
                int firstElement = veze[2 * i];
                int lastElement = veze[2 * i + 1];
                result += elementi[lastElement].VezaSaPrethodnim.Value;
            }
            return result;
        }

        public float getOdbitakUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Odbitak != null)
                    result += e.Odbitak.Value;
            }
            return result;
        }

        public float getPenalizacijaUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Penalizacija != null)
                    result += e.Penalizacija.Value;
            }
            return result;
        }

        public float getOdbitakPenalizacija()
        {
            return getOdbitakUkupno() + getPenalizacijaUkupno();
        }

        public float getPocetnaOcena()
        {
            return getVrednostUkupno() + getZahtevUkupno() + getVezaUkupno();
        }

        public float getIzvedba()
        {
            return pravilo.getPocetnaOcenaIzvedbe(getBrojBodujeSe());
        }

        public int getBrojBodujeSe()
        {
            int result = 0;
            foreach (ElementVezbe e in elementi)
            {
                if (e.BodujeSe)
                    result += 1;
            }
            return result;
        }

        public float getOcena()
        {
            return getPocetnaOcena() + getIzvedba() - getOdbitakUkupno()
                - getPenalizacijaUkupno();
        }

        public bool canCreateVezaSaPrethodnim(int redBroj)
        {
            if (elementi.Count < 2)
                return false;
            if (redBroj < 2 || redBroj > elementi.Count)
                return false;

            return elementi[redBroj - 1].BodujeSe && elementi[redBroj - 2].BodujeSe;
        }

        public bool kreirajVezuSaPrethodnim(int redBroj, float value)
        {
            if (!canCreateVezaSaPrethodnim(redBroj))
                return false;
            if (elementi[redBroj - 1].VezaSaPrethodnim != null)
                return false;
            if (value <= 0f)
                return false;

            int index = redBroj - 1;
            elementi[index].VezaSaPrethodnim = value;

            // merge with previous
            int i = index - 1;
            while (i > 0 && elementi[i].VezaSaPrethodnim != null)
                elementi[i--].VezaSaPrethodnim = value;

            // merge with next
            i = index + 1;
            while (i < elementi.Count && elementi[i].VezaSaPrethodnim != null)
                elementi[i++].VezaSaPrethodnim = value;

            return true;
        }

        public bool raskiniVezu(int redBroj)
        {
            if (!isDeoVeze(redBroj))
                return false;
            else
            {
                int i = redBroj - 1;
                while (i > 0 && elementi[i].VezaSaPrethodnim != null)
                    elementi[i--].VezaSaPrethodnim = null;

                i = redBroj;
                while (i < elementi.Count && elementi[i].VezaSaPrethodnim != null)
                    elementi[i++].VezaSaPrethodnim = null;

                return true;
            }
        }

        public bool isDeoVeze(int redBroj)
        {
            if (redBroj < 1 || redBroj > elementi.Count)
                return false;
            if (elementi.Count < 2)
                return false;
            if (redBroj == 1)
                return elementi[1].VezaSaPrethodnim != null;
            else
            {
                if (elementi[redBroj - 1].VezaSaPrethodnim != null)
                    return true;
                else
                    return redBroj < elementi.Count &&
                        elementi[redBroj].VezaSaPrethodnim != null;
            }
        }

        public List<int> getVeze()
        {
            List<int> result = new List<int>();
            int i = 0;
            while (i < elementi.Count)
            {
                ElementVezbe e = elementi[i];
                if (e.VezaSaPrethodnim != null)
                {
                    result.Add(i - 1);  // prvi element veze
                    while (i < elementi.Count && elementi[i].VezaSaPrethodnim != null)
                        i++;
                    result.Add(i - 1); // poslednji element veze
                }
                else
                    i++;
            }
            return result;
        }

        public int getBrojBodujeSe(GrupaElementa grupaElementa)
        {
            int result = 0;
            foreach (ElementVezbe e in elementi)
            {
                if (e.BodujeSe && e.Grupa == grupaElementa)
                    result += 1;
            }
            return result;
        }

        public bool moveElementUp(byte redBroj)
        {
            if (redBroj < 1 || redBroj > elementi.Count)
                return false;
            if (elementi.Count < 2 || redBroj < 2)
                return false;

            ElementVezbe e = elementi[redBroj - 2];
            elementi[redBroj - 2] = elementi[redBroj - 1];
            elementi[redBroj - 1] = e;
            renumberElements();
            // TODO: Proveriti da li treba da se menja jos nesto u vezbi (osim sto su
            // prenumerisani redni brojevi)
            return true;
        }

        public bool moveElementDown(byte redBroj)
        {
            if (redBroj < 1 || redBroj > elementi.Count)
                return false;
            if (elementi.Count < 2 || redBroj == elementi.Count)
                return false;

            ElementVezbe e = elementi[redBroj - 1];
            elementi[redBroj - 1] = elementi[redBroj];
            elementi[redBroj] = e;
            renumberElements();
            // TODO: Kao za moveElementUp
            return true;
        }

        public bool canSelektujElement(byte redBroj)
        {
            ElementVezbe elem = elementi[redBroj - 1];
            return elem.IsTablicniElement && getBrojBodujeSe() < pravilo.BrojBodovanihElemenata
                && getBrojBodujeSe(elem.Grupa) < pravilo.MaxIstaGrupa;
        }

        public bool validate()
        {
            return true;
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
