using System;
using System.Collections.Generic;
using System.Text;

using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika.Dao;

namespace Gimnastika
{
    // TODO: osmisli bolje resenje za validatore
    public class DatabaseConstraintsValidator
    {
        private static List<ValidationError> validationErrors;

        private static void validateProperties(Element element)
        {
            // validate Naziv
            if (element.Naziv.Length == 0 && element.EngleskiNaziv.Length == 0
            && element.NazivPoGimnasticaru.Length == 0)
            {
                throw new InvalidPropertyException("Naziv ne sme da bude prazan.", "Naziv");
            }
            if (element.Naziv.Length > Element.NAZIV_MAX_LENGTH)
            {
                throw new InvalidPropertyException("Naziv moze da sadrzi maksimalno "
                    + Element.NAZIV_MAX_LENGTH + " znakova.", "Naziv");
            }
            if (element.NazivPoGimnasticaru.Length > Element.NAZIV_GIM_MAX_LENGTH)
            {
                throw new InvalidPropertyException("Naziv po gimnasticaru moze da sadrzi maksimalno "
                    + Element.NAZIV_GIM_MAX_LENGTH + " znakova.", "NazivPoGimnasticaru");
            }
            if (element.EngleskiNaziv.Length > Element.NAZIV_MAX_LENGTH)
            {
                throw new InvalidPropertyException("Engleski naziv moze da sadrzi maksimalno "
                    + Element.NAZIV_MAX_LENGTH + " znakova.", "EngleskiNaziv");
            }
        }

        // can throw DatabaseException
        public static void checkInsert(Element element)
        {
            validationErrors = new List<ValidationError>();
            validateProperties(element);

            // TODO: Nek bude moguce da dva tablicna elementa iste sprave, grupe,
            // tezine i broja, a razlicitog podbroja imaju isti naziv (promeni ovo i
            // kod checkUpdate)

            if (element.Naziv != "" &&
            new ElementDAO().postojiElement(element.Sprava, element.Naziv))
            {
                ValidationError error = new ValidationError();
                error.InvalidProperties = new string[] { "Naziv", "Sprava" };
                error.Message = "Element sa datim nazivom vec postoji.";
                validationErrors.Add(error);
            }
            if (element.EngleskiNaziv != "" &&
            new ElementDAO().postojiElementEng(element.Sprava, element.EngleskiNaziv))
            {
                ValidationError error = new ValidationError();
                error.InvalidProperties = new string[] { "EngleskiNaziv", "Sprava" };
                error.Message = "Element sa datim engleskim nazivom vec postoji.";
                validationErrors.Add(error);
            }
            if (element.NazivPoGimnasticaru != "" &&
            new ElementDAO().postojiElementGim(element.Sprava, element.NazivPoGimnasticaru))
            {
                ValidationError error = new ValidationError();
                error.InvalidProperties = new string[] { "NazivPoGimnasticaru", "Sprava" };
                error.Message = "Element sa datim nazivom po gimnasticaru vec postoji.";
                validationErrors.Add(error);
            }
            if (element.IsTablicniElement)
            {
                if (new ElementDAO().postojiElement(element.Sprava, element.Grupa,
                    element.Broj, element.PodBroj))
                {
                    ValidationError error = new ValidationError();
                    error.InvalidProperties = new string[] { "Grupa", "Broj",
                        "PodBroj", "Sprava" };
                    error.Message = "Vec postoji element sa datim brojem za datu spravu i grupu.";
                    validationErrors.Add(error);
                }
            }
            if (validationErrors.Count > 0)
            {
                throw new DatabaseConstraintException(
                    "Element nije validan.", validationErrors);
            }
        }

