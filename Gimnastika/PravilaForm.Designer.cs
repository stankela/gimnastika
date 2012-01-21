namespace Gimnastika
{
    partial class PravilaForm
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
            this.lblPravila = new System.Windows.Forms.Label();
            this.cmbPravila = new System.Windows.Forms.ComboBox();
            this.btnDodaj = new System.Windows.Forms.Button();
            this.btnPromeni = new System.Windows.Forms.Button();
            this.btnBrisi = new System.Windows.Forms.Button();
            this.btnZatvori = new System.Windows.Forms.Button();
            this.gridIzvedba = new System.Windows.Forms.DataGridView();
            this.lblIzvedba = new System.Windows.Forms.Label();
            this.txtBrojBodovanih = new System.Windows.Forms.TextBox();
            this.lblBrojBodovanih = new System.Windows.Forms.Label();
            this.lblMaxIstaGrupa = new System.Windows.Forms.Label();
            this.txtMaxIstaGrupa = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridIzvedba)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPravila
            // 
            this.lblPravila.AutoSize = true;
            this.lblPravila.Location = new System.Drawing.Point(21, 18);
            this.lblPravila.Name = "lblPravila";
            this.lblPravila.Size = new System.Drawing.Size(39, 13);
            this.lblPravila.TabIndex = 20;
            this.lblPravila.Text = "Pravila";
            // 
            // cmbPravila
            // 
            this.cmbPravila.FormattingEnabled = true;
            this.cmbPravila.Location = new System.Drawing.Point(24, 34);
            this.cmbPravila.Name = "cmbPravila";
            this.cmbPravila.Size = new System.Drawing.Size(121, 21);
            this.cmbPravila.TabIndex = 19;
            // 
            // btnDodaj
            // 
            this.btnDodaj.Location = new System.Drawing.Point(321, 14);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(121, 23);
            this.btnDodaj.TabIndex = 21;
            this.btnDodaj.Text = "Dodaj nova pravila";
            this.btnDodaj.UseVisualStyleBackColor = true;
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // btnPromeni
            // 
            this.btnPromeni.Location = new System.Drawing.Point(321, 43);
            this.btnPromeni.Name = "btnPromeni";
            this.btnPromeni.Size = new System.Drawing.Size(121, 23);
            this.btnPromeni.TabIndex = 22;
            this.btnPromeni.Text = "Promeni pravila";
            this.btnPromeni.UseVisualStyleBackColor = true;
            this.btnPromeni.Click += new System.EventHandler(this.btnPromeni_Click);
            // 
            // btnBrisi
            // 
            this.btnBrisi.Location = new System.Drawing.Point(321, 72);
            this.btnBrisi.Name = "btnBrisi";
            this.btnBrisi.Size = new System.Drawing.Size(122, 23);
            this.btnBrisi.TabIndex = 23;
            this.btnBrisi.Text = "Brisi pravila";
            this.btnBrisi.UseVisualStyleBackColor = true;
            this.btnBrisi.Click += new System.EventHandler(this.btnBrisi_Click);
            // 
            // btnZatvori
            // 
            this.btnZatvori.Location = new System.Drawing.Point(367, 110);
            this.btnZatvori.Name = "btnZatvori";
            this.btnZatvori.Size = new System.Drawing.Size(75, 23);
            this.btnZatvori.TabIndex = 24;
            this.btnZatvori.Text = "Zatvori";
            this.btnZatvori.UseVisualStyleBackColor = true;
            this.btnZatvori.Click += new System.EventHandler(this.btnZatvori_Click);
            // 
            // gridIzvedba
            // 
            this.gridIzvedba.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIzvedba.Location = new System.Drawing.Point(21, 180);
            this.gridIzvedba.Name = "gridIzvedba";
            this.gridIzvedba.RowHeadersVisible = false;
            this.gridIzvedba.Size = new System.Drawing.Size(278, 150);
            this.gridIzvedba.TabIndex = 0;
            // 
            // lblIzvedba
            // 
            this.lblIzvedba.AutoSize = true;
            this.lblIzvedba.Location = new System.Drawing.Point(21, 155);
            this.lblIzvedba.Name = "lblIzvedba";
            this.lblIzvedba.Size = new System.Drawing.Size(93, 13);
            this.lblIzvedba.TabIndex = 28;
            this.lblIzvedba.Text = "Ocena za izvedbu";
            // 
            // txtBrojBodovanih
            // 
            this.txtBrojBodovanih.Location = new System.Drawing.Point(176, 69);
            this.txtBrojBodovanih.Name = "txtBrojBodovanih";
            this.txtBrojBodovanih.Size = new System.Drawing.Size(47, 20);
            this.txtBrojBodovanih.TabIndex = 26;
            // 
            // lblBrojBodovanih
            // 
            this.lblBrojBodovanih.AutoSize = true;
            this.lblBrojBodovanih.Location = new System.Drawing.Point(21, 72);
            this.lblBrojBodovanih.Name = "lblBrojBodovanih";
            this.lblBrojBodovanih.Size = new System.Drawing.Size(145, 13);
            this.lblBrojBodovanih.TabIndex = 25;
            this.lblBrojBodovanih.Text = "Broj elemenata koji se boduju";
            // 
            // lblMaxIstaGrupa
            // 
            this.lblMaxIstaGrupa.Location = new System.Drawing.Point(21, 104);
            this.lblMaxIstaGrupa.Name = "lblMaxIstaGrupa";
            this.lblMaxIstaGrupa.Size = new System.Drawing.Size(145, 29);
            this.lblMaxIstaGrupa.TabIndex = 29;
            this.lblMaxIstaGrupa.Text = "Maksimalan broj elemenata iz iste grupe koji se boduju";
            // 
            // txtMaxIstaGrupa
            // 
            this.txtMaxIstaGrupa.Location = new System.Drawing.Point(176, 104);
            this.txtMaxIstaGrupa.Name = "txtMaxIstaGrupa";
            this.txtMaxIstaGrupa.Size = new System.Drawing.Size(47, 20);
            this.txtMaxIstaGrupa.TabIndex = 31;
            // 
            // PravilaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 346);
            this.Controls.Add(this.txtMaxIstaGrupa);
            this.Controls.Add(this.lblMaxIstaGrupa);
            this.Controls.Add(this.gridIzvedba);
            this.Controls.Add(this.lblIzvedba);
            this.Controls.Add(this.txtBrojBodovanih);
            this.Controls.Add(this.lblBrojBodovanih);
            this.Controls.Add(this.btnZatvori);
            this.Controls.Add(this.btnBrisi);
            this.Controls.Add(this.btnPromeni);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.lblPravila);
            this.Controls.Add(this.cmbPravila);
            this.Name = "PravilaForm";
            this.Text = "PravilaForm";
            ((System.ComponentModel.ISupportInitialize)(this.gridIzvedba)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPravila;
        private System.Windows.Forms.ComboBox cmbPravila;
        private System.Windows.Forms.Button btnDodaj;
        private System.Windows.Forms.Button btnPromeni;
        private System.Windows.Forms.Button btnBrisi;
        private System.Windows.Forms.Button btnZatvori;
        private System.Windows.Forms.DataGridView gridIzvedba;
        private System.Windows.Forms.Label lblIzvedba;
        private System.Windows.Forms.TextBox txtBrojBodovanih;
        private System.Windows.Forms.Label lblBrojBodovanih;
        private System.Windows.Forms.Label lblMaxIstaGrupa;
        private System.Windows.Forms.TextBox txtMaxIstaGrupa;
    }
}