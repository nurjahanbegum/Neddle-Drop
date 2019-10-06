using System;
using System.Collections;
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

namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmCasualAttBonus : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmCasualAttBonus(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptDailyCasual  " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Company";
                dsList.Tables[1].TableName = "Type";
                dsList.Tables[2].TableName = "Employee";

                gridCompany.DataSource = dsList.Tables["Company"];
                gridType.DataSource = dsList.Tables["Type"];
                gridEmployeeID.DataSource = dsList.Tables["Employee"];

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtDateFrom.Value = firstDay;

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddMonths(1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtDateTo.Value = lastDay;
                txtHoliday.Value = 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridCompany_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCompany.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridCompany.DisplayLayout.Bands[0].Columns["SectName"].Width = 221;
            gridCompany.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Company";

            //Change alternate color
            gridCompany.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridCompany.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridCompany.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridCompany.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridCompany.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridEmployeeID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["isChecked"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 215;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmployeeID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmployeeID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmployeeID.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }
        
        private void prcShowReport()
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Employee Details Information...";

                string CompanyId = "0", EmpId = "0", EmpType = "";

                EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                    
                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Company".ToUpper())
                {
                    CompanyId = gridCompany.ActiveRow.Cells["SectId"].Value.ToString();
                }
                if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                }
                rptQuery = "Exec rptCasualAttendance " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + CompanyId + "," + EmpId + ",'" + EmpType + "','" + txtHoliday.Text.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails,rptQuery);

                //SQLQuery = "Exec rptSalaryCasual 3,'10-Nov-13','16-Nov-2013', 0, 0, 'Casual Worker'";

                if(dsDetails.Tables[0].Rows.Count==0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);

                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = rptQuery;
                //clsReport.strDSNMain = DataSourceName;
                //clsReport.dsReport = dsDetails;

                //FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                rptQuery = null;
                DataSourceName = null;
                DataSourceName = null;
                ReportPath = null;
                dsDetails = null;
            }
        }
        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {                        
            gridCompany.Enabled = true;
            gridEmployeeID.Enabled = true;

            if(optCriteria.Value.ToString() =="All")
            {
                gridCompany.Enabled = false;
                gridEmployeeID.Enabled = false;                
            }
            else if (optCriteria.Value.ToString() == "Company")
            {
                gridEmployeeID.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Employee")
            {
                gridCompany.Enabled = false;
            }
        }
        private void frmCasualAttBonus_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmCasualAttBonus_Load(object sender, EventArgs e)
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptCasualAttBonus.rdlc";
            prcShowReport();
        }

        private void gridType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridType.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
            gridType.DisplayLayout.Bands[0].Columns["varName"].Width = 221;
            gridType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Employee Type";

            //Change alternate color
            gridType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }



      }
  }

