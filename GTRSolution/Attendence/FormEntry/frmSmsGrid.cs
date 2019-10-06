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
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Infragistics.Win.UltraWinGrid.ExcelExport;
//using Infragistics.Excel;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmSmsGrid : Form
    {
        private string strValue = "";

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private string Data = "";

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmSmsGrid(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
            loadPorts();
        }

        private void loadPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cboPorts.Items.Add(port);
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


                            sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOut,OtHour,Status,Remarks,Luserid,comid,pcname) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                       row.Cells["dtPunchDate"].Text.ToString() + "','" +
                                       row.Cells["timein"].Text.ToString() + "','" +
                                       row.Cells["timeout"].Text.ToString() + "','" +
                                       row.Cells["otHour"].Value.ToString() + "','" +
                                       row.Cells["Status"].Value.ToString() + "','Fix Attendance - ' + '" +
                                       row.Cells["Remarks"].Value.ToString() + "'," + Common.Classes.clsMain.intUserId +
                                       "," + Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);
                        }
                    }

                    sqlQuery = " exec  [prcProcessAttendFix] " + Common.Classes.clsMain.intComId + ", '" +
                               clsProc.GTRDate(dtInputDate.Value.ToString()) + "' ";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','Insert')";
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
                //Hide column

                //	TimeIn,,,,,							

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true; //Country Name

                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 75; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 150; //Country Name
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 80; //
                gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Width = 60; //
                gridList.DisplayLayout.Bands[0].Columns["TimeOut"].Width = 60; //
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Width = 60; //
                gridList.DisplayLayout.Bands[0].Columns["Status"].Width = 50; //
                gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Hidden = true; //
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

                this.gridList.DisplayLayout.Bands[0].Columns["othour"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                this.gridList.DisplayLayout.Bands[0].Columns["TimeIn"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;

                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm:ss";

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["empName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["sectName"].CellActivation = Activation.NoEdit;
                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

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

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetSmsGrid] 0," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + dtInputDate.Value.ToString() + "','" +
                                  dtInputDate.Value.ToString() + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblEmpID";
                dsList.Tables[2].TableName = "tblSect";
                dsList.Tables[3].TableName = "tblShift";
                dsList.Tables[4].TableName = "tblSts";


                //dsList.Tables[2].TableName = "Country";

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
        }

        private void frmSmsGrid_Load(object sender, EventArgs e)
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

        private void frmSmsGrid_FormClosing(object sender, FormClosingEventArgs e)
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

        private void prcClearData()
        {
            this.gridList.DataSource = null;
            this.cboEmpID.Value = null;
            this.cboEmpID.Text = "";
            this.txtName.Text = "";
            this.dtInputDate.Value = DateTime.Now;

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



            //if (this.cboEmpID.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide Employee ID.");
            //    cboEmpID.Focus();
            //    return true;
            //}
            return false;
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
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                        sqlQuery = " Delete  tblAttfixed where empid = '" + row.Cells["empid"].Text.ToString() +
                                   "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','Delete')";
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

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAtt] " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId;
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


            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetSmsGrid] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
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
                    dtTimeIn.Text = "1-1-1900 09:00";
                    dtTimeOut.Text = "1-1-1900 18:00";
                    dtOt.Text = "1-1-1900 00:00";
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



        }

        private void btnFillData_Click(object sender, EventArgs e)
        {

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
            {
                if (row.Cells["empid"].Text.ToString().Length != 0)
                {
                    //RowID = row.Index + 1;
                    ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                    row.Cells["timein"].Value = dtTimeIn.Value.ToString();
                    row.Cells["timeOut"].Value = dtTimeOut.Value.ToString();
                    row.Cells["othour"].Value = dtOt.Value.ToString();
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
                string sqlQuery = "Exec [prcGetFixAtt] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
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
                    dtTimeIn.Text = "1-1-1900 08:00";
                    dtTimeOut.Text = "1-1-1900 08:00";
                    dtOt.Text = "1-1-1900 08:00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
        }

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            String input = string.Empty;
            String input2 = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select an excel file ";
            dialog.Filter = "Excel files [97-2003] (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            //dialog.InitialDirectory = @"C:\";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                input = dialog.FileName.ToString();
                input2 = dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1);
            }
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            txtFileName.Text = input;
            txtFileName.Tag = input2;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text.Length == 0)
            {
                MessageBox.Show("Please select an excel file, using browse button");
                btnBrowse.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + txtFileName.Text.ToString() + "; Extended Properties='Excel 8.0; HDR=Yes; IMEX=1'";
            try
            {
                var da = new OleDbDataAdapter("SELECT * FROM [AttFix$]", SourceConstr);
                var ds = new DataSet();
                da.Fill(ds);

                prcSaveData(ds);






                MessageBox.Show("Data uploaded successfully. [Total Rows : " + ds.Tables[0].Rows.Count.ToString() + "]");
                //btnProcess.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void prcSaveData(DataSet ds)
        {
            clsConnection clsCon = new clsConnection();
            ArrayList arQuery = new ArrayList();
            string sqlQuery = "";

            try
            {
                // Clear Existing Data
                string query = "Truncate Table tblDN_xls";
                clsCon.GTRSaveDataWithSQLCommand(query);

                txtProcessNo.Text =DateTime.Now.ToString("yyyyMMddHHmmss");
                string dtProcess = DateTime.Today.ToString("dd-MMM-yyyy");

                //Generating Insert Statement Row By Row
                for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString().Replace("'", "").Length > 0)
                    {
                            sqlQuery = "Insert Into tblDN_xls (ComId, xlsFileName, dtProcess, EntryNo, " +
                            " empid,dtPunchDate,TimeIn,TimeOut,OTHour,Status,Remarks,PCName,LUserId) " +
                            " Values(" + Common.Classes.clsMain.intComId + "," +
                            " '" + txtFileName.Tag.ToString() + "', " +
                            " '" + dtProcess + "'," +
                            " '" + txtProcessNo.Text.ToString() + "'," +

                            " '" + ds.Tables[0].Rows[i][0].ToString().Replace("'", "") + "', " +
                            " '" + clsProc.GTRDate(ds.Tables[0].Rows[i][1].ToString().Replace("'", "")) + "', " +
                            " '" + clsProc.GTRTime(ds.Tables[0].Rows[i][2].ToString().Replace("'", "")) + "', " +
                            " '" + clsProc.GTRTime(ds.Tables[0].Rows[i][3].ToString().Replace("'", "")) + "', " +
                            " '" + clsProc.GTRTime(ds.Tables[0].Rows[i][4].ToString().Replace("'", "")) + "', " +
                            " '" + ds.Tables[0].Rows[i][5].ToString().Replace("'", "") + "', " +
                            " '" + ds.Tables[0].Rows[i][14].ToString().Replace("'", "") + "', " +
                            " '" + Common.Classes.clsMain.strComputerName + "', " +
                            // " '" + ds.Tables[0].Rows[i][5].ToString().Replace("'", "").Replace("NA", "0").Replace("N/A", "0") + "', '" +
                            " '" + Common.Classes.clsMain.intUserId + "')";

                        arQuery.Add(sqlQuery);
                    }
                    else
                        break;
                }


               sqlQuery = " exec  [prcProcessAttendFix] " + Common.Classes.clsMain.intComId + ", '" +
               clsProc.GTRDate(dtInputDate.Value.ToString()) + "' ";
               arQuery.Add(sqlQuery);

                //Transaction with database server
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

      
        private void btnSend_Click(object sender, EventArgs e)
            
        {
            SMS sm = new SMS(cboPorts.Text);
            sm.Opens();
            sm.sendSMS(txtPhone.Text, txtMessage.Text);
            sm.Closes();
            MessageBox.Show("Message Sent!");

                //sendmessagetomobile();
        }
       


        protected void sendmessagetomobile()
        {

            try
            {

                string uid = "9999999999";
                string password = "yourway2smspassword";
                //string message = "em sanagathi...";

                string message = txtPhone.Text;
                string no = "999999999";

                //string no = TextBox1.Text;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://ubaid.tk/sms/sms.aspx?uid=" + uid + "&pwd=" + password +

                "&msg=" + message + "&phone=" + no + "&provider=way2sms");

                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                System.IO.

                StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());

                string responseString = respStreamReader.ReadToEnd();

                respStreamReader.Close();

                myResp.Close();
                MessageBox.Show("sent");
               // Response.Write("sent");
            }
            catch (WebException ee) { }
        }

        private void btnSendSmsList_Click(object sender, EventArgs e)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            ArrayList arQuery = new ArrayList();
            //GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery = "";
            Int32 NewId = 0;

            try
            {
                SMS sm = new SMS(cboPorts.Text);
                sm.Opens();
                int count = 0;
                foreach (UltraGridRow row in gridList.Rows)
                {
                    if ((Int16.Parse(row.Cells["isChecked"].Value.ToString()) == 1) && (Int16.Parse(row.Cells["Mobileno"].Text.Length.ToString()) > 10) && (Int16.Parse(row.Cells["Mobileno"].Text.Length.ToString()) < 15) && (Int16.Parse(row.Cells["Remarks"].Text.Length.ToString()) > 0))
                    {
                        row.Appearance.BackColor = Color.Coral;
                        row.Cells["Sending_Sts"].Value = "Running";
                        row.Cells["Sending_Sts"].Refresh();
                        gridList.Refresh();

                        //sqlQuery = "Exec prcDBBackup '" + row.Cells["SLNo"].Value + "','','','" + Common.Classes.clsMain.intUserId + "'";
                        //clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                        sm.sendSMS(row.Cells["MobileNo"].Text.ToString(), row.Cells["empname"].Text.ToString() + " - " + row.Cells["status"].Text.ToString() + " - " + row.Cells["Remarks"].Text.ToString());

                        row.Appearance.BackColor = Color.BlanchedAlmond;
                        row.Cells["Sending_Sts"].Value = "Complete";
                        row.Cells["Sending_Sts"].Refresh();
                        gridList.Refresh();

                        count = count + 1;

                        //SMS Insert
                        //NewId
                                //sqlQuery = "Select Isnull(Max(EmpId),0)+1 As NewId from tblEmp_Info";
                                //NewId = clsCon.GTRCountingData(sqlQuery);

                                sqlQuery = "Insert Into tblsms_send (ComId, empId, emp_Code, dtSent,tmSent, sendSMS, aId, PCName)"
                                           + " Values (" + Common.Classes.clsMain.intComId + ", '" +
                                           row.Cells["empId"].Value.ToString() + "', '" + row.Cells["emp_Code"] + "', '" +
                                           row.Cells["dtSent"] + "', '" + row.Cells["tmSent"] + "', '" + row.Cells["sendSMS"] +
                                           "', " + NewId + ", " + Common.Classes.clsMain.intUserId + "' ";
                                arQuery.Add(sqlQuery);

                    }
                }

                sm.Closes();

                MessageBox.Show("Message Sent !  " + count + " ");

                //MessageBox.Show("AT+CMGF=1" + (char)(13));


                //        dbName = dbName.Substring(0, dbName.Length - 1);

                // MessageBox.Show("Data backup SuccessFully");
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

        private void btnEmail_Click(object sender, EventArgs e)
        {


            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("server.networxhost.com");
                mail.From = new MailAddress("fahad@gtrbd.com");
                mail.To.Add("fahad@dominatebd.com");
                mail.Subject = "Test Mail - 1";
                mail.Body = "mail with attachment";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment("e:/textfile1.txt");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("fahad@gtrbd.com", "abc123");
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
                mail.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //try
            //{
            //    MailMessage m = new MailMessage();
            //    var fromAddress = new MailAddress("fahad@gtrbd.com", "Rubayet Fahad");
            //    var toAddress = new MailAddress("fahad@dominatebd.com", "Rubayet Fahad");
            //    const string fromPassword = "abc123";
            //    const string subject = "Bill for the Purchase";
            //    string body = "Dear Customer your Last Purchase bill is " + "";
            //    //server.networxhost.com smtp.gmail.com


            //    //m.Attachments.Add(new Attachment("textfile.txt", ""));

            //    //Attachment attch = new Attachment(ms, new System.Net.Mime.ContentType("application/pdf"));
                
            //    //m.Attachments.Add(new Attachment("7.exe", "application/octet-stream"));

            //    var smtp = new SmtpClient
            //    {


               
            //        Host = "server.networxhost.com",
                    
            //        Port = 587,
            //        EnableSsl = true,
            //        DeliveryMethod = SmtpDeliveryMethod.Network,
            //        UseDefaultCredentials = false,
            //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)



            //    };

            //    System.Net.Mail.Attachment attachment;
            //    attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            //    m.Attachments.Add(attachment);

            //    using
            //     (var message = new MailMessage(fromAddress, toAddress)
            //     {
                     
            //         Subject = subject,Body = body

            //         //Attachments = attachment
            //     }
            //       )
            //    {
            //        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            //        smtp.Send(message);
            //        MessageBox.Show("Message Sent");
            //    }

            //}

            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
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


        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult dlgRes = 
                    MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Test.xls" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

        //private void SendEmail(string pdfpath, string firstname, string lastname, string title, string company, string mailfrom, string mailto)
        //{
        //    try
        //    {
        //        MailMessage m = new MailMessage();
        //        System.Net.Mail.SmtpClient sc = new System.Net.Mail.SmtpClient();

        //        m.From = new System.Net.Mail.MailAddress(mailfrom);
        //        m.To.Add(mailto);

        //        m.Subject = "Company Gastzugang (" + lastname + ", " + firstname + ")";


        //        // what I must do for sending a pdf with this email 

        //        m.Body = "Gastzugangdaten sind im Anhang enthalten";

        //        sc.Host = SMTPSERVER; // here is the smt path

        //        sc.Send(m);
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Visible = true;
        //        lblErrorMessage.Text = "Folgender Fehler ist aufgetreten: " + ex.Message;
        //    }
        //}

    }
}