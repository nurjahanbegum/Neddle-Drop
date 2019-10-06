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
    public partial class frmAdmisnResult : Form
    {
        DataSet dsList2;
        DataSet dslist4;



        DataSet dsList;
        DataSet dsDetails;
        DataSet dsProcess;


        
        public int RefID;
       // private DataTable dt;
        clsProcedure clsProc = new clsProcedure();
        private clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmAdmisnResult(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                sqlQuery = "Exec [prcgetBasiAddmsn] 0," + Common.Classes.clsMain.intComId + "";
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


                gridResult.DataSource = null;
                gridResult.DataSource = dsList.Tables["Result"];

                //dsList.Tables["Pic"].Columns.Add("Pic", typeof (Bitmap));
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void PrcLoadCombo()
        {       
          


            cboyrnm.DataSource = null;
            cboyrnm.DataSource = dsList.Tables["Sessn"];

           

            cboClsnm.DataSource = null;
            cboClsnm.DataSource = dsList.Tables["Class"];

           

        }

        private void txtNm_Click(object sender, EventArgs e)
        {
            //txtNm.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     

     

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PrcCleraData();

        }
        private void PrcCleraData()
        {
            
        }

       

        private void frmAdmisnRestar_Load(object sender, EventArgs e)
        {
            try
            {
                PrcLoadList();
                PrcLoadCombo();
                //dtAdmsn.Value = DateTime.Today;
               // txtYear.Text = DateTime.Now.Year.ToString();
                
               
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
          

            
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsList = new DataSet();
            int NewID,slno;
            string AdmID;
            string sqlQuery;
            try
            {
               
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

              



                //this.txtYear.Text = dr["yrNm"].ToString();
                
              
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
     
        private void ultraButton4_Click(object sender, EventArgs e)
        {
            PrcLoadList();
          
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
                gridResult.DisplayLayout.Bands[0].Columns["Secname"].Hidden = true;
                gridResult.DisplayLayout.Bands[0].Columns["frmNoAuto"].Header.Caption = "Form No";
                gridResult.DisplayLayout.Bands[0].Columns["frmNoAuto"].Width = 100;
                gridResult.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Name";
                gridResult.DisplayLayout.Bands[0].Columns["Name"].Width = 200;
                gridResult.DisplayLayout.Bands[0].Columns["nmFather"].Header.Caption = "Father Name";
                gridResult.DisplayLayout.Bands[0].Columns["nmFather"].Width = 180;
                gridResult.DisplayLayout.Bands[0].Columns["Secname"].Header.Caption = "Secname";
                gridResult.DisplayLayout.Bands[0].Columns["Secname"].Width = 230;
                gridResult.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Class";
                gridResult.DisplayLayout.Bands[0].Columns["clsName"].Width = 100;
                gridResult.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
                gridResult.DisplayLayout.Bands[0].Columns["sesn"].Width = 90;
                gridResult.DisplayLayout.Bands[0].Columns["Number"].Header.Caption = "Number";
                gridResult.DisplayLayout.Bands[0].Columns["Number"].Width = 75;
                gridResult.DisplayLayout.Bands[0].Columns["Ispassed"].Header.Caption = "PassedYN";
                gridResult.DisplayLayout.Bands[0].Columns["IsWaiting"].Header.Caption = "WaitingYN";
                gridResult.DisplayLayout.Bands[0].Columns["isMerit"].Header.Caption = "MeritYN";


                //gridFrmRcv.DisplayLayout.Bands[0].Columns["Ispassed"].Width = 90;

                //Cell Style
                gridResult.DisplayLayout.Bands[0].Columns["Ispassed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridResult.DisplayLayout.Bands[0].Columns["Ispassed"].CellActivation = Activation.AllowEdit;

                gridResult.DisplayLayout.Bands[0].Columns["IsWaiting"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridResult.DisplayLayout.Bands[0].Columns["IsWaiting"].CellActivation = Activation.AllowEdit;


                gridResult.DisplayLayout.Bands[0].Columns["isMerit"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridResult.DisplayLayout.Bands[0].Columns["isMerit"].CellActivation = Activation.AllowEdit;

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
            cboyrnm.DisplayLayout.Bands[0].Columns["sesn"].Width = cboyrnm.Width;
        }

        private void cboClsnm_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboClsnm.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            cboClsnm.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Admission For";
            cboClsnm.DisplayLayout.Bands[0].Columns["clsName"].Width = cboClsnm.Width;
            cboClsnm.DisplayMember = "clsName";
            cboClsnm.ValueMember = "clsID";
        }
        
        private void prcloadlist ()
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
                cbo.Value = 1;


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


        private void ultraButton1_Click(object sender, EventArgs e)
        {
            prcloadlist();
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
                    sqlQuery = "Insert Into tblAdm_Exam_Result (AdmID, FormNo, clsID, Yrnm, IsPassed, Number,IsMainList,iswaiting)"
                    + "values('" + row.Cells["AdmID"].Value + "','" + row.Cells["frmNoAuto"].Value + "','" + cboClsnm.Value.ToString()
                    + "','" + cboyrnm.Text.ToString() + "','" + row.Cells["IsPassed"].Value + "','" + row.Cells["Number"].Value + "','" + row.Cells["isMerit"].Value + "','" + row.Cells["isWaiting"].Value + "')";
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

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            prcGridPicRowAdd();
        }
        private void prcGridPicRowAdd()
        {
            DataRow dr4;
            dr4 = dsList.Tables["pic"].NewRow();
            dsList.Tables["pic"].Rows.Add(dr4);
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

        private void btnPass_Click(object sender, EventArgs e)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery = "";

            dsProcess = new System.Data.DataSet();
            try
            {

                sqlQuery = "Exec  [dbo].[prcAdmExamCalculation]  '" + cboyrnm.Text + "' , '" + cboClsnm.Value.ToString() + "', '" + txtPassNo.Text + "' ,0 ,'Passed' ,0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsProcess, sqlQuery);

                //    prcClearData();
                ////cboEmpID.Focus();

                //prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlQuery = null;
                clsCon = null;
            }
            prcloadlist();
           
           // [dbo].[prcAdmExamCalculation]  @sesn smallDatetime , @Class int = 0, @marks   float = 0 , @marks2   float = 0, @ProssType varchar(50) ,@waitingNo tinyint = 1
        }

        private void btnMerit_Click(object sender, EventArgs e)
        {

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery = "";

            dsProcess = new System.Data.DataSet();
            try
            {
                //if (fncBlank())
                //{
                //    return;
                //}



                sqlQuery = "Exec  [dbo].[prcAdmExamCalculation]  '" + cboyrnm.Text + "' , '" + cboClsnm.Value.ToString() + "', '" + txtMeritList.Text + "' ,0,'Merit' ,0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsProcess, sqlQuery);

                //    prcClearData();
                ////cboEmpID.Focus();

                //prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlQuery = null;
                clsCon = null;
            }
            prcloadlist();

        }

        private void btnWaiting_Click(object sender, EventArgs e)
        {
            //ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery = "";


            dsProcess = new System.Data.DataSet();
            try
            {
            //if (fncBlank())
            //{
            //    return;
            //}

            sqlQuery = "Exec  [dbo].[prcAdmExamCalculation]  '" + cboyrnm.Text + "' , '" + cboClsnm.Value.ToString() + "', '" + txtWaitNoFrom.Text + "' ,'" + txtWaitNoTo.Text + "' ,'Waiting' ,'"+ txtWaitingNo.Text +"'";
            clsCon.GTRFillDatasetWithSQLCommand(ref dsProcess, sqlQuery);

                //    prcClearData();
                ////cboEmpID.Focus();

                //prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlQuery = null;
                clsCon = null;
            }
            prcloadlist();

        }

       

    }
}