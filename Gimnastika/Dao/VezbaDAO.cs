using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using Gimnastika.Entities;
using Gimnastika.Exceptions;

namespace Gimnastika.Dao
{
    public class VezbaDAO : DAO<Vezba>
    {
        // TODO: DAO klase bi verovatno trebalo da budu singletoni

        protected override string getInsertSQL()
        {
            return "INSERT INTO Vezbe " +
                "(Sprava, Naziv, GimnasticarId, Odbitak, Penalizacija, PraviloId) " +
                "VALUES (@Sprava, @Naziv, @GimnasticarId, @Odbitak, @Penalizacija, @PraviloId)";
        }

        protected override void addInsertParameters(SqlCeCommand cmd, Vezba v)
        {
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = v.Sprava;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Vezba.NAZIV_MAX_LENGTH).Value = v.Naziv;
            if (v.Gimnasticar != null)
                cmd.Parameters.Add("@GimnasticarId", SqlDbType.Int).Value = v.Gimnasticar.Id;
            else
                cmd.Parameters.Add("@GimnasticarId", SqlDbType.Int).Value = DBNull.Value;
            if (v.Odbitak != null)
                cmd.Parameters.Add("@Odbitak", SqlDbType.Real).Value = v.Odbitak;
            else
                cmd.Parameters.Add("@Odbitak", SqlDbType.Real).Value = DBNull.Value;
            if (v.Penalizacija != null)
                cmd.Parameters.Add("@Penalizacija", SqlDbType.Real).Value = v.Penalizacija;
            else
                cmd.Parameters.Add("@Penalizacija", SqlDbType.Real).Value = DBNull.Value;
            cmd.Parameters.Add("@PraviloId", SqlDbType.Int).Value = v.Pravilo.Id;
        }

        protected override string getInsertErrorMsg()
        {
            return "Neuspesan upis vezbe u bazu.";
        }

        protected override void insertDependents(Vezba vezba, SqlCeConnection conn,
            SqlCeTransaction tr)
        {
            insertDependents(vezba, conn, tr, getInsertErrorMsg());
        }

        private void insertDependents(Vezba vezba, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            foreach (ElementVezbe e in vezba.Elementi)
            {
                insertElement(e, conn, tr, errorMsg);
            }
        }

