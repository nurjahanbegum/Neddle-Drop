using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using GTRLibrary;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using ZKFPEngXControl;
using AxZKFPEngXControl;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmIssuerFingerRegister : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private System.Data.DataSet dsEmpList;
        private clsProcedure clsProc = new clsProcedure();
        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private string EmpPic = @"Z:\Com\Pics\EmpPic";
        private string PicName;

        string FP = "";
        private int FMatchType, fpcHandle;
        private bool FAutoIdentify;
        Int64 FingerCount;
        private string FPData = "";
        private string EmpCode = "0";
        private Int64 Chk = 0;
        private Int64 ChkEmpFP = 0;

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmIssuerFingerRegister(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmIssuerFingerRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void frmIssuerFingerRegister_Load(object sender, EventArgs e)
        {
            try
            {

                Radio10.Checked = true;
                radioFinger2.Checked = false;

                prcLoadList();
                prcClearData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetEmpFingerRegister 0,'" + Common.Classes.clsMain.intComId + "',0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "tblFinger";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblFinger"];
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

        public void prcDisplayDetailsEmp(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetails = new System.Data.DataSet();

            lvlDataMatchImg.Visible = false;
            radioFinger2.Checked = false;

            string SqlQuery = "";
            ChkEmpFP = 0;

            //Position Capacity Checking Code
            SqlQuery = "Select dbo.fncCheckEmpFinger (" + Int32.Parse(strParam) + ")";
            ChkEmpFP = clsCon.GTRCountingDataLarge(SqlQuery);


            try
            {
                SqlQuery = "Exec prcGetEmpFingerRegister 1,'" + Common.Classes.clsMain.intComId + "'," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SqlQuery);
                dsDetails.Tables[0].TableName = "details";
                DataRow dr;

                if (dsDetails.Tables["details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["details"].Rows[0];
                    txtId.Text = dr["EmpId"].ToString();
                    txtEmpCode.Text = dr["EmpCode"].ToString();
                    txtName.Text = dr["EmpName"].ToString();
                    txtDesig.Text = dr["DesigName"].ToString();
                    txtSection.Text = dr["SectName"].ToString();
                    dtJDate.Text = dr["dtJoin"].ToString();

                    //btnSave.Text = "&Update";
                    //btnDelete.Enabled = true;


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
                                picPreview.Image = new Bitmap(EmpPic + "\\None.jpg");
                            }
                        }
                        else
                        {
                            picPreview.Image = null;
                        }

                    }
                }

                lvlDataMatchImg.ForeColor = Color.Blue;
                lvlDataMatchImg.Visible = true;
                lvlDataMatchImg.Text = "Finger Not Registered";

                if (ChkEmpFP == 1)
                {
                    //MessageBox.Show("Capacity over for this position.Please communicate with administrator.");
                    //return;

                    if (
                        MessageBox.Show("Finger already registered [" + txtName.Text.ToString() + "]. If you want again finger register/update please click Yes", "",
                                        System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }

                    lvlDataMatchImg.ForeColor = Color.Blue;
                    lvlDataMatchImg.Visible = true;
                    lvlDataMatchImg.Text = "Finger Already Registered";


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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ShowHintInfo(String s)
        {
            if (s != "")
            {
                memoHint.AppendText(s + Environment.NewLine);
            }
        }

        private void ShowHintImage(int iType)
        {
            //if (iType == 0)
            //{
            //    imgNO.Visible = false;
            //    imgOK.Visible = false;
            //}
            //else if (iType == 1)
            //{
            //    imgNO.Visible = false;
            //    imgOK.Visible = true;
            //}
            //else if (iType == 2)
            //{
            //    imgNO.Visible = true;
            //    imgOK.Visible = false;
            //}
        }

        private void prcDataInsert()
        {

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {



                sqlQuery = " Truncate table tblCat_EmpData";
                arQuery.Add(sqlQuery);


                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{52D44F79-9D3E-4D94-83EE-9E95958BA5B6}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{BE75A9EA-1866-4B5E-BA6C-8514306EAB3A}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{EBB99F66-64E8-4A54-B4F0-DD91625D575A}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{CF6A764C-9973-4261-8776-2C1AE77A5957}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{070C8678-CAD4-409B-BF6C-AC19788FAABA}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{90C2E947-8E49-4AE1-BBED-46D2CB2A4F30}'))";
                arQuery.Add(sqlQuery);
                //
                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{A1EE729B-9149-49D9-A73F-6B3CCE8D56C0}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{8A7C0217-0E5D-498D-BBA1-8891ECF7921B}'))";
                arQuery.Add(sqlQuery);

                sqlQuery = " Insert Into tblCat_EmpData(FIData) Select Convert(varchar(Max), dbo.fncDataFIEmpCheckET('{0FED0A3E-9F22-4E23-B3D5-3F6F70D1AFE5}'))";
                arQuery.Add(sqlQuery);


                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

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

        private void prcFingerIssuerSNCheck(string str)
        {

            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            //sqlQuery = "Select dbo.fncDataFICheck ('" + str.ToString() + "')";
            //Chk = clsCon.GTRCountingDataLarge(sqlQuery);
            Chk = 0;

        }
        private void btnDeviceConnect_Click(object sender, EventArgs e)
        {
            // prcDataInsert();
            Chk = 0;

            long nR = ZKFPEngX1.InitEngine();
            if (nR == 0)
            { 
                EDSensorSN.Text = ZKFPEngX1.SensorSN;
                prcFingerIssuerSNCheck(EDSensorSN.Text);

                if (Chk == 0)
                {
                    btnDeviceConnect.Enabled = false;
                    FMatchType = 2;
                    if (Radio9.Checked)
                    {
                        ZKFPEngX1.FPEngineVersion = "9";
                    }
                    else
                    {
                        ZKFPEngX1.FPEngineVersion = "10";
                    }

                    fpcHandle = ZKFPEngX1.CreateFPCacheDB();
                    EDSensorNum.Text = Convert.ToString(ZKFPEngX1.SensorCount);
                    EDSensorIndex.Text = Convert.ToString(ZKFPEngX1.SensorIndex);
                    ////EDSensorSN.Text = ZKFPEngX1.SensorSN;

                    ShowHintInfo("Sensor connected");

                    ZKFPEngX1.EnrollCount = 3;

                    if (ZKFPEngX1.IsRegister)
                    {
                        ZKFPEngX1.CancelEnroll();
                    }

                    FAutoIdentify = false;
                    ZKFPEngX1.SetAutoIdentifyPara(FAutoIdentify, fpcHandle, 8);
                    ShowHintInfo("Begin verification. Please finger touch in device.");
                    FMatchType = 2;
                    lvlDataMatchImg.Visible = true;
                    lvlDataMatchImg.Text = "Please Press Finger";
                    lvlDataMatchImg.ForeColor = Color.Blue;
                    btnCancel.Enabled = true;
                }
                else
                {
                    ZKFPEngX1.EndEngine();
                    ShowHintInfo("Failed to connect sensor");
                }
            }
            else
            {
                ShowHintInfo("Failed to connect sensor");
            }
        }

        private void ZKFPEngX1_OnImageReceived(object sender, IZKFPEngXEvents_OnImageReceivedEvent e)
        {
            ShowHintImage(0);
            Graphics g = PictureBox1.CreateGraphics();
            Bitmap bmp = new Bitmap(PictureBox1.Width, PictureBox1.Height);
            g = Graphics.FromImage(bmp);
            int dc = g.GetHdc().ToInt32();
            ZKFPEngX1.PrintImageAt(dc, 0, 0, bmp.Width, bmp.Height);
            g.Dispose();
            PictureBox1.Image = bmp;
        }

        private void ZKFPEngX1_OnFingerTouching(object sender, EventArgs e)
        {
            lvlDataMatch.Visible = false;
            lvlDataMatchImg.Visible = true;
            lvlDataMatchImg.ForeColor = Color.Blue;
            lvlDataMatchImg.Text = "Please wait..";
            ultraProgressBar1.Value = 0;
            ShowHintInfo("Touching");

        }

        private void ZKFPEngX1_OnFingerLeaving(object sender, EventArgs e)
        {
            ShowHintInfo("Leaving");
        }

        private void ZKFPEngX1_OnFeatureInfo(object sender, IZKFPEngXEvents_OnFeatureInfoEvent e)
        {
            String strTemp = "Fingerprint quality";
            if (e.aQuality != 0)
            {
                strTemp = strTemp + " not good";
            }
            else
            {
                strTemp = strTemp + " good";
            }
            if (ZKFPEngX1.EnrollIndex != 1)
            {
                if (ZKFPEngX1.IsRegister)
                {
                    if (ZKFPEngX1.EnrollIndex - 1 > 0)
                    {
                        strTemp = strTemp + '\n' + " Register status: still press finger " + Convert.ToString(ZKFPEngX1.EnrollIndex - 1) + " times!";
                    }
                }
            }
            ShowHintInfo(strTemp);
        }

        private void ZKFPEngX1_OnEnroll(object sender, IZKFPEngXEvents_OnEnrollEvent e)
        {
            if (e.actionResult)
            {
                //MessageBox.Show("Fingerprint register success！ ", "success! ", MessageBoxButtons.YesNo);
                ZKFPEngX1.AddRegTemplateStrToFPCacheDBEx(fpcHandle, 1, ZKFPEngX1.GetTemplateAsStringEx("10"), FP);
                FP = ZKFPEngX1.GetTemplateAsStringEx("10");
                prcFingerDataSave(FP);
                ShowHintInfo("Fingerprint register success.[" + txtName.Text.ToString() + "]");
                MessageBox.Show("Fingerprint register success.[" + txtName.Text.ToString() + "] ", "success! ", MessageBoxButtons.YesNo);
                lvlDataMatchImg.Text = "Fingerprint register success.";
                prcClearData();
            }
            else
            {
                ShowHintInfo("Fingerprint register failed.[" + txtName.Text.ToString() + "]");
                MessageBox.Show("Fingerprint register failed.[" + txtName.Text.ToString() + "] ", "failed! ", MessageBoxButtons.YesNo);
                lvlDataMatchImg.Text = "Fingerprint register failed.";
            }
        }

        private void prcFingerDataSave(string strFP)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            try
            {

                if (ChkEmpFP == 1)
                {


                    if (radioFinger2.Checked == true)
                    {

                        sqlQuery = "Update tblEmp_FingerData Set ComId = " +
                            Common.Classes.clsMain.intComId + ",FingerData = '" + FP + "',,FPIndex = '6',Privilege='0'  Where EmpId = '" + txtId.Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update','" + txtId.Text.ToString() + "')";
                        arQuery.Add(sqlQuery);

                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    }

                    else
                    {



                        sqlQuery = "Update tblEmp_FingerData Set ComId = " + 
                            Common.Classes.clsMain.intComId + ",FingerData = '" + FP + "',,FPIndex = '6',Privilege='0'  Where EmpId = '" + txtId.Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update','" + txtId.Text.ToString() + "')";
                        arQuery.Add(sqlQuery);

                        clsCon.GTRSaveDataWithSQLCommand(arQuery);
                     }

                  }///radioFinger2 if

                else
                {

                    sqlQuery = "Delete tblEmp_FingerData Where EmpId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Insert into tblEmp_FingerData(ComId,EmpId,FingerData,FPIndex,Privilege) values(" + Common.Classes.clsMain.intComId + ",'" + txtId.Text.ToString() + "','" +
                            FP + "','6','0')";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert','" + txtId.Text.ToString() + "')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

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

        private void ZKFPEngX1_OnCapture(object sender, IZKFPEngXEvents_OnCaptureEvent e)
        {
            int FingerCount = 0;
            fpcHandle = ZKFPEngX1.CreateFPCacheDB();
            FP = ZKFPEngX1.GetTemplateAsStringEx("10");
            ZKFPEngX1.AddRegTemplateStrToFPCacheDBEx(fpcHandle, 1, FP, FP);


            int ID = 0, i, T = 0, fi;
            int Score = new int();
            int ProcessNum = new int();
            ShowHintInfo("Acquired fingerprint template:");

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new DataSet();
            string sqlQuery = "";


            if (FMatchType == 1)
            {
                //ZKFPEngX1.VerFingerFromStr(sRegTemp, sVerTemplate, False, ref regChange) 

            }
            if (FMatchType == 2)//1:N
            {
                if (!FAutoIdentify)
                {

                    sqlQuery = "Exec [prcGetEmpRelIssuer] 0," + Common.Classes.clsMain.intComId + ",0";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                    dsDetails.Tables[0].TableName = "FingerData";


                    int rowCount;
                    for (rowCount = 0; rowCount < dsDetails.Tables["FingerData"].Rows.Count; rowCount++)
                    {

                        if (dsDetails.Tables["FingerData"].Rows[rowCount][0].ToString().Trim().Length > 0 &&
                        dsDetails.Tables["FingerData"].Rows[rowCount]["FingerData"].ToString().Trim().Length > 0)
                        {
                            EmpCode = dsDetails.Tables["FingerData"].Rows[rowCount]["EmpId"].ToString();
                            FPData = dsDetails.Tables["FingerData"].Rows[rowCount]["FingerData"].ToString();

                            if (Radio9.Checked)
                            {

                                ZKFPEngX1.FPEngineVersion = "9";
                                Score = 8;
                                ID = ZKFPEngX1.IdentificationFromStrInFPCacheDB(fpcHandle, FPData, ref Score, ref ProcessNum);
                            }
                            else
                            {
                                ZKFPEngX1.FPEngineVersion = "10";
                                Score = 8;
                                ID = ZKFPEngX1.IdentificationFromStrInFPCacheDB(fpcHandle, FPData, ref Score, ref ProcessNum);
                            }
                            if (ID == -1)
                            {
                                T = 0;
                            }
                            else
                            {
                                String strTemp = "Identification success!\n" + " Score =" + Convert.ToString(Score);
                                ultraProgressBar1.Value = Score;
                                ShowHintInfo(strTemp);
                                ShowHintInfo("Verify success");
                                T = 1;
                                prcFingerVerifySuccess();
                                prcDisplayDetails(EmpCode.ToString());
                                goto Outer;
                                //prcClearData();

                                //ShowHintImage(1);
                            }


                        }
                    }

                    if (T == 0)
                    {
                        ShowHintInfo("Identification Failed! Score = " + Convert.ToString(Score));
                        ShowHintInfo("Sorry,Verify failed!");
                        prcFingerVerifyFailed();
                        //prcClearData();
                    }

                Outer:
                    Console.WriteLine("Verify success");

                }
                else
                {
                    ID = 0;
                    Score = 0;
                    //e.aTemplateobject
                    Array _ObjectArray = (Array)e.aTemplate;
                    int _ObjectCount = _ObjectArray.GetLength(0);
                    for (i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            ID = Convert.ToInt32(_ObjectArray.GetValue(i));

                        }
                        else
                        {
                            Score = Convert.ToInt32(_ObjectArray.GetValue(i));
                        }
                    }
                    if (ID == -1)
                    {
                        ShowHintInfo("Fingerprint Auto Identify Failed!");
                        //ShowHintImage(2);
                    }
                    else
                    {
                        ShowHintInfo("Fingerprint Auto identification success! Score =" + Convert.ToString(Score));
                        //ShowHintImage(1);
                    }

                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ZKFPEngX1.EndEngine();
            btnDeviceConnect.Enabled = true;
            btnCancel.Enabled = false;
            lvlDataMatchImg.Visible = false;
            lvlDataMatch.Visible = false;
            picPreview.Image = null;
            ultraProgressBar1.Value = 0;
            EDSensorSN.Text = "0";
            prcClearData();

        }

        private void prcClearData()
        {
            txtId.Text = "";
            txtEmpCode.Text = "";
            txtName.Text = "";
            txtDesig.Text = "";
            txtSection.Text = "";
            txtUnit.Text = "";
            txtSalary.Text = "";
            dtJDate.Value = DateTime.Today;
            dtRelDate.Value = DateTime.Today;

        }

        private void prcFingerVerifySuccess()
        {
            lvlDataMatch.Visible = true;
            lvlDataMatch.Text = "Data Match";
            lvlDataMatch.ForeColor = Color.Green;

            lvlDataMatchImg.Visible = true;
            lvlDataMatchImg.Text = "Data Match";
            lvlDataMatchImg.ForeColor = Color.Green;
        }

        private void prcFingerVerifyFailed()
        {
            lvlDataMatch.Visible = true;
            lvlDataMatch.Text = "Data Not Match";
            lvlDataMatch.ForeColor = Color.Red;

            lvlDataMatchImg.Visible = true;
            lvlDataMatchImg.Text = "Data Not Match";
            lvlDataMatchImg.ForeColor = Color.Red;

            ultraProgressBar1.Value = 0;
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridList.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "EmpId";
            gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
            gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 120;
            gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 120;
            gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 120;

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
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetailsEmp(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            ZKFPEngX1.BeginEnroll();
            MessageBox.Show("Register Begin");
            ShowHintInfo("Register Begin");
        }

        private void prcDisplayDetails(string strParam)
        {

            dsEmpList = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", sqlQuery1 = "";

            try
            {
                sqlQuery = "Exec [prcGetEmpRelIssuer] 1," + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsEmpList, sqlQuery);
                dsEmpList.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsEmpList.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsEmpList.Tables["Details"].Rows[0];

                    this.txtId.Text = dr["EmpId"].ToString();
                    this.txtEmpCode.Text = dr["EmpCode"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtDesig.Text = dr["DesigName"].ToString();
                    this.txtSection.Text = dr["SectName"].ToString();
                    this.txtUnit.Text = dr["CompName"].ToString();
                    this.txtSalary.Text = dr["GS"].ToString();
                    this.dtJDate.Value = dr["dtJoin"];
                    this.dtRelDate.Value = dr["dtReleased"];

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
                                picPreview.Image = new Bitmap(EmpPic + "\\None.jpg");
                            }
                        }
                        else
                        {
                            picPreview.Image = null;
                        }

                    }


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


    }
   }


