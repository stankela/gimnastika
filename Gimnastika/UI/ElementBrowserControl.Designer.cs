namespace Gimnastika.UI
{
    partial class ElementBrowserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtNetablicni = new System.Windows.Forms.RadioButton();
            this.rbtTablicni = new System.Windows.Forms.RadioButton();
            this.lblTezina = new System.Windows.Forms.Label();
            this.lblGrupa = new System.Windows.Forms.Label();
            this.lblSprava = new System.Windows.Forms.Label();
            this.cmbGrupa = new System.Windows.Forms.ComboBox();
            this.cmbTezina = new System.Windows.Forms.ComboBox();
            this.cmbSprava = new System.Windows.Forms.ComboBox();
            this.gridViewElementi = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewElementi)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtNetablicni);
            this.groupBox1.Controls.Add(this.rbtTablicni);
            this.groupBox1.Controls.Add(this.lblTezina);
            this.groupBox1.Controls.Add(this.lblGrupa);
            this.groupBox1.Controls.Add(this.lblSprava);
            this.groupBox1.Controls.Add(this.cmbGrupa);
            this.groupBox1.Controls.Add(this.cmbTezina);
            this.groupBox1.Controls.Add(this.cmbSprava);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 74);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // rbtNetablicni
            // 
            this.rbtNetablicni.AutoSize = true;
            this.rbtNetablicni.Location = new System.Drawing.Point(463, 33);
            this.rbtNetablicni.Name = "rbtNetablicni";
            this.rbtNetablicni.Size = new System.Drawing.Size(93, 17);
            this.rbtNetablicni.TabIndex = 6;
            this.rbtNetablicni.TabStop = true;
            this.rbtNetablicni.Text = "Ostali elementi";
            this.rbtNetablicni.UseVisualStyleBackColor = true;
            // 
            // rbtTablicni
            // 
            this.rbtTablicni.AutoSize = true;
            this.rbtTablicni.Location = new System.Drawing.Point(463, 14);
            this.rbtTablicni.Name = "rbtTablicni";
            this.rbtTablicni.Size = new System.Drawing.Size(104, 17);
            this.rbtTablicni.TabIndex = 5;
            this.rbtTablicni.TabStop = true;
            this.rbtTablicni.Text = "Tablicni elementi";
            this.rbtTablicni.UseVisualStyleBackColor = true;
            // 
            // lblTezina
            // 
            this.lblTezina.AutoSize = true;
            this.lblTezina.Location = new System.Drawing.Point(306, 14);
            this.lblTezina.Name = "lblTezina";
            this.lblTezina.Size = new System.Drawing.Size(39, 13);
            this.lblTezina.TabIndex = 25;
            this.lblTezina.Text = "Tezina";
            // 
            // lblGrupa
            // 
            this.lblGrupa.AutoSize = true;
            this.lblGrupa.Location = new System.Drawing.Point(161, 16);
            this.lblGrupa.Name = "lblGrupa";
            this.lblGrupa.Size = new System.Drawing.Size(36, 13);
            this.lblGrupa.TabIndex = 24;
            this.lblGrupa.Text = "Grupa";
            // 
            // lblSprava
            // 
            this.lblSprava.AutoSize = true;
            this.lblSprava.Location = new System.Drawing.Point(16, 16);
            this.lblSprava.Name = "lblSprava";
            this.lblSprava.Size = new System.Drawing.Size(41, 13);
            this.lblSprava.TabIndex = 23;
            this.lblSprava.Text = "Sprava";
            // 
            // cmbGrupa
            // 
            this.cmbGrupa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrupa.FormattingEnabled = true;
            this.cmbGrupa.Location = new System.Drawing.Point(164, 32);
            this.cmbGrupa.Name = "cmbGrupa";
            this.cmbGrupa.Size = new System.Drawing.Size(121, 21);
            this.cmbGrupa.TabIndex = 3;
            this.cmbGrupa.DropDown += new System.EventHandler(this.cmbGrupa_DropDown);
            // 
            // cmbTezina
            // 
            this.cmbTezina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTezina.FormattingEnabled = true;
            this.cmbTezina.Location = new System.Drawing.Point(309, 32);
            this.cmbTezina.Name = "cmbTezina";
            this.cmbTezina.Size = new System.Drawing.Size(121, 21);
            this.cmbTezina.TabIndex = 4;
            // 
            // cmbSprava
            // 
            this.cmbSprava.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSprava.FormattingEnabled = true;
            this.cmbSprava.Location = new System.Drawing.Point(16, 32);
            this.cmbSprava.Name = "cmbSprava";
            this.cmbSprava.Size = new System.Drawing.Size(121, 21);
            this.cmbSprava.TabIndex = 2;
            // 
            // gridViewElementi
            // 
            this.gridViewElementi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewElementi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewElementi.Location = new System.Drawing.Point(0, 90);
            this.gridViewElementi.Name = "gridViewElementi";
            this.gridViewElementi.Size = new System.Drawing.Size(587, 186);
            this.gridViewElementi.TabIndex = 7;
            this.gridViewElementi.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridViewElementi_CellFormatting);
            // 
            // ElementBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gridViewElementi);
            this.Name = "ElementBrowserControl";
            this.Size = new System.Drawing.Size(587, 284);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewElementi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtNetablicni;
        public System.Windows.Forms.RadioButton rbtTablicni;
        private System.Windows.Forms.Label lblTezina;
        private System.Windows.Forms.Label lblGrupa;
        private System.Windows.Forms.Label lblSprava;
        public System.Windows.Forms.ComboBox cmbGrupa;
        public System.Windows.Forms.ComboBox cmbTezina;
        public System.Windows.Forms.ComboBox cmbSprava;
        public System.Windows.Forms.DataGridView gridViewElementi;
    }
}
