using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace Gimnastika
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Language.SetKeyboardLanguage(Language.acKeyboardLanguage.hklSerbianLatin);
            //		Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("sr-Latn-CS");
            //      ili
            //		Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("sr-Cyrl-CS");
            //		Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Application.ApplicationExit += App_ApplicationExit;
            Application.ThreadException += App_ThreadException;

            // TODO: Razmisli da li treba ovo
            // Ova naredba uvek prikazuje Windowsovu poruku o gresci (bez obzira da li
            // je prisutan handler za Application.ThreadException)
            Application.SetUnhandledExceptionMode(
              UnhandledExceptionMode.ThrowException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new DatabaseUpdater().updateDatabase();


      //      Application.Run(new MainForm());
            SingleInstanceApplication.Application.Run(args);
        }

        private static void App_ApplicationExit(object sender, EventArgs e)
        {
            // Because ApplicationExit is a static event, you must detach any event
            // handlers attached to this event in the ApplicationExit event handler
            // itself. If you do not detach these handlers, they will remain attached
            // to the event and continue to consume memory.

            // Detach all handlers from static events
            Application.ApplicationExit -= App_ApplicationExit;
        }

        private static void App_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Does user want to save or quit?
            string msg = "Dogodila se nepredvidjena greska u programu:\n\n" +
                          "\t" + e.Exception.Message + "\n\n" +
                          "Ukoliko ste trenutno unosili neke izmene, moguce je da su " +
                          "izmene izgubljene. " +
                          "Mozete da nastavite sa programom i pokusate da snimite " +
                          "izmene. " +
                          "Nakon toga trebalo bi da zatvorite i ponovo pokrenite " +
                          "program, jer je program trenutno u nekonzistentnom stanju " +
                          "i moze da daje nepredvidjene rezultate. " + 
                          "Kontaktirajte autora programa u vezi sa ovom greskom.\n\n" +
                          "Da li zelite da nastavite sa programom i pokusate " +
                          "da snimite izmene? (Ukoliko izaberete 'Ne' program ce " +
                          "odmah biti zatvoren.)";
            DialogResult res = MessageBox.Show(msg, "Nepredvidjena greska", MessageBoxButtons.YesNo);

            // Returning continues the application
            if (res == DialogResult.Yes) return;

            Application.Exit();
        }
    }
}