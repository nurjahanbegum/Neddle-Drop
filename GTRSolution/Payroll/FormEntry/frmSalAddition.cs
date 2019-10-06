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

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmSalAddition : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmSalAddition(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
            Int64 NewId = 0;
            Int64 ChkData = 0;

            DateTime lastDay = new DateTime(dtInputDate.DateTime.Year, dtInputDate.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtInputDate.Value = lastDay;

            try
            {
                //Member Master Table
                if (btnSave.Text.ToString() != "&Save")
                {
                    //Update data
                    sqlQuery = " Update tblSal_Addition Set dtInput = '" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "', amount = '" + clsProc.GTRValidateDouble(txtAmt.Value.ToString()) + "',remarks = '" + txtRemarks.Text.ToString() + "' Where addID = " + Int32.Parse(txtId.Text.ToString());
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery= "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully.");
                }
                else
                {
                    sqlQuery = "Select dbo.fncCheckData (" + Common.Classes.clsMain.intComId + ",'" + cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "','Salary Addition')";
                    ChkData = clsCon.GTRCountingDataLarge(sqlQuery);

                    if (ChkData == 1)
                    {
                        MessageBox.Show("This employee's salary addition amount already inputted. Please select another employee Id.");
                        return;
                    }
                    
                    //add new
                    sqlQuery = "Select Isnull(Max(addID),0)+1 As NewId from tblSal_Addition";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert data
                    sqlQuery = "Insert Into tblSal_Addition ( aId,addID, empid, dtInput,amount, Remarks,LUserId,comid ,PcName) "
                        + " Values (" + NewId + ", " + NewId + ", '" + cboEmpID.Value.ToString() + "', '" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "',  '" + clsProc.GTRValidateDouble(txtAmt.Value.ToString()) + "', '" + txtRemarks.Text.ToString() + "'," + Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ",'" + Common.Classes.clsMain.strComputerName + "')";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
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

                //								
                gridList.DisplayLayout.Bands[0].Columns["addID"].Hidden = true;  //addID
                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;  //EmpID
                gridList.DisplayLayout.Bands[0].Columns["Paysource"].Hidden = true;  //Paysource
                gridList.DisplayLayout.Bands[0].Columns["GS"].Hidden = true;  //GS

                
                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 140;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 80;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 120;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 120;  //Country Name

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name"; 


                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
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
                string sqlQuery = "Exec prcGetSalAddition 0," + Common.Classes.clsMain.intComId + ",0,'" + clsProc.GTRDate(dtDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblEmployee";
                dsList.Tables[1].TableName = "tblEmployeeID";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblEmployee"];

                this.dtDate.Value = DateTime.Now;

                if (dtDate.DateTime.Month == 1)
                {
                    if (dtDate.DateTime.Day <= 6)
                    {

                        var DaysInMonth = DateTime.DaysInMonth(dtDate.DateTime.Year, dtDate.DateTime.Month);
                        var lastDay = new DateTime(dtDate.DateTime.Year, dtDate.DateTime.Month, DaysInMonth);
                        dtDate.Value = lastDay;
                    }
                    else
                    {

                        DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        lastDay = lastDay.AddMonths(1);
                        lastDay = lastDay.AddDays(-(lastDay.Day));
                        dtDate.Value = lastDay;
                    }
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

        private void prcLoadCombo()
        {
            cboEmpID.DataSource = null;
            cboEmpID.DataSource = dsList.Tables["tblEmployeeID"];
            cboEmpID.DisplayMember = "empcode";
            cboEmpID.ValueMember = "empid";


        }

        private void frmSalAddition_Load(object sender, EventArgs e)
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

        private void frmSalAddition_FormClosing(object sender, FormClosingEventArgs e)
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
            this.cboEmpID.Value = null;
            this.cboEmpID.Text = "";
            this.txtName.Text = "";
            this.txtRemarks.Text = "";

            //DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //lastDay = lastDay.AddMonths(1);
            //lastDay = lastDay.AddDays(-(lastDay.Day));
            //dtInputDate.Value = lastDay;

            //this.dtInputDate.Value = DateTime.Now;
            this.dtJoinDate.Value = DateTime.Now;
            this.txtAmt.Text = "0";

            this.cboEmpID.Enabled = true;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboEmpID.Focus();
        }

        private Boolean fncBlank()
        {
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide Employee Name.");
                txtName.Focus();
                return true;
            }

           
            if (this.cboEmpID.Text.Length == 0)
            {
                MessageBox.Show("Please provide Employee ID.");
                cboEmpID.Focus();
                return true;
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
            this.dtInputDate.Value = DateTime.Now;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Active  [" + txtName.Text + "] as Current Employee" , "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery=new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                sqlQuery = "Delete from tblSal_Addition Where addID = " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

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
                string sqlQuery = "Exec prcGetSalAddition " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId + " ,1,'" + clsProc.GTRDate(dtDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblSalAdv";

                DataRow dr;
                if (dsDetails.Tables["tblSalAdv"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblSalAdv"].Rows[0];

                    this.txtId.Text = dr["addID"].ToString();
                    this.cboEmpID.Value = dr["EmpId"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtRemarks.Text = dr["remarks"].ToString();
                    this.dtInputDate.Text = dr["dtInput"].ToString();
                    this.txtAmt.Text = dr["Amount"].ToString();


                    
                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
                    this.cboEmpID.Enabled = false;
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
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["addID"].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

            cboEmpID.DisplayLayout.Bands[0].Columns["empName"].Width = 160;
            cboEmpID.DisplayLayout.Bands[0].Columns["empcode"].Width = 90;

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
                    dtJoinDate.Value = cboEmpID.ActiveRow.Cells["dtJoin"].Value.ToString();

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

        private void txtAmt_Leave(object sender, EventArgs e)
        {
            txtName.Text = txtName.Text.TrimStart();
        }

        private void txtRemarks_Leave(object sender, EventArgs e)
        {
            txtName.Text = txtName.Text.TrimStart();
        }

        private void txtAmt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            DateTime lastDay = new DateTime(dtDate.DateTime.Year, dtDate.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtDate.Value = lastDay;

            try
            {
                string sqlQuery = "Exec prcGetSalAddition 0," + Common.Classes.clsMain.intComId + ",1,'" + clsProc.GTRDate(dtDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblEmployee";


                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblEmployee"];



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
    }
}
