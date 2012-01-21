using System;
using System.Collections.Generic;
using System.Text;

namespace Gimnastika.Exceptions
{
    [Serializable]
    public class InvalidFormatException : Exception
    {
        private string invalidProperty;

        public string InvalidProperty
        {
            get { return invalidProperty; }
        }

        public InvalidFormatException()
        {

        }

        public InvalidFormatException(string message, string propertyName)
            : base(message)
        {
            this.invalidProperty = propertyName;
        }

        public InvalidFormatException(string message, string propertyName,
            Exception inner)
            : base(message, inner)
        {
            this.invalidProperty = propertyName;
        }
  }
}
