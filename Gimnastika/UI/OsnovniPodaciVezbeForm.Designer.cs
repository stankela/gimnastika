namespace Gimnastika.UI
{
    partial class OsnovniPodaciVezbeForm
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
            this.cmbGimnasticar = new System.Windows.Forms.ComboBox();
            this.lblGimnasticar = new System.Windows.Forms.Label();
            this.lblSprava = new System.Windows.Forms.Label();
            this.cmbSprava = new System.Windows.Forms.ComboBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lblNijeObavezno = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblPravila
            // 
            this.lblPravila.AutoSize = true;
            this.lblPravila.Location = new System.Drawing.Point(35, 167);
            this.lblPravila.Name = "lblPravila";
            this.lblPravila.Size = new System.Drawing.Size(96, 13);
            this.lblPravila.TabIndex = 23;
            this.lblPravila.Text = "Pravila ocenjivanja";
            // 
            // cmbPravila
            // 
            this.cmbPravila.FormattingEnabled = true;
            this.cmbPravila.Location = new System.Drawing.Point(38, 183);
            this.cmbPravila.Name = "cmbPravila";
            this.cmbPravila.Size = new System.Drawing.Size(138, 21);
            this.cmbPravila.TabIndex = 3;
            // 
            // cmbGimnasticar
            // 
            this.cmbGimnasticar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGimnasticar.FormattingEnabled = true;
            this.cmbGimnasticar.Location = new System.Drawing.Point(38, 73);
            this.cmbGimnasticar.Name = "cmbGimnasticar";
            this.cmbGimnasticar.Size = new System.Drawing.Size(138, 21);
            this.cmbGimnasticar.TabIndex = 1;
            // 
            // lblGimnasticar
            // 
            this.lblGimnasticar.AutoSize = true;
            this.lblGimnasticar.Location = new System.Drawing.Point(35, 57);
            this.lblGimnasticar.Name = "lblGimnasticar";
            this.lblGimnasticar.Size = new System.Drawing.Size(62, 13);
            this.lblGimnasticar.TabIndex = 21;
            this.lblGimnasticar.Text = "Gimnasticar";
            // 
            // lblSprava
            // 
            this.lblSprava.AutoSize = true;
            this.lblSprava.Location = new System.Drawing.Point(35, 112);
            this.lblSprava.Name = "lblSprava";
            this.lblSprava.Size = new System.Drawing.Size(41, 13);
            this.lblSprava.TabIndex = 20;
            this.lblSprava.Text = "Sprava";
            // 
            // cmbSprava
            // 
            this.cmbSprava.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSprava.FormattingEnabled = true;
            this.cmbSprava.Location = new System.Drawing.Point(38, 128);
            this.cmbSprava.Name = "cmbSprava";
            this.cmbSprava.Size = new System.Drawing.Size(138, 21);
            this.cmbSprava.TabIndex = 2;
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(35, 20);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(118, 13);
            this.lblPrompt.TabIndex = 24;
            this.lblPrompt.Text = "Osnovni podaci o vezbi";
            // 
            // lblNijeObavezno
            // 
            this.lblNijeObavezno.AutoSize = true;
            this.lblNijeObavezno.Location = new System.Drawing.Point(194, 76);
            this.lblNijeObavezno.Name = "lblNijeObavezno";
            this.lblNijeObavezno.Size = new System.Drawing.Size(81, 13);
            this.lblNijeObavezno.TabIndex = 25;
            this.lblNijeObavezno.Text = "(Nije obavezno)";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(38, 283);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(128, 283);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(35, 227);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(66, 13);
            this.lblNaziv.TabIndex = 38;
            this.lblNaziv.Text = "Naziv vezbe";
            // 
            // txtNaziv
            // 
            this.txtNaziv.BackColor = System.Drawing.SystemColors.Window;
            this.txtNaziv.Location = new System.Drawing.Point(38, 243);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(237, 20);
            this.txtNaziv.TabIndex = 37;
            // 
            // OsnovniPodaciVezbeForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(300, 322);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblNijeObavezno);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.lblPravila);
            this.Controls.Add(this.cmbPravila);
            this.Controls.Add(this.cmbGimnasticar);
            this.Controls.Add(this.lblGimnasticar);
            this.Controls.Add(this.lblSprava);
            this.Controls.Add(this.cmbSprava);
            this.Name = "OsnovniPodaciVezbeForm";
            this.Text = "OsnovniPodaciVezbeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPravila;
        private System.Windows.Forms.ComboBox cmbPravila;
        private System.Windows.Forms.ComboBox cmbGimnasticar;
        private System.Windows.Forms.Label lblGimnasticar;
        private System.Windows.Forms.Label lblSprava;
        private System.Windows.Forms.ComboBox cmbSprava;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblNijeObavezno;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
    }
}