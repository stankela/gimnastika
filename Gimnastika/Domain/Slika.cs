using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Gimnastika.Domain
{
    public class Slika : File
    {
        private bool podrazumevana = false;
        public virtual bool Podrazumevana
        {
            get { return podrazumevana; }
            set { podrazumevana = value; }
        }

        private byte procenatRedukcije = 100;
        public virtual byte ProcenatRedukcije
        {
            get { return procenatRedukcije; }
            set
            {
                if (value >= 50 && value <= 100)
                    procenatRedukcije = value;
                else
                    throw new ArgumentException();
            }
        }

        private Image image = null;
        public virtual Image Image
        {
            get 
            {
                if (image == null)
                    image = loadImage();
                return image; 
            }
        }

        public Slika()
        { 
        
        }

        public Slika(string relFileNamePath, bool podrazumevana, byte procenatRedukcije)
            : base(relFileNamePath)
        {
            this.podrazumevana = podrazumevana;
            this.procenatRedukcije = procenatRedukcije;
        }

        private Image loadImage()
        {
            Image result = null;
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            string fullName = appDir + "\\" + this.RelFileNamePath;
            try
            {
                result = Image.FromFile(fullName);
                return result;
            }
            catch (OutOfMemoryException)
            {
                // The file does not have a valid image format.
                // -or-
                // GDI+ does not support the pixel format of the file.
                return null;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

    }
}
