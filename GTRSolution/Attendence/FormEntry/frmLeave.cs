using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using GTRLibrary;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmLeave : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private System.Data.DataSet dsLeaveBalance;
        private clsProcedure clsProc = new clsProcedure();
        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private int secId_update = 0; // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmLeave(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmLeave_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetail = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        public Boolean fncBlank()
        {
            if(cboEmpCode.Text.Trim() == "")
            {
                MessageBox.Show("Provide Employee Code.");
                cboEmpCode.Focus();
                return true;
            }
            else if (cboEmpCode.IsItemInList(cboEmpCode.Text) == false)
            {
                MessageBox.Show("Select Employee Code From.");
                cboEmpCode.Focus();
                return true;
            }

            if(cboType.Text.Trim( ) == "")
            {
                MessageBox.Show("Provide Leave Type.");
                cboType.Focus();
                return true;
            }
            else if (cboType.IsItemInList(cboType.Text) == false)
            {
                MessageBox.Show("Select Leave Type From.");
                cboType.Focus();
                return true;
            }

            if (dtInput.Text == "")
            {
                dtInput.Text = DateTime.Today.ToString();

            }
            if (dtDateFrom.Text == "")
            {
                dtDateFrom.Text = DateTime.Today.ToString();
            }
            if (dtDateTo.Text == "")
            {
                MessageBox.Show("Select To Date");
            }

            if (txtTotalDays.Text.Trim() == "")
            {
                txtTotalDays.Text = (dtDateTo.DateTime - dtDateFrom.DateTime).Days.ToString() + 1;
            }

            if (txtApproved.Text.Trim() == "")
            {
                MessageBox.Show("Provide Number of days Approved.");
                txtApproved.Focus();
                return true;
            }
            if(cboDuration.Value.ToString()=="1")
            {
                float hour = float.Parse(txtTotalDays.Text.ToString());
                txtTotalDays.Text = (hour*0.125).ToString();
                //((Ttime - Ftime) * 0.08).ToString();

                //    if(txtFromHr.Text.ToString().Trim()=="" || txtFromHr.Text.ToString().Trim()=="hh:mm")
                //    {
                //        MessageBox.Show("Provede From Hour.");
                //        return true;
                //    }
                //    else if (txtToHr.Text.ToString().Trim() == "" || txtToHr.Text.ToString().Trim() == "hh:mm")
                //    {
                //        MessageBox.Show("Provede To Hour.");
                //        return true;
                //    }
                //    else
                //    {
                //        for(int i = 0; i <= txtFromHr.Text.Length; i++)
                //        {
                //            string str = txtFromHr.Text.ToString().Trim();
                //            if(str[i] != '0' ||str[i] != '1' ||str[i] != '2' ||str[i] != '3' ||str[i] != '4' ||str[i] != '5' ||str[i] != '6' ||str[i] != '7' ||str[i] != '8' ||str[i] != '9' ||str[i] != ':' )
                //            {
                //                MessageBox.Show("Insert only Number And ':' . ");
                //                return true;
                //            }
                //            str = txtToHr.Text.ToString().Trim();
                //            if (str[i] != '0' || str[i] != '1' || str[i] != '2' || str[i] != '3' || str[i] != '4' || str[i] != '5' || str[i] != '6' || str[i] != '7' || str[i] != '8' || str[i] != '9' || str[i] != ':')
                //            {
                //                MessageBox.Show("Insert only Number And ':' . ");
                //                return true;
                //            }
                //        }

                //    }
                //    ////////////////
                //    float Ftime, Ttime;
                //    Ftime = hour(txtFromHr.Text.ToString().Trim());
                //    Ttime = hour(txtToHr.Text.ToString().Trim());
                //    if(cboFromAMPM.Value == cboToAMPM.Value)
                //    {
                //        if(Ttime < Ftime )
                //        {
                //            MessageBox.Show("To Hour Must Greater Then From Houre [OR] Select AM/PM Correctly.");
                //        }
                //        else
                //        {
                //            txtTotalDays.Text = ((Ttime - Ftime)*0.08).ToString();
                //        }
                //    }
                //    else
                //    {
                //        txtTotalDays.Text = (((Ttime + 12) - Ftime) * 0.08).ToString();
                //    }
                //    //////////////

            }

            return false;
        }

        public float hour(string str)
        {
            float hh, mm, time;
            hh = float.Parse(str.Substring(0,str.IndexOf(":")-1));
            mm = float.Parse(str.Substring(str.IndexOf(":") + 1,str.Length-1 ));
            time = hh + mm;
            return time;
        }



        public void prcLoadList( string str)
        {

            var DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DaysInMonth);
            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtFrom.Value = firstDay;
            dtTo.Value = lastDay;
            
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetLeave -1,'" + clsProc.GTRDate(str) + "'," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Leave";
                dsList.Tables[1].TableName = "EmpCode";
                dsList.Tables[2].TableName = "EntryToday";
                dsList.Tables[3].TableName = "LeaveBalance";
                dsList.Tables[4].TableName = "Duration";
                //dsList.Tables[5].TableName = "AmPm";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["EntryToday"];

                gridLeaveBalance .DataSource = null;
                gridLeaveBalance.DataSource = dsList.Tables["LeaveBalance"];
            
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

        public void prcLoadListIndividual(string str)
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "		Select A.lvId, A.empId, B.empCode, B.empName, A.lvType, CONVERT(varchar,A.dtFrom,106) as dtFrom, CONVERT(Varchar, A.dtTo,106)As dtTo, A.totalDay, A.lvApp, A.Remark  From tblLeave_Avail A Inner Join tblEmp_Info B On B.empID = A.EmpId where A.empid = " + cboEmpCode.Value + "order by lvId desc";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Leave";

                //dsList.Tables[5].TableName = "AmPm";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Leave"];

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


        public void prcLoadLeaveBalance( string str)
        {
            clsConnection clsCon = new clsConnection();
            dsLeaveBalance = new System.Data.DataSet();

            DateTime FirstYear = new DateTime(dtInput.DateTime.Year, 1, 1);

            try
            {
                string sqlQuery = "Exec prcGetLeaveBalance  " + Int32.Parse(str) + ",'" + clsProc.GTRDate(FirstYear.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsLeaveBalance, sqlQuery);
                dsLeaveBalance.Tables[0].TableName = "LeaveBalance";

                gridLeaveBalance.DataSource = null;
                gridLeaveBalance.DataSource = dsLeaveBalance.Tables["LeaveBalance"];
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

        public void prcLoadCombo()
        {
            cboType .DataSource = null;
            cboType.DataSource = dsList.Tables["Leave"];

            cboEmpCode.DataSource = null;
            cboEmpCode.DataSource = dsList.Tables["EmpCode"];

            cboDuration .DataSource = null;
            cboDuration.DataSource = dsList.Tables["Duration"];

            //cboFromAMPM .DataSource = null;
            //cboFromAMPM.DataSource = dsList.Tables["AmPm"];

            //cboToAMPM .DataSource = null;
            //cboToAMPM.DataSource = dsList.Tables["AmPm"];
        }

        public void prcDisplayDetails(string strParam, string dt)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetLeave  " + Int32.Parse(strParam) + ", '" + clsProc.GTRDate(dt) + "'," + Common.Classes.clsMain.intComId + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, sqlQuery);
                dsDetail.Tables[0].TableName = "Shift";
                DataRow dr;

                if (dsDetail.Tables["Shift"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["Shift"].Rows[0];
                    txtId.Text = dr["lvId"].ToString();
                    cboEmpCode.Value = dr["empid"].ToString();
                    cboType.Text = dr["lvType"].ToString();
                    dtInput.Text = dr["dtInput"].ToString();
                    dtDateFrom.Text = dr["dtFrom"].ToString();
                    dtDateTo.Text = dr["dtTo"].ToString();
                    txtTotalDays.Text = dr["totalDay"].ToString();
                    txtApproved.Text = dr["lvApp"].ToString();
                    txtRemarks.Text = dr["Remark"].ToString();

                    this.btnSave.Text = " &Update";
                    this.btnDelete.Enabled = true;
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

        public void prcClearData()
        {
            cboDuration.Value = 0;
            //cboFromAMPM.Value = 0;
            //cboToAMPM.Value = 0;
            dtInput.Value = DateTime.Today;
            dtDateFrom.Value = DateTime.Today;
            dtDateTo.Value = DateTime.Today;
            dtDateFrom.Enabled = true ;
           // dtDateTo.Enabled = true ;


            txtId.Text = "";
            cboEmpCode.Text = "";
            cboType.Text = "";
            dtInput.Text = DateTime.Today.ToString();
            dtDateFrom.Text = DateTime.Today.ToString();
            dtDateTo.Text = DateTime.Today.ToString();
            txtTotalDays.Text = "";
            txtApproved.Text = "0";
            txtRemarks.Text = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

        }

        private void frmLeave_Load(object sender, EventArgs e)
        {
            try
            {
                cboDuration.Value = 0;
                //cboFromAMPM.Value = 0;
                //cboToAMPM.Value = 0;
                //dtInput.Enabled = false;
                dtInput.Value = DateTime.Today;
                dtDateFrom .Value = DateTime.Today;
                dtDateTo.Value = DateTime.Today;
                dtDateTo.Enabled = false;

                prcLoadList(DateTime.Today.ToString());
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



        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            DateTime FirstYearDay = new DateTime(dtDateTo.DateTime.Year, 1, 1);

            string sqlQuery = "";
            Int64 NewId = 0;
            Int64 ChkLv = 0;
            Int64 ChkData = 0;

            //sqlQuery = "Select dbo.fncCheckDataLeave (" + Common.Classes.clsMain.intComId + ",'" + cboEmpCode.Value.ToString() + "','" + cboType.Text.ToString() + "','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "')";
            //ChkData = clsCon.GTRCountingDataLarge(sqlQuery);

            //if (ChkData == 1)
            //{
            //    MessageBox.Show("This employee's joining date not over 90 days(3 months). So, CL Leave not Acceptable. Please select another employee Id.");
            //    return;
            //}


            try
            {
                if (btnSave.Text.ToString().ToUpper() != "&Save".ToUpper())
                {
                    //Update  

                            if (cboType.Text == "CLH")
                            {
                                 cboType.Text = "CL";
                            }
                            else if (cboType.Text == "SLH")
                            {
                                 cboType.Text = "SL";
                            }
                            else if (cboType.Text == "ELH")
                            {
                                 cboType.Text = "EL";
                            }

                        //if (float.Parse(txtApproved.Text.ToString()) <= (float.Parse(gridLeaveBalance.Rows[0].Cells[cboType.Text.ToString()].Value.ToString())) - (float.Parse(gridLeaveBalance.Rows[0].Cells["A" + cboType.Text.ToString() + ""].Value.ToString())))
                        //{
                            sqlQuery = "update tblLeave_Balance Set A" + dsDetail.Tables[0].Rows[0]["lvType"].ToString() +
                                       " = A" + dsDetail.Tables[0].Rows[0]["lvType"].ToString() + " - " +
                                       dsDetail.Tables[0].Rows[0]["lvApp"].ToString() + "  where EmpId = " +
                                       dsDetail.Tables[0].Rows[0]["empId"].ToString() + " and dtOpBal = '" + clsProc.GTRDate(FirstYearDay.ToString()) + "'";
                            arQuery.Add(sqlQuery);
                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType,EmpId)"
                                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" +
                                       sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update'," + dsDetail.Tables[0].Rows[0]["empId"].ToString() + ")";
                            arQuery.Add(sqlQuery);

                            //-------------------------
                            sqlQuery = "update tblLeave_Avail set EmpId = '" + cboEmpCode.Value + "', dtInput ='" +
                                        clsProc.GTRDate(dtInput.DateTime.ToString())  + "', dtFrom = '" +clsProc.GTRDate(dtDateFrom.DateTime.ToString())+
                                       "', dtTo = '" +clsProc.GTRDate(dtDateTo.DateTime.ToString())+ "', totalDay = '" +
                                       txtTotalDays.Text.ToString() + "', lvType = '" + cboType.Text.ToString() +
                                       "', lvApp = '" + txtApproved.Text.ToString() + "', Remark = '" +
                                       txtRemarks.Text.ToString() + "' where lvId = '" +
                                       int.Parse(txtId.Text.ToString()) + "'";
                            arQuery.Add(sqlQuery);
                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType,EmpId)"
                                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" +
                                       sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update'," + cboEmpCode.Value.ToString() + ")";
                            arQuery.Add(sqlQuery);
                            //--------------------------------
                            sqlQuery = "Update tblLeave_Balance Set A" + cboType.Text.ToString() + "  = A" +
                                       cboType.Text.ToString() + " + " + float.Parse(txtApproved.Text.ToString()) +
                                       " where EmpId = " + cboEmpCode.Value.ToString() + " and dtOpBal = '" + clsProc.GTRDate(FirstYearDay.ToString()) + "'";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" +
                                       sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update'," + cboEmpCode.Value.ToString() + ")";
                            arQuery.Add(sqlQuery);

                            sqlQuery = "prcProcessLeave " + Common.Classes.clsMain.intComId + ",'" + cboEmpCode.Value.ToString() + "','" 
                                      + int.Parse(txtId.Text.ToString()) + "','"
                                      + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "','"
                                      + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                            arQuery.Add(sqlQuery);

                            clsCon.GTRSaveDataWithSQLCommand(arQuery);

                            MessageBox.Show("Data Updated Succefully");

                            prcLoadList(DateTime.Today.ToString());
                            prcLoadCombo();
                            prcClearData();

                        //}
                        //else
                        //{
                        //    MessageBox.Show("You Have Not " + txtApproved.Text.ToString() + " Days as " + cboType.Text.ToString() + " Leave.");
                        //}
                    
                }
                else ////Insert to table
                {


                    sqlQuery = "Select dbo.fncCheckEmpLeave (" + Common.Classes.clsMain.intComId + ", '" + cboEmpCode.Value.ToString() + "','" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "','" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "')";
                    ChkLv = clsCon.GTRCountingDataLarge(sqlQuery);


                    if (ChkLv == 1)
                    {
                        MessageBox.Show("This Employee Leave Date ['" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "'] or ['" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'] already inputed. Please input another leave Date.");
                        return;
                    }

                    sqlQuery = "Select dbo.fncCheckEmpStatus (" + Common.Classes.clsMain.intComId + ", '" + cboEmpCode.Value.ToString() + "','" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "','" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "')";
                    ChkLv = clsCon.GTRCountingDataLarge(sqlQuery);


                    if (ChkLv == 1)
                    {
                        MessageBox.Show("This Employee is Present from Date ['" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "'] to ['" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "']. Please Check Job Card.");
                        return;
                    }
                    sqlQuery = " Select Isnull(Max(lvId),0)+1 As NewId from tblLeave_Avail";
                    NewId = clsCon.GTRCountingDataLarge(sqlQuery);

                    if (float.Parse(txtApproved.Text.ToString()) <= (float.Parse(gridLeaveBalance.Rows[0].Cells[cboType.Text.ToString()].Value.ToString())) - (float.Parse(gridLeaveBalance.Rows[0].Cells["A" + cboType.Text.ToString() + ""].Value.ToString())))
                    {
                        
                    //Insert to Table
                    ////--------------------------
                        sqlQuery =
                            "Insert into tblLeave_Avail(ComId, lvId, EmpId, dtInput, dtFrom, dtTo, totalDay, lvType, lvApp, Remark, PCName, LUserId)" +
                            "Values('" + Common.Classes.clsMain.intComId + "','" + NewId + "','" +
                            cboEmpCode.Value.ToString() + "','" + clsProc.GTRDate(dtInput.DateTime.ToString()) + "','" +
                            clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "','" +
                            clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "','" + txtTotalDays.Text.ToString() + "','" +
                            cboType.Text.ToString() + "','" + txtApproved.Text.ToString() + "','" +
                            txtRemarks.Text.ToString() + "','" + Common.Classes.clsMain.strComputerName + "','" +
                            Common.Classes.clsMain.intUserId + "')";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType,EmpId)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                                   sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert','" + cboEmpCode.Value.ToString() + "')";
                        arQuery.Add(sqlQuery);

                        if (cboType.Text == "CLH")
                        {
                            cboType.Text = "CL";
                        }
                        else if (cboType.Text == "SLH")
                        {
                            cboType.Text = "SL";
                        }
                        else if (cboType.Text == "ELH")
                        {
                            cboType.Text = "EL";
                        }

                        sqlQuery = "Update tblLeave_Balance Set A" + cboType.Text.ToString() + "  = A" +
                                   cboType.Text.ToString() + " + " + float.Parse(txtApproved.Text.ToString()) +
                                   " where EmpId = " + cboEmpCode.Value.ToString() + " and dtOpBal = '" + clsProc.GTRDate(FirstYearDay.ToString()) + "'";
                        arQuery.Add(sqlQuery );

                        sqlQuery = "prcProcessLeave " + Common.Classes.clsMain.intComId + ",'" + cboEmpCode.Value.ToString() + "','" + NewId + "','" 
                                                      + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "','" 
                                                      + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                        arQuery.Add(sqlQuery);

                        ////----------------------------------------

                        clsCon.GTRSaveDataWithSQLCommand(arQuery);
                        MessageBox.Show("Data Saved Succefully");

                        prcLoadList(DateTime.Today.ToString());
                        prcLoadCombo();
                        prcClearData();
                    }
                    else
                    {
                        MessageBox.Show("" + cboType.Text.ToString() + " Leave balance over.You can not input " + txtApproved.Text.ToString() + " days as " + cboType.Text.ToString() + " Leave.");
                    }
                }

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Leave Information of [" + gridList.ActiveRow.Cells[2].Text.ToString() + "]", "",
                    System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            DateTime FirstYear = new DateTime(dtDateFrom.DateTime.Year, 1 , 1);

            string sqlQuery = "";

            try
            {

                if (cboType.Text == "ML")
                {


                    sqlQuery = "Update B Set B.AML = 0 from tblLeave_Balance B,tblLeave_Avail A where B.EmpID = A.EmpID and A.lvId = '" + Int64.Parse(txtId.Text.ToString()) +
                               "' And A.lvType = 'ML' and B.dtOpBal = '" + clsProc.GTRDate(FirstYear.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                    //Delete Data
                    sqlQuery = "Delete from tblLeave_Avail where lvId = '" + Int64.Parse(txtId.Text.ToString()) + "' And lvType = '" + cboType.Text.ToString() +
                                           "' and lvApp = '" + txtApproved.Text.ToString() + "' and dtFrom = '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) +
                                           "' and dtTo = '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                    arQuery.Add(sqlQuery);
                }
                else if (cboType.Text == "CL" || cboType.Text == "CLH")
                {


                    sqlQuery = "Update B Set B.ACL = B.ACL - '" + txtApproved.Text.ToString() + "' from tblLeave_Balance B,tblLeave_Avail A where B.EmpID = A.EmpID and A.lvId = '" + Int64.Parse(txtId.Text.ToString()) +
                               "' And A.lvType in ('CL','CLH') and B.dtOpBal = '" + clsProc.GTRDate(FirstYear.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                    //Delete Data
                    sqlQuery = "Delete from tblLeave_Avail where lvId = '" + Int64.Parse(txtId.Text.ToString()) + "' And lvType = '" + cboType.Text.ToString() +
                                           "' and lvApp = '" + txtApproved.Text.ToString() + "' and dtFrom = '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) +
                                           "' and dtTo = '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                    arQuery.Add(sqlQuery);
                }

                else if (cboType.Text == "SL" || cboType.Text == "SLH")
                {


                    sqlQuery = "Update B Set B.ASL = B.ASL - '" + txtApproved.Text.ToString() + "' from tblLeave_Balance B,tblLeave_Avail A where B.EmpID = A.EmpID and A.lvId = '" + Int64.Parse(txtId.Text.ToString()) +
                               "' And A.lvType in ('SL','SLH') and B.dtOpBal = '" + clsProc.GTRDate(FirstYear.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                    //Delete Data
                    sqlQuery = "Delete from tblLeave_Avail where lvId = '" + Int64.Parse(txtId.Text.ToString()) + "' And lvType = '" + cboType.Text.ToString() +
                                           "' and lvApp = '" + txtApproved.Text.ToString() + "' and dtFrom = '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) +
                                           "' and dtTo = '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                    arQuery.Add(sqlQuery);
                }


                else if (cboType.Text == "EL" || cboType.Text == "ELH")
                {


                    sqlQuery = "Update B Set B.AEL = B.AEL - '" + txtApproved.Text.ToString() + "' from tblLeave_Balance B,tblLeave_Avail A where B.EmpID = A.EmpID and A.lvId = '" + Int64.Parse(txtId.Text.ToString()) +
                               "' And A.lvType in ('EL','ELH') and B.dtOpBal = '" + clsProc.GTRDate(FirstYear.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                    //Delete Data
                    sqlQuery = "Delete from tblLeave_Avail where lvId = '" + Int64.Parse(txtId.Text.ToString()) + "' And lvType = '" + cboType.Text.ToString() +
                                           "' and lvApp = '" + txtApproved.Text.ToString() + "' and dtFrom = '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) +
                                           "' and dtTo = '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                    arQuery.Add(sqlQuery);
                }


                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType,EmpId)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete','" + cboEmpCode.Value.ToString() + "')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                prcLoadList(DateTime.Today.ToString());
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

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        
        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTotalDays_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtApproved_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtTotalDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtApproved_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Leave Type";
            cboType.DisplayLayout.Bands[0].Columns["varName"].Width = cboType.Width - 5;

            cboType.DisplayMember = "varName";

        }

        private void cboEmpCode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpCode.DisplayLayout.Bands[0].Columns["empID"].Hidden = true;

            cboEmpCode.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            cboEmpCode.DisplayLayout.Bands[0].Columns["empCode"].Width = cboEmpCode.Width - 5;

            cboEmpCode.DisplayMember = "empCode";
            cboEmpCode.ValueMember = "empID";
        }


        private void cboEmpCode_Leave(object sender, EventArgs e)
        {
            try
            {
                prcLoadLeaveBalance(cboEmpCode.Value.ToString());
                prcLoadListIndividual(dtInput.DateTime.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridLeaveBalance_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;

            gridLeaveBalance.DisplayLayout.Bands[0].Columns["LWP"].Hidden = true;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ALWP"].Hidden = true;

            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ACCL"].Hidden = true;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["AACCL"].Hidden = true;

            gridLeaveBalance.DisplayLayout.Bands[0].Columns["CLH"].Hidden = true;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ACLH"].Hidden = true;

            gridLeaveBalance.DisplayLayout.Bands[0].Columns["SLH"].Hidden = true;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ASLH"].Hidden = true;

            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ELH"].Hidden = true;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["AELH"].Hidden = true;

            //Set Caption
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Code";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["CL"].Header.Caption = "CL(Total)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ACL"].Header.Caption = "CL(Enjoyed)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["SL"].Header.Caption = "SL(Total)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ASL"].Header.Caption = "SL(Enjoyed)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["EL"].Header.Caption = "EL(Total)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["AEL"].Header.Caption = "EL(Enjoyed)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ML"].Header.Caption = "ML(Total)";
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["AML"].Header.Caption = "ML(Enjoyed)";

            //Set 
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["empCode"].Width = 70;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["empName"].Width = 220;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["CL"].Width = 80;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ACL"].Width = 100;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["SL"].Width = 80;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ASL"].Width = 100;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["EL"].Width = 80;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["AEL"].Width = 100;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["ML"].Width = 80;
            gridLeaveBalance.DisplayLayout.Bands[0].Columns["AML"].Width = 100;



            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

 
        private void dtDateTo_Leave(object sender, EventArgs e)
        {
            //int x = (dtDateTo.DateTime - dtDateFrom.DateTime).Days + 1;
            //txtTotalDays.Text = x.ToString();
        }

        private void cboEmpCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void gridList_DoubleClick_1(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["lvId"].Value.ToString(), DateTime.Today.ToString());
                prcLoadLeaveBalance(gridList.ActiveRow.Cells["empId"].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            prcLoadList(dtInput.DateTime.ToString());
        }

        private void dtInput_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtDateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtDateFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtDateTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridList.DisplayLayout.Bands[0].Columns["lvId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Code";
            gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
            gridList.DisplayLayout.Bands[0].Columns["lvType"].Header.Caption = "Type";
            gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Header.Caption = "From(Date)";
            gridList.DisplayLayout.Bands[0].Columns["dtTo"].Header.Caption = "To(Date)";
            gridList.DisplayLayout.Bands[0].Columns["totalDay"].Header.Caption = "Days";
            gridList.DisplayLayout.Bands[0].Columns["lvApp"].Header.Caption = "Approved";
            gridList.DisplayLayout.Bands[0].Columns["Remark"].Header.Caption = "Remark";

            //Set 
            gridList.DisplayLayout.Bands[0].Columns["empCode"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["lvType"].Width = 50;
            gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Width = 90;
            gridList.DisplayLayout.Bands[0].Columns["dtTo"].Width = 90;
            gridList.DisplayLayout.Bands[0].Columns["totalDay"].Width = 50;
            gridList.DisplayLayout.Bands[0].Columns["lvApp"].Width = 70;
            gridList.DisplayLayout.Bands[0].Columns["Remark"].Width = 150;



            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["lvId"].OriginalValue.ToString(), DateTime.Today.ToString());
                prcLoadLeaveBalance(gridList.ActiveRow.Cells["EmpId"].OriginalValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboDuration_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDuration.DisplayLayout.Bands[0].Columns["DurationId"].Hidden = true;

            cboDuration.DisplayLayout.Bands[0].Columns["DurationType"].Header.Caption = "Duration";
            cboDuration.DisplayLayout.Bands[0].Columns["DurationType"].Width = cboDuration.Width - 5;

            cboDuration.DisplayMember = "DurationType";
            cboDuration.ValueMember  = "DurationId";
        }

        //private void cboFromAMPM_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    cboFromAMPM.DisplayLayout.Bands[0].Columns["Id"].Hidden = true;

        //    cboFromAMPM.DisplayLayout.Bands[0].Columns["Type"].Header.Caption = "Type";
        //    cboFromAMPM.DisplayLayout.Bands[0].Columns["Type"].Width = cboFromAMPM.Width - 5;

        //    cboFromAMPM.DisplayMember = "Type";
        //    cboFromAMPM.ValueMember = "Id";
        //}

        //private void cboToAMPM_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    cboToAMPM.DisplayLayout.Bands[0].Columns["Id"].Hidden = true;

        //    cboToAMPM.DisplayLayout.Bands[0].Columns["Type"].Header.Caption = "Type";
        //    cboToAMPM.DisplayLayout.Bands[0].Columns["Type"].Width = cboToAMPM.Width - 5;

        //    cboToAMPM.DisplayMember = "Type";
        //    cboToAMPM.ValueMember = "Id";
        //}

        private void cboDuration_ValueChanged(object sender, EventArgs e)
        {
            //pnlDuration.Visible = false;
            dtDateFrom.Enabled = true;
            dtDateTo.Enabled = true;
            
            //if (cboDuration.Value.ToString()== "1")
            //{
            //   //// pnlDuration.Visible = true;
            //    dtDateFrom.Value = DateTime.Today;
            //    dtDateTo.Value = DateTime.Today;
            //    dtDateFrom.Enabled = false ;
            //    dtDateTo.Enabled = false;


            //}
        }

        private void txtTotalDays_Leave(object sender, EventArgs e)
        {
            string str = txtTotalDays.Text.ToString().Trim();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '0')
                {
                    if( str[i] != '1')
                    {
                        if (str[i] != '2')
                        {
                            if (str[i] != '3')
                            {
                                if (str[i] != '4')
                                {
                                    if (str[i] != '5')
                                    {
                                        if (str[i] != '6')
                                        {
                                            if (str[i] != '7')
                                            {
                                                if (str[i] != '8')
                                                {
                                                    if (str[i] != '9')
                                                    {
                                                        if (str[i] != '.')
                                                        {
                                                            MessageBox.Show("Insert only Number. ");
                                                            return;
                                                        }

                                                    }

                                                }

                                            }

                                        }

                                    }

                                }

                            }

                        }

                    }
                }
            }

        }

        private void dtDateFrom_Leave(object sender, EventArgs e)
        {
            if (cboType.Text == "CL" || cboType.Text == "SL" || cboType.Text == "EL" || cboType.Text == "ML" || cboType.Text == "ACCL" || cboType.Text == "LWP")
            {
                int x = int.Parse(txtTotalDays.Text.ToString().Trim());
                dtDateTo.DateTime = dtDateFrom.DateTime.AddDays(x).AddDays(-1);

                txtApproved.Text = txtTotalDays.Text;
                //int y = double.Parse(dtDateFrom.DateTime.AddDays(x));
                //dtDateTo.DateTime.AddDays(-1);
            }
            else if (cboType.Text == "CLH" || cboType.Text == "SLH" || cboType.Text == "ELH")
            {

                dtDateTo.DateTime = dtDateFrom.DateTime;
                txtApproved.Text = txtTotalDays.Text;
            }
        }

        private void cboDuration_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtInput_ValueChanged(object sender, EventArgs e)
        {
            //prcLoadList(clsProc.GTRDate(dtInput.ToString()));
            //prcLoadCombo();
        }


        private void gridJob_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide column

                gridJob.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true; //Country Name
                gridJob.DisplayLayout.Bands[0].Columns["isChecked"].Hidden = true;
                gridJob.DisplayLayout.Bands[0].Columns["ShiftId"].Hidden = true;
                gridJob.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;

                //Set Width
                gridJob.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70; //Short Name
                //gridJob.DisplayLayout.Bands[0].Columns["ShiftId"].Width = 100; //Shift
                gridJob.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 85; //
                gridJob.DisplayLayout.Bands[0].Columns["TimeIn"].Width = 55; //
                gridJob.DisplayLayout.Bands[0].Columns["TimeOut"].Width = 55; //
                gridJob.DisplayLayout.Bands[0].Columns["OTHour"].Width = 55; //
                gridJob.DisplayLayout.Bands[0].Columns["Status"].Width = 40; //
                //gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Hidden = true; //
                // gridList.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;  //

                //Set Caption
                gridJob.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                //gridJob.DisplayLayout.Bands[0].Columns["ShiftId"].Header.Caption = "Shift";
                gridJob.DisplayLayout.Bands[0].Columns["dtPunchDate"].Header.Caption = "Punchdate";
                gridJob.DisplayLayout.Bands[0].Columns["TimeIn"].Header.Caption = "Time In";
                gridJob.DisplayLayout.Bands[0].Columns["TimeOut"].Header.Caption = "Time Out";
                gridJob.DisplayLayout.Bands[0].Columns["OTHour"].Header.Caption = "Ot Hour";
                gridJob.DisplayLayout.Bands[0].Columns["Status"].Header.Caption = "Status";
                //this.gridJob.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                //    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;


                gridJob.DisplayLayout.Bands[0].Columns["dtPunchDate"].Format = "dd-MMM-yyyy  dddd";

                //e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

                gridJob.DisplayLayout.Bands[0].Columns["dtPunchDate"].SortIndicator = SortIndicator.Ascending;

                gridJob.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridJob.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;
                e.Layout.Override.FixedRowSortOrder = FixedRowSortOrder.Sorted;

                //Selection Style Will Be Row Selector
                this.gridJob.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridJob.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridJob.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;


                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dtFrom.DateTime.Month == 12)
            {
                var firstDay = new DateTime(dtFrom.DateTime.Year + 1, dtFrom.DateTime.Month - 11, 1);
                dtFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);


                dtTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtTo.DateTime.Year, dtTo.DateTime.Month + 1);
                var lastDay = new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month + 1, DaysInMonth);
                var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month + 1, 1);
                dtFrom.Value = firstDay;
                dtTo.Value = lastDay;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (dtFrom.DateTime.Month == 1)
            {
                var firstDay = new DateTime(dtFrom.DateTime.Year - 1, dtFrom.DateTime.Month + 11, 1);
                dtFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);


                dtTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtTo.DateTime.Year, dtTo.DateTime.Month - 1);
                var lastDay = new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month - 1, DaysInMonth);
                var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month - 1, 1);
                dtFrom.Value = firstDay;
                dtTo.Value = lastDay;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            if (cboEmpCode.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Employee ID");
                cboEmpCode.Focus();
                return;
            }
            try
            {
                string sqlQuery = "Exec [prcGetManualAtt] " + Common.Classes.clsMain.intComId + ",'" + cboEmpCode.Value.ToString() + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "', 1 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                dsList.Tables[0].TableName = "tblFixData";

                dsList.Tables[1].TableName = "tblAttedDate";

                if (dsList.Tables["tblFixData"].Rows.Count > 0)
                {
                    DataRow dr = dsList.Tables["tblFixData"].Rows[0];


                    lvlP.Text = dr["Present"].ToString();
                    lvlA.Text = dr["Absent"].ToString();
                    lvlL.Text = dr["lateDay"].ToString();
                    lvlLH.Text = dr["latehrttl"].ToString();
                    lvlLV.Text = dr["Leave"].ToString();
                    lvlH.Text = dr["HDay"].ToString();
                    lvlWh.Text = dr["WDay"].ToString();
                    lvlOT.Text = dr["OTHr"].ToString();

                }
                else
                {

                    lvlP.Text = "0";
                    lvlA.Text = "0";
                    lvlL.Text = "0";
                    lvlLH.Text = "0";
                    lvlLV.Text = "0";
                    lvlH.Text = "0";
                    lvlWh.Text = "0";
                    lvlOT.Text = "0";

                }

                gridJob.DataSource = null;
                gridJob.DataSource = dsList.Tables["tblAttedDate"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
                dsList = null;
            }
        }



        

    }
}
