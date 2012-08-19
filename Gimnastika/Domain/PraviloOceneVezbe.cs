using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;

namespace Gimnastika.Domain
{
    public class PocetnaOcenaIzvedbe : DomainObject, IComparable
    {
        private int minBrojElemenata;
        private int maxBrojElemenata;
        private float pocetnaOcena;

        private readonly int MAX_LIMIT = int.MaxValue;

        public PocetnaOcenaIzvedbe()
        {

        }

        public PocetnaOcenaIzvedbe(int minBrojElemenata, int maxBrojElemenata,
            float pocetnaOcena)
        {
            if (validate(minBrojElemenata, maxBrojElemenata, pocetnaOcena))
            {
                this.minBrojElemenata = minBrojElemenata;
                this.maxBrojElemenata = maxBrojElemenata;
                this.pocetnaOcena = pocetnaOcena;
            }
        }

        public PocetnaOcenaIzvedbe(int minBrojElemenata, float pocetnaOcena)
        {
            if (validate(minBrojElemenata, MAX_LIMIT, pocetnaOcena))
            {
                this.minBrojElemenata = minBrojElemenata;
                this.maxBrojElemenata = MAX_LIMIT;
                this.pocetnaOcena = pocetnaOcena;
            }
        }

        private bool validate(int minBrojElemenata, int maxBrojElemenata,
            float pocetnaOcena)
        {
            if (minBrojElemenata < 0)
                throw new InvalidPropertyException("Broj elemenata ne sme da bude " +
                    "manji od nula.", "MinBrojElemenata");
            if (maxBrojElemenata < 0)
                throw new InvalidPropertyException("Broj elemenata ne sme da bude " +
                    "manji od nula.", "MaxBrojElemenata");
            if (minBrojElemenata > maxBrojElemenata)
                throw new InvalidPropertyException("Donja granica opsega ne sma da " +
                    "bude veca od gornje granica opsega.", "MinBrojElemenata");
            if (pocetnaOcena < 0)
                throw new InvalidPropertyException("Pocetna ocena ne sme da bude " +
                    "manja od nula", "PocetnaOcena");
            return true;
        }

        public bool validate()
        {
            return validate(minBrojElemenata, maxBrojElemenata, pocetnaOcena);
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            PocetnaOcenaIzvedbe ocena = (PocetnaOcenaIzvedbe)domainObject;
            minBrojElemenata = ocena.minBrojElemenata;
            maxBrojElemenata = ocena.maxBrojElemenata;
            pocetnaOcena = ocena.pocetnaOcena;
        }

        public int MinBrojElemenata
        {
            get { return minBrojElemenata; }
            set { minBrojElemenata = value; }
        }

        public int MaxBrojElemenata
        {
            get { return maxBrojElemenata; }
            set { maxBrojElemenata = value; }
        }

        public float PocetnaOcena
        {
            get { return pocetnaOcena; }
            set { pocetnaOcena = value; }
        }

        public string OpsegString
        {
            get
            {
                if (minBrojElemenata == maxBrojElemenata
                || maxBrojElemenata == int.MaxValue)
                    return minBrojElemenata.ToString();
                else
                    return minBrojElemenata + " - " + maxBrojElemenata;
            }
        }

        public string OpsegOcenaString
        {
            get
            {
                string result = OpsegString.Replace(" ", "") + "=" + pocetnaOcena.ToString("F2");
                return result.Replace('.', ',');
            }
        }

        public bool imaGornjuGranicu()
        {
            return maxBrojElemenata != MAX_LIMIT;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is PocetnaOcenaIzvedbe)
            {
                PocetnaOcenaIzvedbe other = (PocetnaOcenaIzvedbe)obj;
                return this.minBrojElemenata.CompareTo(other.minBrojElemenata);
            }
            throw new ArgumentException("object is not a PocetnaOcenaIzvedbe");
        }

        #endregion

