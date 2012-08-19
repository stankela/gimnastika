using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;
using System.Data.SqlServerCe;
using System.Data;

namespace Gimnastika.Domain
{
    public class Vezba : DomainObject
    {
        public static readonly int NAZIV_MAX_LENGTH = 128;

        private Sprava sprava;
        public virtual Sprava Sprava
        {
            get { return sprava; }
            set { sprava = value; }
        }

        private string naziv;
        public virtual string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        // TODO: Polja odbitak i penalizacija bi trebalo izbaciti
        private Nullable<float> odbitak;
        public virtual Nullable<float> Odbitak
        {
            get { return odbitak = getOdbitakUkupno(); }
            private set { odbitak = value; }
        }

        private Nullable<float> penalizacija;
        public virtual Nullable<float> Penalizacija
        {
            get { return penalizacija = getPenalizacijaUkupno(); }
            private set { penalizacija = value; }
        }

        private Gimnasticar gimnasticar;
        public virtual Gimnasticar Gimnasticar
        {
            get { return gimnasticar; }
            set { gimnasticar = value; }
        }

        private PraviloOceneVezbe pravilo;
        public virtual PraviloOceneVezbe Pravilo
        {
            get { return pravilo; }
            set { pravilo = value; }
        }

        private IList<ElementVezbe> elementi = new List<ElementVezbe>();
        public virtual IList<ElementVezbe> Elementi
        {
            get { return elementi; }
            private set { elementi = value; }
        }

        public virtual void DodajElement(ElementVezbe element)
        {
            if (element == null)
                return;
            Elementi.Add(element);
            renumberElements();
        }

        public virtual void DodajElement(int index, ElementVezbe element)
        {
            if (index < 0 || index > elementi.Count)
                return;
            if (element == null)
                return;
            Elementi.Insert(index, element);
            renumberElements();
        }

        public virtual void UkloniElement(ElementVezbe element)
        {
            if (element == null)
                return;
            if (Elementi.Remove(element))
            {
                renumberElements();
            }
        }

        public virtual void UkloniElemente()
        {
            Elementi.Clear();
        }

        private void renumberElements()
        {
            for (int i = 0; i < Elementi.Count; i++)
            {
                Elementi[i].RedBroj = (byte)(i + 1);
            }
        }


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

        public virtual void restore(Vezba original)
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

