using System;
using System.Collections.Generic;
using System.Text;

using Gimnastika.Exceptions;

namespace Gimnastika.Domain
{
    public class ElementVezbe : DomainObject, IComparable
    {
        private byte redBroj;
        public virtual byte RedBroj
        {
            get { return redBroj; }
            set { redBroj = value; }
        }

        private bool bodujeSe;
        public virtual bool BodujeSe
        {
            get { return bodujeSe; }
            set { bodujeSe = value; }
        }

        private Nullable<float> vezaSaPrethodnim;
        public virtual Nullable<float> VezaSaPrethodnim
        {
            get { return vezaSaPrethodnim; }
            set { vezaSaPrethodnim = value; }
        }

        private Nullable<float> zahtev;
        public virtual Nullable<float> Zahtev
        {
            get { return zahtev; }
            set { zahtev = value; }
        }

        private Nullable<float> odbitak;
        public virtual Nullable<float> Odbitak
        {
            get { return odbitak; }
            set { odbitak = value; }
        }

        private Nullable<float> penalizacija;
        public virtual Nullable<float> Penalizacija
        {
            get { return penalizacija; }
            set { penalizacija = value; }
        }

        // following properties are duplicated from Element

        private string naziv;
        public virtual string Naziv
        {
            get { return naziv; }
            private set { naziv = value; }
        }

        private string engleskiNaziv;
        public virtual string EngleskiNaziv
        {
            get { return engleskiNaziv; }
            private set { engleskiNaziv = value; }
        }

        private bool isTablicniElement;
        public virtual bool IsTablicniElement
        {
            get { return isTablicniElement; }
            private set { isTablicniElement = value; }
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
            set { podBroj = value; }
        }

        public ElementVezbe()
        { 
        
        }

        public ElementVezbe(string naziv, string engleskiNaziv, bool isTablicniElement,
            GrupaElementa grupa, TezinaElementa tezina, short broj, byte podBroj)
        {
            this.naziv = naziv;
            this.engleskiNaziv = engleskiNaziv;
            this.isTablicniElement = isTablicniElement;
            this.grupa = grupa;
            this.tezina = tezina;
            this.broj = broj;
            this.podBroj = podBroj;

            this.bodujeSe = false;
        }

        public ElementVezbe(string naziv, string engleskiNaziv, bool isTablicniElement,
            GrupaElementa grupa, TezinaElementa tezina, short broj, byte podBroj,
            bool bodujeSe, Nullable<float> vezaSaPrethodnim, Nullable<float> zahtev, 
            Nullable<float> odbitak, Nullable<float> penalizacija)
        {
            this.naziv = naziv;
            this.engleskiNaziv = engleskiNaziv;
            this.isTablicniElement = isTablicniElement;
            this.grupa = grupa;
            this.tezina = tezina;
            this.broj = broj;
            this.podBroj = podBroj;

            this.bodujeSe = bodujeSe;
            this.vezaSaPrethodnim = vezaSaPrethodnim;
            this.zahtev = zahtev;
            this.odbitak = odbitak;
            this.penalizacija = penalizacija;
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            ElementVezbe ev = (ElementVezbe)domainObject;
            redBroj = ev.redBroj;
            bodujeSe = ev.bodujeSe;
            vezaSaPrethodnim = ev.vezaSaPrethodnim;
            zahtev = ev.zahtev;
            odbitak = ev.odbitak;
            penalizacija = ev.penalizacija;


            naziv = ev.naziv;
            engleskiNaziv = ev.engleskiNaziv;
            isTablicniElement = ev.isTablicniElement;
            grupa = ev.grupa;
            tezina = ev.tezina;
            broj = ev.broj;
            podBroj = ev.podBroj;
        }

        public virtual string GrupaBroj
        {
            get
            {
                return GrupaBrojClass.ToString(isTablicniElement, grupa, broj, podBroj);
            }
        }

        public virtual Nullable<float> Vrednost
        {
            get
            {
                if (!isTablicniElement)
                    return null;
                else
                    return Element.VrednostTezine[(int)tezina];
            }
        }

        public virtual string NazivElementa
        {
            get
            {
                if (naziv != "")
                    return naziv;
                else
                    return engleskiNaziv;
            }
        }


        #region IComparable Members

        public virtual int CompareTo(object obj)
        {
            if (obj is ElementVezbe)
            {
                ElementVezbe other = (ElementVezbe)obj;
                return this.RedBroj.CompareTo(other.RedBroj);
            }
            throw new ArgumentException("object is not a ElementVezbe");
        }

        #endregion
    }
}
