using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;
using System.Data.OleDb;
using System.Net;
using System.IO.Ports;
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmFixAttDriver : Form
    {
        private string strValue = "";

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private string Data = "";

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmFixAttDriver(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmFixAttDriver_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();

                this.dtInputDate.Value = DateTime.Now;
                this.dtInputToDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAttDriver] 0," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + dtInputDate.Value.ToString() + "','" +
                                  dtInputDate.Value.ToString() + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblEmpID";
                dsList.Tables[2].TableName = "tblSts";


                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];

                dtTimeIn.Text = "00:00";
                dtTimeOut.Text = "00:00";
                dtOT.Text = "0.00";



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

        private void prcLoadCombo()
        {
            cboEmpID.DataSource = null;
            cboEmpID.DataSource = dsList.Tables["tblEmpID"];
            cboEmpID.DisplayMember = "empcode";
            cboEmpID.ValueMember = "empid";

            cboStatus.DataSource = null;
            cboStatus.DataSource = dsList.Tables["tblSts"];
            cboStatus.DisplayMember = "varname";
            cboStatus.ValueMember = "varid";


            cboStatus1.DataSource = null;
            cboStatus1.DataSource = dsList.Tables["tblSts"];
            cboStatus1.DisplayMember = "varname";
            cboStatus1.ValueMember = "varid";
        }

        private void prcClearData()
        {
            this.gridList.DataSource = null;
            this.cboEmpID.Value = null;
            this.cboEmpID.Text = "";
            this.txtName.Text = "";
            this.cboStatus.Value = null;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboEmpID.Focus();

            this.optCriteria.Value = "All";
            groupData.Enabled = false;
            groupBoxCombo.Enabled = false;
            txtName.Enabled = false;
            this.cboStatus.Enabled = false;
            btnFillData.Enabled = false;

        }

        private Boolean fncBlank()
        {


            if (optCriteria.Value == "All")
            {
                Data = "";
            }
            else if (optCriteria.Value == "EmpID")
            {
                if (this.cboEmpID.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Employee code.");
                    cboEmpID.Focus();
                    return true;
                }
            }

            else if (optCriteria.Value == "Status")
            {
                if (this.cboStatus.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Status");
                    cboStatus.Focus();
                    return true;
                }
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
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;

            try
            {
                //Member Master Table
                if (btnSave.Text.ToString() != "&Save")
                {

                }
                else
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Delete  tblAttfixedCasual where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblAttfixedCasual(empid,dtPunchDate,TimeIn,TimeOut,OTHr,Status,Remarks,TimeInPrev,TimeOutPrev,OTHrPrev,StatusPrev,Luserid,comid,pcname) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                       row.Cells["dtPunchDate"].Text.ToString() + "','" +
                                       row.Cells["timein"].Text.ToString() + "','" +
                                       row.Cells["timeout"].Text.ToString() + "','" +
                                       row.Cells["otHour"].Value.ToString() + "','" +
                                       row.Cells["Status"].Value.ToString() + "','" +
                                       row.Cells["Remarks"].Value.ToString() + "','" +
                                       row.Cells["TimeInPrev"].Text.ToString() + "','" +
                                       row.Cells["TimeOutPrev"].Text.ToString() + "','" +
                                       row.Cells["OTHrPrev"].Value.ToString() + "','" +
                                       row.Cells["StatusPrev"].Value.ToString() + "'," +
                                       Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);

                            string sqlQuery1 = "Exec [prcProcessAttendFixDriver] " + Common.Classes.clsMain.intComId + ",'" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["empid"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery1);
                        }
                    }


                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully.");
                }
                prcClearData();
                cboEmpID.Focus();

                prcLoadList();
                prcLoadCombo();
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

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true; //EmpID
                gridList.DisplayLayout.Bands[0].Columns["TimeInPrev"].Hidden = true; //TimeInPrev
                gridList.DisplayLayout.Bands[0].Columns["TimeOutPrev"].Hidden = true; //TimeInOut
                gridList.DisplayLayout.Bands[0].Columns["OTHrPrev"].Hidden = true; //OTHr
                gridList.DisplayLayout.Bands[0].Columns["StatusPrev"].Hidden = true; //StatusPrev

                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 75; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 150; //Country Name
                gridList.DisplayLayout.Bands[0].Columns["ResUser"].Width = 90; //Res User
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 90; //
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Width = 65; //
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Width = 65; //
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Width = 65; //
                gridList.DisplayLayout.Bands[0].Columns["Status"].Width = 50; //
                gridList.DisplayLayout.Bands[0].Columns["remarks"].Width = 70; //
                // gridList.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;  //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp. Code";
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Header.Caption = "Puchdate";
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Header.Caption = "Time In";
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Header.Caption = "Time Out";
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Header.Caption = "Ot Hour";
                gridList.DisplayLayout.Bands[0].Columns["Status"].Header.Caption = "Status";
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "remarks";

                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //this.gridList.DisplayLayout.Bands[0].Columns["othour"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;

                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Format = "HH:mm";

                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm";

                ////this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm:ss";
                //this.gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Format = "dd-MMM-yyyy";

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["empName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["ResUser"].CellActivation = Activation.NoEdit;
                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////RowHeight
                gridList.DisplayLayout.Override.DefaultRowHeight = 20;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmFixAttDriver_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            uTab = null;
            clsProc = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to Delete FixAttendance Which Are shown in the Grid", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0)
                    {
                        //RowID = row.Index + 1;
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixedCasual As A

                        sqlQuery = " Delete  tblAttfixedCasual where empid = '" + row.Cells["empid"].Text.ToString() +
                                   "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                cboEmpID.Focus();
                prcLoadList();
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    prcClearData();
            //    prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboCountryName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCountryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboEmpID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            cboEmpID.DisplayLayout.Bands[0].Columns["empName"].Width = 135;
            cboEmpID.DisplayLayout.Bands[0].Columns["empcode"].Width = 75;

            cboEmpID.DisplayLayout.Bands[0].Columns["empid"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["dtjoin"].Hidden = true;

            cboEmpID.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp. Code";
            cboEmpID.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";

            cboEmpID.DisplayMember = "empcode";
            cboEmpID.ValueMember = "empid";

        }

        private void cboEmpID_RowSelected(object sender, RowSelectedEventArgs e)
        {
            try
            {
                if (this.cboEmpID.IsItemInList() == false)
                {
                    //MessageBox.Show("Please Provide valid data [or, select from list].");
                    //cboEmpID.Focus();
                    return;
                }

                if (cboEmpID.Value != null)
                {
                    //
                    txtName.Text = cboEmpID.ActiveRow.Cells["empName"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
        }

        private void dtJoinDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboStatus.DisplayLayout.Bands[0].Columns["varname"].Width = cboStatus.Width;
            cboStatus.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Status";
            cboStatus.DisplayLayout.Bands[0].Columns["varid"].Hidden = true;
        }


        private void cboStatus1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboStatus1.DisplayLayout.Bands[0].Columns["varname"].Width = cboStatus1.Width;
            cboStatus1.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Status";

            cboStatus1.DisplayLayout.Bands[0].Columns["varid"].Hidden = true;
        }

        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            if (optCriteria.Value == "All")
            {
                cboEmpID.Enabled = false;
                cboStatus.Enabled = false;
                groupBoxCombo.Enabled = false;
            }
            else if (optCriteria.Value == "EmpID")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = true;
                cboStatus.Enabled = false;
            }

            else if (optCriteria.Value == "Status")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboStatus.Enabled = true;
            }


        }

        private void cboAddList_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }


            if (optCriteria.Value == "All")
            {
                Data = "";
            }
            else if (optCriteria.Value == "EmpID")
            {
                Data = cboEmpID.Value.ToString();
            }

            else if (optCriteria.Value == "Status")
            {
                Data = cboStatus.Text.ToString();
            }


            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAttDriver] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "','" +
                                  clsProc.GTRDate(dtInputToDate.Value.ToString()) + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;

                    cboStatus1.Text = "P";
                    dtTimeIn.Text = "00:00";
                    dtTimeOut.Text = "00:00";
                    dtOT.Text = "0.00";
                }
                else
                {

                    MessageBox.Show("No Data Found.");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }



        }

        private void btnFillData_Click(object sender, EventArgs e)
        {

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
            {
                if (row.Cells["empid"].Text.ToString().Length != 0)
                {
                    //RowID = row.Index + 1;
                    ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixedCasual As A

                    row.Cells["timein"].Value = dtTimeIn.Value.ToString();
                    row.Cells["timeOut"].Value = dtTimeOut.Value.ToString();
                    row.Cells["othour"].Value = dtOT.Value.ToString();
                    row.Cells["status"].Value = cboStatus1.Text.ToString();

                }
            }

        }

        private void cboEmpID_ValueChanged(object sender, EventArgs e)
        {


            if (this.cboEmpID.IsItemInList() == false)
            {
                //MessageBox.Show("Please Provide valid data [or, select from list].");
                //cboEmpID.Focus();
                return;
            }

            if (cboEmpID.Value == null)
                return;

            strValue = cboEmpID.Value.ToString();
        }



        private void cboStatus_ValueChanged(object sender, EventArgs e)
        {
            if (cboStatus.Value == null)
                return;

            strValue = cboStatus.Text.ToString();
        }
      

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    row.Cells["isChecked"].Value = 1;
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    row.Cells["isChecked"].Value = 0;
                }
            }

        }

        private void gridList_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (gridList.ActiveRow.IsFilterRow != true)
            {

                DataSet dsChange = new DataSet();
                clsConnection clscon = new clsConnection();
                string sqlQuery = "";
                try
                {
                    sqlQuery = "Exec prcProcessManualAttDriver " + Common.Classes.clsMain.intComId + ",'" + gridList.ActiveRow.Cells["empid"].Value + "','" + clsProc.GTRDate(gridList.ActiveRow.Cells["dtPunchDate"].Value.ToString()) + "', 1,'" + gridList.ActiveRow.Cells["TimeIn"].Value.ToString() + "','" + gridList.ActiveRow.Cells["TimeOut"].Value.ToString() + "','" + gridList.ActiveRow.Cells["Status"].Value.ToString() + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsChange, sqlQuery);
                    dsChange.Tables[0].TableName = "Change";
                    DataRow dr = dsChange.Tables["Change"].Rows[0];
                    if (dsChange.Tables["Change"].Rows.Count > 0)
                    {
                        gridList.ActiveRow.Cells["OtHour"].Value = dr["OtHour"];
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    clscon = null;
                }
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Fix Attendance (Casual Driver)List.xls" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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
            GridToToExcel.Export(gridList, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string ReportPath = "";
            string SQLQuery = "";

            //Collecting Parameter Value


            //Report Criteria & Procedure
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardCasual.rdlc";
            SQLQuery = "Exec rptJobCardDriver " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "', '" + clsProc.GTRDate(dtInputToDate.Value.ToString()) + "',0,'" + cboEmpID.Value.ToString() + "','=ALL=' ";

            string DataSourceName = "DataSet1";
            string FormCaption = "Report :: Job Card ...";

            GTRLibrary.clsReport.strReportPathMain = ReportPath;
            GTRLibrary.clsReport.strQueryMain = SQLQuery;
            GTRLibrary.clsReport.strDSNMain = DataSourceName;

            FM.prcShowReport(FormCaption);
        }
    }
}