        private void insertElement(ElementVezbe e, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string insertElementSQL = "INSERT INTO ElementiVezbe " +
                "(VezbaId, RedBroj, BodujeSe, VezaSaPrethodnim, Zahtev, Odbitak, Penalizacija, " +
                "Naziv, EngleskiNaziv, TablicniElement, Grupa, Tezina, Broj, PodBroj) " +
                "VALUES (@VezbaId, @RedBroj, @BodujeSe, @VezaSaPrethodnim, @Zahtev, @Odbitak, @Penalizacija, " +
                "@Naziv, @EngleskiNaziv, @TablicniElement, @Grupa, @Tezina, @Broj, @PodBroj)";
            SqlCeCommand cmd = new SqlCeCommand(insertElementSQL, conn, tr);

            cmd.Parameters.Add("@VezbaId", SqlDbType.Int).Value = e.Vezba.Id;
            cmd.Parameters.Add("@RedBroj", SqlDbType.TinyInt).Value = e.RedBroj;
            cmd.Parameters.Add("@BodujeSe", SqlDbType.Bit).Value = e.BodujeSe;
            if (e.VezaSaPrethodnim != null)
                cmd.Parameters.Add("@VezaSaPrethodnim", SqlDbType.Real).Value = e.VezaSaPrethodnim;
            else
                cmd.Parameters.Add("@VezaSaPrethodnim", SqlDbType.Real).Value = DBNull.Value;
            if (e.Zahtev != null)
                cmd.Parameters.Add("@Zahtev", SqlDbType.Real).Value = e.Zahtev;
            else
                cmd.Parameters.Add("@Zahtev", SqlDbType.Real).Value = DBNull.Value;
            if (e.Odbitak != null)
                cmd.Parameters.Add("@Odbitak", SqlDbType.Real).Value = e.Odbitak;
            else
                cmd.Parameters.Add("@Odbitak", SqlDbType.Real).Value = DBNull.Value;
            if (e.Penalizacija != null)
                cmd.Parameters.Add("@Penalizacija", SqlDbType.Real).Value = e.Penalizacija;
            else
                cmd.Parameters.Add("@Penalizacija", SqlDbType.Real).Value = DBNull.Value;

            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = e.Naziv;
            cmd.Parameters.Add("@EngleskiNaziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = e.EngleskiNaziv;
            cmd.Parameters.Add("@TablicniElement", SqlDbType.Bit).Value = e.IsTablicniElement;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value = e.Grupa;
            cmd.Parameters.Add("@Tezina", SqlDbType.TinyInt).Value = e.Tezina;
            cmd.Parameters.Add("@Broj", SqlDbType.SmallInt).Value = e.Broj;
            cmd.Parameters.Add("@PodBroj", SqlDbType.TinyInt).Value = e.PodBroj;
            
            int recordsAffected = cmd.ExecuteNonQuery();
            if (recordsAffected != 1)
            {
                throw new DatabaseException(errorMsg);
            }
        }

        protected override string getUpdateSQL()
        {
            return "UPDATE Vezbe " +
                "SET Sprava = @Sprava, Naziv = @Naziv, GimnasticarId = @GimnasticarId, " +
                    "Odbitak = @Odbitak, Penalizacija = @Penalizacija, PraviloId = @PraviloId " +
                "WHERE Id = @Id";
        }

        protected override void addUpdateParameters(SqlCeCommand cmd, Vezba v)
        {
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = v.Sprava;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Vezba.NAZIV_MAX_LENGTH).Value = v.Naziv;
            if (v.Gimnasticar != null)
                cmd.Parameters.Add("@GimnasticarId", SqlDbType.Int).Value = v.Gimnasticar.Id;
            else
                cmd.Parameters.Add("@GimnasticarId", SqlDbType.Int).Value = DBNull.Value;
            if (v.Odbitak != null)
                cmd.Parameters.Add("@Odbitak", SqlDbType.Real).Value = v.Odbitak;
            else
                cmd.Parameters.Add("@Odbitak", SqlDbType.Real).Value = DBNull.Value;
            if (v.Penalizacija != null)
                cmd.Parameters.Add("@Penalizacija", SqlDbType.Real).Value = v.Penalizacija;
            else
                cmd.Parameters.Add("@Penalizacija", SqlDbType.Real).Value = DBNull.Value;
            cmd.Parameters.Add("@PraviloId", SqlDbType.Int).Value = v.Pravilo.Id;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = v.Id;
        }

        protected override string getUpdateErrorMsg()
        {
            return "Neuspesna promena vezbe u bazi.";
        }

        protected override void updateDependents(Vezba v, SqlCeConnection conn, SqlCeTransaction tr)
        {
            deleteDependents(v, conn, tr, getUpdateErrorMsg());
            insertDependents(v, conn, tr, getUpdateErrorMsg());
        }

        protected override string getTableName()
        {
            return "Vezbe";
        }

        protected override string getDeleteErrorMsg()
        {
            return "Neuspesno brisanje vezbe iz baze.";
        }

        protected override void deleteDependents(Vezba vezba, SqlCeConnection conn,
          SqlCeTransaction tr)
        {
            deleteDependents(vezba, conn, tr, getDeleteErrorMsg());
        }

        private void deleteDependents(Vezba vezba, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string deleteElementsSQL = "DELETE FROM ElementiVezbe " +
                "WHERE VezbaId = @VezbaId";
            SqlCeCommand cmd = new SqlCeCommand(deleteElementsSQL, conn, tr);
            cmd.Parameters.Add("@VezbaId", SqlDbType.Int).Value = vezba.Id;

            int recordsAffected = cmd.ExecuteNonQuery();
            // Ne proveravam recordsAffected, zato sto kada se delete obavlja u sklopu
            // update, tada se recordsAffected ne poredi sa trenutnim brojem elemenata
            // vec sa brojem elemenata pre promene vezbe (a on mi je nepoznat). 
            //if (recordsAffected != vezba.Elementi.Count)
            //{
            //    throw new DatabaseException(errorMsg);
            //}
        }

