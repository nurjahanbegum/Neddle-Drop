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
    public partial class frmFixAttB : Form
    {
        private string strValue = "";

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private string Data = "";

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmFixAttB(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }


        private void prcLoadCombo()
        {
            cboEmpID.DataSource = null;
            cboEmpID.DataSource = dsList.Tables["tblEmpID"];
            cboEmpID.DisplayMember = "empcode";
            cboEmpID.ValueMember = "empid";

            cboSection.DataSource = null;
            cboSection.DataSource = dsList.Tables["tblSect"];
            cboSection.DisplayMember = "sectname";
            cboSection.ValueMember = "sectid";

            cboShiftTime.DataSource = null;
            cboShiftTime.DataSource = dsList.Tables["tblShift"];
            cboShiftTime.DisplayMember = "shiftname";
            cboShiftTime.ValueMember = "shiftid";


            cboStatus.DataSource = null;
            cboStatus.DataSource = dsList.Tables["tblSts"];
            cboStatus.DisplayMember = "varname";
            cboStatus.ValueMember = "varid";


            cboStatus1.DataSource = null;
            cboStatus1.DataSource = dsList.Tables["tblSts"];
            cboStatus1.DisplayMember = "varname";
            cboStatus1.ValueMember = "varid";

            uddShift.DataSource = null;
            uddShift.DataSource = dsList.Tables["tblShiftID"];
            gridList.DisplayLayout.Bands[0].Columns["ShiftId"].ValueList = uddShift;


        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAttBuyer] 0," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + dtInputDate.Value.ToString() + "','" +
                                  dtInputDate.Value.ToString() + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblEmpID";
                dsList.Tables[2].TableName = "tblSect";
                dsList.Tables[3].TableName = "tblShift";
                dsList.Tables[4].TableName = "tblSts";
                dsList.Tables[5].TableName = "tblShiftID";

                //dsList.Tables[2].TableName = "Country";

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

        private void prcClearData()
        {
            this.gridList.DataSource = null;
            this.cboEmpID.Value = null;
            this.cboEmpID.Text = "";
            this.txtName.Text = "";
            this.dtInputDate.Value = DateTime.Now;
            this.dtInputToDate.Value = DateTime.Now;

            this.cboSection.Value = null;
            this.cboShiftTime.Value = null;
            this.cboStatus.Value = null;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboEmpID.Focus();

            this.optCriteria.Value = "All";
            groupData.Enabled = false;
            groupBoxCombo.Enabled = false;
            txtName.Enabled = false;
            this.cboSection.Enabled = false;
            this.cboShiftTime.Enabled = false;
            this.cboStatus.Enabled = false;
            btnFillData.Enabled = false;

        }

        private void prcClearDataSave()
        {
            this.gridList.DataSource = null;
            this.cboEmpID.Value = null;
            this.cboEmpID.Text = "";
            this.txtName.Text = "";

            this.cboSection.Value = null;
            this.cboShiftTime.Value = null;
            this.cboStatus.Value = null;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboEmpID.Focus();

            this.optCriteria.Value = "All";
            groupData.Enabled = false;
            groupBoxCombo.Enabled = false;
            txtName.Enabled = false;
            this.cboSection.Enabled = false;
            this.cboShiftTime.Enabled = false;
            this.cboStatus.Enabled = false;
            btnFillData.Enabled = false;

        }

        private void frmFixAttB_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAttBuyer] " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblReleased";

                DataRow dr;
                if (dsDetails.Tables["tblReleased"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblReleased"].Rows[0];

                    this.txtId.Text = dr["relid"].ToString();
                    this.cboEmpID.Value = dr["empid"].ToString();
                    this.dtInputDate.Text = dr["reldate"].ToString();

                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
                }
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
            else if (optCriteria.Value == "Sec")
            {
                Data = cboSection.Value.ToString();
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                Data = cboShiftTime.Value.ToString();
            }
            else if (optCriteria.Value == "Status")
            {
                Data = cboStatus.Text.ToString();
            }
            else if (optCriteria.Value == "Missing")
            {
                Data = "Missing";
            }


            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAttBuyer] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "','" +
                                  clsProc.GTRDate(dtInputToDate.Value.ToString()) + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];
                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].ValueList = uddShift;
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

                // this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                // this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
            finally
            {
                clsCon = null;
            }


        }


        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide column

                //	TimeIn,,,,,							

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true; //EmpId
                gridList.DisplayLayout.Bands[0].Columns["TimeInPrev"].Hidden = true; //TimeInPrev
                gridList.DisplayLayout.Bands[0].Columns["TimeOutPrev"].Hidden = true; //TimeInOut
                gridList.DisplayLayout.Bands[0].Columns["OTHrPrev"].Hidden = true; //OTHr
                gridList.DisplayLayout.Bands[0].Columns["StatusPrev"].Hidden = true; //StatusPrev
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Hidden = true; //OTHour
                gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Hidden = true; //

                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 75; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 110; //Country Name
                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Width = 80; //Shift
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 95; //
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Width = 65; //
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Width = 70; //
                gridList.DisplayLayout.Bands[0].Columns["Status"].Width = 50; //

                // gridList.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;  //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Header.Caption = "Check";
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp Code";
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Header.Caption = "Shift";
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Header.Caption = "Punchdate";
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Header.Caption = "Time In";
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Header.Caption = "Time Out";
                gridList.DisplayLayout.Bands[0].Columns["Status"].Header.Caption = "Status";
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";
                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;


                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;

                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Format = "HH:mm";
                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm";

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //gridList.DisplayLayout.Override.AllowMultiCellOperations = AllowMultiCellOperation.All;

                gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////Stop Updating-Asad
                //this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

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


        private void fncGridData(ref ArrayList arQuery, String newID, String Saleid)
        {



            //Common.Classes.clsConnection clsCon = new Common.Classes.clsConnection("CustBill");
            try
            {




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
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
                    //Update data
                    //sqlQuery = " Update tblEmp_Released Set relDate = '" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "' Where RelID = " + Int32.Parse(txtId.Text.ToString());
                    //arQuery.Add(sqlQuery);

                    //// Insert Information To Log File
                    //sqlQuery= "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                    //    + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    //arQuery.Add(sqlQuery);

                    ////Transaction with database
                    //clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    //MessageBox.Show("Data Updated Succefully.");
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


                            sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOutB,OTB,Status,Remarks,ShiftID,TimeInPrev,TimeOutPrev,OTHrPrev,StatusPrev,Luserid,comid,pcname) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                       row.Cells["dtPunchDate"].Text.ToString() + "','" +
                                       row.Cells["timein"].Text.ToString() + "','" +
                                       row.Cells["timeout"].Text.ToString() + "','0','" +
                                       row.Cells["Status"].Value.ToString() + "','" +
                                       row.Cells["Remarks"].Value.ToString() + "','" +
                                       row.Cells["ShiftID"].Value.ToString() + "','" +
                                       row.Cells["TimeInPrev"].Text.ToString() + "','" +
                                       row.Cells["TimeOutPrev"].Text.ToString() + "','" +
                                       row.Cells["OTHrPrev"].Value.ToString() + "','" +
                                       row.Cells["StatusPrev"].Value.ToString() + "'," +
                                       Common.Classes.clsMain.intUserId + "," + 
                                       Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);

                            //if (row.Cells["OTHour"].Value.ToString() != "0.00")
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

                            string sqlQuery2 = "Exec [prcProcessAttendFixB] " + Common.Classes.clsMain.intComId + ",'" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["empid"].Text.ToString() + "'";
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
                prcClearDataSave();
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


        private void frmFixAttB_FormClosing(object sender, FormClosingEventArgs e)
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
            else if (optCriteria.Value == "Sec")
            {
                if (this.cboSection.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Section");
                    cboSection.Focus();
                    return true;
                }
            }

            else if (optCriteria.Value == "ShiftTime")
            {
                if (this.cboShiftTime.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Shift");
                    cboShiftTime.Focus();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to delete FixAttendance which are shown in the Grid", "",
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
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                        sqlQuery = " Delete  tblAttfixed where empid = '" + row.Cells["empid"].Text.ToString() +
                                   "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
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
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboCountryName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCountryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
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
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboStatus.DisplayLayout.Bands[0].Columns["varname"].Width = cboStatus.Width;
            cboStatus.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Status";
            cboStatus.DisplayLayout.Bands[0].Columns["varid"].Hidden = true;
        }

        private void cboShiftTime_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboShiftTime.DisplayLayout.Bands[0].Columns["shiftname"].Width = cboShiftTime.Width;
            cboShiftTime.DisplayLayout.Bands[0].Columns["shiftname"].Header.Caption = "Shift";
            cboShiftTime.DisplayLayout.Bands[0].Columns["shiftid"].Hidden = true;
        }

        private void cboSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSection.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSection.Width;

            cboSection.DisplayLayout.Bands[0].Columns["sectname"].Header.Caption = "Section";
            cboSection.DisplayLayout.Bands[0].Columns["sectId"].Hidden = true;
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
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboStatus.Enabled = false;
                groupBoxCombo.Enabled = false;
            }
            else if (optCriteria.Value == "EmpID")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = true;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboStatus.Enabled = false;
            }
            else if (optCriteria.Value == "Sec")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = true;
                cboShiftTime.Enabled = false;
                cboStatus.Enabled = false;
            }

            else if (optCriteria.Value == "SubSec")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboStatus.Enabled = false;
            }

            else if (optCriteria.Value == "Band")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboStatus.Enabled = false;
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = true;
                cboStatus.Enabled = false;
            }
            else if (optCriteria.Value == "Status")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboStatus.Enabled = true;
            }




        }


        private void btnFillData_Click(object sender, EventArgs e)
        {
            if (chkIn.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0)
                    {

                        row.Cells["timein"].Value = dtTimeIn.Text.ToString();
                        row.Cells["status"].Value = cboStatus1.Text.ToString();

                    }
                }
            }
            else if (chkOut.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0)
                    {

                        row.Cells["timeOut"].Value = dtTimeOut.Text.ToString();

                    }
                }
            }
            else if (chkIn.Checked == false && chkOut.Checked == false)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0)
                    {
                        //RowID = row.Index + 1;
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                        row.Cells["timein"].Value = dtTimeIn.Text.ToString();
                        row.Cells["timeOut"].Value = dtTimeOut.Text.ToString();
                        row.Cells["othour"].Value = dtOT.Value.ToString();
                        row.Cells["status"].Value = cboStatus1.Text.ToString();

                    }
                }
            }

            chkIn.Checked = false;
            chkOut.Checked = false;

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

        private void cboSection_ValueChanged(object sender, EventArgs e)
        {
            if (cboSection.Value == null)
                return;

            strValue = cboSection.Value.ToString();
        }

        private void cboShiftTime_ValueChanged(object sender, EventArgs e)
        {
            if (cboShiftTime.Value == null)
                return;

            strValue = cboShiftTime.Value.ToString();
        }

        private void cboStatus_ValueChanged(object sender, EventArgs e)
        {
            if (cboStatus.Value == null)
                return;

            strValue = cboStatus.Text.ToString();
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {

            if (optCriteria.Value == "All")
            {
                Data = "";
            }
            else if (optCriteria.Value == "EmpID")
            {
                Data = cboEmpID.Value.ToString();
            }
            else if (optCriteria.Value == "Sec")
            {
                Data = cboSection.Value.ToString();
            }
            else if (optCriteria.Value == "Missing")
            {
                Data = "Missing";
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                Data = cboShiftTime.Value.ToString();
            }
            else if (optCriteria.Value == "Status")
            {
                Data = cboStatus.Text.ToString();
            }


            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            //dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAttBuyer] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "','" +
                                  clsProc.GTRDate(dtInputDate.Value.ToString()) + "' ";
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
            finally
            {
                clsCon = null;
            }
        }

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16)e.KeyChar);
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

        //private void gridList_AfterCellUpdate(object sender, CellEventArgs e)
        //{
        //    if (gridList.ActiveRow.IsFilterRow != true)
        //    {

        //            DataSet dsChange = new DataSet();
        //            clsConnection clscon = new clsConnection();
        //            string sqlQuery = "";
        //            try
        //            {
        //                sqlQuery = "Exec prcProcessManualAtt " + Common.Classes.clsMain.intComId + ",'" +
        //                           gridList.ActiveRow.Cells["empid"].Value + "','" +
        //                           clsProc.GTRDate(gridList.ActiveRow.Cells["dtPunchDate"].Value.ToString()) + "', " +
        //                           gridList.ActiveRow.Cells["ShiftId"].Value + ",'" +
        //                           gridList.ActiveRow.Cells["TimeIn"].Value.ToString() + "','" +
        //                           gridList.ActiveRow.Cells["TimeOut"].Value.ToString() + "' ";
        //                clscon.GTRFillDatasetWithSQLCommand(ref dsChange, sqlQuery);
        //                dsChange.Tables[0].TableName = "Change";
        //                DataRow dr = dsChange.Tables["Change"].Rows[0];
        //                if (dsChange.Tables["Change"].Rows.Count > 0)
        //                {
        //                    gridList.ActiveRow.Cells["OTHour"].Value = dr["OtHour"];
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //            finally
        //            {
        //                clscon = null;
        //            }
        //    }
        //}















    }
}