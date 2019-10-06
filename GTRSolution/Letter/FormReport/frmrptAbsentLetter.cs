using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Letter.FormReport
{
    public partial class frmrptAbsentLetter : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptAbsentLetter(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptLetterAbsent " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                dsList.Tables[0].TableName = "Criteria";
                dsList.Tables[1].TableName = "type";
                dsList.Tables[2].TableName = "EmpStatus";
                dsList.Tables[3].TableName = "EmpType";
                dsList.Tables[4].TableName = "Section";
                dsList.Tables[5].TableName = "Employee";


                gridCriteria.DataSource = dsList.Tables["Criteria"];
                gridEmpStatus.DataSource = dsList.Tables["EmpStatus"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                gridArea.DataSource = dsList.Tables["Section"];
                gridEmp.DataSource = dsList.Tables["Employee"];
                gridIncrType.DataSource = dsList.Tables["type"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clscon = null;
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void frmrptSales_Load(object sender, EventArgs e)
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

        private void frmrptSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridArea_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridArea.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            //gridArea.DisplayLayout.Bands[0].Columns["SLNO"].Hidden = true;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Width = 160;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridArea.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridArea.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridArea.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridArea.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridArea.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridArea.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string SQLQuery = "", ReportPath = "", Criteria = "", Status = "", SectId = "0", type = "", Lettertype = "", EmpId = "0";

                Criteria = gridCriteria.ActiveRow.Cells["CValue"].Value.ToString();
                Lettertype = gridIncrType.ActiveRow.Cells["CValue"].Value.ToString();
                Status = gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString();
                if (Criteria == "Section")
                {
                    SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                }
                type = gridEmpType.ActiveRow.Cells["EmpType"].Value.ToString();
                if (Criteria == "Employee")
                {
                    EmpId = gridEmp.ActiveRow.Cells["EmpId"].Value.ToString();
                }

                if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "First")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAbsent1.rdlc";
                    SQLQuery = "Exec rptLetterAbsent " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "' ";
                }
                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Second")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAbsent2.rdlc";
                    SQLQuery = "Exec rptLetterAbsent " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "' ";
                }
                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Third")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAbsent3.rdlc";
                    SQLQuery = "Exec rptLetterAbsent " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "' ";
                }

                //else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Dismiss")
                //{
                //    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptSuspendDismiss.rdlc";
                //    SQLQuery = "Exec rptLetterSuspend " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId + ",'" + Lettertype + "' ";
                //}

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                string DataSourceName = "DataSet1";
                string FormCaption = "Report :: Suspend Letter...";

                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                clsReport.strQueryMain = SQLQuery;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
            }
        }

        private void gridArea_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyValue);
        }

        private void gridCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Width = 150;
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

        private void gridEmpStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            //gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridEmpStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Width = 150;
            gridEmpStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Header.Caption = "Employee Status";

            //Change alternate color
            gridEmpStatus.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmpStatus.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmpStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmpStatus.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmpStatus.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridEmpStatus.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            //gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Width = 150;
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Employee Type";

            //Change alternate color
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmpType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmpType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmpType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridEmpType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmp.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 130;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 245;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmp.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            //Filtering

            this.gridEmp.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridIncrType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridIncrType.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridIncrType.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridIncrType.DisplayLayout.Bands[0].Columns["Type"].Width = 150;
            gridIncrType.DisplayLayout.Bands[0].Columns["Type"].Header.Caption = "Letter Type";

            //Change alternate color
            gridIncrType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridIncrType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridIncrType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridIncrType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridIncrType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridIncrType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridCriteria_AfterRowActivate(object sender, EventArgs e)
        {
            if(gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper()=="Employee Wise".ToUpper())
            {
                gridEmp.Enabled = true;
                gridArea.Enabled = false;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Section Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = true;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Joining Date Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
                group1.Visible = true;
            }
            else
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
                group1.Visible = false;
            }
        }

        private void gridEmpStatus_AfterRowActivate(object sender, EventArgs e)
        {
            if (gridEmpStatus.ActiveRow.Cells[0].Value.ToString().ToUpper() == "=ALL=" || gridEmpStatus.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Current".ToUpper())
            {
                group1.Visible = false;
                dtFrom.Value = "1-Jan-1990";
                dtTo.Value = DateTime.Now;
            }
             else
            {
                group1.Visible = true;
            }
            
        }
    }
}