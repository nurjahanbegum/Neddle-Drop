using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmEmpCasual : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmpCasual(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                                Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmEmpCasual_Load(object sender, EventArgs e)
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

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetEmployeeCasual] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";
                dsList.Tables[1].TableName = "tblEmpType";
                dsList.Tables[2].TableName = "tblDesig";
                dsList.Tables[3].TableName = "tblSection";
                dsList.Tables[4].TableName = "tblshift";
                dsList.Tables[5].TableName = "tblShitCat";
                dsList.Tables[6].TableName = "tblReligion";
                dsList.Tables[7].TableName = "tblsex";
                dsList.Tables[8].TableName = "tblCSComp";




                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clsCon = null;
            }
        }

        private void prcLoadCombo()
        {

            //cboCode.DataSource = null;
            //cboCode.DataSource = dsList.Tables["tblgrid"];

            cboEmpType.DataSource = null;
            cboEmpType.DataSource = dsList.Tables["tblEmpType"];

            cboDesig.DataSource = null;
            cboDesig.DataSource = dsList.Tables["tblDesig"];

            cboSec.DataSource = null;
            cboSec.DataSource = dsList.Tables["tblSection"];

            cboShift.DataSource = null;
            cboShift.DataSource = dsList.Tables["tblshift"];

            cboShiftCat.DataSource = null;
            cboShiftCat.DataSource = dsList.Tables["tblShitCat"];

            cboRelegion.DataSource = null;
            cboRelegion.DataSource = dsList.Tables["tblReligion"];

            cboSex.DataSource = null;
            cboSex.DataSource = dsList.Tables["tblsex"];

            cboCom.DataSource = null;
            cboCom.DataSource = dsList.Tables["tblCSComp"];

            dtJDate.Value = DateTime.Today;
            dtBirthDate.Value = DateTime.Today;


            cboEmpType.Text = "Casual Worker";

            cboShift.Text = "";
            cboShiftCat.Text = "";
            cboRelegion.Text = "Islam";
            cboSex.Text = "Male";
        }
        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetEmployeeCasual " + Common.Classes.clsMain.intComId + " , " +
                                  Int32.Parse(strParam) + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];

                    this.txtnewcode.Text = dr["Empid"].ToString();
                    this.txtCode.Text = dr["EmpCode"].ToString();
                    //this.cboCode.Value = dr["empid"].ToString();
                    this.cboEmpType.Text = dr["emptype"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtBName.Text = dr["EmpNameb"].ToString();
                    this.cboDesig.Value = dr["DesigID"].ToString();
                    this.cboSec.Value = dr["SectId"].ToString();
                    this.txtGS.Text = dr["GS"].ToString();
                    this.cboShift.Text = dr["ShiftType"].ToString();
                    this.cboShiftCat.Text = dr["ShiftCat"].ToString();
                    this.cboRelegion.Text = dr["Religion"].ToString();
                    this.cboSex.Text = dr["Sex"].ToString();
                    this.cboCom.Text = dr["CSComName"].ToString();
                    this.txtHr.Text = dr["PerHr"].ToString();
                    this.txtCardNo.Text = dr["cardno"].ToString();

                    this.dtBirthDate.Value = dr["dtBirth"];
                    this.dtJDate.Value = dr["dtJoin"];
                    this.txtCode.Text = dr["EmpCode"].ToString();


                    this.txtCode.Value = dr["empCode"].ToString();


                    if (dr["IsAllowOT"].ToString() == "1")
                    {
                        checkOT.Checked = true;
                    }
                    if (dr["IsInactive"].ToString() == "1")
                    {
                        checkYesNo.Checked = true;
                    }

                    this.btnSave.Text = "&Update";
                    //this.btnSave.Enabled = false;
                    this.btnDelete.Enabled = true;
                }
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prcClearData()
        {
            //cboCode.Text = "";
            cboEmpType.Value = "";
            txtCode.Text = "";
            txtnewcode.Text = "";

            txtName.Text = "";
            txtBName.Text = "";

            cboSec.Value = "";
            cboDesig.Value = "";
            cboCom.Value = "";
            txtHr.Value = "";
            txtCardNo.Text = "";
            txtGS.Text = "";



            dtJDate.Value = DateTime.Today;
            dtBirthDate.Value = DateTime.Today;

            cboShift.Value = "";
            cboShiftCat.Value = "";
            cboRelegion.Value = "";



            checkOT.Checked = false;
            checkYesNo.Checked = false;
            cboEmpType.Text = "Casual Worker";

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
            txtCode.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
            prcLoadList();
            prcLoadCombo();
            btnSave.Enabled = true;
        }

        private void cboEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpType.DisplayLayout.Bands[0].Columns["emptype"].Width = cboEmpType.Width;
            cboEmpType.DisplayLayout.Bands[0].Columns["emptype"].Header.Caption = "Employee Type";
            //cboEmpType.DisplayMember = "emptype";
            //cboEmpType.ValueMember = "emptype";
        }

        private void cboDesig_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDesig.DisplayLayout.Bands[0].Columns["DesigName"].Width = cboDesig.Width;
            cboDesig.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
            cboDesig.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;
            cboDesig.DisplayMember = "DesigName";
            cboDesig.ValueMember = "DesigId";
        }

        private void cboSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSec.Width;
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            cboSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboSec.DisplayMember = "SectName";
            cboSec.ValueMember = "SectId";
        }

        private void cboCom_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCom.DisplayLayout.Bands[0].Columns["CSComName"].Width = cboCom.Width;
            cboCom.DisplayLayout.Bands[0].Columns["CSComName"].Header.Caption = "Company Name";
            cboCom.DisplayLayout.Bands[0].Columns["CSComId"].Hidden = true;
            cboCom.DisplayMember = "CSComName";
            cboCom.ValueMember = "CSComId";
        }


        private void cboShift_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboShift.DisplayLayout.Bands[0].Columns["ShiftType"].Width = cboShift.Width;
            cboShift.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Shift Type";
            cboShift.DisplayMember = "ShiftType";
            cboShift.ValueMember = "ShiftID";
        }

        private void cboShiftCat_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboShiftCat.DisplayLayout.Bands[0].Columns["ShiftCat"].Width = cboShiftCat.Width;
            cboShiftCat.DisplayLayout.Bands[0].Columns["ShiftCat"].Header.Caption = "Shift Catagory";
            cboShiftCat.DisplayMember = "ShiftCat";
            cboShiftCat.ValueMember = "ShiftCat";
        }

        private void cboRelegion_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboRelegion.DisplayLayout.Bands[0].Columns["Religion"].Width = cboRelegion.Width;
            cboRelegion.DisplayLayout.Bands[0].Columns["Religion"].Header.Caption = "Religion";
            cboRelegion.DisplayMember = "Religion";
            cboRelegion.ValueMember = "Religion";
        }

        private void cboSex_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSex.DisplayLayout.Bands[0].Columns["Sex"].Width = cboSex.Width;
            cboSex.DisplayLayout.Bands[0].Columns["Sex"].Header.Caption = "Gender";
            cboSex.DisplayMember = "Sex";
            cboSex.ValueMember = "Sex";
        }



        private void frmEmpCasual_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Grid Width
                gridList.DisplayLayout.Bands[0].Columns["empid"].Hidden = true; //Employee ID
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80; //Employee code
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 100; //Employee Name
                gridList.DisplayLayout.Bands[0].Columns["CSComName"].Width = 80; //Employee Name
                gridList.DisplayLayout.Bands[0].Columns["EmpSec"].Width = 100; //Section 
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 100; //Designation Name
                gridList.DisplayLayout.Bands[0].Columns["EmpType"].Width = 100; //Emp Type
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 100; //Join Date

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["CSComName"].Header.Caption = "Company Name";
                gridList.DisplayLayout.Bands[0].Columns["EmpSec"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Emp Type";
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";

                this.gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Format = "dd-MMM-yyyy";

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

        private Boolean fncBlank()
        {


            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide Name");
                txtName.Focus();
                return true;
            }

            if (this.cboSec.Text.Length == 0)
            {
                MessageBox.Show("Please provide employee section");
                cboSec.Focus();
                return true;
            }





            return false;
        }


        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridList.ActiveRow.IsFilterRow == false)
                {
                    prcClearData();

                    // prcLoadList();
                    //prcLoadCombo();
                    prcDisplayDetails(gridList.ActiveRow.Cells["empid"].Value.ToString());
                    //cboCode.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int64 NewId = 0;
            try
            {

                //Member Master Table
                if (btnSave.Text != "&Save")
                {

                    //Update
                    sqlQuery = "Update tblEmpCasual_Info set   EmpCode = '" + this.txtCode.Text.ToString() + "', empType = '" +
                               this.cboEmpType.Value.ToString() + "', EmpName ='" + this.txtName.Text.ToString() +
                               "', empNameB = '" + this.txtBName.Text.ToString() + "', DesigID = '" +
                               this.cboDesig.Value.ToString() + "', SectId= '" + this.cboSec.Value.ToString() +
                               "', CSComId= '" + this.cboCom.Value.ToString() + "',dtJoin= '" + clsProc.GTRDate(this.dtJDate.Value.ToString()) + "',GS='" +
                               this.txtGS.Text.ToString() + "',PerHr='" + this.txtHr.Text.ToString() + "',ShiftType='" +
                               this.cboShift.Text.ToString() + "',ShiftID='" + this.cboShift.Value.ToString() + "',ShiftCat= '" + this.cboShiftCat.Text.ToString() +
                               "',Religion='" + this.cboRelegion.Value.ToString() + "',Sex='" + this.cboSex.Value.ToString() +
                               "',cardno = '" + txtCardNo.Text.ToString() + "' ,IsAllowOT='" +
                               checkOT.Tag.ToString() + "',IsInactive='" + checkYesNo.Tag.ToString() 
                               + "' where empid =  '" + this.txtnewcode.Text + "' ";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update','" + this.txtnewcode.Text + "')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //NewId
                    sqlQuery = "Select Isnull(Max(EmpId),0)+1 As NewId from tblEmpCasual_Info";
                    NewId = clsCon.GTRCountingDataLarge(sqlQuery);

                    //Insert Data
                    sqlQuery = "Insert Into tblEmpCasual_Info (ComId,aEmpid,Empid, EmpCode, empType, EmpName, empNameB, DesigID, SectId,CSComId, dtJoin,GS,PerHr, ShiftType, ShiftID, ShiftCat, Religion, Sex,cardno, IsAllowOT, IsInactive)"
                               + " Values (" + Common.Classes.clsMain.intComId + ", " + NewId + ", " + NewId + ",'" +
                               NewId + "', '" + this.cboEmpType.Value.ToString() + "',' " +
                               this.txtName.Text + "',' " + this.txtBName.Text.ToString() + "', " +
                               this.cboDesig.Value.ToString() + ", " + this.cboSec.Value.ToString() + "," + this.cboCom.Value.ToString() + ",'" +
                               this.clsProc.GTRDate(this.dtJDate.Value.ToString()) + "','" +
                               this.txtGS.Text.ToString() + "','" + this.txtHr.Text.ToString() + "','" + this.cboShift.Text.ToString() + "','" + this.cboShift.Value.ToString() + "', '" + 
                               this.cboShiftCat.Text.ToString() + "','" +
                               this.cboRelegion.Value.ToString() + "','" + this.cboSex.Value.ToString() + "','" +
                               NewId + "', '" +
                               this.checkOT.Tag.ToString() + "','" +
                               this.checkYesNo.Tag.ToString() + "')";


                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert'," + NewId + ")";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                }
                prcClearData();
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
                //cboCode.Value = null;
            }
          }


        private void checkOT_CheckedChanged(object sender, EventArgs e)
        {
            checkOT.Tag = 0;
            if (checkOT.Checked == true)
            {
                checkOT.Tag = 1;
            }
        }


        private void checkYesNo_CheckedChanged(object sender, EventArgs e)
        {
            checkYesNo.Tag = 0;
            if (checkYesNo.Checked == true)
            {
                checkYesNo.Tag = 1;
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtBName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDesig_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCom_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtJDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtBirthDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtGS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboShift_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboShiftCat_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboRelegion_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboSex_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboGrade_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboLine_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboPaysource_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboPayMode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkOT_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkPF_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkYesNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtBName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtGS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }
        //private void txtHr_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        //}
        private void txtHr_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to Delete Employee information of [" + txtCode.Text.ToString() + "]", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";
                //Delete Data
                sqlQuery = "Delete from tblEmpCasual_Info Where EmpID = " + Int32.Parse(txtCode.Value.ToString()) + " and comid = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete'," + Int32.Parse(txtCode.Value.ToString()) + ")";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcClearData();
                prcLoadList();
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


        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCardNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }



        private void cboCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }



        private Double fncValidateDouble(string value)
        {
            Double dbl;
            try
            {
                dbl = Double.Parse(value);
            }
            catch (Exception)
            {
                dbl = 0;
            }
            return dbl;

        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (gridList.ActiveRow.IsFilterRow == false)
                {
                    prcClearData();

                    // prcLoadList();
                    //prcLoadCombo();
                    prcDisplayDetails(gridList.ActiveRow.Cells["empid"].Value.ToString());
                    //cboCode.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }









    }
}
