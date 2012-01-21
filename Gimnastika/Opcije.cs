using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Exceptions;
using System.Globalization;

namespace Gimnastika
{
    public class Opcije : ICloneable
    {
        protected static Opcije instance = null;
        public static Opcije Instance
        { 
            get
            {
                if (instance == null)
                    instance = new Opcije();
                return instance;
            }
        }

        private int podrazumevanoPraviloID = 1;
        public int PodrazumevanoPraviloID
        {
            get { return podrazumevanoPraviloID; }
            set { podrazumevanoPraviloID = value; }
        }

        private string playerFileName = "";
        public string PlayerFileName
        {
            get 
            {
                if (playerFileName == "")
                    playerFileName = 
                        @"C:\Program Files\Windows Media Player\wmplayer.exe";
                return playerFileName; 
            }
            set { playerFileName = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            Opcije result = new Opcije();
            return result;
        }

        #endregion

        public void restore(Opcije original)
        {

        }

        public static readonly string ConnectionString = @"Data Source = ../../gimnastika_podaci.sdf;";

        private CultureInfo serbianCultureInfo = new CultureInfo("sr-Latn-CS");

        public char DecimalSeparator
        {
            get { return serbianCultureInfo.NumberFormat.NumberDecimalSeparator[0]; }
        }
    }
}
