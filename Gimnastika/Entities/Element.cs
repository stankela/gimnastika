using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;
using System.Text.RegularExpressions;
using System.Data.SqlServerCe;
using System.Data;
using System.ComponentModel;
using Gimnastika.Dao;

namespace Gimnastika.Entities
{
    public enum Sprava
    { 
        Undefined = 0,
        Parter = 1,
        Konj = 2,
        Karike = 3,
        Preskok = 4,
        Razboj = 5,
        Vratilo = 6
    }

    public enum TezinaElementa
    {
        Undefined = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6,
        G = 7
    }

    public enum GrupaElementa
    {
        Undefined = 0,
        I = 1,
        II = 2,
        III = 3,
        IV = 4,
        V = 5
    }

    public class Element : DomainObject
    {
        private string naziv;
        private string engleskiNaziv;
        private string nazivPoGimnasticaru;
        private Sprava sprava;
        private bool isTablicniElement;
        private GrupaElementa grupa;
        private TezinaElementa tezina;
        private short broj;
        private byte podBroj;
        private GrupaBrojClass grupaBroj;
        private List<Video> videoKlipovi = new List<Video>();
        private List<Slika> slike = new List<Slika>();
        private List<Element> varijante = null; // lazy load
        private Element parent; // lazy load

        private Nullable<int> parentId = null; // for lazy load
        public Nullable<int> ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public static readonly int NAZIV_MAX_LENGTH = 128;
        public static readonly int NAZIV_GIM_MAX_LENGTH = 64;
        private static readonly char brojDelimiter = ',';
        private static readonly string rgxBrojPodBroj = @"^\s*\d{1,4}\s*(,\s*\d{1,2}\s*)?$";

        public static readonly float[] VrednostTezine = new float[8] {
            0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f
        };

        public Element()
        {
            grupaBroj = new GrupaBrojClass(this);
        }

        public Element(string naziv, string engleskiNaziv, string nazivPoGimnasticaru,
            Sprava sprava)
        {
            this.naziv = naziv;
            this.engleskiNaziv = engleskiNaziv;
            this.nazivPoGimnasticaru = nazivPoGimnasticaru;
            this.sprava = sprava;
            this.isTablicniElement = false;
            this.grupa = GrupaElementa.Undefined;
            this.tezina = TezinaElementa.Undefined;
            this.broj = 0;
            this.podBroj = 0;
            this.grupaBroj = new GrupaBrojClass(this);
            this.parent = null;
        }

        public Element(string naziv, string engleskiNaziv, string nazivPoGimnasticaru,
            Sprava sprava, GrupaElementa grupa, TezinaElementa tezina, short broj,
            byte podBroj)
        {
            this.naziv = naziv;
            this.engleskiNaziv = engleskiNaziv;
            this.nazivPoGimnasticaru = nazivPoGimnasticaru;
            this.sprava = sprava;
            this.isTablicniElement = true;
            this.grupa = grupa;
            this.tezina = tezina;
            this.broj = broj;
            this.podBroj = podBroj;
            this.grupaBroj = new GrupaBrojClass(this);
            this.parent = null;
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            Element element = (Element)domainObject;
            naziv = element.naziv;
            engleskiNaziv = element.engleskiNaziv;
            nazivPoGimnasticaru = element.nazivPoGimnasticaru;
            sprava = element.sprava;
            isTablicniElement = element.isTablicniElement;
            grupa = element.grupa;
            tezina = element.tezina;
            broj = element.broj;
            podBroj = element.podBroj;
            grupaBroj = element.grupaBroj;
            parentId = element.parentId;
            if (shouldClone(new TypeAsocijacijaPair(typeof(Video))))
            {
                foreach (Video v in element.VideoKlipovi)
                    videoKlipovi.Add((Video)v.Clone());
            }
            else
            {
                foreach (Video v in element.VideoKlipovi)
                    videoKlipovi.Add(v);
            }
            if (shouldClone(new TypeAsocijacijaPair(typeof(Slika))))
            {
                foreach (Slika s in element.Slike)
                    slike.Add((Slika)s.Clone());
            }
            else
            {
                foreach (Slika s in element.Slike)
                    slike.Add(s);
            }
            // TODO: Ovo je error prone, zato sto ako kasnije promenim (refaktorisem)
            // ime promenljive 'varijante', kod nece raditi. Vidi da li moze nekako
            // drugacije

            varijante = new List<Element>();
            if (shouldClone(new TypeAsocijacijaPair(typeof(Element), "varijante")))
            {
                foreach (Element e in element.Varijante)
                    varijante.Add((Element)e.Clone());
            }
            else
            {
                foreach (Element e in element.Varijante)
                    varijante.Add(e);
            }                
            parent = element.parent;
            if (parent != null && shouldClone(new TypeAsocijacijaPair(
                typeof(Element), "parent")))
            {
                parent = (Element)element.Clone();
            }
        }

