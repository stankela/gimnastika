using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using Gimnastika.Entities;
using Gimnastika.Exceptions;

namespace Gimnastika.Dao
{
    public class PraviloOceneVezbeDAO : DAO<PraviloOceneVezbe>
    {
        protected override string getInsertSQL()
        {
            return "INSERT INTO PravilaOceneVezbe " +
                "(Naziv, BrojBodovanihElemenata, MaxIstaGrupa) " +
                "VALUES (@Naziv, @BrojBodovanihElemenata, @MaxIstaGrupa)";
        }

        protected override void addInsertParameters(SqlCeCommand cmd, PraviloOceneVezbe p)
        {
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, PraviloOceneVezbe.NAZIV_MAX_LENGTH).Value = p.Naziv;
            cmd.Parameters.Add("@BrojBodovanihElemenata", SqlDbType.Int).Value = p.BrojBodovanihElemenata;
            cmd.Parameters.Add("@MaxIstaGrupa", SqlDbType.Int).Value = p.MaxIstaGrupa;
        }

        protected override string getInsertErrorMsg()
        {
            return "Neuspesan upis pravila za ocenu vezbe u bazu.";
        }

        protected override void insertDependents(PraviloOceneVezbe pravilo,
            SqlCeConnection conn, SqlCeTransaction tr)
        {
            insertDependents(pravilo, conn, tr, getInsertErrorMsg());
        }

        private void insertDependents(PraviloOceneVezbe pravilo, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            foreach (PocetnaOcenaIzvedbe ocena in pravilo.PocetneOceneIzvedbe)
            {
                insertElement(pravilo.Id, ocena, conn, tr, errorMsg);
            }
        }

        private void insertElement(int praviloId, PocetnaOcenaIzvedbe ocena, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string insertElementSQL = "INSERT INTO PocetneOceneIzvedbe " +
                "(PraviloId, MinBrojElemenata, MaxBrojElemenata, PocetnaOcena) " +
                "VALUES (@PraviloId, @MinBrojElemenata, @MaxBrojElemenata, @PocetnaOcena)";
            SqlCeCommand cmd = new SqlCeCommand(insertElementSQL, conn, tr);

            cmd.Parameters.Add("@PraviloId", SqlDbType.Int).Value = praviloId;
            cmd.Parameters.Add("@MinBrojElemenata", SqlDbType.Int).Value = ocena.MinBrojElemenata;
            if (ocena.imaGornjuGranicu())
                cmd.Parameters.Add("@MaxBrojElemenata", SqlDbType.Int).Value = ocena.MaxBrojElemenata;
            else
                cmd.Parameters.Add("@MaxBrojElemenata", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@PocetnaOcena", SqlDbType.Real).Value = ocena.PocetnaOcena;

            int recordsAffected = cmd.ExecuteNonQuery();
            if (recordsAffected != 1)
            {
                throw new DatabaseException(errorMsg);
            }
        }

        protected override string getUpdateSQL()
        {
            return "UPDATE PravilaOceneVezbe " +
                "SET Naziv = @Naziv, BrojBodovanihElemenata = @BrojBodovanihElemenata, " +
                    "MaxIstaGrupa = @MaxIstaGrupa " +
                "WHERE Id = @Id";
        }

        protected override void addUpdateParameters(SqlCeCommand cmd, PraviloOceneVezbe pravilo)
        {
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, PraviloOceneVezbe.NAZIV_MAX_LENGTH).Value = pravilo.Naziv;
            cmd.Parameters.Add("@BrojBodovanihElemenata", SqlDbType.Int).Value = pravilo.BrojBodovanihElemenata;
            cmd.Parameters.Add("@MaxIstaGrupa", SqlDbType.Int).Value = pravilo.MaxIstaGrupa;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = pravilo.Id;
        }

        protected override string getUpdateErrorMsg()
        {
            return "Neuspesna promena pravila za ocenu vezbe u bazi.";
        }

        protected override void updateDependents(PraviloOceneVezbe pravilo,
            SqlCeConnection conn, SqlCeTransaction tr)
        {
            deleteDependents(pravilo, conn, tr, getUpdateErrorMsg());
            insertDependents(pravilo, conn, tr, getUpdateErrorMsg());
        }

        protected override string getTableName()
        {
            return "PravilaOceneVezbe";
        }

        protected override string getDeleteErrorMsg()
        {
            return "Neuspesno brisanje pravila za ocenu vezbe iz baze.";
        }

        protected override void deleteDependents(PraviloOceneVezbe pravilo, SqlCeConnection conn,
          SqlCeTransaction tr)
        {
            deleteDependents(pravilo, conn, tr, getDeleteErrorMsg());
        }

