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

namespace GTRHRIS.Campus.FormEntry
{
    public partial class frmStudent : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();
        private string EmpPic = @"Z:\Com\Pics\EmpPic";
        private string PicName;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmStudent(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlQuery = "Exec [prcGetEmployee] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                //Tab : Basic
                dsList.Tables[0].TableName = "tblempid";
                dsList.Tables[1].TableName = "tblemptype";
                dsList.Tables[2].TableName = "tblBlood";
                dsList.Tables[3].TableName = "tblMaritialsts";
                dsList.Tables[4].TableName = "tblRelegion";
                dsList.Tables[5].TableName = "tblCaste";
                dsList.Tables[6].TableName = "tblSex";
                dsList.Tables[7].TableName = "tblNationality";

                // Tab Office....
                dsList.Tables[8].TableName = "TblDesignation";
                dsList.Tables[9].TableName = "tblsection";
                dsList.Tables[10].TableName = "tblGrade";
                dsList.Tables[11].TableName = "tblLine";
                dsList.Tables[12].TableName = "tblFloor";
                dsList.Tables[13].TableName = "tblShift";
                dsList.Tables[14].TableName = "tblPaysource";
                dsList.Tables[15].TableName = "tblPayMode";
                dsList.Tables[16].TableName = "tblEducation";
                dsList.Tables[17].TableName = "tblExpiriance";
                dsList.Tables[18].TableName = "tblBankName";
                dsList.Tables[19].TableName = "tblDistrict";
                dsList.Tables[20].TableName = "admissnForm";
                dsList.Tables[21].TableName = "template";
                dsList.Tables[22].TableName = "gridExam";

                gridEdu.DataSource = null;
                gridEdu.DataSource = dsList.Tables["tblEducation"];

                gridExp.DataSource = null;
                gridExp.DataSource = dsList.Tables["tblExpiriance"];

                gridExam.DataSource = null;
                gridExam.DataSource = dsList.Tables["gridExam"];

                if (Int32.Parse(clsMain.strRelationalId) != 0)
                {
                    prcDisplayDetails(clsMain.strRelationalId);
                    clsMain.strRelationalId = "0";
                }
                tabs.Tabs["std"].Visible = false;
                tabs.Tabs["tech"].Visible = false;

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

            cboEmpType.DataSource = null;
            cboEmpType.DataSource = dsList.Tables["tblemptype"];

            cboBlood.DataSource = null;
            cboBlood.DataSource = dsList.Tables["tblBlood"];

            cboMarit.DataSource = null;
            cboMarit.DataSource = dsList.Tables["tblMaritialsts"];

            cboRelegion.DataSource = null;
            cboRelegion.DataSource = dsList.Tables["tblRelegion"];

            cboCaste.DataSource = null;
            cboCaste.DataSource = dsList.Tables["tblCaste"];

            cboSex.DataSource = null;
            cboSex.DataSource = dsList.Tables["tblSex"];

            cboNationality.DataSource = null;
            cboNationality.DataSource = dsList.Tables["tblNationality"];

            // Tab Office Information
            cboDesig.DataSource = null;
            cboDesig.DataSource = dsList.Tables["TblDesignation"];

            cboSec.DataSource = null;
            cboSec.DataSource = dsList.Tables["tblsection"];

            cboGrade.DataSource = null;
            cboGrade.DataSource = dsList.Tables["tblGrade"];

            cboLine.DataSource = null;
            cboLine.DataSource = dsList.Tables["tblLine"];

            cboFloor.DataSource = null;
            cboFloor.DataSource = dsList.Tables["tblFloor"];

            cboShift.DataSource = null;
            cboShift.DataSource = dsList.Tables["tblShift"];

            cboPaysource.DataSource = null;
            cboPaysource.DataSource = dsList.Tables["tblPaysource"];

            cboPayMode.DataSource = null;
            cboPayMode.DataSource = dsList.Tables["tblPayMode"];

            cboBank.DataSource = null;
            cboBank.DataSource = dsList.Tables["tblBankName"];

            cboCurrDist.DataSource = null;
            cboCurrDist.DataSource = dsList.Tables["tblDistrict"];

            cboPreDist.DataSource = null;
            cboPreDist.DataSource = dsList.Tables["tblDistrict"];

            cboAdmissnForm.DataSource = null;
            cboAdmissnForm.DataSource = dsList.Tables["admissnForm"];

            cboExam.DataSource = null;
            cboExam.DataSource = dsList.Tables["template"];
        }