        public bool overlaps(PocetnaOcenaIzvedbe ocena)
        {
            return ocena.MinBrojElemenata >= this.minBrojElemenata
                && ocena.MinBrojElemenata <= this.maxBrojElemenata
            || ocena.MaxBrojElemenata >= this.minBrojElemenata
                && ocena.MaxBrojElemenata <= this.maxBrojElemenata;
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
            set
            {
                if (validateBrojBodovanihElemenata(value))
                    brojBodovanihElemenata = value;
            }
        }

        private int maxIstaGrupa;
        public virtual int MaxIstaGrupa
        {
            get { return maxIstaGrupa; }
            set
            {
                if (validateMaxIstaGrupa(value))
                    maxIstaGrupa = value;
            }
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

        public PraviloOceneVezbe(string naziv, int brojBodovanihElemenata, int maxIstaGrupa)
        {
            this.naziv = naziv;
            if (validateBrojBodovanihElemenata(brojBodovanihElemenata))
                this.brojBodovanihElemenata = brojBodovanihElemenata;    
            if (validateMaxIstaGrupa(maxIstaGrupa))
                this.maxIstaGrupa = maxIstaGrupa;    
        }

        private bool validateBrojBodovanihElemenata(int brojBodovanihElemenata)
        {
            if (brojBodovanihElemenata < 1)
                throw new InvalidPropertyException("Broj elemenata koji se boduju " +
                    "mora da bude veci od nula.", "BrojBodovanihElemenata");
            return true;
        }

        private bool validateMaxIstaGrupa(int maxIstaGrupa)
        {
            if (maxIstaGrupa < 1)
                throw new InvalidPropertyException("Maksimalan broj elemenata iz iste " + 
                    "grupe koji se boduju mora da bude veci od nula.", "MaxIstaGrupa");
            return true;
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
            return naziv;
        }

        public virtual void dodajPocetnuOcenuIzvedbe(PocetnaOcenaIzvedbe ocena)
        {
            for (int i = 0; i < pocetneOceneIzvedbe.Count; i++)
            {
                if (pocetneOceneIzvedbe[i].overlaps(ocena))
                    throw new InvalidPropertyException("Opsezi za ocene ne smeju " +
                       "da se preklapaju.", "PocetneOceneIzvedbe");
            }
            pocetneOceneIzvedbe.Add(ocena);
            sortirajPocetneOceneIzvedbe();
        }

        public virtual void ukloniPocetnuOcenuIzvedbe(PocetnaOcenaIzvedbe ocena)
        {
            pocetneOceneIzvedbe.Remove(ocena);
        }

        private void UkloniPocetneOceneIzvedbe()
        {
            pocetneOceneIzvedbe.Clear();
        }

        public virtual float getPocetnaOcenaIzvedbe(int brojBodovanihElemenata)
        {
            for (int i = 0; i < pocetneOceneIzvedbe.Count; i++)
            {
                if (brojBodovanihElemenata >= pocetneOceneIzvedbe[i].MinBrojElemenata
                && brojBodovanihElemenata <= pocetneOceneIzvedbe[i].MaxBrojElemenata)
                    return pocetneOceneIzvedbe[i].PocetnaOcena;
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

        public virtual bool validate()
        {
            if (!validateBrojBodovanihElemenata(brojBodovanihElemenata))
                return false;
            if (!validateMaxIstaGrupa(maxIstaGrupa))
                return false;

            foreach (PocetnaOcenaIzvedbe pocOcena in pocetneOceneIzvedbe)
            {
                if (!pocOcena.validate())
                    return false;
            }

            sortirajPocetneOceneIzvedbe();
            for (int i = 0; i < pocetneOceneIzvedbe.Count - 1; i++)
            {
                for (int j = i + 1; j < pocetneOceneIzvedbe.Count; j++)
                {
                    if (pocetneOceneIzvedbe[j].MinBrojElemenata <=
                        pocetneOceneIzvedbe[i].MaxBrojElemenata)
                        throw new InvalidPropertyException("Opsezi za ocene ne smeju " +
                            "da se preklapaju.", "PocetneOceneIzvedbe");
                }

            }
            return true;
        }
    }
}