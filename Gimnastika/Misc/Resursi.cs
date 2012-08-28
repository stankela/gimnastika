using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;

namespace Gimnastika
{
    internal class Resursi
    {
        public static string getImeSprave(Sprava sprava)
        {
            switch (sprava)
            { 
                case Sprava.Parter:
                    return "Parter";

                case Sprava.Konj:
                    return "Konj sa hvataljkama";

                case Sprava.Karike:
                    return "Karike";

                case Sprava.Preskok:
                    return "Preskok";

                case Sprava.Razboj:
                    return "Razboj";

                case Sprava.Vratilo:
                    return "Vratilo";

                default:
                    return "Undefined";
            }
        }

        public static SpravaNazivPair[] SpravaNazivTable
        { 
            get
            {
                SpravaNazivPair[] result = new SpravaNazivPair[6];
                Sprava[] sprave = { Sprava.Parter, Sprava.Konj, Sprava.Karike,
                    Sprava.Preskok, Sprava.Razboj, Sprava.Vratilo};
                for (int i = 0; i < 6; i++)
                    result[i] = new SpravaNazivPair(sprave[i], getImeSprave(sprave[i]));
                return result;
            }
        }

        public static SpravaNazivPair[] SpravaNazivTableEx
        {
            get
            {
                SpravaNazivPair[] spravaNazivTable = SpravaNazivTable;
                SpravaNazivPair[] result = 
                    new SpravaNazivPair[spravaNazivTable.Length + 1];
                result[0] = new SpravaNazivPair(Sprava.Undefined, "Sve sprave");
                spravaNazivTable.CopyTo(result, 1);
                return result;
            }
        }

        public static GrupaNazivPair[] GrupaNazivTable
        {
            get
            {
                return new GrupaNazivPair[]
                {
                    new GrupaNazivPair(GrupaElementa.I, "I"), 
                    new GrupaNazivPair(GrupaElementa.II, "II"), 
                    new GrupaNazivPair(GrupaElementa.III, "III"), 
                    new GrupaNazivPair(GrupaElementa.IV, "IV"), 
                    new GrupaNazivPair(GrupaElementa.V, "V")
                };
            }
        }

        public static GrupaNazivPair[] GrupaNazivTableEx
        {
            get
            {
                GrupaNazivPair[] grupaNazivTable = GrupaNazivTable;
                GrupaNazivPair[] result =
                    new GrupaNazivPair[grupaNazivTable.Length + 1];
                result[0] = new GrupaNazivPair(GrupaElementa.Undefined, "Sve grupe");
                grupaNazivTable.CopyTo(result, 1);
                return result;
            }
        }

        public static TezinaNazivPair[] TezinaNazivTable
        { 
            get
            {
                return new TezinaNazivPair[]
                {
                    new TezinaNazivPair(TezinaElementa.A, "A"),
                    new TezinaNazivPair(TezinaElementa.B, "B"),
                    new TezinaNazivPair(TezinaElementa.C, "C"),
                    new TezinaNazivPair(TezinaElementa.D, "D"),
                    new TezinaNazivPair(TezinaElementa.E, "E"),
                    new TezinaNazivPair(TezinaElementa.F, "F"),
                    new TezinaNazivPair(TezinaElementa.G, "G")
                };
            }
        }

        public static TezinaNazivPair[] TezinaNazivTableEx
        {
            get
            {
                TezinaNazivPair[] tezinaNazivTable = TezinaNazivTable;
                TezinaNazivPair[] result =
                    new TezinaNazivPair[tezinaNazivTable.Length + 1];
                result[0] = new TezinaNazivPair(TezinaElementa.Undefined, "Sve tezine");
                tezinaNazivTable.CopyTo(result, 1);
                return result;
            }
        }

    }

    internal class SpravaNazivPair
    {
        private Sprava sprava;
        private string naziv;

        public Sprava Sprava
        {
            get { return sprava; }
            set { sprava = value; }
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public SpravaNazivPair(Sprava sprava, string naziv)
        {
            this.sprava = sprava;
            this.naziv = naziv;
        }

        public override string ToString()
        {
            return naziv;
        }
    }

    internal class GrupaNazivPair
    { 
        private GrupaElementa grupa;
        private string naziv;

        public GrupaElementa Grupa
        {
            get { return grupa; }
            set { grupa = value; }
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public GrupaNazivPair(GrupaElementa grupa, string naziv)
        {
            this.grupa = grupa;
            this.naziv = naziv;
        }

        public override string ToString()
        {
            return naziv;
        }
    }

    internal class TezinaNazivPair
    {
        private TezinaElementa tezina;
        private string naziv;

        public TezinaElementa Tezina
        {
            get { return tezina; }
            set { tezina = value; }
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public TezinaNazivPair(TezinaElementa tezina, string naziv)
        {
            this.tezina = tezina;
            this.naziv = naziv;
        }
    }
}
