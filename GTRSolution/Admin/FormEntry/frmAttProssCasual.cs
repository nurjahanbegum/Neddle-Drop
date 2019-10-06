using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmAttProssCasual : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmAttProssCasual(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
             if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;

            string sqlQuery1 = "";
            Int64 ChkLock = 0;


            sqlQuery1 = "Select dbo.fncProcessLock (" + Common.Classes.clsMain.intComId + ", 'Casual Attend Lock','" + clsProc.GTRDate(dtFromDate.Value.ToString()) + "')";
            ChkLock = clsCon.GTRCountingDataLarge(sqlQuery1);


            if (ChkLock == 1)
            {
                MessageBox.Show("Process Lock. Please communicate with Administrator.");
                return;
            }


            try
            {
             
                {
                    //add new
                    //sqlQuery = "Select Isnull(Max(advID),0)+1 As NewId from tblSal_Adv";
                    //NewId = clsCon.GTRCountingData(sqlQuery);

                    DateTime dt1 = dtFromDate.DateTime;
                    DateTime dt2 = dtLast.DateTime;

                    TimeSpan ts = dt1 - dt2;

                   int days = ts.Days;

                   if (days > 1)
                   {

                       MessageBox.Show("Please Run The Process For " + clsProc.GTRDate(dtLast.DateTime.AddDays(1).ToString()) + " ");
                       dtFromDate.Focus();
                       dtFromDate.Value = (dtLast.DateTime.AddDays(1));
                   }


                   if (chkMonthly.Checked == true)
                   {

                       //int ICount  = 0; /// For the Process PrDate Count
                       //int ICount2 = 0; ///' For the Process PrDate Count

                       int Y = 0, X = 0;
                       //double Z = 1;

                       X = dtFromDate.DateTime.Day;
                       Y = dtToDate.DateTime.Day;

                       while (X <= Y)
                       {

                           lblProcess.Text = "Processing " + clsProc.GTRDate(dtLast.DateTime.ToString()) + " .....";

                           dsDetails = new System.Data.DataSet();

                           {
                               sqlQuery = "Exec prcProcessAttendCasual " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFromDate.Value.ToString()) + "'";
                               int i = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                               //sqlQuery = "Exec prcProcessAttendShift " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFromDate.Value.ToString()) + "'";
                               //clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);


                               //dsDetails.Tables[0].TableName = "tblSalAdv";
                           }

                           dtFromDate.Value = dtFromDate.DateTime.AddDays(1);

                           X++;
                           //Z++;

                       }

                       //'            Call DaySelector
                       //        Call Praram
                       //        CON.Execute "PrProcessAtt '" & AmzDate(DTPicker1.value) & "'"
                       //        CON.Execute "BuyerJobcardPrint '" & AmzDate(DTPicker1.value) & "'"
                       //        'If RSCounter(rspcs2) = 0 Then

                       //                            Call Connect
                       //                                ICount2 = TotalRecord("tblPrDate", "PrDate", AmzDate(DTPicker1.value))
                       //                                If ICount2 < 1 Then
                       //                                    CON.Execute "insert tblPrDate Values ('" & AmzDate(DTPicker1.value) & "')"
                       //                                End If
                       //        'Call ProcessAtt(0)         
                       //    End If
                       //         Next
                      

                   }
                   else
                   {

                       lblProcess.Text = "Processing " + clsProc.GTRDate(dtLast.DateTime.ToString()) + " .....";

                       dsDetails = new System.Data.DataSet();

                       {
                           if (OptSts.Value == "H" || OptSts.Value == "R" || OptSts.Value == "W")
                           {
                               sqlQuery = "delete tblProssType where ProssDt =  '" + clsProc.GTRDate(dtFromDate.Value.ToString()) + "';   insert into tblProssType(ProssDt,DaySts,DayStsB,IsLock) values ('" + clsProc.GTRDate(dtFromDate.Value.ToString()) + "','" + (OptSts.Value.ToString()) + "','" + (OptSts.Value.ToString()) + "',0)";
                               clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                           }


                           sqlQuery = "Exec prcProcessAttendCasual " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFromDate.Value.ToString()) + "'";
                           int i = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                       }
                   
                   }
                   MessageBox.Show("Process complete");

                }
                prcClearData();
                //cboEmpID.Focus();

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
            }
        }

       

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetDailyAttProcess] " + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblLasDate";
                dsList.Tables[1].TableName = "tblSection";
                dsList.Tables[2].TableName = "tblEmployee";
                dsList.Tables[3].TableName = "tblShift";
                dsList.Tables[4].TableName = "tblDesignation";

                cboDesignation.DataSource = null;
                cboDesignation.DataSource = dsList.Tables["tblDesignation"];

                cboSection.DataSource = null;
                cboSection.DataSource = dsList.Tables["tblSection"];

                cboShiftTime.DataSource = null;
                cboShiftTime.DataSource = dsList.Tables["tblshift"];

                cboEmpID.DataSource = null;
                cboEmpID.DataSource = dsList.Tables["tblEmployee"];

                DataRow dr;
                if (dsList.Tables["tblLasDate"].Rows.Count > 0)
                {
                    dr = dsList.Tables["tblLasDate"].Rows[0];

                    this.dtLast.Value = dr["prossDT"].ToString();

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

        private void prcLoadCombo()
        {
            //cboEmpID.DataSource = null;
            //cboEmpID.DataSource = dsList.Tables["tblEmployeeID"];
            //cboEmpID.DisplayMember = "empcode";
            //cboEmpID.ValueMember = "empid";


        }

        private void frmAttProssCasual_Load(object sender, EventArgs e)
        {
            try
            {
                
                prcClearData();
                prcLoadList();
                prcLoadCombo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmAttProssCasual_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            uTab = null;
            clsProc = null;
        }

        private void prcClearData()
        {
            //this.cboEmpID.Value = null;

            //this.dtLast.Value = DateTime.Now;
            dtToDate.Visible = false;
            lblTo.Visible = false;
            cboEmpID.Enabled = false;
            txtEmpName.Enabled = false;
            cboSection.Enabled = false;
            cboShiftTime.Enabled = false;
            cboDesignation.Enabled = false;
            chkMonthly.Checked = false;
            //this.txtAmt.Text = "0";

            //this.cboEmpID.Enabled = true;

           // this.btnSave.Text = "&Save";

        }

        private Boolean fncBlank()
        {
           
            //if (this.cboEmpID.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide Employee ID.");
            //    cboEmpID.Focus();
            //    return true;
            //}
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Active  ['asdf'] as Current Employee" , "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery=new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                sqlQuery = "Delete from tblSal_Adv Where advID = 0";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                //cboEmpID.Focus();

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

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetSalAdv " + Int32.Parse(strParam)+","+Common.Classes.clsMain.intComId ;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblSalAdv";

                DataRow dr;
                if (dsDetails.Tables["tblSalAdv"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblSalAdv"].Rows[0];

                    //this.txtAmt.Text = dr["Amount"].ToString();


                    
                    //this.btnSave.Text = "&Update";
                    //this.btnDelete.Enabled = true;
                    //this.cboEmpID.Enabled = false;
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
               // prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
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



        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboCountryName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCountryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

       
        
        private void dtJoinDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void ultraLabel6_Click(object sender, EventArgs e)
        {

        }

        private void ultraLabel16_Click(object sender, EventArgs e)
        {

        }

        private void chkMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMonthly.Checked == true)
            {
            dtToDate.Visible = true;
            lblTo.Visible = true;

            DateTime lastDay = new DateTime(dtFromDate.DateTime.Year, dtFromDate.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtToDate.Value = lastDay;


//            dtToDate.Value = clsProc.GTRLastDayOfMonth(dtFromDate.DateTime);
            }
            else
            {
                dtToDate.Visible = false;
                lblTo.Visible = false;
            }
        }

        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            if (optCriteria.Value == "All")
            {
                cboEmpID.Enabled = false;
                txtEmpName.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboDesignation.Enabled = false;
            }
            else if (optCriteria.Value == "EmpID")
            {
                cboEmpID.Enabled = true;
                txtEmpName.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboDesignation.Enabled = false;
            }
            else if (optCriteria.Value == "Sec")
            {
                cboEmpID.Enabled = false;
                txtEmpName.Enabled = false;
                cboSection.Enabled = true;
                cboShiftTime.Enabled = false;
                cboDesignation.Enabled = false;
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                cboEmpID.Enabled = false;
                txtEmpName.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = true;
                cboDesignation.Enabled = false;
            }
            else if (optCriteria.Value == "Desig")
            {
                cboEmpID.Enabled = false;
                txtEmpName.Enabled = false;
                cboSection.Enabled = false;
                cboShiftTime.Enabled = false;
                cboDesignation.Enabled = true;
            }



        }
    }
}
