using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;

namespace Gimnastika.Domain
{
    public class PocetnaOcenaIzvedbe : DomainObject, IComparable
    {
        public static readonly int MAX_LIMIT = 999;

        private int minBrojElemenata;
        public virtual int MinBrojElemenata
        {
            get { return minBrojElemenata; }
            set { minBrojElemenata = value; }
        }

        private int maxBrojElemenata;
        public virtual int MaxBrojElemenata
        {
            get { return maxBrojElemenata; }
            set { maxBrojElemenata = value; }
        }

        private float pocetnaOcena;
        public virtual float PocetnaOcena
        {
            get { return pocetnaOcena; }
            set { pocetnaOcena = value; }
        }

        public PocetnaOcenaIzvedbe()
        {

        }

        public override void validate(Notification notification)
        {
            if (minBrojElemenata < 0)
            {
                notification.RegisterMessage(
                    "MinBrojElemenata", "Broj elemenata ne sme da bude manji od nula.");
            }
            if (maxBrojElemenata < 0)
            {
                notification.RegisterMessage(
                    "MaxBrojElemenata", "Broj elemenata ne sme da bude manji od nula.");
            }
            if (minBrojElemenata > maxBrojElemenata)
            {
                notification.RegisterMessage(
                    "MinBrojElemenata", "Donja granica opsega ne sma da " +
                    "bude veca od gornje granica opsega.");
            }
            if (pocetnaOcena < 0)
            {
                notification.RegisterMessage(
                    "PocetnaOcena", "Pocetna ocena ne sme da bude manja od nula.");
            }
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            PocetnaOcenaIzvedbe ocena = (PocetnaOcenaIzvedbe)domainObject;
            minBrojElemenata = ocena.minBrojElemenata;
            maxBrojElemenata = ocena.maxBrojElemenata;
            pocetnaOcena = ocena.pocetnaOcena;
        }

        public virtual string OpsegString
        {
            get
            {
                if (minBrojElemenata == maxBrojElemenata
                || maxBrojElemenata == MAX_LIMIT)
                    return minBrojElemenata.ToString();
                else
                    return minBrojElemenata + " - " + maxBrojElemenata;
            }
        }

        public virtual string OpsegOcenaString
        {
            get
            {
                string result = OpsegString.Replace(" ", "") + "=" + pocetnaOcena.ToString("F2");
                return result.Replace('.', ',');
            }
        }

        public virtual bool imaGornjuGranicu()
        {
            return maxBrojElemenata != MAX_LIMIT;
        }

        #region IComparable Members

        public virtual int CompareTo(object obj)
        {
            if (obj is PocetnaOcenaIzvedbe)
            {
                PocetnaOcenaIzvedbe other = (PocetnaOcenaIzvedbe)obj;
                return this.minBrojElemenata.CompareTo(other.minBrojElemenata);
            }
            throw new ArgumentException("object is not a PocetnaOcenaIzvedbe");
        }

        #endregion

        public virtual bool overlaps(PocetnaOcenaIzvedbe ocena)
        {
            return ocena.MinBrojElemenata >= this.minBrojElemenata
                && ocena.MinBrojElemenata <= this.maxBrojElemenata
            || ocena.MaxBrojElemenata >= this.minBrojElemenata
                && ocena.MaxBrojElemenata <= this.maxBrojElemenata;
        }

