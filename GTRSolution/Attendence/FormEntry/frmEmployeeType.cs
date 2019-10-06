﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;


namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmEmployeeType : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();
        private string EmpPic = @"Z:\Com\Pics\EmpPic";
        private string PicName;

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmployeeType(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                                Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmEmployeeType_Load(object sender, EventArgs e)
        {
            try
            {
                GroupSalary.Visible = true;
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
                string sqlQuery = "Exec [prcGetEmployeeShort] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";
                dsList.Tables[1].TableName = "tblEmpType";
                dsList.Tables[2].TableName = "tblDesig";
                dsList.Tables[3].TableName = "tblSection";
                dsList.Tables[4].TableName = "tblSubSection";
                dsList.Tables[5].TableName = "tblDept";
                dsList.Tables[6].TableName = "tblshift";
                dsList.Tables[7].TableName = "tblReligion";
                dsList.Tables[8].TableName = "tblsex";
                dsList.Tables[9].TableName = "tblGrade";
                dsList.Tables[10].TableName = "tblPayMode";
                dsList.Tables[11].TableName = "tblWeekDay";
                dsList.Tables[12].TableName = "tblGradeInsur";
                dsList.Tables[13].TableName = "tblBand";
                dsList.Tables[14].TableName = "tblCategory";
                dsList.Tables[15].TableName = "tblIncenBand";
                dsList.Tables[16].TableName = "tblIncenSubBand";
                dsList.Tables[17].TableName = "tblBusStop";


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

            cboSubSec.DataSource = null;
            cboSubSec.DataSource = dsList.Tables["tblSubSection"];

            cboDept.DataSource = null;
            cboDept.DataSource = dsList.Tables["tblDept"];

            cboShift.DataSource = null;
            cboShift.DataSource = dsList.Tables["tblshift"];

            cboRelegion.DataSource = null;
            cboRelegion.DataSource = dsList.Tables["tblReligion"];

            cboSex.DataSource = null;
            cboSex.DataSource = dsList.Tables["tblsex"];

            cboGrade.DataSource = null;
            cboGrade.DataSource = dsList.Tables["tblGrade"];

            cboPayMode.DataSource = null;
            cboPayMode.DataSource = dsList.Tables["tblPayMode"];

            cboWeekDay.DataSource = null;
            cboWeekDay.DataSource = dsList.Tables["tblWeekDay"];

            cboGradeInsur.DataSource = null;
            cboGradeInsur.DataSource = dsList.Tables["tblGradeInsur"];

            cboBand.DataSource = null;
            cboBand.DataSource = dsList.Tables["tblBand"];

            cboCategory.DataSource = null;
            cboCategory.DataSource = dsList.Tables["tblCategory"];

            cboIncenBand.DataSource = null;
            cboIncenBand.DataSource = dsList.Tables["tblIncenBand"];

            cboIncenSubBand.DataSource = null;
            cboIncenSubBand.DataSource = dsList.Tables["tblIncenSubBand"];

            cboBusStop.DataSource = null;
            cboBusStop.DataSource = dsList.Tables["tblBusStop"];

            dtJDate.Value = DateTime.Today;
            dtBirthDate.Value = DateTime.Today;


            cboEmpType.Text = "Non-Management Employee";
            cboWeekDay.Value = 6;
            cboShift.Text = "";
            cboPayMode.Text = "Cash";
            cboRelegion.Text = "Islam";
            cboSex.Text = "Female";
            cboGrade.Text = "=N/A=";
            cboGradeInsur.Text = "=N/A=";
            cboBand.Text = "=N/A=";
            cboCategory.Text = "=N/A=";
            cboIncenBand.Text = "=N/A=";
            cboIncenSubBand.Text = "=N/A=";
            cboBusStop.Text = "=N/A=";
        }

        //private void btnSalary_Click(object sender, EventArgs e)
        //{
        //    GroupSalary.Visible = false;
        //}
        private void prcDisplayDetails(string strParam)
        {


            dsDetails = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "", sqlQuery1 = "";
            Int64 ActiveSalary = 0; 

            //Salary Permission Code
            sqlQuery = "Exec prcPermission_SalaryUser " + Common.Classes.clsMain.intComId + " ," + GTRHRIS.Common.Classes.clsMain.intUserId + ", " + Int32.Parse(strParam) + " ";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);

            sqlQuery1 = "Select dbo.fncCheckEmpSalary (" + Common.Classes.clsMain.intComId + ", " + GTRHRIS.Common.Classes.clsMain.intUserId + ")";
            ActiveSalary = clsCon.GTRCountingDataLarge(sqlQuery1);
            

            if (ActiveSalary == 1)
            {
                GroupSalary.Visible = true;
            }

            else
            {
                GroupSalary.Visible = false;
            }

            try
            {
                sqlQuery = "Exec prcGetEmployeeShort " + Common.Classes.clsMain.intComId + " , " +
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
                    this.cboSubSec.Value = dr["SubSectId"].ToString();
                    this.cboDept.Value = dr["DeptId"].ToString();
                    this.txtGS.Text = dr["GS"].ToString();
                    this.txtBS.Text = dr["BS"].ToString();
                    this.txtHR.Text = dr["HR"].ToString();
                    this.txtMA.Text = dr["MA"].ToString();
                    this.txttrn.Text = dr["trn"].ToString();
                    this.txtOtherAllow.Text = dr["OtherAllow"].ToString();
                    this.cboShift.Text = dr["ShiftDesc"].ToString();
                    this.cboRelegion.Text = dr["Religion"].ToString();
                    this.cboSex.Text = dr["Sex"].ToString();
                    this.cboGrade.Text = dr["Grade"].ToString();
                    this.txtCardNo.Text = dr["cardno"].ToString();

                    this.dtBirthDate.Value = dr["dtBirth"];
                    this.dtJDate.Value = dr["dtJoin"];
                    this.dtPFDate.Value = dr["dtPF"];
                    this.dtConfDate.Value = dr["dtConfirm"];
                    this.dIncreDate.Value = dr["dtIncrement"];
                    this.cboPayMode.Text = dr["Paymode"].ToString();
                    this.txtCode.Text = dr["EmpCode"].ToString();
                    this.cboWeekDay.Value = dr["weekdayid"].ToString();
                    this.cboGradeInsur.Text = dr["GradeIns"].ToString();
                    this.cboBand.Text = dr["Band"].ToString();
                    this.cboCategory.Text = dr["Category"].ToString();
                    this.cboIncenBand.Text = dr["BandIncen"].ToString();
                    this.cboIncenSubBand.Text = dr["SubBandIncen"].ToString();
                    this.cboBusStop.Text = dr["BusStop"].ToString();
                    this.txtMobAllow.Text = dr["mobileAllow"].ToString();
                    this.txtCode.Value = dr["empCode"].ToString();
                    this.txtFather.Value = dr["EmpFather"].ToString();
                    this.txtSpouse.Value = dr["EmpSpouse"].ToString();
                    this.txtAccNo.Value = dr["BankAcNo"].ToString();
                    this.txtMobile.Value = dr["EmpMobile"].ToString();
                    this.txtCurrAdd.Value = dr["EmpCurrAdd"].ToString();
                    this.txtPerAdd.Value = dr["EmpPerAdd"].ToString();

                    if (dr["EmpPicLocation"].ToString().Length > 0)
                    {
                        if ((EmpPic + "\\" + dr["EmpPicLocation"].ToString()).Length > 0)
                        {
                            if (File.Exists(EmpPic + "\\" + dr["EmpPicLocation"].ToString()))
                            {
                                picPreview.Image = new Bitmap(EmpPic + "\\" + dr["EmpPicLocation"].ToString());
                            }
                            else
                            {
                                picPreview.Image = new Bitmap(EmpPic + "\\1.jpg");
                            }
                        }
                        else
                        {
                            picPreview.Image = null;
                        }

                    }


                    if (dr["IsAllowPF"].ToString() == "1")
                    {
                        checkPF.Checked = true;
                    }
                    if (dr["IsAllowOT"].ToString() == "1")
                    {
                        checkOT.Checked = true;
                    }
                    if (dr["IsInactive"].ToString() == "1")
                    {
                        checkYesNo.Checked = true;
                    }
                    if (dr["IsTiffin"].ToString() == "1")
                    {
                        checkTiffin.Checked = true;
                    }
                    if (dr["IsTrnDeduction"].ToString() == "1")
                    {
                        checktrn.Checked = true;
                    }
                    if (dr["IsAllowAttBns"].ToString() == "1")
                    {
                        checkAttnBns.Checked = true;
                    }
                    if (dr["IsIncenBonus"].ToString() == "1")
                    {
                        checkIncenBns.Checked = true;
                    }
                    if (dr["IsConfirm"].ToString() == "1")
                    {
                        checkConfirm.Checked = true;
                    }
                    if (dr["IsSalary"].ToString() == "1")
                    {
                        checkSalary.Checked = true;
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


            //txtCode.Text = NewId;
            cboEmpType.Value = "";
            //txtCode.Text = "";
            txtnewcode.Text = "";
            txtCode.Text = "";

            txtName.Text = "";
            txtBName.Text = "";

            cboSec.Value = "";
            cboSubSec.Value = "";
            cboDept.Value = "";
            cboDesig.Value = "";
            cboGrade.Value = "";
            cboGradeInsur.Value = "";
            cboBand.Value = "";
            cboCategory.Value = "";
            cboIncenBand.Value = "";
            cboIncenSubBand.Value = "";
            cboBusStop.Value = "";
            txtCardNo.Text = "";
            txtFather.Text = "=N/A=";
            txtSpouse.Text = "=N/A=";
            txtMobile.Text = "=N/A=";
            txtAccNo.Text = "=N/A=";
            txtCurrAdd.Text = "=N/A=";
            txtPerAdd.Text = "=N/A=";


            dtJDate.Value = DateTime.Today;
            dtBirthDate.Value = DateTime.Today;
            dtPFDate.Value = "1-Jan-1900";
            dtConfDate.Value = "1-Jan-1900";
            dIncreDate.Value = "1-Jan-1900";

            cboShift.Value = "";
            cboRelegion.Value = "";

            cboSex.Value = "";

            cboPayMode.Value = "";

            txtMobAllow.Text = "0";
            txttrn.Text = "0";
            txtGS.Text = "0";
            txtBS.Text = "0";
            txtHR.Text = "0";
            txtMA.Text = "0";
            txtOtherAllow.Text = "0";

            picPreview.Image = null;


            checkOT.Checked = false;
            checkPF.Checked = false;
            checkYesNo.Checked = false;
            checkTiffin.Checked = false;
            checktrn.Checked = false;
            checkAttnBns.Checked = false;
            checkIncenBns.Checked = false;
            checkConfirm.Checked = false;
            checkSalary.Checked = false;

            cboWeekDay.Value = 6;
            cboEmpType.Text = "Non-Management Employee";

            GroupSalary.Visible = true;

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

        private void cboShift_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboShift.DisplayLayout.Bands[0].Columns["ShiftDesc"].Width = cboShift.Width;
            cboShift.DisplayLayout.Bands[0].Columns["ShiftDesc"].Header.Caption = "Shift Type";
            cboShift.DisplayMember = "ShiftDesc";
            cboShift.ValueMember = "ShiftID";
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

        private void cboGrade_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Width = cboGrade.Width;
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
            cboGrade.DisplayMember = "Grade";
            cboGrade.ValueMember = "Grade";
        }

        private void cboPayMode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPayMode.DisplayLayout.Bands[0].Columns["Paymode"].Width = cboPayMode.Width;
            cboPayMode.DisplayLayout.Bands[0].Columns["Paymode"].Header.Caption = "Pay Mode";
            cboPayMode.DisplayMember = "Paymode";
            cboPayMode.ValueMember = "Paymode";
        }

        private void frmEmployeeType_FormClosing(object sender, FormClosingEventArgs e)
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
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70; //Employee code
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 100; //Employee Name
                gridList.DisplayLayout.Bands[0].Columns["EmpSec"].Width = 100; //Section 
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 80; //Grade 
                gridList.DisplayLayout.Bands[0].Columns["Band"].Width = 70; //Card No
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 90; //Designation Name

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["EmpSec"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";

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
            //if (this.txtCode.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide employee code no.");
            //    txtCode.Focus();
            //    return true;
            //}

            //if (this.cboEmpType.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide employee type.");
            //    cboEmpType.Focus();
            //    return true;
            //}

            //if (this.cboEmpType.IsItemInList() == false)
            //{
            //    MessageBox.Show("Please provide valid data [or, select from list].");
            //    cboEmpType.Focus();
            //    return true;
            //}


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


            if (this.txtGS.Text.Length == 0)
            {
                MessageBox.Show("Please provide gross salary.");
                txtGS.Focus();
                return true;
            }

            if (this.cboWeekDay.Text.Length == 0)
            {
                MessageBox.Show("Please provide WeeklyHoliday.");
                cboWeekDay.Focus();
                return true;
            }

            if (this.cboShift.Text.Length == 0)
            {
                MessageBox.Show("Please provide Shift Code.");
                cboShift.Focus();
                return true;
            }

            if (this.cboWeekDay.IsItemInList() == false)
            {
                MessageBox.Show("Please provide valid data [or, select from list].");
                cboWeekDay.Focus();
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
            Int64 DesigCapacity = 0, EmpCapacity = 0, ChkDesig=0;
            try
            {

                //Member Master Table
                if (btnSave.Text != "&Save")
                {

                    //Update
                    sqlQuery = "Update tblEmp_Info set   EmpCode = '" + this.txtnewcode.Text + "',EmpName ='" +
                               this.txtName.Text.ToString() + "', empNameB = '" + this.txtBName.Text.ToString() +
                               "', EmpType = '" + this.cboEmpType.Value.ToString() + "',DesigID = '" +
                               this.cboDesig.Value.ToString() + "', SectId= '" + this.cboSec.Value.ToString() +
                               "',SubSectID='" + this.cboSubSec.Value.ToString() + "',DeptID='" + this.cboDept.Value.ToString() + "', dtJoin= '" + clsProc.GTRDate(this.dtJDate.Value.ToString()) + "',dtBirth='" +
                               clsProc.GTRDate(this.dtBirthDate.Value.ToString()) + "',dtPF= '" + clsProc.GTRDate(this.dtPFDate.Value.ToString()) +
                               "',dtConfirm= '" + clsProc.GTRDate(this.dtConfDate.Value.ToString()) + "',dtIncrement= '" + clsProc.GTRDate(this.dIncreDate.Value.ToString()) + "',GS='" +
                               this.txtGS.Text.ToString() + "',BS='" + this.txtBS.Text.ToString() +
                               "',HR='" + this.txtHR.Text.ToString() + "',MA='" + this.txtMA.Text.ToString() +
                               "',trn='" + this.txttrn.Text.ToString() + "',OtherAllow='" + this.txtOtherAllow.Text.ToString() + "',ShiftType='" +
                               this.cboShift.Text.ToString() + "',ShiftID='" + this.cboShift.Value.ToString() +
                               "',Religion='" + this.cboRelegion.Value.ToString() + "',Sex='" + this.cboSex.Value.ToString() +
                               "',Grade='" +
                               this.cboGrade.Value.ToString() + "',GradeIns='" + this.cboGradeInsur.Value.ToString() +
                               "',cardno = '" + txtCardNo.Text.ToString() + "',PayMode='" +
                               this.cboPayMode.Value.ToString() + "',IsAllowOT='" +
                               checkOT.Tag.ToString() + "', IsAllowPF='" + this.checkPF.Tag.ToString() +
                               "', IsInactive='" + checkYesNo.Tag.ToString() + "',IsTiffin='" + checkTiffin.Tag.ToString() +
                               "', IsTrnDeduction='" + checktrn.Tag.ToString() + "', IsAllowAttBns='" + checkAttnBns.Tag.ToString() +
                               "', IsIncenBonus='" + checkIncenBns.Tag.ToString() + "',IsConfirm='" + checkConfirm.Tag.ToString() +
                               "', IsSalary='" + checkSalary.Tag.ToString() + "', Weekdayid='" + cboWeekDay.Value.ToString() +
                               "', mobileAllow='" + txtMobAllow.Text.ToString() + 
                               "', Band='" + this.cboBand.Value.ToString() + "', Category='" + this.cboCategory.Value.ToString() + 
                               "', BandIncen='" + this.cboIncenBand.Value.ToString() + "', SubBandIncen='" + this.cboIncenSubBand.Value.ToString() +
                               "', BusStop='" + this.cboBusStop.Value.ToString() + "',EmpPicLocation= '" + txtnewcode.Text + ".jpg" + "',EmpFather= '" + txtFather.Text.ToString() +
                               "', EmpSpouse='" + txtSpouse.Text.ToString() + "', BankAcNo = '" + txtAccNo.Text.ToString() + 
                               "', EmpMobile= '" + txtMobile.Text.ToString() + "', EmpCurrAdd = '" + txtCurrAdd.Text.ToString() + 
                               "', EmpPerAdd = '" + txtPerAdd.Text.ToString() + "' Where empid =  '" + this.txtnewcode.Text + "' ";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    
                    //Designation Capacity Checking Code

                    sqlQuery = "Select dbo.fncCheckDesigCapacity (" + Common.Classes.clsMain.intComId + ", " + this.cboDesig.Value.ToString() + ")";
                    ChkDesig = clsCon.GTRCountingDataLarge(sqlQuery);
 

                        if (ChkDesig==1)
                        {
                            MessageBox.Show("Capacity over for this designation.Please communicate to administrator.");
                            return;
                        }


                    //NewId
                        sqlQuery = "Select Isnull(Max(EmpId),0)+1 As NewId from tblEmp_Info";
                        NewId = clsCon.GTRCountingDataLarge(sqlQuery);

                        String EmpImageID = NewId + ".jpg";

                        //Insert Data
                        sqlQuery = "Insert Into tblEmp_Info (ComId,aEmpid,Empid, EmpCode, empType, EmpName, empNameB, DesigID, SectId, SectIdSal, DeptID, SubSectID, dtJoin,dtBirth,dtIncrement,dtConfirm,dtPF,GS, ShiftType, ShiftID, Religion, Sex, Grade,cardno, PayMode, IsAllowOT, IsAllowPF, IsInactive,weekdayid,mobileAllow,GradeIns,Band,Category,BandIncen,SubBandIncen,BusStop,IsAllowAttBns,IsConfirm,IsTrnDeduction,IsIncenBonus,IsSalary,IsTiffin,trn,BS,HR,MA,OtherAllow,EmpPicLocation,EmpFather,EmpSpouse,BankAcNo,EmpMobile,EmpCurrAdd,EmpPerAdd)"
                                   + " Values (" + Common.Classes.clsMain.intComId + ", " + NewId + ", " + NewId + ",'" + NewId + "', '" +
                                   this.cboEmpType.Value.ToString() + "',' " +
                                   this.txtName.Text + "',' " + this.txtBName.Text.ToString() + "', " +
                                   this.cboDesig.Value.ToString() + ", " + this.cboSec.Value.ToString() + "," +
                                   this.cboSec.Value.ToString() + ",'" + this.cboDept.Value.ToString() + "','" +
                                   this.cboSubSec.Value.ToString() + "','" +
                                   this.clsProc.GTRDate(this.dtJDate.Value.ToString()) + "','" +
                                   this.clsProc.GTRDate(this.dtBirthDate.Value.ToString()) + "','" +
                                   this.clsProc.GTRDate(this.dIncreDate.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtConfDate.Value.ToString()) + "','" +
                                   this.clsProc.GTRDate(this.dtPFDate.Value.ToString()) + "','" +
                                   this.txtGS.Text.ToString() + "','" + this.cboShift.Text.ToString() + "','" + 
                                   this.cboShift.Value.ToString() + "','" +
                                   this.cboRelegion.Value.ToString() + "','" + this.cboSex.Value.ToString() + "','" +
                                   this.cboGrade.Value.ToString() + "','" +
                                   this.txtCardNo.Text.ToString() + "' , '" +
                                   this.cboPayMode.Value.ToString() + "','" +
                                   this.checkOT.Tag.ToString() + "','" + this.checkPF.Tag.ToString() + "','" +
                                   this.checkYesNo.Tag.ToString() + "','" +
                                   this.cboWeekDay.Value.ToString() + "','" + this.txtMobAllow.Text.ToString() + "','" +
                                   this.cboGradeInsur.Value.ToString() + "','" + this.cboBand.Value.ToString() + "','" +
                                   this.cboCategory.Value.ToString() + "','" + this.cboIncenBand.Value.ToString() + "','" +
                                   this.cboIncenSubBand.Value.ToString() + "','" + this.cboBusStop.Value.ToString() + "','" +
                                   this.checkAttnBns.Tag.ToString() + "','" + this.checkConfirm.Tag.ToString() + "','" +
                                   this.checktrn.Tag.ToString() + "','" + this.checkIncenBns.Tag.ToString() + "','" +
                                   this.checkSalary.Tag.ToString() + "','" + this.checkTiffin.Tag.ToString() + "','" + 
                                   this.txttrn.Text.ToString() + "','" + this.txtBS.Text.ToString() + "','" + 
                                   this.txtHR.Text.ToString() + "','" + this.txtMA.Text.ToString() + "','" +
                                   this.txtOtherAllow.Text.ToString() + "','" + EmpImageID + "', '" +
                                   this.txtFather.Text.ToString() + "','" + this.txtSpouse.Text.ToString() + "','" +
                                   this.txtAccNo.Text.ToString() + "','" + this.txtMobile.Text.ToString() + "','" +
                                   this.txtCurrAdd.Text.ToString() + "','" + this.txtPerAdd.Text.ToString() + "')";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
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

        private void checkPF_CheckedChanged(object sender, EventArgs e)
        {
            checkPF.Tag = 0;
            if (checkPF.Checked == true)
            {
                checkPF.Tag = 1;
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

        private void dtJDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtBirthDate_KeyDown(object sender, KeyEventArgs e)
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

        private void cboBlood_KeyDown(object sender, KeyEventArgs e)
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
                sqlQuery = "Delete from tblEmp_Info Where EmpID= " + Int32.Parse(txtnewcode.Value.ToString()) + " and comid = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcLoadList();
                prcClearData();
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

        private void cboWeekDay_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboWeekDay.DisplayLayout.Bands[0].Columns["name"].Header.Caption = "Weekly Holiday";
            cboWeekDay.DisplayLayout.Bands[0].Columns["value"].Hidden = true;
            cboWeekDay.DisplayLayout.Bands[0].Columns["name"].Width = cboWeekDay.Width;

            cboWeekDay.ValueMember = "value";
            cboWeekDay.DisplayMember = "name";
        }

        //private void cboCode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{

        //    // A.EmpName, B.SectName As EmpSec, D.DesigName, A.Grade, A.CardNo
        //    cboCode.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
        //    cboCode.DisplayLayout.Bands[0].Columns["EmpName"].Hidden = true;

        //    cboCode.DisplayLayout.Bands[0].Columns["EmpSec"].Hidden = true;
        //    cboCode.DisplayLayout.Bands[0].Columns["DesigName"].Hidden = true;
        //    cboCode.DisplayLayout.Bands[0].Columns["Grade"].Hidden = true;

        //    cboCode.DisplayLayout.Bands[0].Columns["CardNo"].Hidden = true;
        //    //cboCode.DisplayLayout.Bands[0].Columns["ShiftCat"].Hidden = true;

        //    cboCode.DisplayMember = "empCode";
        //    cboCode.ValueMember = "empId";
        //}

        private void cboCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        //private void cboCode_ValueChanged(object sender, EventArgs e)
        //{
        //    prcClearData();
        //    if (this.cboCode.IsItemInList() == false)
        //    {
        //        //MessageBox.Show("Please Provide valid data [or, select from list].");
        //        //cboEmpID.Focus();

        //        prcLoadCombo();
        //        return;
        //    }

        //    if (cboCode.Value == null)
        //    {
        //        return;
        //    }
        //    prcDisplayDetails(cboCode.Value.ToString());
        //}


        private void txtMobileNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtAccNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtOldGS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTaxGS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboWeekDay_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboWeekDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtAccNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtOldGS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtTaxGS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        //private void cboCode_Leave(object sender, EventArgs e)
        //{
        //    prcClearData();
        //    if (this.cboCode.IsItemInList() == false)
        //    {
        //        //MessageBox.Show("Please Provide valid data [or, select from list].");
        //        //cboEmpID.Focus();

        //        prcLoadCombo();
        //        txtCode.Text = cboCode.Text;
        //        return;
        //    }


        //    if (cboCode.Value == null)
        //    {
        //        return;
        //    }
        //    prcDisplayDetails(cboCode.Value.ToString());
        //}


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

        private void cboDept_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDeptBangla_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboSubSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dIncreDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtConfDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtPFDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboGradeInsur_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboBand_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCategory_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboIncenBand_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboIncenSubBand_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboBusStop_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txttrn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkTiffin_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checktrn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkAttnBns_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkIncenBns_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void checkSalary_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtGS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMA_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtBS_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMobAllow_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtHR_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtOtherAllow_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtGS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void txttrn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void txtBS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void txtHR_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void txtMA_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void txtOtherAllow_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void txtMobAllow_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRNumber(e.KeyChar.ToString());
        }

        private void cboDept_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDept.DisplayLayout.Bands[0].Columns["DeptName"].Width = cboDept.Width;
            cboDept.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Department";
            cboDept.DisplayLayout.Bands[0].Columns["DeptId"].Hidden = true;
            cboDept.DisplayMember = "DeptName";
            cboDept.ValueMember = "DeptId";
        }


        private void cboSubSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSubSec.DisplayLayout.Bands[0].Columns["SubSectName"].Width = cboSubSec.Width;
            cboSubSec.DisplayLayout.Bands[0].Columns["SubSectName"].Header.Caption = "Sub Section";
            cboSubSec.DisplayLayout.Bands[0].Columns["SubSectId"].Hidden = true;
            cboSubSec.DisplayMember = "SubSectName";
            cboSubSec.ValueMember = "SubSectId";
        }

        private void cboGradeInsur_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGradeInsur.DisplayLayout.Bands[0].Columns["InsuranceGrade"].Width = cboGradeInsur.Width;
            cboGradeInsur.DisplayLayout.Bands[0].Columns["InsuranceGrade"].Header.Caption = "Insurance Grade";
            cboGradeInsur.DisplayMember = "InsuranceGrade";
            cboGradeInsur.ValueMember = "InsuranceGrade";
        }

        private void cboBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBand.DisplayLayout.Bands[0].Columns["Band"].Width = cboBand.Width;
            cboBand.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
            cboBand.DisplayMember = "Band";
            cboBand.ValueMember = "Band";
        }

        private void cboCategory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCategory.DisplayLayout.Bands[0].Columns["Category"].Width = cboBand.Width;
            cboCategory.DisplayLayout.Bands[0].Columns["Category"].Header.Caption = "Category";
            cboCategory.DisplayMember = "Category";
            cboCategory.ValueMember = "Category";
        }

        private void cboIncenBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboIncenBand.DisplayLayout.Bands[0].Columns["IncenBand"].Width = cboIncenBand.Width;
            cboIncenBand.DisplayLayout.Bands[0].Columns["IncenBand"].Header.Caption = "Incentive Band";
            cboIncenBand.DisplayMember = "IncenBand";
            cboIncenBand.ValueMember = "IncenBand";
        }

        private void cboIncenSubBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboIncenSubBand.DisplayLayout.Bands[0].Columns["IncenSubBand"].Width = cboIncenSubBand.Width;
            cboIncenSubBand.DisplayLayout.Bands[0].Columns["IncenSubBand"].Header.Caption = "Incentive Sub Band";
            cboIncenSubBand.DisplayMember = "IncenSubBand";
            cboIncenSubBand.ValueMember = "IncenSubBand";
        }

        private void cboBusStop_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBusStop.DisplayLayout.Bands[0].Columns["BusStop"].Width = cboBusStop.Width;
            cboBusStop.DisplayLayout.Bands[0].Columns["BusStop"].Header.Caption = "Bus Stoppage";
            cboBusStop.DisplayMember = "BusStop";
            cboBusStop.ValueMember = "BusStop";
        }

        private void checkTiffin_CheckedChanged(object sender, EventArgs e)
        {
            checkTiffin.Tag = 0;
            if (checkTiffin.Checked == true)
            {
                checkTiffin.Tag = 1;
            }
        }

        private void checktrn_CheckedChanged(object sender, EventArgs e)
        {
            checktrn.Tag = 0;
            if (checktrn.Checked == true)
            {
                checktrn.Tag = 1;
            }
        }

        private void checkAttnBns_CheckedChanged(object sender, EventArgs e)
        {
            checkAttnBns.Tag = 0;
            if (checkAttnBns.Checked == true)
            {
                checkAttnBns.Tag = 1;
            }
        }

        private void checkIncenBns_CheckedChanged(object sender, EventArgs e)
        {
            checkIncenBns.Tag = 0;
            if (checkIncenBns.Checked == true)
            {
                checkIncenBns.Tag = 1;
            }
        }

        private void checkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            checkConfirm.Tag = 0;
            if (checkConfirm.Checked == true)
            {
                checkConfirm.Tag = 1;
            }
        }

        private void checkSalary_CheckedChanged(object sender, EventArgs e)
        {
            checkSalary.Tag = 0;
            if (checkSalary.Checked == true)
            {
                checkSalary.Tag = 1;
            }
        }

        private void txtGS_ValueChanged(object sender, EventArgs e)
        {
            if (txtGS.Text.Length == 0)
            {
                txtBS.Value = 0;
                txtHR.Value = 0;
                txtMA.Value = 0;

            }

            if (double.Parse(txtGS.Value.ToString()) >= 0)
            {

                txtBS.Value = Math.Round((((double.Parse(txtGS.Value.ToString()) - 560) /1.4)));
                txtMA.Value = 560;
                txtHR.Value = (double.Parse(txtGS.Value.ToString()) - ((double.Parse(txtBS.Value.ToString())) + (double.Parse(txtMA.Value.ToString()))));
                
                //Previous Salary Structure
                //txtBS.Value = Math.Round((double.Parse(txtGS.Value.ToString()) * 0.60));
                //txtMA.Value = Math.Round((double.Parse(txtGS.Value.ToString()) * 0.10));
                //txtHR.Value = (double.Parse(txtGS.Value.ToString()) - ((double.Parse(txtBS.Value.ToString())) + (double.Parse(txtMA.Value.ToString()))));
            }
   
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (gridList.ActiveRow.IsFilterRow == false)
                {
                    prcClearData();

                    prcDisplayDetails(gridList.ActiveRow.Cells["empid"].Value.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog diagOpen = new OpenFileDialog();
                diagOpen.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (diagOpen.ShowDialog() == DialogResult.OK)
                {
                    PicName = diagOpen.FileName.Substring(diagOpen.FileName.LastIndexOf("\\") + 1);
                    txtImageName.Text = txtCode.Text + PicName.Substring(PicName.LastIndexOf(".") + 0);

                    txtImageName.Tag = diagOpen.FileName;
                    picPreview.Image = new Bitmap(diagOpen.FileName);
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }


        private void btnUpload_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtImageName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFather_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFather_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtSpouse_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSpouse_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCurrAdd_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCurrAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPerAdd_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPerAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void btnEmp_Click(object sender, EventArgs e)
        {

            if (gridList.Rows.Count == 0)
            {
                return;
            }
            clsMain.strRelationalId = gridList.ActiveRow.Cells["EmpID"].Value.ToString();
            FM.prcExecuteChildForm("Attendence.FormEntry", "frmEmployee");

        }


    }
}
