using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmDLIClock : Form
    {
        private string strFileLoc = Common.Classes.clsMain.AppPath + @"\Download\";
        private string strFileName = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        clsProcedure clsProc = new clsProcedure();
        Common.FormEntry.frmMaster FM;

        public frmDLIClock(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (fncBlank())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCollectData_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCollectData_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
            uTab = null;
        }

        private void prcClearData()
        {
            this.btnSave.Text = "&Process";
        }

        private Boolean fncBlank()
        {
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
