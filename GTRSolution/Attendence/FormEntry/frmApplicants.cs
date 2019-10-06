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

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmApplicants : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();
        private string EmpPic = @"Z:\Com\Pics\EmpPic";
        private string PicName;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmApplicants(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlQuery = "Exec [prcGetApplicants] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                //Tab : Basic
                dsList.Tables[0].TableName = "tblCode";
                dsList.Tables[1].TableName = "tblCountry";
                dsList.Tables[2].TableName = "tblBlood";
                dsList.Tables[3].TableName = "tblMaritialsts";

                dsList.Tables[4].TableName = "tblRelegion";
                dsList.Tables[5].TableName = "tblSex";
                dsList.Tables[6].TableName = "tblJobType";
                dsList.Tables[7].TableName = "tblDistrict";

                dsList.Tables[8].TableName = "tblEducation";
                dsList.Tables[9].TableName = "tblTraining";
                dsList.Tables[10].TableName = "tblExpiriance";
                dsList.Tables[11].TableName = "tblReference";
                dsList.Tables[12].TableName = "tblCom";

                gridEdu.DataSource = null;
                gridEdu.DataSource = dsList.Tables["tblEducation"];

                gridTraining.DataSource = null;
                gridTraining.DataSource = dsList.Tables["tblTraining"];

                gridExp.DataSource = null;
                gridExp.DataSource = dsList.Tables["tblExpiriance"];

                gridRef.DataSource = null;
                gridRef.DataSource = dsList.Tables["tblReference"];

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
            cboCode.DataSource = dsList.Tables["tblCode"];

            cboNationality.DataSource = null;
            cboNationality.DataSource = dsList.Tables["tblCountry"];

            cboBlood.DataSource = null;
            cboBlood.DataSource = dsList.Tables["tblBlood"];

            cboMarit.DataSource = null;
            cboMarit.DataSource = dsList.Tables["tblMaritialsts"];

            cboRelegion.DataSource = null;
            cboRelegion.DataSource = dsList.Tables["tblRelegion"];

            cboSex.DataSource = null;
            cboSex.DataSource = dsList.Tables["tblSex"];

            cboType.DataSource = null;
            cboType.DataSource = dsList.Tables["tblJobType"];

            cboCurrDist.DataSource = null;
            cboCurrDist.DataSource = dsList.Tables["tblDistrict"];

            cboPreDist.DataSource = null;
            cboPreDist.DataSource = dsList.Tables["tblDistrict"];

            cboApplyTo.DataSource = null;
            cboApplyTo.DataSource = dsList.Tables["tblCom"];
        }

        private Boolean fncBlank()
        {
            if (this.cboCode.Text.Length == 0)
            {
                MessageBox.Show("Please Provide employee Code");
                cboCode.Focus();
                return true;
            }
            //if (this.cboPreDist.Text.Length == 0)
            //{
            //    MessageBox.Show("Please Provide present district");
            //    cboPreDist.Focus();
            //    return true;
            //}

            return false;
        }

        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetApplicants " + Common.Classes.clsMain.intComId + " , " + 
                                  Int32.Parse(strParam) + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";
                dsDetails.Tables[1].TableName = "Education";
                dsDetails.Tables[2].TableName = "Training";
                dsDetails.Tables[3].TableName = "Experience";
                dsDetails.Tables[4].TableName = "Reference";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    // Tab Basic Information\
                    this.txtId.Text = dr["AppId"].ToString();
                    this.txtCode.Text = dr["App_Code"].ToString();
                    this.txtName.Text = dr["AppName"].ToString();
                    this.cboApplyTo.Value = dr["comId"].ToString();
                    //Tab Office Information
                    this.txtFather.Text = dr["AppFather"].ToString();
                    this.txtMother.Text = dr["AppMother"].ToString();
                    this.dtApply.Text = dr["dtApp"].ToString();
                    this.txtNationalID.Text = dr["VoterNo"].ToString();
                    this.cboNationality.Value = dr["countryId"].ToString();
                    this.cboBlood.Text = dr["BloodGroup"].ToString();
                    this.dtBirth.Text = dr["dtBirth"].ToString();
                    this.cboMarit.Text = dr["MaritalSts"].ToString();
                    this.cboRelegion.Text = dr["Religion"].ToString();
                    this.cboSex.Text = dr["Sex"].ToString();

                    this.txtMobile.Text = dr["mobileself"].ToString();
                    this.txtMobHome.Text = dr["mobileHome"].ToString();
                    this.txtPassport.Text = dr["PassportNo"].ToString();
                    this.txtMail.Text = dr["AppEmail"].ToString();
                    this.cboType.Text = dr["AppType"].ToString();

                    this.txtCurrAdd.Text = dr["AppCurrAdd"].ToString();
                    this.cboCurrPost.Text = dr["AppCurrPO"].ToString();
                    this.cboCurrPS.Text = dr["AppCurrPS"].ToString();
                    this.cboCurrDist.Text = dr["DistId"].ToString();
                    this.cboCurrCity.Text = dr["AppCurrCity"].ToString();

                    this.txtPreadd.Text = dr["AppPerAdd"].ToString();
                    this.cboPrePost.Text = dr["AppPerPO"].ToString();
                    this.cboPrePS.Text = dr["AppPerPO"].ToString();
                    this.cboPreDist.Text = dr["DistId"].ToString();
                    this.cboPreCity.Text = dr["AppPerCity"].ToString();
                    if (dr["AppPicLocation"].ToString().Length > 0)
                    {
                        picPreview.Image = new Bitmap(EmpPic + "\\" + dr["AppPicLocation"].ToString());
                    }
                }
                gridEdu.DataSource = null;
                gridEdu.DataSource = dsDetails.Tables["Education"];

                gridTraining.DataSource = null;
                gridTraining.DataSource = dsDetails.Tables["Training"];

                gridExp.DataSource = null;
                gridExp.DataSource = dsDetails.Tables["Experience"];

                gridRef.DataSource = null;
                gridRef.DataSource = dsDetails.Tables["Reference"];

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
            cboCode.Text = "";
            txtName.Text = "";
            txtFather.Text = "";
            txtMother.Text = "";
            dtApply.Value = DateTime.Today.ToString();
            txtNationalID.Text = "";
            cboNationality.Text = "";
            cboBlood.Text = "";
            dtBirth.Value = DateTime.Today.ToString();
            cboMarit.Text = "";
            cboRelegion.Text = "";
            cboSex.Value = "";
            txtMobile.Text = "";
            txtMobHome.Text = "";
            txtMail.Text = "";
            txtPassport.Text = "";
            cboType.Text = "";

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
                    sqlQuery = " Delete From tblApp_Edu  Where AppId = " + Int32.Parse(txtId.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Delete From tblApp_Training  Where AppId = " + Int32.Parse(txtId.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Delete From tblApp_Exp  Where AppId = " + Int32.Parse(txtId.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Delete From tblApplicants_Ref  Where AppId = " + Int32.Parse(txtId.Text.ToString()) + " ";
                    arQuery.Add(sqlQuery);

                    //Update
                    sqlQuery = "Update tblJobApp_Info set App_Code = '" + this.txtCode.Text.ToString() + "', AppName = '" +
                               txtName.Text.ToString() + "', applytocom = '"+cboApplyTo.Value.ToString() +"',AppFather = '" + txtFather.Text.ToString() + "', AppMother = '" +
                               txtMother.Text.ToString() + "', dtApp = '" + clsProc.GTRDate(dtApply.Value.ToString()) +
                               "', VoterNo = '" + txtNationalID.Text.ToString() + "', Nationality = '" + cboNationality.Value.ToString() + "', BloodGroup = '" +
                               cboBlood.Text.ToString() + "', dtBirth = '" + dtBirth.Value.ToString() +"', MaritalSts = '" + cboMarit.Text.ToString() + "', Religion = '" +
                               cboRelegion.Text.ToString() + "', Sex = '" +cboSex.Text.ToString() + "',  mobileself = '" + txtMobile.Text.ToString() +
                               "', mobileHome = '" + txtMobHome.Text.ToString() + "', PassportNo = '" +txtPassport.Text.ToString() + "', AppEmail = '" +
                               txtMail.Text.ToString() + "', AppType = '" + cboType.Text.ToString() +
                               "', AppCurrAdd = '" + txtCurrAdd.Text.ToString() + "', AppCurrPO = '" +cboCurrPost.Text.ToString() + "', AppCurrPS = '" +
                               cboCurrPS.Text.ToString() + "', AppCurrDistId = '" + cboCurrDist.Value.ToString() + "', AppCurrCity = '" + cboCurrCity.Text.ToString() + "', AppPerAdd = '" +
                               txtPreadd.Text.ToString() + "', AppPerPO = '" + cboPrePost.Text.ToString() + "', AppPerPS = '" + cboPrePS.Text.ToString() + "', AppPerCity = '" +
                               cboPreCity.Text.ToString() + "', AppPicLocation = '" + 
                               txtImageName.Text.ToString() + "'  where  AppId = '" + this.txtId.Text.ToString() + "' ";

                    arQuery.Add(sqlQuery);

                    fncGetGridEdu(ref arQuery, txtId.Text);
                    fncGetGridTraining(ref arQuery, txtId.Text);
                    fncGetgridExp(ref arQuery, txtId.Text);
                    fncGetGridRefrence(ref arQuery, txtId.Text);

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
                    //NewId
                    sqlQuery = "Select Isnull(Max(AppId),0)+1 As NewId from tblJobApp_Info";
                    NewId = clsCon.GTRCountingDataLarge(sqlQuery);

                    //Insert Data
                    sqlQuery =
                        "Insert Into tblJobApp_Info (ComID, AppId, App_Code, AppName, applytocom, AppFather, AppMother, dtApp, VoterNo, Nationality, BloodGroup, dtBirth, MaritalSts" +
                        ",Religion,sex, mobileself, mobileHome, PassportNo,AppEmail, AppType, AppCurrAdd, AppCurrPO,AppCurrPS, AppCurrDistId" +
                        ", AppCurrCity, AppPerAdd, AppPerPO, AppPerPS, AppPerDistId, AppPerCity, aId,AppPicLocation)"
                        + " Values ('" + Common.Classes.clsMain.intComId + "', " + NewId + ",'" +
                        txtCode.Text.ToString() + "','" + txtName.Text.ToString() + "', '"+cboApplyTo.Value.ToString() +"', '" + txtFather.Text.ToString() +
                        "', '" + txtMother.Text.ToString() + "', '" + dtApply.Value.ToString() + "', '" +
                        txtNationalID.Text.ToString() + "', '" + cboNationality.Value.ToString() + "','" +
                        cboBlood.Text.ToString() + "', '" + dtBirth.Value.ToString() + "', '" + cboMarit.Text.ToString() +
                        "', '" + cboRelegion.Text.ToString() + "', '" +cboSex.Text.ToString() + "', '" + txtMobile.Text.ToString() + "', '" +
                        txtMobHome.Text.ToString() + "', '" + txtPassport.Text.ToString() + "', '" +
                        txtMail.Text.ToString() + "', '" + cboType.Text.ToString() + "', '" + txtCurrAdd.Text.ToString() +
                        "', '" + cboCurrPost.Text.ToString() + "', '" +cboCurrPS.Text.ToString() + "', '" + cboCurrDist.Value.ToString() + "', '" +
                        cboCurrCity.Text.ToString() + "', '" +txtPreadd.Text.ToString() + "', '" + cboPrePost.Text.ToString() + "', '" +
                        cboPrePS.Text.ToString() + "', '" +cboPreDist.Value.ToString() + "', '" + cboPreCity.Text.ToString() + "', "+NewId+", '"+txtImageName.Text.ToString()+"')";

                    arQuery.Add(sqlQuery);

                    fncGetGridEdu(ref arQuery, NewId.ToString());
                    fncGetGridTraining(ref arQuery, NewId.ToString());
                    fncGetgridExp(ref arQuery, NewId.ToString());
                    fncGetGridRefrence(ref arQuery, NewId.ToString());

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to Delete Applicants information of ['" + txtCode.Text.ToString() + "']", "",
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
                sqlQuery = "Delete from tblJobApp_Info Where AppID = " + Int32.Parse(txtId.Value.ToString()) +
                           " and comid = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
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

        private void cboSex_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSex.DisplayLayout.Bands[0].Columns["Sex"].Width = cboSex.Width;
            cboSex.DisplayLayout.Bands[0].Columns["Sex"].Header.Caption = "Gender";
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

        private void gridEdu_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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
        private void fncGetGridEdu(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";
            foreach (UltraGridRow row in this.gridEdu.Rows)
            {
                sqlQuery =
                    "Insert Into tblApp_Edu (AppId,ExamName,ExamResult,MajorSub,BoardName,InstituteName,PassingYear,aId)" +
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

        private void fncGetGridTraining(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";
            foreach (UltraGridRow row in this.gridTraining.Rows)
            {
                sqlQuery =
                    "Insert Into tblApp_Training (AppId,CourseName,MajorSub,InstituteName, dtFrom,dtTo, Remarks,aId)" +
                    " Values (" + Int32.Parse(strId) + ", '" + row.Cells["CourseName"].Value.ToString() + "','" +
                    row.Cells["MajorSub"].Text.ToString()
                    + "','" + row.Cells["InstituteName"].Text.ToString() + "','" + row.Cells["dtFrom"].Value.ToString() +
                    "', '" + row.Cells["dtTo"].Value.ToString() + "','" +
                    row.Cells["Remarks"].Text.ToString()
                    + "', " + Int32.Parse(strId) + ")";
                arQuery.Add(sqlQuery);
            }
        }

        private void fncGetGridRefrence(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";
            foreach (UltraGridRow row in this.gridRef.Rows)
            {
                sqlQuery =
                    "Insert Into tblapplicants_ref (AppId,RefName,RefAdd,RefPhone, RefEmail,RefOrg, RefDesig, aId)" +
                    " Values (" + Int32.Parse(strId) + ", '" + row.Cells["RefName"].Value.ToString() + "','" +
                    row.Cells["RefAdd"].Text.ToString()
                    + "','" + row.Cells["RefPhone"].Text.ToString() + "','" + row.Cells["RefEmail"].Value.ToString() +
                    "', '" + row.Cells["RefOrg"].Value.ToString() + "','" +
                    row.Cells["RefDesig"].Text.ToString()
                    + "', " + Int32.Parse(strId) + ")";
                arQuery.Add(sqlQuery);

            }
        }

        private void fncGetgridExp(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";


            foreach (UltraGridRow row in this.gridExp.Rows)
            {
                sqlQuery = "Insert Into tblApp_Exp (AppId,PrevCom,PrevDesig,PrevSalary,Remarks,dtFrom,dtTo,aId)" +
                           " Values (" + Int32.Parse(strId) + ", '" + row.Cells["PrevCom"].Value.ToString() + "','" +
                           row.Cells["PrevDesig"].Text.ToString()
                           + "','" + row.Cells["PrevSalary"].Text.ToString() + "','" +
                           row.Cells["Remarks"].Text.ToString() + "','" + row.Cells["dtFrom"].Text.ToString() + "','" +
                           row.Cells["dtTo"].Text.ToString()
                           + "'," + Int32.Parse(strId) + ")";
                arQuery.Add(sqlQuery);
                //                   fncGetBarcodeData(ref arQuery,strId, row.Cells["Barcode"].Text.ToString());
            }
        }
        private void btnADDEdu_Click(object sender, EventArgs e)
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

        private void btnADD_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr;
                if (btnSave.Text == "&Save")
                {
                    dr = dsList.Tables["tblTraining"].NewRow();
                    dsList.Tables["tblTraining"].Rows.Add(dr);
                }
                else
                {
                    dr = dsDetails.Tables["Training"].NewRow();

                    dsDetails.Tables["Training"].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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

        private void txtFather_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtFather_KeyPress(object sender, KeyPressEventArgs e)
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
            //clsProc.GTRTabMove((Int16) e.KeyCode);
            //tabControl1.TabPages[1].
            tabControl1.SelectTab(1);
        }

        private void cboCurrDist_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboCurrPS_KeyDown(object sender, KeyEventArgs e)
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

        private void txtFather_Leave(object sender, EventArgs e)
        {
            txtMother.Text = txtMother.Text.TrimStart();
        }

        private void txtMohter_Leave(object sender, EventArgs e)
        {
            txtName.Text = txtName.Text.TrimStart();
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
            txtMother.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtMother.Text);
            //txtName.Focus();
            txtMother.SelectionStart = txtMother.Text.Length;
        }

        private void txtMohter_ValueChanged(object sender, EventArgs e)
        {
            txtName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtName.Text);
            //txtName.Focus();
            txtName.SelectionStart = txtName.Text.Length;
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

        private void frmApplicants_Load(object sender, EventArgs e)
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

        private void frmApplicants_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
        }

        private void cboCode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCode.DisplayLayout.Bands[0].Columns["AppId"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["App_Code"].Width = 140;
            cboCode.DisplayLayout.Bands[0].Columns["AppName"].Width = 140;
            cboCode.DisplayLayout.Bands[0].Columns["App_Code"].Header.Caption = "Applicants Code";
            cboCode.DisplayLayout.Bands[0].Columns["AppName"].Header.Caption = "Name";
        }

        private void cboNationality_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboNationality.DisplayLayout.Bands[0].Columns["countryId"].Hidden = true;

            cboNationality.DisplayLayout.Bands[0].Columns["countryName"].Width = cboNationality.Width;
            cboNationality.DisplayLayout.Bands[0].Columns["countryName"].Header.Caption = "Nationality";
            cboNationality.DisplayMember = "countryName";
            cboNationality.ValueMember = "countryId";
        }

        private void gridTraining_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                gridTraining.DisplayLayout.Bands[0].Columns["CourseName"].Header.Caption = "Course Name";
                gridTraining.DisplayLayout.Bands[0].Columns["MajorSub"].Header.Caption = "Major In";
                gridTraining.DisplayLayout.Bands[0].Columns["InstituteName"].Header.Caption = "Institute Name";
                gridTraining.DisplayLayout.Bands[0].Columns["dtFrom"].Header.Caption = "Date Form";
                gridTraining.DisplayLayout.Bands[0].Columns["dtTo"].Header.Caption = "Date To";
                gridTraining.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

                gridTraining.DisplayLayout.Bands[0].Columns["CourseName"].Width = 140;
                gridTraining.DisplayLayout.Bands[0].Columns["MajorSub"].Width = 100;
                gridTraining.DisplayLayout.Bands[0].Columns["InstituteName"].Width = 120;
                gridTraining.DisplayLayout.Bands[0].Columns["dtFrom"].Width = 100;
                gridTraining.DisplayLayout.Bands[0].Columns["dtTo"].Width = 100;
                gridTraining.DisplayLayout.Bands[0].Columns["Remarks"].Width = 120;


                gridTraining.DisplayLayout.Bands[0].Columns["dtFrom"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                gridTraining.DisplayLayout.Bands[0].Columns["dtTo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                //Change alternate color
                gridTraining.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridTraining.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridTraining.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridExp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridTraining.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

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

        private void btnExp_Click(object sender, EventArgs e)
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
                    dr = dsDetails.Tables["Experience"].NewRow();
                    dsDetails.Tables["Experience"].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr;
                if (btnSave.Text == "&Save")
                {
                    dr = dsList.Tables["tblReference"].NewRow();
                    dsList.Tables["tblReference"].Rows.Add(dr);
                }
                else
                {
                    dr = dsDetails.Tables["Reference"].NewRow();
                    dsDetails.Tables["Reference"].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridExp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

        private void gridRef_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                gridRef.DisplayLayout.Bands[0].Columns["RefName"].Header.Caption = "Name";
                gridRef.DisplayLayout.Bands[0].Columns["RefAdd"].Header.Caption = "Address";
                gridRef.DisplayLayout.Bands[0].Columns["RefPhone"].Header.Caption = "Phone";
                gridRef.DisplayLayout.Bands[0].Columns["RefEmail"].Header.Caption = "E-Mail";
                gridRef.DisplayLayout.Bands[0].Columns["RefOrg"].Header.Caption = "Organization";
                gridRef.DisplayLayout.Bands[0].Columns["RefDesig"].Header.Caption = "Designation";

                gridRef.DisplayLayout.Bands[0].Columns["RefName"].Width = 140;
                gridRef.DisplayLayout.Bands[0].Columns["RefAdd"].Width = 110;
                gridRef.DisplayLayout.Bands[0].Columns["RefPhone"].Width = 80;
                gridRef.DisplayLayout.Bands[0].Columns["RefEmail"].Width = 100;
                gridRef.DisplayLayout.Bands[0].Columns["RefOrg"].Width = 100;
                gridRef.DisplayLayout.Bands[0].Columns["RefDesig"].Width = 120;


                //gridRef.DisplayLayout.Bands[0].Columns["dtFrom"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                //gridRef.DisplayLayout.Bands[0].Columns["dtTo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                ////Change alternate color
                gridRef.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridRef.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridRef.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridExp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridRef.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

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

        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["JobType"].Width = cboRelegion.Width;
            cboType.DisplayLayout.Bands[0].Columns["JobType"].Header.Caption = "Job Type";
        }

        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void gridEdu_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void gridTraining_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
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

        private void gridExp_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void gridRef_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnUpload_Click_1(object sender, EventArgs e)
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

        private void cboApplyTo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboApplyTo.DisplayLayout.Bands[0].Columns["comId"].Hidden = true;
            cboApplyTo.DisplayLayout.Bands[0].Columns["comName"].Width = cboApplyTo.Width;
            cboApplyTo.DisplayLayout.Bands[0].Columns["comName"].Header.Caption = "Apply To Company";

            cboApplyTo.DisplayMember = "comName";
            cboApplyTo.ValueMember = "comId";
        }
    }
}


