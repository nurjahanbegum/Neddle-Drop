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
    public partial class frmFixAttAuto : Form
    {
        private string strValue = "";

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private string Data = "";

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmFixAttAuto(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }


        private void prcLoadCombo()
        {
            cboEmployee.DataSource = null;
            cboEmployee.DataSource = dsList.Tables["tblEmpID"];
            cboEmployee.DisplayMember = "empcode";
            cboEmployee.ValueMember = "empid";


            uddShift.DataSource = null;
            uddShift.DataSource = dsList.Tables["tblShiftID"];
            gridList.DisplayLayout.Bands[0].Columns["ShiftId"].ValueList = uddShift;

        }

        private void prcLoadList()
        {

            //var DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DaysInMonth);
            //var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //dtFrom.Value = firstDay;
            //dtTo.Value = lastDay;
            
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetManualAtt] " + Common.Classes.clsMain.intComId + ",'" + cboEmployee.Text.ToString() +
                    "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "', 0 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblEmpID";
                dsList.Tables[2].TableName = "tblShiftID";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];


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

        private void prcClearData()
        {
            this.gridList.DataSource = null;
            this.btnSave.Text = "&Save";
        }

        private void frmFixAttAuto_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();

                var DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DaysInMonth);
                var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtFrom.Value = firstDay;
                dtTo.Value = lastDay;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            if (cboEmployee.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Employee ID");
                cboEmployee.Focus();
                return;
            }
            try
            {
                string sqlQuery = "Exec [prcGetManualAtt] " + Common.Classes.clsMain.intComId + ",'" + cboEmployee.Text.ToString() + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "', 1 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                
                dsDetails.Tables[0].TableName = "tblFixData";

                dsDetails.Tables[1].TableName = "tblAttedDate";
                
                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    DataRow dr = dsDetails.Tables["tblFixData"].Rows[0];


                    lvlName.Text = dr["EmpName"].ToString();
                    lvlDesig.Text = dr["DesigName"].ToString();
                    lvlSect.Text = dr["SectName"].ToString();
                    lvlJoin.Text = dr["dtJoin"].ToString();
                    
                    lvlP.Text = dr["Present"].ToString();
                    lvlA.Text = dr["Absent"].ToString();
                    lvlL.Text = dr["lateDay"].ToString();
                    lvlLH.Text = dr["latehrttl"].ToString();
                    lvlLV.Text = dr["Leave"].ToString();
                    lvlH.Text = dr["HDay"].ToString();
                    lvlWh.Text = dr["WDay"].ToString();
                    lvlOT.Text = dr["OTHr"].ToString();

                }
                else
                {
                    lvlName.Text = "0";
                    lvlDesig.Text = "0";
                    lvlSect.Text = "0";
                    lvlJoin.Text = "0";
                    
                    lvlP.Text = "0";
                    lvlA.Text = "0";
                    lvlL.Text = "0";
                    lvlLH.Text = "0";
                    lvlLV.Text = "0";
                    lvlH.Text = "0";
                    lvlWh.Text = "0";
                    lvlOT.Text = "0";

                }

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblAttedDate"];
                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].ValueList = uddShift;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
                dsDetails = null;
            }
        }


        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide column

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true; //EmpID
                gridList.DisplayLayout.Bands[0].Columns["TimeInPrev"].Hidden = true; //TimeInPrev
                gridList.DisplayLayout.Bands[0].Columns["TimeOutPrev"].Hidden = true; //TimeInOut
                gridList.DisplayLayout.Bands[0].Columns["OTHrPrev"].Hidden = true; //OTHr
                gridList.DisplayLayout.Bands[0].Columns["StatusPrev"].Hidden = true; //StatusPrev

                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 65; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Width = 110; //Shift
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 110; //
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Width = 70; //
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Width = 70; //
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Width = 65; //
                gridList.DisplayLayout.Bands[0].Columns["Status"].Width = 55; //
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Width = 105; //
                //gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Hidden = true; //
                // gridList.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;  //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Header.Caption = "Shift";
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Header.Caption = "Punchdate";
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Header.Caption = "Time In";
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Header.Caption = "Time Out";
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Header.Caption = "Ot Hour";
                gridList.DisplayLayout.Bands[0].Columns["Status"].Header.Caption = "Status";
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";
                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //this.gridList.DisplayLayout.Bands[0].Columns["Othour"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;

                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                
                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Format = "HH:mm";
                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm";

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;


                ////RowHeight
                gridList.DisplayLayout.Override.DefaultRowHeight = 22;

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
    


        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

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
                    //Update data
                    //sqlQuery = " Update tblEmp_Released Set relDate = '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' Where RelID = " + Int32.Parse(txtId.Text.ToString());
                    //arQuery.Add(sqlQuery);

                }
                else
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {
                            //RowID = row.Index + 1;
                            ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                            sqlQuery = " Delete  tblAttfixed where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOut,OT,Status,Remarks,ShiftID,TimeInPrev,TimeOutPrev,OTHrPrev,StatusPrev,Luserid,comid,pcname,IsInactive) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                       row.Cells["dtPunchDate"].Text.ToString() + "','" +
                                       row.Cells["timein"].Text.ToString() + "','" +
                                       row.Cells["timeout"].Text.ToString() + "','" +
                                       row.Cells["otHour"].Value.ToString() + "','" +
                                       row.Cells["Status"].Value.ToString() + "','" +
                                       row.Cells["Remarks"].Value.ToString() + "','" +
                                       row.Cells["ShiftID"].Value.ToString() + "','" +
                                       row.Cells["TimeInPrev"].Text.ToString() + "','" +
                                       row.Cells["TimeOutPrev"].Text.ToString() + "','" +
                                       row.Cells["OTHrPrev"].Value.ToString() + "','" +
                                       row.Cells["StatusPrev"].Value.ToString() + "'," +
                                       Common.Classes.clsMain.intUserId + "," + 
                                       Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "','1')";
                            arQuery.Add(sqlQuery);

                            //if (row.Cells["otHour"].Value.ToString() != "0.00")
                            //{
                            //    string sqlQuery1 = " Update P Set P.FirstAppId = I.FirstAppId,P.FinalAppId = I.FinalAppId,P.AppFirst = I.AppFirst,P.AppFinal = I.AppFinal"
                            //                            + " from tblAttfixed as P "
                            //                            + " inner join tblInput_Permission as I on I.ComId = P.ComID "
                            //                            + " inner join tblEmp_Info as E on E.ComId = P.ComID and E.OfficeGrade = I.EmpType "
                            //                            + " Where P.EmpId = '" + row.Cells["empid"].Text.ToString()
                            //                            + "' and P.ComId = " + Common.Classes.clsMain.intComId + " and P.dtPunchDate = '"
                            //                            + row.Cells["dtPunchDate"].Text.ToString() + "' and I.PType = 'Fix-Attendance'";
                            //    arQuery.Add(sqlQuery1);
                            //} 

                            string sqlQuery2 = "Exec [prcProcessAttendFixAuto] " + Common.Classes.clsMain.intComId + ",'" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["empid"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery2);
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
                prcClearData();
                cboEmployee.Focus();

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

        private void frmFixAttAuto_FormClosing(object sender, FormClosingEventArgs e)
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

   
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void cboEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboEmployee_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cboEmployee_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmployee.DisplayLayout.Bands[0].Columns["empName"].Width = 135;
            cboEmployee.DisplayLayout.Bands[0].Columns["empcode"].Width = 75;

            cboEmployee.DisplayLayout.Bands[0].Columns["empid"].Hidden = true;
            //cboEmployee.DisplayLayout.Bands[0].Columns["SectName"].Hidden = true;

            cboEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp. Code";
            cboEmployee.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";

            cboEmployee.DisplayMember = "empcode";
            cboEmployee.ValueMember = "empid";
        }

        private void uddShift_RowSelected(object sender, RowSelectedEventArgs e)
        {

            if (uddShift.ActiveRow == null)
            {
                return;
            }

            gridList.ActiveRow.Cells["ShiftId"].Value = uddShift.ActiveRow.Cells["ShiftId"].Value.ToString();

        }

        private void uddShift_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            uddShift.DisplayLayout.Bands[0].Columns["ShiftId"].Hidden = true;
            uddShift.DisplayLayout.Bands[0].Columns["ShiftDesc"].Header.Caption = "Shift Name";
            uddShift.DisplayMember = "ShiftDesc";
            uddShift.ValueMember = "ShiftId";

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
            dlgSurveyExcel.FileName = "Fix Attendance List.xls" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dtFrom.DateTime.Month == 12)
            {
                var firstDay = new DateTime(dtFrom.DateTime.Year + 1, dtFrom.DateTime.Month - 11, 1);
                dtFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);


                dtTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtTo.DateTime.Year, dtTo.DateTime.Month + 1);
                var lastDay = new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month + 1, DaysInMonth);
                var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month + 1, 1);
                dtFrom.Value = firstDay;
                dtTo.Value = lastDay;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            
            if (dtFrom.DateTime.Month == 1)
            {
                var firstDay = new DateTime(dtFrom.DateTime.Year - 1, dtFrom.DateTime.Month + 11, 1);
                dtFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);


                dtTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtTo.DateTime.Year, dtTo.DateTime.Month - 1);
                var lastDay = new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month - 1, DaysInMonth);
                var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month - 1, 1);
                dtFrom.Value = firstDay;
                dtTo.Value = lastDay;
            }
        }

        private void gridList_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (gridList.ActiveRow.IsFilterRow != true )
            {

                    DataSet dsChange = new DataSet();
                    clsConnection clscon = new clsConnection();
                    string sqlQuery = "";
                    try
                    {
                        sqlQuery = "Exec prcProcessManualAtt " + Common.Classes.clsMain.intComId + ",'" + gridList.ActiveRow.Cells["empid"].Value + "','" + clsProc.GTRDate(gridList.ActiveRow.Cells["dtPunchDate"].Value.ToString()) + "', " + gridList.ActiveRow.Cells["ShiftId"].Value + ",'" + gridList.ActiveRow.Cells["TimeIn"].Value.ToString() + "','" + gridList.ActiveRow.Cells["TimeOut"].Value.ToString() + "' ";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsChange, sqlQuery);
                        dsChange.Tables[0].TableName = "Change";
                        DataRow dr = dsChange.Tables["Change"].Rows[0];
                        if (dsChange.Tables["Change"].Rows.Count > 0)
                        {
                            gridList.ActiveRow.Cells["OTHour"].Value = dr["OtHour"];
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
          }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string ReportPath = "";
            string SQLQuery = "";

            //Collecting Parameter Value


            //Report Criteria & Procedure
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCard.rdlc";
            SQLQuery = "Exec rptJobCard " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "',0, '','', '" + cboEmployee.Value.ToString() + "' ";

            string DataSourceName = "DataSet1";
            string FormCaption = "Report :: Job Card ...";

            GTRLibrary.clsReport.strReportPathMain = ReportPath;
            GTRLibrary.clsReport.strQueryMain = SQLQuery;
            GTRLibrary.clsReport.strDSNMain = DataSourceName;

            FM.prcShowReport(FormCaption);
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

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Int32)e.KeyCode == 40)
            {

                int a;

                int b = gridList.Rows.Count;

                   if (b != gridList.ActiveRow.Index + 1)
                   {

                        if (gridList.ActiveCell.Column.ToString().ToUpper() == "TimeIn".ToUpper())
                        {
                            gridList.Rows[gridList.ActiveRow.Index + 1].Cells["TimeIn"].Activate();
                            gridList.PerformAction(UltraGridAction.EnterEditMode);

                        }
                        else if (gridList.ActiveCell.Column.ToString().ToUpper() == "TimeOut".ToUpper())
                        {
                            gridList.Rows[gridList.ActiveRow.Index + 1].Cells["TimeOut"].Activate();
                            gridList.PerformAction(UltraGridAction.EnterEditMode);

                        }
                        else if (gridList.ActiveCell.Column.ToString().ToUpper() == "Status".ToUpper())
                        {
                            gridList.Rows[gridList.ActiveRow.Index + 1].Cells["Status"].Activate();
                            gridList.PerformAction(UltraGridAction.EnterEditMode);

                        }
                    }

            }

           else if ((Int32)e.KeyCode == 38)
            {

                int a;

                int b = gridList.Rows.Count;

                   if ( gridList.ActiveRow.Index - 1 != 0)
                   {

                        if (gridList.ActiveCell.Column.ToString().ToUpper() == "TimeIn".ToUpper())
                        {
                            gridList.Rows[gridList.ActiveRow.Index - 1].Cells["TimeIn"].Activate();
                            gridList.PerformAction(UltraGridAction.EnterEditMode);

                        }
                        else if (gridList.ActiveCell.Column.ToString().ToUpper() == "TimeOut".ToUpper())
                        {
                            gridList.Rows[gridList.ActiveRow.Index - 1].Cells["TimeOut"].Activate();
                            gridList.PerformAction(UltraGridAction.EnterEditMode);

                        }
                        else if (gridList.ActiveCell.Column.ToString().ToUpper() == "Status".ToUpper())
                        {
                            gridList.Rows[gridList.ActiveRow.Index - 1].Cells["Status"].Activate();
                            gridList.PerformAction(UltraGridAction.EnterEditMode);

                        }
                    }


               }


                //if (gridList.ActiveCell.Column.ToString().ToUpper() == "status".ToUpper())
                //{
                //    //if (gridList.ActiveRow.Cells["UnitPrice"].Text.Length > 1 && gridList.ActiveRow.Cells["Qty"].Text.Length > 1)
                //    //{
                //    gridList.Rows[gridList.ActiveRow.Index + 1].Cells["timein"].Activate();
                //    // gridList.ActiveRow.Cells["Total"].Value = ((double.Parse(gridList.ActiveRow.Cells["UnitPrice"].Value.ToString())) * (double.Parse(gridList.ActiveRow.Cells["Qty"].Value.ToString()))).ToString();


                //    //}
                //}
                //else
                //{
                //    clsProc.GTRTabMove((Int16)e.KeyCode);
                //}



            }




    }
}