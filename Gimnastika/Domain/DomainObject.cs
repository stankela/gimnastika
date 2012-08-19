using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using Gimnastika.Exceptions;

namespace Gimnastika.Domain
{
    public abstract class DomainObject
    {
        private int id;
        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }


        public virtual void validate(Notification notification)
        {
            throw new Exception("Derived class should implement this method.");
        }
        
        private static Dictionary<int, DomainObject> clonedObjects =
            new Dictionary<int, DomainObject>();
        private static List<TypeAsocijacijaPair> region = null;

        // deep copies the whole graph, or only the types in region
        public virtual DomainObject Clone()
        {
            // gets the unique ID (within an AppDomain) for an object
            int hash = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this); 

            DomainObject result;
            if (clonedObjects.Count == 0)
            {
                // pocetak kloniranja (koren hijerarhije)
                result = (DomainObject)
                    this.GetType().GetConstructor(new Type[0]/*prazan niz*/).Invoke(null);
                clonedObjects.Add(hash, result);
                result.deepCopy(this);
                // kraj kloniranja

                // priprema za sledece kloniranje
                clonedObjects.Clear();
                region = null;
            }
            else
            {
                // kloniranje nekog objekta koji nije koren
                if (!clonedObjects.ContainsKey(hash))
                {
                    // prvi put se nailazi na dati objekt
                    result = (DomainObject)
                        this.GetType().GetConstructor(new Type[0]).Invoke(null);
                    clonedObjects.Add(hash, result);
                    result.deepCopy(this);
                }
                else
                {
                    result = clonedObjects[hash];
                }
            }
            return result;
        }

        // deep copies part of the graph
        public virtual DomainObject Clone(TypeAsocijacijaPair[] reg)
        {
            if (reg != null && reg.Length > 0)
            {
                region = new List<TypeAsocijacijaPair>(reg);
            }
            return Clone();
        }

        protected virtual void deepCopy(DomainObject domainObject)
        {
            id = domainObject.id;
        }

        protected static bool shouldClone(TypeAsocijacijaPair typeAsocijacija)
        {
            if (region == null)
                return true;
            foreach (TypeAsocijacijaPair ta in region)
            {
                if (ta.Type == typeAsocijacija.Type
                && ta.Asocijacija == typeAsocijacija.Asocijacija)
                    return true;
            }
            return false;
        }

        // shallow copy
        public virtual DomainObject Copy()
        {
            return (DomainObject)this.MemberwiseClone();
        }
    }

    public class TypeAsocijacijaPair
    {
        private Type type;
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        private string asocijacija;
        public string Asocijacija
        {
            get { return asocijacija; }
            set { asocijacija = value; }
        }

        public TypeAsocijacijaPair(Type type)
        {
            this.type = type;
            this.asocijacija = String.Empty;
        }

        public TypeAsocijacijaPair(Type type, string asocijacija)
        {
            this.type = type;
            this.asocijacija = asocijacija;
        }
    }
}