        // Ova dva metoda su neophodna samo kada asocijaciju od PraviloOceneVezbe ka PocetnaOcenaIzvedbe mapiram kao set.
        public override bool Equals(object other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (!(other is PocetnaOcenaIzvedbe)) return false;

            PocetnaOcenaIzvedbe that = (PocetnaOcenaIzvedbe)other;
            return this.MinBrojElemenata == that.MinBrojElemenata
                && this.MaxBrojElemenata == that.MaxBrojElemenata
                && this.PocetnaOcena == that.PocetnaOcena;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 14;
                result = 29 * result + MinBrojElemenata.GetHashCode();
                result = 29 * result + MaxBrojElemenata.GetHashCode();
                result = 29 * result + PocetnaOcena.GetHashCode();
                return result;
            }
        }
    }

    public class PraviloOceneVezbe : DomainObject
    {
        public static readonly int NAZIV_MAX_LENGTH = 64;

        private string naziv;
        public virtual string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        private int brojBodovanihElemenata;
        public virtual int BrojBodovanihElemenata
        {
            get { return brojBodovanihElemenata; }
            set { brojBodovanihElemenata = value; }
        }

        private int maxIstaGrupa;
        public virtual int MaxIstaGrupa
        {
            get { return maxIstaGrupa; }
            set { maxIstaGrupa = value; }
        }

        private IList<PocetnaOcenaIzvedbe> pocetneOceneIzvedbe = new List<PocetnaOcenaIzvedbe>();
        public virtual IList<PocetnaOcenaIzvedbe> PocetneOceneIzvedbe
        {
            get { return pocetneOceneIzvedbe; }
            set { pocetneOceneIzvedbe = value; }
        }


        public PraviloOceneVezbe()
        { 

        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            PraviloOceneVezbe pravilo = (PraviloOceneVezbe)domainObject;
            naziv = pravilo.naziv;
            brojBodovanihElemenata = pravilo.brojBodovanihElemenata;
            maxIstaGrupa = pravilo.maxIstaGrupa;
            if (shouldClone(new TypeAsocijacijaPair(typeof(PocetnaOcenaIzvedbe))))
            {
                foreach (PocetnaOcenaIzvedbe ocena in pravilo.pocetneOceneIzvedbe)
                {
                    pocetneOceneIzvedbe.Add((PocetnaOcenaIzvedbe)ocena.Clone());
                }
            }
            else
            {
                foreach (PocetnaOcenaIzvedbe ocena in pravilo.pocetneOceneIzvedbe)
                {
                    pocetneOceneIzvedbe.Add(ocena);
                }
            }
        }

        public virtual void restore(PraviloOceneVezbe original)
        {
            brojBodovanihElemenata = original.brojBodovanihElemenata;
            maxIstaGrupa = original.maxIstaGrupa;
            naziv = original.naziv;
            UkloniPocetneOceneIzvedbe();
            foreach (PocetnaOcenaIzvedbe ocena in original.pocetneOceneIzvedbe)
            {
                dodajPocetnuOcenuIzvedbe((PocetnaOcenaIzvedbe)ocena.Copy());
            }
        }

        public override string ToString()
        {
            return Naziv;
        }

        public virtual void dodajPocetnuOcenuIzvedbe(PocetnaOcenaIzvedbe ocena)
        {
            for (int i = 0; i < PocetneOceneIzvedbe.Count; i++)
            {
                if (PocetneOceneIzvedbe[i].overlaps(ocena))
                    throw new InvalidPropertyException("Opsezi za ocene ne smeju " +
                       "da se preklapaju.", "PocetneOceneIzvedbe");
            }
            PocetneOceneIzvedbe.Add(ocena);
            sortirajPocetneOceneIzvedbe();
        }

        public virtual void ukloniPocetnuOcenuIzvedbe(PocetnaOcenaIzvedbe ocena)
        {
            PocetneOceneIzvedbe.Remove(ocena);
        }

        private void UkloniPocetneOceneIzvedbe()
        {
            PocetneOceneIzvedbe.Clear();
        }

        public virtual float getPocetnaOcenaIzvedbe(int brojBodovanihElemenata)
        {
            for (int i = 0; i < PocetneOceneIzvedbe.Count; i++)
            {
                if (brojBodovanihElemenata >= PocetneOceneIzvedbe[i].MinBrojElemenata
                && brojBodovanihElemenata <= PocetneOceneIzvedbe[i].MaxBrojElemenata)
                    return PocetneOceneIzvedbe[i].PocetnaOcena;
            }
            return 0;
        }

        public virtual void sortirajPocetneOceneIzvedbe()
        {
            List<PocetnaOcenaIzvedbe> copy = new List<PocetnaOcenaIzvedbe>(PocetneOceneIzvedbe);
            copy.Sort();
            PocetneOceneIzvedbe.Clear();
            foreach (PocetnaOcenaIzvedbe o in copy)
            {
                PocetneOceneIzvedbe.Add(o);
            }
        }

        public override void validate(Notification notification)
        {
            // TODO: Sledeca dva if izraza su se nalazila u setterima za svojstva BrojBodovanihElemenata i MaxIstaGrupa
            // (tacnije nalazila su se u private metodima validateBrojBodovanihElemenata i validateMaxIstaGrupa klase
            // PraviloOceneVezbe a pozivi ovih metoda su se nalazili u setterim svojstava BrojBodovanihElemenata i 
            // MaxIstaGrupa)
            // Izbacio sam ih iz settera (da bi NHibernate radio korektno) i prebacio ovde.
            // Proveri da li je ovo dobro.
            if (BrojBodovanihElemenata < 1)
            {
                notification.RegisterMessage(
                    "BrojBodovanihElemenata", "Broj elemenata koji se boduju " +
                    "mora da bude veci od nula.");
            }
            if (MaxIstaGrupa < 1)
            {
                notification.RegisterMessage(
                    "MaxIstaGrupa", "Maksimalan broj elemenata iz iste " +
                    "grupe koji se boduju mora da bude veci od nula.");
            }
            
            foreach (PocetnaOcenaIzvedbe pocOcena in PocetneOceneIzvedbe)
            {
                pocOcena.validate(notification);
            }

            sortirajPocetneOceneIzvedbe();
            for (int i = 0; i < PocetneOceneIzvedbe.Count - 1; i++)
            {
                for (int j = i + 1; j < PocetneOceneIzvedbe.Count; j++)
                {
                    if (PocetneOceneIzvedbe[j].MinBrojElemenata <=
                        PocetneOceneIzvedbe[i].MaxBrojElemenata)
                    {
                        notification.RegisterMessage(
                            "PocetneOceneIzvedbe", "Opsezi za ocene ne smeju " +
                            "da se preklapaju.");
                    }
                }

            }
        }

    }
}