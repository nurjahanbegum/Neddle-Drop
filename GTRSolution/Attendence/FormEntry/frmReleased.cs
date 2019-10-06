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

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmReleased : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmReleased(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
            Int32 NewId = 0;
            Int64 ChkRel = 0;

            try
            {
                //Member Master Table
                if (btnSave.Text.ToString() != "&Save")
                {
                    //Update data
                    sqlQuery = " Update tblEmp_Released Set dtReleased = '" + clsProc.GTRDate(dtReleasedDate.Value.ToString()) + "',remarks = '" + txtRemarks.Text.ToString() + "' Where RelID = " + Int32.Parse(txtId.Text.ToString());
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update','" + cboEmpID.Value.ToString() + "')";
                    arQuery.Add(sqlQuery);


                    sqlQuery = " Update tblEmp_info Set dtReleased = '" + clsProc.GTRDate(dtReleasedDate.Value.ToString()) + "', IsInactive = 1 Where empid =  '" + cboEmpID.Value.ToString() + "' and ComId = " + Common.Classes.clsMain.intComId + "";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery= "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update','" + cboEmpID.Value.ToString() + "')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully.");
                }
                else
                {

                    sqlQuery = "Select dbo.fncCheckEmpRel (" + Common.Classes.clsMain.intComId + ", '" + cboEmpID.Value.ToString() + "')";
                    ChkRel = clsCon.GTRCountingDataLarge(sqlQuery);


                    if (ChkRel == 1)
                    {
                        MessageBox.Show("This Employee ID already Exist. Please input another Employee ID.");
                        return;
                    }
                    
                    //add new
                    sqlQuery = "Select Isnull(Max(RelID),0)+1 As NewId from tblEmp_Released";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert data
                    sqlQuery = "Insert Into tblEmp_Released ( aId,RelID, empid, dtReleased, Remarks,ComId ,LUserId,PcName) "
                        + " Values (" + NewId + ", " + NewId + ", '" + cboEmpID.Value.ToString() + "', '" + clsProc.GTRDate(dtReleasedDate.Value.ToString()) + "',  '" + txtRemarks.Text.ToString() + "'," + Common.Classes.clsMain.intComId + "," + Common.Classes.clsMain.intUserId + ",'" + Common.Classes.clsMain.strComputerName + "')";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert','" + cboEmpID.Value.ToString() + "')";
                    arQuery.Add(sqlQuery);



                    sqlQuery = " Update tblEmp_info Set dtReleased = '" + clsProc.GTRDate(dtReleasedDate.Value.ToString()) + "', IsInactive = 1  Where empid =  '" + cboEmpID.Value.ToString() + "' and ComId = " + Common.Classes.clsMain.intComId + "";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert','" + cboEmpID.Value.ToString() + "')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully.");
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

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["Paysource"].Hidden = true;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["relid"].Hidden = true;  //Country Name


                
                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 180;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 180;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 180;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 180;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["GS"].Width = 180;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["Grade"].Width = 180;  //Country Name
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 180;  //Country Name


                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["GS"].Header.Caption = "GS";
                gridList.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
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
                string sqlQuery = "Exec prcGetReleased 0,"+ Common.Classes.clsMain.intComId +"";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblEmployee";
                dsList.Tables[1].TableName = "tblEmployeeID";
                //dsList.Tables[2].TableName = "Country";

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

        private void prcLoadCombo()
        {
            cboEmpID.DataSource = null;
            cboEmpID.DataSource = dsList.Tables["tblEmployeeID"];
            cboEmpID.DisplayMember = "empcode";
            cboEmpID.ValueMember = "empid";


        }

        private void frmReleased_Load(object sender, EventArgs e)
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

        private void frmReleased_FormClosing(object sender, FormClosingEventArgs e)
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
            this.dtReleasedDate.Value = DateTime.Now;
            this.dtJoinDate.Value = DateTime.Now;


            this.btnSave.Text = "&Save";
            this.btnActivate.Enabled = false;
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
        }



        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetReleased " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblReleased";

                DataRow dr;
                if (dsDetails.Tables["tblReleased"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblReleased"].Rows[0];

                    this.txtId.Text = dr["relid"].ToString();
                    this.cboEmpID.Value = dr["empid"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtRemarks.Text = dr["remarks"].ToString();
                    this.dtReleasedDate.Text = dr["relDate"].ToString();

                    
                    this.btnSave.Text = "&Update";
                    this.btnActivate.Enabled = true;
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
                prcDisplayDetails(gridList.ActiveRow.Cells["RelID"].Value.ToString());
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
            cboEmpID.DisplayLayout.Bands[0].Columns["relid"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["remarks"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["dtReleased"].Hidden = true;



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


                    txtName.Text = cboEmpID.ActiveRow.Cells["empName"].Value.ToString();
                    dtJoinDate.Value = cboEmpID.ActiveRow.Cells["dtJoin"].Value.ToString();
                    dtReleasedDate.Value = cboEmpID.ActiveRow.Cells["dtReleased"].Value.ToString();
                    txtRemarks.Value = cboEmpID.ActiveRow.Cells["remarks"].Value.ToString();

                    this.btnSave.Text = "&Save";

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

        private void cboEmpID_ValueChanged(object sender, EventArgs e)
        {
            if (cboEmpID.Value == null)
            {
                return;
            }

            //prcDisplayDetails(cboEmpID.ActiveRow.Cells["RelID"].Text.ToString());
        }

        private void txtRemarks_MouseHover(object sender, EventArgs e)
        {
               //Color defaultColor;
               // Color hoverColor = Color.Orange;
            //this.txtRemarks a = (txtRemarks)sender;
            //a.BackColor = SystemColors.Window;
            ////OnMouseHover(this.BackColor);
            //this.txtRemarks.BackColor = System.Drawing.Color.DarkCyan;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Active  [" + txtName.Text + "] as Current Employee", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                sqlQuery = "Delete from tblEmp_Released Where RelID = " + Int32.Parse(txtId.Text) + " and ComId = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                sqlQuery = " Update tblEmp_info Set IsInactive = 0,dtReleased = null Where empid =  '" + cboEmpID.Value.ToString() + "' and ComId = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType,EmpId)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Active','" + cboEmpID.Value.ToString() + "')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Activate Successfully.");

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete  [" + txtName.Text + "] as Released Employee", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                sqlQuery = "Delete from tblEmp_Released Where RelID = " + Int32.Parse(txtId.Text) + " and ComId = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType,EmpId)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete','" + cboEmpID.Value.ToString() + "')";
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


    }
}
