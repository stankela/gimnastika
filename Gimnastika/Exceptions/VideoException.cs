using System;
using System.Collections.Generic;
using System.Text;

namespace Gimnastika.Exceptions
{
    // TODO: Za izuzetke tipa VideoException i DatabaseException koji samo sadrze
    // poruku (i informaciju o tipu izuzetka, koja je sadrzana u nazivu klase izuzetka)
    // trebalo bi napraviti osnovnu klasu

    [Serializable]
    public class VideoException : Exception
    {
        public VideoException()
        {

        }

        public VideoException(string message)
            : base(message)
        {

        }

        public VideoException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
