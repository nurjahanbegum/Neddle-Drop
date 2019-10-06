using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Attendence.FormReport
{
    public partial class frmrptFestBonusNo : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptFestBonusNo(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlquary = "Exec prcrptFestBonus " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "Criteria";
                dsList.Tables[1].TableName = "prossType";
                dsList.Tables[2].TableName = "Unit";
                dsList.Tables[3].TableName = "section";
                dsList.Tables[4].TableName = "paymode";
                dsList.Tables[5].TableName = "Employee";
                dsList.Tables[6].TableName = "FestCriteria";

                gridFestCriteria.DataSource = dsList.Tables["FestCriteria"];
                gridCriteria.DataSource = dsList.Tables["Criteria"];
                gridUnit.DataSource = dsList.Tables["unit"];
                gridSec.DataSource = dsList.Tables["section"];
                gridPaymode.DataSource = dsList.Tables["paymode"];
                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridProssType.DataSource = dsList.Tables["prossType"];

                //gridSec.DataSource = dsList.Tables["IncType"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void frmrptFestBonusNo_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmrptFestBonusNo_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        private void gridCriteria_AfterRowActivate(object sender, EventArgs e)
        {
            //if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "General".ToUpper())
            //{
            //    group1.Visible = true;
            //   // group2.Visible = false;
            //}
            //else if (gridCriteria.ActiveRow.Cells[0].Text.ToString().ToUpper() != "General".ToUpper())
            //{
            //    group1.Visible = false;
            //   // group2.Visible = true;
            //}
        }



        private void gridCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["SlNo"].Hidden = true;

            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Width = 170;
            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Header.Caption = "Criteria";

            //Change alternate color
            gridCriteria.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridCriteria.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridCriteria.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridCriteria.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridCriteria.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridCriteria.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 195;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSec.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["empCode"].Width = 110;
            gridSec.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridSec.DisplayLayout.Bands[0].Columns["EmpName"].Width = 242;
            gridSec.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Festival Bonus Information...";

                if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "FestSheet".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptFestBonusDetails.rdlc";
                    SQLQuery = "Exec [rptFestBonus] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + gridSec.ActiveRow.Cells[0].Value.ToString() + "', '" + gridPaymode.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "'";
                }
                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Summary".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptFestBonusSum.rdlc";
                    SQLQuery = "Exec [rptFestBonus] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + gridSec.ActiveRow.Cells[0].Value.ToString() + "', '" + gridPaymode.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "', 'Summary'";
                }


                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);

                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = SQLQuery;
                //clsReport.strDSNMain = DataSourceName;

                //FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridPaymode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridPaymode.DisplayLayout.Bands[0].Columns["PayMode"].Width = 170;
            //Change alternate color
            gridPaymode.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridPaymode.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridPaymode.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridPaymode.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridPaymode.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridPaymode.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmployee_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployee.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Width = 110;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Width = 230;
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmployee.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmployee.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmployee.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmployee.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmployee.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridEmployee.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }



        private void gridUnit_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridUnit.DisplayLayout.Bands[0].Columns["unitid"].Hidden = true;
            // Grid Width
            gridUnit.DisplayLayout.Bands[0].Columns["Unit"].Width = 145;
            //Change alternate color
            gridUnit.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridUnit.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridUnit.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridUnit.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridUnit.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridUnit.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {            
            gridProssType.DisplayLayout.Bands[0].Columns["dtProcess"].Hidden = true;
            //Grid Width
            gridProssType.DisplayLayout.Bands[0].Columns["Description1"].Width = 235;
            //Change alternate color
            gridProssType.DisplayLayout.Bands[0].Columns["Description1"].Header.Caption = "Bonus Date";
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridProssType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridProssType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridProssType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridProssType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridFestCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridFestCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Width = 170;
            gridFestCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Header.Caption = "Bonus Type";
            //Change alternate color
            gridFestCriteria.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridFestCriteria.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridFestCriteria.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridFestCriteria.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridFestCriteria.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridFestCriteria.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }
   }
}