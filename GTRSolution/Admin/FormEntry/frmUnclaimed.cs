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
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmUnclaimed : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmUnclaimed(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '','',0,0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "Employee";
                dsList.Tables[2].TableName = "AllowType";
                dsList.Tables[3].TableName = "Grid";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridDetails.DataSource = dsList.Tables["Grid"];

                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void prcLoadSaveData()
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlquary = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '','',0,0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[1].TableName = "Employee";
                dsList.Tables[2].TableName = "AllowType";
                dsList.Tables[3].TableName = "Grid";

                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridDetails.DataSource = dsList.Tables["Grid"];


            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void prcLoadCombo()
        {
            try
            {

                cboType.DataSource = dsList.Tables["AllowType"];

                cboType.Text = "Transport";


            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void cboType_ValueChanged(object sender, EventArgs e)
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlquary = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '" + cboType.Text.ToString() + "','',0,1";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "ProssType";

                gridProssType.DataSource = dsList.Tables["ProssType"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void frmUnclaimed_Load(object sender, EventArgs e)
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

        private void frmUnclaimed_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
      

        private void btnPreview_Click(object sender, EventArgs e)
        {

            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            
            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Unclaimed..";


                ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptUnclaimed.rdlc";
                SQLQuery = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '" + cboType.Text.ToString() + "','" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "',0,3";

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 285;
            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "Process Type";
           
            //Change alternate color
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
            //gridProssType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

        }


        private void optCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void btnPreview_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["AllowName"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["AllowName"].Header.Caption = "Allowance Type";
            cboType.DisplayLayout.Bands[0].Columns["AllowID"].Hidden = true;
            cboType.DisplayMember = "AllowName";
            cboType.ValueMember = "AllowName";

        }

     
        private void gridEmployee_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployee.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Width = 75;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Width = 160;
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //create Check Box in List
            //gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Style = ColumnStyle.CheckBox;
            // gridEmployee.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.Always;

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
            //gridEmployee.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].AllowRowFiltering = DefaultableBoolean.False;
        }

    

        private void GridToToExcel_InitializeColumn(object sender, InitializeColumnEventArgs e)
        {
            try
            {
                if (e.Column.DataType == typeof(System.DateTime?) && e.Column.Format != null)
                {
                    e.ExcelFormatStr = e.Column.Format.Replace("tt", "AM/PM");
                }
                else
                {
                    e.ExcelFormatStr = e.Column.Format;
                }
            }
            catch (Exception ex)
            {
                //ExceptionFramework.ExceptionPolicy.HandleException(ex, "DefaultPolicy");
            }
        }
        
        private void btnExcel_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string SQLQuery = "";

            SQLQuery = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '" + cboType.Text.ToString() + "','" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "',4";

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                dsDetails.Tables[0].TableName = "Rpt";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsDetails.Tables["Rpt"];


            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Unclaimed Employee Details" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            Application.DoEvents();
            UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            GridToToExcel.Export(gridExcel, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }

        private void btnExcelType_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string SQLQuery = "";

            SQLQuery = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '" + cboType.Text.ToString() + "','" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "',3";

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

            dsDetails.Tables[0].TableName = "Rpt";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsDetails.Tables["Rpt"];


            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = cboType.Text.ToString() + "-" +gridProssType.ActiveRow.Cells[0].Value.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            Application.DoEvents();
            UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            GridToToExcel.Export(gridExcel, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }

      
        
        private void gridExcel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Change alternate color
            gridExcel.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridExcel.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridExcel.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridExcel.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridExcel.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void gridDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                gridDetails.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["ProssType"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["dtDate"].Hidden = true;

                ////Set Caption
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Amount";
                gridDetails.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 120;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 80;
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Width = 85;
                gridDetails.DisplayLayout.Bands[0].Columns["Remarks"].Width = 110;

                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].CellActivation = Activation.NoEdit;


                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                ////Select Full Row when click on any cell
                ////e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                ////Selection Style Will Be Row Selector
                ////gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////Stop Updating
                ////gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                gridDetails.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            clsConnection clsCon = new clsConnection();
            dsList = new DataSet();

            try
            {
                string sqlquary = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '" + cboType.Text.ToString() + "','" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "',0,2";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "Grid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];



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

        private Boolean fncBlank()
        {


            if (cboType.Text.Length == 0)
            {
                MessageBox.Show("Please provide Report Type.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboType.Focus();
                return true;
            }



            return false;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }


            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {
                //Member Master Table
                if (btnSave.Text.ToString() == "&Save")
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Delete  tblUnclaimed Where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() +
                                       "' and ComID = " + Common.Classes.clsMain.intComId + " and AllowType = '" + cboType.Text.ToString() +
                                       "' and PaidYN = 1";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblUnclaimed(EmpId,dtDate,ProssType,AllowType,TotalAmount,Remarks,Luserid,PCName,ComId,PaidYN) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["dtDate"].Text.ToString() + "','"
                                       + row.Cells["ProssType"].Value.ToString() + "','" + cboType.Text.ToString() + "','" 
                                       + row.Cells["Amount"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "'," 
                                       + Common.Classes.clsMain.intUserId + ",'" + Common.Classes.clsMain.strComputerName + "'," 
                                       + Common.Classes.clsMain.intComId + ",1)";
                            arQuery.Add(sqlQuery);

                        }
                    }


                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully.");
                }
                //prcClearData();
                prcLoadSaveData();
                //PrcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {


            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {
                //Member Master Table
                if (btnDelete.Text.ToString() == "&Delete")
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Update tblUnclaimed Set PaidYN = '0' Where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() +
                                       "' and ComID = " + Common.Classes.clsMain.intComId + " and AllowType = '" + cboType.Text.ToString() +
                                       "' and PaidYN = 1";
                            arQuery.Add(sqlQuery);

                        }
                    }


                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Delete Succefully.");
                }
                //prcClearData();
                prcLoadSaveData();
                //PrcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Unclaimed..";


                ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptUnclaimedEmp.rdlc";
                SQLQuery = "Exec rptUnclaimed " + Common.Classes.clsMain.intComId + ", '" + cboType.Text.ToString() + "','" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "',4";

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPaid_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Do you want to update this employee all unlclaimed amount.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "",EmpID = "";

            EmpID = gridEmployee.ActiveRow.Cells[0].Value.ToString();

            try
            {
                //Member Master Table
                if (btnPaid.Text.ToString() == "&All Paid")
                {

                    sqlQuery = " Update tblUnclaimed Set PaidYN = '0' Where EmpId = '" + EmpID +
                                       "' and ComID = " + Common.Classes.clsMain.intComId + "  and PaidYN = 1";
                    arQuery.Add(sqlQuery);
                }


                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Paid')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Update Successfully Complete.");

                    prcLoadSaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }




   }
}