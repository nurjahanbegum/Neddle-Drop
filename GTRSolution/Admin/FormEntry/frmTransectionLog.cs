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
using Infragistics.Win.UltraWinGrid.ExcelExport;
using GTRLibrary;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmTransectionLog : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmTransectionLog(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmTransectionLog_Load(object sender, System.EventArgs e)
        {
            prcLoadList();
            prcLoadCombo("");
        }

        private void frmTransectionLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec rptTranLog " + Common.Classes.clsMain.intUserId + ", 0,0,'','','',''";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "UserList";
                dsList.Tables[1].TableName = "tblEmp";
                dsList.Tables[2].TableName = "tblType";

                prcModifyDataset();

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["UserList"];

                gridEmployeeID.DataSource = null;
                gridEmployeeID.DataSource = dsList.Tables["tblEmp"];

                gridType.DataSource = null;
                gridType.DataSource = dsList.Tables["tblType"];

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

        private void prcLoadCombo(string sqlQuery)
        {

        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;   //User Id
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 260;      //User Name
                //gridList.DisplayLayout.Bands[0].Columns[2].Hidden = true;   //Group Id
                //gridList.DisplayLayout.Bands[0].Columns[3].Width = 185;     //Group Name

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "User Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "User Name";
                //gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Group Id";
                //gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group Name";

                //Change alternate color
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

                //Use Filtering
                this.gridList.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;

                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void prcClearData()
        {
            txtUserId.Text = "0";
            prcLoadCombo("");
        }

        private Boolean fncBlank()
        {
            if (txtUserId.ToString().Length == 0)
            {
                MessageBox.Show("Please select user name.");
                gridList.Focus();
                return true;
            }
            if (this.gridEmployeeID.Rows.Count == 0)
            {
                MessageBox.Show("Data not found.");
                this.gridEmployeeID.Focus();
                return true;
            }

            //To Insert Data With New Value
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridEmployeeID.Rows)
            {
                if (Int16.Parse(row.Cells["isGroup"].Text.ToString()) == 1)
                {
                    if(Int32.Parse(row.Cells["IsDefault"].Text.ToString())==1)
                    {
                        MessageBox.Show("You cannot select group as your default company.");
                        this.gridList.Focus();
                        return true;
                    }
                }
            }
            return false;
        }
        public void prcModifyDataset()
        {
            for (int i = 0; i <= dsList.Tables[0].Rows.Count - 1; i++)
            {
                dsList.Tables[0].Rows[i]["LUserName"] = clsProc.GTRDecryptWord(dsList.Tables[0].Rows[i]["LUserName"].ToString());
            }
        }

        private void gridEmployeeID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;

            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 215;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Stop Cell Modify
            //gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
            //gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;

            //Change alternate color
            this.gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridType.DisplayLayout.Bands[0].Columns["Sl"].Hidden = true;

            gridType.DisplayLayout.Bands[0].Columns["Type"].Width = 215;
            gridType.DisplayLayout.Bands[0].Columns["Type"].Header.Caption = "Type";

            //Change alternate color
            this.gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            gridEmployeeID.Enabled = false;
            gridList.Enabled = false;
            gridType.Enabled = true;

            if (optCriteria.Value.ToString() == "Employee")
            {
                gridList.Enabled = false;
                gridEmployeeID.Enabled = true;
            }
            else if (optCriteria.Value.ToString() == "User")
            {
                gridList.Enabled = true;
                gridEmployeeID.Enabled = false;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptTranLog.rdlc";
            prcShowReport();
        }

        private void prcShowReport()
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            
            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Transection Log...";

                string EmpId = "0", UserId = "0",Type = "=ALL=";

                EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                UserId = gridList.ActiveRow.Cells["LUserId"].Value.ToString();
                Type = gridType.ActiveRow.Cells["Type"].Value.ToString();

                if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    rptQuery = "Exec rptTranLog '" + UserId + "',1, '" + EmpId + "','" + Type + "','Employee','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                }
                else if (optCriteria.Value.ToString().ToUpper() == "User".ToUpper())
                {
                    rptQuery = "Exec rptTranLog '" + UserId + "',1, '" + EmpId + "','" + Type + "','User','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                }

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = rptQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
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
                clsCon = null;
            }
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

        private void btnExcelType_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string EmpId = "0", UserId = "0", Type = "=ALL=";

            EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
            UserId = gridList.ActiveRow.Cells["LUserId"].Value.ToString();
            Type = gridType.ActiveRow.Cells["Type"].Value.ToString();

            if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
            {
                string SQLQuery = "Exec rptTranLog '" + UserId + "',1, '" + EmpId + "','" + Type + "','Employee','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                dsDetails.Tables[0].TableName = "Rpt";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsDetails.Tables["Rpt"];
            }
            else if (optCriteria.Value.ToString().ToUpper() == "User".ToUpper())
            {
                string SQLQuery = "Exec rptTranLog '" + UserId + "',1, '" + EmpId + "','" + Type + "','User','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                dsDetails.Tables[0].TableName = "Rpt";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsDetails.Tables["Rpt"];
            }

            if (dsDetails.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Data Not Found");
                return;
            }


            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Transection Log_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

    }
}