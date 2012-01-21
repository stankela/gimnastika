using System;
using System.Collections.Generic;
using System.Text;

namespace Gimnastika
{
    public class ValidationError
    {
        string[] invalidProperies;
        string message;

        public string[] InvalidProperties
        {
            get { return invalidProperies; }
            set { invalidProperies = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

}
