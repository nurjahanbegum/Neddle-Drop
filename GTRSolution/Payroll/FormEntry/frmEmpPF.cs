using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmEmpPF : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmpPF(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetEmployeePF] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                //Tab : Basic
                dsList.Tables[0].TableName = "tblempid";

                if (Int32.Parse(clsMain.strRelationalId) != 0)
                {
                    prcDisplayDetails(clsMain.strRelationalId);
                    clsMain.strRelationalId = "0";
                }
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
            //Tab Basic Information
            cboCode.DataSource = null;
            cboCode.DataSource = dsList.Tables["tblempid"];


        }

        private Boolean fncBlank()
        {

            if (this.txtReg.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Register No");
                txtReg.Focus();
                return true;
            }

            if (this.dtPF.Text.Length == 0)
            {
                MessageBox.Show("Please Provide PF Date");
                dtPF.Focus();
                return true;
            }

            return false;
        }

        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetEmployeePF " + Common.Classes.clsMain.intComId + " , " +
                                  Int32.Parse(strParam) + " ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";



                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    // Tab Basic Information\
                    //this.cboCode.Value = dr["empCode"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.dtJDate.Text = dr["dtJoin"].ToString();
                    this.dtPF.Text = dr["dtPF"].ToString();
                    this.txtDesig.Text = dr["DesigName"].ToString();
                    this.txtSectName.Text = dr["SectName"].ToString();
                    this.txtGS.Text = dr["GS"].ToString();
                    this.txtEmpType.Text = dr["EmpType"].ToString();
                    this.txtBand.Text = dr["Band"].ToString();
                    this.txtReg.Text = dr["RegNo"].ToString();

                    checkPF.Checked = false;
                    
                    if (dr["IsAllowPF"].ToString() == "1")
                    {
                        checkPF.Checked = true;
                    }

                }


                this.btnSave.Text = "&Save";
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

        private void prcClearData()
        {
            cboCode.Value = "";
            txtName.Text = "";
            dtJDate.Value = "";
            dtPF.Value = "";
            txtGS.Text = "";
            txtDesig.Text = "";
            txtSectName.Text = "";
            txtEmpType.Text = "";
            txtBand.Text = "";
            txtReg.Text = "";

            checkPF.Checked = false;

            btnSave.Text = "&Save";

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
            try
            {

                    sqlQuery = "Update tblEmp_Info Set RegNo = '" + txtReg.Value.ToString()
                               + "', dtPF = '" + clsProc.GTRDate(this.dtPF.Value.ToString()) + "' Where ComID = " + Common.Classes.clsMain.intComId 
                               + " and EmpID = '" + cboCode.Value.ToString() + "'";
                    arQuery.Add(sqlQuery);



                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Employee PF Info Update Successfully");


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
                //arQuery = null;
                //clsCon = null;
            }
        }

        private void frmEmpPF_Load(object sender, EventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void frmEmpPF_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
        }

        // Tab Basic
        private void cboCode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCode.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            
            cboCode.DisplayLayout.Bands[0].Columns["empName"].Width = 120;
            cboCode.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Name";
            
            cboCode.DisplayMember = "empCode";
            cboCode.ValueMember = "empId";
        }




        private void dtJDate_Validating(object sender, CancelEventArgs e)
        {
            if (dtPF.DateTime.ToString("dd-MMM-yyyy") == DateTime.Today.ToString("dd-MMM-yyyy") ||
                dtPF.Value.ToString().Length == 0)
            {
                dtPF.Value = dtJDate.DateTime.AddMonths(3);
            }

        }

        private void cboCurrCity_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCurrCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void dtJDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtPFDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtProvision_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtGS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtGS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRCurrency(e.KeyChar.ToString());
        }

        private void txtBName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtBS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRCurrency(e.KeyChar.ToString());
        }

        private void txtBS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboBank_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtAccNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboPaysource_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboShift_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboFloor_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboLine_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboPayMode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void checkPF_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void checkYes_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtFather_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtFather_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtFatherBName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtFatherBName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtMohter_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtMohter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtMotherBName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtMotherBName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtSpouse_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtSpouse_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtSpouseB_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtSpouseB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtNationalID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtNationalID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void cboNationality_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtBirth_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboBlood_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboRelegion_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCaste_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboSex_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboMarit_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtMail_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtMail_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtPassport_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtPassport_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtCurrAdd_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtCurrAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtPreadd_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtPreadd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void cboCurrPost_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboPrePost_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void cboPrePost_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboPrePS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboPreDist_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboPreCity_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCurrDist_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCurrPS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboEmpType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboDesig_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboGrade_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

       

        private void cboCode_ValueChanged(object sender, EventArgs e)
        {
            if (this.cboCode.IsItemInList() == false)
            {
                //MessageBox.Show("Please Provide valid data [or, select from list].");
                //cboEmpID.Focus();
                prcClearData();
                prcLoadCombo();
                return;
            }


            if (cboCode.Value == null)
            {
                return;
            }
            prcDisplayDetails(cboCode.Value.ToString());
        }


        private void txtName_Leave(object sender, EventArgs e)
        {
            txtName.Text = txtName.Text.TrimStart();
        }

        private void txtGS_Leave(object sender, EventArgs e)
        {
            txtGS.Text = txtGS.Text.TrimStart();
        }


        private void txtName_ValueChanged(object sender, EventArgs e)
        {
            txtName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtName.Text);
            //txtName.Focus();
            txtName.SelectionStart = txtName.Text.Length;
        }

        private void checkPF_CheckedChanged(object sender, EventArgs e)
        {
            checkPF.Tag = 0;
            if (checkPF.Checked == true)
            {
                checkPF.Tag = 1;
            }
        }

        //private void cboUnit_ValueChanged(object sender, EventArgs e)
        //{

        //    GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
        //    dsList = new System.Data.DataSet();


        //    try
        //    {

        //        string sqlQuery = "Exec [prcGetShift] '" + cboUnit.Value.ToString() + "', 0";
        //        clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
        //        //Tab : Basic
        //        dsList.Tables[0].TableName = "tblShift";

        //        prcLoadShift();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        clsCon = null;
        //    }

        //}






    }
}


