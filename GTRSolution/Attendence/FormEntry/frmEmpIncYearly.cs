using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Infragistics.Win;
using GTRLibrary;
using Infragistics.Win.UltraWinGrid;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmEmpIncYearly : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private DataView dvStyle;
        private DataView dvSpec;
        private DataView dvColor;

        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmEmpIncYearly(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmEmpIncYearly_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                PrcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            prcGetBasedLoad();
        }

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();
            try
            {
                sqlQuery = "Exec prcGetIncYearly " + Common.Classes.clsMain.intComId + ",0,0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                dsList.Tables[0].TableName = "tblEmp";

                int year = DateTime.Now.Year;
                DateTime YearfirstDay = new DateTime(year, 1, 1);
                dtInc.Value = YearfirstDay;

                dtJoinFrom.Value = "1-Jan-1950";
                dtJoinTo.Value = DateTime.Now;

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
        private void PrcLoadCombo()
        {
            try
            {
                cboEmpId.DataSource = null;
                cboEmpId.DataSource = dsList.Tables["tblEmp"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private  void prcClearData()
        {

            txtName.Value = "";
            txtSection.Value = "";
            txtDesig.Value = "";
            txtGS.Value = "";
            txtBS.Value = "";
            txtRateAll.Value = "";
            txtPerAll.Value = "";
            txtGSInd.Value = "";
            txtRateInd.Value = "";
            txtPerInd.Value = "";
            txtGSFr.Value = "";
            txtRateFr.Value = "";
            txtPerFr.Value = "";
            txtPrSalFr.Value = "";
            txtPrUSDFr.Value = "";
            txtGSSP.Value = "";
            txtProSalSp.Value = "";
            txtProUSDSp.Value = "";

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmpIncYearly_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void prcGetBasedLoad()
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            string Band = "";
            string SectId = "0", EmpId = "0";

            //Collecting Parameter Value
            if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
            {
            }




            try
            {
                sqlQuery = "Exec prcGetIncAll " + Common.Classes.clsMain.intComId + ", " + EmpId + "," + SectId + ",'" + Band + "','" + optCriteria.Value.ToString() + "','" + clsProc.GTRDate(dtJoinFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";



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


        private void dtDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)(e.KeyCode));
        }

        private void txtReqNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)(e.KeyCode));
        }

        private void cboStyle_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)(e.KeyCode));
        }

        private void txtLine_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)(e.KeyCode));
        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)(e.KeyCode));
        }

        private void cboBuyer_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)(e.KeyCode));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Do you want to approve all employee increment ?", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;
            try
            {

                sqlQuery = " Update E Set  E.GS = A.NewSal, E.GSUSD = A.NewSalUSD, E.GS = A.GS, E.BS = A.BS,E.HR = A.HR, E.MA = A.MA from tblEmp_Info E, tblEmp_Incr A Where E.EmpID = A.EmpID And E.ComId = " + Common.Classes.clsMain.intComId + " And A.ComId = " + Common.Classes.clsMain.intComId + " and A.dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and A.IncYearly = 2";
                arQuery.Add(sqlQuery);

                sqlQuery = " Update tblEmp_Incr Set IncYearly = 1 Where ComId = " + Common.Classes.clsMain.intComId + " And dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly = 2";
                arQuery.Add(sqlQuery);

                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                             + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                             "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Increment Approve Successfully Completed");

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

        private void cboStyle_Validating(object sender, CancelEventArgs e)
        {
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete all employee Increment.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string sqlQuery = "";
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {

                if (btnDelete.Text.ToString() == " &Delete")
                {

                    sqlQuery = " Update E Set  E.GS = A.OldSal, E.GSUSD = A.OldSalUSD from tblEmp_Info E, tblEmp_Incr A Where E.EmpID = A.EmpID And E.ComId = " + Common.Classes.clsMain.intComId + " And A.ComId = " + Common.Classes.clsMain.intComId + " and A.dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and A.IncYearly = 1";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Update E Set  E.BS = round(((E.GS-560)/1.4),0),E.HR = E.GS-(round(((E.GS-560)/1.4),0)+560),E.MA = 560 from tblEmp_Info E, tblEmp_Incr A Where E.EmpID = A.EmpID And E.ComId = " + Common.Classes.clsMain.intComId + " And A.ComId = " + Common.Classes.clsMain.intComId + " and A.dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and A.IncYearly = 1";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Delete tblEmp_Incr Where ComId = " + Common.Classes.clsMain.intComId + " And dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly = 1";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Delete SuccessFuly");
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

        private void btnRpt_Click(object sender, EventArgs e)
        {
           
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            int year = dtInc.DateTime.Year;
            DateTime YearfirstDay = new DateTime(year, 1, 1);
            dtInc.Value = YearfirstDay;

            string ReportPath = "", SQLQuery = "", FormCaption = "", DataSourceName = "DataSet1";
            DataSourceName = "DataSet1";

            FormCaption = "Report :: Yearly Increment...";

            try
            {

                     ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrList.rdlc";
                     SQLQuery = "Exec rptIncrementYearly " + Common.Classes.clsMain.intComId + " ,'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "',0";

                     clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                     if (dsDetails.Tables[0].Rows.Count == 0)
                     {
                         MessageBox.Show("Data Not Found");
                         return;
                     }


                    clsReport.strReportPathMain = ReportPath;
                    clsReport.dsReport = dsDetails;
                    clsReport.strDSNMain = DataSourceName;
                    Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                    Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                    FM.prcShowReport(FormCaption);
                
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

        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["varName"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Increment Type";
            cboType.DisplayMember = "varName";
            cboType.ValueMember = "varName";
        }

        private Boolean fncBlankAll()
        {


            if (txtRateAll.Text.Length == 0)
            {
                MessageBox.Show("Please provide dollar rate.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRateAll.Focus();
                return true;
            }
            if (txtPerAll.Text.Length == 0)
            {
                MessageBox.Show("Please provide percentage.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPerAll.Focus();
                return true;
            }


            return false;


        }
        private void btnIncAllUp_Click(object sender, EventArgs e)
        {
            if (fncBlankAll())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";

            int year = dtInc.DateTime.Year;
            DateTime YearfirstDay = new DateTime(year, 1, 1);
            dtInc.Value = YearfirstDay;


            try
            {

                    sqlQuery = "Exec prcProcessIncrement " + Common.Classes.clsMain.intComId + ",'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinFrom.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinTo.Value.ToString()) + "','" + txtRateAll.Value.ToString() + "','" + txtPerAll.Value.ToString() + "',0,0";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Update tblEmp_Incr Set LUserId = " + GTRHRIS.Common.Classes.clsMain.intUserId + ",PCName = '" + Common.Classes.clsMain.strComputerName + "' Where ComId = " + Common.Classes.clsMain.intComId + " and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");

                    prcClearData();

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

        private void btnIncAllDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Yearly Increment.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string sqlQuery = "";
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {

                    sqlQuery = "Delete tblEmp_Incr Where ComId = " + Common.Classes.clsMain.intComId + " and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                    arQuery.Add(sqlQuery);
    
                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Delete Successfully");
                    
                    prcClearData();

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
        private Boolean fncBlankInd()
        {


            if (txtRateInd.Text.Length == 0)
            {
                MessageBox.Show("Please provide dollar rate.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRateInd.Focus();
                return true;
            }
            if (txtPerInd.Text.Length == 0)
            {
                MessageBox.Show("Please provide percentage.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPerInd.Focus();
                return true;
            }
            if (cboEmpId.Text.Length == 0)
            {
                MessageBox.Show("Please provide Employee Id.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboEmpId.Focus();
                return true;
            }


            return false;


        }
        private void btnIncIndUp_Click(object sender, EventArgs e)
        {
            if (fncBlankInd())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";

            int year = dtInc.DateTime.Year;
            DateTime YearfirstDay = new DateTime(year, 1, 1);
            dtInc.Value = YearfirstDay;

            try
            {

                sqlQuery = "Exec prcProcessIncrement " + Common.Classes.clsMain.intComId + ",'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinFrom.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinTo.Value.ToString()) + "','" + txtRateInd.Value.ToString() + "','" + txtPerInd.Value.ToString() + "','" + cboEmpId.Value.ToString() + "',1";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                arQuery.Add(sqlQuery);

                sqlQuery = "Update tblEmp_Incr Set LUserId = " + GTRHRIS.Common.Classes.clsMain.intUserId + ",PCName = '" + Common.Classes.clsMain.strComputerName + "' Where ComId = " + Common.Classes.clsMain.intComId + " and EmpId = '" + cboEmpId.Value.ToString() + "' and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved Successfully");

                prcClearData();

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

        private void btnIncIndDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Yearly Increment this Employee.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string sqlQuery = "";
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {

                sqlQuery = "delete tblEmp_Incr Where ComId = " + Common.Classes.clsMain.intComId + " and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and EmpId = '" + cboEmpId.Value.ToString() + "' and IncYearly in (1,2)";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Delete Successfully");

                prcClearData();

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

        private void txtMonth_ValueChanged(object sender, EventArgs e)
        {

                if (txtMonth.Text.Length == 0)
                {
                    txtPrSalFr.Value = 0;

                }

                if (double.Parse(txtMonth.Value.ToString()) > 0)
                {

                    Double NewGSUSD = 0;
                    Int64 GS = 0, Basic = 0, HR = 0, MA = 0, NewGS = 0;

                    GS = Convert.ToInt64(double.Parse(txtGS.Value.ToString()));
                    Basic = Convert.ToInt64(Math.Round((double.Parse(txtBS.Value.ToString()) * double.Parse(txtPerFr.Value.ToString())) / 100));
                    Basic = Convert.ToInt64(Math.Round((Convert.ToDouble(Basic) / 12.0) * double.Parse(txtMonth.Value.ToString())) + double.Parse(txtBS.Value.ToString()));
                    HR = Convert.ToInt64(Math.Round(double.Parse(Basic.ToString()) * 0.40));
                    MA = 560;

                    NewGS = (Basic + HR + MA);
                    NewGSUSD =  Math.Round(Convert.ToDouble(NewGS) / double.Parse(txtRateFr.Value.ToString()), 2, MidpointRounding.AwayFromZero)   ;
                    txtPrSalFr.Value = NewGS;
                    txtPrUSDFr.Value = NewGSUSD;

                }
        }

        private Boolean fncBlankFr()
        {
            if (txtPrSalFr.Text.Length == 0)
            {
                MessageBox.Show("Please provide Proposed Salary.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrSalFr.Focus();
                return true;
            }

            if (txtPrUSDFr.Text.Length == 0)
            {
                MessageBox.Show("Please provide Proposed USD Salary.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrUSDFr.Focus();
                return true;
            }

            if (txtRateFr.Text.Length == 0)
            {
                MessageBox.Show("Please provide Dollar Rate.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRateFr.Focus();
                return true;
            }

            if (txtMonth.Text.Length == 0)
            {
                MessageBox.Show("Please provide Month.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMonth.Focus();
                return true;
            }

            if (txtPerFr.Text.Length == 0)
            {
                MessageBox.Show("Please provide Percentage.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPerFr.Focus();
                return true;
            }

            if (cboEmpId.Text.Length == 0)
            {
                MessageBox.Show("Please provide Employee Id.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboEmpId.Focus();
                return true;
            }
            return false;

        }
        private void btnIncFracUp_Click(object sender, EventArgs e)
        {
            if (fncBlankFr())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";

            int year = dtInc.DateTime.Year;
            DateTime YearfirstDay = new DateTime(year, 1, 1);
            dtInc.Value = YearfirstDay;

            try
            {

                sqlQuery = "Exec prcProcessIncrement " + Common.Classes.clsMain.intComId + ",'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinFrom.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinTo.Value.ToString()) + "','" + txtRateFr.Value.ToString() + "','" + txtPerFr.Value.ToString() + "','" + cboEmpId.Value.ToString() + "',2";
                arQuery.Add(sqlQuery);

                sqlQuery = "Update tblEmp_Incr Set NewSal = '" + txtPrSalFr.Value.ToString() + "',NewSalUSD = '" + txtPrUSDFr.Value.ToString() + "',FMonth = '" + txtMonth.Value.ToString() + "'  Where ComId = " + Common.Classes.clsMain.intComId + " and EmpId = '" + cboEmpId.Value.ToString() + "' and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                arQuery.Add(sqlQuery);

                sqlQuery = "Exec prcProcessIncrement " + Common.Classes.clsMain.intComId + ",'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinFrom.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinTo.Value.ToString()) + "','" + txtRateFr.Value.ToString() + "','" + txtPerFr.Value.ToString() + "','" + cboEmpId.Value.ToString() + "',3";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                arQuery.Add(sqlQuery);

                sqlQuery = "Update tblEmp_Incr Set LUserId = " + GTRHRIS.Common.Classes.clsMain.intUserId + ",PCName = '" + Common.Classes.clsMain.strComputerName + "' Where ComId = " + Common.Classes.clsMain.intComId + " and EmpId = '" + cboEmpId.Value.ToString() + "' and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved Successfully");

                prcClearData();

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

        private void btnIncSpUp_Click(object sender, EventArgs e)
        {

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";

            int year = dtInc.DateTime.Year;
            DateTime YearfirstDay = new DateTime(year, 1, 1);
            dtInc.Value = YearfirstDay;

            try
            {

                sqlQuery = "Exec prcProcessIncrement " + Common.Classes.clsMain.intComId + ",'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinFrom.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinTo.Value.ToString()) + "',0,0,'" + cboEmpId.Value.ToString() + "',4";
                arQuery.Add(sqlQuery);

                sqlQuery = "Update tblEmp_Incr Set NewSal = '" + txtProSalSp.Value.ToString() + "',NewSalUSD = '" + txtProUSDSp.Value.ToString() + "' Where ComId = " + Common.Classes.clsMain.intComId + " and EmpId = '" + cboEmpId.Value.ToString() + "' and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                arQuery.Add(sqlQuery);

                sqlQuery = "Exec prcProcessIncrement " + Common.Classes.clsMain.intComId + ",'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinFrom.Value.ToString()) + "','" + this.clsProc.GTRDate(this.dtJoinTo.Value.ToString()) + "',0,0,'" + cboEmpId.Value.ToString() + "',5";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                arQuery.Add(sqlQuery);

                sqlQuery = "Update tblEmp_Incr Set LUserId = " + GTRHRIS.Common.Classes.clsMain.intUserId + ",PCName = '" + Common.Classes.clsMain.strComputerName + "' Where ComId = " + Common.Classes.clsMain.intComId + " and EmpId = '" + cboEmpId.Value.ToString() + "' and dtInc = '" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "' and IncYearly in (1,2)";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved Successfully");

                prcClearData();

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

        private void cboEmpId_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpName"].Width = 135;
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 75;
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp. Code";
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
            cboEmpId.DisplayMember = "EmpCode";
            cboEmpId.ValueMember = "EmpId";
        }

        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            lblAll.Enabled = true;
            lblInd.Enabled = false;
            lblFr.Enabled = false;
            lblSp.Enabled = false;
            btnIncAllUp.Enabled = true;
            btnIncAllDel.Enabled = true;
            btnIncIndUp.Enabled = false;
            btnIncIndDel.Enabled = false;
            btnIncFracUp.Enabled = false;
            btnIncSpUp.Enabled = false;
            lblEmp.Enabled = false;

            if (optCriteria.Value.ToString().ToUpper() == "AllInc".ToUpper())
            {
                lblAll.Enabled = true;
                lblInd.Enabled = false;
                lblFr.Enabled = false;
                lblSp.Enabled = false;
                btnIncAllUp.Enabled = true;
                btnIncAllDel.Enabled = true;
                btnIncIndUp.Enabled = false;
                btnIncIndDel.Enabled = false;
                btnIncFracUp.Enabled = false;
                btnIncSpUp.Enabled = false;
                lblEmp.Enabled = false;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "IndInc".ToUpper())
            {
                lblAll.Enabled = false;
                lblInd.Enabled = true;
                lblFr.Enabled = false;
                lblSp.Enabled = false;
                btnIncAllUp.Enabled = false;
                btnIncAllDel.Enabled = false;
                btnIncIndUp.Enabled = true;
                btnIncIndDel.Enabled = true;
                btnIncFracUp.Enabled = false;
                btnIncSpUp.Enabled = false;
                lblEmp.Enabled = true;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "FracInc".ToUpper())
            {
                lblAll.Enabled = false;
                lblInd.Enabled = false;
                lblFr.Enabled = true;
                lblSp.Enabled = false;
                btnIncAllUp.Enabled = false;
                btnIncAllDel.Enabled = false;
                btnIncIndUp.Enabled = false;
                btnIncIndDel.Enabled = false;
                btnIncFracUp.Enabled = true;
                btnIncSpUp.Enabled = false;
                lblEmp.Enabled = true;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "SpInc".ToUpper())
            {
                lblAll.Enabled = false;
                lblInd.Enabled = false;
                lblFr.Enabled = false;
                lblSp.Enabled = true;
                btnIncAllUp.Enabled = false;
                btnIncAllDel.Enabled = false;
                btnIncIndUp.Enabled = false;
                btnIncIndDel.Enabled = false;
                btnIncFracUp.Enabled = false;
                btnIncSpUp.Enabled = true;
                lblEmp.Enabled = true;
            }
        }

        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetIncYearly " + Common.Classes.clsMain.intComId + " , " + Int32.Parse(strParam) + ",1";

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    // Tab Basic Information\
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtSection.Text = dr["SectName"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtDesig.Text = dr["DesigName"].ToString();
                    this.txtGS.Text = dr["GS"].ToString();
                    this.txtBS.Text = dr["BS"].ToString();
                    this.txtGSInd.Text = dr["GSUSD"].ToString();
                    this.txtGSFr.Text = dr["GSUSD"].ToString();
                    this.txtGSSP.Text = dr["GSUSD"].ToString();

                }

                //this.btnSave.Text = "&Update";
                //this.btnDelete.Enabled = true;
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

        private void cboEmpId_ValueChanged(object sender, EventArgs e)
        {
            if (this.cboEmpId.IsItemInList() == false)
            {
                prcClearData();
                //prcLoadCombo();
                return;
            }

            if (cboEmpId.Value == null)
            {
                return;
            }
            prcDisplayDetails(cboEmpId.Value.ToString());
        }
        private void GridToToExcel_InitializeColumn(object sender, InitializeColumnEventArgs e)
        {
            try
            {
                if (e.Column.DataType == typeof(System.DateTime?) && e.Column.Format != null)
                {
                    e.ExcelFormatStr = e.Column.Format.Replace("tt", "AM/PM");
                }
                else
                {
                    e.ExcelFormatStr = e.Column.Format;
                }
            }
            catch (Exception ex)
            {
                //ExceptionFramework.ExceptionPolicy.HandleException(ex, "DefaultPolicy");
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            string SQLQuery = "";

            SQLQuery = "Exec rptIncrementYearly " + Common.Classes.clsMain.intComId + " ,'" + this.clsProc.GTRDate(this.dtInc.Value.ToString()) + "',0";

            clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

            dsList.Tables[0].TableName = "YearlyInc";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["YearlyInc"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Yearly Increment" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            Application.DoEvents();
            UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            GridToToExcel.Export(gridExcel, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }




    }
}
