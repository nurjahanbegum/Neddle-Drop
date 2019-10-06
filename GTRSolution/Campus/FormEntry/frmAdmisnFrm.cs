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

namespace GTRHRIS.Campus.FormEntry
{
    public partial class frmAdmisnFrm : Form
    {
        DataSet dsList2;
        DataSet dslist3;
        DataSet dslist4;


        DataSet dsList;
        DataSet dsDetails;
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

        private void frmAdmisnFrm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void PrcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
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
                dsList.Tables[8].TableName = "Sessn";
                dsList.Tables[9].TableName = "sex";

                DataRow dr;
                if (dsList.Tables["YarNm"].Rows.Count > 0)
                {
                    dr = dsList.Tables["YarNm"].Rows[0];

                    this.txtSesion.Text = dr["SesYear"].ToString();
                }

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["GridData"];

                gridFrmRcv.DataSource = null;
                gridFrmRcv.DataSource = dsList.Tables["ForMGrid"];
            
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

            cbofrmNm.DataSource = null;
            cbofrmNm.DataSource = dsList.Tables["LoadFormNm"];

            cboClas.DataSource = null;
            cboClas.DataSource = dsList.Tables["Class"];
            if (cboClas.Rows.Count > 0)
            {
                cboClas.Value = cboClas.Rows[0].Cells["clsId"].Value.ToString();
                //cboDeskLook.Enabled = false;
            }

           
            cboYear.DataSource = null;
            cboYear.DataSource = dsList.Tables["Sessn"];


            if (cboYear.Rows.Count > 0)
            {
                cboYear.Value = cboYear.Rows[0].Cells["sesn"].Value.ToString();
                //cboDeskLook.Enabled = false;
            }

            cboReligion.DataSource = null;
            cboReligion.DataSource = dsList.Tables["Religion"];


            cboSex.DataSource = null;
            cboSex.DataSource = dsList.Tables["sex"];

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

        private void frmAdmisnFrm_Load(object sender, EventArgs e)
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
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
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

                    //sqlQuery = "Select convert(int,ISNULL(max(slno),0 )+1) as slno from tblAdm_Form where sesn = '"+ txtSesion.Text +"' and clsId = '"+cboClas.Value.ToString()+ "'";
                    sqlQuery = "Select convert(int,ISNULL(max(slno),0 )+1) as slno from tblAdm_Form where sesn = '" + txtSesion.Text + "'";
                    slno = clsCon.GTRCountingData(sqlQuery);


                    sqlQuery = "Insert Into tblAdm_Form (sesn,AdmId,slno, frmNo, frmNoAuto,nmFirst, nmMiddle, nmLast, nmFather, nmMother, clsid, yrNm,frmTakenDt,Address,sex,relegion,phone,mobile,amount,Reference,remarks)";
                    sqlQuery = sqlQuery + "values('" + txtSesion.Text.Trim() + "'," + NewID + "," + slno + ",'" + txtFormNo.Text.Trim() + "'," + AdmID + ",'" + txtNm.Text.Trim() + "','" + txtMn.Text.Trim() + "','" + txtLn.Text.Trim() + "','" + txtFatherNm.Text.Trim() + "','" + txtMotherNm.Text.Trim()
                        + "','" + cboAdmisnFor.Value.ToString() + "','" + txtSesion.Text.Trim() + "','" + clsProc.GTRDate(dtAdmsn.Value.ToString()) + "','" + txtAddress.Text + "','" + cboSex.Text + "','" + cboReligion.Text + "','" + txtPhone.Text + "','" + txtMobile.Text + "','" + txtAmt.Text + "','" + txtReferrance.Text + "','" + txtFrmRemarks.Text + "')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Saved Successfully");
                
                }
                PrcCleraData();
                PrcLoadList();
                PrcLoadCombo();
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






            string sqlQuery = "Exec [prcEnrDetails] " + Common.Classes.clsMain.intComId + ", " + Int32.Parse(strParam) + ",'ENR'";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetailsENR, sqlQuery);
            dsDetailsENR.Tables[0].TableName = "tblENR";
            dsDetailsENR.Tables[1].TableName = "payseg";
            dsDetailsENR.Tables[2].TableName = "tranhead";
            dsDetailsENR.Tables[3].TableName = "tblFeeMGT";

           


            DataRow dr;
            if (dsDetailsENR.Tables["tblENR"].Rows.Count > 0)
            {
                dr = dsDetailsENR.Tables["tblENR"].Rows[0];
                {
                   

                }

               
            }
            else
            //cboTempLateID.DataSource = 0;
            //cboTempLateID.Text = "";

            if (dsDetailsENR.Tables["tblFeeMGT"].Rows.Count > 0)
            {
                dr = dsDetailsENR.Tables["tblFeeMGT"].Rows[0];
                {
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
                   

               }


                else
                {
                   
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
                gridFrmRcv.DisplayLayout.Bands[0].Columns["secname"].Hidden = true;

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
            //cbofrmNm.Value = 1;

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

                PrcCleraData();
                PrcLoadList();
                PrcLoadCombo();

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
      

       

        private void ultraButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void ultraButton1_Click(object sender, EventArgs e)
        {
           
            dslist4 = new DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {
                dslist4.Tables[0].TableName = "Result";
                dslist4.Tables[1].TableName = "nm";
               

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

        private Boolean fncBlnkReg()
        {


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
               
                PrcLoadList();
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

       

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            prcGridPicRowAdd();
        }
        private void prcGridPicRowAdd()
        {
            DataRow dr4;
        }

       
        private void btnCncl_Click(object sender, EventArgs e)
        {
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

        
        private void ultraButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNm_ValueChanged(object sender, EventArgs e)
        
        {

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