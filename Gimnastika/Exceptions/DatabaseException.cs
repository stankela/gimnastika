using System;
using System.Collections.Generic;
using System.Text;

namespace Gimnastika.Exceptions
{
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException()
        {

        }

        public DatabaseException(string message)
            : base(message)
        {

        }

        public DatabaseException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
