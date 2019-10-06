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
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmLeaveBalance : Form
    {
        string strValue = "";

        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        string Data = "";

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmLeaveBalance(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
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
                    try
                    {
                        foreach (UltraGridRow row in this.gridList.Rows)
                        {
                            if (row.Cells["isChecked"].Value.ToString() == "1")
                            {

                                sqlQuery = " Delete  tblLeave_Balance where empid = '" + row.Cells["empid"].Text.ToString() + "' and dtOpBal =  '" + row.Cells["dtOpeningDate"].Text.ToString() + "'";
                                arQuery.Add(sqlQuery);


                                sqlQuery = "insert into tblLeave_Balance (EmpId, dtOpBal, CL,SL, EL, ML,ComID)"
                                           + "values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                           row.Cells["dtOpeningDate"].Text.ToString() + "', '" +row.Cells["CL"].Value.ToString() +
                                           "' , '" + row.Cells["SL"].Value.ToString() + "','" + row.Cells["EL"].Value.ToString() +
                                           "', '" + row.Cells["ML"].Value.ToString() + "'," + Common.Classes.clsMain.intComId + ")";
                                          
                                arQuery.Add(sqlQuery);
                            }
                        }

                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                    + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        clsCon.GTRSaveDataWithSQLCommand(arQuery);
                        MessageBox.Show("Data Update Successfully");

                        prcLoadList();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        clsCon = null;
                    }
                else
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0)
                        {
                            //RowID = row.Index + 1;
                            ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                            sqlQuery = " Delete  tblLeave_Balance where empid = '" + row.Cells["empid"].Text.ToString() + "' ";
                            arQuery.Add(sqlQuery);
                        }
                    } 

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
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

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;  //Country Name
                
                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70;
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150; 

                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 130;  //
                gridList.DisplayLayout.Bands[0].Columns["dtOpeningDate"].Width = 60;  //
                gridList.DisplayLayout.Bands[0].Columns["CL"].Width = 55;  //
                gridList.DisplayLayout.Bands[0].Columns["SL"].Width = 55;  //
                gridList.DisplayLayout.Bands[0].Columns["EL"].Width = 55;  //
                gridList.DisplayLayout.Bands[0].Columns["ML"].Width = 55;  //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";

                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["dtOpeningDate"].Header.Caption = "Opening Date";
                gridList.DisplayLayout.Bands[0].Columns["CL"].Header.Caption = "CL";
                gridList.DisplayLayout.Bands[0].Columns["SL"].Header.Caption = "SL";
                gridList.DisplayLayout.Bands[0].Columns["EL"].Header.Caption = "EL";
                gridList.DisplayLayout.Bands[0].Columns["ML"].Header.Caption = "ML";
                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;

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
                string sqlQuery = "Exec [prcGetLeaveBalace] 0," + Common.Classes.clsMain.intComId + ",0,'" + optCriteria.Value + "','" + strValue + "','" + dtInputDate.Value + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblEmpID";
                dsList.Tables[2].TableName = "tblSect";
                dsList.Tables[3].TableName = "tblDate";
                //dsList.Tables[4].TableName = "tblSts";

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

            dtInputDate.DataSource = null;
            dtInputDate.DataSource = dsList.Tables["tblDate"];
            if (dtInputDate.Rows.Count > 0)
            {
                dtInputDate.Value = dtInputDate.Rows[0].Cells["dtOpeningDate"].Value.ToString();
            }
        }

        private void frmLeaveBalance_Load(object sender, EventArgs e)
        {
            try
            {


                prcClearData();
                prcLoadList();
                prcLoadCombo();
                groupData.Enabled = false;
                btnFillData.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLeaveBalance_FormClosing(object sender, FormClosingEventArgs e)
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
            this.dtInputDate.Value = null;

            this.cboSection.Value = null;
            this.cboShiftTime.Value = null;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboEmpID.Focus();

            this.optCriteria.Value = "All";
            //groupData.Enabled = false;
            groupBoxCombo.Enabled = false;
            txtName.Enabled = false;
            this.cboSection.Enabled = false;
            this.cboShiftTime.Enabled = false;
            btnFillData.Enabled = false;

            groupData.Enabled = false;
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

            if (this.dtInputDate.Text.Length == 0)
            {
                MessageBox.Show("Please provide Balance Opening Date code no.");
                dtInputDate.Focus();
                return true;
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete Leave Which Are shown in the Grid" , "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery=new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["isChecked"].Value.ToString() == "1")
                    {
                        //RowID = row.Index + 1;
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                        sqlQuery = " Delete  tblLeave_Balance where empid = '" + row.Cells["empid"].Text.ToString() + "' ";
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

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAtt] " + Int32.Parse(strParam)+","+Common.Classes.clsMain.intComId ;
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



        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            if (optCriteria.Value == "All")
            {
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                groupBoxCombo.Enabled = false;
            }
            else if (optCriteria.Value == "EmpID")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = true;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
            }
            else if (optCriteria.Value == "Sec")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = true;
                cboShiftTime.Enabled = false;
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = true;
            }
            else if (optCriteria.Value == "Status")
            {
                groupBoxCombo.Enabled = true;
                cboEmpID.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
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
               Data  = "";
            }
            else if (optCriteria.Value == "EmpID")
            {
                Data = cboEmpID.Value.ToString();
            }
            else if (optCriteria.Value == "Sec")
            {
                Data = cboSection.Value.ToString();
            }


            if (cboEmpID.Value == null)
            { 
                cboEmpID.Value = 0;
            }

            if (cboSection.Value == null)
            {
                cboSection.Value = 0;
            }


            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetLeaveBalace] 1," + Common.Classes.clsMain.intComId + ",'" + cboEmpID.Value + "','" + optCriteria.Value + "','" + strValue + "','" + dtInputDate.Value + "','" + cboSection.Value + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;

                    //cboStatus1.Text = "P";
                    //dtTimeIn.Text = "1-1-1900 08:00";
                    //dtTimeOut.Text = "1-1-1900 08:00";
                    //dtOt.Text = "1-1-1900 08:00";
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

            btnSave.Text = "&Update";
            btnDelete.Enabled = true;


        }

        private void btnFillData_Click(object sender, EventArgs e)
        {

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
            {
                if (row.Cells["isChecked"].Value.ToString() == "1")
                {
                    //RowID = row.Index + 1;
                    ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                    row.Cells["cl"].Value = txtCL.Value.ToString();
                    row.Cells["sl"].Value = txtSL.Value.ToString();
                    row.Cells["el"].Value = txtEL.Value.ToString();
                    row.Cells["dtOpeningDate"].Value = dtInputDate.Text.ToString();

                }
            } 

        }

        private void cboEmpID_ValueChanged(object sender, EventArgs e)
        {
         
            if(cboEmpID.Value == null)
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



            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            //dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAtt] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value + "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;

                    //cboStatus1.Text = "P";
                    //dtTimeIn.Text = "1-1-1900 08:00";
                    //dtTimeOut.Text = "1-1-1900 08:00";
                    //dtOt.Text = "1-1-1900 08:00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }


        }

        private void dtInputDate_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            dtInputDate.DisplayLayout.Bands[0].Columns["dtOpeningDate"].Width = dtInputDate.Width;
            dtInputDate.DisplayLayout.Bands[0].Columns["dtOpeningDate"].Header.Caption = "Opening Date";
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupData.Enabled = true;
                btnFillData.Enabled = true;
            }
            else 
            {
                groupData.Enabled = false;
                btnFillData.Enabled = false;
            }
        }

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
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
            dlgSurveyExcel.FileName = "Leave Balance List.xls" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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
    }
}