        private Boolean fncBlank()
        {
            if (this.cboCode.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee Code");
                cboCode.Focus();
                return true;
            }
            if (this.cboEmpType.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee type");
                cboEmpType.Focus();
                return true;
            }
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee name");
                txtName.Focus();
                return true;
            }
            if (this.cboDesig.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee designation");
                cboDesig.Focus();
                return true;
            }
            if (this.cboSec.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee section");
                cboSec.Focus();
                return true;
            }
            if (this.cboSec.IsItemInList() == false)
            {
                MessageBox.Show("Please Provide valid data [or, select from list].");
                cboSec.Focus();
                return true;
            }
            if (this.cboPaysource.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee pay source");
                cboPaysource.Focus();
                return true;
            }
            if (this.cboPayMode.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee shift");
                cboPayMode.Focus();
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
                string sqlQuery = "Exec prcGetEmployee " + Common.Classes.clsMain.intComId + " , " +
                                  Int32.Parse(strParam) + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";
                dsDetails.Tables[1].TableName = "Education";
                dsDetails.Tables[2].TableName = "Experiance";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    // Tab Basic Information\
                    this.txtEmpID.Text = dr["empid"].ToString();
                    //this.cboCode.Value = dr["empCode"].ToString();
                    this.cboEmpType.Text = dr["EmpType"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtBName.Text = dr["EmpNameB"].ToString();
                    this.cboDesig.Text = dr["DesigID"].ToString();
                    this.cboSec.Text = dr["SectId"].ToString();
                    this.cboGrade.Text = dr["Grade"].ToString();
                    this.cboShift.Value = dr["shiftid"].ToString();
                    this.cboFloor.Text = dr["Floor"].ToString();
                    this.cboLine.Text = dr["Line"].ToString();
                    this.dtJDate.Text = dr["dtJoin"].ToString();
                    this.dtPFDate.Text = dr["dtPF"].ToString();
                    this.dtConfirm.Text = dr["dtConfirm"].ToString();
                    this.dtProvision.Text = dr["dtProvisionEnd"].ToString();

                    this.cboPaysource.Text = dr["PaySource"].ToString();
                    this.cboPayMode.Text = dr["PayMode"].ToString();
                    this.txtGS.Text = dr["GS"].ToString();
                    this.txtBS.Text = dr["BS"].ToString();
                    this.txtAccNo.Text = dr["BankAcNo"].ToString();
                    this.cboBank.Text = dr["BankId"].ToString();
                    //Tab Office Information
                    this.txtFather.Text = dr["EmpFather"].ToString();
                    this.txtFatherBName.Text = dr["EmpFatherB"].ToString();
                    this.txtMohter.Text = dr["EmpMother"].ToString();
                    this.txtMotherBName.Text = dr["EmpMotherB"].ToString();
                    this.txtSpouse.Text = dr["EmpSpouse"].ToString();
                    this.txtSpouseB.Text = dr["EmpSpouseB"].ToString();
                    this.cboNationality.Text = dr["Nationality"].ToString();
                    this.txtNationalID.Text = dr["VoterNo"].ToString();

                    this.dtBirth.Text = dr["dtBirth"].ToString();
                    this.cboBlood.Text = dr["BloodGroup"].ToString();
                    this.cboRelegion.Text = dr["Religion"].ToString();
                    this.cboCaste.Text = dr["Caste"].ToString();
                    this.cboSex.Text = dr["Sex"].ToString();

                    this.cboMarit.Text = dr["MaritalSts"].ToString();
                    this.txtMobile.Text = dr["EmpMobile"].ToString();
                    this.txtMail.Text = dr["EmpEmail"].ToString();

                    this.txtPassport.Text = dr["PassportNo"].ToString();
                    this.txtCurrAdd.Text = dr["EmpCurrAdd"].ToString();
                    this.cboCurrPost.Text = dr["EmpCurrPO"].ToString();
                    this.cboCurrPS.Text = dr["EmpCurrPS"].ToString();
                    this.cboCurrDist.Text = dr["DistId"].ToString();
                    this.cboCurrCity.Text = dr["EmpCurrCity"].ToString();
                    this.txtPreadd.Text = dr["EmpPerAdd"].ToString();
                    this.cboPrePost.Text = dr["EmpPerPO"].ToString();
                    this.cboPrePS.Text = dr["EmpPerPS"].ToString();
                    this.cboPreDist.Text = dr["DistId"].ToString();
                    this.cboPreCity.Text = dr["EmpPerCity"].ToString();
                    if (dr["EmpPicLocation"].ToString().Length > 0)
                    {
                        picPreview.Image = new Bitmap(EmpPic + "\\" + dr["EmpPicLocation"].ToString());
                    }
                }
                gridEdu.DataSource = null;
                gridEdu.DataSource = dsDetails.Tables["Education"];

                gridExp.DataSource = null;
                gridExp.DataSource = dsDetails.Tables["Experiance"];

                this.btnSave.Text = "&Update";
                this.btnDelete.Enabled = true;
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
            cboEmpType.Value = "";
            txtName.Text = "";
            txtBName.Text = "";
            cboDesig.Value = "";
            cboSec.Value = "";
            dtJDate.Value = "";
            txtGS.Text = "";
            cboGrade.Value = "";
            cboLine.Value = "";
            cboSex.Value = "";
            cboFloor.Value = "";
            cboPaysource.Value = "";
            cboPayMode.Value = "";
            dtPFDate.Value = "";
            dtProvision.Value = DateTime.Today.ToString();
            cboBank.Value = "";

            txtFather.Text = "";
            txtFatherBName.Text = "";
            txtMohter.Text = "";
            txtMotherBName.Text = "";
            txtSpouse.Text = "";
            txtSpouseB.Text = "";
            cboNationality.Value = "";
            txtNationalID.Text = "";
            cboRelegion.Value = "";
            cboCaste.Value = "";
            cboSex.Value = "";
            cboMarit.Value = "";
            txtMobile.Text = "";
            txtMail.Text = "";
            txtPassport.Text = "";
            txtCurrAdd.Text = "";
            cboCurrPost.Text = "";
            cboCurrPS.Text = "";
            cboCurrCity.Text = "";
            cboCurrDist.Text = "";
            txtPreadd.Text = "";
            cboPrePost.Text = "";
            cboPrePS.Text = "";
            cboPreCity.Text = "";
            cboPreDist.Text = "";

            txtAccNo.Text = "";
            //cboShift.Value = 
            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int64 NewId = 0;
            try
            {
                //Member Master Table
                if (btnSave.Text != "&Save")
                {
                    sqlQuery = " Delete From tblEmpExam_Data  Where empId = " + Int32.Parse(txtEmpID.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Delete From tblEmp_Edu  Where empId = " + Int32.Parse(txtEmpID.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Delete From tblEmp_Exp  Where empId = " + Int32.Parse(txtEmpID.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    if (dtPFDate.Value == null)
                    {
                        dtPFDate.Value = "1/1/1900";
                    }
                    if (dtProvision.Value == null)
                    {
                        dtProvision.Value = "1/1/1900";
                    }
                    if (dtBirth.Value == null)
                    {
                        dtBirth.Value = "1/1/1900";
                    }
                    //Update
                    sqlQuery = "Update tblEmp_Info set EmpCode = '" + this.cboCode.Text.ToString() + "', empType = '" +
                               this.cboEmpType.Value.ToString() + "', empName ='" +
                               this.txtName.Text.ToString() + "', empNameB = '" + this.txtBName.Text.ToString() +
                               "', DesigID = '" + this.cboDesig.Value.ToString() + "', SectId = '" + this.cboSec.Value.ToString() + "',  Grade = '" + this.cboGrade.Value.ToString() +
                               "', ShiftType='" + this.cboShift.Value.ToString() + "', Floor = '" + this.cboFloor.Value.ToString() + "', Line = '" + this.cboLine.Value.ToString() +
                               "',  dtjoin= '" + clsProc.GTRDate(this.dtJDate.Value.ToString()) + "', dtPF = '" + clsProc.GTRDate(this.dtPFDate.Value.ToString()) + "',dtConfirm='" +
                               clsProc.GTRDate(this.dtConfirm.Value.ToString()) + "', dtProvisionEnd ='" + clsProc.GTRDate(this.dtProvision.Value.ToString()) + "',  PaySource = '" +
                               this.cboPaysource.Value.ToString() + "', PayMode = '" + this.cboPayMode.Value.ToString() +
                               "', GS='" + this.txtGS.Text.ToString() + "', BS = '" + this.txtBS.Text.ToString() + "',  BankAcNo = '" + this.txtAccNo.Text.ToString() +
                               "', BankId= '" + this.cboBank.Value.ToString() + "', IsAllowOT = '" + this.checkOT.Tag.ToString() + "', IsAllowPF = '" + this.checkPF.Tag.ToString() +
                               "', IsInactive = '" + this.checkYes.Tag.ToString() + "', EmpFather = '" +this.txtFather.Text.ToString() + "',  EmpFatherB= '" +this.txtFatherBName.Text.ToString() + "',EmpMother= '" +
                               this.txtMohter.Text.ToString() + "', EmpMotherB ='" + this.txtMotherBName.Text.ToString() +
                               "', EmpSpouse = '" + this.txtSpouse.Text.ToString() + "', EmpSpouseB = '" +this.txtSpouseB.Text.ToString() + "',  Nationality = '" +
                               this.cboNationality.Text.ToString() + "', VoterNo = '" +this.txtNationalID.Text.ToString() + "', dtBirth= '" +
                               clsProc.GTRDate(this.dtBirth.Value.ToString()) + "', BloodGroup = '" + this.cboBlood.Text.ToString() + "', Religion = '" + this.cboRelegion.Text.ToString() +
                               "', Caste = '" +this.cboCaste.Text.ToString() + "', Sex = '" + this.cboSex.Text.ToString() +
                               "', MaritalSts = '" + this.cboMarit.Text.ToString() + "',EmpMobile = '" +this.txtMobile.Text.ToString() + "',  EmpEmail = '" + this.txtMail.Text.ToString() +
                               "', PassportNo = '" + this.txtPassport.Text.ToString() + "',  EmpCurrAdd= '" + this.txtCurrAdd.Text.ToString() + "', EmpCurrPO ='" + this.cboCurrPost.Text.ToString() +
                               "',  EmpCurrPS ='" + this.cboCurrPS.Text.ToString() + "', EmpCurrDistId ='" +this.cboCurrDist.Value.ToString() + "', EmpCurrCity='" +
                               this.cboCurrCity.Text.ToString() + "', EmpPerAdd ='" + this.txtPreadd.Text.ToString() +
                               "', EmpPerPO ='" + this.cboPrePost.Text.ToString() + "',EmpPerPS= '" + this.cboPrePS.Text.ToString() + "',EmpPerCity ='" + this.cboPreCity.Text.ToString() +
                               "', EmpPerDistId = '" + this.cboPreDist.Value.ToString() + "',   WorkPlace = '" + this.cboFloor.Text.ToString() + "',EmpPicLocation='" + txtImageName.Text.ToString() +
                               "'  where  EmpId = '" + this.txtEmpID.Text.ToString() + "' ";

                    arQuery.Add(sqlQuery);

                    fncGetExmaData(ref arQuery, txtEmpID.Text);
                    fncGetGridEdu(ref arQuery, txtEmpID.Text);
                    fncGetgridExp(ref arQuery, txtEmpID.Text);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //NewId
                    sqlQuery = "Select Isnull(Max(EmpId),0)+1 As NewId from tblEmp_Info";
                    NewId = clsCon.GTRCountingDataLarge(sqlQuery);

                    //Insert Data
                    sqlQuery =
                        "Insert Into tblEmp_Info (admId,aEmpID, EmpId, comId,EmpCode, EmpName, EmpNameB,EmpType, EmpFather, EmpFatherB, EmpMother, EmpMotherB, EmpSpouse, EmpSpouseB, EmpCurrAdd" +
                        ",EmpCurrCity,EmpCurrPO,EmpCurrPS, EmpCurrDistId, EmpPerAdd,EmpPerPO, EmpPerPS, EmpPerCity, EmpPerDistId,EmpMobile,EmpPhone, EmpEmail" +
                        ", Sex, Religion, Caste, BloodGroup, MaritalSts, dtBirth, dtJoin, dtConfirm , dtProvisionEnd" +
                        ",SectId, SectIdSal, DesigID, Grade, Floor, Line, GS, BS, IsAllowOT, ShiftType, WorkPlace, Nationality, PassportNo,IsAllowPF, dtPF,PaySource, PayMode" +
                        ", BankId, BankAcNo, IsInactive, aId,EmpPicLocation)"
                        + " Values ("+cboAdmissnForm.Value.ToString()+", "+ NewId + ", " + NewId + ",'" + Common.Classes.clsMain.intComId + "','" +this.cboAdmissnForm.Text.ToString() + "',' " +
                        this.txtName.Text + "',' " + this.txtBName.Text.ToString() + "','" + this.cboEmpType.Value.ToString() + "',' " + this.txtFather.Text.ToString() + "','" +
                        this.txtFatherBName.Text.ToString() + "','" + this.txtMohter.Text.ToString() + "','" + this.txtMotherBName.Text.ToString() + "','" + this.txtSpouse.Text.ToString() + "','" +
                        this.txtSpouseB.Text.ToString() + "',' " + this.txtCurrAdd.Text.ToString() + "','" + this.cboCurrCity.Text.ToString() + "', '" +
                        this.cboCurrPost.Text.ToString() + "', '" + this.cboCurrPS.Text.ToString() + "', '" + this.cboCurrDist.Value.ToString() + "','" + this.txtPreadd.Text.ToString() + "', '" +
                        this.cboPrePost.Text.ToString() + "','" + this.cboPrePS.Text.ToString() + "', '" + this.cboPreCity.Text.ToString() + "', '" + this.cboPreDist.Value.ToString() + "','" +
                        this.txtMobile.Text.ToString() + "','" + this.txtPhone.Text.ToString() + "','" + this.txtMail.Text.ToString() + "','" + cboSex.Text +
                        "','" + cboRelegion.Text + "','" + this.cboCaste.Text.ToString() + "','" + this.cboBlood.Text.ToString() + "', '" + this.cboMarit.Text.ToString() + "', '" +
                        this.dtBirth.Value.ToString() + "','" + this.dtJDate.Value.ToString() + "','" + this.dtConfirm.Value.ToString() + "', '" +
                        this.dtProvision.Value.ToString() + "','" + this.cboSec.Value.ToString() + "', '" + this.cboSec.Value.ToString() + "', '" +
                        this.cboDesig.Value.ToString() + "', '" + this.cboGrade.Value.ToString() + "', '" + this.cboFloor.Value.ToString() + "', '" + this.cboLine.Value.ToString() + "', '" +
                        this.txtGS.Text.ToString() + "', '" + this.txtBS.Text.ToString() + "', '" + this.checkOT.Tag.ToString() + "', '" + this.cboShift.Value.ToString() + "', '" +
                        this.cboLine.Value.ToString() + "', '" + this.cboNationality.Text.ToString() + "', '" + this.txtPassport.Text.ToString() + "', '" + this.checkPF.Tag.ToString() + "', '" +
                        this.dtPFDate.Value.ToString() + "','" + this.cboPaysource.Value.ToString() + "', '" + this.cboPayMode.Value.ToString() + "', '" + this.cboBank.Value.ToString() + "', '" + this.txtAccNo.Text.ToString() + "', '" +
                        this.checkYes.Tag.ToString() + "'," + NewId + ",'" + txtImageName.Text.ToString() + "')";
                    arQuery.Add(sqlQuery);

                    fncGetExmaData(ref arQuery, NewId.ToString());
                    fncGetGridEdu(ref arQuery, NewId.ToString());
                    fncGetgridExp(ref arQuery, NewId.ToString());

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);
                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                }
                if (txtImageName.Text.Length != 0)
                {
                    if (txtImageName.Tag != null) //If New Image then it will be copy else no need to copy
                    {
                        string strTarget = EmpPic + @"\" + txtImageName.Text;
                        File.Copy(txtImageName.Tag.ToString(), strTarget, true);
                        //  File.Delete(strTarget);
                        //string str = clsMain.strPicPathStore;
                    }
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
                //arQuery = null;
                //clsCon = null;
            }
        }

        private void frmStudent_Load(object sender, EventArgs e)
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to Delete Employee information of ['" + cboCode.Text.ToString() + "']", "",
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
                sqlQuery = "Delete from tblEmp_Info Where EmpID = " + Int32.Parse(cboCode.Value.ToString()) +
                           " and comid = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','Delete')";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }
        private void frmStudent_FormClosing(object sender, FormClosingEventArgs e)
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
            cboCode.DisplayLayout.Bands[0].Columns["EmpNameB"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["EmpType"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Grade"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["ShiftType"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Floor"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Line"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["BloodGroup"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["MaritalSts"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Religion"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Caste"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Sex"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Nationality"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["dtJoin"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["dtPF"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["dtConfirm"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["dtProvisionEnd"].Hidden = true;
            
            cboCode.DisplayLayout.Bands[0].Columns["PaySource"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["PayMode"].Hidden = true;
            
            cboCode.DisplayLayout.Bands[0].Columns["GS"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            
            cboCode.DisplayLayout.Bands[0].Columns["empName"].Width = 120;
            cboCode.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Name";
            
            cboCode.DisplayMember = "empCode";
            cboCode.ValueMember = "empId";
        }

        private void cboBlood_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBlood.DisplayLayout.Bands[0].Columns["BloodGroup"].Width = cboBlood.Width;
            cboBlood.DisplayLayout.Bands[0].Columns["BloodGroup"].Header.Caption = "Blood Group";
        }
        private void cboMarit_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboMarit.DisplayLayout.Bands[0].Columns["MaritalSts"].Width = cboMarit.Width;
            cboMarit.DisplayLayout.Bands[0].Columns["MaritalSts"].Header.Caption = "Marital Status";
        }
        private void cboRelegion_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboRelegion.DisplayLayout.Bands[0].Columns["Religion"].Width = cboRelegion.Width;
            cboRelegion.DisplayLayout.Bands[0].Columns["Religion"].Header.Caption = "Religion";
        }
        private void cboEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpType.DisplayLayout.Bands[0].Columns["emptype"].Width = cboEmpType.Width;
            cboEmpType.DisplayLayout.Bands[0].Columns["emptype"].Header.Caption = "Employee Type";
        }
        private void cboCaste_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCaste.DisplayLayout.Bands[0].Columns["Caste"].Width = cboCaste.Width;
            cboCaste.DisplayLayout.Bands[0].Columns["Caste"].Header.Caption = "Caste";
        }
        private void cboSex_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSex.DisplayLayout.Bands[0].Columns["Sex"].Width = cboSex.Width;
            cboSex.DisplayLayout.Bands[0].Columns["Sex"].Header.Caption = "Gender";
        }
        private void cboNationality_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboNationality.DisplayLayout.Bands[0].Columns["countryId"].Hidden = true;
            cboNationality.DisplayLayout.Bands[0].Columns["nationality"].Width = cboNationality.Width;
            cboNationality.DisplayLayout.Bands[0].Columns["nationality"].Header.Caption = "Nationality";
            cboNationality.DisplayMember = "nationality";
            cboNationality.ValueMember = "countryId";
        }
        // Tab Office Information
        private void cboDesig_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDesig.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;
            cboDesig.DisplayLayout.Bands[0].Columns["DesigName"].Width = cboDesig.Width;
            cboDesig.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
            cboDesig.DisplayMember = "DesigName";
            cboDesig.ValueMember = "DesigId";
        }

        private void cboSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSec.Width;
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            cboSec.DisplayMember = "SectName";
            cboSec.ValueMember = "SectId";
        }
        private void cboGrade_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Width = cboGrade.Width;
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
        }
        private void cboLine_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboLine.DisplayLayout.Bands[0].Columns["Line"].Width = cboLine.Width;
            cboLine.DisplayLayout.Bands[0].Columns["Line"].Header.Caption = "Line";
        }
        private void cboFloor_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFloor.DisplayLayout.Bands[0].Columns["floor"].Width = cboFloor.Width;
            cboFloor.DisplayLayout.Bands[0].Columns["floor"].Header.Caption = "Floor";
        }
        private void cboShift_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboShift.DisplayLayout.Bands[0].Columns["ShiftType"].Width = cboShift.Width;
            cboShift.DisplayLayout.Bands[0].Columns["shiftid"].Hidden  = true;
            cboShift.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Shift Type";
            cboShift.ValueMember= "shiftid";
            cboShift.DisplayMember = "ShiftType";
        }

        private void cboPaysource_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPaysource.DisplayLayout.Bands[0].Columns["paysource"].Width = cboPaysource.Width;
            cboPaysource.DisplayLayout.Bands[0].Columns["paysource"].Header.Caption = "Pay Source";
            //cboPaysource.DisplayMember = "paysource";
            //cboPaysource.ValueMember = "paysource";
        }
        private void cboPayMode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPayMode.DisplayLayout.Bands[0].Columns["Paymode"].Width = cboPayMode.Width;
            cboPayMode.DisplayLayout.Bands[0].Columns["Paymode"].Header.Caption = "Pay Mode";
            //cboPayMode.DisplayMember = "Paymode";
            //cboPayMode.ValueMember = "Paymode";
        }

        private void fncGetExmaData(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";
            foreach (UltraGridRow row in this.gridExam.Rows)
            {
                sqlQuery =
                    "Insert Into tblEmpExam_Data (empId, OSubId, OTmarks, OGetmarks, TSubId, TTmarks, TGetmarks, ThSubId, ThTmarks, ThGetmarks, FSubId, FTmarks, FGetMarks,aId)" +
                    " Values (" + Int32.Parse(strId) + ", '" + row.Cells["OSubId"].Value.ToString() + "','" + row.Cells["OTmarks"].Text.ToString()
                    + "','" + row.Cells["OGetmarks"].Text.ToString() + "','" + row.Cells["TSubId"].Text.ToString() + "','" + row.Cells["TTmarks"].Text.ToString() +
                    "', '" + row.Cells["TGetmarks"].Text.ToString() + "','" + row.Cells["ThSubId"].Text.ToString() + "','" + row.Cells["ThTmarks"].Text.ToString()
                    + "','" + row.Cells["ThGetmarks"].Text.ToString() + "', '" + row.Cells["FSubId"].Text.ToString() + "', '" + row.Cells["FTmarks"].Text.ToString() + "','" +
                    row.Cells["FGetMarks"].Text.ToString() + "'," + Int32.Parse(strId) + ")";
                arQuery.Add(sqlQuery);
                //                   fncGetBarcodeData(ref arQuery,strId, row.Cells["Barcode"].Text.ToString());
            }
        }
        private void fncGetGridEdu(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";
            foreach (UltraGridRow row in this.gridEdu.Rows)
            {
                sqlQuery =
                    "Insert Into tblEmp_Edu (empId,ExamName,ExamResult,MajorSub,BoardName,InstituteName,PassingYear,aId)" +
                    " Values (" + Int32.Parse(strId) + ", '" + row.Cells["examName"].Value.ToString() + "','" +
                    row.Cells["examresult"].Text.ToString()
                    + "','" + row.Cells["majorSub"].Text.ToString() + "','" + row.Cells["BoardName"].Text.ToString() +
                    "', '" + row.Cells["Institutename"].Text.ToString() + "','" +
                    row.Cells["passingyear"].Text.ToString()
                    + "'," + Int32.Parse(strId) + ")";
                arQuery.Add(sqlQuery);
                //                   fncGetBarcodeData(ref arQuery,strId, row.Cells["Barcode"].Text.ToString());
            }
        }
        private void fncGetgridExp(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";
            foreach (UltraGridRow row in this.gridExp.Rows)
            {
                sqlQuery = "Insert Into tblEmp_Exp (EmpId,PrevCom,PrevDesig,PrevSalary,Remarks,dtFrom,dtTo,aId)" +
                           " Values (" + Int32.Parse(strId) + ", '" + row.Cells["PrevCom"].Value.ToString() + "','" +
                           row.Cells["DesigNamePrev"].Text.ToString()
                           + "','" + row.Cells["SalaryPrev"].Text.ToString() + "','" +
                           row.Cells["Remarks"].Text.ToString() + "','" + row.Cells["dtFrom"].Text.ToString() + "','" +
                           row.Cells["dtTo"].Text.ToString()
                           + "'," + Int32.Parse(strId) + ")";
                arQuery.Add(sqlQuery);
                //                   fncGetBarcodeData(ref arQuery,strId, row.Cells["Barcode"].Text.ToString());
            }
        }
        private void cboBank_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBank.DisplayLayout.Bands[0].Columns["BankId"].Hidden = true;
            cboBank.DisplayLayout.Bands[0].Columns["BankName"].Width = cboBank.Width;
            cboBank.DisplayLayout.Bands[0].Columns["BankName"].Header.Caption = "Bank Name";
            cboBank.DisplayMember = "BankName";
            cboBank.ValueMember = "BankId";
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
       private void cboCurrDist_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCurrDist.DisplayLayout.Bands[0].Columns["DistId"].Hidden = true;
            cboCurrDist.DisplayLayout.Bands[0].Columns["DistName"].Width = cboCurrDist.Width;
            cboCurrDist.DisplayLayout.Bands[0].Columns["DistName"].Header.Caption = "District";
            cboCurrDist.DisplayMember = "DistName";
            cboCurrDist.ValueMember = "DistId";
        }

        private void cboPreDist_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPreDist.DisplayLayout.Bands[0].Columns["DistId"].Hidden = true;
            cboPreDist.DisplayLayout.Bands[0].Columns["DistName"].Width = cboPreDist.Width;
            cboPreDist.DisplayLayout.Bands[0].Columns["DistName"].Header.Caption = "District";
            cboPreDist.DisplayMember = "DistName";
            cboPreDist.ValueMember = "DistId";
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            txtName.Text = txtName.Text.TrimStart();
        }

        private void txtGS_Leave(object sender, EventArgs e)
        {
            txtGS.Text = txtGS.Text.TrimStart();
        }
        private void txtBName_Leave(object sender, EventArgs e)
        {
            txtBName.Text = txtBName.Text.TrimStart();
        }
        private void txtBS_Leave(object sender, EventArgs e)
        {
            txtBS.Text = txtBS.Text.TrimStart();
        }
        private void txtAccNo_Leave(object sender, EventArgs e)
        {
            txtAccNo.Text = txtAccNo.Text.TrimStart();
        }
        private void txtFather_Leave(object sender, EventArgs e)
        {
            txtFather.Text = txtFather.Text.TrimStart();
        }
        private void txtMohter_Leave(object sender, EventArgs e)
        {
            txtMohter.Text = txtMohter.Text.TrimStart();
        }
        private void txtSpouse_Leave(object sender, EventArgs e)
        {
            txtSpouse.Text = txtSpouse.Text.TrimStart();
        }
        private void txtMobile_Leave(object sender, EventArgs e)
        {
            txtMobile.Text = txtMobile.Text.TrimStart();
        }
        private void txtPassport_Leave(object sender, EventArgs e)
        {
            txtPassport.Text = txtPassport.Text.TrimStart();
        }
        private void txtCurrAdd_Leave(object sender, EventArgs e)
        {
            txtCurrAdd.Text = txtCurrAdd.Text.TrimStart();
        }

        private void txtFatherBName_Leave(object sender, EventArgs e)
        {
            txtFatherBName.Text = txtFatherBName.Text.TrimStart();
        }
        private void txtMotherBName_Leave(object sender, EventArgs e)
        {
            txtMotherBName.Text = txtMotherBName.Text.TrimStart();
        }
        private void txtSpouseB_Leave(object sender, EventArgs e)
        {
            txtSpouseB.Text = txtSpouseB.Text.TrimStart();
        }
        private void txtNationalID_Leave(object sender, EventArgs e)
        {
            txtNationalID.Text = txtNationalID.Text.TrimStart();
        }
        private void txtMail_Leave(object sender, EventArgs e)
        {
            txtMail.Text = txtMail.Text.TrimStart();
        }
        private void txtPreadd_Leave(object sender, EventArgs e)
        {
            txtPreadd.Text = txtPreadd.Text.TrimStart();
        }
        private void txtFather_ValueChanged(object sender, EventArgs e)
        {
            txtFather.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFather.Text);
            //txtName.Focus();
            txtFather.SelectionStart = txtFather.Text.Length;
        }

        private void txtName_ValueChanged(object sender, EventArgs e)
        {
            txtName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtName.Text);
            //txtName.Focus();
            txtName.SelectionStart = txtName.Text.Length;
        }

        private void txtMohter_ValueChanged(object sender, EventArgs e)
        {
            txtMohter.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtMohter.Text);
            //txtName.Focus();
            txtMohter.SelectionStart = txtMohter.Text.Length;
        }
        private void txtSpouse_ValueChanged(object sender, EventArgs e)
        {
            txtSpouse.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtSpouse.Text);
            //txtName.Focus();
            txtSpouse.SelectionStart = txtSpouse.Text.Length;
        }

        private void txtCurrAdd_ValueChanged(object sender, EventArgs e)
        {
            txtCurrAdd.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtCurrAdd.Text);
            //txtName.Focus();
            txtCurrAdd.SelectionStart = txtCurrAdd.Text.Length;
        }

        private void txtPreadd_ValueChanged(object sender, EventArgs e)
        {
            txtPreadd.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtPreadd.Text);
            //txtName.Focus();
            txtPreadd.SelectionStart = txtPreadd.Text.Length;
        }

        private void cboCode_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboCode.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["EmpNameB"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["EmpType"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Grade"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["ShiftType"].Hidden = true;
            //cboCode.DisplayLayout.Bands[0].Columns["ShiftCat"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["Floor"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Line"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["BloodGroup"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["MaritalSts"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Religion"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Caste"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Sex"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["Nationality"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["dtJoin"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["dtPF"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["dtConfirm"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["dtProvisionEnd"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["PaySource"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["PayMode"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["GS"].Hidden = true;
            //cboCode.DisplayLayout.Bands[0].Columns["dtProvisionEnd"].Hidden = true;
            //cboCode.DisplayLayout.Bands[0].Columns["PaySource"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";

            cboCode.DisplayLayout.Bands[0].Columns["empName"].Width = 120;
            cboCode.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Name";
            cboCode.DisplayMember = "empCode";
            cboCode.ValueMember = "empId";
        }

        private void cboEmpType_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpType.DisplayLayout.Bands[0].Columns["emptype"].Width = cboEmpType.Width;
            cboEmpType.DisplayLayout.Bands[0].Columns["emptype"].Header.Caption = "Employee Type";
        }
        private void cboSec_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSec.Width;
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            cboSec.DisplayMember = "SectName";
            cboSec.ValueMember = "SectId";
        }
        private void cboDesig_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboDesig.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;
            cboDesig.DisplayLayout.Bands[0].Columns["DesigName"].Width = cboDesig.Width;
            cboDesig.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
            cboDesig.DisplayMember = "DesigName";
            cboDesig.ValueMember = "DesigId";
        }

        private void cboGrade_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Width = cboGrade.Width;
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
        }

        private void cboShift_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboShift.DisplayLayout.Bands[0].Columns["ShiftType"].Width = cboShift.Width;
            cboShift.DisplayLayout.Bands[0].Columns["shiftid"].Hidden = true;
            cboShift.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Shift Type";
            cboShift.ValueMember = "shiftid";
            cboShift.DisplayMember = "ShiftType";
        }

        private void cboFloor_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboFloor.DisplayLayout.Bands[0].Columns["floor"].Width = cboFloor.Width;
            cboFloor.DisplayLayout.Bands[0].Columns["floor"].Header.Caption = "Floor";
        }

        private void cboLine_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboLine.DisplayLayout.Bands[0].Columns["Line"].Width = cboLine.Width;
            cboLine.DisplayLayout.Bands[0].Columns["Line"].Header.Caption = "Line";
        }

        private void cboPaysource_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboPaysource.DisplayLayout.Bands[0].Columns["paysource"].Width = cboPaysource.Width;
            cboPaysource.DisplayLayout.Bands[0].Columns["paysource"].Header.Caption = "Pay Source";
        }

        private void cboPayMode_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboPayMode.DisplayLayout.Bands[0].Columns["Paymode"].Width = cboPayMode.Width;
            cboPayMode.DisplayLayout.Bands[0].Columns["Paymode"].Header.Caption = "Pay Mode";
        }

        private void cboBank_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboBank.DisplayLayout.Bands[0].Columns["BankId"].Hidden = true;
            cboBank.DisplayLayout.Bands[0].Columns["BankName"].Width = cboBank.Width;
            cboBank.DisplayLayout.Bands[0].Columns["BankName"].Header.Caption = "Bank Name";
            cboBank.DisplayMember = "BankName";
            cboBank.ValueMember = "BankId";
        }

        private void cboNationality_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboNationality.DisplayLayout.Bands[0].Columns["countryId"].Hidden = true;
            cboNationality.DisplayLayout.Bands[0].Columns["nationality"].Width = cboNationality.Width;
            cboNationality.DisplayLayout.Bands[0].Columns["nationality"].Header.Caption = "Nationality";
            cboNationality.DisplayMember = "nationality";
            cboNationality.ValueMember = "countryId";
        }

        private void cboBlood_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboBlood.DisplayLayout.Bands[0].Columns["BloodGroup"].Width = cboBlood.Width;
            cboBlood.DisplayLayout.Bands[0].Columns["BloodGroup"].Header.Caption = "Blood Group";
        }

        private void cboRelegion_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboRelegion.DisplayLayout.Bands[0].Columns["Religion"].Width = cboRelegion.Width;
            cboRelegion.DisplayLayout.Bands[0].Columns["Religion"].Header.Caption = "Religion";
        }

        private void cboMarit_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboMarit.DisplayLayout.Bands[0].Columns["MaritalSts"].Width = cboMarit.Width;
            cboMarit.DisplayLayout.Bands[0].Columns["MaritalSts"].Header.Caption = "Marital Status";
        }

        private void cboCaste_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboCaste.DisplayLayout.Bands[0].Columns["Caste"].Width = cboCaste.Width;
            cboCaste.DisplayLayout.Bands[0].Columns["Caste"].Header.Caption = "Caste";
        }

        private void cboCurrDist_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboCurrDist.DisplayLayout.Bands[0].Columns["DistId"].Hidden = true;
            cboCurrDist.DisplayLayout.Bands[0].Columns["DistName"].Width = cboCurrDist.Width;
            cboCurrDist.DisplayLayout.Bands[0].Columns["DistName"].Header.Caption = "District";
            cboCurrDist.DisplayMember = "DistName";
            cboCurrDist.ValueMember = "DistId";
        }

        private void cboPreDist_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            cboPreDist.DisplayLayout.Bands[0].Columns["DistId"].Hidden = true;
            cboPreDist.DisplayLayout.Bands[0].Columns["DistName"].Width = cboPreDist.Width;
            cboPreDist.DisplayLayout.Bands[0].Columns["DistName"].Header.Caption = "District";
            cboPreDist.DisplayMember = "DistName";
            cboPreDist.ValueMember = "DistId";
        }

        private void gridEdu_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridEdu.DisplayLayout.Bands[0].Columns["examName"].Header.Caption = "Name of Degree";
                gridEdu.DisplayLayout.Bands[0].Columns["examresult"].Header.Caption = "Result";
                gridEdu.DisplayLayout.Bands[0].Columns["majorSub"].Header.Caption = "Major";
                gridEdu.DisplayLayout.Bands[0].Columns["BoardName"].Header.Caption = "Board Name";
                gridEdu.DisplayLayout.Bands[0].Columns["Institutename"].Header.Caption = "Institutename";
                gridEdu.DisplayLayout.Bands[0].Columns["passingyear"].Header.Caption = "Passing Year";

                gridEdu.DisplayLayout.Bands[0].Columns["examName"].Width = 170;
                gridEdu.DisplayLayout.Bands[0].Columns["examresult"].Width = 100;
                gridEdu.DisplayLayout.Bands[0].Columns["majorSub"].Width = 140;
                gridEdu.DisplayLayout.Bands[0].Columns["BoardName"].Width = 150;
                gridEdu.DisplayLayout.Bands[0].Columns["Institutename"].Width = 170;
                gridEdu.DisplayLayout.Bands[0].Columns["passingyear"].Width = 100;

                //Change alternate color
                gridEdu.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridEdu.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                //Selection Style Will Be Row Selector
                this.gridEdu.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridEdu.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridEdu.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                //this.gridEdu.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void gridExp_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridExp.DisplayLayout.Bands[0].Columns["PrevCom"].Header.Caption = "Company Name";
                gridExp.DisplayLayout.Bands[0].Columns["PrevDesig"].Header.Caption = "Designation";
                gridExp.DisplayLayout.Bands[0].Columns["PrevSalary"].Header.Caption = "Salary";
                gridExp.DisplayLayout.Bands[0].Columns["dtFrom"].Header.Caption = "Date Form";
                gridExp.DisplayLayout.Bands[0].Columns["dtTo"].Header.Caption = "Date To";
                gridExp.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

                gridExp.DisplayLayout.Bands[0].Columns["PrevCom"].Width = 140;
                gridExp.DisplayLayout.Bands[0].Columns["PrevDesig"].Width = 110;
                gridExp.DisplayLayout.Bands[0].Columns["PrevSalary"].Width = 80;
                gridExp.DisplayLayout.Bands[0].Columns["dtFrom"].Width = 100;
                gridExp.DisplayLayout.Bands[0].Columns["dtTo"].Width = 100;
                gridExp.DisplayLayout.Bands[0].Columns["Remarks"].Width = 120;
                gridExp.DisplayLayout.Bands[0].Columns["dtFrom"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                gridExp.DisplayLayout.Bands[0].Columns["dtTo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                //Change alternate color
                gridExp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridExp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridExp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridExp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridExp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                //    this.gridExp.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnADDEdu_Click_1(object sender, EventArgs e)
        {
            try
            {
                DataRow dr;
                if (btnSave.Text == "&Save")
                {
                    dr = dsList.Tables["tblEducation"].NewRow();
                    dsList.Tables["tblEducation"].Rows.Add(dr);
                }
                else
                {
                    dr = dsDetails.Tables["Education"].NewRow();
                    dsDetails.Tables["Education"].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnADD_Click_1(object sender, EventArgs e)
        {
            try
            {
                DataRow dr;
                if (btnSave.Text == "&Save")
                {
                    dr = dsList.Tables["tblExpiriance"].NewRow();
                    dsList.Tables["tblExpiriance"].Rows.Add(dr);
                }
                else
                {
                    dr = dsDetails.Tables["Experiance"].NewRow();

                    dsDetails.Tables["Experiance"].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog diagOpen = new OpenFileDialog();
                diagOpen.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (diagOpen.ShowDialog() == DialogResult.OK)
                {
                    PicName = diagOpen.FileName.Substring(diagOpen.FileName.LastIndexOf("\\") + 1);
                    txtImageName.Text = cboCode.Text + PicName.Substring(PicName.LastIndexOf(".") + 0);
                    txtImageName.Tag = diagOpen.FileName;
                    picPreview.Image = new Bitmap(diagOpen.FileName);
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
        private void chkStd_CheckedChanged(object sender, EventArgs e)
        {
            prcCheckChange();
        }
        private void chkTech_CheckedChanged(object sender, EventArgs e)
        {
            prcCheckChange();
        }

        private void cboAdmissnForm_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["AdmId"].Hidden = true;
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["frmNo"].Width = 80;
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["Name"].Width = 112;
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["sesn"].Width = 70;
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["SectName"].Width = 100;

            cboAdmissnForm.DisplayLayout.Bands[0].Columns["frmNo"].Header.Caption = "Form No";
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Name";
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
            cboAdmissnForm.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Class";
            cboAdmissnForm.DisplayMember = "frmNo";
            cboAdmissnForm.ValueMember = "AdmId";
            //cboAdmissnForm.Tag = "frmNo";

        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetInfoToEntry] " + Common.Classes.clsMain.intComId + "," + cboAdmissnForm.Value.ToString() + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    // Tab Basic Information\
                    //this.cboCode.Text = dr["frmNo"].ToString();
                    this.txtName.Text = dr["Name"].ToString();
                    this.cboSex.Value = dr["sex"].ToString();
                    this.txtFather.Text = dr["nmFather"].ToString();
                    this.txtMohter.Text = dr["nmMother"].ToString();
                    this.cboSec.Text = dr["sectName"].ToString();
                    this.cboRelegion.Text = dr["Relegion"].ToString();
                    this.txtMobile.Text = dr["Mobile"].ToString();
                    this.txtPhone.Text = dr["Phone"].ToString();
                    this.txtCurrAdd.Text = dr["Address"].ToString();


                }
                else
                {
                    MessageBox.Show("No Data Found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcCheckChange()
        {
            if(chkStd.Checked == true)
            {
                tabs.Tabs["std"].Visible = true;
                tabs.Tabs["tech"].Visible = false;
                chkTech.Checked = false;
            }
            else if (chkTech.Checked == true)
            {
                tabs.Tabs["std"].Visible = false;
                tabs.Tabs["tech"].Visible = true;
                chkStd.Checked = false;
            }
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

        private void btnExmLoad_Click(object sender, EventArgs e)
        {
            dsList = new DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec [prcGetExamMark]" + clsMain.intComId + ", " + cboExam.Value.ToString() + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tbl_Temp";

                gridExam.DataSource = null;
                gridExam.DataSource = dsList.Tables["tbl_Temp"];

                //gridList.DataSource = null;
                //gridList.DataSource = dsList.Tables["gridList"];

                //prcInitializeVoucherGrid();
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

        private void cboExam_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboExam.DisplayLayout.Bands[0].Columns["tmpId"].Hidden = true;
            cboExam.DisplayLayout.Bands[0].Columns["SectName"].Width = cboExam.Width;
            cboExam.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Template Name";

            cboExam.ValueMember = "tmpId";
            cboExam.DisplayMember = "SectName";
        }

        private void gridExam_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
//            	Select Top 0 A.empId,B.SubjectCode,B.subjectName,A.oSubId, A.OTmarks,OGetmarks,
//	C.SubjectCode,C.subjectName,A.tSubId, A.tTmarks,TGetmarks,
//	D.SubjectCode,D.subjectName,A.tHSubId, A.tHTmarks,THGetmarks,
//	E.SubjectCode,E.subjectName,A.FSubId, A.FTmarks,FGetmarks



            gridExam.DisplayLayout.Bands[0].Columns["EMPID"].Hidden = true; // SubjectId 

            gridExam.DisplayLayout.Bands[0].Columns["oSubId"].Hidden = true; // SubjectId  
            gridExam.DisplayLayout.Bands[0].Columns["SubjectCode"].Width = 100; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["SubjectName"].Width = 140; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["OTmarks"].Width = 55; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["OGetmarks"].Width = 55; // SubjectName 

            gridExam.DisplayLayout.Bands[0].Columns["TSubId"].Hidden = true; // SubjectId  
            gridExam.DisplayLayout.Bands[0].Columns["tSubjectCode"].Width = 100; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["tSubjectName"].Width = 140; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["tTmarks"].Width = 65; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["tGetmarks"].Width = 65; // SubjectName 

            gridExam.DisplayLayout.Bands[0].Columns["ThSubId"].Hidden = true; // SubjectId  
            gridExam.DisplayLayout.Bands[0].Columns["thSubjectCode"].Width = 100; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["thSubjectName"].Width = 140; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["thTmarks"].Width = 65; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["thGetmarks"].Width = 65; // SubjectName 


            gridExam.DisplayLayout.Bands[0].Columns["FSubId"].Hidden = true; // SubjectId  
            gridExam.DisplayLayout.Bands[0].Columns["FSubjectCode"].Width = 100; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["FSubjectName"].Width = 140; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["FTmarks"].Width = 65; // SubjectName 
            gridExam.DisplayLayout.Bands[0].Columns["FGetmarks"].Width = 65; // SubjectName 

            //Caption
            gridExam.DisplayLayout.Bands[0].Columns["SubjectCode"].Header.Caption = "Subject Code";
            gridExam.DisplayLayout.Bands[0].Columns["SubjectName"].Header.Caption = "Subject Name";
            gridExam.DisplayLayout.Bands[0].Columns["OTmarks"].Header.Caption = "Total Marks";
            gridExam.DisplayLayout.Bands[0].Columns["OGetmarks"].Header.Caption = "Get Marks";


            gridExam.DisplayLayout.Bands[0].Columns["tSubjectCode"].Header.Caption = "Subject Code";
            gridExam.DisplayLayout.Bands[0].Columns["tSubjectName"].Header.Caption = "Subject Name";
            gridExam.DisplayLayout.Bands[0].Columns["tTmarks"].Header.Caption = "Total Marks";
            gridExam.DisplayLayout.Bands[0].Columns["tGetmarks"].Header.Caption = "Get Marks";


            gridExam.DisplayLayout.Bands[0].Columns["thSubjectCode"].Header.Caption = "Subject Code";
            gridExam.DisplayLayout.Bands[0].Columns["thSubjectName"].Header.Caption = "Subject Name";
            gridExam.DisplayLayout.Bands[0].Columns["thTmarks"].Header.Caption = "Total Marks";
            gridExam.DisplayLayout.Bands[0].Columns["ThGetmarks"].Header.Caption = "Get Marks";

            gridExam.DisplayLayout.Bands[0].Columns["FSubjectCode"].Header.Caption = "Subject Code";
            gridExam.DisplayLayout.Bands[0].Columns["FSubjectName"].Header.Caption = "Subject Name";
            gridExam.DisplayLayout.Bands[0].Columns["FTmarks"].Header.Caption = "Total Marks";
            gridExam.DisplayLayout.Bands[0].Columns["FGetmarks"].Header.Caption = "Get Marks";

            //Change alternate color
            gridExam.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridExam.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Hiding +/- Indicator
            this.gridExam.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void uddFeeTp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddFeeTp.DisplayLayout.Bands[0].Columns["subjectId"].Hidden = true;
            uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectCode"].Hidden = true;
            uddFeeTp.DisplayLayout.Bands[0].Columns["marks"].Hidden = true;
            uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectName"].Width = gridExam.DisplayLayout.Bands[0].Columns["subjectId"].Width;
            uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectName"].Header.Caption = "Subject";

            uddFeeTp.ValueMember = "subjectId";
            uddFeeTp.DisplayMember = "SubjectName";
        }
        private void prcInitializeVoucherGrid()
        {
            DataTable dt = new DataTable("tbl_Temp");
            dt.Columns.Add("subjectId", typeof(String));
            dt.Columns.Add("SubjectName", typeof(String));
            dt.Columns.Add("SubjectCode", typeof(String));
            dt.Columns.Add("marks", typeof(String));
            
            for (int i = 0; i < 15; i++)
            prcAddRow(ref dt);

            gridExam.DataSource = null;
            gridExam.DataSource = dt;

            gridExam.DisplayLayout.Bands[0].Columns["subjectId"].ValueList = uddFeeTp;
        }
        private void prcAddRow(ref DataTable dt)
        {
            if(cboExam.Text == "1")
            {
                dt.Rows.Add("");
            }
            else if (cboExam.Text == "2")
            {
                dt.Rows.Add("", "");
            }
            else if (cboExam.Text == "3")
            {
                dt.Rows.Add("","","");
            }
            else
            {
                dt.Rows.Add("", "", "","");
            }
        }
    }
}


