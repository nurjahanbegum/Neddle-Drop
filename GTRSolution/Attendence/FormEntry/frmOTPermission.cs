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

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmOTPermission : Form
    {
        private string strValue = "";

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private string Data = "";

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmOTPermission(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
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

                            sqlQuery = " Delete  tblOTPermission where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "' and Approval = 'N'";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblOTPermission(empid,dtPunchDate,PrevOTHour,OtHour,Remarks,Luserid,comid,pcname,Approval) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                       row.Cells["dtPunchDate"].Text.ToString() + "','" +
                                       row.Cells["PrevOT"].Value.ToString() + "','" +
                                       row.Cells["otHour"].Value.ToString() + "',' ' + '" +
                                       row.Cells["Remarks"].Value.ToString() + "'," + Common.Classes.clsMain.intUserId +
                                       "," + Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "','N')";
                            arQuery.Add(sqlQuery);

                            sqlQuery = " Update P Set P.FirstAppId = I.FirstAppId,P.FinalAppId = I.FinalAppId,P.AppFirst = I.AppFirst,P.AppFinal = I.AppFinal"
                                        + " from tblOTPermission as P "
                                        + " inner join tblInput_Permission as I on I.ComId = P.ComID "
                                        + " inner join tblEmp_Info as E on E.ComId = P.ComID and E.OfficeGrade = I.EmpType "
                                        + " Where P.EmpId = '" + row.Cells["empid"].Text.ToString() 
                                        + "' and P.ComId = " + Common.Classes.clsMain.intComId + " and P.dtPunchDate = '"
                                        + row.Cells["dtPunchDate"].Text.ToString() + "' and P.Approval = 'N' and I.PType = 'Over Time'";
                            arQuery.Add(sqlQuery);

                        }
                    }

                    //sqlQuery = " exec  [prcProcessOTPermission] " + Common.Classes.clsMain.intComId + ", '" +
                    //           clsProc.GTRDate(dtInputDate.Value.ToString()) + "','" + clsProc.GTRDate(dtInputToDate.Value.ToString()) + "' ";
                    //arQuery.Add(sqlQuery);

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
                cboDept.Focus();

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
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 160; //Country Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 150;
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Width = 150;
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 90; //
                gridList.DisplayLayout.Bands[0].Columns["PrevOT"].Width = 60; // Previous OT Hour
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Width = 65; //
                gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Hidden = true; //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee ID";
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section Name";
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Header.Caption = "Date";
                gridList.DisplayLayout.Bands[0].Columns["OTHour"].Header.Caption = "Ot Hour";
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";
                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //this.gridList.DisplayLayout.Bands[0].Columns["othour"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                //this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                //this.gridList.DisplayLayout.Bands[0].Columns["Timeout"].Format = "HH:mm:ss";
                this.gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Format = "dd-MMM-yyyy";

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["empName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].CellActivation = Activation.NoEdit;
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
                string sqlQuery = "Exec [prcGetOTPermission] 0," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + dtInputDate.Value.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblDept";
                dsList.Tables[2].TableName = "tblSect";
                dsList.Tables[3].TableName = "tblSubSect";
                dsList.Tables[4].TableName = "tblBand";
                dsList.Tables[5].TableName = "tblShift";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];
                groupBoxCombo.Enabled = true;


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
            cboDept.DataSource = null;
            cboDept.DataSource = dsList.Tables["tblDept"];
            cboDept.DisplayMember = "DeptName";
            cboDept.ValueMember = "DeptID";

            cboSection.DataSource = null;
            cboSection.DataSource = dsList.Tables["tblSect"];
            cboSection.DisplayMember = "sectname";
            cboSection.ValueMember = "sectid";

            cboSubSection.DataSource = null;
            cboSubSection.DataSource = dsList.Tables["tblSubSect"];
            cboSubSection.DisplayMember = "SubSectName";
            cboSubSection.ValueMember = "SubSectId";

            cboBand.DataSource = null;
            cboBand.DataSource = dsList.Tables["tblBand"];
            cboBand.DisplayMember = "varname";
            cboBand.ValueMember = "aid";

            cboShiftTime.DataSource = null;
            cboShiftTime.DataSource = dsList.Tables["tblShift"];
            cboShiftTime.DisplayMember = "shiftname";
            cboShiftTime.ValueMember = "shiftid";

        }

        private void frmOTPermission_Load(object sender, EventArgs e)
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

        private void frmOTPermission_FormClosing(object sender, FormClosingEventArgs e)
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
            this.cboDept.Value = null;
            //this.cboDept.Text = "";
            //this.txtName.Text = "";
            this.dtInputDate.Value = DateTime.Now;

            this.cboSection.Value = null;
            this.cboShiftTime.Value = null;
            this.cboBand.Value = null;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboDept.Focus();

            //this.optCriteria.Value = "All";
            groupData.Enabled = false;
            groupBoxCombo.Enabled = true;
            this.cboDept.Enabled = false;
            this.cboSection.Enabled = false;
            this.cboSubSection.Enabled = false;
            this.cboShiftTime.Enabled = false;
            this.cboBand.Enabled = false;
            btnFillData.Enabled = false;

        }

        private Boolean fncBlank()
        {


            if (optCriteria.Value == "Dept")
            {
                if (this.cboDept.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Department");
                    cboDept.Focus();
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
            else if (optCriteria.Value == "SubSec")
            {
                if (this.cboSubSection.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Sub Section");
                    cboSubSection.Focus();
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
            else if (optCriteria.Value == "Band")
            {
                if (this.cboBand.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Band");
                    cboBand.Focus();
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
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblOTPermission As A

                        sqlQuery = " Delete  tblOTPermission where empid = '" + row.Cells["empid"].Text.ToString() +
                                   "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                cboDept.Focus();
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



        private void cboDept_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboDept_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboDept_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            cboDept.DisplayLayout.Bands[0].Columns["DeptName"].Width = cboDept.Width;

            cboDept.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Department";
            cboDept.DisplayLayout.Bands[0].Columns["DeptId"].Hidden = true;

            cboDept.DisplayMember = "DeptName";
            cboDept.ValueMember = "DeptID";

        }

        private void dtJoinDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBand.DisplayLayout.Bands[0].Columns["varname"].Width = cboBand.Width;
            cboBand.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Status";
            cboBand.DisplayLayout.Bands[0].Columns["aid"].Hidden = true;
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

        private void cboSubSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSubSection.DisplayLayout.Bands[0].Columns["SubSectName"].Width = cboSubSection.Width;

            cboSubSection.DisplayLayout.Bands[0].Columns["Subsectname"].Header.Caption = "Sub Section";
            cboSubSection.DisplayLayout.Bands[0].Columns["SubsectId"].Hidden = true;
        }


        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            if (optCriteria.Value == "ALL")
            {
                groupBoxCombo.Enabled = false;
                cboDept.Enabled = false;
                cboSection.Enabled = false;
                cboSubSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboBand.Enabled = false;
            }
            else if (optCriteria.Value == "Dept")
            {
                groupBoxCombo.Enabled = true;
                cboDept.Enabled = true;
                cboSection.Enabled = false;
                cboSubSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboBand.Enabled = false;
            }
            else if (optCriteria.Value == "Sec")
            {
                groupBoxCombo.Enabled = true;
                cboDept.Enabled = false;
                cboSection.Enabled = true;
                cboSubSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboBand.Enabled = false;
            }
            else if (optCriteria.Value == "SubSec")
            {
                groupBoxCombo.Enabled = true;
                cboDept.Enabled = false;
                cboSection.Enabled = false;
                cboSubSection.Enabled = true;
                cboShiftTime.Enabled = false;
                cboBand.Enabled = false;
            }

            else if (optCriteria.Value == "Band")
            {
                groupBoxCombo.Enabled = true;
                cboDept.Enabled = false;
                cboSection.Enabled = false;
                cboSubSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboBand.Enabled = true;
            }

            else if (optCriteria.Value == "ShiftTime")
            {
                groupBoxCombo.Enabled = true;
                cboDept.Enabled = false;
                cboSection.Enabled = false;
                cboSubSection.Enabled = false;
                cboShiftTime.Enabled = true;
                cboBand.Enabled = false;
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
            }
            else if (optCriteria.Value == "Dept")
            {
                Data = cboDept.Value.ToString();
            }
            else if (optCriteria.Value == "Sec")
            {
                Data = cboSection.Value.ToString();
            }
            else if (optCriteria.Value == "SubSec")
            {
                Data = cboSubSection.Value.ToString();
            }
            else if (optCriteria.Value == "Band")
            {
                Data = cboBand.Text.ToString();
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                Data = cboShiftTime.Value.ToString();
            }


            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetOTPermission] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;

                    txtOT.Value = "0.0";
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

                    row.Cells["othour"].Value = txtOT.Value.ToString();


                }
            }

        }


        private void cboDept_ValueChanged(object sender, EventArgs e)
        {
            if (cboDept.Value == null)
                return;

            strValue = cboDept.Value.ToString();
        }

        private void cboSection_ValueChanged(object sender, EventArgs e)
        {
            if (cboSection.Value == null)
                return;

            strValue = cboSection.Value.ToString();
        }

        private void cboSubSection_ValueChanged(object sender, EventArgs e)
        {
            if (cboSubSection.Value == null)
                return;

            strValue = cboSubSection.Value.ToString();
        }

        private void cboBand_ValueChanged(object sender, EventArgs e)
        {
            if (cboBand.Value == null)
                return;

            strValue = cboBand.Text.ToString();
        }

        private void cboShiftTime_ValueChanged(object sender, EventArgs e)
        {
            if (cboShiftTime.Value == null)
                return;

            strValue = cboShiftTime.Value.ToString();
        }


        private void ultraButton1_Click(object sender, EventArgs e)
        {

            if (optCriteria.Value == "ALL")
            {

            }
            else if (optCriteria.Value == "Dept")
            {
                Data = cboDept.Value.ToString();
            }
            else if (optCriteria.Value == "Sec")
            {
                Data = cboSection.Value.ToString();
            }
            else if (optCriteria.Value == "SubSec")
            {
                Data = cboSubSection.Value.ToString();
            }
            else if (optCriteria.Value == "Band")
            {
                Data = cboBand.Text.ToString();
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                Data = cboShiftTime.Value.ToString();
            }



            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            //dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetOTPermission] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value +
                                  "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;

                    txtOT.Value = "0.0";
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

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked == true)
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



    }
}