        protected override string getSelectAllSQL()
        {
            // NOTE: Upotreba LEFT JOIN-a omogucava da rezultat sadrzi i vezbe bez
            // elemenata. U vrsti vezbe bez elemenata, sva polja elementa su NULL
            return "SELECT V.Id, V.Sprava, V.Naziv, V.GimnasticarId, V.Odbitak AS VezbaOdbitak, V.Penalizacija AS VezbaPenalizacija, V.PraviloId, " +
                "E.RedBroj, E.BodujeSe, E.VezaSaPrethodnim, E.Zahtev, E.Odbitak AS ElementOdbitak, E.Penalizacija AS ElementPenalizacija, " +
                "E.Naziv AS NazivElementa, E.EngleskiNaziv, E.TablicniElement, " +
                "E.Grupa, E.Tezina, E.Broj, E.PodBroj, " +
                "G.Id AS GimnsticarId, G.Ime, G.Prezime " +
                "FROM Vezbe V " +
                "LEFT JOIN Gimnasticari G " +
                    "ON V.GimnasticarId = G.Id " +
                "LEFT JOIN ElementiVezbe E " +
                    "ON V.Id = E.VezbaId " +
                "ORDER BY V.Id, E.RedBroj";
        }

        protected override string getSelectByIdSQL()
        {
            return "SELECT V.Id, V.Sprava, V.Naziv, V.GimnasticarId, V.Odbitak AS VezbaOdbitak, V.Penalizacija AS VezbaPenalizacija, V.PraviloId, " +
                "E.RedBroj, E.BodujeSe, E.VezaSaPrethodnim, E.Zahtev, E.Odbitak AS ElementOdbitak, E.Penalizacija AS ElementPenalizacija, " +
                "E.Naziv AS NazivElementa, E.EngleskiNaziv, E.TablicniElement, " +
                "E.Grupa, E.Tezina, E.Broj, E.PodBroj, " +
                "G.Id AS GimnsticarId, G.Ime, G.Prezime " +
                "FROM Vezbe V " +
                "LEFT JOIN Gimnasticari G " +
                    "ON V.GimnasticarId = G.Id " +
                "LEFT JOIN ElementiVezbe E " +
                    "ON V.Id = E.VezbaId " +
                "WHERE V.Id = @Id";
        }

        protected override string getGetManyErrorMsg()
        {
            return "Neuspesno citanje vezbi iz baze.";
        }

        protected override string getGetOneErrorMsg()
        {
            return "Neuspesno citanje vezbe iz baze.";
        }

        protected override List<Vezba> loadAll(SqlCeDataReader rdr)
        {
            List<Vezba> result = new List<Vezba>();
            const int NO_ID = -2;
            int currId = NO_ID;
            Vezba vezba = null;
            while (rdr.Read())
            {
                if ((int)rdr["Id"] != currId)
                {
                    // prva vrsta nove vezbe
                    currId = (int)rdr["Id"];
                    vezba = loadVezba(rdr);
                    vezba.Gimnasticar = loadGimnasticar(rdr);
                    result.Add(vezba);
                }
                vezba.DodajElement(loadElementVezbe(rdr));
            }
            return result;
        }

        // rider je pozicioniran na prvu vrstu
        protected override Vezba load(SqlCeDataReader rdr)
        {
            Vezba result = null;
            bool prvaVrsta = true;
            do
            {
                if (prvaVrsta)
                {
                    prvaVrsta = false;
                    result = loadVezba(rdr);
                    result.Gimnasticar = loadGimnasticar(rdr);
                }
                result.DodajElement(loadElementVezbe(rdr));
            }
            while (rdr.Read());
            return result;
        }