        public void restore(Element original)
        {
            naziv = original.naziv;
            engleskiNaziv = original.engleskiNaziv;
            nazivPoGimnasticaru = original.nazivPoGimnasticaru;
            sprava = original.sprava;
            isTablicniElement = original.isTablicniElement;
            grupa = original.grupa;
            tezina = original.tezina;
            broj = original.broj;
            podBroj = original.podBroj;
            grupaBroj = original.grupaBroj;
            parentId = original.parentId;
            parent = original.parent;

            videoKlipovi.Clear();
            foreach (Video v in original.VideoKlipovi)
                dodajVideo((Video)v.Copy());

            slike.Clear();
            foreach (Slika s in original.Slike)
                dodajSliku((Slika)s.Copy());

            varijante.Clear();
            foreach (Element e in original.Varijante)
                dodajVarijantu((Element)e.Copy());
        }

        public override string ToString()
        {
            string nazivEl = NazivString;
            string result;
            result = String.Format("{0}, {1}", nazivEl, sprava);
            if (IsTablicniElement)
            {
                result += String.Format(", {0}-{1}", grupa, broj);
                if (podBroj != 0)
                {
                    result += String.Format(",{0}", podBroj);
                }
                result += String.Format(" {0}", tezina);
            }
            return result;
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value.Trim(); }
        }

        public string EngleskiNaziv
        {
            get { return engleskiNaziv; }
            set { engleskiNaziv = value.Trim(); }
        }

        public string NazivPoGimnasticaru
        {
            get { return nazivPoGimnasticaru; }
            set { nazivPoGimnasticaru = value.Trim(); }
        }

        public string NazivString
        {
            get
            {
                string result = this.naziv;
                if (result == "")
                    result = this.nazivPoGimnasticaru;
                if (result == "")
                    result = this.engleskiNaziv;
                return result;
            }
        }
        
        public Sprava Sprava
        {
            get { return sprava; }
            set 
            {
                sprava = value;
                foreach (Element e in Varijante)
                    e.Sprava = value;
            }
        }

        public bool IsTablicniElement
        {
            get { return isTablicniElement; }
        }

        public GrupaElementa Grupa
        {
            get { return grupa; }
            set 
            { 
                grupa = value;
                foreach (Element e in Varijante)
                    e.Grupa = value;
            }
        }

        public TezinaElementa Tezina
        {
            get { return tezina; }
            set 
            { 
                tezina = value;
                foreach (Element e in Varijante)
                    e.Tezina = value;
            }
        }

