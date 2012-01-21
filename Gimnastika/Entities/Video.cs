using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using Gimnastika.Exceptions;

namespace Gimnastika.Entities
{
    public class Video : File
    {
        public Video()
        { 
        
        }

        public Video(string relFileNamePath)
            : base(relFileNamePath)
        { 
        
        }

        public void play()
        {
            if (Opcije.Instance.PlayerFileName == ""
            || !System.IO.File.Exists(Opcije.Instance.PlayerFileName)
            || Path.GetExtension(Opcije.Instance.PlayerFileName) != ".exe")
            {
                throw new VideoException("Specifikujte u opcijama program za pustanje videa.");
            }

            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            string fullName = appDir + "\\" + RelFileNamePath;
            if (!System.IO.File.Exists(fullName))
            {
                throw new VideoException("Datoteka " + fullName + " ne postoji.");
            }

            System.Diagnostics.Process pr = new System.Diagnostics.Process();
            pr.StartInfo.FileName = Opcije.Instance.PlayerFileName;
            pr.StartInfo.Arguments = "\"" + fullName + "\"";
            pr.StartInfo.WorkingDirectory = Application.ExecutablePath;
            bool error = false;
            try
            {
                pr.Start();
            }
            catch (InvalidOperationException)
            {
                // No file name was specified in the Process component's StartInfo.
                // -or- 
                // The ProcessStartInfo.UseShellExecute member of the StartInfo
                // property is true while ProcessStartInfo.RedirectStandardInput,
                // ProcessStartInfo.RedirectStandardOutput, or 
                // ProcessStartInfo.RedirectStandardError is true.
                error = true;
            }
            catch (Win32Exception)
            {
                // There was an error in opening the associated file.
                error = true;
            }
            if (error)
            {
                throw new VideoException("Greska prilikom pokretanja videa.");
            }
        }
    }
}
