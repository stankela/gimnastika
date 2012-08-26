using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Gimnastika.Domain
{
    public class File : DomainObject
    {
        public static readonly int FILE_NAME_MAX_LENGTH = 256;

        private string relFileNamePath;
        public virtual string RelFileNamePath
        {
            get { return relFileNamePath; }
            set { relFileNamePath = value; }
        }

        public virtual string FileName
        {
            get { return Path.GetFileName(relFileNamePath); }
        }

        public File()
        { 
        
        }

        public File(string relFileNamePath)
        {
            this.relFileNamePath = relFileNamePath;
        }

        protected override void deepCopy(DomainObject domainObject)
        {
            base.deepCopy(domainObject);

            File file = (File)domainObject;
            relFileNamePath = file.relFileNamePath;
        }
    }
}
