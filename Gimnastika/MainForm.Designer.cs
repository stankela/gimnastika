namespace Gimnastika.UI
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnDatoteka = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDatotekaKraj = new System.Windows.Forms.ToolStripMenuItem();
            this.mnGimnasticari = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPromenaGimnasticara = new System.Windows.Forms.ToolStripMenuItem();
            this.mnVezbe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnTabelaElemenata = new System.Windows.Forms.ToolStripMenuItem();
            this.mnVezbeVezbe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPromenaElemenata = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPregledElemenata = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPravilaOcenjivanja = new System.Windows.Forms.ToolStripMenuItem();
            this.mnOpcije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnOpcijeOpcije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnNaziviGrupa = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDatoteka,
            this.mnGimnasticari,
            this.mnVezbe,
            this.mnOpcije});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(292, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnDatoteka
            // 
            this.mnDatoteka.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDatotekaKraj});
            this.mnDatoteka.Name = "mnDatoteka";
            this.mnDatoteka.Size = new System.Drawing.Size(63, 20);
            this.mnDatoteka.Text = "Datoteka";
            // 
            // mnDatotekaKraj
            // 
            this.mnDatotekaKraj.Name = "mnDatotekaKraj";
            this.mnDatotekaKraj.Size = new System.Drawing.Size(104, 22);
            this.mnDatotekaKraj.Text = "Kraj";
            this.mnDatotekaKraj.Click += new System.EventHandler(this.mnDatotekaKraj_Click);
            // 
            // mnGimnasticari
            // 
            this.mnGimnasticari.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnPromenaGimnasticara});
            this.mnGimnasticari.Name = "mnGimnasticari";
            this.mnGimnasticari.Size = new System.Drawing.Size(76, 20);
            this.mnGimnasticari.Text = "Gimnasticari";
            // 
            // mnPromenaGimnasticara
            // 
            this.mnPromenaGimnasticara.Name = "mnPromenaGimnasticara";
            this.mnPromenaGimnasticara.Size = new System.Drawing.Size(190, 22);
            this.mnPromenaGimnasticara.Text = "Promena gimnasticara";
            this.mnPromenaGimnasticara.Click += new System.EventHandler(this.mnPromenaGimnasticara_Click);
            // 
            // mnVezbe
            // 
            this.mnVezbe.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnTabelaElemenata,
            this.mnNaziviGrupa,
            this.mnVezbeVezbe,
            this.mnPromenaElemenata,
            this.mnPregledElemenata,
            this.mnPravilaOcenjivanja});
            this.mnVezbe.Name = "mnVezbe";
            this.mnVezbe.Size = new System.Drawing.Size(48, 20);
            this.mnVezbe.Text = "Vezbe";
            // 
            // mnTabelaElemenata
            // 
            this.mnTabelaElemenata.Name = "mnTabelaElemenata";
            this.mnTabelaElemenata.Size = new System.Drawing.Size(180, 22);
            this.mnTabelaElemenata.Text = "Tabela elemenata";
            this.mnTabelaElemenata.Click += new System.EventHandler(this.mnTabelaElemenata_Click);
            // 
            // mnVezbeVezbe
            // 
            this.mnVezbeVezbe.Name = "mnVezbeVezbe";
            this.mnVezbeVezbe.Size = new System.Drawing.Size(180, 22);
            this.mnVezbeVezbe.Text = "Vezbe";
            this.mnVezbeVezbe.Click += new System.EventHandler(this.mnVezbeVezbe_Click);
            // 
            // mnPromenaElemenata
            // 
            this.mnPromenaElemenata.Name = "mnPromenaElemenata";
            this.mnPromenaElemenata.Size = new System.Drawing.Size(180, 22);
            this.mnPromenaElemenata.Text = "Promena elemenata";
            this.mnPromenaElemenata.Click += new System.EventHandler(this.mnPromenaElemenata_Click);
            // 
            // mnPregledElemenata
            // 
            this.mnPregledElemenata.Name = "mnPregledElemenata";
            this.mnPregledElemenata.Size = new System.Drawing.Size(180, 22);
            this.mnPregledElemenata.Text = "Pregled elemenata";
            this.mnPregledElemenata.Click += new System.EventHandler(this.mnPregledElemenata_Click);
            // 
            // mnPravilaOcenjivanja
            // 
            this.mnPravilaOcenjivanja.Name = "mnPravilaOcenjivanja";
            this.mnPravilaOcenjivanja.Size = new System.Drawing.Size(180, 22);
            this.mnPravilaOcenjivanja.Text = "Pravila ocenjivanja";
            this.mnPravilaOcenjivanja.Click += new System.EventHandler(this.mnPravilaOcenjivanja_Click);
            // 
            // mnOpcije
            // 
            this.mnOpcije.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnOpcijeOpcije});
            this.mnOpcije.Name = "mnOpcije";
            this.mnOpcije.Size = new System.Drawing.Size(49, 20);
            this.mnOpcije.Text = "Opcije";
            // 
            // mnOpcijeOpcije
            // 
            this.mnOpcijeOpcije.Name = "mnOpcijeOpcije";
            this.mnOpcijeOpcije.Size = new System.Drawing.Size(115, 22);
            this.mnOpcijeOpcije.Text = "Opcije";
            this.mnOpcijeOpcije.Click += new System.EventHandler(this.mnOpcijeOpcije_Click);
            // 
            // mnNaziviGrupa
            // 
            this.mnNaziviGrupa.Name = "mnNaziviGrupa";
            this.mnNaziviGrupa.Size = new System.Drawing.Size(180, 22);
            this.mnNaziviGrupa.Text = "Nazivi grupa";
            this.mnNaziviGrupa.Click += new System.EventHandler(this.mnNaziviGrupa_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnDatoteka;
        private System.Windows.Forms.ToolStripMenuItem mnDatotekaKraj;
        private System.Windows.Forms.ToolStripMenuItem mnVezbe;
        private System.Windows.Forms.ToolStripMenuItem mnGimnasticari;
        private System.Windows.Forms.ToolStripMenuItem mnPromenaGimnasticara;
        private System.Windows.Forms.ToolStripMenuItem mnPromenaElemenata;
        private System.Windows.Forms.ToolStripMenuItem mnPregledElemenata;
        private System.Windows.Forms.ToolStripMenuItem mnOpcije;
        private System.Windows.Forms.ToolStripMenuItem mnOpcijeOpcije;
        private System.Windows.Forms.ToolStripMenuItem mnVezbeVezbe;
        private System.Windows.Forms.ToolStripMenuItem mnPravilaOcenjivanja;
        private System.Windows.Forms.ToolStripMenuItem mnTabelaElemenata;
        private System.Windows.Forms.ToolStripMenuItem mnNaziviGrupa;
    }
}