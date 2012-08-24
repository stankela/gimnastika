namespace Gimnastika.UI
{
    partial class GrupeForm
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
            this.cmbSprava = new System.Windows.Forms.ComboBox();
            this.cmbGrupa = new System.Windows.Forms.ComboBox();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.txtEngNaziv = new System.Windows.Forms.TextBox();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.lblEngNaziv = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblSprava = new System.Windows.Forms.Label();
            this.lblGrupa = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbSprava
            // 
            this.cmbSprava.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSprava.FormattingEnabled = true;
            this.cmbSprava.Location = new System.Drawing.Point(15, 25);
            this.cmbSprava.Name = "cmbSprava";
            this.cmbSprava.Size = new System.Drawing.Size(123, 21);
            this.cmbSprava.TabIndex = 1;
            // 
            // cmbGrupa
            // 
            this.cmbGrupa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrupa.FormattingEnabled = true;
            this.cmbGrupa.Location = new System.Drawing.Point(165, 25);
            this.cmbGrupa.Name = "cmbGrupa";
            this.cmbGrupa.Size = new System.Drawing.Size(92, 21);
            this.cmbGrupa.TabIndex = 2;
            this.cmbGrupa.DropDown += new System.EventHandler(this.cmbGrupa_DropDown);
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(12, 79);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(297, 20);
            this.txtNaziv.TabIndex = 3;
            // 
            // txtEngNaziv
            // 
            this.txtEngNaziv.Location = new System.Drawing.Point(15, 134);
            this.txtEngNaziv.Name = "txtEngNaziv";
            this.txtEngNaziv.Size = new System.Drawing.Size(294, 20);
            this.txtEngNaziv.TabIndex = 4;
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(9, 63);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(34, 13);
            this.lblNaziv.TabIndex = 4;
            this.lblNaziv.Text = "Naziv";
            // 
            // lblEngNaziv
            // 
            this.lblEngNaziv.AutoSize = true;
            this.lblEngNaziv.Location = new System.Drawing.Point(12, 118);
            this.lblEngNaziv.Name = "lblEngNaziv";
            this.lblEngNaziv.Size = new System.Drawing.Size(75, 13);
            this.lblEngNaziv.TabIndex = 5;
            this.lblEngNaziv.Text = "Engleski naziv";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(149, 172);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblSprava
            // 
            this.lblSprava.AutoSize = true;
            this.lblSprava.Location = new System.Drawing.Point(12, 9);
            this.lblSprava.Name = "lblSprava";
            this.lblSprava.Size = new System.Drawing.Size(41, 13);
            this.lblSprava.TabIndex = 7;
            this.lblSprava.Text = "Sprava";
            // 
            // lblGrupa
            // 
            this.lblGrupa.AutoSize = true;
            this.lblGrupa.Location = new System.Drawing.Point(162, 9);
            this.lblGrupa.Name = "lblGrupa";
            this.lblGrupa.Size = new System.Drawing.Size(36, 13);
            this.lblGrupa.TabIndex = 8;
            this.lblGrupa.Text = "Grupa";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(234, 172);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // GrupeForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(330, 207);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblGrupa);
            this.Controls.Add(this.lblSprava);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblEngNaziv);
            this.Controls.Add(this.lblNaziv);
            this.Controls.Add(this.txtEngNaziv);
            this.Controls.Add(this.txtNaziv);
            this.Controls.Add(this.cmbGrupa);
            this.Controls.Add(this.cmbSprava);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GrupeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GrupeForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GrupeForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSprava;
        private System.Windows.Forms.ComboBox cmbGrupa;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.TextBox txtEngNaziv;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.Label lblEngNaziv;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblSprava;
        private System.Windows.Forms.Label lblGrupa;
        private System.Windows.Forms.Button btnCancel;
    }
}