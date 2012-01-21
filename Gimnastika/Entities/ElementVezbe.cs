using System;
using System.Collections.Generic;
using System.Text;

using Gimnastika.Exceptions;

namespace Gimnastika.Entities
{
    public class ElementVezbe : DomainObject
    {
        private byte redBroj;
        private Vezba vezba;
        private bool bodujeSe;
        private Nullable<float> vezaSaPrethodnim;
        private Nullable<float> zahtev;
        private Nullable<float> odbitak;
        private Nullable<float> penalizacija;

        // duplicated from Element
        private string naziv;
        private string engleskiNaziv;
        private bool isTablicniElement;
        private GrupaElementa grupa;
        private TezinaElementa tezina;
        private short broj;
        private byte podBroj;

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


            vezba = ev.vezba;
            if (vezba != null && shouldClone(new TypeAsocijacijaPair(typeof(Vezba))))
                vezba = (Vezba)vezba.Clone();
        }

        public byte RedBroj
        {
            get { return redBroj; }
            set { redBroj = value; }
        }

        public Vezba Vezba
        {
            get { return vezba; }
            set
            {
                // dvosmernu asocijaciju izmedju Vezbe i ElementaVezbe kontrolise
                // Vezba

                // kada je potrebno da se najpre raskine postojeca asocijacija
                // elementa i vezbe
                if (vezba != null)
                    vezba.UkloniElement(this);

                vezba = value;
                if (vezba != null)
                    vezba.DodajElement(this);
            }
        }

        // Metod koji koristi klasa Vezba, za upravljanje asocijacijama izmedju
        // Vezbe i ElementaVezbe
        internal void setVezba(Vezba vezba)
        {
            this.vezba = vezba;
        }


        public bool BodujeSe
        {
            get { return bodujeSe; }
            set { bodujeSe = value; }
        }

        public Nullable<float> VezaSaPrethodnim
        {
            get { return vezaSaPrethodnim; }
            set { vezaSaPrethodnim = value; }
        }

        public Nullable<float> Zahtev
        {
            get { return zahtev; }
            set { zahtev = value; }
        }

        public Nullable<float> Odbitak
        {
            get { return odbitak; }
            set { odbitak = value; }
        }

        public Nullable<float> Penalizacija
        {
            get { return penalizacija; }
            set { penalizacija = value; }
        }

        public TezinaElementa Tezina
        {
            get { return tezina; }
        }

        public string GrupaBroj
        {
            get
            {
                return GrupaBrojClass.ToString(isTablicniElement, grupa, broj, podBroj);
            }
        }

        public Nullable<float> Vrednost
        {
            get
            {
                if (!isTablicniElement)
                    return null;
                else
                    return Element.VrednostTezine[(int)tezina];
            }
        }

        public string Naziv
        {
            get { return naziv; }
        }

        public string EngleskiNaziv
        {
            get { return engleskiNaziv; }
        }

        public bool IsTablicniElement
        {
            get { return isTablicniElement; }
        }

        public GrupaElementa Grupa
        {
            get { return grupa; }
        }

        public short Broj
        {
            get { return broj; }
        }

        public byte PodBroj
        {
            get { return podBroj; }
            set { podBroj = value; }
        }

        public string NazivElementa
        {
            get
            {
                if (naziv != "")
                    return naziv;
                else
                    return engleskiNaziv;
            }
        }

    }
}
