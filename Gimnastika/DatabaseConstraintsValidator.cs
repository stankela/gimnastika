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

        // can throw DatabaseException
        /*public static void checkInsert(Element element)
        {
            validationErrors = new List<ValidationError>();
            validateProperties(element);

        }

        // can throw DatabaseException
        public static void checkUpdate(Element element, Element original)
        {
            validationErrors = new List<ValidationError>();
            validateProperties(element);
            if (validationErrors.Count > 0)
            {
                throw new DatabaseConstraintException(
                    "Element nije validan.", validationErrors);
            }
        }*/

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
