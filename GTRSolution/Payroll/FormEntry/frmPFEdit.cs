using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmPFEdit : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmPFEdit(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlquary = "Exec PrcProcessPFManual  " + Common.Classes.clsMain.intComId + ",'',0,0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "tblEmp";
                dsList.Tables[2].TableName = "tblBand";
                dsList.Tables[3].TableName = "tblGrid";

                gridProssType.DataSource = null;
                gridProssType.DataSource = dsList.Tables["ProssType"];

                gridEmp.DataSource = null;
                gridEmp.DataSource = dsList.Tables["tblEmp"];


                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["tblGrid"];

                //DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //dtDateFrom.Value = firstDay;

                //DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //lastDay = lastDay.AddMonths(1);
                //lastDay = lastDay.AddDays(-(lastDay.Day));
                //dtDateTo.Value = lastDay;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }       




        
        private void prcShowReport()
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Night Allowance...";

                string ProssType = "",SectId = "0", Band = "";

                ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

                SectId = gridEmp.ActiveRow.Cells["SectId"].Value.ToString();


                rptQuery = "Exec PrcProcessPFManual " + Common.Classes.clsMain.intComId + ",'" + ProssType + "'," + SectId + ",2";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails,rptQuery);

                //SQLQuery = "Exec rptSalaryCasual 3,'10-Nov-13','16-Nov-2013', 0, 0, 'Casual Worker'";

                if(dsDetails.Tables[0].Rows.Count==0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }
                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = rptQuery;
                clsReport.strDSNMain = DataSourceName;
                clsReport.dsReport = dsDetails;

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                rptQuery = null;
                DataSourceName = null;
                DataSourceName = null;
                ReportPath = null;
                dsDetails = null;
            }
        }

        private void frmPFEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmPFEdit_Load(object sender, EventArgs e)
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

 

        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 285;
            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "Process Type";

            //Change alternate color
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridProssType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridProssType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridProssType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridProssType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }




        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridEmp.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpID"].Width = 80;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 160;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpID"].Header.Caption = "EmpID";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";

            //Change alternate color
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

       

        private void gridDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                gridDetails.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["ProssType"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["dtPF"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Hidden = true;

                //Set Caption
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 60; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridDetails.DisplayLayout.Bands[0].Columns["PayTotal"].Header.Caption = "PayTotal";
                gridDetails.DisplayLayout.Bands[0].Columns["Profit"].Header.Caption = "Interest";


                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Width = 135;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["PayTotal"].Width = 90;
                gridDetails.DisplayLayout.Bands[0].Columns["Profit"].Width = 75;


                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["PayTotal"].CellActivation = Activation.NoEdit;


                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Hiding +/- Indicator
                gridDetails.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                //e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            string Band = "";
            string  EmpId = "0",ProssType = "";


            ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

            EmpId = gridEmp.ActiveRow.Cells["EmpID"].Value.ToString();



            try
            {
                sqlQuery = "Exec PrcProcessPFManual " + Common.Classes.clsMain.intComId + ",'" + ProssType + "'," + EmpId + ",2";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["tblGrid"];


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

        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {
                //Member Master Table

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1" &&
                            row.Cells["Profit"].Value.ToString() != "0")
                        {

                            sqlQuery = " Update tblEmp_PF Set Profit = '" + row.Cells["Profit"].Text.ToString()
                                                             + "' Where EmpID = '" + row.Cells["EmpID"].Text.ToString()
                                                             + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                                       sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Save')";
                            arQuery.Add(sqlQuery);


                            sqlQuery = "Exec PrcProcessPFManual " + Common.Classes.clsMain.intComId + ",'" + row.Cells["ProssType"].Text.ToString() + "','" + row.Cells["EmpID"].Text.ToString() + "',3";
                            arQuery.Add(sqlQuery);

                            //Transaction with database
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        }
                    }



                    MessageBox.Show("Data Save Succefully.");


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

        private void btnCorrection_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {
                //Member Master Table

                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0 &&
                        row.Cells["isChecked"].Value.ToString() == "1" &&
                        row.Cells["Profit"].Value.ToString() != "0")
                    {

                        sqlQuery = " Update tblEmp_PF Set CLProfit = CLProfit - Profit,GrandTotal = GrandTotal-Profit,PayProfit = PayProfit - Profit,PayTotal = PayTotal - Profit Where EmpID = '" + row.Cells["EmpID"].Text.ToString()
                                                         + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = " Update tblEmp_PF Set Profit = 0 Where EmpID = '" + row.Cells["EmpID"].Text.ToString()
                                 + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                                   sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);


                        sqlQuery = "Exec PrcProcessPFManual " + Common.Classes.clsMain.intComId + ",'" + row.Cells["ProssType"].Text.ToString() + "','" + row.Cells["EmpID"].Text.ToString() + "',4";
                        arQuery.Add(sqlQuery);

                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    }
                }



                MessageBox.Show("Data Correction Complete Succefully.");


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

        //private Boolean fncBlank()
        //{


        //    if (dtDateFrom.Text.Length == 0)
        //    {
        //        MessageBox.Show("Please provide requisition date.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        dtDateFrom.Focus();
        //        return true;
        //    }



        //    return false;


        //}


        //private void gridDetails_AfterCellUpdate(object sender, CellEventArgs e)
        //{
        //    if (gridDetails.ActiveRow.IsFilterRow != true)
        //    {

        //        DataSet dsChange = new DataSet();
        //        clsConnection clscon = new clsConnection();
        //        try
        //        {


        //            Int32 CF = 0;
        //            Int32 Night = 0;
        //            Int32 Food = 0;
        //            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in gridDetails.Rows)
        //            {

        //                if (row.Cells["empid"].Text.ToString().Length != 0 &&
        //                    row.Cells["NightAbsent"].Value.ToString() != "0")
        //                {
        //                    double NightPay = Math.Round(500 - (33.33 * Convert.ToDouble(row.Cells[7].Value)), 0);
        //                    double FoodPay = Math.Round(450 - (30 * Convert.ToDouble(row.Cells[7].Value)),0);

        //                    Night = Convert.ToInt32(NightPay);
        //                    Food = Convert.ToInt32(FoodPay);


        //                    CF = (Night % 5);

        //                    if (CF > 0)
        //                    {
        //                        CF = 5 - CF;
        //                    }
        //                    else
        //                    {
        //                        CF = 0;
        //                    }


        //                    Night = Night + CF;

        //                    row.Cells[8].Value = Night;
        //                    row.Cells[9].Value = Food;
        //                    row.Cells[10].Value = Night + Food;

        //                }
        //            }
                    
                               
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
         //}


 



      }
  }

