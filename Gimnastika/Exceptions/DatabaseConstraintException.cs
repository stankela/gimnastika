using System;
using System.Collections.Generic;
using System.Text;

namespace Gimnastika.Exceptions
{
    [Serializable]
    class DatabaseConstraintException : Exception
    {
        private List<ValidationError> validationErrors;

        public List<ValidationError> ValidationErrors
        {
            get { return validationErrors; }
        }

        public DatabaseConstraintException()
        {

        }

        public DatabaseConstraintException(string message, List<ValidationError> validationErrors)
            : base(message)
        {
            this.validationErrors = validationErrors;
        }

        public DatabaseConstraintException(string message, List<ValidationError> validationErrors,
            Exception inner)
            : base(message, inner)
        {
            this.validationErrors = validationErrors;
        }
   }
}
