using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GTRHRIS.Common;
using GTRHRIS.Attendence.FormEntry;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmPFPross : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private System.Data.DataView dvSection;
        private DataView dvGrid;

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmPFPross(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmPFPross_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPFPross_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                //prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetSalPross] 0," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblReligion";
                dsList.Tables[1].TableName = "tblSalType";
                dsList.Tables[2].TableName = "tblFestType";
                dsList.Tables[3].TableName = "tblEmployee";


               // dvGrid = dsList.Tables["tblGridList"].DefaultView;
              
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

        //private void prcLoadCombo()
        //{
           

        //    cboRelegion.DataSource = null;
        //    cboRelegion.DataSource = dsList.Tables["tblReligion"];
        //    cboRelegion.DisplayMember = "varname";
        //    cboRelegion.ValueMember = "varid";

        //    cboSalType.DataSource = null;
        //    cboSalType.DataSource = dsList.Tables["tblSalType"];
        //    cboSalType.DisplayMember = "varname";
        //    cboSalType.ValueMember = "varid";

        //    cboFestType.DataSource = null;
        //    cboFestType.DataSource = dsList.Tables["tblFestType"];
        //    cboFestType.DisplayMember = "varname";
        //    cboFestType.ValueMember = "varid";

        //}

        //private void prcDisplayDetails(string strParam)
        //{
        //    dsDetails = new System.Data.DataSet();
        //    GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
        //    try
        //    {
        //        string sqlQuery = "Exec prcGetSalPross " + Common.Classes.clsMain.intComId + " , " +Int32.Parse(strParam) + " ";
        //        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
        //        dsDetails.Tables[0].TableName = "Details";

        //        DataRow dr;
        //        if (dsDetails.Tables["Details"].Rows.Count > 0)
        //        {
        //            dr = dsDetails.Tables["Details"].Rows[0];
                   
        //            this.dtJoinUpto.Value = dr["dtInc"].ToString();
                   
        //            this.txtDollarRate.Value = dr["amount"].ToString();
        //            this.txtPer.Value = dr["Percentage"].ToString();

                    
        //            this.cboRelegion.Text = dr["NewGrade"].ToString();
                   
        //            cboSalType.Value = dr["NewSectIdSal"].ToString();

        //            cboFestType.Value = dr["NewOTStatus"].ToString();

        //            this.btnSave.Text = "&Update";
        //            this.btnDelete.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        clsCon = null;
        //    }
        //}

        private void prcClearData()
        {

            //DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); 
            //dtFirst.Value = firstDay;
            ////firstDay = firstDay.AddDays(-(firstDay.Day - 1));

            //DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //lastDay = lastDay.AddMonths(1);
            //lastDay = lastDay.AddDays(-(lastDay.Day));
            //dtLast.Value = lastDay;

            dtFirst.Value = DateTime.Now;

            if (dtFirst.DateTime.Month == 1)
            {
                if (dtFirst.DateTime.Day <= 6)
                {
                    var firstDay = new DateTime(dtFirst.DateTime.Year - 1, dtFirst.DateTime.Month + 11, 1);
                    dtFirst.Value = firstDay;
                    var DaysInMonth = DateTime.DaysInMonth(dtFirst.DateTime.Year, dtFirst.DateTime.Month);
                    var lastDay = new DateTime(dtFirst.DateTime.Year, dtFirst.DateTime.Month, DaysInMonth);
                    dtLast.Value = lastDay;
                }
                else
                {

                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtFirst.Value = firstDay;

                    DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDay = lastDay.AddMonths(1);
                    lastDay = lastDay.AddDays(-(lastDay.Day));
                    dtLast.Value = lastDay;
                }
            }

            else
            {

                if (dtFirst.DateTime.Day <= 6)
                {
                    var DaysInMonth = DateTime.DaysInMonth(dtLast.DateTime.Year, dtLast.DateTime.Month - 1);
                    var lastDay = new DateTime(dtLast.DateTime.Year, dtLast.DateTime.Month - 1, DaysInMonth);
                    var firstDay = new DateTime(dtFirst.DateTime.Year, dtFirst.DateTime.Month - 1, 1);
                    dtFirst.Value = firstDay;
                    dtLast.Value = lastDay;
                }

                else
                {
                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtFirst.Value = firstDay;

                    DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDay = lastDay.AddMonths(1);
                    lastDay = lastDay.AddDays(-(lastDay.Day));
                    dtLast.Value = lastDay;
                }

            }



            dtFirstYearEnd.Value = DateTime.Now;

            if (dtFirstYearEnd.DateTime.Month == 1)
            {
                if (dtFirstYearEnd.DateTime.Day <= 6)
                {
                    var firstDay = new DateTime(dtFirstYearEnd.DateTime.Year - 1, dtFirstYearEnd.DateTime.Month + 11, 1);
                    dtFirstYearEnd.Value = firstDay;
                    var DaysInMonth = DateTime.DaysInMonth(dtFirstYearEnd.DateTime.Year, dtFirstYearEnd.DateTime.Month);
                    var lastDay = new DateTime(dtFirstYearEnd.DateTime.Year, dtFirstYearEnd.DateTime.Month, DaysInMonth);
                    dtLastYearEnd.Value = lastDay;
                }
                else
                {

                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtFirstYearEnd.Value = firstDay;

                    DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDay = lastDay.AddMonths(1);
                    lastDay = lastDay.AddDays(-(lastDay.Day));
                    dtLastYearEnd.Value = lastDay;
                }
            }

            else
            {

                if (dtFirstYearEnd.DateTime.Day <= 6)
                {
                    var DaysInMonth = DateTime.DaysInMonth(dtLastYearEnd.DateTime.Year, dtLastYearEnd.DateTime.Month - 1);
                    var lastDay = new DateTime(dtLastYearEnd.DateTime.Year, dtLastYearEnd.DateTime.Month - 1, DaysInMonth);
                    var firstDay = new DateTime(dtFirstYearEnd.DateTime.Year, dtFirstYearEnd.DateTime.Month - 1, 1);
                    dtFirstYearEnd.Value = firstDay;
                    dtLastYearEnd.Value = lastDay;
                }

                else
                {
                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtFirstYearEnd.Value = firstDay;

                    DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDay = lastDay.AddMonths(1);
                    lastDay = lastDay.AddDays(-(lastDay.Day));
                    dtLastYearEnd.Value = lastDay;
                }

            }



            //dtFirst = ((int.Parse(DateTime.Now.Year.ToString()).ToString()), DateTime.Now.Month, 1)

           
            txtPer.Value = 0;
          
            chkPF.Text = "No";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            //this.txtCode.Focus();
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
            Int32 NewId = 0;
            try
            {
                //if (dtJoinUpto.Value != null)
                //{
                //    dtJoinUpto.Value = clsProc.GTRDate(dtJoinUpto.Value.ToString());
                //}
                //Member Master Table
                if (txtPer.Text.Length != 0)
                {

                    //Update
                   
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //NewId
                    sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert Data
                    //sqlQuery = "INSERT into dbo.tblEmp_Incr(IncId, IncType, EmpId, dtInc, Amount, Percentage, OldSal, CurrSal, OldDesigId, NewDesigId, HR, TA, FA, PBns, OldSectId, NewSectId, OldGrade, NewGrade, OldOTstatus, NewOTstatus, OldStatus, NewStatus, newSectIdSal, OldSectIdSal, IsInactive, ComId, LUserId, PCName)"
                    //           + " Values (" + NewId + ",'" + clsProc.GTRDate(dtJoinUpto.Value.ToString()) + "'"+ " ," + txtDollarRate.Text + ",'" + txtPer.Text +
                    //           ",'" + Common.Classes.clsMain.strComputerName + "')";
                    //arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                }
                prcClearData();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to delete Increament information of 0", "",
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
                //sqlQuery = "Delete from tblEmp_Incr Where incID = ;
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','Delete')";
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Boolean fncBlank()
        {
            //General Information
            tabEmployee.Tabs[0].Selected = true;
          //if (this.cboRelegion.Text.Length == 0 && cboRelegion.Text != "Increment")
          //  {
          //      MessageBox.Show("Please provide Grade");
          //      cboRelegion.Focus();
          //      return true;
          //  }
            
          //  if (this.txtDollarRate.Text.Length == 0)
          //  {
          //      MessageBox.Show("Please provide Increment Amount");
          //      txtDollarRate.Focus();
          //      return true;
          //  }

            if (this.txtPer.Text.Length == 0)
            {
                MessageBox.Show("Please provide Increment Percentage");
                txtPer.Focus();
                return true;
            }

            return false;
        }


        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtCodeTran_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtCodeFigure_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }



      

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dsList.Tables["tblemployee"].Rows.Count == 0)
            {
                MessageBox.Show("Data not found in grid to filter");
                return;
            }

            DataView dvSource = new DataView();
            try
            {
               
            }
            catch
            {

                {

                    dvSource.RowFilter = "";
                                  

                }
            }
            finally
            {
                dvSource = null;
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            if (dsList.Tables["tblemployee"].Rows.Count == 0)
            {
                MessageBox.Show("Data not found in grid to filter");
                return;
            }

            DataView dvSource = new DataView();
            try
            {
              

                //cboReference.DisplayLayout.Bands[0].ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dvSource = null;
            }
        }

        private void cboFilterFName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void chkInactive_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }



        private void cboType_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewDesig_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtAmt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtPer_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewGrade_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewSalSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewStatus_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewSection_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewDesig_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void txtAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void txtPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewGrade_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewSalSec_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewSection_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboEmpID_Enter(object sender, EventArgs e)
        {

        }





        private void txtPer_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtPer);
        }



        private void chkFestBonus_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPF.Checked == true)
            {
                festPanel.Enabled = true;
                chkPF.Text = "Yes";

            }
            else
            {
                festPanel.Enabled = false;
                chkPF.Text = "No";

            }
        }



        private void btnSalProcessFull_Click(object sender, EventArgs e)
        {
            string Description = "";

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dtLast.DateTime.Month).ToString();


            Description = strMonthName + "-" + (dtLast.DateTime.Year);
            btnSalProcessFull.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            string sqlQuery1 = "";
            Int64 ChkLock = 0;


            sqlQuery1 = "Select dbo.fncProcessLock (" + Common.Classes.clsMain.intComId + ", 'PF Process','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "')";
            ChkLock = clsCon.GTRCountingDataLarge(sqlQuery1);


            if (ChkLock == 1)
            {
                MessageBox.Show("Process Lock. Please communicate with Administrator.");
                return;
            }

            try
            {

                string sqlQuery = "Exec prcProcessPF " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "','" + Description + "'";
                int i = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Process')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("PF Process Complete");
                btnSalProcessFull.Text = "PF Process";

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

        private void btnFYear_Click(object sender, EventArgs e)
        {
            string Description = "";

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dtLast.DateTime.Month).ToString();


            Description = strMonthName + "-" + (dtLast.DateTime.Year);
            btnFYear.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            //string sqlQuery1 = "";
            //Int64 ChkLock = 0;


            //sqlQuery1 = "Select dbo.fncProcessLock (" + Common.Classes.clsMain.intComId + ", 'Salary Lock','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "')";
            //ChkLock = clsCon.GTRCountingDataLarge(sqlQuery1);


            //if (ChkLock == 1)
            //{
            //    MessageBox.Show("Process Lock. Please communicate to Administrator.");
            //    return;
            //}

            try
            {

                string sqlQuery = "Exec prcProcessPF " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFirstYearEnd.Value.ToString()) + "','" + clsProc.GTRDate(dtLastYearEnd.Value.ToString()) + "','" + Description + "','" + txtPer.Value.ToString() + "',1";
                int i = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Process')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("Year End PF Process Complete");
                btnFYear.Text = "Year End PF Process";

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





    }
}