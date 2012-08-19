using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Gimnastika.Domain;

namespace Gimnastika
{
    public partial class SelektujElementeControl : UserControl
    {
        private CheckBox[] checkBoxesGrupe;
        private CheckBox[] checkBoxesTezine;

        public SelektujElementeControl()
        {
            InitializeComponent();
            initUI();
        }

        private void initUI()
        {
            checkBoxesGrupe = new CheckBox[] { chbSveGrupe, chb1, chb2, chb3, chb4, chb5 };
            checkBoxesTezine = new CheckBox[] { chbSveTezine, chbA, chbB, chbC, chbD, chbE,
                chbE, chbF, chbG};

            foreach (CheckBox chb in checkBoxesTezine)
                chb.CheckedChanged += chbTezine_CheckedChanged;
            foreach (CheckBox chb in checkBoxesGrupe)
                chb.CheckedChanged += chbGrupe_CheckedChanged;
        }

        public void resetuj()
        {
            resetujGrupe();
            resetujTezine();
        }

        private void resetujGrupe()
        {
            foreach (CheckBox chb in checkBoxesGrupe)
                chb.Checked = false;
        }

        private void resetujTezine()
        {
            foreach (CheckBox chb in checkBoxesTezine)
                chb.Checked = false;
        }

        private void resetujOstaleGrupe(CheckBox neDiraj)
        {
            foreach (CheckBox chb in checkBoxesGrupe)
            {
                if (chb != neDiraj)
                    chb.Checked = false;
            }
        }

        private void resetujOstaleTezine(CheckBox neDiraj)
        {
            foreach (CheckBox chb in checkBoxesTezine)
            {
                if (chb != neDiraj)
                    chb.Checked = false;
            }
        }

        public List<TezinaElementa> getSelektovaneTezine()
        {
         List<TezinaElementa> result = new List<TezinaElementa>();
         if (chbSveTezine.Checked)
         {
             result.AddRange(new TezinaElementa[] {
                 TezinaElementa.A, TezinaElementa.B, TezinaElementa.C,
                 TezinaElementa.D, TezinaElementa.E, TezinaElementa.F,
                 TezinaElementa.G});
         }
         else
         {
             if (chbA.Checked)
                 result.Add(TezinaElementa.A);
             if (chbB.Checked)
                 result.Add(TezinaElementa.B);
             if (chbC.Checked)
                 result.Add(TezinaElementa.C);
             if (chbD.Checked)
                 result.Add(TezinaElementa.D);
             if (chbE.Checked)
                 result.Add(TezinaElementa.E);
             if (chbF.Checked)
                 result.Add(TezinaElementa.F);
             if (chbG.Checked)
                 result.Add(TezinaElementa.G);
         }
         return result;
        }

        public List<GrupaElementa> getSelektovaneGrupe()
        {
         List<GrupaElementa> result = new List<GrupaElementa>();
         if (chbSveGrupe.Checked)
         {
             result.AddRange(new GrupaElementa[] { 
                 GrupaElementa.I, GrupaElementa.II, GrupaElementa.III,
                 GrupaElementa.IV, GrupaElementa.V});
         }
         else
         {
             if (chb1.Checked)
                 result.Add(GrupaElementa.I);
             if (chb2.Checked)
                 result.Add(GrupaElementa.II);
             if (chb3.Checked)
                 result.Add(GrupaElementa.III);
             if (chb4.Checked)
                 result.Add(GrupaElementa.IV);
             if (chb5.Checked)
                 result.Add(GrupaElementa.V);
         }
         return result;
        }

        private void chbTezine_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb = (CheckBox)sender;
            if (chb.Checked == false)
                return;
            if (chb == chbSveTezine)
                resetujOstaleTezine(chb);
            else
                chbSveTezine.Checked = false;
        }

        private void chbGrupe_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb = (CheckBox)sender;
            if (chb.Checked == false)
                return;
            if (chb == chbSveGrupe)
                resetujOstaleGrupe(chb);
            else
                chbSveGrupe.Checked = false;
        }
    }
}
