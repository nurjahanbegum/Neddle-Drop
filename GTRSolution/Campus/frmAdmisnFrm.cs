using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Campus
{
    public partial class frmAdmisnFrm : Form
    {
        DataSet dsList2;
        DataSet dslist1;
        DataSet dslist3;
        DataSet dslist4;


        DataSet dsList;
        DataSet dsDetails;
        DataSet dsDetails1;
        DataSet dsDetails2;
        DataSet dsDetailsENR;

        
        public int RefID;
       // private DataTable dt;
        clsProcedure clsProc = new clsProcedure();
        private clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmAdmisnFrm(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmAdmisnRestar_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            clsProc = null;
        }



        private void ChangeEvent()
        {
            
            try
            {
                if (cboClas.Text.Length > 0)
                    cbofrmNm.Value = "";

                if (cbofrmNm.Text.Length > 0)
                    cboClas.Value = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void PrcLoadList2(int Template)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsList2 = new DataSet();
            string sqlQuery;
            try
            {
                sqlQuery = "Exec prcgetBasiAddmsn 0," + Common.Classes.clsMain.intComId + ",'', "+ Template +"";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList2, sqlQuery);
                dsList2.Tables[0].TableName = "GridData";
                dsList2.Tables[1].TableName = "Class";
                dsList2.Tables[2].TableName = "LoadFormNm";
                dsList2.Tables[3].TableName = "ForMGrid";
                dsList2.Tables[4].TableName = "YarNm";
                dsList2.Tables[5].TableName = "Result";
                dsList2.Tables[6].TableName = "FormNo";
                dsList2.Tables[7].TableName = "Religion";
                dsList2.Tables[8].TableName = "Pic";
                dsList2.Tables[9].TableName = "PicBy";
                dsList2.Tables[10].TableName = "Sessn";
                dsList2.Tables[11].TableName = "FeeTemp";
                dsList2.Tables[12].TableName = "EnrData";
                dsList2.Tables[13].TableName = "Templateid";
                dsList2.Tables[14].TableName = "Templatewith";
                dsList2.Tables[15].TableName = "paysegment";
                dsList2.Tables[16].TableName = "fee";


                uddFeeTp.DataSource = null;
                uddFeeTp.DataSource = dsList2.Tables["fee"];
                txtRegNo1.DisplayLayout.Bands[0].Columns["Headname"].ValueList = uddFeeTp;

                //Segment
                uddPaySeg.DataSource = null;
                uddPaySeg.DataSource = dsList2.Tables["paysegment"];
                txtRegNo1.DisplayLayout.Bands[0].Columns["Segname"].ValueList = uddPaySeg;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void PrcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsList = new DataSet();
            string sqlQuery;
            try
            {
                sqlQuery = "Exec prcgetBasiAddmsn 0," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "GridData";
                dsList.Tables[1].TableName = "Class";
                dsList.Tables[2].TableName = "LoadFormNm";
                dsList.Tables[3].TableName = "ForMGrid";
                dsList.Tables[4].TableName = "YarNm";
                dsList.Tables[5].TableName = "Result";
                dsList.Tables[6].TableName = "FormNo";
                dsList.Tables[7].TableName = "Religion";
                dsList.Tables[8].TableName = "Pic";
                dsList.Tables[9].TableName = "PicBy";
                dsList.Tables[10].TableName = "Sessn";
                dsList.Tables[11].TableName = "FeeTemp";
                dsList.Tables[12].TableName = "EnrData";
                dsList.Tables[13].TableName = "TranHead";
                dsList.Tables[14].TableName = "paysegment";
                dsList.Tables[15].TableName = "sex";


                


                DataRow dr;
                if (dsList.Tables["YarNm"].Rows.Count > 0)
                {
                    dr = dsList.Tables["YarNm"].Rows[0];

                    this.txtSesion.Text = dr["SesYear"].ToString();
                    //this.txtShipVatPr.Text = dr["vatper"].ToString();


                }



                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["GridData"];

                gridFrmRcv.DataSource = null;
                gridFrmRcv.DataSource = dsList.Tables["ForMGrid"];

                gridResult.DataSource = null;
                gridResult.DataSource = dsList.Tables["Result"];

                dsList.Tables["Pic"].Columns.Add("Pic", typeof (Bitmap));
                gridPics.DataSource = null;
                gridPics.DataSource = dsList.Tables["Pic"];

                uddPicBy.DataSource = null;
                uddPicBy.DataSource = dsList.Tables["PicBy"];
                gridPics.DisplayLayout.Bands[0].Columns["PicBy"].ValueList = uddPicBy;

                txtRegNo1.DataSource = null;
                txtRegNo1.DataSource = dsList.Tables["FeeTemp"];
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void PrcLoadCombo()
        {       
            cboAdmisnFor.DataSource = null;
            cboAdmisnFor.DataSource = dsList.Tables["Class"];

            txtAdmsnfor.DataSource = null;
            txtAdmsnfor.DataSource = dsList.Tables["Class"];

            cbofrmNm.DataSource = null;
            cbofrmNm.DataSource = dsList.Tables["LoadFormNm"];

            cboClas.DataSource = null;
            cboClas.DataSource = dsList.Tables["Class"];
            if (cboClas.Rows.Count > 0)
            {
                cboClas.Value = cboClas.Rows[0].Cells["clsId"].Value.ToString();
                //cboDeskLook.Enabled = false;
            }

            cboClassE.DataSource = null;
            cboClassE.DataSource = dsList.Tables["Class"];


            cboYear.DataSource = null;
            cboYear.DataSource = dsList.Tables["Sessn"];

            cboyrnm.DataSource = null;
            cboyrnm.DataSource = dsList.Tables["Sessn"];

            if (cboYear.Rows.Count > 0)
            {
                cboYear.Value = cboYear.Rows[0].Cells["sesn"].Value.ToString();
                //cboDeskLook.Enabled = false;
            }

            cboClsnm.DataSource = null;
            cboClsnm.DataSource = dsList.Tables["Class"];

            cbofrmNo.DataSource = null;
            cbofrmNo.DataSource = dsList.Tables["FormNo"];

            cboRlgn.DataSource = null;
            cboRlgn.DataSource = dsList.Tables["Religion"];

            cboFromSsn.DataSource = null;
            cboFromSsn.DataSource = dsList.Tables["Sessn"];

            cboToSsn.DataSource = null;
            cboToSsn.DataSource = dsList.Tables["Sessn"];

            cboRegNo.DataSource = null;
            cboRegNo.DataSource = dsList.Tables["EnrData"];


            cboReligion.DataSource = null;
            cboReligion.DataSource = dsList.Tables["Religion"];


            cboSex.DataSource = null;
            cboSex.DataSource = dsList.Tables["sex"];

            //cboTempLateID.DataSource = null;
            //cboTempLateID.DataSource = dsList.Tables["Templateid"];

        }

        private void txtNm_Click(object sender, EventArgs e)
        {
            //txtNm.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNm_Enter(object sender, EventArgs e)
        {
            if (txtNm.ForeColor != Color.Black)
            {
                txtNm.Text = "";
            }
        }

        private void txtNm_Leave(object sender, EventArgs e)
        {
            if (txtNm.Text.Length <= 0)
            {
                txtNm.Text = "First Name";
                txtNm.ForeColor = Color.Gray;
            }
        }



        private void txtNm_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtNm.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }



        private void txtMn_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtMn.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }



        private void txtLn_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtLn.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }



        private void txtMn_Enter(object sender, EventArgs e)
        {
            if (txtMn.ForeColor != Color.Black)
            {
                txtMn.Text = "";
            }
        }



        private void txtMn_Leave(object sender, EventArgs e)
        {
            if (txtMn.Text.Length <= 0)
            {
                txtMn.Text = "Middle Name";
                txtMn.ForeColor = Color.Gray;
            }
        }



        private void txtLn_Leave(object sender, EventArgs e)
        {
            if (txtLn.Text.Length <= 0)
            {
                txtLn.Text = "Last Name";
                txtLn.ForeColor = Color.Gray;
            }
        }



        private void txtLn_Enter(object sender, EventArgs e)
        {
            if (txtLn.ForeColor != Color.Black)
            {
                txtLn.Text = "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PrcCleraData();

        }
        private void PrcCleraData()
        {
            this.txtNm.Text = "First Name";
            this.txtMn.Text = "Middle Name";
            this.txtLn.Text = "Last Name";
            txtSesion.Text = null;
            txtAdmsnID.Text = null;
            txtMotherNm.Text = null;
            txtFatherNm.Text = null;
            txtFormNo.Text = null;
            txtAddress.Text = null;
            dtAdmsn.Value = DateTime.Now;

            this.cboSex.Text = "";
            this.cboReligion.Text = "";
            this.txtPhone.Text = "";
            this.txtMobile.Text = "";

            this.txtAmt.Text = "";
            this.txtReferrance.Text = "";
            this.txtFrmRemarks.Text = "";
            this.txtSlNo.Text = "";



            this.txtNm.ForeColor = Color.Gray;
            this.txtMn.ForeColor = Color.Gray;
            this.txtLn.ForeColor = Color.Gray;

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
            cboAdmisnFor.Text = null;
        }

        private void cboAdmisnFor_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboAdmisnFor.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            cboAdmisnFor.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Admission For";
            cboAdmisnFor.DisplayLayout.Bands[0].Columns["clsName"].Width = cboAdmisnFor.Width;
            cboAdmisnFor.DisplayMember = "clsName";
            cboAdmisnFor.ValueMember = "clsId";

        }

        private void frmAdmisnRestar_Load(object sender, EventArgs e)
        {
            try
            {
                PrcLoadList();
                PrcLoadCombo();
                dtAdmsn.Value = DateTime.Today;
               // txtYear.Text = DateTime.Now.Year.ToString();
                
               
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridList.DisplayLayout.Bands[0].Columns["admid"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Name";
                gridList.DisplayLayout.Bands[0].Columns["Name"].Width = 220;
                gridList.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Class";
                gridList.DisplayLayout.Bands[0].Columns["clsName"].Width = 80;
                gridList.DisplayLayout.Bands[0].Columns["nmFather"].Header.Caption = "Father Name";
                gridList.DisplayLayout.Bands[0].Columns["nmFather"].Width = 170;
                gridList.DisplayLayout.Bands[0].Columns["nmMother"].Header.Caption = "Mother Name";
                gridList.DisplayLayout.Bands[0].Columns["nmMother"].Width = 160;
                gridList.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
                gridList.DisplayLayout.Bands[0].Columns["sesn"].Width = 70;
                //Change alternate color
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

                //Use Filtering
                this.gridList.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private  Boolean fncBlankA()
        {
            if (txtNm.ForeColor!=Color.Black)
            {
                MessageBox.Show("Please Provide First Name");
                txtNm.Focus();
                return true;
            }
            if (txtFatherNm.Text.Length==0)
            {
                MessageBox.Show("Please Provide Father Name");
                txtFatherNm.Focus();
                return true;
            }
            if (txtMotherNm.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Mother Name");
                txtMotherNm.Focus();
                return true;
            }
            if (cboAdmisnFor.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Addmisson For");
                cboAdmisnFor.Focus();
                return true;
            }
            if (txtSesion.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Session");
                txtSesion.Focus();
                return true;
            }
            return false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if(fncBlankA()==true)
                return;

            
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsList = new DataSet();
            int NewID,slno;
            string AdmID;
            string sqlQuery;
            try
            {
                if (txtAdmsnID.Text.Length > 0)
                {
                    sqlQuery = "update  tblAdm_Form set frmNo='" + txtFormNo.Text.ToString() + "', nmFirst='" + txtNm.Text + "', nmMiddle='" + txtMn.Text
                        + "', nmLast='" + txtLn.Text.Trim() + "', nmFather='" + txtFatherNm.Text.Trim() + "', nmMother='" + txtMotherNm.Text.Trim()
                        + "', sesn='" + txtSesion.Text.Trim() + "',frmTakenDt='" + clsProc.GTRDate(dtAdmsn.Value.ToString()) + "',clsid='" + cboAdmisnFor.Value.ToString()
                        + "', sex = '" + cboSex.Text.Trim() + "', relegion = '" + cboReligion.Text.Trim() + "',mobile = '" + txtMobile.Text.Trim() + "',Phone = '" + txtPhone.Text.Trim()
                        + "', amount = " + txtAmt.Text.Trim() + ",Reference = '" + txtReferrance.Text.Trim() + "',remarks = '" + txtFrmRemarks.Text.Trim() + "' " +
                        " where admid='"+txtAdmsnID.Text+"'";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                         + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','UPDATE')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Update Successfully");
                }
                else
                {
                    NewID = 0;
                    slno = 0;

                    //Select ISNULL(max((right(frmNoAuto,6))),100000 )+1 as NewID from tblAdm_Form
                    sqlQuery = "Select ISNULL(max(frmNoAuto),100000 )+1 as NewID from tblAdm_Form";
                    AdmID = clsCon.GTRCountingData(sqlQuery).ToString();
                    
                   // string addata =AdmID.ToString().Substring(4, 6);
                    sqlQuery = "Select convert(int,ISNULL(max(AdmId),0 )+1) as NewID from tblAdm_Form";
                    NewID = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Select ISNULL(max(slno),0 )+1 as slno from tblAdm_Form where sesn = '"+ txtSession.Text +"'";
                    slno = clsCon.GTRCountingData(sqlQuery);

                    //sqlQuery = "Select ISNULL(max(frmNoAuto),100000 )+1 as NewID from tblAdm_Form";
                    //AdmID = clsCon.GTRCountingData(sqlQuery).ToString();
                    //string addata =AdmID.ToString();
                    //sqlQuery = "Select convert(int,ISNULL(max(AdmId),0 )+1) as NewID from tblAdm_Form";




                    sqlQuery = "Insert Into tblAdm_Form (sesn,AdmId,slno, frmNo, frmNoAuto,nmFirst, nmMiddle, nmLast, nmFather, nmMother, clsid, yrNm,frmTakenDt,Address,sex,relegion,phone,mobile,amount,Reference,remarks)";
                    sqlQuery = sqlQuery + "values('" + txtSesion.Text.Trim() + "'," + NewID + "," + slno + ",'" + txtFormNo.Text.Trim() + "'," + AdmID + ",'" + txtNm.Text.Trim() + "','" + txtMn.Text.Trim() + "','" + txtLn.Text.Trim() + "','" + txtFatherNm.Text.Trim() + "','" + txtMotherNm.Text.Trim()
                        + "','" + cboAdmisnFor.Value.ToString() + "','" + txtSesion.Text.Trim() + "','" + clsProc.GTRDate(dtAdmsn.Value.ToString()) + "','" + txtAddress.Text + "','" + cboSex.Text + "','" + cboReligion.Text + "','" + txtPhone.Text + "','" + txtMobile.Text + "','" + txtAmt.Text + "','" + txtReferrance.Text + "','" + txtRemarks.Text + "')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Saved Successfully");
                
                }
                PrcCleraData();
                PrcLoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
                arQuery = null;
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            PrcCleraData();
            //dtAdmsn.Value = null;
            txtNm.ForeColor = Color.Black;
            txtMn.ForeColor = Color.Black;
            txtLn.ForeColor = Color.Black;
            //txtYear.Text = null;
            prcDisplayDetails(gridList.ActiveRow.Cells["AdmId"].Value.ToString());
        }
       
        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");

            string sqlQuery = "Exec prcgetBasiAddmsn " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId + ",''";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "AdmsnInfo";

            DataRow dr;
            if (dsDetails.Tables["AdmsnInfo"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["AdmsnInfo"].Rows[0];

                this.txtAdmsnID.Text = dr["admId"].ToString();
                this.dtAdmsn.Value = dr["frmTakenDt"].ToString();
                this.txtFormNo.Text = dr["frmNo"].ToString();
                this.txtNm.Text = dr["nmFirst"].ToString();
                this.txtMn.Text = dr["nmMiddle"].ToString();
                this.txtLn.Text = dr["nmLast"].ToString();
                this.txtFatherNm.Text = dr["nmFather"].ToString();
                this.txtMotherNm.Text = dr["nmMother"].ToString();
                this.cboAdmisnFor.Text = dr["clsid"].ToString();
                this.txtAddress.Text = dr["SecName"].ToString();
                this.txtSesion.Text = dr["sesn"].ToString();


                this.txtAmt.Text = dr["Amount"].ToString();
                this.txtReferrance.Text = dr["Reference"].ToString();
                this.txtFrmRemarks.Text = dr["remarks"].ToString();
                this.txtSlNo.Text = dr["slno"].ToString();
                this.cboSex.Text = dr["sex"].ToString();
                this.cboReligion.Text = dr["relegion"].ToString();
                this.txtMobile.Text = dr["mobile"].ToString();
                this.txtPhone.Text = dr["Phone"].ToString();

                this.txtAddress.Text = dr["Address"].ToString();
                this.cboReligion.Text = dr["relegion"].ToString();
                this.txtRemarks.Text = dr["remarks"].ToString();



                //this.txtYear.Text = dr["yrNm"].ToString();
                
                this.btnDelete.Enabled = true;
                this.btnSave.Text = "&Update";
            }
        }

        private void prcDisplayDetailsTemplate(string strParam)
        {
            dsDetails2 = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");

            
            string sqlQuery = "Exec prcgetTemplateID " + Common.Classes.clsMain.intComId + ",''," + Int32.Parse(strParam) + "";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails2, sqlQuery);
            if (dsDetails2 == null)
            {
                return;
            }
            else

            dsDetails2.Tables[0].TableName = "templateDetails";
            dsDetails2.Tables[1].TableName = "payseg";
            dsDetails2.Tables[2].TableName = "tranhead";
        }


        private void prcDisplayDetailsENR(string strParam, string strParam2)
        {
            dsDetailsENR = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");



            txtRollNo.Text = cboRegNo.ActiveRow.Cells["RollNo"].Text.ToString();
            txtName.Text = cboRegNo.ActiveRow.Cells["FullName"].Text.ToString();

            txtFatherNameE.Text = cboRegNo.ActiveRow.Cells["FatherName"].Value.ToString();
            cboClassE.Text = cboRegNo.ActiveRow.Cells["CurrCls"].Value.ToString();
            txtSession.Text = cboRegNo.ActiveRow.Cells["sesn"].Value.ToString();



            string sqlQuery = "Exec [prcEnrDetails] " + Common.Classes.clsMain.intComId + ", " + Int32.Parse(strParam) + ",'ENR'";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetailsENR, sqlQuery);
            dsDetailsENR.Tables[0].TableName = "tblENR";
            dsDetailsENR.Tables[1].TableName = "payseg";
            dsDetailsENR.Tables[2].TableName = "tranhead";
            dsDetailsENR.Tables[3].TableName = "tblFeeMGT";

            uddPaySeg.DataSource = null;
            uddPaySeg.DataSource = dsDetailsENR.Tables["payseg"];

            uddFeeTp.DataSource = null;
            uddFeeTp.DataSource = dsDetailsENR.Tables["tranhead"];

            txtRegNo1.DataSource = null;
            txtRegNo1.DataSource = dsDetailsENR.Tables["tblFeeMGT"];



            DataRow dr;
            if (dsDetailsENR.Tables["tblENR"].Rows.Count > 0)
            {
                dr = dsDetailsENR.Tables["tblENR"].Rows[0];
                {
                    txtEnrollID.Text = dr["vEnrlID"].ToString();
                    txtRollNo.Text = dr["RollNo"].ToString();


                    txtRegNo1.DataSource = null;
                    txtRegNo1.DataSource = dsDetailsENR.Tables["tblFeeMGT"];

                }

                if (cboRegNo.Text.Length == 0)
                {
                    this.btnSaveEnr.Text = "&Save";
                    //this.btnDlt.Enabled = false;
                }

                else
                {
                    this.btnSaveEnr.Text = "&Update";
                    //this.btnDlt.Enabled = true;
                }
            }
            else
            //cboTempLateID.DataSource = 0;
            //cboTempLateID.Text = "";

            if (dsDetailsENR.Tables["tblFeeMGT"].Rows.Count > 0)
            {
                dr = dsDetailsENR.Tables["tblFeeMGT"].Rows[0];
                {
                    cboTempLateID.Value = dr["feemgtid"].ToString();
                }
            }
        }

        private void prcDisplayDetailsReg(string strParam, string strParam2)
        {
            dsDetails = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");

            string sqlQuery = "Exec prcgetBasiAddmsn " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId + ",'REG'";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "AdmsnInfo";
            dsDetails.Tables[1].TableName = "Pic";
            //dsDetails.Tables[2].TableName = "Template";

            dsDetails.Tables["Pic"].Columns.Add("Pic", typeof(Bitmap));


            try
            {

                DataRow dr2;
                if (dsDetails.Tables["Pic"].Rows.Count > 0)
                {
                    //dsList.Tables["Pic"].Columns.Add("Pic", typeof(Bitmap));
                    gridPics.DataSource = null;
                    gridPics.DataSource = dsList.Tables["Pic"];

                    foreach (DataRow drpic in dsDetails.Tables["Pic"].Rows)
                    {
                        dr2 = dsList.Tables["Pic"].NewRow();
                        string strTarget = Common.Classes.clsMain.strPicPathCmps + @"\" + drpic["PicName"].ToString();
                        string STr = Common.Classes.clsMain.strPicPathCmps + @"\Temp\" + drpic["PicName"].ToString();
                        //File.Delete(STr);
                        if (File.Exists(STr) == false)
                        {
                            File.Copy(strTarget, STr, false);
                        }
                        dr2 = dsList.Tables["Pic"].NewRow();
                        dr2["PicBy"] = drpic["PicBy"].ToString();
                        dr2["picname"] = drpic["picname"].ToString();
                        // dr2["pict"] = drpic["picname"].ToString();
                        // Attachment at=new Attachment(@"D:\Work\Another One solution\GTRSolution\gt\Com\pics\CMPS\" + drpic["PicName"].ToString());
                        dr2["Pic"] = new Bitmap(Common.Classes.clsMain.strPicPathCmps + @"\Temp\" + drpic["PicName"].ToString());
                        dsList.Tables["Pic"].Rows.Add(dr2);
                    }
                }
                else
                {
                    dsList.Tables["Pic"].Rows.Clear();

                    //dsList.Tables["Pic"].Columns.Add("Pic", typeof(Bitmap));
                    //gridPics.DataSource = null;
                    //gridPics.DataSource = dsList.Tables["Pic"];

                    //gridPics.DataSource = null;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            DataRow dr;
            if (dsDetails.Tables["AdmsnInfo"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["AdmsnInfo"].Rows[0];
                if (dr["Src"].ToString() == "ADMEXAM")
                {
                    txtfnm.Text = cbofrmNo.ActiveRow.Cells["nmFirst"].Text.ToString();
                    txtfnm.ForeColor = System.Drawing.Color.Black;
                    txtmdlnm.Text = cbofrmNo.ActiveRow.Cells["nmMiddle"].Text.ToString();
                    txtmdlnm.ForeColor = System.Drawing.Color.Black;
                    txtlnm.Text = cbofrmNo.ActiveRow.Cells["nmLast"].Text.ToString();
                    txtlnm.ForeColor = System.Drawing.Color.Black;
                    txtfanm.Text = cbofrmNo.ActiveRow.Cells["nmFather"].Text.ToString();
                    txtMoNm.Text = cbofrmNo.ActiveRow.Cells["nmMother"].Text.ToString();
                    txtRegID.Tag = dr["admid"].ToString();
                    txtAdmsnfor.Text = dr["clsid"].ToString();


               }


                else
                {
                    //a.RegId, a.regno, a.AdmId,a.RollNo, a.nmPreFx, A.FNm, A.MNm, A.LNm,A.nmPostFx,A.FatherName,A.MotherName,A.cAdd,cCity,cDist,A.pAdd,A.pCity,pDist,
		            //A.DOB,A.Relgn,A.Cst,A.BGrp,A.FDesig,     A.FJob,     A.AvgMnIncome,A.RelDt,A.CurrCls,A.CurrSec,A.Sesn,A.Rmks,A.Sts,A.aID
                    
                    txtfnm.Text = cbofrmNo.ActiveRow.Cells["nmFirst"].Text.ToString();
                    txtfnm.ForeColor = System.Drawing.Color.Black;
                    txtmdlnm.Text = cbofrmNo.ActiveRow.Cells["nmMiddle"].Text.ToString();
                    txtmdlnm.ForeColor = System.Drawing.Color.Black;
                    txtlnm.Text = cbofrmNo.ActiveRow.Cells["nmLast"].Text.ToString();
                    txtlnm.ForeColor = System.Drawing.Color.Black;
                    txtfanm.Text = cbofrmNo.ActiveRow.Cells["nmFather"].Text.ToString();
                    txtMoNm.Text = cbofrmNo.ActiveRow.Cells["nmMother"].Text.ToString();
                    txtCA.Text = dr["cAdd"].ToString();
                    txtCD.Text = dr["cCity"].ToString();
                    txtCC.Text = dr["cDist"].ToString();
                    txtPA.Text = dr["pAdd"].ToString();
                    txtPD.Text = dr["pCity"].ToString();
                    txtPC.Text = dr["pDist"].ToString();
                    dtDOB.Value = dr["DOB"].ToString();
                    txtCust.Text = dr["Cst"].ToString();
                    cboRlgn.Text = dr["Relgn"].ToString();
                    txtBG.Text = dr["BGrp"].ToString();
                    txtAdmsnfor.Text = dr["CurrCls"].ToString();
                    txtSec.Text = dr["CurrSec"].ToString();
                    txtRegNo.Text = dr["regno"].ToString();
                    txtRoll.Text = dr["RollNo"].ToString();
                    txtssn.Text = dr["Sesn"].ToString();
                    txtFO.Text = dr["FJob"].ToString();
                    txtFD.Text = dr["FDesig"].ToString();
                    txtMI.Text = dr["AvgMnIncome"].ToString();
                    txtsts.Text = dr["Sts"].ToString();
                    txtRmrks.Text = dr["Rmks"].ToString();
                    txtRegID.Tag = dr["admid"].ToString();
                    txtRegID.Text = dr["RegId"].ToString();

                    //gridPics.DataSource = null;


                    //gridPics.DisplayLayout.Bands[0].Columns["Pic"] = dr["picname"].ToString();
                    //gridPics.DisplayLayout.Bands[0].Columns["Pic"] = dr["picby"].ToString();
                }

                if (txtRegID.Text.Length == 0)
                {
                    this.btnSaveR.Text = "&Save";
                    this.btnDlt.Enabled = false;
                }
                
                else
                {
                    this.btnSaveR.Text = "&Update";
                    this.btnDlt.Enabled = true;
                }

            }
        }

        private void dtAdmsn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFormNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFatherNm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMotherNm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboAdmisnFor_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSesion_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtYear_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtSesion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboAdmisnFor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMotherNm_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtFatherNm_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtFormNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void dtAdmsn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete Addmisson  information of [" + txtNm.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {
                string sqlQuery = "Delete tblAdm_Form where AdmID='" + txtAdmsnID.Text.ToString() + "'";
                arQuery.Add(sqlQuery);

                sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                PrcCleraData();
                PrcLoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridFrmRcv_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            try
            {
                gridFrmRcv.DisplayLayout.Bands[0].Columns["admID"].Hidden = true;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["frmNoAuto"].Header.Caption = "Form No";
                gridFrmRcv.DisplayLayout.Bands[0].Columns["frmNoAuto"].Width = 100;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Name";
                gridFrmRcv.DisplayLayout.Bands[0].Columns["Name"].Width = 240;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["nmFather"].Header.Caption = "Father Name";
                gridFrmRcv.DisplayLayout.Bands[0].Columns["nmFather"].Width = 230;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Class";
                gridFrmRcv.DisplayLayout.Bands[0].Columns["clsName"].Width = 100;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
                gridFrmRcv.DisplayLayout.Bands[0].Columns["sesn"].Width = 90;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["RcvYN"].Header.Caption = "ReceiveYN";
                gridFrmRcv.DisplayLayout.Bands[0].Columns["RcvYN"].Width = 90;

                //Cell Style
                gridFrmRcv.DisplayLayout.Bands[0].Columns["RcvYN"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["RcvYN"].CellActivation = Activation.AllowEdit;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["sesn"].CellActivation = Activation.NoEdit;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["clsName"].CellActivation = Activation.NoEdit;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["nmFather"].CellActivation = Activation.NoEdit;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["Name"].CellActivation = Activation.NoEdit;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["frmNoAuto"].CellActivation = Activation.NoEdit;
                gridFrmRcv.DisplayLayout.Bands[0].Columns["secname"].CellActivation = Activation.NoEdit;

                //Change alternate color
                this.gridFrmRcv.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                this.gridFrmRcv.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridFrmRcv.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridFrmRcv.DisplayLayout.Override.AllowUpdate= DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridFrmRcv.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                this.gridFrmRcv.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbofrmNm_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cbofrmNm.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cbofrmNm.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Form No";
            cbofrmNm.DisplayLayout.Bands[0].Columns[1].Width = cbofrmNm.Width;
            cbofrmNm.DisplayMember = "frmNoAuto";
            cbofrmNm.ValueMember = "admID";
            cbofrmNm.Value = 1;

        }
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            chkAll.Tag = 0;
            if (chkAll.Checked == true)
            {
                chkAll.Tag = 1;
                cbofrmNm.Text = null;
            }
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            if (fncBlank() == true)
                return;
            dslist3 = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {



                string sqlQuery = "Exec prcGetfrmRcv '" + cbofrmNm.Value + "','" + chkAll.Tag + "','" + cboYear.Value.ToString() + "','" + cboClas.Value + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dslist3, sqlQuery);
                dslist3.Tables[0].TableName = "FormData";

                gridFrmRcv.DataSource = null;
                gridFrmRcv.DataSource = dslist3.Tables["FormData"];

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



        private void btnReceive_Click(object sender, EventArgs e)
        {

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery;
            try
            {
                foreach (UltraGridRow row in gridFrmRcv.Rows)
                {
                    sqlQuery = "Update tblAdm_Form set IsReceive='" + row.Cells["RcvYN"].Value + "' where admID='" + row.Cells["admID"].Value + "' ";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Receive')";
                    arQuery.Add(sqlQuery);
                }
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                MessageBox.Show("Receive Form Successfully");
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        private void prcClrData()
        {
            cboYear.Text = null;
            cbofrmNm.Text = null;
            cboClas.Text = null;
            chkAll.Checked = false;
        }

        private void ultraButton4_Click(object sender, EventArgs e)
        {
            PrcLoadList();
            prcClrData();
        }

        private void cboClas_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboClas.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            cboClas.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Admission For";
            cboClas.DisplayLayout.Bands[0].Columns["clsName"].Width = cboClas.Width;
            cboClas.DisplayMember = "clsName";
            cboClas.ValueMember = "clsId";
            //cboClas.Value = 1;
        }

        private void cboYear_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboYear.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Year";
            cboYear.DisplayLayout.Bands[0].Columns["sesn"].Width = cboYear.Width;
        }
        private Boolean fncBlank()
        {
            if (cboYear.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Addmisson Year");

                cboYear.Focus();
                return true;
            }
            return false;
        }
        private Boolean fncBlankRe()
        {
            if (cboyrnm.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Addmisson Year");

                cboyrnm.Focus();
                return true;
            }
            if (cboClsnm.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Class");

                cboClsnm.Focus();
                return true;
            }
            return false;
        }

        private void gridResult_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridResult.DisplayLayout.Bands[0].Columns["AdmID"].Hidden = true;
                gridResult.DisplayLayout.Bands[0].Columns["frmNoAuto"].Header.Caption = "Form No";
                gridResult.DisplayLayout.Bands[0].Columns["frmNoAuto"].Width = 100;
                gridResult.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Name";
                gridResult.DisplayLayout.Bands[0].Columns["Name"].Width = 240;
                gridResult.DisplayLayout.Bands[0].Columns["nmFather"].Header.Caption = "Father Name";
                gridResult.DisplayLayout.Bands[0].Columns["nmFather"].Width = 230;
                gridResult.DisplayLayout.Bands[0].Columns["Secname"].Header.Caption = "Secname";
                gridResult.DisplayLayout.Bands[0].Columns["Secname"].Width = 230;
                gridResult.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Class";
                gridResult.DisplayLayout.Bands[0].Columns["clsName"].Width = 100;
                gridResult.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
                gridResult.DisplayLayout.Bands[0].Columns["sesn"].Width = 90;
                gridResult.DisplayLayout.Bands[0].Columns["Number"].Header.Caption = "Number";
                gridResult.DisplayLayout.Bands[0].Columns["Number"].Width = 120;
                gridResult.DisplayLayout.Bands[0].Columns["Ispassed"].Header.Caption = "PassedYN";
                //gridFrmRcv.DisplayLayout.Bands[0].Columns["Ispassed"].Width = 90;

                //Cell Style
                gridResult.DisplayLayout.Bands[0].Columns["Ispassed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridResult.DisplayLayout.Bands[0].Columns["Ispassed"].CellActivation = Activation.AllowEdit;
                gridResult.DisplayLayout.Bands[0].Columns["Number"].CellActivation = Activation.AllowEdit;
                gridResult.DisplayLayout.Bands[0].Columns["sesn"].CellActivation = Activation.NoEdit;
                gridResult.DisplayLayout.Bands[0].Columns["clsName"].CellActivation = Activation.NoEdit;
                gridResult.DisplayLayout.Bands[0].Columns["nmFather"].CellActivation = Activation.NoEdit;
                gridResult.DisplayLayout.Bands[0].Columns["Name"].CellActivation = Activation.NoEdit;
                gridResult.DisplayLayout.Bands[0].Columns["frmNoAuto"].CellActivation = Activation.NoEdit;
                gridResult.DisplayLayout.Bands[0].Columns["Secname"].CellActivation = Activation.NoEdit;
                //Change alternate color
                this.gridResult.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                this.gridResult.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridResult.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridFrmRcv.DisplayLayout.Override.AllowUpdate= DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridResult.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                this.gridResult.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ultraButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboyrnm_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboyrnm.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Year";
            cboyrnm.DisplayLayout.Bands[0].Columns["sesn"].Width = cboYear.Width;
        }

        private void cboClsnm_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboClsnm.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            cboClsnm.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Admission For";
            cboClsnm.DisplayLayout.Bands[0].Columns["clsName"].Width = cboClas.Width;
            cboClsnm.DisplayMember = "clsName";
            cboClsnm.ValueMember = "clsID";
        }
        
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            if (fncBlankRe() == true)
                return;
            dslist4 = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {
                string sqlQuery = "Exec prcGetfrmResult '" + cboyrnm.Value.ToString() + "'," + cboClsnm.Value + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dslist4, sqlQuery);
                dslist4.Tables[0].TableName = "Result";
                dslist4.Tables[1].TableName = "nm";
                gridResult.DataSource = null;
                gridResult.DataSource = dslist4.Tables["Result"];

                cbo.DataSource = null;
                cbo.DataSource = dslist4.Tables["nm"];
                cbo.ValueMember = "id";
                cbo.DisplayMember = "number";
                cbo.Value =1;


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

        private void btnRsltSave_Click(object sender, EventArgs e)
        {
            string sqlQuery;
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {
             
                Int32 hNmbr = 0;
                sqlQuery = "Delete tblAdm_Exam_Result where clsid='" + cboClsnm.Value.ToString() + "' and Yrnm='" + cboyrnm.Text.ToString() + "'";
                arQuery.Add(sqlQuery);
                foreach (UltraGridRow row in gridResult.Rows)
                {
                    Int32 i = 0;
                    if (row.Cells["Number"].Text.Length > 0)
                    {
                        i = Int32.Parse(row.Cells["Number"].Text.ToString());

                        if (i > Int32.Parse(cbo.Text.ToString()))
                        {
                            hNmbr = 1;
                        }
                    }
                    sqlQuery = "Insert Into tblAdm_Exam_Result (AdmID, FormNo, clsID, Yrnm, IsPassed, Number,IsMainList)"
                    + "values('" + row.Cells["AdmID"].Value + "','" + row.Cells["frmNoAuto"].Value + "','" + cboClsnm.Value.ToString()
                    + "','" + cboyrnm.Text.ToString() + "','" + row.Cells["IsPassed"].Value + "','" + row.Cells["Number"].Value + "',"+hNmbr+")";
                    arQuery.Add(sqlQuery);
                }

                sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Result')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                MessageBox.Show("Data Save SuccessFully");
                PrcClrrData();
                PrcLoadList();
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                clsCon = null;
                arQuery = null;
            }
        }

        private void cboyrnm_Validating(object sender, CancelEventArgs e)
        {
            if (cboyrnm.Text.Length > 0)
            {
                if (cboyrnm.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide a Valid Data [or Select List]");
                    cboyrnm.Focus();
                }
            }
        }

        private void cboClsnm_Validating(object sender, CancelEventArgs e)
        {
            if (cboClsnm.Text.Length > 0)
            {
                if (cboClsnm.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide a Valid Data [or Select List]");
                    cboClsnm.Focus();
                }
            }
        }
        private void PrcClrrData()
        {
            cboyrnm.Text = null;
            cboClsnm.Text = null;
        }

        private void ultraButton5_Click(object sender, EventArgs e)
        {
            PrcClrrData();
            PrcLoadList();
        }

        private void txtfnm_Enter(object sender, EventArgs e)
        {
            //if (txtfnm.ForeColor != Color.Black)
            //{
            //    txtfnm.Text = "";
            //}
        }

        private void txtmdlnm_Enter(object sender, EventArgs e)
        {
            //if (txtmdlnm.ForeColor != Color.Black)
            //{
            //    txtmdlnm.Text = "";
            //}
        }

        private void txtlnm_Enter(object sender, EventArgs e)
        {
            //if (txtlnm.ForeColor != Color.Black)
            //{
            //    txtlnm.Text = "";
            //}
        }

        private void txtCA_Enter(object sender, EventArgs e)
        {
        
            //    if (txtCA.ForeColor != Color.Black)
        //    {
        //        txtCA.Text = "";
        //    }
        
        }

        private void txtCD_Enter(object sender, EventArgs e)
        {
            //if (txtCD.ForeColor != Color.Black)
            //{
            //    txtCD.Text = "";
            //}
        }

        private void txtCC_Enter(object sender, EventArgs e)
        {
            //if (txtCC.ForeColor != Color.Black)
            //{
            //    txtCC.Text = "";
            //}
        }

        private void txtPA_Enter(object sender, EventArgs e)
        {
            //if (txtPA.ForeColor != Color.Black)
            //{
            //    txtPA.Text = "";
            //}
        }

        private void txtPD_Enter(object sender, EventArgs e)
        {
            //if (txtPD.ForeColor != Color.Black)
            //{
            //    txtPD.Text = "";
            //}
        }

        private void txtPC_Enter(object sender, EventArgs e)
        {
            //if (txtPC.ForeColor != Color.Black)
            //{
            //    txtPC.Text = "";
            //}
        }

        private void txtfnm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtmdlnm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtlnm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCA_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCD_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCC_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPA_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPD_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPC_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCA_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtCA.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtfnm_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtfnm.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtmdlnm_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtmdlnm.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtlnm_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtlnm.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtCD.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCC_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtCC.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPA_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPA.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPD_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPD.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPC_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPC.ForeColor = System.Drawing.Color.Black;
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtfnm_Leave(object sender, EventArgs e)
        {
            if (txtfnm.Text.Length <= 0)
            {
                txtfnm.Text = "First Name";
                txtfnm.ForeColor = Color.Gray;
            }
        }

        private void txtmdlnm_Leave(object sender, EventArgs e)
        {
            if (txtmdlnm.Text.Length <= 0)
            {
                txtmdlnm.Text = "Middle Name";
                txtmdlnm.ForeColor = Color.Gray;
            }
        }

        private void txtlnm_Leave(object sender, EventArgs e)
        {
            if (txtlnm.Text.Length <= 0)
            {
                txtlnm.Text = "Last Name";
                txtlnm.ForeColor = Color.Gray;
            }
        }

        private void txtCA_Leave(object sender, EventArgs e)
        {

            if (txtCA.Text.Length <= 0)
            {
                txtCA.Text = "Address";
                txtCA.ForeColor = Color.Gray;
            }
        }

        private void txtCD_Leave(object sender, EventArgs e)
        {
            if (txtCD.Text.Length <= 0)
            {
                txtCD.Text = "District";
                txtCD.ForeColor = Color.Gray;
            }
        }

        private void txtCC_Leave(object sender, EventArgs e)
        {
            if (txtCC.Text.Length <= 0)
            {
                txtCC.Text = "City";
                txtCC.ForeColor = Color.Gray;
            }
        }

        private void txtPA_Leave(object sender, EventArgs e)
        {
            if (txtPA.Text.Length <= 0)
            {
                txtPA.Text = "Address";
                txtPA.ForeColor = Color.Gray;
            }
        }

        private void txtPD_Leave(object sender, EventArgs e)
        {
            if (txtPD.Text.Length <= 0)
            {
                txtPD.Text = "District";
                txtPD.ForeColor = Color.Gray;
            }
           
        }

        private void txtPC_Leave(object sender, EventArgs e)
        {
            if (txtPC.Text.Length <= 0)
            {
                txtPC.Text = "City";
                txtPC.ForeColor = Color.Gray;
            }
        }

        private void cbofrmNo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cbofrmNo.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[2].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[3].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[4].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[5].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[6].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[7].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[8].Hidden = true;
            cbofrmNo.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Form No.";
            cbofrmNo.DisplayLayout.Bands[0].Columns[1].Width = cbofrmNo.Width;
            cbofrmNo.DisplayMember = "FormNo";
            cbofrmNo.ValueMember = "admid";
        }

        private void cbofrmNo_RowSelected(object sender, RowSelectedEventArgs e)
        {
            if (cbofrmNo.Value == null)
            {
                prcClearDataReg();
                return;
            }
                else
                prcClearDataReg();

            prcDisplayDetailsReg(cbofrmNo.Value.ToString(),"0");
        }

        private void dtReg_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cbofrmNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cbofrmNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtfanm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        

        private void txtMoNm_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboRlgn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCust_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtBG_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFO_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFD_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMI_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRmrks_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRegNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRoll_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtssn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cbofrmNo_Validating(object sender, CancelEventArgs e)
        {
            if(cbofrmNo.Text.Length>0)
            {
                if(cbofrmNo.IsItemInList()==false)
                {
                    MessageBox.Show("Please Provide a Valid Data [or Select List]");
                    cbofrmNo.Focus();
                }
            }
        }

        private void txtfanm_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMoNm_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtDOB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboRlgn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCust_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtBG_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtFO_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtFD_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMI_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtRmrks_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }



        private void txtSec_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtRegNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtRoll_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtssn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void dtDOB_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private  void prcClearDataReg()
        {
            //cbofrmNo.Text = null;
            this.txtfnm.Text = "First Name";
            this.txtmdlnm.Text = "Middle Name";
            this.txtlnm.Text = "Last Name";
            txtfanm.Text = null;
            txtMoNm.Text = null;
            txtCA.Text = "Address";
            txtCD.Text = "District";
            txtCC.Text = "City";
            txtPA.Text = "Address";
            txtPD.Text = "District";
            txtPC.Text = "City";
            cboRlgn.Text = null;
            txtCust.Text = null;
            txtBG.Text = null;
            txtAdmsnfor.Text = null;
            txtRegID.Text = null;
            txtRoll.Text = null;
            txtRegNo.Text = null;
            txtSec.Text = null;
            txtssn.Text = null;
            txtFO.Text = null;
            txtFD.Text = null;
            txtMI.Text = null;
            txtRmrks.Text = null;
            txtsts.Text = null;

            this.txtfnm.ForeColor = Color.Gray;
            this.txtmdlnm.ForeColor = Color.Gray;
            this.txtlnm.ForeColor = Color.Gray;

            this.txtCA.ForeColor = Color.Gray;
            this.txtCD.ForeColor = Color.Gray;
            this.txtCC.ForeColor = Color.Gray;

            this.txtPA.ForeColor = Color.Gray;
            this.txtPD.ForeColor = Color.Gray;
            this.txtPC.ForeColor = Color.Gray;

            btnSaveR.Text = "&Save";
            btnDlt.Enabled = false;
      
        }
        private Boolean fncBlnkReg()
        {
            if (txtfnm.ForeColor != Color.Black)
            {
                MessageBox.Show("Please Provide First Name");
                txtfnm.Focus();
                return true;
            }
            if (txtmdlnm.ForeColor != Color.Black)
            {
                MessageBox.Show("Please Provide Middle Name");
                txtmdlnm.Focus();
                return true;
            }
            if (txtlnm.ForeColor != Color.Black)
            {
                MessageBox.Show("Please Provide Last Name");
                txtmdlnm.Focus();
                return true;
            }

            return false;
        }
        

        private void btnSaveR_Click(object sender, EventArgs e)
        {
            ArrayList arQuery=new ArrayList();
            //double NewID;
            double RefID2;

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery;
            try
            {


                if (txtRegID.Text.Length > 0)
                {
                    sqlQuery="Delete tbladm_Reg_pic where regId='"+txtRegID.Text.ToString()+"'";
                        arQuery.Add(sqlQuery);
                    RefID2 = double.Parse(txtRegID.Text.ToString());
                    sqlQuery = "Update tblAdm_Reg set RegNo='" + txtRegNo.Text + "', AdmId='" + txtRegID.Tag +
                               "', RollNo='" + txtRoll.Text
                               + "',FNm='" + txtfnm.Text + "', MNm='" + txtmdlnm.Text + "', LNm='" + txtlnm.Text +
                               "',FatherName='" + txtfanm.Text
                               + "', MotherName='" + txtMoNm.Text + "',cAdd='" + txtCA.Text + "', cCity='" + txtCC.Text +
                               "', cDist='" + txtCD.Text
                               + "', pAdd='" + txtPA.Text + "', pCity='" + txtPC.Text + "', pDist='" + txtPD.Text +
                               "', DOB='" + clsProc.GTRDate(dtDOB.Value.ToString())
                               + "', Relgn='" + cboRlgn.Text + "', Cst='" + txtCust.Text + "', BGrp='" + txtBG.Text +
                               "', FDesig='" + txtFD.Text + "', FJob='" + txtFO.Text + "' , sts = '" + txtsts.Text + "' , currsec = '" + txtSec.Text +
                               "', AvgMnIncome='" + txtMI.Text + "', CurrCls='" + txtAdmsnfor.Value +
                               "', Sesn='" + txtssn.Text + "', Rmks='" + txtRmrks.Text + "' where RegId='"+txtRegID.Text+"'";
                    arQuery.Add(sqlQuery);
                    
                    fncGridData(ref arQuery, RefID2.ToString());
                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Update SuccessFully");
                }
                else
                {
                    double NewID = 0;
                    sqlQuery = "Select Isnull(max(RegId),0)+1 as NewID from tblAdm_Reg";
                    NewID = clsCon.GTRCountingDataLarge(sqlQuery);
                    RefID2 = NewID;
                    sqlQuery = "Insert Into tblAdm_Reg ( RegId, RegNo, AdmId, RollNo, FNm, MNm, LNm,FatherName, MotherName,cAdd, cCity, cDist, pAdd, pCity, pDist, DOB,Relgn, Cst, BGrp, FDesig, FJob, AvgMnIncome, CurrCls, CurrSec, Sesn, Rmks, Sts)";
                    sqlQuery += "values(" + NewID + ",'" + txtRegNo.Text + "','" + txtRegID.Tag + "','" + txtRoll.Text + "','" + txtfnm.Text
                               +"','"+txtmdlnm.Text+"','"+txtlnm.Text+"','"+txtfanm.Text+"','"+txtMoNm.Text+"','"+txtCA.Text
                               +"','"+txtCC.Text+"','"+txtCD.Text+"','"+txtPA.Text+"','"+txtPC.Text+"','"+txtPD.Text
                               +"','"+clsProc.GTRDate(dtDOB.Value.ToString())+"','"+cboRlgn.Value+"','"+txtCust.Text
                               +"','"+txtBG.Text+"','"+txtFD.Text+"','"+txtFO.Text+"','"+txtMI.Text+"','"+txtAdmsnfor.Value+"','"+txtSec.Text
                               + "','" + txtssn.Text + "','" + txtRmrks.Text + "','" + txtsts.Text + "')";
                    arQuery.Add(sqlQuery);
                    fncGridData(ref arQuery, RefID2.ToString());
                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','INSERT')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Save SuccessFully");

                }
               
                PrcLoadList();
                prcClearDataReg();
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
                arQuery = null;
            }
        }

        private void fncGridData(ref ArrayList arQuery, string strID)
        {
            try
            {
                string pic="";
                foreach (UltraGridRow row in gridPics.Rows)
                {
                   
                    #region CopyImage

                    if (row.Cells["PicName"].Text.Length != 0)
                    {
                        if (row.Cells["Pict"].Text.Length != 0) //If New Image then it w
                        {

                            string formate = row.Cells["PicName"].Text.ToString().Substring(row.Cells["PicName"].Text.ToString().LastIndexOf("."));
                            
                            pic = row.Cells["PicName"].Text.ToString().Replace(row.Cells["PicName"].Text.ToString(), row.Cells["PicBy"].Text.ToString().Substring(0, 1) + strID + formate);
                            row.Cells["Pict"].Value = pic;
                            string strTarget = Common.Classes.clsMain.strRelationalId + @"\" + pic;
                            // File.Delete(row.Cells["pict"].Value.ToString());
                            File.Copy(row.Cells["pict"].Text.ToString(), strTarget, true);
                        }

                        else {
                            row.Cells["Pict"].Value = row.Cells["PicName"].Value;
                              }
                    }

                    #endregion

                    string sqlQuery = "Insert Into  tblAdm_Reg_Pic  (picName, PicBy, regid)";
                    sqlQuery += "values('"+ row.Cells["pict"].Text.ToString()+"','" + row.Cells["picBy"].Text.ToString() + "'," + Int32.Parse(strID) + ")";
                    arQuery.Add(sqlQuery);

                }
            }
            catch (Exception ex)
            {

                throw(ex);
            }
            finally
            {
               // arQuery = null;
            }

        }
        private void fncGridDataEnroll(ref ArrayList arQuery, string strID)
        {
            try
            {
                foreach (UltraGridRow row in txtRegNo1.Rows)
                {
                    //select   from 
                    string sqlQuery = "Insert Into  tblFee_Mgt_Data  (RegId,FeeMgtId,FeeMgtName,FeeAmt,HeadId,paySegID,rowNo)";
                    sqlQuery += "values('" + cboRegNo.Value.ToString() + "'," + cboTempLateID.Value + ",'" + cboTempLateID.Text.ToString() + "','" + row.Cells["amount"].Text.ToString() + "','" + row.Cells["headname"].Value.ToString() + "','" + row.Cells["Segname"].Value.ToString() + "','" + row.Cells["rowno"].Text.ToString() + "')";
                    arQuery.Add(sqlQuery);

                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                // arQuery = null;
            }

        }
        private void gridPics_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridPics.DisplayLayout.Bands[0].Columns["Pict"].Hidden =true;
            gridPics.DisplayLayout.Bands[0].Columns["Pic"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Image;
            gridPics.DisplayLayout.Bands[0].Columns["Button"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            gridPics.DisplayLayout.Bands[0].Columns["PicBy"].Style =Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;
            //Change alternate color
            this.gridResult.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            this.gridResult.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;
            this.gridPics.DisplayLayout.Override.DefaultRowHeight = 50;
            //Select Full Row when click on any cell
            // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridResult.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //this.gridFrmRcv.DisplayLayout.Override.AllowUpdate= DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridResult.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //gridPics.DisplayLayout.Bands[0].Columns["Pic"].Style

        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            prcGridPicRowAdd();
        }
        private void prcGridPicRowAdd()
        {
            DataRow dr4;
            dr4 = dsList.Tables["pic"].NewRow();
            dsList.Tables["pic"].Rows.Add(dr4);
            gridPics.DisplayLayout.Bands[0].Columns["PicBy"].ValueList = uddPicBy;
        }

        private void gridPics_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.ToString() == "Button")
            {
                try
                {
                    OpenFileDialog diagOpen = new OpenFileDialog();
                    diagOpen.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                    //diagOpen.Filter = "Icon Files(*.ico)|*.ico";
                    if (diagOpen.ShowDialog() == DialogResult.OK)
                    {
                        //gridPics.ActiveRow.Cells["pict"].Value = null;
                        //gridPics.ActiveRow.Cells["picName"].Value = null;
                        gridPics.ActiveRow.Cells["pict"].Value = diagOpen.FileName;
                        gridPics.ActiveRow.Cells["picName"].Value = diagOpen.FileName.Substring(diagOpen.FileName.LastIndexOf("\\") + 1);
                        gridPics.ActiveRow.Cells["Pic"].Value = new Bitmap(diagOpen.FileName);
                    }
                }
                catch (Exception)
                {
                    throw new ApplicationException("Failed loading image");
                }
            }
        }

        private void btnCncl_Click(object sender, EventArgs e)
        {
            prcClearDataReg();
            cbofrmNo.Text = null;
            PrcLoadList();
        }

        private void btnCls_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboFromSsn_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFromSsn.DisplayLayout.Bands[0].Columns["Sesn"].Header.Caption = "Session";
            cboFromSsn.DisplayLayout.Bands[0].Columns["Sesn"].Width = cboFromSsn.Width;
        }

        private void cboToSsn_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboToSsn.DisplayLayout.Bands[0].Columns["Sesn"].Header.Caption = "Session";
            cboToSsn.DisplayLayout.Bands[0].Columns["Sesn"].Width = cboToSsn.Width;
        }

        private void cboFromYr_Validating(object sender, CancelEventArgs e)
        {
            if(cboFromSsn.Text.Length>0)
            {
                if(cboFromSsn.IsItemInList()==false)
                {
                    MessageBox.Show("Please Provide Valid Data or[Select List]");
                    cboFromSsn.Focus();
                }
            }
        }

        private void cboToYr_Validating(object sender, CancelEventArgs e)
        {
            if (cboToSsn.Text.Length > 0)
            {
                if (cboToSsn.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide Valid Data or[Select List]");
                    cboToSsn.Focus();
                }
            }
        }

        private void btnRegLoad_Click(object sender, EventArgs e)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {
                //string sqlQuery="Exec Prc"
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cboClas_Leave(object sender, EventArgs e)
        {
            ChangeEvent();
        }

        private void cbofrmNm_Leave(object sender, EventArgs e)
        {
            ChangeEvent();
        }

        private void cboRlgn_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            cboRlgn.DisplayLayout.Bands[0].Columns["vartype"].Hidden = true;
            cboRlgn.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Relegion";
            cboRlgn.DisplayLayout.Bands[0].Columns["varname"].Width = cboRlgn.Width;
            cboRlgn.DisplayMember = "varname";
            cboRlgn.ValueMember = "varname";
        }


        private void cboYear_Click(object sender, EventArgs e)
        {
            PrcLoadCombo();

        }

        private void txtsecn_KeyDown(object sender, KeyEventArgs e)
        {
             clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        

        private void cbofrmNo_Click(object sender, EventArgs e)
        {
            PrcLoadCombo();
        }

        private void txtsecn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }
        private void txtFatherNm_Leave(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtFatherNm);
       
        }
        private void txtMotherNm_Leave(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtFatherNm);
        }
        private void txtsecn_Leave(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtAddress);
        }

        private void txtSesion_Leave(object sender, EventArgs e)
        {
          
        }

        private void txtAdmsnfor_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            txtAdmsnfor.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            txtAdmsnfor.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Admission For";
            txtAdmsnfor.DisplayLayout.Bands[0].Columns["clsName"].Width = txtAdmsnfor.Width;
            txtAdmsnfor.DisplayMember = "clsName";
            txtAdmsnfor.ValueMember = "clsId";
        }

        private void txtRmrks_Leave(object sender, EventArgs e)
        {
            btnSaveR.Focus();
        }

        private void txtAdmsnfor_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtAdmsnfor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtsts_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtsts_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void gridtemplateLoad_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //A.FeeMgtID,HeadId as HeadName,paySegID  Segname,Amount,RowNo
            
            txtRegNo1.DisplayLayout.Bands[0].Columns["RowNo"].Hidden = true;
            txtRegNo1.DisplayLayout.Bands[0].Columns["HeadName"].Header.Caption = "Head Name";
            txtRegNo1.DisplayLayout.Bands[0].Columns["HeadName"].Width = 100;
            txtRegNo1.DisplayLayout.Bands[0].Columns["Segname"].Header.Caption = "Segment";
            txtRegNo1.DisplayLayout.Bands[0].Columns["Segname"].Width = 240;
            txtRegNo1.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Amount";
            txtRegNo1.DisplayLayout.Bands[0].Columns["Amount"].Width = 230;

            txtRegNo1.DisplayLayout.Bands[0].Columns["FeeMgtID"].Hidden = true;

            //Cell Style
            txtRegNo1.DisplayLayout.Bands[0].Columns["HeadName"].CellActivation = Activation.NoEdit;
            txtRegNo1.DisplayLayout.Bands[0].Columns["Segname"].CellActivation = Activation.NoEdit;
            txtRegNo1.DisplayLayout.Bands[0].Columns["Amount"].CellActivation = Activation.NoEdit;
           
                //Change alternate color
            this.txtRegNo1.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            this.txtRegNo1.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
            this.txtRegNo1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridFrmRcv.DisplayLayout.Override.AllowUpdate= DefaultableBoolean.False;

                //Hiding +/- Indicator
            this.txtRegNo1.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                this.txtRegNo1.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;

                txtRegNo1.DisplayLayout.Bands[0].Columns["headname"].ValueList = uddFeeTp;
                txtRegNo1.DisplayLayout.Bands[0].Columns["segname"].ValueList = uddPaySeg;
            }

        private void cboRegNo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            cboRegNo.DisplayLayout.Bands[0].Columns["regid"].Hidden = true;
            cboRegNo.DisplayLayout.Bands[0].Columns["RegNo"].Header.Caption = "Registration No";
            cboRegNo.DisplayLayout.Bands[0].Columns["FullName"].Header.Caption = "Full Name";
            cboRegNo.DisplayLayout.Bands[0].Columns["RollNo"].Hidden = true;
            cboRegNo.DisplayLayout.Bands[0].Columns["FatherName"].Hidden = true;
            cboRegNo.DisplayLayout.Bands[0].Columns["Sesn"].Hidden = true;
            cboRegNo.DisplayLayout.Bands[0].Columns["CurrCls"].Hidden = true;

            cboRegNo.DisplayLayout.Bands[0].Columns["FullName"].Width = 200;

            cboRegNo.DisplayMember = "RegNo";
            cboRegNo.ValueMember = "regid";

        }

        private void cboRegNo_RowSelected(object sender, RowSelectedEventArgs e)
        {

            if (cboRegNo.Value == null)
            {

                return;

            }
            else
                //prcClearDataReg();

            this.txtEnrollID.Text = "";
            prcDisplayDetailsENR(cboRegNo.Value.ToString(), "0");


        }

        private void cboClassE_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboClassE.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            cboClassE.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Admission For";
            cboClassE.DisplayLayout.Bands[0].Columns["clsName"].Width = cboClas.Width;
            cboClassE.DisplayMember = "clsName";
            cboClassE.ValueMember = "clsId";
        }

        private void ultraButton12_Click(object sender, EventArgs e)
        {

            if (txtRegNo1.Rows.Count>0)
            {
                MessageBox.Show("ID already Contains Enroll Data");
                cboRegNo.Focus();
                return;
            }

            if (cboTempLateID.Text.Length == 0)
            {
                MessageBox.Show("There is No Preloaded Template For this ID");
                cboTempLateID.Focus();
                return;
            }

            if (cboTempLateID.Value == null)
            {
                return;
            }
            else
            prcDisplayDetailsTemplate(cboTempLateID.Value.ToString());

            txtRegNo1.DataSource = null;
            txtRegNo1.DataSource = dsDetails2.Tables["Templatedetails"];

            uddPaySeg.DataSource = null;
            uddPaySeg.DataSource = dsDetails2.Tables["payseg"];

            uddFeeTp.DataSource = null;
            uddFeeTp.DataSource = dsDetails2.Tables["tranhead"];


            txtRegNo1.DisplayLayout.Bands[0].Columns["headname"].ValueList = uddFeeTp;
            txtRegNo1.DisplayLayout.Bands[0].Columns["segname"].ValueList = uddPaySeg;
        }

        private void ultraButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uddFeeTp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddFeeTp.DisplayLayout.Bands[0].Columns["headid"].Hidden = true;

            uddFeeTp.DisplayLayout.Bands[0].Columns["headname"].Width = txtRegNo1.DisplayLayout.Bands[0].Columns[1].Width;
            uddFeeTp.DisplayLayout.Bands[0].Columns["headname"].Header.Caption = "Fee Type";

            uddFeeTp.ValueMember = "headid";
            uddFeeTp.DisplayMember = "headname";
        }

        private void uddPaySeg_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddPaySeg.DisplayLayout.Bands[0].Columns["varid"].Hidden = true;

            uddPaySeg.DisplayLayout.Bands[0].Columns["varname"].Width = txtRegNo1.DisplayLayout.Bands[0].Columns[2].Width;
            uddPaySeg.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Pay Segment";

            uddPaySeg.ValueMember = "varid";
            uddPaySeg.DisplayMember = "varname";
        }

        private void cboClassE_ValueChanged(object sender, EventArgs e)
        {
            
            dslist1 = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            
            
            {
                if (cboClassE.Text.Length != 0)
                {
                    string sqlQuery = "Exec prcgetTemplateid " + Common.Classes.clsMain.intComId + ",'" + cboClassE.Value.ToString() + "',''";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dslist1, sqlQuery);
                    dslist1.Tables[0].TableName = "Templateid";

                    cboTempLateID.DataSource = null;
                    cboTempLateID.DataSource = dslist1.Tables["Templateid"];
                    cboTempLateID.ValueMember = "FeeMgtID";
                    cboTempLateID.DisplayMember = "FeeMgtName";
                   // cboTempLateID.Value = 1;
                }
            }

            catch (Exception)
            {
                //throw (ex);
            }
            finally
            {
                clsCon = null;
            }
        }

        private void btnSaveEnr_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            //double NewID;
            double RefID2;

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery;
            try
            {
                if (txtEnrollID.Text.Length > 0)
                {
                    sqlQuery = "Delete tblFee_Mgt_Data where regid='" + cboRegNo.Value.ToString() + "'";
                    arQuery.Add(sqlQuery);
                    RefID2 = double.Parse(cboTempLateID.Value.ToString());
                    sqlQuery = "Update tblAdm_Reg set RegNo='" + txtRegNo.Text + "', AdmId='" + txtRegID.Tag +
                               "', RollNo='" + txtRoll.Text
                               + "',FNm='" + txtfnm.Text + "', MNm='" + txtmdlnm.Text + "', LNm='" + txtlnm.Text +
                               "',FatherName='" + txtfanm.Text
                               + "', MotherName='" + txtMoNm.Text + "',cAdd='" + txtCA.Text + "', cCity='" + txtCC.Text +
                               "', cDist='" + txtCD.Text
                               + "', pAdd='" + txtPA.Text + "', pCity='" + txtPC.Text + "', pDist='" + txtPD.Text +
                               "', DOB='" + clsProc.GTRDate(dtDOB.Value.ToString())
                               + "', Relgn='" + cboRlgn.Text + "', Cst='" + txtCust.Text + "', BGrp='" + txtBG.Text +
                               "', FDesig='" + txtFD.Text + "', FJob='" + txtFO.Text + "' , sts = '" + txtsts.Text + "' , currsec = '" + txtSec.Text +
                               "', AvgMnIncome='" + txtMI.Text + "', CurrCls='" + txtAdmsnfor.Value +
                               "', Sesn='" + txtssn.Text + "', Rmks='" + txtRmrks.Text + "' where RegId='" + cboRegNo.Value + "'";
                    arQuery.Add(sqlQuery);
                    
                    
                    fncGridDataEnroll(ref arQuery, RefID2.ToString());
                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Update SuccessFully");
                }
                else
                {
                    double NewID = 0;
                    sqlQuery = "Select Isnull(max(aEnrlID),0)+1 as NewID from tblAdm_Enroll";
                    NewID = clsCon.GTRCountingDataLarge(sqlQuery);
                    RefID2 = NewID;
                    //Sub,Major,PreCls,Major1,Grp,Yrnm
                    sqlQuery = "Insert Into tblAdm_Enroll ( aEnrlID,vEnrlID,RegID,RollNo,Seson,clsId,Major,Grp,Yrnm,Session,Course,Rmks,aid) values(" + NewID + ",'" + NewID + "','" + cboRegNo.Value.ToString() + "','" + txtRollNo.Text + "','" + cboSeason.Text.ToString() + "'," + cboClassE.Value + ",'" + cboMajorE.Text.ToString() + "','" + cboGroupE.Text.ToString() + "','" + txtYearEnroll.Text.ToString() + "','" + txtSession.Text.ToString() + "','" + cboCourse.Text.ToString()
                               + "','" + txtRemarks.Text + "'," + NewID + ")";
                    arQuery.Add(sqlQuery);

                    
                    fncGridDataEnroll(ref arQuery, RefID2.ToString());
                    
                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','INSERT')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Save SuccessFully");

                }

                PrcLoadList();
                prcClearDataReg();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
                arQuery = null;
            }
        }

        private void btnCancelEnr_Click(object sender, EventArgs e)
        {
            prcclearenrdata();
        }

        private void prcclearenrdata()
        {
            
            this.txtEnrollID.Text = "";
            this.cboRegNo.Text = "";
            this.txtRollNo.Text = "";
            this.txtName.Text = "";
            this.txtFatherNameE.Text = "";
            this.cboClassE.Text = "";
            this.txtSession.Text = "";
            this.txtYearEnroll.Text = "";
            this.cboMajorE.Text = "";
            this.cboGroupE.Text = "";
            this.cboCourse.Text = "";
            this.cboSeason.Text = "";
            this.txtRemarks.Text = "";
            dtEnroll.Value = DateTime.Now;

            btnSaveEnr.Text = "&Save";
            //btnDelete.Enabled = false;
        }

        private void ultraButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void txtAdmsnID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSlNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboSex_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboReligion_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtAmt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtReferrance_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFrmRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFrmRemarks_Leave(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtSesion);
            btnSave.Focus();
        }

        private void cboSex_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSex.DisplayLayout.Bands[0].Columns["vartype"].Hidden = true;
            cboSex.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Relegion";
            cboSex.DisplayLayout.Bands[0].Columns["varname"].Width = cboSex.Width;
            cboSex.DisplayMember = "varname";
            cboSex.ValueMember = "varname";
        }

        private void cboReligion_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboReligion.DisplayLayout.Bands[0].Columns["vartype"].Hidden = true;
            cboReligion.DisplayLayout.Bands[0].Columns["varname"].Header.Caption = "Relegion";
            cboReligion.DisplayLayout.Bands[0].Columns["varname"].Width = cboReligion.Width;
            cboReligion.DisplayMember = "varname";
            cboReligion.ValueMember = "varname";
        }

    }
}