        public virtual float getVrednostUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in Elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Vrednost != null)
                    result += e.Vrednost.Value;
            }
            return result;
        }

        public virtual float getZahtevUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in Elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Zahtev != null)
                    result += e.Zahtev.Value;
            }
            return result;
        }

        public virtual float getVezaUkupno()
        {
            float result = 0;
            List<int> veze = getVeze();
            for (int i = 0; i < veze.Count / 2; i++)
            {
                int firstElement = veze[2 * i];
                int lastElement = veze[2 * i + 1];
                result += Elementi[lastElement].VezaSaPrethodnim.Value;
            }
            return result;
        }

        public virtual float getOdbitakUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in Elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Odbitak != null)
                    result += e.Odbitak.Value;
            }
            return result;
        }

        public virtual float getPenalizacijaUkupno()
        {
            float result = 0;
            foreach (ElementVezbe e in Elementi)
            {
                if (!e.BodujeSe)
                    continue;
                if (e.Penalizacija != null)
                    result += e.Penalizacija.Value;
            }
            return result;
        }

        public virtual float getOdbitakPenalizacija()
        {
            return getOdbitakUkupno() + getPenalizacijaUkupno();
        }

        public virtual float getPocetnaOcena()
        {
            return getVrednostUkupno() + getZahtevUkupno() + getVezaUkupno();
        }

        public virtual float getIzvedba()
        {
            return Pravilo.getPocetnaOcenaIzvedbe(getBrojBodujeSe());
        }

        public virtual int getBrojBodujeSe()
        {
            int result = 0;
            foreach (ElementVezbe e in Elementi)
            {
                if (e.BodujeSe)
                    result += 1;
            }
            return result;
        }

        public virtual float getOcena()
        {
            return getPocetnaOcena() + getIzvedba() - getOdbitakUkupno()
                - getPenalizacijaUkupno();
        }

        public virtual bool canCreateVezaSaPrethodnim(int redBroj)
        {
            if (Elementi.Count < 2)
                return false;
            if (redBroj < 2 || redBroj > Elementi.Count)
                return false;

            return Elementi[redBroj - 1].BodujeSe && Elementi[redBroj - 2].BodujeSe;
        }

        public virtual bool kreirajVezuSaPrethodnim(int redBroj, float value)
        {
            if (!canCreateVezaSaPrethodnim(redBroj))
                return false;
            if (Elementi[redBroj - 1].VezaSaPrethodnim != null)
                return false;
            if (value <= 0f)
                return false;

            int index = redBroj - 1;
            Elementi[index].VezaSaPrethodnim = value;

            // merge with previous
            int i = index - 1;
            while (i > 0 && Elementi[i].VezaSaPrethodnim != null)
                Elementi[i--].VezaSaPrethodnim = value;

            // merge with next
            i = index + 1;
            while (i < Elementi.Count && Elementi[i].VezaSaPrethodnim != null)
                Elementi[i++].VezaSaPrethodnim = value;

            return true;
        }

        public virtual bool raskiniVezu(int redBroj)
        {
            if (!isDeoVeze(redBroj))
                return false;
            else
            {
                int i = redBroj - 1;
                while (i > 0 && Elementi[i].VezaSaPrethodnim != null)
                    Elementi[i--].VezaSaPrethodnim = null;

                i = redBroj;
                while (i < Elementi.Count && Elementi[i].VezaSaPrethodnim != null)
                    Elementi[i++].VezaSaPrethodnim = null;

                return true;
            }
        }

        public virtual bool isDeoVeze(int redBroj)
        {
            if (redBroj < 1 || redBroj > Elementi.Count)
                return false;
            if (Elementi.Count < 2)
                return false;
            if (redBroj == 1)
                return Elementi[1].VezaSaPrethodnim != null;
            else
            {
                if (Elementi[redBroj - 1].VezaSaPrethodnim != null)
                    return true;
                else
                    return redBroj < Elementi.Count &&
                        Elementi[redBroj].VezaSaPrethodnim != null;
            }
        }

        public virtual List<int> getVeze()
        {
            List<int> result = new List<int>();
            int i = 0;
            while (i < Elementi.Count)
            {
                ElementVezbe e = Elementi[i];
                if (e.VezaSaPrethodnim != null)
                {
                    result.Add(i - 1);  // prvi element veze
                    while (i < Elementi.Count && Elementi[i].VezaSaPrethodnim != null)
                        i++;
                    result.Add(i - 1); // poslednji element veze
                }
                else
                    i++;
            }
            return result;
        }

        public virtual int getBrojBodujeSe(GrupaElementa grupaElementa)
        {
            int result = 0;
            foreach (ElementVezbe e in Elementi)
            {
                if (e.BodujeSe && e.Grupa == grupaElementa)
                    result += 1;
            }
            return result;
        }

        public virtual bool moveElementDown(byte redBroj)
        {
            if (redBroj < 1 || redBroj > Elementi.Count)
                return false;
            if (Elementi.Count < 2 || redBroj < 2)
                return false;

            ElementVezbe e = Elementi[redBroj - 1];
            Elementi.RemoveAt(redBroj - 1);
            Elementi.Insert(redBroj - 2, e);
            renumberElements();
            // TODO: Proveriti da li treba da se menja jos nesto u vezbi (osim sto su
            // prenumerisani redni brojevi)
            return true;
        }

        public virtual bool moveElementUp(byte redBroj)
        {
            if (redBroj < 1 || redBroj > Elementi.Count)
                return false;
            if (Elementi.Count < 2 || redBroj == Elementi.Count)
                return false;

            ElementVezbe e = Elementi[redBroj - 1];
            Elementi.RemoveAt(redBroj - 1);
            Elementi.Insert(redBroj, e);
            renumberElements();
            // TODO: Kao za moveElementUp
            return true;
        }

        public virtual bool canSelektujElement(byte redBroj)
        {
            ElementVezbe elem = Elementi[redBroj - 1];
            return elem.IsTablicniElement && getBrojBodujeSe() < Pravilo.BrojBodovanihElemenata
                && getBrojBodujeSe(elem.Grupa) < Pravilo.MaxIstaGrupa;
        }

        public virtual bool validate()
        {
            // TODO: Vidi zasto sam ovo stavio.
            return true;
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
