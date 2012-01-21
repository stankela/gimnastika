using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using Gimnastika.Entities;
using Gimnastika.Exceptions;

namespace Gimnastika.Dao
{
    public class ElementDAO : DAO<Element>
    {
        protected override string getInsertSQL()
        {
            return "INSERT INTO Elementi " +
                "(Sprava, Naziv, EngleskiNaziv, NazivPoGimnasticaru, TablicniElement, Grupa, Tezina, Broj, PodBroj, ParentId) " +
                "VALUES (@Sprava, @Naziv, @NazivPoGimnasticaru, @EngleskiNaziv, @TablicniElement, @Grupa, @Tezina, @Broj, @PodBroj, @ParentId)";
        }

        protected override void addInsertParameters(SqlCeCommand cmd, Element e)
        {
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = e.Sprava;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = e.Naziv;
            cmd.Parameters.Add("@EngleskiNaziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = e.EngleskiNaziv;
            cmd.Parameters.Add("@NazivPoGimnasticaru", SqlDbType.NVarChar, Element.NAZIV_GIM_MAX_LENGTH).Value = e.NazivPoGimnasticaru;
            cmd.Parameters.Add("@TablicniElement", SqlDbType.Bit).Value = e.IsTablicniElement;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value = e.Grupa;
            cmd.Parameters.Add("@Tezina", SqlDbType.TinyInt).Value = e.Tezina;
            cmd.Parameters.Add("@Broj", SqlDbType.SmallInt).Value = e.Broj;
            cmd.Parameters.Add("@PodBroj", SqlDbType.TinyInt).Value = e.PodBroj;
            if (e.Parent != null)
                cmd.Parameters.Add("@ParentId", SqlDbType.Int).Value = e.Parent.Id;
            else
                cmd.Parameters.Add("@ParentId", SqlDbType.Int).Value = DBNull.Value;
        }

        protected override string getInsertErrorMsg()
        {
            return "Neuspesan upis elementa u bazu.";
        }

        protected override void insertDependents(Element element, SqlCeConnection conn,
            SqlCeTransaction tr)
        {
            insertVideos(element, conn, tr, getInsertErrorMsg());
            insertSlike(element, conn, tr, getInsertErrorMsg());
        }

        private void insertVideos(Element element, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            foreach (Video video in element.VideoKlipovi)
            {
                insertVideo(video, element.Id, conn, tr, errorMsg);
            }
        }

        private void insertVideo(Video video, int elementId, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string insertVideoSQL = "INSERT INTO Video " +
                "(RelFileNamePath, ElementId) " +
                "VALUES (@RelFileNamePath, @ElementId)";
            SqlCeCommand cmd = new SqlCeCommand(insertVideoSQL, conn, tr);

            cmd.Parameters.Add("@RelFileNamePath", SqlDbType.NVarChar, Video.FILE_NAME_MAX_LENGTH).Value = video.RelFileNamePath;
            cmd.Parameters.Add("@ElementId", SqlDbType.Int).Value = elementId;

            int recordsAffected = cmd.ExecuteNonQuery();
            if (recordsAffected != 1)
            {
                throw new DatabaseException(errorMsg);
            }
        }

        private void insertSlike(Element element, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            foreach (Slika slika in element.Slike)
            {
                insertSlika(slika, element.Id, conn, tr, errorMsg);
            }
        }

        private void insertSlika(Slika slika, int elementId, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string insertSlikaSQL = "INSERT INTO Slike " +
                "(RelFileNamePath, Podrazumevana, ProcenatRedukcije, ElementId) " +
                "VALUES (@RelFileNamePath, @Podrazumevana, @ProcenatRedukcije, @ElementId)";
            SqlCeCommand cmd = new SqlCeCommand(insertSlikaSQL, conn, tr);

            cmd.Parameters.Add("@RelFileNamePath", SqlDbType.NVarChar, Video.FILE_NAME_MAX_LENGTH).Value = slika.RelFileNamePath;
            cmd.Parameters.Add("@Podrazumevana", SqlDbType.Bit).Value = slika.Podrazumevana;
            cmd.Parameters.Add("@ProcenatRedukcije", SqlDbType.TinyInt).Value = slika.ProcenatRedukcije;
            cmd.Parameters.Add("@ElementId", SqlDbType.Int).Value = elementId;

            int recordsAffected = cmd.ExecuteNonQuery();
            if (recordsAffected != 1)
            {
                throw new DatabaseException(errorMsg);
            }
        }

        protected override void persistDetails(Element element, Element original,
            SqlCeConnection conn, SqlCeTransaction tr)
        {
            if (!element.isVarijanta())
                persistVarijante(element, original, conn, tr);
        }

        private void persistVarijante(Element element, Element original, 
            SqlCeConnection conn, SqlCeTransaction tr)
        {
            List<Element> added = new List<Element>();
            List<Element> updated = new List<Element>();
            List<Element> deleted = new List<Element>();
            diffVarijante(element, original, added, updated, deleted);
            foreach (Element e in deleted)
            {
                // TODO: Umesto 0 bi trebalo uvesti PLACEHOLDER_ID
                if (e.Id != 0)
                    deleteVarijanta(e, conn, tr);
            }
            foreach (Element e in updated)
            {
                if (e.Id != 0)
                    updateVarijanta(e, conn, tr);
                else
                    insertVarijanta(e, conn, tr);
            }
            foreach (Element e in added)
            {
                if (e.Id != 0)
                    updateVarijanta(e, conn, tr);
                else
                    insertVarijanta(e, conn, tr);
            }
        }

        private void diffVarijante(Element current, Element original,
            List<Element> added, List<Element> updated, List<Element> deleted)
        {
            // kada se dodaje novi element original je null
            if (original == null)
            {
                foreach (Element e in current.Varijante)
                    added.Add(e);
            }
            else
            {
                foreach (Element e in current.Varijante)
                {
                    if (!findElement(original.Varijante, e))
                        added.Add(e);
                    else
                        updated.Add(e);
                }
                foreach (Element e in original.Varijante)
                {
                    if (!findElement(current.Varijante, e))
                        deleted.Add(e);
                }
            }
        }

        private bool findElement(List<Element> elementi, Element e)
        {
            if (elementi.IndexOf(e) != -1)
                return true;
            // dodatna provera za slucaj da postoje dve instance istog elementa
            foreach (Element e2 in elementi)
            {
                if (e2.Id == e.Id)
                    return true;
            }
            return false;
        }

        private void insertVarijanta(Element e, SqlCeConnection conn, SqlCeTransaction tr)
        {
            SqlCeCommand cmd = new SqlCeCommand(getInsertSQL(), conn, tr);
            addInsertParameters(cmd, e);

            int recordsAffected = cmd.ExecuteNonQuery(); // can throw InvalidOperationException
            if (recordsAffected != 1)
            {
                // konkurentna greska
                throw new DatabaseException();
            }

            string sqlGetId = "SELECT @@IDENTITY";
            SqlCeCommand cmd2 = new SqlCeCommand(sqlGetId, conn, tr);
            e.Id = Convert.ToInt32(cmd2.ExecuteScalar()); // can throw InvalidOperationException
            insertDependents(e, conn, tr); // can throw DatabaseException
        }

        private void updateVarijanta(Element e, SqlCeConnection conn, SqlCeTransaction tr)
        {
            SqlCeCommand cmd = new SqlCeCommand(getUpdateSQL(), conn, tr);
            addUpdateParameters(cmd, e);
            int recordsAffected = cmd.ExecuteNonQuery();
            if (recordsAffected != 1)
            {
                throw new DatabaseException(getUpdateErrorMsg());
            }
            updateDependents(e, conn, tr);
        }

        private void deleteVarijanta(Element e, SqlCeConnection conn, SqlCeTransaction tr)
        {
            string deleteVarijanta = "DELETE FROM Elementi " +
                "WHERE Id = @Id";
            SqlCeCommand cmd = new SqlCeCommand(deleteVarijanta, conn, tr);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = e.Id;

            int recordsAffected = cmd.ExecuteNonQuery();
            if (recordsAffected != 1)
            {
                throw new DatabaseException();
            }
            deleteDependents(e, conn, tr);
        }

        protected override string getUpdateSQL()
        {
            return "UPDATE Elementi " +
                "SET Sprava = @Sprava, Naziv = @Naziv, EngleskiNaziv = @EngleskiNaziv, NazivPoGimnasticaru = @NazivPoGimnasticaru, TablicniElement = @TablicniElement, Grupa = @Grupa, Tezina = @Tezina, Broj = @Broj, PodBroj = @PodBroj, ParentId = @ParentId " +
                "WHERE Id = @Id";
        }

        protected override void addUpdateParameters(SqlCeCommand cmd, Element e)
        {
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = e.Sprava;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = e.Naziv;
            cmd.Parameters.Add("@EngleskiNaziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = e.EngleskiNaziv;
            cmd.Parameters.Add("@NazivPoGimnasticaru", SqlDbType.NVarChar, Element.NAZIV_GIM_MAX_LENGTH).Value = e.NazivPoGimnasticaru;
            cmd.Parameters.Add("@TablicniElement", SqlDbType.Bit).Value = e.IsTablicniElement;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value = e.Grupa;
            cmd.Parameters.Add("@Tezina", SqlDbType.TinyInt).Value = e.Tezina;
            cmd.Parameters.Add("@Broj", SqlDbType.SmallInt).Value = e.Broj;
            cmd.Parameters.Add("@PodBroj", SqlDbType.TinyInt).Value = e.PodBroj;
            if (e.Parent != null)
                cmd.Parameters.Add("@ParentId", SqlDbType.Int).Value = e.Parent.Id;
            else
                cmd.Parameters.Add("@ParentId", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = e.Id;
        }

        protected override string getUpdateErrorMsg()
        {
            return "Neuspesna promena elementa u bazi.";
        }

        protected override void updateDependents(Element element, SqlCeConnection conn, SqlCeTransaction tr)
        {
            deleteVideos(element, conn, tr, getUpdateErrorMsg());
            insertVideos(element, conn, tr, getUpdateErrorMsg());

            deleteSlike(element, conn, tr, getUpdateErrorMsg());
            insertSlike(element, conn, tr, getUpdateErrorMsg());
        }

        protected override string getTableName()
        {
            return "Elementi";
        }

        protected override string getDeleteErrorMsg()
        {
            return "Neuspesno brisanje elementa iz baze.";
        }

        protected override void deleteDependents(Element element, SqlCeConnection conn,
            SqlCeTransaction tr)
        {
            deleteVideos(element, conn, tr, getDeleteErrorMsg());
            deleteSlike(element, conn, tr, getUpdateErrorMsg());
        }

        private void deleteVideos(Element element, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string deleteVideosSQL = "DELETE FROM Video " +
                "WHERE ElementId = @ElementId";
            SqlCeCommand cmd = new SqlCeCommand(deleteVideosSQL, conn, tr);
            cmd.Parameters.Add("@ElementId", SqlDbType.Int).Value = element.Id;

            int recordsAffected = cmd.ExecuteNonQuery();
        }

        private void deleteSlike(Element element, SqlCeConnection conn,
            SqlCeTransaction tr, string errorMsg)
        {
            string deleteSlikeSQL = "DELETE FROM Slike " +
                "WHERE ElementId = @ElementId";
            SqlCeCommand cmd = new SqlCeCommand(deleteSlikeSQL, conn, tr);
            cmd.Parameters.Add("@ElementId", SqlDbType.Int).Value = element.Id;

            int recordsAffected = cmd.ExecuteNonQuery();
        }

        protected override void deleteDetails(Element element, SqlCeConnection conn,
            SqlCeTransaction tr)
        {
            if (!element.isVarijanta())
            {
                foreach (Element e in element.Varijante)
                {
                    if (e.Id != 0)
                        deleteVarijanta(e, conn, tr);
                }
            }
        }

        protected override string getSelectAllSQL()
        {
            return "SELECT E.Id, E.Sprava, E.Naziv, E.EngleskiNaziv, E.TablicniElement, E.NazivPoGimnasticaru, " +
                "E.Grupa, E.Tezina, E.Broj, E.PodBroj, E.ParentId, " +
                "V.Id AS VideoId, V.RelFileNamePath AS VideoRelFileNamePath, " +
                "S.Id AS SlikaId, S.RelFileNamePath AS SlikaRelFileNamePath, S.Podrazumevana, S.ProcenatRedukcije " +
                "FROM Elementi E " +
                "LEFT JOIN Video V " +
                    "ON E.Id = V.ElementId " +
                "LEFT JOIN Slike S " +
                    "ON E.Id = S.ElementId " +
                "WHERE E.PodBroj = 0 " +
                "ORDER BY E.Id, V.Id, S.Id";
        }

        protected override string getSelectByIdSQL()
        {
            return "SELECT E.Id, E.Sprava, E.Naziv, E.EngleskiNaziv, E.TablicniElement, E.NazivPoGimnasticaru, " +
                "E.Grupa, E.Tezina, E.Broj, E.PodBroj, E.ParentId, " +
                "V.Id AS VideoId, V.RelFileNamePath AS VideoRelFileNamePath, " +
                "S.Id AS SlikaId, S.RelFileNamePath AS SlikaRelFileNamePath, S.Podrazumevana, S.ProcenatRedukcije " +
                "FROM Elementi E " +
                "LEFT JOIN Video V " +
                    "ON E.Id = V.ElementId " +
                "LEFT JOIN Slike S " +
                    "ON E.Id = S.ElementId " +
                "WHERE E.Id = @Id " +
                "ORDER BY V.Id, S.Id";
        }

        protected override string getGetManyErrorMsg()
        {
            return "Neuspesno citanje elemenata iz baze.";
        }

        protected override string getGetOneErrorMsg()
        {
            return "Neuspesno citanje elementa iz baze.";
        }

        protected override List<Element> loadAll(SqlCeDataReader rdr)
        {
            // vrste su sortirane po elementu, videu i slici (tim redosledom)
            List<Element> result = new List<Element>();

            const int NO_ID = -2;
            int currElementId = NO_ID;
            int currVideoId = NO_ID;

            bool imaVideo = false;
            bool imaSlika = false;
            bool dodajSliku = true;

            Element element = null;
            while (rdr.Read())
            {
                if ((int)rdr["Id"] != currElementId)
                {
                    // prva vrsta novog elementa
                    currElementId = (int)rdr["Id"];
                    element = loadElement(rdr);
                    result.Add(element);
                    currVideoId = NO_ID;   // resetuj pokazivac trenutnog videa
                    imaVideo = !Convert.IsDBNull(rdr["VideoId"]);
                    imaSlika = !Convert.IsDBNull(rdr["SlikaId"]);
                    dodajSliku = true;
                }
                if (imaVideo && ((int)rdr["VideoId"] != currVideoId))
                {
                    // nova vrsta videa za dati element
                    element.dodajVideo(loadVideo(rdr));
                    if (currVideoId != NO_ID)
                    {
                        // nova vrsta videa nije prva za dati element, sto znaci
                        // da su sve slike i varijante za dati element dodate
                        dodajSliku = false;
                    }
                    currVideoId = (int)rdr["VideoId"];
                }
                if (dodajSliku && imaSlika)
                {
                    element.dodajSliku(loadSlika(rdr));
                }
            }
            return result;
        }

        // rider je pozicioniran na prvu vrstu
        protected override Element load(SqlCeDataReader rdr)
        {
            // vrste su sortirane po elementu, videu i slici (tim redosledom)
            Element result = null;

            const int NO_ID = -2;
            bool prvaVrsta = true;
            int currVideoId = NO_ID;

            bool imaVideo = false;
            bool imaSlika = false;
            bool dodajSliku = true;

            do
            {
                if (prvaVrsta)
                {
                    prvaVrsta = false;
                    result = loadElement(rdr);
                    imaVideo = !Convert.IsDBNull(rdr["VideoId"]);
                    imaSlika = !Convert.IsDBNull(rdr["SlikaId"]);
                    dodajSliku = true;
                }
                if (imaVideo && ((int)rdr["VideoId"] != currVideoId))
                {
                    // nova vrsta videa za dati element
                    result.dodajVideo(loadVideo(rdr));
                    if (currVideoId != NO_ID)
                    {
                        // nova vrsta videa nije prva za dati element, sto znaci
                        // da su sve slike za dati element dodate
                        dodajSliku = false;
                    }
                    currVideoId = (int)rdr["VideoId"];
                }
                if (dodajSliku && imaSlika)
                {
                    result.dodajSliku(loadSlika(rdr));
                }
            }
            while (rdr.Read());
            return result;
        }

        private Element loadElement(SqlCeDataReader rdr)
        {
            Element element;
            string naziv = (string)rdr["Naziv"];
            string engNaziv = "";
            if (!Convert.IsDBNull(rdr["EngleskiNaziv"]))
                engNaziv = (string)rdr["EngleskiNaziv"];
            string nazivPoGim = "";
            if (!Convert.IsDBNull(rdr["NazivPoGimnasticaru"]))
                nazivPoGim = (string)rdr["NazivPoGimnasticaru"];
            Sprava sprava = (Sprava)(byte)(rdr["Sprava"]);
            bool isTablicniElement = (bool)rdr["TablicniElement"];
            if (!isTablicniElement)
            {
                element = new Element(naziv, engNaziv, nazivPoGim, sprava);
            }
            else
            {
                GrupaElementa grupa = (GrupaElementa)(byte)rdr["Grupa"];
                TezinaElementa tezina = (TezinaElementa)(byte)rdr["Tezina"];
                short broj = (short)rdr["Broj"];
                byte podBroj = (byte)rdr["PodBroj"];
                element = new Element(naziv, engNaziv, nazivPoGim, sprava, grupa, tezina, broj, podBroj);
            }
            Nullable<int> parentId = null;
            if (!Convert.IsDBNull(rdr["ParentId"]))
                parentId = (int)rdr["ParentId"];
            element.ParentId = parentId;    // lazy load
            element.Id = (int)rdr["Id"];
            return element;
        }

        private Video loadVideo(SqlCeDataReader rdr)
        {
            if (Convert.IsDBNull(rdr["VideoId"]))
                // element bez videa
                return null;
            Video result;
            string relFileNamePath = (string)rdr["VideoRelFileNamePath"];
            result = new Video(relFileNamePath);
            result.Id = (int)rdr["VideoId"];
            return result;
        }

        private Slika loadSlika(SqlCeDataReader rdr)
        {
            if (Convert.IsDBNull(rdr["SlikaId"]))
                return null;
            Slika result;
            string relFileNamePath = (string)rdr["SlikaRelFileNamePath"];
            bool podrazumevana = (bool)rdr["Podrazumevana"];
            byte procenatRedukcije = (byte)rdr["ProcenatRedukcije"];
            result = new Slika(relFileNamePath, podrazumevana, procenatRedukcije);
            result.Id = (int)rdr["SlikaId"];
            return result;
        }

        // can throw DatabaseException
        public bool postojiElement(Sprava sprava, string naziv)
        {
            string selectBySpravaNaziv = "SELECT * FROM Elementi " +
                "WHERE Sprava = @Sprava AND Naziv = @Naziv";
            SqlCeCommand cmd = new SqlCeCommand(selectBySpravaNaziv);
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = sprava;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = naziv;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }

        // can throw DatabaseException
        public bool postojiElementGim(Sprava sprava, string nazivPoGim)
        {
            if (nazivPoGim == "")
                return true;

            string selectBySpravaNazivGim = "SELECT * FROM Elementi " +
                "WHERE Sprava = @Sprava AND NazivPoGimnasticaru = @NazivPoGimnasticaru";
            SqlCeCommand cmd = new SqlCeCommand(selectBySpravaNazivGim);
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = sprava;
            cmd.Parameters.Add("@NazivPoGimnasticaru", SqlDbType.NVarChar, Element.NAZIV_GIM_MAX_LENGTH).Value = nazivPoGim;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }

        // can throw DatabaseException
        public bool postojiElementEng(Sprava sprava, string engNaziv)
        {
            if (engNaziv == "")
                return true;

            string selectBySpravaEngNaziv = "SELECT * FROM Elementi " +
                "WHERE Sprava = @Sprava AND EngleskiNaziv = @EngleskiNaziv";
            SqlCeCommand cmd = new SqlCeCommand(selectBySpravaEngNaziv);
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = sprava;
            cmd.Parameters.Add("@EngleskiNaziv", SqlDbType.NVarChar, Element.NAZIV_MAX_LENGTH).Value = engNaziv;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }

        // can throw DatabaseException
        public bool postojiElement(Sprava sprava, GrupaElementa grupa,
            short broj, byte podBroj)
        {
            string selectByBroj = "SELECT * FROM Elementi " +
                "WHERE Sprava = @Sprava AND Grupa = @Grupa AND Broj = @Broj " +
                    "AND PodBroj = @PodBroj";
            SqlCeCommand cmd = new SqlCeCommand(selectByBroj);
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = sprava;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value = grupa;
            cmd.Parameters.Add("@Broj", SqlDbType.SmallInt).Value = broj;
            cmd.Parameters.Add("@PodBroj", SqlDbType.TinyInt).Value = podBroj;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }

        // can throw DatabaseException
        public List<Element> getVarijante(int parentId)
        {
            string selectByParentId = "SELECT E.Id, E.Sprava, E.Naziv, E.EngleskiNaziv, E.NazivPoGimnasticaru, E.TablicniElement, " +
                "E.Grupa, E.Tezina, E.Broj, E.PodBroj, E.ParentId, " +
                "V.Id AS VideoId, V.RelFileNamePath AS VideoRelFileNamePath, " +
                "S.Id AS SlikaId, S.RelFileNamePath AS SlikaRelFileNamePath, S.Podrazumevana, S.ProcenatRedukcije " +
                "FROM Elementi E " +
                "LEFT JOIN Video V " +
                    "ON E.Id = V.ElementId " +
                "LEFT JOIN Slike S " +
                    "ON E.Id = S.ElementId " +
                "WHERE E.ParentId = @ParentId " +
                "ORDER BY E.PodBroj";

            SqlCeCommand cmd = new SqlCeCommand(selectByParentId);
            cmd.Parameters.Add("@ParentId", SqlDbType.Int).Value = parentId;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetManyErrorMsg());
            List<Element> result = loadAll(rdr);
            rdr.Close();
            return result;
        }

        // can throw DatabaseException
        public List<Element> getElementsBySprava(Sprava sprava)
        {
            string selectBySprava = "SELECT E.Id, E.Sprava, E.Naziv, E.EngleskiNaziv, E.NazivPoGimnasticaru, E.TablicniElement, " +
                "E.Grupa, E.Tezina, E.Broj, E.PodBroj, E.ParentId, " +
                "V.Id AS VideoId, V.RelFileNamePath AS VideoRelFileNamePath, " +
                "S.Id AS SlikaId, S.RelFileNamePath AS SlikaRelFileNamePath, S.Podrazumevana, S.ProcenatRedukcije " +
                "FROM Elementi E " +
                "LEFT JOIN Video V " +
                    "ON E.Id = V.ElementId " +
                "LEFT JOIN Slike S " +
                    "ON E.Id = S.ElementId " +
                "WHERE E.Sprava = @Sprava AND E.PodBroj = 0 " +
                "ORDER BY E.Id, V.Id, S.Id";

            SqlCeCommand cmd = new SqlCeCommand(selectBySprava);
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = sprava;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            List<Element> result = loadAll(rdr);
            rdr.Close();
            return result;
        }
    }
}