        private void deleteDependents(PraviloOceneVezbe pravilo, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string deleteElementsSQL = "DELETE FROM PocetneOceneIzvedbe " +
                "WHERE PraviloId = @PraviloId";
            SqlCeCommand cmd = new SqlCeCommand(deleteElementsSQL, conn, tr);
            cmd.Parameters.Add("@PraviloId", SqlDbType.Int).Value = pravilo.Id;

            int recordsAffected = cmd.ExecuteNonQuery();
        }
        protected override string getSelectAllSQL()
        {
            return "SELECT V.Id, V.Naziv, V.BrojBodovanihElemenata, V.MaxIstaGrupa, " +
                "I.MinBrojElemenata, I.MaxBrojElemenata, I.PocetnaOcena " +
                "FROM PravilaOceneVezbe V INNER JOIN PocetneOceneIzvedbe I " +
                "ON V.Id = I.PraviloId " +
                "ORDER BY V.Id";
        }

        protected override string getGetManyErrorMsg()
        {
            return "Neuspesno citanje pravila za ocenu vezbi iz baze.";
        }

        protected override string getGetOneErrorMsg()
        {
            return "Neuspesno citanje pravila za ocenu vezbe iz baze.";
        }

        protected override List<PraviloOceneVezbe> loadAll(SqlCeDataReader rdr)
        {
            List<PraviloOceneVezbe> result = new List<PraviloOceneVezbe>();
            const int NO_ID = -2;
            int currId = NO_ID;
            PraviloOceneVezbe pravilo = null;
            while (rdr.Read())
            {
                if ((int)rdr["Id"] != currId)
                {
                    // prva vrsta novog pravila
                    currId = (int)rdr["Id"];
                    if (pravilo != null)
                        pravilo.sortirajPocetneOceneIzvedbe();
                    pravilo = loadPravilo(rdr);
                    result.Add(pravilo);
                }
                pravilo.dodajPocetnuOcenuIzvedbe(loadElement(rdr));
            }

            // za slucaj da je ucitano samo jedno pravilo
            if (pravilo != null)
                pravilo.sortirajPocetneOceneIzvedbe();

            return result;
        }

        // rider je pozicioniran na prvu vrstu
        protected override PraviloOceneVezbe load(SqlCeDataReader rdr)
        {
            PraviloOceneVezbe result = null;
            bool prvaVrsta = true;
            do
            {
                if (prvaVrsta)
                {
                    prvaVrsta = false;
                    result = loadPravilo(rdr);
                }
                result.dodajPocetnuOcenuIzvedbe(loadElement(rdr));
            }
            while (rdr.Read());
            result.sortirajPocetneOceneIzvedbe();
            return result;
        }

        private PraviloOceneVezbe loadPravilo(SqlCeDataReader rdr)
        {
            string naziv = (string)rdr["Naziv"];
            int brojBodovanih = (int)rdr["BrojBodovanihElemenata"];
            int maxIstaGrupa = (int)rdr["MaxIstaGrupa"];
            PraviloOceneVezbe pravilo = new PraviloOceneVezbe(naziv, brojBodovanih, maxIstaGrupa);
            pravilo.Id = (int)rdr["Id"];
            return pravilo;
        }

        private PocetnaOcenaIzvedbe loadElement(SqlCeDataReader rdr)
        {
            int min = (int)rdr["MinBrojElemenata"];
            Nullable<int> max = null;
            if (!Convert.IsDBNull(rdr["MaxBrojElemenata"]))
                max = (int)rdr["MaxBrojElemenata"];
            float pocOcena = (float)rdr["PocetnaOcena"];
            if (max.HasValue)
                return new PocetnaOcenaIzvedbe(min, max.Value, pocOcena);
            else
                return new PocetnaOcenaIzvedbe(min, pocOcena);
        }

        protected override string getSelectByIdSQL()
        {
            return "SELECT V.Id, V.Naziv, V.BrojBodovanihElemenata, V.MaxIstaGrupa, " +
                "I.MinBrojElemenata, I.MaxBrojElemenata, I.PocetnaOcena " +
                "FROM PravilaOceneVezbe V INNER JOIN PocetneOceneIzvedbe I " +
                "ON V.Id = I.PraviloId " +
                "WHERE V.Id = @Id";
        }

        // can throw DatabaseException
        public bool postojiPravilo(string naziv)
        {
            string selectByNaziv = "SELECT * FROM PravilaOceneVezbe " +
                "WHERE Naziv = @Naziv";
            SqlCeCommand cmd = new SqlCeCommand(selectByNaziv);
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, PraviloOceneVezbe.NAZIV_MAX_LENGTH).Value = naziv;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }

    }
}
