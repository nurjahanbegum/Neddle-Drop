using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;


namespace GTRHRIS.HK.FormEntry
{
    public partial class frmMonth : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmMonth(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmMonth_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetail = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboYear_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnShow_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }


        private void btnShow_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            try
            {
                string yr = cboYear.Text.ToString();
                prcLoadList(cboYear.Text.ToString());
                prcLoadCombo();

                prcClearData();
                cboYear.Focus();
                cboYear.Value = yr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Boolean fncBlank()
        {
            if( cboYear.Text.Trim().ToString().Length==0)
            {
                MessageBox.Show("Please Provide Year.");
                cboYear.Focus();
                return true;
            }
            return false;
        }
        public void prcLoadList(string strYear)
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetMonth  " + strYear;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "Year";
                dsList.Tables[1].TableName = "Month";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Month"];
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }
        }
        public void prcLoadCombo()
        {
            try
            {
                cboYear .DataSource = null;
                cboYear.DataSource = dsList.Tables["Year"];
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void prcClearData()
        {
            cboYear.Text = "";
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["MonthName"].Header.Caption = "Month Name";
            gridList.DisplayLayout.Bands[0].Columns["YearName"].Header.Caption = "Year";
            gridList.DisplayLayout.Bands[0].Columns["BeginDate"].Header.Caption = "Begin Date";
            gridList.DisplayLayout.Bands[0].Columns["EndDate"].Header.Caption = "End Date";
            gridList.DisplayLayout.Bands[0].Columns["TotalDays"].Header.Caption = "Total Days";

            //Date Formate
            gridList.DisplayLayout.Bands[0].Columns["BeginDate"].Format = "dd.MMM.yyyy";
            gridList.DisplayLayout.Bands[0].Columns["EndDate"].Format = "dd.MMM.yyyy";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["MonthName"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["YearName"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["BeginDate"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["EndDate"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["TotalDays"].Width = 150;
            
            //Change alternate color
            gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void cboYear_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //set Caption
            cboYear.DisplayLayout.Bands[0].Columns["YearName"].Header.Caption = "Year Name";

            //set Width
            cboYear.DisplayLayout.Bands[0].Columns["YearName"].Width = cboYear.Width;

            //initialize members
            cboYear.DisplayMember = "YearName";
            cboYear.ValueMember = "YearName";
        }

        private void frmMonth_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList("0");
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
