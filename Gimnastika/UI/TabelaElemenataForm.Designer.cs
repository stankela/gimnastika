namespace Gimnastika.UI
{
    partial class TabelaElemenataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabelaElemenataForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVelicinaSlike = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnNoviElement = new System.Windows.Forms.Button();
            this.txtZoom = new System.Windows.Forms.TextBox();
            this.btnZoomDropDown = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblZumiraj = new System.Windows.Forms.Label();
            this.lblGrupa = new System.Windows.Forms.Label();
            this.lblSprava = new System.Windows.Forms.Label();
            this.cmbGrupa = new System.Windows.Forms.ComboBox();
            this.cmbSprava = new System.Windows.Forms.ComboBox();
            this.panelTabela = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.contextMenuTabela = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnDodajElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPromeniElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPromeniVelicinuSlike = new System.Windows.Forms.ToolStripMenuItem();
            this.mnBrisiElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnIzaberiElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnIzaberiVarijantu = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.contextMenuTabela.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblVelicinaSlike);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnNoviElement);
            this.panel1.Controls.Add(this.txtZoom);
            this.panel1.Controls.Add(this.btnZoomDropDown);
            this.panel1.Controls.Add(this.lblZumiraj);
            this.panel1.Controls.Add(this.lblGrupa);
            this.panel1.Controls.Add(this.lblSprava);
            this.panel1.Controls.Add(this.cmbGrupa);
            this.panel1.Controls.Add(this.cmbSprava);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 59);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // lblVelicinaSlike
            // 
            this.lblVelicinaSlike.AutoSize = true;
            this.lblVelicinaSlike.Location = new System.Drawing.Point(473, 9);
            this.lblVelicinaSlike.Name = "lblVelicinaSlike";
            this.lblVelicinaSlike.Size = new System.Drawing.Size(68, 13);
            this.lblVelicinaSlike.TabIndex = 11;
            this.lblVelicinaSlike.Text = "Velicina slike";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(467, 25);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 28);
            this.trackBar1.TabIndex = 10;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 10;
            this.trackBar1.MouseLeave += new System.EventHandler(this.trackBar1_MouseLeave);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(605, 23);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnNoviElement
            // 
            this.btnNoviElement.Location = new System.Drawing.Point(356, 22);
            this.btnNoviElement.Name = "btnNoviElement";
            this.btnNoviElement.Size = new System.Drawing.Size(91, 23);
            this.btnNoviElement.TabIndex = 8;
            this.btnNoviElement.Text = "Novi element";
            this.btnNoviElement.UseVisualStyleBackColor = true;
            this.btnNoviElement.Click += new System.EventHandler(this.btnNoviElement_Click);
            // 
            // txtZoom
            // 
            this.txtZoom.Location = new System.Drawing.Point(255, 25);
            this.txtZoom.Name = "txtZoom";
            this.txtZoom.Size = new System.Drawing.Size(66, 20);
            this.txtZoom.TabIndex = 7;
            this.txtZoom.Leave += new System.EventHandler(this.txtZoom_Leave);
            this.txtZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZoom_KeyPress);
            // 
            // btnZoomDropDown
            // 
            this.btnZoomDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnZoomDropDown.ImageIndex = 0;
            this.btnZoomDropDown.ImageList = this.imageList1;
            this.btnZoomDropDown.Location = new System.Drawing.Point(320, 25);
            this.btnZoomDropDown.Name = "btnZoomDropDown";
            this.btnZoomDropDown.Size = new System.Drawing.Size(20, 20);
            this.btnZoomDropDown.TabIndex = 6;
            this.btnZoomDropDown.UseVisualStyleBackColor = true;
            this.btnZoomDropDown.Click += new System.EventHandler(this.btnZoomDropDown_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "DropDown.bmp");
            // 
            // lblZumiraj
            // 
            this.lblZumiraj.AutoSize = true;
            this.lblZumiraj.Location = new System.Drawing.Point(252, 8);
            this.lblZumiraj.Name = "lblZumiraj";
            this.lblZumiraj.Size = new System.Drawing.Size(41, 13);
            this.lblZumiraj.TabIndex = 5;
            this.lblZumiraj.Text = "Zumiraj";
            // 
            // lblGrupa
            // 
            this.lblGrupa.AutoSize = true;
            this.lblGrupa.Location = new System.Drawing.Point(144, 9);
            this.lblGrupa.Name = "lblGrupa";
            this.lblGrupa.Size = new System.Drawing.Size(88, 13);
            this.lblGrupa.TabIndex = 3;
            this.lblGrupa.Text = "Grupa elemenata";
            // 
            // lblSprava
            // 
            this.lblSprava.AutoSize = true;
            this.lblSprava.Location = new System.Drawing.Point(9, 9);
            this.lblSprava.Name = "lblSprava";
            this.lblSprava.Size = new System.Drawing.Size(41, 13);
            this.lblSprava.TabIndex = 2;
            this.lblSprava.Text = "Sprava";
            // 
            // cmbGrupa
            // 
            this.cmbGrupa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrupa.FormattingEnabled = true;
            this.cmbGrupa.Location = new System.Drawing.Point(147, 25);
            this.cmbGrupa.Name = "cmbGrupa";
            this.cmbGrupa.Size = new System.Drawing.Size(93, 21);
            this.cmbGrupa.TabIndex = 1;
            this.cmbGrupa.TabStop = false;
            this.cmbGrupa.DropDownClosed += new System.EventHandler(this.cmbGrupa_DropDownClosed);
            this.cmbGrupa.DropDown += new System.EventHandler(this.cmbGrupa_DropDown);
            // 
            // cmbSprava
            // 
            this.cmbSprava.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSprava.FormattingEnabled = true;
            this.cmbSprava.Location = new System.Drawing.Point(12, 25);
            this.cmbSprava.Name = "cmbSprava";
            this.cmbSprava.Size = new System.Drawing.Size(120, 21);
            this.cmbSprava.TabIndex = 0;
            this.cmbSprava.TabStop = false;
            this.cmbSprava.DropDownClosed += new System.EventHandler(this.cmbSprava_DropDownClosed);
            this.cmbSprava.DropDown += new System.EventHandler(this.cmbSprava_DropDown);
            // 
            // panelTabela
            // 
            this.panelTabela.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTabela.Location = new System.Drawing.Point(0, 106);
            this.panelTabela.Name = "panelTabela";
            this.panelTabela.Size = new System.Drawing.Size(706, 323);
            this.panelTabela.TabIndex = 1;
            this.panelTabela.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTabela_Paint);
            this.panelTabela.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelTabela_Scroll);
            this.panelTabela.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTabela_MouseDown);
            this.panelTabela.Resize += new System.EventHandler(this.panelTabela_Resize);
            this.panelTabela.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTabela_MouseUp);
            // 
            // panelHeader
            // 
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 59);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(706, 47);
            this.panelHeader.TabIndex = 3;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            this.panelHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelHeader_MouseDown);
            // 
            // contextMenuTabela
            // 
            this.contextMenuTabela.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDodajElement,
            this.mnPromeniElement,
            this.mnPromeniVelicinuSlike,
            this.mnBrisiElement,
            this.mnSeparator1,
            this.mnCut,
            this.mnPaste,
            this.mnSeparator2,
            this.mnIzaberiElement,
            this.mnIzaberiVarijantu});
            this.contextMenuTabela.Name = "contextMenuTabela";
            this.contextMenuTabela.Size = new System.Drawing.Size(185, 192);
            // 
            // mnDodajElement
            // 
            this.mnDodajElement.Name = "mnDodajElement";
            this.mnDodajElement.Size = new System.Drawing.Size(184, 22);
            this.mnDodajElement.Text = "Dodaj novi element";
            this.mnDodajElement.Click += new System.EventHandler(this.mnDodajElement_Click);
            // 
            // mnPromeniElement
            // 
            this.mnPromeniElement.Name = "mnPromeniElement";
            this.mnPromeniElement.Size = new System.Drawing.Size(184, 22);
            this.mnPromeniElement.Text = "Promeni element";
            this.mnPromeniElement.Click += new System.EventHandler(this.mnPromeniElement_Click);
            // 
            // mnPromeniVelicinuSlike
            // 
            this.mnPromeniVelicinuSlike.Name = "mnPromeniVelicinuSlike";
            this.mnPromeniVelicinuSlike.Size = new System.Drawing.Size(184, 22);
            this.mnPromeniVelicinuSlike.Text = "Promeni velicinu slike";
            this.mnPromeniVelicinuSlike.Click += new System.EventHandler(this.mnPromeniVelicinuSlike_Click);
            // 
            // mnBrisiElement
            // 
            this.mnBrisiElement.Name = "mnBrisiElement";
            this.mnBrisiElement.Size = new System.Drawing.Size(184, 22);
            this.mnBrisiElement.Text = "Brisi element";
            this.mnBrisiElement.Click += new System.EventHandler(this.mnBrisiElement_Click);
            // 
            // mnSeparator1
            // 
            this.mnSeparator1.Name = "mnSeparator1";
            this.mnSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // mnCut
            // 
            this.mnCut.Name = "mnCut";
            this.mnCut.Size = new System.Drawing.Size(184, 22);
            this.mnCut.Text = "Cut";
            this.mnCut.Click += new System.EventHandler(this.mnCut_Click);
            // 
            // mnPaste
            // 
            this.mnPaste.Name = "mnPaste";
            this.mnPaste.Size = new System.Drawing.Size(184, 22);
            this.mnPaste.Text = "Paste";
            this.mnPaste.Click += new System.EventHandler(this.mnPaste_Click);
            // 
            // mnSeparator2
            // 
            this.mnSeparator2.Name = "mnSeparator2";
            this.mnSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // mnIzaberiElement
            // 
            this.mnIzaberiElement.Name = "mnIzaberiElement";
            this.mnIzaberiElement.Size = new System.Drawing.Size(184, 22);
            this.mnIzaberiElement.Text = "Izaberi element";
            this.mnIzaberiElement.Click += new System.EventHandler(this.mnIzaberiElement_Click);
            // 
            // mnIzaberiVarijantu
            // 
            this.mnIzaberiVarijantu.Name = "mnIzaberiVarijantu";
            this.mnIzaberiVarijantu.Size = new System.Drawing.Size(184, 22);
            this.mnIzaberiVarijantu.Text = "Izaberi varijantu";
            this.mnIzaberiVarijantu.Click += new System.EventHandler(this.mnIzaberiVarijantu_Click);
            // 
            // TabelaElemenataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 429);
            this.Controls.Add(this.panelTabela);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panel1);
            this.Name = "TabelaElemenataForm";
            this.Text = "TabelaElemenataForm";
            this.Shown += new System.EventHandler(this.TabelaElemenataForm_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TabelaElemenataForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.contextMenuTabela.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelTabela;
        private System.Windows.Forms.ComboBox cmbGrupa;
        private System.Windows.Forms.ComboBox cmbSprava;
        private System.Windows.Forms.Label lblGrupa;
        private System.Windows.Forms.Label lblSprava;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblZumiraj;
        private System.Windows.Forms.TextBox txtZoom;
        private System.Windows.Forms.Button btnZoomDropDown;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuTabela;
        private System.Windows.Forms.ToolStripMenuItem mnDodajElement;
        private System.Windows.Forms.ToolStripMenuItem mnPromeniElement;
        private System.Windows.Forms.ToolStripMenuItem mnBrisiElement;
        private System.Windows.Forms.Button btnNoviElement;
        private System.Windows.Forms.ToolStripSeparator mnSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnCut;
        private System.Windows.Forms.ToolStripMenuItem mnPaste;
        private System.Windows.Forms.ToolStripSeparator mnSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnIzaberiElement;
        private System.Windows.Forms.ToolStripMenuItem mnIzaberiVarijantu;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label lblVelicinaSlike;
        private System.Windows.Forms.ToolStripMenuItem mnPromeniVelicinuSlike;
    }
}