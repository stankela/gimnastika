namespace Gimnastika.UI
{
    partial class VezbaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VezbaForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolBtnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSnimi = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnDatoteka = new System.Windows.Forms.ToolStripMenuItem();
            this.mnNovaVezba = new System.Windows.Forms.ToolStripMenuItem();
            this.mnOtvoriVezbu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnZatvori = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnSnimi = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnBrisiVezbu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.krajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnZatvoriSve = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTab = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.mnPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnNew,
            this.toolBtnOpen,
            this.toolBtnSnimi,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(711, 25);
            this.toolStrip1.TabIndex = 29;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolBtnNew
            // 
            this.toolBtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnNew.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnNew.Image")));
            this.toolBtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnNew.Name = "toolBtnNew";
            this.toolBtnNew.Size = new System.Drawing.Size(23, 22);
            this.toolBtnNew.Text = "Nova vezba";
            this.toolBtnNew.Click += new System.EventHandler(this.toolBtnNew_Click);
            // 
            // toolBtnOpen
            // 
            this.toolBtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnOpen.Image")));
            this.toolBtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnOpen.Name = "toolBtnOpen";
            this.toolBtnOpen.Size = new System.Drawing.Size(23, 22);
            this.toolBtnOpen.Text = "Otvori vezbu";
            this.toolBtnOpen.Click += new System.EventHandler(this.toolBtnOpen_Click);
            // 
            // toolBtnSnimi
            // 
            this.toolBtnSnimi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnSnimi.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSnimi.Image")));
            this.toolBtnSnimi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSnimi.Name = "toolBtnSnimi";
            this.toolBtnSnimi.Size = new System.Drawing.Size(23, 22);
            this.toolBtnSnimi.Text = "Snimi vezbu";
            this.toolBtnSnimi.Click += new System.EventHandler(this.toolBtnSnimi_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // mnDatoteka
            // 
            this.mnDatoteka.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnNovaVezba,
            this.mnOtvoriVezbu,
            this.toolStripSeparator2,
            this.mnZatvori,
            this.toolStripSeparator3,
            this.mnSnimi,
            this.toolStripSeparator1,
            this.mnBrisiVezbu,
            this.toolStripSeparator5,
            this.mnPrint,
            this.toolStripMenuItem1,
            this.krajToolStripMenuItem});
            this.mnDatoteka.Name = "mnDatoteka";
            this.mnDatoteka.Size = new System.Drawing.Size(63, 20);
            this.mnDatoteka.Text = "Datoteka";
            // 
            // mnNovaVezba
            // 
            this.mnNovaVezba.Name = "mnNovaVezba";
            this.mnNovaVezba.Size = new System.Drawing.Size(156, 22);
            this.mnNovaVezba.Text = "Nova vezba";
            this.mnNovaVezba.Click += new System.EventHandler(this.mnNovaVezba_Click);
            // 
            // mnOtvoriVezbu
            // 
            this.mnOtvoriVezbu.Name = "mnOtvoriVezbu";
            this.mnOtvoriVezbu.Size = new System.Drawing.Size(156, 22);
            this.mnOtvoriVezbu.Text = "Otvori vezbu";
            this.mnOtvoriVezbu.Click += new System.EventHandler(this.mnOtvoriVezbu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
            // 
            // mnZatvori
            // 
            this.mnZatvori.Name = "mnZatvori";
            this.mnZatvori.Size = new System.Drawing.Size(156, 22);
            this.mnZatvori.Text = "Zatvori vezbu";
            this.mnZatvori.Click += new System.EventHandler(this.mnZatvori_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(153, 6);
            // 
            // mnSnimi
            // 
            this.mnSnimi.Name = "mnSnimi";
            this.mnSnimi.Size = new System.Drawing.Size(156, 22);
            this.mnSnimi.Text = "Snimi vezbu";
            this.mnSnimi.Click += new System.EventHandler(this.mnSnimi_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // mnBrisiVezbu
            // 
            this.mnBrisiVezbu.Name = "mnBrisiVezbu";
            this.mnBrisiVezbu.Size = new System.Drawing.Size(156, 22);
            this.mnBrisiVezbu.Text = "Brisi vezbu";
            this.mnBrisiVezbu.Click += new System.EventHandler(this.mnBrisiVezbu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(153, 6);
            // 
            // krajToolStripMenuItem
            // 
            this.krajToolStripMenuItem.Name = "krajToolStripMenuItem";
            this.krajToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.krajToolStripMenuItem.Text = "Kraj";
            this.krajToolStripMenuItem.Click += new System.EventHandler(this.krajToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDatoteka,
            this.mnWindow});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.mnWindow;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(711, 24);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnWindow
            // 
            this.mnWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnZatvoriSve});
            this.mnWindow.Name = "mnWindow";
            this.mnWindow.Size = new System.Drawing.Size(57, 20);
            this.mnWindow.Text = "Window";
            this.mnWindow.DropDownOpening += new System.EventHandler(this.mnWindow_DropDownOpening);
            // 
            // mnZatvoriSve
            // 
            this.mnZatvoriSve.Name = "mnZatvoriSve";
            this.mnZatvoriSve.Size = new System.Drawing.Size(171, 22);
            this.mnZatvoriSve.Text = "Zatvori sve vezbe";
            this.mnZatvoriSve.Click += new System.EventHandler(this.mnZatvoriSve_Click);
            // 
            // panelTab
            // 
            this.panelTab.BackColor = System.Drawing.SystemColors.Control;
            this.panelTab.Controls.Add(this.tabControl1);
            this.panelTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTab.Location = new System.Drawing.Point(0, 49);
            this.panelTab.Name = "panelTab";
            this.panelTab.Size = new System.Drawing.Size(711, 24);
            this.panelTab.TabIndex = 32;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.ItemSize = new System.Drawing.Size(58, 18);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(711, 21);
            this.tabControl1.TabIndex = 32;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(703, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(703, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // mnPrint
            // 
            this.mnPrint.Name = "mnPrint";
            this.mnPrint.Size = new System.Drawing.Size(156, 22);
            this.mnPrint.Text = "Stampaj vezbu";
            this.mnPrint.Click += new System.EventHandler(this.mnPrint_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // VezbaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 403);
            this.Controls.Add(this.panelTab);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VezbaForm";
            this.Text = "VezbaForm";
            this.MdiChildActivate += new System.EventHandler(this.VezbaForm_MdiChildActivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VezbaForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelTab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnDatoteka;
        private System.Windows.Forms.ToolStripMenuItem mnNovaVezba;
        private System.Windows.Forms.ToolStripMenuItem mnOtvoriVezbu;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnWindow;
        private System.Windows.Forms.ToolStripMenuItem mnZatvoriSve;
        private System.Windows.Forms.ToolStripButton toolBtnNew;
        private System.Windows.Forms.ToolStripButton toolBtnOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem krajToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnZatvori;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel panelTab;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem mnSnimi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolBtnSnimi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnBrisiVezbu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}