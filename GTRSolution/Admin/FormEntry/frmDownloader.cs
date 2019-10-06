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
using System.Globalization;


namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmDownloader : Form
    {

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private System.Data.DataSet dsEmpList;

        private bool bIsConnected = false;

        //CZKEMClass axCZKEM1 = new CZKEMClass();
        CZKEM axCZKEM1 = new CZKEM();
        private int iMachineNumber = 1;


        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;


        public string GTRHexToDecCon(string proxCardNo)
        {
            string str = int.Parse(proxCardNo, NumberStyles.HexNumber).ToString();
            switch (str.Length)
            {
                case 3:
                    str = "0000000" + str;
                    break;
                case 4:
                    str = "000000" + str;
                    break;
                case 5:
                    str = "00000" + str;
                    break;
                case 6:
                    str = "0000" + str;
                    break;
                case 7:
                    str = "000" + str;
                    break;
                case 8:
                    str = "00" + str;
                    break;
                case 9:
                    str = "0" + str;
                    break;
            }
            return str;
        }

        //Convert Dec To Hex
        public string GTRDecToHexCon(string proxCardNo)
        {
            string str = int.Parse(proxCardNo).ToString("X");
            ///string str = number.ToString("X");
            switch (str.Length)
            {

                case 3:
                    str = "0000000" + str;
                    break;
                case 4:
                    str = "000000" + str;
                    break;                
                case 5:
                    str = "00000" + str;
                    break;
                case 6:
                    str = "0000" + str;
                    break;
                case 7:
                    str = "000" + str;
                    break;
                case 8:
                    str = "00" + str;
                    break;
                case 9:
                    str = "0" + str;
                    break;
            }
            return str;
        }


        public frmDownloader(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmDownloader_FormClosing(object sender, FormClosingEventArgs e)
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


        private void frmDownloader_Load(object sender, EventArgs e)
        {
            try
            {

                chkClear.Checked = false;
                btnSave.Enabled = true;

                prcLoadStatus();
                prcLoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcLoadStatus()
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {
                sqlQuery = "Update tblMachineNo_GTR set  Status = 'Disconnect'";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

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

        private void prcLoadList()
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            try
            {              
                sqlQuery = "Exec prcGetIpAddressGTR 0," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";

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



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dsList.Tables["Grid"].NewRow();

            dsList.Tables["Grid"].Rows.Add(dr);

            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            int NewId = 0;
            string sqlQuery = "";

            try
            {
                int rowCount;
                for (rowCount = 0; rowCount < dsList.Tables["Grid"].Rows.Count; rowCount++)
                {

                    if (dsList.Tables["Grid"].Rows[rowCount][0].ToString().Trim().Length > 0 &&
                        dsList.Tables["Grid"].Rows[rowCount]["Flag"].ToString() == "1")
                    {
                        //Update Table
                        sqlQuery = "Update tblMachineNo_GTR Set IpAddress = '" + dsList.Tables["Grid"].Rows[rowCount][2] +
                                   "', Location = '" + dsList.Tables["Grid"].Rows[rowCount][3] + "', isActive = '" +
                                   dsList.Tables["Grid"].Rows[rowCount][4] + "', isTFT = '" + 
                                   dsList.Tables["Grid"].Rows[rowCount][5] + "' where Id = '" +
                                   dsList.Tables["Grid"].Rows[rowCount][1] + "' and ComId = " + Common.Classes.clsMain.intComId + "";
                        arQuery.Add(sqlQuery);

                    }
                    else if (dsList.Tables["Grid"].Rows[rowCount][0].ToString().Trim().Length == 0)
                    {

                        // Insert To Table
                        sqlQuery = "Insert into tblMachineNo_GTR(ComId, IpAddress, Location, isActive, isTFT) values(" + Common.Classes.clsMain.intComId + ",'" +
                                   dsList.Tables["Grid"].Rows[rowCount][2] + "','" +
                                   dsList.Tables["Grid"].Rows[rowCount][3] + "','" +
                                   dsList.Tables["Grid"].Rows[rowCount][4] + "','" +
                                   dsList.Tables["Grid"].Rows[rowCount][5] + "')";
                        arQuery.Add(sqlQuery);

                    }

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);
                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved [Or/And ] Update Succefully");

                prcLoadList();

                btnSave.Enabled = false;

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

        private void chkClear_CheckedChanged(object sender, EventArgs e)
        {
            chkClear.Tag = 0;
            if (chkClear.Checked == true)
            {
                chkClear.Tag = 1;
            }
        }
        private void prcbtnDisable()
        {

            btnClose.Enabled = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnGetTime.Enabled = false;
            btnSetTime.Enabled = false;
            btnDownload.Enabled = false;
            btnLog.Enabled = false;
        }

        private void prcbtnActivate()
        {

            btnClose.Enabled = true;
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            btnGetTime.Enabled = true;
            btnSetTime.Enabled = true;
            btnDownload.Enabled = true;
            btnLog.Enabled = true;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {

            prcbtnDisable();
            prcExecuteDownload();
            prcbtnActivate();

        }


        private void prcExecuteDownload()
        {

            gridInfo.DataSource = null;

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            try
            {

                string sqlQuery = "", DeviceIP = "", GetDeviceIP = "";
                Int32 NewId = 0;
                Int32 RowID;

                string sdwEnrollNumber = "", DeviceIp = "", pDate = "", pTime = "", CardNo = "", EnrollID ="";
                int idwTMachineNumber = 0, idwEMachineNumber = 0;
                int idwVerifyMode = 0, idwInOutMode = 0, idwEnrollNumber = 0, EnrollData = 0;

                int idwYear = 0, idwMonth = 0, idwDay = 0;
                int idwHour = 0, idwMinute = 0, idwSecond = 0;

                int idwWorkcode = 0, idwErrorCode = 0;

                int iGLCount = 0, iIndex = 0;

                //Raw Data Clear
                sqlQuery = "Truncate Table tblRawdataMIGTR_Temp";
                arQuery.Add(sqlQuery);


                sqlQuery = "Update tblMachineNo_GTR set  Status = 'Disconnect'";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

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
                        else
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            MessageBox.Show("Device not connected. Please check your device or network connection.Problem IP Address = " + GetDeviceIP.ToString(),"Error");
                        }

                        //ZK Black & White Device Download Code

                        if (row.Cells["BlackWhite"].Value.ToString() == "1")
                        {
                            sqlQuery = "Update tblMachineNo_GTR set  Status = 'Connect' where IpAddress ='" + row.Cells["IpAddress"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);


                            Cursor = Cursors.WaitCursor;
                            //lvLogs.Items.Clear();
                            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                            if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
                            {
                                while (axCZKEM1.GetGeneralLogData(iMachineNumber, ref idwTMachineNumber, ref idwEnrollNumber,
                                            ref idwEMachineNumber, ref idwVerifyMode, ref idwInOutMode, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute))//get records from the memory
                                {
                                    //iGLCount++;

                                    DeviceIp = GetDeviceIP.Substring(7);

                                    //CardNo = idwEnrollNumber.ToString();
                                    //CardNo = GTRDecToHexCon(CardNo);

                                    EnrollData = int.Parse(sdwEnrollNumber);
                                    //EnrollID = EnrollData.ToString("0000000000");

                                    //pDate = idwDay + "-" + idwMonth + "-" + idwYear;
                                    pDate = idwYear + "-" + idwMonth + "-" + idwDay;
                                    pTime = idwHour + ":" + idwMinute + ":" + idwSecond;


                                    sqlQuery = "Insert into tblRawdataMIGTR_Temp(ComId,DeviceNo,CardNo,dtPunchDate,dtPunchTime) values(" + Common.Classes.clsMain.intComId + ",'" +
                                                DeviceIp + "','" + sdwEnrollNumber + "','" + pDate + "','" + pTime + "')";
                                    arQuery.Add(sqlQuery);


                                    sqlQuery = "Insert into tblRawdata(ComId,DeviceNo,CardNo,dtPunchDate,dtPunchTime,LUserID,PCName) values(" + Common.Classes.clsMain.intComId + ",'" +
                                                DeviceIp + "','" + sdwEnrollNumber + "','" + pDate + "','" + pTime + "'," + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + Common.Classes.clsMain.strComputerName + "')";
                                    arQuery.Add(sqlQuery);


                                    //iIndex++;
                                } //while

                            }  //if

                            if (chkClear.Checked == true)
                            {
                                axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                                if (axCZKEM1.ClearGLog(iMachineNumber))
                                {
                                    axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                                    //MessageBox.Show("All att Logs have been cleared from teiminal!", "Success");
                                }
                            }

                            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                            Cursor = Cursors.Default;

                            axCZKEM1.Disconnect();


                        }  //if

                        //ZK Color Device Download Code
                        else
                        {
                            sqlQuery = "Update tblMachineNo_GTR set  Status = 'Connect' where IpAddress ='" + row.Cells["IpAddress"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);
                            
                            Cursor = Cursors.WaitCursor;
                            //lvLogs.Items.Clear();
                            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                            if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
                            {
                                while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                                {
                                    //iGLCount++;

                                    DeviceIp = GetDeviceIP.Substring(7);
                                    
                                    //CardNo = sdwEnrollNumber;
                                    //CardNo = GTRDecToHexCon(CardNo);

                                    EnrollData = int.Parse(sdwEnrollNumber);
                                    EnrollID = EnrollData.ToString("0000000000");

                                    //pDate = idwDay + "-" + idwMonth + "-" + idwYear;
                                    pDate = idwYear + "-" + idwMonth + "-" + idwDay;
                                    pTime = idwHour + ":" + idwMinute + ":" + idwSecond;


                                    sqlQuery = "Insert into tblRawdataMIGTR_Temp(ComId,DeviceNo,CardNo,dtPunchDate,dtPunchTime) values(" + Common.Classes.clsMain.intComId + ",'" +
                                                DeviceIp + "','" + EnrollID + "','" + pDate + "','" + pTime + "')";
                                    arQuery.Add(sqlQuery);


                                    sqlQuery = "Insert into tblRawdata(ComId,DeviceNo,CardNo,dtPunchDate,dtPunchTime,LUserID,PCName) values(" + Common.Classes.clsMain.intComId + ",'" +
                                                DeviceIp + "','" + EnrollID + "','" + pDate + "','" + pTime + "'," + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + Common.Classes.clsMain.strComputerName + "')";
                                    arQuery.Add(sqlQuery);


                                    //iIndex++;
                                } //while

                            }  //if

                            if (chkClear.Checked == true)
                            {
                                axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                                if (axCZKEM1.ClearGLog(iMachineNumber))
                                {
                                    axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                                    //MessageBox.Show("All att Logs have been cleared from teiminal!", "Success");
                                }
                            }

                            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                            Cursor = Cursors.Default;

                            axCZKEM1.Disconnect();

                        } //else



                    } //if
                } //foreach

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Download')";
                arQuery.Add(sqlQuery);

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcLoadList();
                prcRawData();

            }  //try
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }
        }

        private void prcRawData()
        {


            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsDetails = new DataSet();
            try
            {
                sqlQuery = "Exec prcGetIpAddressGTR 1," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Raw";

                gridInfo.DataSource = null;
                gridInfo.DataSource = dsDetails.Tables["Raw"];
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

        private void btnLog_Click(object sender, EventArgs e)
        {
            prcbtnDisable();
            prcDataLog();
            prcbtnActivate();

        }

        private void prcDataLog()
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", DeviceIP = "", GetDeviceIP = "";
            Int32 NewId = 0;
            Int32 RowID;

            string DeviceIp = "", pDate = "", pTime = "";

            int idwYear = 0, idwMonth = 0, idwDay = 0;
            int idwHour = 0, idwMinute = 0, idwSecond = 0;

            int idwErrorCode = 0;


            try
            {

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

                        bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                        if (bIsConnected == true)
                        {
                            iMachineNumber = 1;
                            axCZKEM1.RegEvent(iMachineNumber, 65535);
                        }

                        int iValue = 0;

                        axCZKEM1.EnableDevice(iMachineNumber, true);//disable the device
                        if (axCZKEM1.GetDeviceStatus(iMachineNumber, 6, ref iValue)) //Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
                        {
                            //MessageBox.Show("The count of the AttLogs in the device is " + iValue.ToString(), "Success");

                            sqlQuery = "Insert into tblTempCount_GTR(ComId,IpAddress,Cnt) values(" + Common.Classes.clsMain.intComId + ",'" +
                                        GetDeviceIP + "','" + iValue + "')";
                            arQuery.Add(sqlQuery);

                        }
                        else
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                        }
                        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

                    }

                    axCZKEM1.Disconnect();
                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcGetDataLog();

            }//try

            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void prcGetDataLog()
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsDetails = new DataSet();

            try
            {
                sqlQuery = "Exec prcGetIpAddressGTR 4," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "DeviceLog";

                gridInfo.DataSource = null;
                gridInfo.DataSource = dsDetails.Tables["DeviceLog"];


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

        private void btnGetTime_Click(object sender, EventArgs e)
        {

            prcbtnDisable();
            prcGetTime();
            prcbtnActivate();


        }

        private void prcGetTime()
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", DeviceIP = "", GetDeviceIP = "";
            Int32 NewId = 0;
            Int32 RowID;

            string DeviceIp = "", pDate = "", pTime = "";

            int idwYear = 0, idwMonth = 0, idwDay = 0;
            int idwHour = 0, idwMinute = 0, idwSecond = 0;

            int idwErrorCode = 0;


            try
            {
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

                        bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                        if (bIsConnected == true)
                        {
                            iMachineNumber = 1;
                            axCZKEM1.RegEvent(iMachineNumber, 65535);
                        }

                        Cursor = Cursors.WaitCursor;
                        if (axCZKEM1.GetDeviceTime(iMachineNumber, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond))
                        {
                            DeviceIp = GetDeviceIP;
                            pDate = idwYear + "-" + idwMonth + "-" + idwDay;
                            pTime = idwHour + ":" + idwMinute + ":" + idwSecond;

                            sqlQuery = "Insert into tblTempCount_GTR(ComId,IpAddress,dtDate,dtTime) values(" + Common.Classes.clsMain.intComId + ",'" +
                                        DeviceIp + "','" + pDate + "','" + pTime + "')";
                            arQuery.Add(sqlQuery);

                        }
                        else
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                        }
                        Cursor = Cursors.Default;


                    }

                    axCZKEM1.Disconnect();
                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcGetDate();
            }

            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }


        private void prcGetDate()
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsDetails = new DataSet();

            try
            {
                sqlQuery = "Exec prcGetIpAddressGTR 2," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "DeviceDate";

                gridInfo.DataSource = null;
                gridInfo.DataSource = dsDetails.Tables["DeviceDate"];


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

        private void btnSetTime_Click(object sender, EventArgs e)
        {

            prcbtnDisable();
            prcSetTime();
            prcbtnActivate();

        }

        private void prcSetTime()
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", DeviceIP = "", GetDeviceIP = "";
            Int32 NewId = 0;
            Int32 RowID;

            string DeviceIp = "", pDate = "", pTime = "";

            int idwYear = 0, idwMonth = 0, idwDay = 0;
            int idwHour = 0, idwMinute = 0, idwSecond = 0;

            int idwErrorCode = 0;

            try
            {
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

                        bIsConnected = axCZKEM1.Connect_Net(GetDeviceIP, 4370);
                        if (bIsConnected == true)
                        {
                            iMachineNumber = 1;
                            axCZKEM1.RegEvent(iMachineNumber, 65535);
                        }


                        Cursor = Cursors.WaitCursor;
                        if (axCZKEM1.SetDeviceTime(iMachineNumber))
                        {

                            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                            //MessageBox.Show("Successfully set the time of the machine and the terminal to sync PC!", "Success");

                            if (axCZKEM1.GetDeviceTime(iMachineNumber, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond))
                            {
                                DeviceIp = GetDeviceIP;
                                pDate = idwYear + "-" + idwMonth + "-" + idwDay;
                                pTime = idwHour + ":" + idwMinute + ":" + idwSecond;

                                sqlQuery = "Insert into tblTempCount_GTR(ComId,IpAddress,dtDate,dtTime) values(" + Common.Classes.clsMain.intComId + ",'" +
                                            DeviceIp + "','" + pDate + "','" + pTime + "')";
                                arQuery.Add(sqlQuery);

                            }

                        }

                        else
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                        }
                        Cursor = Cursors.Default;
                    }

                    axCZKEM1.Disconnect();
                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcGetDate();
            }

            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
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

        private void gridDetails_AfterCellUpdate(object sender, CellEventArgs e)
        {
            gridDetails.ActiveRow.Cells[7].Value = 1;
            btnSave.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete this IP Address ?", "",
                        System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }


            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";

            try
            {

                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                {
                    if (row.Cells["Id"].Text.ToString().Length != 0 &&
                        row.Cells["isChecked"].Value.ToString() == "1" && row.Cells["IpAddress"].Text.ToString().Length != 0)
                    {

                        //Delete Data
                        sqlQuery = "Delete from tblMachineNo_GTR Where Id= '" + row.Cells["Id"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);


                    }

                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Succefully Complete.");

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

        private void ultraGroupBox2_Click(object sender, EventArgs e)
        {

        }







        }
   }


