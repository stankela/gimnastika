using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;
using System.Data.SqlServerCe;
using System.Data;

namespace Gimnastika.Entities
{
    public class Gimnasticar : DomainObject
    {
        private string ime;
        private string prezime;

        public static readonly int IME_MAX_LENGTH = 16;
        public static readonly int PREZIME_MAX_LENGTH = 32;

        public Gimnasticar()
        {

        }

        public Gimnasticar(string ime, string prezime)
        {
            this.ime = ime;
            this.prezime = prezime;
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            Gimnasticar gimnasticar = (Gimnasticar)domainObject;
            ime = gimnasticar.ime;
            prezime = gimnasticar.prezime;
        }

        public void restore(Gimnasticar original)
        {
            ime = original.ime;
            prezime = original.prezime;
        }

        public override bool Equals(object other)
        {
            if (this == other)
                return true;
            Gimnasticar g = other as Gimnasticar;
            if (g == null)
                return false;
            if (this.ime != g.ime || this.prezime != g.prezime)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result;
                result = this.ime.GetHashCode();
                result = 29 * result + this.prezime.GetHashCode();
                return result;
            }
        }

        public override string ToString()
        {
            string result = prezime;
            if (ime != "")
            {
                if (prezime != "")
                    result += " ";
                result += ime;
            }
            return result;
        }

        public string Ime
        {
            get { return ime; }
            set { ime = value.Trim(); }
        }

        public string Prezime
        {
            get { return prezime; }
            set { prezime = value.Trim(); }
        }

        public string PrezimeIme
        {
            get { return ToString(); }
        }
    }
}
