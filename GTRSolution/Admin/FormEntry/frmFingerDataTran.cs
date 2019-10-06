using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Linq;
using System.Text;
using GTRHRIS.Common.Classes;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using GTRLibrary;
using zkemkeeper;
using System.IO;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;


namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmFingerDataTran : Form
    {

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private System.Data.DataSet dsEmpList;

        private bool bIsConnected = false;

        CZKEM axCZKEM1 = new CZKEM();
        private int iMachineNumber = 1;


        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmFingerDataTran(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmFingerDataTran_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            dsEmpList = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }


        private void frmFingerDataTran_Load(object sender, EventArgs e)
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
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            try
            {              
                sqlQuery = "Exec prcGetIpAddressGTRTran 0," + Common.Classes.clsMain.intComId + ",'',0,''";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblSex";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];


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

            cboSection.DataSource = null;
            cboSection.DataSource = dsList.Tables["tblSect"];

            cboBand.DataSource = null;
            cboBand.DataSource = dsList.Tables["tblSex"];


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                gridDetails.DisplayLayout.Bands[0].Columns["Id"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["Flag"].Hidden = true;

                ////Set Caption
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50; //Short Name
                //gridDetails.DisplayLayout.Bands[0].Columns["Id"].Width = 45; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["Location"].Width = 80; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["Active"].Width = 50; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["BlackWhite"].Width = 72; //Short Name


                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                gridDetails.DisplayLayout.Bands[0].Columns["Active"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox; ;

                gridDetails.DisplayLayout.Bands[0].Columns["BlackWhite"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox; ;
                ////Stop Cell Modify
                //gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Row Hight
                //gridDetails.DisplayLayout.Override.DefaultRowHeight = 20;

                //Hiding +/- Indicator
                gridDetails.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                ////Use Filtering
                //e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void gridInfo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                //this.gridInfo.DisplayLayout.Bands[0].Columns["ComId"].Hidden = true;

                ////Set Caption
                //gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50; //Short Name
                //gridDetails.DisplayLayout.Bands[0].Columns[0].Header.Caption = "ID";

                this.gridInfo.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                                                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Change alternate color
                gridInfo.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridInfo.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Hiding +/- Indicator
                gridInfo.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
 
                ArrayList arQuery = new ArrayList();
                GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

                string sqlQuery = "", DeviceIP = "", GetDeviceIP = "";
                Int32 NewId = 0;
                Int32 RowID;

                string DeviceIp = "", pDate = "", pTime = "";

                int Count = 0;

                int idwErrorCode = 0;


                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                {
                    if (row.Cells["Id"].Text.ToString().Length != 0 &&
                        row.Cells["isChecked"].Value.ToString() == "1" && row.Cells["Active"].Value.ToString() == "1")
                    {

                        GetDeviceIP = "" + row.Cells["IpAddress"].Text.ToString() + "";

                        bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                        if (bIsConnected == true)
                        {
                            iMachineNumber = 1;
                            axCZKEM1.RegEvent(iMachineNumber, 65535);
                        }

                        Count = Convert.ToInt32(txtRestore.Text);
                        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                        if (axCZKEM1.RestoreLogData(iMachineNumber, Count)) //Here we use the function "RestoreLogData" and Restore Number.
                        {
                            MessageBox.Show("There have Restore " + Count.ToString(), " Attendance Data");


                        }
                        else
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            MessageBox.Show("Operation failed,Attendance Data not Restore,Please input less number.ErrorCode=" + idwErrorCode.ToString(), "Error");
                        }

                        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

                    }

                    axCZKEM1.Disconnect();
                } // for


        }

        private void btnFPInfo_Click(object sender, EventArgs e)
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsDetails = new DataSet();

            try
            {
                sqlQuery = "Exec prcGetIpAddressGTRTran 6," + Common.Classes.clsMain.intComId + ",'',0,''";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "FingerData";

                gridInfo.DataSource = null;
                gridInfo.DataSource = dsDetails.Tables["FingerData"];


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

        private void btnFP_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", GetDeviceIP = "";
            string DeviceIp = "", pDate = "", pTime = "";

            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            int idwFingerIndex;
            string sTmpData = "";
            int iTmpLength = 0;
            int iFlag = 0;


            //Data Clear
            sqlQuery = "Truncate table tblTempCount_GTR";
            arQuery.Add(sqlQuery);
            clsCon.GTRSaveDataWithSQLCommand(arQuery);


            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
            {
                if (row.Cells["Id"].Text.ToString().Length != 0 &&
                    row.Cells["isChecked"].Value.ToString() == "1" && row.Cells["Active"].Value.ToString() == "1")
                {

                    GetDeviceIP = "" + row.Cells["IpAddress"].Text.ToString() + "";

                    axCZKEM1.EnableDevice(iMachineNumber, false);
                    Cursor = Cursors.WaitCursor;

                    bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                    if (bIsConnected == true)
                    {
                        iMachineNumber = 1;
                        axCZKEM1.RegEvent(iMachineNumber, 65535);
                    }

                    axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
                    axCZKEM1.ReadAllTemplate(iMachineNumber);//read all the users' fingerprint templates to the memory
                    while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
                    {
                        for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                        {
                            if (axCZKEM1.GetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                            {
                                sqlQuery = "Delete tblEmp_FingerData Where EmpId = '" + sdwEnrollNumber + "' and FPIndex = " + idwFingerIndex + "";
                                arQuery.Add(sqlQuery);

                                sqlQuery = "Insert into tblEmp_FingerData(ComId,EmpId,FingerData,FPIndex,Privilege,Password) values('2','" + sdwEnrollNumber + "','" +
                                                sTmpData + "'," + idwFingerIndex + "," + iPrivilege + ",'" + sPassword + "')";
                                arQuery.Add(sqlQuery);

                                sqlQuery = "Insert into tblTempCount_GTR(ComId,EmpId,FingerData,FPIndex,Privilege,Password) values('2','" + sdwEnrollNumber + "','" +
                                                sTmpData + "'," + idwFingerIndex + "," + iPrivilege + ",'" + sPassword + "')";
                                arQuery.Add(sqlQuery);

                            }  //if
                        }  //for
                    }   //while

                    axCZKEM1.EnableDevice(iMachineNumber, true);
                    Cursor = Cursors.Default;

                }   //if


                axCZKEM1.Disconnect();

            }   //foreach

            clsCon.GTRSaveDataWithSQLCommand(arQuery);

            prcGetFingerData();
        }

        private void prcGetFingerData()
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsDetails = new DataSet();

            try
            {
                sqlQuery = "Exec prcGetIpAddressGTRTran 5," + Common.Classes.clsMain.intComId + ",'',0,''";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "FingerData";

                gridInfo.DataSource = null;
                gridInfo.DataSource = dsDetails.Tables["FingerData"];


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

        private void btnFPSave_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", GetDeviceIP = "";
            string DeviceIp = "", pDate = "", pTime = "";

            int idwErrorCode = 0;

            string sdwEnrollNumber = "";
            string sName = "";
            int idwFingerIndex = 0;
            string sTmpData = "";
            int iPrivilege = 0;
            string sPassword = "";
            int iFlag = 0;
            string sEnabled = "";
            bool bEnabled = false;


            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
            {
                if (row.Cells["Id"].Text.ToString().Length != 0 &&
                    row.Cells["isChecked"].Value.ToString() == "1" && row.Cells["Active"].Value.ToString() == "1")
                {

                    GetDeviceIP = "" + row.Cells["IpAddress"].Text.ToString() + "";

                    axCZKEM1.EnableDevice(iMachineNumber, false);
                    Cursor = Cursors.WaitCursor;

                    bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                    if (bIsConnected == true)
                    {
                        iMachineNumber = 1;
                        axCZKEM1.RegEvent(iMachineNumber, 65535);
                    }



                    int rowCount;
                    for (rowCount = 0; rowCount < dsDetails.Tables["FingerData"].Rows.Count; rowCount++)
                    {
                        if (dsDetails.Tables["FingerData"].Rows[rowCount][0].ToString().Trim().Length > 0 &&
                                            dsDetails.Tables["FingerData"].Rows[rowCount]["isChecked"].ToString() == "1" &&
                                            dsDetails.Tables["FingerData"].Rows[rowCount]["FingerData"].ToString().Trim().Length > 0)
                        {
                            sdwEnrollNumber = dsDetails.Tables["FingerData"].Rows[rowCount]["EmpId"].ToString();
                            sName = "";
                            sTmpData = dsDetails.Tables["FingerData"].Rows[rowCount]["FingerData"].ToString();
                            idwFingerIndex = Convert.ToInt32(dsDetails.Tables["FingerData"].Rows[rowCount]["FPIndex"].ToString());
                            iPrivilege = Convert.ToInt32(dsDetails.Tables["FingerData"].Rows[rowCount]["Privilege"].ToString()); ;
                            sPassword = dsDetails.Tables["FingerData"].Rows[rowCount]["Password"].ToString();
                            sEnabled = "true";
                            iFlag = 1;


                            if (sEnabled == "true")
                            {
                                bEnabled = true;
                            }
                            else
                            {
                                bEnabled = false;
                            }

                            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the device
                            {
                                axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the device
                            }
                            else
                            {
                                axCZKEM1.GetLastError(ref idwErrorCode);
                                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                                Cursor = Cursors.Default;
                                axCZKEM1.EnableDevice(iMachineNumber, true);
                                return;
                            }
                        }//if
                    } //for
                    axCZKEM1.RefreshData(iMachineNumber);
                    Cursor = Cursors.Default;
                    axCZKEM1.EnableDevice(iMachineNumber, true);
                    //MessageBox.Show("Successfully Upload fingerprint templates, " + "total:" + lvDownload.Items.Count.ToString(), "Success");


                }   //if


                axCZKEM1.Disconnect();

            }   //foreach


            MessageBox.Show("Finger Data Transfer Successfully Complete.");
        }

        private void cboSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSection.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSection.Width;
            cboSection.DisplayLayout.Bands[0].Columns["sectname"].Header.Caption = "Section";
            cboSection.DisplayLayout.Bands[0].Columns["sectId"].Hidden = true;

            cboSection.DisplayMember = "SectName";
            cboSection.ValueMember = "SectId";
        }

        private void cboBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBand.DisplayLayout.Bands[0].Columns["varName"].Width = cboBand.Width;
            cboBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";

            cboBand.DisplayMember = "varName";
            cboBand.ValueMember = "varName";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            //if (fncBlank())
            //{
            //    return;
            //}
            String Data = "",SectId = "0",Sex = "";


            if (optCriteria.Value == "ALL")
            {
                Data = "ALL";
                SectId = "0";
                Sex = "";
            }
            else if (optCriteria.Value == "Sect")
            {
                Data = "Sect";
                SectId = cboSection.Value.ToString();
                Sex = "";
            }
            else if (optCriteria.Value == "Sex")
            {
                Data = "Sex";
                SectId = "0";
                Sex = cboBand.Value.ToString();
            }
            else if (optCriteria.Value == "Release")
            {
                Data = "Release";
                SectId = "0";
                Sex = "";
            }
            
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsDetails = new DataSet();

            try
            {
                sqlQuery = "Exec prcGetIpAddressGTRTran 7," + Common.Classes.clsMain.intComId + ",'" + Data.ToString() + "','" + SectId.ToString() + "','" + Sex.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "FingerData";

                gridInfo.DataSource = null;
                gridInfo.DataSource = dsDetails.Tables["FingerData"];


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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridInfo.Rows)
                {
                    row.Cells["isChecked"].Value = 1;
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridInfo.Rows)
                {
                    row.Cells["isChecked"].Value = 0;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", DeviceIP = "", GetDeviceIP = "";
            Int32 NewId = 0;
            Int32 RowID;

            string DeviceIp = "", pDate = "", pTime = "";

            int Count = 0;

            int idwErrorCode = 0;


            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
            {
                if (row.Cells["Id"].Text.ToString().Length != 0 &&
                    row.Cells["isChecked"].Value.ToString() == "1" && row.Cells["Active"].Value.ToString() == "1")
                {

                    GetDeviceIP = "" + row.Cells["IpAddress"].Text.ToString() + "";

                    bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                    if (bIsConnected == true)
                    {
                        iMachineNumber = 1;
                        axCZKEM1.RegEvent(iMachineNumber, 65535);
                    }

                    int iDataFlag = 5;

                    Cursor = Cursors.WaitCursor;
                    if (axCZKEM1.ClearData(iMachineNumber, iDataFlag))
                    {
                        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                        MessageBox.Show("Clear all the UserInfo data!", "Success");
                    }
                    else
                    {
                        axCZKEM1.GetLastError(ref idwErrorCode);
                        MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                    }
                    Cursor = Cursors.Default;

                }

                axCZKEM1.Disconnect();
            } // for
        }
      }
   }


