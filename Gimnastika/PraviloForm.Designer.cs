namespace Gimnastika
{
    partial class PraviloForm
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
            this.lblBrojBodovanih = new System.Windows.Forms.Label();
            this.txtBrojBodovanih = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblIzvedba = new System.Windows.Forms.Label();
            this.gridIzvedba = new System.Windows.Forms.DataGridView();
            this.lblNazivPravila = new System.Windows.Forms.Label();
            this.txtNazivPravila = new System.Windows.Forms.TextBox();
            this.lblMaxIstaGrupa = new System.Windows.Forms.Label();
            this.txtMaxIstaGrupa = new System.Windows.Forms.TextBox();
            this.btnDodajOcenu = new System.Windows.Forms.Button();
            this.btnIzbrisiOcenu = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridIzvedba)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBrojBodovanih
            // 
            this.lblBrojBodovanih.AutoSize = true;
            this.lblBrojBodovanih.Location = new System.Drawing.Point(22, 60);
            this.lblBrojBodovanih.Name = "lblBrojBodovanih";
            this.lblBrojBodovanih.Size = new System.Drawing.Size(145, 13);
            this.lblBrojBodovanih.TabIndex = 0;
            this.lblBrojBodovanih.Text = "Broj elemenata koji se boduju";
            // 
            // txtBrojBodovanih
            // 
            this.txtBrojBodovanih.Location = new System.Drawing.Point(173, 57);
            this.txtBrojBodovanih.Name = "txtBrojBodovanih";
            this.txtBrojBodovanih.Size = new System.Drawing.Size(44, 20);
            this.txtBrojBodovanih.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(25, 325);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(121, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblIzvedba
            // 
            this.lblIzvedba.AutoSize = true;
            this.lblIzvedba.Location = new System.Drawing.Point(22, 141);
            this.lblIzvedba.Name = "lblIzvedba";
            this.lblIzvedba.Size = new System.Drawing.Size(93, 13);
            this.lblIzvedba.TabIndex = 13;
            this.lblIzvedba.Text = "Ocena za izvedbu";
            // 
            // gridIzvedba
            // 
            this.gridIzvedba.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIzvedba.Location = new System.Drawing.Point(25, 157);
            this.gridIzvedba.Name = "gridIzvedba";
            this.gridIzvedba.RowHeadersVisible = false;
            this.gridIzvedba.Size = new System.Drawing.Size(278, 150);
            this.gridIzvedba.TabIndex = 1;
            // 
            // lblNazivPravila
            // 
            this.lblNazivPravila.AutoSize = true;
            this.lblNazivPravila.Location = new System.Drawing.Point(22, 24);
            this.lblNazivPravila.Name = "lblNazivPravila";
            this.lblNazivPravila.Size = new System.Drawing.Size(68, 13);
            this.lblNazivPravila.TabIndex = 15;
            this.lblNazivPravila.Text = "Naziv pravila";
            // 
            // txtNazivPravila
            // 
            this.txtNazivPravila.Location = new System.Drawing.Point(96, 21);
            this.txtNazivPravila.Name = "txtNazivPravila";
            this.txtNazivPravila.Size = new System.Drawing.Size(121, 20);
            this.txtNazivPravila.TabIndex = 2;
            // 
            // lblMaxIstaGrupa
            // 
            this.lblMaxIstaGrupa.Location = new System.Drawing.Point(22, 91);
            this.lblMaxIstaGrupa.Name = "lblMaxIstaGrupa";
            this.lblMaxIstaGrupa.Size = new System.Drawing.Size(145, 36);
            this.lblMaxIstaGrupa.TabIndex = 17;
            this.lblMaxIstaGrupa.Text = "Maksimalan broj elemenata iz iste grupe koji se boduju";
            // 
            // txtMaxIstaGrupa
            // 
            this.txtMaxIstaGrupa.Location = new System.Drawing.Point(173, 91);
            this.txtMaxIstaGrupa.Name = "txtMaxIstaGrupa";
            this.txtMaxIstaGrupa.Size = new System.Drawing.Size(44, 20);
            this.txtMaxIstaGrupa.TabIndex = 4;
            // 
            // btnDodajOcenu
            // 
            this.btnDodajOcenu.Location = new System.Drawing.Point(322, 157);
            this.btnDodajOcenu.Name = "btnDodajOcenu";
            this.btnDodajOcenu.Size = new System.Drawing.Size(87, 23);
            this.btnDodajOcenu.TabIndex = 20;
            this.btnDodajOcenu.Text = "Dodaj ocenu";
            this.btnDodajOcenu.UseVisualStyleBackColor = true;
            this.btnDodajOcenu.Click += new System.EventHandler(this.btnDodajOcenu_Click);
            // 
            // btnIzbrisiOcenu
            // 
            this.btnIzbrisiOcenu.Location = new System.Drawing.Point(322, 195);
            this.btnIzbrisiOcenu.Name = "btnIzbrisiOcenu";
            this.btnIzbrisiOcenu.Size = new System.Drawing.Size(87, 23);
            this.btnIzbrisiOcenu.TabIndex = 21;
            this.btnIzbrisiOcenu.Text = "Izbrisi ocenu";
            this.btnIzbrisiOcenu.UseVisualStyleBackColor = true;
            this.btnIzbrisiOcenu.Click += new System.EventHandler(this.btnIzbrisiOcenu_Click);
            // 
            // PraviloForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(452, 365);
            this.Controls.Add(this.btnIzbrisiOcenu);
            this.Controls.Add(this.btnDodajOcenu);
            this.Controls.Add(this.txtMaxIstaGrupa);
            this.Controls.Add(this.lblMaxIstaGrupa);
            this.Controls.Add(this.txtNazivPravila);
            this.Controls.Add(this.lblNazivPravila);
            this.Controls.Add(this.gridIzvedba);
            this.Controls.Add(this.lblIzvedba);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtBrojBodovanih);
            this.Controls.Add(this.lblBrojBodovanih);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PraviloForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PraviloForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PraviloForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.gridIzvedba)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBrojBodovanih;
        private System.Windows.Forms.TextBox txtBrojBodovanih;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblIzvedba;
        private System.Windows.Forms.DataGridView gridIzvedba;
        private System.Windows.Forms.Label lblNazivPravila;
        private System.Windows.Forms.TextBox txtNazivPravila;
        private System.Windows.Forms.Label lblMaxIstaGrupa;
        private System.Windows.Forms.TextBox txtMaxIstaGrupa;
        private System.Windows.Forms.Button btnDodajOcenu;
        private System.Windows.Forms.Button btnIzbrisiOcenu;
    }
}