        private Vezba loadVezba(SqlCeDataReader rdr)
        {
            Sprava sprava = (Sprava)(byte)(rdr["Sprava"]);
            string naziv = (string)rdr["Naziv"];
            Nullable<float> odbitak = null;
            if (!Convert.IsDBNull(rdr["VezbaOdbitak"]))
                odbitak = (float)rdr["VezbaOdbitak"];
            Nullable<float> penal = null;
            if (!Convert.IsDBNull(rdr["VezbaPenalizacija"]))
                penal = (float)rdr["VezbaPenalizacija"];
            Vezba vezba = new Vezba(sprava, naziv, odbitak, penal);
            vezba.Pravilo = new PraviloOceneVezbeDAO().getById((int)rdr["PraviloId"]);
            vezba.Id = (int)rdr["Id"];
            return vezba;
        }

        private ElementVezbe loadElementVezbe(SqlCeDataReader rdr)
        {
            if (Convert.IsDBNull(rdr["RedBroj"]))
            {
                // vezba bez elemenata
                return null;
            }
            bool bodujeSe = (bool)rdr["BodujeSe"];
            Nullable<float> veza = null;
            if (!Convert.IsDBNull(rdr["VezaSaPrethodnim"]))
                veza = (float)rdr["VezaSaPrethodnim"];
            Nullable<float> zahtev = null;
            if (!Convert.IsDBNull(rdr["Zahtev"]))
                zahtev = (float)rdr["Zahtev"];
            Nullable<float> odbitak = null;
            if (!Convert.IsDBNull(rdr["ElementOdbitak"]))
                odbitak = (float)rdr["ElementOdbitak"];
            Nullable<float> penalizacija = null;
            if (!Convert.IsDBNull(rdr["ElementPenalizacija"]))
                penalizacija = (float)rdr["ElementPenalizacija"];

            string naziv = (string)rdr["NazivElementa"];
            string engNaziv = (string)rdr["EngleskiNaziv"];
            bool isTablicniElement = (bool)rdr["TablicniElement"];
            GrupaElementa grupa = (GrupaElementa)(byte)rdr["Grupa"];
            TezinaElementa tezina = (TezinaElementa)(byte)rdr["Tezina"];
            short broj = (short)rdr["Broj"];
            byte podBroj = (byte)rdr["PodBroj"];

            return new ElementVezbe(naziv, engNaziv, isTablicniElement, grupa, tezina,
                broj, podBroj, bodujeSe, veza, zahtev, odbitak, penalizacija);
        }

        private Gimnasticar loadGimnasticar(SqlCeDataReader rdr)
        {
            if (Convert.IsDBNull(rdr["GimnasticarId"]))
                // vezba bez gimnasticara
                return null;

            string ime = (string)rdr["Ime"];
            string prezime = (string)rdr["Prezime"];
            Gimnasticar gimnasticar = new Gimnasticar(ime, prezime);
            gimnasticar.Id = (int)rdr["GimnasticarId"];
            return gimnasticar;
        }

        public bool postojiVezba(Sprava sprava, string naziv,
            Nullable<int> gimId)
        {
            // NOTE: SQL Server Compact ne dozvoljava poredjenje @GimnasticarId IS NULL
            //string selectBySpravaNazivGimnasticar = "SELECT * FROM Vezbe " +
            //    "WHERE Sprava = @Sprava AND Naziv = @Naziv " +
            //    "AND (GimnasticarId = @GimnasticarId OR @GimnasticarId IS NULL AND GimnasticarId IS NULL)";
            string selectBySpravaNazivGimnasticar;
            if (gimId != null)
            {
                selectBySpravaNazivGimnasticar = "SELECT * FROM Vezbe " +
                    "WHERE Sprava = @Sprava AND Naziv = @Naziv " +
                    "AND GimnasticarId = @GimnasticarId";
            }
            else
            {
                selectBySpravaNazivGimnasticar = "SELECT * FROM Vezbe " +
                    "WHERE Sprava = @Sprava AND Naziv = @Naziv ";
            }
            SqlCeCommand cmd = new SqlCeCommand(selectBySpravaNazivGimnasticar);
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = sprava;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Vezba.NAZIV_MAX_LENGTH).Value = naziv;
            if (gimId != null)
                cmd.Parameters.Add("@GimnasticarId", SqlDbType.Int).Value = gimId;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }
    }
}