        public short Broj
        {
            get { return broj; }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidPropertyException("Broj mora da bude veci od 0.", "Broj");
                }
                broj = value;
                foreach (Element e in Varijante)
                    e.Broj = value;
            }
        }

        public byte PodBroj
        {
            get { return podBroj; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidPropertyException("Podbroj ne sme da bude negativan.", "PodBroj");
                }
                podBroj = value;
            }
        }

        public static bool isValidBrojPodBroj(string brojPodBroj)
        {
            return Regex.IsMatch(brojPodBroj, rgxBrojPodBroj);
        }

        public static string formatBrojPodBroj(int broj, int podBroj)
        {
            string result = broj.ToString();
            if (podBroj > 0)
            {
                result += brojDelimiter + " " + podBroj.ToString();
            }
            return result;
        }

        public string BrojPodBroj
        {
            get 
            {
                if (!IsTablicniElement)
                    return String.Empty;
                else
                    return formatBrojPodBroj(broj, podBroj);
            }
            set
            {
                if (isValidBrojPodBroj(value))
                {
                    if (value.IndexOf(brojDelimiter) == -1)
                    {
                        broj = Int16.Parse(value);
                        podBroj = 0;
                    }
                    else
                    {
                        string[] s = value.Split(brojDelimiter);
                        broj = Int16.Parse(s[0]);
                        podBroj = Byte.Parse(s[1]);
                    }
                    foreach (Element e in Varijante)
                        e.Broj = broj;
                }
                else
                {
                    throw new InvalidPropertyException(
                        "Broj elementa nije u zahtevanom formatu. Ukoliko je u pitanju " +
                        "varijanta elementa, broj varijante treba da je razdvojen zarezom (npr. 7,1).", "Broj");
                }
            }
        }

        public GrupaBrojClass GrupaBroj
        {
            get { return grupaBroj; }
        }

        public Nullable<float> Vrednost
        {
            get
            {
                if (!IsTablicniElement)
                    return null;
                else
                    return VrednostTezine[(int)tezina];
            }
        }

        public void setPolozajUTablici(GrupaElementa grupa, TezinaElementa tezina,
            string broj)
        {
            this.Grupa = grupa;
            this.Tezina = tezina;
            BrojPodBroj = broj;
            isTablicniElement = true;

            // TODO: Ispitati zasto nisam postavio nove vrednosti i za varijante (i da
            // li ih treba postaviti)
        }

        public void ponistiPolozajUTablici()
        {
            this.Grupa = GrupaElementa.Undefined;
            this.Tezina = TezinaElementa.Undefined;
            this.broj = 0;
            this.podBroj = 0;
            isTablicniElement = false;

            foreach (Element e in Varijante)
                e.ponistiPolozajUTablici();
        }

        public IList<Video> VideoKlipovi
        {
            get { return videoKlipovi.AsReadOnly(); }
        }

        public void dodajVideo(Video video)
        {
            if (video != null)
                videoKlipovi.Add(video);
        }

        public void ukloniVideo(Video video)
        {
            videoKlipovi.Remove(video);
        }

        public void ukloniVideoKlipove()
        {
            videoKlipovi.Clear();
        }

        public IList<Slika> Slike
        {
            get { return slike.AsReadOnly(); }
        }

        public void dodajSliku(Slika slika)
        {
            if (slika != null)
                slike.Add(slika);
        }

        public void ukloniSliku(Slika slika)
        {
            slike.Remove(slika);
        }

        public void ukloniSlike()
        {
            slike.Clear();
        }

        public List<Element> Varijante
        {
            get 
            {
                if (varijante == null)
                {
                    varijante = new List<Element>();
                    if (Id != 0) // element je ucitan iz baze
                    {
                        foreach (Element e in new ElementDAO().getVarijante(this.Id))
                            dodajVarijantu(e); // mora ovako da bi se pravilno
                                                // uspostavile veze
                    }
                }
                return varijante; 
            }
        }

        public void dodajVarijantu(Element varijanta)
        {
            if (varijanta != null)
                varijanta.Parent = this;
        }

        public void ukloniVarijantu(Element varijanta)
        {
            if (varijanta != null && varijanta.Parent == this)
                varijanta.Parent = null;
        }

        public Element Parent
        {
            get 
            {
                if (parent == null && parentId != null)
                {
                    parent = new ElementDAO().getById(parentId.Value);
                    parentId = null; // da ga ne bi ponovo ucitavao ako je u
                                     // medjuvremenu nuliram
                }
                return parent; 
            }
            set 
            {
                // Zbog lazy loada, u uslovu prve if naredbe bi umesto polja parent
                // trebalo da bude svojsvo Parent (da inicijalizuje polje parent), ali
                // to dovodi do beskonacne petlje. U ovom slucaju medjutim, moze i bez
                // toga - prva if naredba sluzi da se raskine veza sa postojecim
                // parentom, ali ako postoji veza sa parentom tada polje parent nije
                // null pa ne treba da se inicijalizuje sa lazy loadom
                if (parent != null)
                    parent.Varijante.Remove(this);
                parent = value;
                if (parent != null)
                    parent.Varijante.Add(this);
            }
        }
        
        public bool isVarijanta()
        {
            return Parent != null;
        }

        public string VarijantaString
        {
            get
            {
                if (!isVarijanta())
                    return String.Empty;

                string nazivEl = naziv;
                if (nazivEl == "")
                    nazivEl = engleskiNaziv;
                return podBroj + " - " + nazivEl;
            }
        }

        public bool validate()
        {
            if (naziv == "" && engleskiNaziv == "" && nazivPoGimnasticaru == "")
                throw new InvalidPropertyException("Naziv ne sme da bude prazan.",
                    "Naziv");
            if (!Enum.IsDefined(typeof(Sprava), sprava) || sprava == Sprava.Undefined)
                throw new InvalidPropertyException("Nedozvoljena vrednost za spravu.", 
                    "Sprava");
            
            if (!isTablicniElement)
            {
                if (!isVarijanta())
                {
                    foreach (Element e in Varijante)
                    {
                        if (isTablicniElement != e.isTablicniElement)
                            throw new InvalidPropertyException("Varijanta i osnovni " +
                                "element moraju oboje da budu ili tablicni ili " +
                                "netablicni.", "Varijante");
                    }
                    if (!variantsAreDistinct())
                        throw new InvalidPropertyException("Nije dozvoljeno da dve varijante " +
                            "imaju isti broj.", "Varijante");
                }
                else
                {
                    if (isTablicniElement != Parent.IsTablicniElement)
                        throw new InvalidPropertyException("Varijanta i osnovni " +
                            "element moraju oboje da budu ili tablicni ili " +
                            "netablicni.", "IsTablicniElement");

                }
            }
            else
            {
                if (!Enum.IsDefined(typeof(GrupaElementa), grupa) || grupa == GrupaElementa.Undefined)
                    throw new InvalidPropertyException("Nedozvoljena vrednost za grupu elementa.",
                        "Grupa");
                if (!Enum.IsDefined(typeof(TezinaElementa), tezina) || tezina == TezinaElementa.Undefined)
                    throw new InvalidPropertyException("Nedozvoljena vrednost za tezinu elementa.",
                        "Tezina");
                if (broj <= 0)
                    throw new InvalidPropertyException("Nedozvoljena vrednost za broj elementa.",
                        "Broj");

                if (!isVarijanta())
                {
                    if (podBroj != 0)
                        throw new InvalidPropertyException("Nedozvoljena vrednost za broj elementa.",
                            "Broj");
                    foreach (Element e in Varijante)
                    {
                        if (sprava != e.sprava)
                            throw new InvalidPropertyException("Varijanta i osnovni " +
                                "element moraju da budu za istu spravu.", "Varijante");
                        if (isTablicniElement != e.isTablicniElement)
                            throw new InvalidPropertyException("Varijanta i osnovni " +
                                "element moraju oboje da budu ili tablicni ili " +
                                "netablicni.", "Varijante");
                        if (grupa != e.grupa)
                            throw new InvalidPropertyException("Varijanta i osnovni " +
                                "element moraju da imaju istu grupu.", "Varijante");
                        if (tezina != e.tezina)
                            throw new InvalidPropertyException("Varijanta i osnovni " +
                                "element moraju da imaju istu tezinu.", "Varijante");
                        if (broj != e.broj)
                            throw new InvalidPropertyException("Varijanta i osnovni " +
                                "element moraju da imaju isti broj.", "Varijante");
                        if (!variantsAreDistinct())
                            throw new InvalidPropertyException("Nije dozvoljeno da dve varijante " +
                                "imaju isti broj.", "Varijante");
                    }
                }
                else
                {
                    if (sprava != Parent.Sprava)
                        throw new InvalidPropertyException("Nedozvoljena vrednost za " + 
                            "spravu. Sprava mora da bude kao kod osnovnog elementa.",
                            "Sprava");
                    if (isTablicniElement != Parent.IsTablicniElement)
                        throw new InvalidPropertyException("Varijanta i osnovni " +
                            "element moraju oboje da budu ili tablicni ili " +
                            "netablicni.", "IsTablicniElement");
                    if (grupa != Parent.Grupa)
                        throw new InvalidPropertyException("Nedozvoljena vrednost za " +
                            "grupu. Grupa mora da bude kao kod osnovnog elementa.",
                            "Grupa");
                    if (tezina != Parent.Tezina)
                        throw new InvalidPropertyException("Nedozvoljena vrednost za " +
                            "tezinu. Tezina mora da bude kao kod osnovnog elementa.",
                            "Tezina");
                    if (broj != Parent.Broj)
                        throw new InvalidPropertyException("Nedozvoljena vrednost za " +
                            "broj. Broj mora da bude kao kod osnovnog elementa.",
                            "Broj");
                    if (podBroj <= 0)
                        throw new InvalidPropertyException("Nedozvoljena vrednost za " +
                            "broj varijante. Broj varijante mora da bude veci od nula.",
                            "Broj");
                }
            }
            return true;
        }

        public bool variantsAreDistinct()
        {
            if (Varijante.Count < 2)
                return true;

            PropertyDescriptor propDesc =
                TypeDescriptor.GetProperties(typeof(Element))["PodBroj"];
            SortComparer<Element> comparer = new SortComparer<Element>(propDesc, ListSortDirection.Ascending);
            List<Element> copy = new List<Element>(Varijante);
            copy.Sort(comparer);

            for (int i = 0; i < copy.Count - 1; i++)
            {
                if (copy[i].PodBroj == copy[i + 1].PodBroj)
                    return false;
            }
            return true;
        }

        public Slika getPodrazumevanaSlika()
        {
            foreach (Slika s in slike)
            {
                if (s.Podrazumevana)
                    return s;
            }
            if (slike.Count > 0)
                return slike[0];

            return null;
        }

        public static TezinaElementa getTezina(int broj)
        {
            return (TezinaElementa)((broj - 1) % 6 + 1);
        }

        public void promeniGrupuBroj(GrupaElementa grupa, int broj)
        {
            if (isTablicniElement && isVarijanta() == false
            && (sprava != Sprava.Parter || grupa != GrupaElementa.V))
            {
                Grupa = grupa;
                Broj = (short)broj;
                Tezina = getTezina(broj);
            }
        }
    }

    public class GrupaBrojClass : IComparable
    {
        private Element element;

        public GrupaBrojClass(Element element)
        {
            this.element = element;
        }

        public override string ToString()
        {
            return ToString(element.IsTablicniElement, element.Grupa, element.Broj,
                element.PodBroj);
        }

        public static string ToString(bool isTablicniElement, GrupaElementa grupa,
            int broj, int podBroj)
        {
            if (!isTablicniElement)
                return String.Empty;
            else
                return String.Format("{0} - {1}", grupa, Element.formatBrojPodBroj(broj, podBroj));
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is GrupaBrojClass)
            {
                GrupaBrojClass other = (GrupaBrojClass)obj;

                int grupaOrder = this.element.Grupa.CompareTo(other.element.Grupa);
                int brojOrder = this.element.Broj.CompareTo(other.element.Broj);
                int podBrojOrder = this.element.PodBroj.CompareTo(other.element.PodBroj);
                if (grupaOrder != 0)
                    return grupaOrder;
                else if (brojOrder != 0)
                    return brojOrder;
                else
                    return podBrojOrder;
            }
            throw new ArgumentException("object is not a GrupaBrojClass");
        }

        #endregion
    }

}
