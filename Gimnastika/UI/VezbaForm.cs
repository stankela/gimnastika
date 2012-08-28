using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Report;

namespace Gimnastika.UI
{
    public partial class VezbaForm : Form
    {
        private int newCount = 0;

        public VezbaForm()
        {
            InitializeComponent();

            Text = "Vezbe";
            tabControl1.TabPages.Clear();
            panelTab.Visible = false;
            this.IsMdiContainer = true;
            SetMdiClient();     // NOTE: Kada je prozor Mdi kontejner, on sadrzi jos
                                // jednu dodatnu kontrolu tipa MdiClient koja
                                // upravlja Mdi decom. Ova kontrola zauzima
                                // client oblast Mdi parenta, i u stvari ona sadrzi
                                // svu Mdi decu (dakle Mdi parent ne sadrzi Mdi decu).
                                // Kontrola tipa MdiClient ima spustenu spoljnu ivicu
                                // i to ne moze da se izmeni iz NET koda zato sto
                                // MdiClient nema svojstvo FormBorderStyle. Zato je
                                // potrebno koristiti Win32 pozive.
        }

        private void SetMdiClient()
        {
            MdiClient mdiClient = getMdiClient();

            // There is no BorderStyle property exposed by the MdiClient class,
            // but this can be controlled by Win32 functions. A Win32 ExStyle
            // of WS_EX_CLIENTEDGE is equivalent to a Fixed3D border and a
            // Style of WS_BORDER is equivalent to a FixedSingle border.

            // This code is inspired Jason Dori's article:
            // "Adding designable borders to user controls".
            // http://www.codeproject.com/cs/miscctrl/CsAddingBorders.asp

            // Get styles using Win32 calls
            int style = NativeMethods.GetWindowLong(mdiClient.Handle, (int)Win32.GetWindowLongIndex.GWL_STYLE);
            int exStyle = NativeMethods.GetWindowLong(mdiClient.Handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE);

            style &= ~((int)Win32.WindowStyles.WS_BORDER);
            exStyle &= ~((int)Win32.WindowExStyles.WS_EX_CLIENTEDGE);

            // Set the styles using Win32 calls
            NativeMethods.SetWindowLong(mdiClient.Handle, (int)Win32.GetWindowLongIndex.GWL_STYLE, style);
            NativeMethods.SetWindowLong(mdiClient.Handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE, exStyle);

            // Cause an update of the non-client area.
            updateStyles();
        }

