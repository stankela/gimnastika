namespace Gimnastika.UI
{
    partial class VezbaEditorForm
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
            this.gridElementi = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridElementi)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridElementi);
            this.panel1.Controls.SetChildIndex(this.gridElementi, 0);
            this.panel1.Controls.SetChildIndex(this.panel2, 0);
            // 
            // gridElementi
            // 
            this.gridElementi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridElementi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridElementi.Location = new System.Drawing.Point(0, 60);
            this.gridElementi.Name = "gridElementi";
            this.gridElementi.Size = new System.Drawing.Size(693, 261);
            this.gridElementi.TabIndex = 1;
            this.gridElementi.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridElementi_CellValueChanged);
            this.gridElementi.Leave += new System.EventHandler(this.gridElementi_Leave);
            this.gridElementi.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.gridElementi_CellParsing);
            this.gridElementi.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridElementi_CellFormatting);
            this.gridElementi.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridElementi_MouseUp);
            this.gridElementi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridElementi_CellClick);
            // 
            // VezbaEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 370);
            this.Name = "VezbaEditorForm";
            this.Text = "VezbaEditorForm";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridElementi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridElementi;
    }
}