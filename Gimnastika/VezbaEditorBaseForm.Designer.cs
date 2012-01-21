namespace Gimnastika
{
    partial class VezbaEditorBaseForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VezbaEditorBaseForm));
            this.toolBtnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolBtnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.toolBtnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolBtnObelezi = new System.Windows.Forms.ToolStripButton();
            this.toolBtnBrisi = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnIzracunaj = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.cntMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnVezaSaPrethodnim = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPonistiVezu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDodajElementeIzTablice = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDodajElemente = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblGimnasticarCaption = new System.Windows.Forms.Label();
            this.lblGimnasticarValue = new System.Windows.Forms.Label();
            this.lblSpravaCaption = new System.Windows.Forms.Label();
            this.lblPraviloCaption = new System.Windows.Forms.Label();
            this.lblSpravaValue = new System.Windows.Forms.Label();
            this.lblPraviloValue = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.cntMenuGrid.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBtnAdd
            // 
            this.toolBtnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnAdd.Image")));
            this.toolBtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnAdd.Name = "toolBtnAdd";
            this.toolBtnAdd.Size = new System.Drawing.Size(23, 22);
            this.toolBtnAdd.Text = "Dodaj elemente";
            this.toolBtnAdd.Click += new System.EventHandler(this.toolBtnAdd_Click);
            // 
            // toolBtnMoveUp
            // 
            this.toolBtnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnMoveUp.Image")));
            this.toolBtnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnMoveUp.Name = "toolBtnMoveUp";
            this.toolBtnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.toolBtnMoveUp.Text = "Pomeri gore";
            this.toolBtnMoveUp.Click += new System.EventHandler(this.toolBtnMoveUp_Click);
            // 
            // toolBtnMoveDown
            // 
            this.toolBtnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnMoveDown.Image")));
            this.toolBtnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnMoveDown.Name = "toolBtnMoveDown";
            this.toolBtnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.toolBtnMoveDown.Text = "Pomeri dole";
            this.toolBtnMoveDown.Click += new System.EventHandler(this.toolBtnMoveDown_Click);
            // 
            // toolBtnObelezi
            // 
            this.toolBtnObelezi.CheckOnClick = true;
            this.toolBtnObelezi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnObelezi.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnObelezi.Image")));
            this.toolBtnObelezi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnObelezi.Name = "toolBtnObelezi";
            this.toolBtnObelezi.Size = new System.Drawing.Size(23, 22);
            this.toolBtnObelezi.Text = "Obelezi";
            this.toolBtnObelezi.Click += new System.EventHandler(this.toolBtnObelezi_Click);
            // 
            // toolBtnBrisi
            // 
            this.toolBtnBrisi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnBrisi.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnBrisi.Image")));
            this.toolBtnBrisi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnBrisi.Name = "toolBtnBrisi";
            this.toolBtnBrisi.Size = new System.Drawing.Size(23, 22);
            this.toolBtnBrisi.Text = "Brisi element";
            this.toolBtnBrisi.Click += new System.EventHandler(this.toolBtnBrisi_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolBtnIzracunaj
            // 
            this.toolBtnIzracunaj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnIzracunaj.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnIzracunaj.Image")));
            this.toolBtnIzracunaj.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnIzracunaj.Name = "toolBtnIzracunaj";
            this.toolBtnIzracunaj.Size = new System.Drawing.Size(23, 22);
            this.toolBtnIzracunaj.Text = "Izracunaj";
            this.toolBtnIzracunaj.Click += new System.EventHandler(this.toolBtnIzracunaj_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnAdd,
            this.toolBtnMoveUp,
            this.toolBtnMoveDown,
            this.toolBtnObelezi,
            this.toolBtnBrisi,
            this.toolStripSeparator2,
            this.toolBtnIzracunaj});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(693, 25);
            this.toolStrip1.TabIndex = 40;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(693, 24);
            this.menuStrip1.TabIndex = 39;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // cntMenuGrid
            // 
            this.cntMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnVezaSaPrethodnim,
            this.mnPonistiVezu,
            this.mnDodajElementeIzTablice,
            this.mnDodajElemente});
            this.cntMenuGrid.Name = "cntMenuGrid";
            this.cntMenuGrid.Size = new System.Drawing.Size(205, 92);
            this.cntMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.cntMenuGrid_Opening);
            // 
            // mnVezaSaPrethodnim
            // 
            this.mnVezaSaPrethodnim.Name = "mnVezaSaPrethodnim";
            this.mnVezaSaPrethodnim.Size = new System.Drawing.Size(204, 22);
            this.mnVezaSaPrethodnim.Text = "Veza sa prethodnim";
            this.mnVezaSaPrethodnim.Click += new System.EventHandler(this.mnVezaSaPrethodnim_Click);
            // 
            // mnPonistiVezu
            // 
            this.mnPonistiVezu.Name = "mnPonistiVezu";
            this.mnPonistiVezu.Size = new System.Drawing.Size(204, 22);
            this.mnPonistiVezu.Text = "Ponisti vezu";
            this.mnPonistiVezu.Click += new System.EventHandler(this.mnPonistiVezu_Click);
            // 
            // mnDodajElementeIzTablice
            // 
            this.mnDodajElementeIzTablice.Name = "mnDodajElementeIzTablice";
            this.mnDodajElementeIzTablice.Size = new System.Drawing.Size(204, 22);
            this.mnDodajElementeIzTablice.Text = "Dodaj elemente iz tablice";
            this.mnDodajElementeIzTablice.Click += new System.EventHandler(this.mnDodajElementeIzTablice_Click);
            // 
            // mnDodajElemente
            // 
            this.mnDodajElemente.Name = "mnDodajElemente";
            this.mnDodajElemente.Size = new System.Drawing.Size(204, 22);
            this.mnDodajElemente.Text = "Dodaj elemente";
            this.mnDodajElemente.Click += new System.EventHandler(this.mnDodajElemente_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblPraviloValue);
            this.panel2.Controls.Add(this.lblSpravaValue);
            this.panel2.Controls.Add(this.lblPraviloCaption);
            this.panel2.Controls.Add(this.lblSpravaCaption);
            this.panel2.Controls.Add(this.lblGimnasticarValue);
            this.panel2.Controls.Add(this.lblGimnasticarCaption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(693, 43);
            this.panel2.TabIndex = 38;
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            // 
            // lblGimnasticarCaption
            // 
            this.lblGimnasticarCaption.AutoSize = true;
            this.lblGimnasticarCaption.Location = new System.Drawing.Point(6, 16);
            this.lblGimnasticarCaption.Name = "lblGimnasticarCaption";
            this.lblGimnasticarCaption.Size = new System.Drawing.Size(65, 13);
            this.lblGimnasticarCaption.TabIndex = 32;
            this.lblGimnasticarCaption.Text = "Gimnasticar:";
            this.lblGimnasticarCaption.Click += new System.EventHandler(this.lblGimnasticarCaption_Click);
            // 
            // lblGimnasticarValue
            // 
            this.lblGimnasticarValue.AutoSize = true;
            this.lblGimnasticarValue.Location = new System.Drawing.Point(68, 16);
            this.lblGimnasticarValue.Name = "lblGimnasticarValue";
            this.lblGimnasticarValue.Size = new System.Drawing.Size(86, 13);
            this.lblGimnasticarValue.TabIndex = 33;
            this.lblGimnasticarValue.Text = "Ime gimnasticara";
            this.lblGimnasticarValue.Click += new System.EventHandler(this.lblGimnasticarValue_Click);
            // 
            // lblSpravaCaption
            // 
            this.lblSpravaCaption.AutoSize = true;
            this.lblSpravaCaption.Location = new System.Drawing.Point(165, 16);
            this.lblSpravaCaption.Name = "lblSpravaCaption";
            this.lblSpravaCaption.Size = new System.Drawing.Size(44, 13);
            this.lblSpravaCaption.TabIndex = 34;
            this.lblSpravaCaption.Text = "Sprava:";
            this.lblSpravaCaption.Click += new System.EventHandler(this.lblSpravaCaption_Click);
            // 
            // lblPraviloCaption
            // 
            this.lblPraviloCaption.AutoSize = true;
            this.lblPraviloCaption.Location = new System.Drawing.Point(284, 16);
            this.lblPraviloCaption.Name = "lblPraviloCaption";
            this.lblPraviloCaption.Size = new System.Drawing.Size(99, 13);
            this.lblPraviloCaption.TabIndex = 35;
            this.lblPraviloCaption.Text = "Pravila ocenjivanja:";
            this.lblPraviloCaption.Click += new System.EventHandler(this.lblPraviloCaption_Click);
            // 
            // lblSpravaValue
            // 
            this.lblSpravaValue.AutoSize = true;
            this.lblSpravaValue.Location = new System.Drawing.Point(205, 16);
            this.lblSpravaValue.Name = "lblSpravaValue";
            this.lblSpravaValue.Size = new System.Drawing.Size(41, 13);
            this.lblSpravaValue.TabIndex = 36;
            this.lblSpravaValue.Text = "Sprava";
            this.lblSpravaValue.Click += new System.EventHandler(this.lblSpravaValue_Click);
            // 
            // lblPraviloValue
            // 
            this.lblPraviloValue.AutoSize = true;
            this.lblPraviloValue.Location = new System.Drawing.Point(380, 16);
            this.lblPraviloValue.Name = "lblPraviloValue";
            this.lblPraviloValue.Size = new System.Drawing.Size(39, 13);
            this.lblPraviloValue.TabIndex = 37;
            this.lblPraviloValue.Text = "Pravila";
            this.lblPraviloValue.Click += new System.EventHandler(this.lblPraviloValue_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 321);
            this.panel1.TabIndex = 29;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // VezbaEditorBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(693, 370);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VezbaEditorBaseForm";
            this.Text = "VezbaEditorBaseForm";
            this.Deactivate += new System.EventHandler(this.VezbaEditorBaseForm_Deactivate);
            this.Click += new System.EventHandler(this.VezbaEditorBaseForm_Click);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VezbaEditorBaseForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cntMenuGrid.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton toolBtnAdd;
        private System.Windows.Forms.ToolStripButton toolBtnMoveUp;
        private System.Windows.Forms.ToolStripButton toolBtnMoveDown;
        private System.Windows.Forms.ToolStripButton toolBtnObelezi;
        private System.Windows.Forms.ToolStripButton toolBtnBrisi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolBtnIzracunaj;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        protected System.Windows.Forms.ContextMenuStrip cntMenuGrid;
        private System.Windows.Forms.ToolStripMenuItem mnVezaSaPrethodnim;
        private System.Windows.Forms.ToolStripMenuItem mnPonistiVezu;
        private System.Windows.Forms.ToolStripMenuItem mnDodajElemente;
        private System.Windows.Forms.ToolStripMenuItem mnDodajElementeIzTablice;
        protected System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblPraviloValue;
        private System.Windows.Forms.Label lblSpravaValue;
        private System.Windows.Forms.Label lblPraviloCaption;
        private System.Windows.Forms.Label lblSpravaCaption;
        private System.Windows.Forms.Label lblGimnasticarValue;
        private System.Windows.Forms.Label lblGimnasticarCaption;
        protected System.Windows.Forms.Panel panel1;
    }
}