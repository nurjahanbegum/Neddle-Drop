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

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmIncAll : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private DataView dvStyle;
        private DataView dvSpec;
        private DataView dvColor;

        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmIncAll(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmIncAll_Load(object sender, EventArgs e)
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
                sqlQuery = "Exec prcGetIncAll " + Common.Classes.clsMain.intComId + ", 0, 0,'','','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblBand";
                dsList.Tables[3].TableName = "tblEmp";
                dsList.Tables[4].TableName = "tblIncType";


                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];


                var DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtFrom.Value = firstDay;

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

                cboSec.DataSource = null;
                cboSec.DataSource = dsList.Tables["tblSect"];

                cboBand.DataSource = null;
                cboBand.DataSource = dsList.Tables["tblBand"];

                cboEmp.DataSource = null;
                cboEmp.DataSource = dsList.Tables["tblEmp"];

                cboType.DataSource = null;
                cboType.DataSource = dsList.Tables["tblIncType"];


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
            //PrcSRRNo();
            txtPer.Text = "";
            cboSec.Text = "";
            cboBand.Text = "";
            cboEmp.Text = "";
            cboType.Text = "";

            checkBox2.Checked = false;


            DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtFrom.Value = lastDay;

            btnDelete.Enabled = false;
            btnSave.Text = "&Save";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIncAll_FormClosing(object sender, FormClosingEventArgs e)
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

        private void gridDetails_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                gridDetails.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;

                //Set Caption
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 60; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].Header.Caption = "Gross";
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Increment Amt";
                gridDetails.DisplayLayout.Bands[0].Columns["NewGS"].Header.Caption = "New GS";
                gridDetails.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 140;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Width = 140;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Width = 120;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 90;
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].Width = 85;
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Width = 85;
                gridDetails.DisplayLayout.Bands[0].Columns["NewGS"].Width = 85;
                gridDetails.DisplayLayout.Bands[0].Columns["Remarks"].Width = 130;

                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].CellActivation = Activation.NoEdit;


                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //gridDetails.DisplayLayout.Bands[0].Columns["isInactive"].Style =
                //   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //gridDetails.DisplayLayout.Bands[0].Columns["aId"].Style =
                //   Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerWithSpin;

                ////Select Full Row when click on any cell
                ////e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                ////Selection Style Will Be Row Selector
                ////gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////Stop Updating
                ////gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                gridDetails.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

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

            else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
            {
                SectId = cboSec.Value.ToString();
            }

            else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
            {
                Band = cboBand.Text.ToString();
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
            {
                EmpId = cboEmp.Value.ToString();
            }


            try
            {
                sqlQuery = "Exec prcGetIncAll " + Common.Classes.clsMain.intComId + ", " + EmpId + "," + SectId + ",'" + Band + "','" + optCriteria.Value.ToString() + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];

                checkBox2.Checked = false;


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
            if (fncBlank())
            {
                return;
            }

            var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
            var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, 1);
            dtFrom.Value = firstDay;
            
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;
            try
            {


                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0 &&
                        row.Cells["isChecked"].Value.ToString() == "1")
                    {

                        //NewId
                        sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                        NewId = clsCon.GTRCountingData(sqlQuery);

                        sqlQuery = " Delete tblEmp_Incr Where empid = '" + row.Cells["empid"].Text.ToString() +
                                   "'  and ComID = " + Common.Classes.clsMain.intComId + " and dtInc = '" + clsProc.GTRDate(this.dtFrom.Value.ToString()) + "'";
                        arQuery.Add(sqlQuery);


                        sqlQuery = " Insert Into tblEmp_Incr(ComId,IncId,EmpId,dtInc,OldSal,Amount,NewSal,IncType,PCName,LUserId) "
                                   + " Values (" + Common.Classes.clsMain.intComId + "," + NewId + ",'" + 
                                   row.Cells["empid"].Text.ToString() + "', '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                                   row.Cells["GS"].Value.ToString() + "','" +
                                   row.Cells["Amount"].Value.ToString() + "','" + row.Cells["NewGS"].Value.ToString() + "','" + 
                                   row.Cells["Remarks"].Value.ToString() + "','" + Common.Classes.clsMain.strComputerName + "'," + GTRHRIS.Common.Classes.clsMain.intUserId + ")";
                        arQuery.Add(sqlQuery);

                        sqlQuery = " Update E Set  E.OldSectID = A.SectID, E.OldDesigID = A.DesigID  from tblEmp_Incr E, tblEmp_info A Where E.EmpID = A.EmpID And E.IncId = " + NewId + " and E.EmpID =   '" + 
                                    row.Cells["empid"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = " Update tblEmp_Info Set GS = '" + row.Cells["NewGS"].Value.ToString() + "',IsConfirm = '1',dtConfirm = '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' Where  EmpID = '" + row.Cells["empid"].Text.ToString() +  
                                   "' and ComID = " + Common.Classes.clsMain.intComId + " and '" + row.Cells["Remarks"].Value.ToString() + "' = 'Confirmation'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = " Update tblEmp_Info Set GS = '" + row.Cells["NewGS"].Value.ToString() + "',dtIncrement = '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' Where  EmpID = '" + row.Cells["empid"].Text.ToString() +
                                   "' and ComID = " + Common.Classes.clsMain.intComId + " and '" + row.Cells["Remarks"].Value.ToString() + "' = 'Increment'";
                        arQuery.Add(sqlQuery);

                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    }
                }

                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                             + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                             "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved Successfully");

                prcClearData();
                prcLoadList();
                PrcLoadCombo();

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


        private Boolean fncBlank()
        {
 

            if (dtFrom.Text.Length == 0)
            {
                MessageBox.Show("Please provide requisition date.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtFrom.Focus();
                return true;
            }
    
            

            return false;


        }


        private void cboStyle_Validating(object sender, CancelEventArgs e)
        {
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Advance Salary Amount.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
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

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Delete  tblSal_Suspense Where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtInput =  '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' and ComID = " + Common.Classes.clsMain.intComId + "";
                            arQuery.Add(sqlQuery);

                        }
                    }


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


        private void cboSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSec.Width;
            cboSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            cboSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboSec.DisplayMember = "SectName";
            cboSec.ValueMember = "SectId";
        }

        private void cboBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboBand.DisplayLayout.Bands[0].Columns["varName"].Width = cboBand.Width;
            cboBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";
            cboBand.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            cboBand.DisplayMember = "varName";
            cboBand.ValueMember = "varId";
        }

        private void cboEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = cboBand.Width;
            cboEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";
            //cboEmp.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            cboEmp.DisplayMember = "EmpName";
            cboEmp.ValueMember = "EmpId";
        }


        private void btnCalculate_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            Int32 rowCount;


            try
            {
                if (txtPer.Text == "0")
                {

                    for (rowCount = 0; rowCount < dsList.Tables["Grid"].Rows.Count; rowCount++)
                    {

                        gridDetails.Rows[rowCount].Cells[8].Value = txtPer.Text.ToString();
                        gridDetails.Rows[rowCount].Cells[9].Value = gridDetails.Rows[rowCount].Cells[7].Value;
                        gridDetails.Rows[rowCount].Cells[10].Value = cboType.Text.ToString();

                    }
                                     
                }

                else

                {

                    for (rowCount = 0; rowCount < dsList.Tables["Grid"].Rows.Count; rowCount++)
                    {
                        Double GS = Convert.ToInt64(gridDetails.Rows[rowCount].Cells[7].Value);
                        Double BS = Convert.ToInt64((GS - 560) / 1.4);
                        Double HR = Convert.ToInt64(GS - (BS + 560));

                        Double Percentage = double.Parse(txtPer.Text.ToString());

                        Int64 BSNew = Convert.ToInt64(BS + ((BS * Percentage) / 100));
                        Int64 HRNew = Convert.ToInt64((BSNew * 40) / 100);
                        Int64 GSNew = BSNew + HRNew + 560;

                        Double IncAmount = ((BSNew + HRNew + 560) - GS);

                        gridDetails.Rows[rowCount].Cells[8].Value = IncAmount.ToString();
                        gridDetails.Rows[rowCount].Cells[9].Value = GSNew.ToString();
                        gridDetails.Rows[rowCount].Cells[10].Value = cboType.Text.ToString();

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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                {
                    row.Cells["isChecked"].Value = 1;
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                {
                    row.Cells["isChecked"].Value = 0;
                }
            }
        }

        private void btnRpt_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
            var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, 1);
            dtFrom.Value = firstDay;

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;

            string ReportPath = "", SQLQuery1 = "", FormCaption = "", DataSourceName = "DataSet1";
            DataSourceName = "DataSet1";

            FormCaption = "Report :: Loan Information...";

            try
            {


                sqlQuery = " Delete  tblTempIncr";
                arQuery.Add(sqlQuery);

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Delete  tblTempIncr Where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "'  and ComID = " + Common.Classes.clsMain.intComId + "";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblTempIncr(ComId,EmpId,dtInc,PrevSal,Amount,NewSal,Remarks) "
                                       + " Values (" + Common.Classes.clsMain.intComId + ",'" + row.Cells["empid"].Text.ToString() + "', '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + 
                                       row.Cells["GS"].Value.ToString() + "','" +
                                       row.Cells["Amount"].Value.ToString() + "','" + row.Cells["NewGS"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "')";
                            arQuery.Add(sqlQuery);

                        }
                    }


                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrTemp.rdlc";
                    SQLQuery1 = "Exec [rptIncTmp] " + Common.Classes.clsMain.intComId + "";



                    clsReport.strReportPathMain = ReportPath;
                    clsReport.strQueryMain = SQLQuery1;
                    clsReport.strDSNMain = DataSourceName;

                    FM.prcShowReport(FormCaption);
                
                //prcClearData();
                //prcLoadList();
                //PrcLoadCombo();
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


    }
}