        private MdiClient getMdiClient()
        {
            MdiClient result = null;
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(MdiClient))
                    result = c as MdiClient;
            }
            return result;
        }

        private void updateStyles()
        {
            // To show style changes, the non-client area must be repainted. Using the
            // control's Invalidate method does not affect the non-client area.
            // Instead use a Win32 call to signal the style has changed.

            MdiClient mdiClient = getMdiClient();
            NativeMethods.SetWindowPos(mdiClient.Handle, IntPtr.Zero, 0, 0, 0, 0,
                Win32.FlagsSetWindowPos.SWP_NOACTIVATE |
                Win32.FlagsSetWindowPos.SWP_NOMOVE |
                Win32.FlagsSetWindowPos.SWP_NOSIZE |
                Win32.FlagsSetWindowPos.SWP_NOZORDER |
                Win32.FlagsSetWindowPos.SWP_NOOWNERZORDER |
                Win32.FlagsSetWindowPos.SWP_FRAMECHANGED);
        }
        
        private void mnNovaVezba_Click(object sender, EventArgs e)
        {
            novaVezba();
        }

        private void novaVezba()
        {
            VezbaEditorForm f = new VezbaEditorForm();
            if (f.Initialized)
            {
                newCount++;
                f.MdiParent = this;
                f.FormBorderStyle = FormBorderStyle.None;
                f.Dock = DockStyle.Fill;
                f.MainMenuStrip.Visible = false; // hide menu because it will be
                                                    // merged into parent's menu
                f.ToolStrip.Visible = false;

                tabControl1.TabPages.Add("NovaVezba" + newCount);
                int pageIndex = tabControl1.TabPages.Count - 1;
                tabControl1.TabPages[pageIndex].Tag = f;
                tabControl1.SelectedIndex = pageIndex;

                panelTab.Visible = true;
                f.Show();

                makeCaption();
            }
        }

        private void makeCaption()
        {
            Text = "Vezbe" + " - " + fileTitle();
        }

        private string fileTitle()
        {
            // TODO:
            return "";
            //return (strFileName != null && strFileName.Length > 0) ?
            //             Path.GetFileName(strFileName) : "Untitled";
        }

        private void mnOtvoriVezbu_Click(object sender, EventArgs e)
        {
            otvoriVezbu();
        }

        private void otvoriVezbu()
        {
            OtvoriVezbuForm f = new OtvoriVezbuForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                // TODO: Proveriti da li je data vezba vec otvorena
                // i aktivirati je ako jeste
                /*
                VezbaEditorForm f2 = findOpenedVezba(f.VezbaId);
                if (f2 == null)
                    f2 = new VezbaEditorForm(f.VezbaId);
                else
                {
                    // TODO: Ovo nije dovoljno - potrebno je i tabove odgovarajuce
                    // obraditi
                    f2.Activate();
                }
                 */

                VezbaEditorForm f2 = new VezbaEditorForm(f.VezbaId);
                if (f2.Initialized)
                {
                    f2.MdiParent = this;
                    f2.FormBorderStyle = FormBorderStyle.None;
                    f2.Dock = DockStyle.Fill;
                    f2.MainMenuStrip.Visible = false;
                    f2.ToolStrip.Visible = false;

                    tabControl1.TabPages.Add(f2.Vezba.Naziv);
                    int pageIndex = tabControl1.TabPages.Count - 1;
                    tabControl1.TabPages[pageIndex].Tag = f2;
                    tabControl1.SelectedIndex = pageIndex;

                    panelTab.Visible = true;
                    f2.Show();

                    makeCaption();
                }
            }
        }

        private void mnZatvoriSve_Click(object sender, EventArgs e)
        {
            foreach (Form mdiChildForm in MdiChildren)
            {
                zatvoriVezbu(mdiChildForm as VezbaEditorBaseForm);
            }
        }

        // A separator is automatically shown between the last window list menu item
        // you manually added at design-time and any dynamically added menu items for
        // MDI children. However, when all MDI children are closed, the separator
        // does not disappear. This event handler copes with it.
        private void mnWindow_DropDownOpening(object sender, EventArgs e)
            {
                // Hide separator if it is the last menu strip item in
                // the window list menu
                ToolStripItemCollection items =
                    this.menuStrip1.MdiWindowListItem.DropDownItems;
                if (items[items.Count - 1] is ToolStripSeparator)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != -1)
                (tabControl1.SelectedTab.Tag as Form).BringToFront();
        }

        // Occurs when a multiple-document interface (MDI) child form is activated
        // or closed (and hence some other child form activated) within an MDI
        // application. Also occurs when the last child form is closed.
        private void VezbaForm_MdiChildActivate(object sender, EventArgs e)
        {
            // najpre ukloni tab prozora koji je (ako je) zatvoren
            for (int i = tabControl1.TabPages.Count - 1; i >= 0; i--)
            {
                Form f = tabControl1.TabPages[i].Tag as Form;
                if (!isOpenMdiChild(f))
                    tabControl1.TabPages.RemoveAt(i);
            }
            // selektuj tab trenutno aktivnog prozora
            if (this.ActiveMdiChild != null)
                tabControl1.SelectedTab = pronadjiTab(this.ActiveMdiChild);
        }

        private bool isOpenMdiChild(Form f)
        {
            foreach (Form f2 in this.MdiChildren)
            {
                if (f2 == f)
                    return true;
            }
            return false;
        }

        private TabPage pronadjiTab(Form f)
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Tag == f)
                    return tab;
            }
            return null;
        }

        private void toolBtnNew_Click(object sender, EventArgs e)
        {
            novaVezba();
        }

        private void toolBtnOpen_Click(object sender, EventArgs e)
        {
            otvoriVezbu();
        }

        private bool zatvoriVezbu(VezbaEditorBaseForm f)
        {
            if (f == null)
                return true;
            int before = this.MdiChildren.Length;
            f.Activate();
            f.Close();
            if (this.MdiChildren.Length < before)
            {
                tabControl1.TabPages.Remove(pronadjiTab(f));
                if (this.MdiChildren.Length == 0)
                    panelTab.Visible = false;
                return true;
            }
            else
                return false;
        }

        private void krajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // NOTE:
        // The ToolStrip control that ships with VS2005 works well with one exception:
        // automatic unmerging - it has no provision for automatic unmerging
        // of child form tool strip controls from the MDI Parent form tool strip.
        // To work around this you need to use the ToolStripManagers RevertMerge
        // method. The answer lies in using OnMdiChildActivate method that fires the
        // MdiChildActivate Event. If you try using MdiChildActivate by itself
        // nothing happens by the way.

        // (Osim ovoga, za merging je potrebno podesiti AllowMerge za ToolStrip na
        // true, kao i MergeAction i MergeIndex za ToolStripItem, na slican nacin kao
        // kod MenuStrip kontrole.)
        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e); //REQUIRED
            handleChildMerge(); //Handle merging
        }

        private void handleChildMerge()
        {
            ToolStripManager.RevertMerge(this.toolStrip1);
            VezbaEditorBaseForm childForm = this.ActiveMdiChild as VezbaEditorBaseForm;
            if (childForm != null)
            {
                ToolStripManager.Merge(childForm.ToolStrip, this.toolStrip1);
            }
        }

        private void mnZatvori_Click(object sender, EventArgs e)
        {
            zatvoriAktivnuVezbu();
        }

        private void zatvoriAktivnuVezbu()
        {
            VezbaEditorBaseForm childForm = this.ActiveMdiChild as VezbaEditorBaseForm;
            if (childForm != null)
            {
                zatvoriVezbu(childForm);
            }
        }

        private void mnSnimi_Click(object sender, EventArgs e)
        {
            snimiAktivnuVezbu();
        }

        private void snimiAktivnuVezbu()
        {
            VezbaEditorBaseForm childForm = this.ActiveMdiChild as VezbaEditorBaseForm;
            if (childForm != null && childForm.Modified)
            {
                childForm.save();
            }
        }

        private void toolBtnSnimi_Click(object sender, EventArgs e)
        {
            snimiAktivnuVezbu();
        }

        private void mnBrisiVezbu_Click(object sender, EventArgs e)
        {
            brisiAktivnuVezbu();
        }

        private void brisiAktivnuVezbu()
        {
            VezbaEditorBaseForm childForm = this.ActiveMdiChild as VezbaEditorBaseForm;
            if (childForm != null && childForm.brisiVezbu())
            {
                zatvoriVezbu(childForm);
            }
        }

        private void VezbaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nije potrebno proveravati u svim otvorenim vezbama da li ima
            // nesacuvanih izmena zato sto to MdiParent radi po automatizmu.
        }

        private void mnPrint_Click(object sender, EventArgs e)
        {
            VezbaEditorBaseForm childForm = this.ActiveMdiChild as VezbaEditorBaseForm;
            if (childForm != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                PrintPreviewForm p = new PrintPreviewForm();
                p.setIzvestaj(new VezbaIzvestaj(childForm));
                p.ShowDialog();

                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private VezbaEditorBaseForm findOpenedVezba(int vezbaId)
        {
            foreach (Form f in MdiChildren)
            {
                VezbaEditorBaseForm editor = f as VezbaEditorBaseForm;
                if (editor.Vezba != null && editor.Vezba.Id == vezbaId)
                    return editor;
            }
            return null;
        }

    }
}