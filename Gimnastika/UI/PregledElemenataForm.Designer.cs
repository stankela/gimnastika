namespace Gimnastika.UI
{
    partial class PregledElemenataForm
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
            this.panelSlika = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnZatvori = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrikazi = new System.Windows.Forms.Button();
            this.selektujElementeControl1 = new Gimnastika.UI.SelektujElementeControl();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSlika
            // 
            this.panelSlika.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSlika.Location = new System.Drawing.Point(0, 0);
            this.panelSlika.Name = "panelSlika";
            this.panelSlika.Size = new System.Drawing.Size(697, 446);
            this.panelSlika.TabIndex = 1;
            this.panelSlika.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSlika_Paint);
            this.panelSlika.Resize += new System.EventHandler(this.panelSlika_Resize);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnZatvori);
            this.panel2.Controls.Add(this.btnLast);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.btnPrevious);
            this.panel2.Controls.Add(this.btnFirst);
            this.panel2.Controls.Add(this.btnPrikazi);
            this.panel2.Controls.Add(this.selektujElementeControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(697, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(306, 446);
            this.panel2.TabIndex = 2;
            // 
            // btnZatvori
            // 
            this.btnZatvori.Location = new System.Drawing.Point(14, 383);
            this.btnZatvori.Name = "btnZatvori";
            this.btnZatvori.Size = new System.Drawing.Size(75, 23);
            this.btnZatvori.TabIndex = 24;
            this.btnZatvori.Text = "Zatvori";
            this.btnZatvori.UseVisualStyleBackColor = true;
            this.btnZatvori.Click += new System.EventHandler(this.btnZatvori_Click);
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(184, 338);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(32, 23);
            this.btnLast.TabIndex = 23;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(151, 338);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 22;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(78, 340);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(72, 20);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = "1";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(46, 338);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(32, 23);
            this.btnPrevious.TabIndex = 21;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(14, 338);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(32, 23);
            this.btnFirst.TabIndex = 20;
            this.btnFirst.Text = "<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnPrikazi
            // 
            this.btnPrikazi.Location = new System.Drawing.Point(14, 277);
            this.btnPrikazi.Name = "btnPrikazi";
            this.btnPrikazi.Size = new System.Drawing.Size(136, 23);
            this.btnPrikazi.TabIndex = 1;
            this.btnPrikazi.Text = "Prikazi elemente";
            this.btnPrikazi.UseVisualStyleBackColor = true;
            this.btnPrikazi.Click += new System.EventHandler(this.btnPrikazi_Click);
            // 
            // selektujElementeControl1
            // 
            this.selektujElementeControl1.Location = new System.Drawing.Point(14, 11);
            this.selektujElementeControl1.Name = "selektujElementeControl1";
            this.selektujElementeControl1.Size = new System.Drawing.Size(279, 260);
            this.selektujElementeControl1.TabIndex = 0;
            // 
            // PregledElemenataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 446);
            this.Controls.Add(this.panelSlika);
            this.Controls.Add(this.panel2);
            this.Name = "PregledElemenataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pregled elemenata";
            this.Load += new System.EventHandler(this.PregledElemenataForm_Load);
            this.Shown += new System.EventHandler(this.PregledElemenataForm_Shown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSlika;
        private System.Windows.Forms.Panel panel2;
        private SelektujElementeControl selektujElementeControl1;
        private System.Windows.Forms.Button btnPrikazi;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnZatvori;
    }
}