        // can throw DatabaseException
        public static void checkUpdate(Element element, Element original)
        {
            validationErrors = new List<ValidationError>();
            validateProperties(element);
            if (element.Sprava != original.Sprava
            || element.Naziv != original.Naziv)
            {
                if (element.Naziv != "" && new ElementDAO().postojiElement(element.Sprava, element.Naziv))
                {
                    ValidationError error = new ValidationError();
                    error.InvalidProperties = new string[] { "Naziv", "Sprava" };
                    error.Message = "Element sa datim nazivom vec postoji.";
                    validationErrors.Add(error);
                }
            }
            if (element.Sprava != original.Sprava
            || element.EngleskiNaziv != original.EngleskiNaziv)
            {
                if (element.EngleskiNaziv != ""
                && new ElementDAO().postojiElementEng(element.Sprava, element.EngleskiNaziv))
                {
                    ValidationError error = new ValidationError();
                    error.InvalidProperties = new string[] { "EngleskiNaziv", "Sprava" };
                    error.Message = "Element sa datim engleskim nazivom vec postoji.";
                    validationErrors.Add(error);
                }
            }
            if (element.Sprava != original.Sprava
            || element.NazivPoGimnasticaru != original.NazivPoGimnasticaru)
            {
                if (element.NazivPoGimnasticaru != "" &&
                new ElementDAO().postojiElementGim(element.Sprava, element.NazivPoGimnasticaru))
                {
                    ValidationError error = new ValidationError();
                    error.InvalidProperties = new string[] { "NazivPoGimnasticaru", "Sprava" };
                    error.Message = "Element sa datim nazivom po gimnasticaru vec postoji.";
                    validationErrors.Add(error);
                }
            }        
            if (element.IsTablicniElement)
            {
                if (element.Sprava != original.Sprava
                || element.Grupa != original.Grupa
                || element.Tezina != original.Tezina
                || element.Broj != original.Broj
                || element.PodBroj != original.PodBroj)
                {
                    if (new ElementDAO().postojiElement(element.Sprava, element.Grupa,
                        element.Broj, element.PodBroj))
                    {
                        ValidationError error = new ValidationError();
                        error.InvalidProperties = new string[] { "Grupa", "Broj",
                            "PodBroj", "Sprava" };
                        error.Message = "Vec postoji element sa datim brojem za datu spravu i grupu.";
                        validationErrors.Add(error);
                    }
                }
            }
            if (validationErrors.Count > 0)
            {
                throw new DatabaseConstraintException(
                    "Element nije validan.", validationErrors);
            }
        }

        public static void checkInsertElements(Vezba vezba)
        {
            // TODO
        }

        public static void checkUpdateElements(Vezba vezba, Vezba original)
        {
            // TODO
        }

        private static void validateProperties(PraviloOceneVezbe pravilo)
        {
            // validate Naziv
            if (pravilo.Naziv.Length == 0)
            {
                throw new InvalidPropertyException("Naziv pravila ne sme da bude prazan.", "Naziv");
            }
            if (pravilo.Naziv.Length > PraviloOceneVezbe.NAZIV_MAX_LENGTH)
            {
                throw new InvalidPropertyException("Naziv pravila moze da sadrzi maksimalno "
                    + PraviloOceneVezbe.NAZIV_MAX_LENGTH + " znakova.", "Naziv");
            }
        }

        // can throw DatabaseException
        public static void checkInsert(PraviloOceneVezbe pravilo)
        {
            validationErrors = new List<ValidationError>();
            validateProperties(pravilo);

            if (new PraviloOceneVezbeDAO().postojiPravilo(pravilo.Naziv))
            {
                ValidationError error = new ValidationError();
                error.InvalidProperties = new string[] { "Ime", "Prezime" };
                error.Message = "Pravilo sa datim nazivom vec postoji.";
                validationErrors.Add(error);
            }
            if (validationErrors.Count > 0)
            {
                throw new DatabaseConstraintException(
                    "Pravilo nije validno.", validationErrors);
            }
        }

        // can throw DatabaseException
        public static void checkUpdate(PraviloOceneVezbe pravilo, PraviloOceneVezbe original)
        {
            validationErrors = new List<ValidationError>();
            validateProperties(pravilo);
            if (pravilo.Naziv != original.Naziv)
            {
                if (new PraviloOceneVezbeDAO().postojiPravilo(pravilo.Naziv))
                {
                    ValidationError error = new ValidationError();
                    error.InvalidProperties = new string[] { "Ime", "Prezime" };
                    error.Message = "Pravilo sa datim nazivom vec postoji.";
                    validationErrors.Add(error);
                }
            }
            if (validationErrors.Count > 0)
            {
                throw new DatabaseConstraintException(
                    "Pravilo nije validno.", validationErrors);
            }
        }
    }
}
