namespace Gimnastika
{
    partial class OpcijeForm
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
            this.lblPravilo = new System.Windows.Forms.Label();
            this.cmbPravilo = new System.Windows.Forms.ComboBox();
            this.lblVideo = new System.Windows.Forms.Label();
            this.txtVideo = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPromeni = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPravilo
            // 
            this.lblPravilo.AutoSize = true;
            this.lblPravilo.Location = new System.Drawing.Point(15, 22);
            this.lblPravilo.Name = "lblPravilo";
            this.lblPravilo.Size = new System.Drawing.Size(118, 13);
            this.lblPravilo.TabIndex = 0;
            this.lblPravilo.Text = "Podrazumevana pravila";
            // 
            // cmbPravilo
            // 
            this.cmbPravilo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPravilo.FormattingEnabled = true;
            this.cmbPravilo.Location = new System.Drawing.Point(18, 38);
            this.cmbPravilo.Name = "cmbPravilo";
            this.cmbPravilo.Size = new System.Drawing.Size(121, 21);
            this.cmbPravilo.TabIndex = 1;
            // 
            // lblVideo
            // 
            this.lblVideo.AutoSize = true;
            this.lblVideo.Location = new System.Drawing.Point(15, 70);
            this.lblVideo.Name = "lblVideo";
            this.lblVideo.Size = new System.Drawing.Size(148, 13);
            this.lblVideo.TabIndex = 2;
            this.lblVideo.Text = "Program za prikazivanje videa";
            // 
            // txtVideo
            // 
            this.txtVideo.Location = new System.Drawing.Point(18, 88);
            this.txtVideo.Name = "txtVideo";
            this.txtVideo.Size = new System.Drawing.Size(344, 20);
            this.txtVideo.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(275, 127);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(368, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPromeni
            // 
            this.btnPromeni.Location = new System.Drawing.Point(368, 86);
            this.btnPromeni.Name = "btnPromeni";
            this.btnPromeni.Size = new System.Drawing.Size(75, 23);
            this.btnPromeni.TabIndex = 4;
            this.btnPromeni.Text = "Promeni";
            this.btnPromeni.UseVisualStyleBackColor = true;
            this.btnPromeni.Click += new System.EventHandler(this.btnPromeni_Click);
            // 
            // OpcijeForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(457, 162);
            this.Controls.Add(this.btnPromeni);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtVideo);
            this.Controls.Add(this.lblVideo);
            this.Controls.Add(this.cmbPravilo);
            this.Controls.Add(this.lblPravilo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpcijeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpcijeForm";
            this.Shown += new System.EventHandler(this.OpcijeForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPravilo;
        private System.Windows.Forms.ComboBox cmbPravilo;
        private System.Windows.Forms.Label lblVideo;
        private System.Windows.Forms.TextBox txtVideo;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPromeni;
    }
}