using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;
using System.Text.RegularExpressions;
using System.Data.SqlServerCe;
using System.Data;
using System.ComponentModel;
using Gimnastika.Dao;
using Iesi.Collections.Generic;

namespace Gimnastika.Domain
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
        public static readonly int NAZIV_MAX_LENGTH = 128;
        public static readonly int NAZIV_GIM_MAX_LENGTH = 64;
        private static readonly char brojDelimiter = ',';
        private static readonly string rgxBrojPodBroj = @"^\s*\d{1,4}\s*(,\s*\d{1,2}\s*)?$";

        public static readonly float[] VrednostTezine = new float[8] {
            0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f
        };

        private string naziv;
        public virtual string Naziv
        {
            get { return naziv; }
            set { naziv = value.Trim(); }
        }

        private string engleskiNaziv;
        public virtual string EngleskiNaziv
        {
            get { return engleskiNaziv; }
            set { engleskiNaziv = value.Trim(); }
        }

        private string nazivPoGimnasticaru;
        public virtual string NazivPoGimnasticaru
        {
            get { return nazivPoGimnasticaru; }
            set { nazivPoGimnasticaru = value.Trim(); }
        }

        private Sprava sprava;
        public virtual Sprava Sprava
        {
            get { return sprava; }
            private set { sprava = value; }
        }

        private bool isTablicniElement;
        public virtual bool IsTablicniElement
        {
            get { return isTablicniElement; }
            set { isTablicniElement = value; }
        }

        private GrupaElementa grupa;
        public virtual GrupaElementa Grupa
        {
            get { return grupa; }
            private set { grupa = value; }
        }

        private TezinaElementa tezina;
        public virtual TezinaElementa Tezina
        {
            get { return tezina; }
            private set { tezina = value; }
        }

        private short broj;
        public virtual short Broj
        {
            get { return broj; }
            private set { broj = value; }
        }

        private byte podBroj;
        public virtual byte PodBroj
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

        private Element parent;
        public virtual Element Parent
        {
            get { return parent; }
            private set { parent = value; }  // Metodi dodajVarijantu i ukloniVarijantu kontrolisu dvosmernu asocijaciju
                                             // element <-----> varijanta
        }

        private ISet<Element> varijante = new HashedSet<Element>();
        public virtual ISet<Element> Varijante
        {
            get { return varijante; }
            set { varijante = value; }
        }

        private ISet<Video> videoKlipovi = new HashedSet<Video>();
        public virtual ISet<Video> VideoKlipovi
        {
            get { return videoKlipovi; }
            set { videoKlipovi = value; }
        }

        private ISet<Slika> slike = new HashedSet<Slika>();
        public virtual ISet<Slika> Slike
        {
            get { return slike; }
            set { slike = value; }
        }

        private GrupaBrojClass grupaBroj;

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
            //parentId = element.parentId;
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

            //varijante = new List<Element>();
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

        public virtual void restore(Element original)
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
            //parentId = original.parentId;
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

        public virtual void changeSprava(Sprava value)
        {
            Sprava = value;
            if (!isVarijanta())
            {
                foreach (Element e in Varijante)
                    e.Sprava = value;
            }
        }

        public virtual void changeGrupa(GrupaElementa value)
        {
            Grupa = value;
            if (!isVarijanta())
            {
                foreach (Element e in Varijante)
                    e.Grupa = value;
            }
        }

        public virtual void changeTezina(TezinaElementa value)
        {
            Tezina = value;
            if (!isVarijanta())
            {
                foreach (Element e in Varijante)
                    e.Tezina = value;
            }
        }

        public virtual void changeBroj(short value)
        {
            if (value <= 0)
            {
                throw new InvalidPropertyException("Broj mora da bude veci od 0.", "Broj");
            }
            Broj = value;
            if (!isVarijanta())
            {
                foreach (Element e in Varijante)
                    e.Broj = value;
            }
        }

        public override string ToString()
        {
            string nazivEl = NazivString;
            string result;
            result = String.Format("{0}, {1}", nazivEl, sprava);
            if (IsTablicniElement)
            {
                result += String.Format(", {0}-{1}", Grupa, Broj);
                if (PodBroj != 0)
                {
                    result += String.Format(",{0}", PodBroj);
                }
                result += String.Format(" {0}", Tezina);
            }
            return result;
        }

        public virtual string NazivString
        {
            get
            {
                string result = this.Naziv;
                if (result == "")
                    result = this.NazivPoGimnasticaru;
                if (result == "")
                    result = this.EngleskiNaziv;
                return result;
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

        public virtual string BrojPodBroj
        {
            get 
            {
                if (!IsTablicniElement)
                    return String.Empty;
                else
                    return formatBrojPodBroj(Broj, PodBroj);
            }
            set
            {
                if (isValidBrojPodBroj(value))
                {
                    if (value.IndexOf(brojDelimiter) == -1)
                    {
                        Broj = Int16.Parse(value);
                        PodBroj = 0;
                    }
                    else
                    {
                        string[] s = value.Split(brojDelimiter);
                        Broj = Int16.Parse(s[0]);
                        PodBroj = Byte.Parse(s[1]);
                    }
                    if (!isVarijanta())
                    {
                        foreach (Element e in Varijante)
                            e.changeBroj(Broj);
                    }
                }
                else
                {
                    throw new InvalidPropertyException(
                        "Broj elementa nije u zahtevanom formatu. Ukoliko je u pitanju " +
                        "varijanta elementa, broj varijante treba da je razdvojen zarezom (npr. 7,1).", "Broj");
                }
            }
        }

        public virtual GrupaBrojClass GrupaBroj
        {
            get { return grupaBroj; }
        }

        public virtual Nullable<float> Vrednost
        {
            get
            {
                if (!IsTablicniElement)
                    return null;
                else
                    return VrednostTezine[(int)Tezina];
            }
        }

        public virtual void setPolozajUTablici(GrupaElementa grupa, TezinaElementa tezina,
            string broj)
        {
            this.changeGrupa(grupa);
            this.changeTezina(tezina);
            BrojPodBroj = broj;
            IsTablicniElement = true;

            // TODO: Ispitati zasto nisam postavio nove vrednosti i za varijante (i da
            // li ih treba postaviti)
        }

        public virtual void ponistiPolozajUTablici()
        {
            this.changeGrupa(GrupaElementa.Undefined);
            this.changeTezina(TezinaElementa.Undefined);
            this.Broj = 0;
            this.PodBroj = 0;
            IsTablicniElement = false;

            if (!isVarijanta())
            {
                foreach (Element e in Varijante)
                    e.ponistiPolozajUTablici();
            }
        }

        public virtual void dodajVideo(Video video)
        {
            if (video != null)
                videoKlipovi.Add(video);
        }

        public virtual void ukloniVideo(Video video)
        {
            videoKlipovi.Remove(video);
        }

        public virtual void ukloniVideoKlipove()
        {
            videoKlipovi.Clear();
        }

        public virtual void dodajSliku(Slika slika)
        {
            if (slika != null)
                slike.Add(slika);
        }

        public virtual void ukloniSliku(Slika slika)
        {
            slike.Remove(slika);
        }

        public virtual void ukloniSlike()
        {
            slike.Clear();
        }

        public virtual void dodajVarijantu(Element varijanta)
        {
            if (varijanta == null)
                return;
            if (varijanta.Parent != null)
                varijanta.Parent.ukloniVarijantu(varijanta);
            varijanta.Parent = this;
            Varijante.Add(varijanta);
        }

        public virtual void ukloniVarijantu(Element varijanta)
        {
            if (varijanta == null)
                return;
            if (varijanta.Parent == this)
            {
                varijanta.Parent = null;
                Varijante.Remove(varijanta);
            }
        }

        public virtual bool isVarijanta()
        {
            return Parent != null;
        }

        public virtual string VarijantaString
        {
            get
            {
                if (!isVarijanta())
                    return String.Empty;

                string nazivEl = Naziv;
                if (nazivEl == "")
                    nazivEl = EngleskiNaziv;
                return PodBroj + " - " + nazivEl;
            }
        }

        public override void validate(Notification notification)
        {
            if (Naziv == "" && EngleskiNaziv == "" && NazivPoGimnasticaru == "")
                notification.RegisterMessage("Naziv", "Naziv ne sme da bude prazan.");
            if (Naziv.Length > NAZIV_MAX_LENGTH)
            {
                notification.RegisterMessage("Naziv", "Naziv moze da sadrzi maksimalno "
                    + Element.NAZIV_MAX_LENGTH + " znakova.");
            }
            if (NazivPoGimnasticaru.Length > NAZIV_GIM_MAX_LENGTH)
            {
                notification.RegisterMessage("NazivPoGimnasticaru", "Naziv po gimnasticaru moze da sadrzi maksimalno "
                    + Element.NAZIV_GIM_MAX_LENGTH + " znakova.");
            }
            if (EngleskiNaziv.Length > NAZIV_MAX_LENGTH)
            {
                notification.RegisterMessage("EngleskiNaziv", "Engleski naziv moze da sadrzi maksimalno "
                    + Element.NAZIV_MAX_LENGTH + " znakova.");
            }
            if (!Enum.IsDefined(typeof(Sprava), Sprava) || Sprava == Sprava.Undefined)
                notification.RegisterMessage("Sprava", "Nedozvoljena vrednost za spravu.");

            if (!IsTablicniElement)
            {
                if (!isVarijanta())
                {
                    foreach (Element e in Varijante)
                    {
                        if (IsTablicniElement != e.IsTablicniElement)
                            notification.RegisterMessage("Varijante", "Varijanta i osnovni " +
                                "element moraju oboje da budu ili tablicni ili " +
                                "netablicni.");
                    }
                    if (!variantsAreDistinct())
                        notification.RegisterMessage("Varijante", "Nije dozvoljeno da dve varijante " +
                            "imaju isti broj.");
                }
                else
                {
                    if (IsTablicniElement != Parent.IsTablicniElement)
                        notification.RegisterMessage("IsTablicniElement", "Varijanta i osnovni " +
                            "element moraju oboje da budu ili tablicni ili " +
                            "netablicni.");

                }
            }
            else
            {
                if (!Enum.IsDefined(typeof(GrupaElementa), Grupa) || Grupa == GrupaElementa.Undefined)
                    notification.RegisterMessage("Grupa", "Nedozvoljena vrednost za grupu elementa.");
                if (!Enum.IsDefined(typeof(TezinaElementa), Tezina) || Tezina == TezinaElementa.Undefined)
                    notification.RegisterMessage("Tezina", "Nedozvoljena vrednost za tezinu elementa.");
                if (Broj <= 0)
                    notification.RegisterMessage("Broj", "Nedozvoljena vrednost za broj elementa.");

                if (!isVarijanta())
                {
                    if (PodBroj != 0)
                        notification.RegisterMessage("Broj", "Nedozvoljena vrednost za broj elementa.");
                    foreach (Element e in Varijante)
                    {
                        if (Sprava != e.Sprava)
                            notification.RegisterMessage("Varijante", "Varijanta i osnovni " +
                                "element moraju da budu za istu spravu.");
                        if (IsTablicniElement != e.IsTablicniElement)
                            notification.RegisterMessage("Varijante", "Varijanta i osnovni " +
                                "element moraju oboje da budu ili tablicni ili " +
                                "netablicni.");
                        if (Grupa != e.Grupa)
                            notification.RegisterMessage("Varijante", "Varijanta i osnovni " +
                                "element moraju da imaju istu grupu.");
                        if (Tezina != e.Tezina)
                            notification.RegisterMessage("Varijante", "Varijanta i osnovni " +
                                "element moraju da imaju istu tezinu.");
                        if (Broj != e.Broj)
                            notification.RegisterMessage("Varijante", "Varijanta i osnovni " +
                                "element moraju da imaju isti broj.");
                        if (!variantsAreDistinct())
                            notification.RegisterMessage("Varijante", "Nije dozvoljeno da dve varijante " +
                                "imaju isti broj.");
                    }
                }
                else
                {
                    if (Sprava != Parent.Sprava)
                        notification.RegisterMessage("Sprava", "Nedozvoljena vrednost za " + 
                            "spravu. Sprava mora da bude kao kod osnovnog elementa.");
                    if (IsTablicniElement != Parent.IsTablicniElement)
                        notification.RegisterMessage("IsTablicniElement", "Varijanta i osnovni " +
                            "element moraju oboje da budu ili tablicni ili " +
                            "netablicni.");
                    if (Grupa != Parent.Grupa)
                        notification.RegisterMessage("Grupa", "Nedozvoljena vrednost za " +
                            "grupu. Grupa mora da bude kao kod osnovnog elementa.");
                    if (Tezina != Parent.Tezina)
                        notification.RegisterMessage("Tezina", "Nedozvoljena vrednost za " +
                            "tezinu. Tezina mora da bude kao kod osnovnog elementa.");
                    if (Broj != Parent.Broj)
                        notification.RegisterMessage("Broj", "Nedozvoljena vrednost za " +
                            "broj. Broj mora da bude kao kod osnovnog elementa.");
                    if (PodBroj <= 0)
                        notification.RegisterMessage("Broj", "Nedozvoljena vrednost za " +
                            "broj varijante. Broj varijante mora da bude veci od nula.");
                }
            }
        }

        public virtual bool variantsAreDistinct()
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

        public virtual Slika getPodrazumevanaSlika()
        {
            foreach (Slika s in Slike)
            {
                if (s.Podrazumevana)
                    return s;
            }
            if (Slike.Count > 0)
            {
                //return Slike[0];
                IEnumerator<Slika> enumerator = Slike.GetEnumerator();
                enumerator.Reset();
                enumerator.MoveNext();
                return enumerator.Current;
            }

            return null;
        }

        public static TezinaElementa getTezina(int broj)
        {
            return (TezinaElementa)((broj - 1) % 6 + 1);
        }

        public virtual void promeniGrupuBroj(GrupaElementa grupa, int broj)
        {
            if (IsTablicniElement && isVarijanta() == false
            && (Sprava != Sprava.Parter || grupa != GrupaElementa.V))
            {
                this.changeGrupa(grupa);
                this.changeBroj((short)broj);
                this.changeTezina(getTezina(broj));
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
