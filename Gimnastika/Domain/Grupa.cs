using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;

namespace Gimnastika.Domain
{
    public class Grupa : DomainObject
    {
        public static int NAZIV_MAX_LENGTH = 128;

        Sprava sprava;
        public virtual Sprava Sprava
        {
            get { return sprava; }
            set { sprava = value; }
        }

        GrupaElementa grupa;
        public virtual GrupaElementa GrupaElemenata
        {
            get { return grupa; }
            set { grupa = value; }
        }

        string naziv;
        public virtual string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        string engNaziv;
        public virtual string EngNaziv
        {
            get { return engNaziv; }
            set { engNaziv = value; }
        }

        public Grupa()
        {

        }

        public Grupa(Sprava sprava, GrupaElementa grupa, string naziv, string engNaziv)
        {
            this.sprava = sprava;
            this.grupa = grupa;
            this.naziv = naziv;
            this.engNaziv = engNaziv;
        }

        public virtual bool validate()
        {
            if (naziv.Length > Grupa.NAZIV_MAX_LENGTH)
            {
                throw new InvalidPropertyException("Naziv moze da sadrzi maksimalno "
                    + Grupa.NAZIV_MAX_LENGTH + " znakova.", "Naziv");
            }
            if (engNaziv.Length > Grupa.NAZIV_MAX_LENGTH)
            {
                throw new InvalidPropertyException("Naziv moze da sadrzi maksimalno "
                    + Grupa.NAZIV_MAX_LENGTH + " znakova.", "EngNaziv");
            }
            return true;
        }
    }
}
