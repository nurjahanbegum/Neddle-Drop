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
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmrptManualDeno : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();
        //GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptManualDeno(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',0,'','','',0,'','',0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[2].TableName = "Section"; 
                dsList.Tables[4].TableName = "PayMode";
                dsList.Tables[6].TableName = "EmpType";
                dsList.Tables[7].TableName = "EmpStatus";
                dsList.Tables[8].TableName = "Band";
                dsList.Tables[9].TableName = "Grid";
                dsList.Tables[10].TableName = "tblDType";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridSec.DataSource = dsList.Tables["Section"];
                gridBand.DataSource = dsList.Tables["Band"];
                gridDetails.DataSource = dsList.Tables["Grid"];

                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            try
            {
                cboPayMode.DataSource = dsList.Tables["PayMode"];
                cboEmpType.DataSource = dsList.Tables["EmpType"];
                cboStatus.DataSource = dsList.Tables["EmpStatus"];
                cboDType.DataSource = dsList.Tables["tblDType"];

                cboPayMode.Text = "=ALL=";
                cboEmpType.Text = "=ALL=";
                cboStatus.Text = "=ALL=";
                cboDType.Text = "Salary";

            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }

        private void frmrptManualDeno_Load(object sender, EventArgs e)
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

        private void frmrptManualDeno_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void gridSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 190;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;

            gridBand.DisplayLayout.Bands[0].Columns["chk"].Width = 50;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 130;

            gridBand.DisplayLayout.Bands[0].Columns["Chk"].Header.Caption = "Check";
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";

            gridBand.DisplayLayout.Bands[0].Columns["Chk"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

            //Change alternate color
            gridBand.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridBand.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            //gridBand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //gridBand.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridBand.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            string ReportPath = "", SQLQuery1 = "", FormCaption = "", DataSourceName = "DataSet1";
            DataSourceName = "DataSet1";

            FormCaption = "Report :: Manual Denomination ...";

            try
            {
                sqlQuery = "Delete tblTempMDeno";
                arQuery.Add(sqlQuery);

                foreach (UltraGridRow row in this.gridDetails.Rows)
                {
                    sqlQuery = "Insert Into tblTempMDeno (Band,Person,SalaryTtl,TK1000,TK500,TK100,TK50,TK20,TK10,TK5)" +
                               " Values ('" + row.Cells["Band"].Text.ToString() +
                               "','" + row.Cells["Person"].Text.ToString() + "','" + row.Cells["SalaryTtl"].Text.ToString() +
                               "','" + row.Cells["TK1000"].Text.ToString() + "','" + row.Cells["TK500"].Text.ToString() +
                               "','" + row.Cells["TK100"].Text.ToString() + "','" + row.Cells["TK50"].Text.ToString() +
                               "','" + row.Cells["TK20"].Text.ToString() + "','" + row.Cells["TK10"].Text.ToString() + "','" + row.Cells["TK5"].Text.ToString() + "')";
                    arQuery.Add(sqlQuery);

                }

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumDenomination.rdlc";
                sqlQuery = "Exec [rptSalaryDenoManual] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboDType.Text.ToString() + "'";


                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);

                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = sqlQuery;
                //clsReport.strDSNMain = DataSourceName;

                //FM.prcShowReport(FormCaption);

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

            //try
            //{
            //    string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "";
            //    //string Check1000 = "", Check500 = "", Check100 = "", Check50 = "", Check20 = "", Check10 = "", Check5 = "";

            //    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();



            //    DataSourceName = "DataSet1";
            //    FormCaption = "Report :: Salary Information...";



            //        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumDenomination.rdlc";
            //        SQLQuery = "Exec [rptSalaryDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";

                

            //    clsReport.strReportPathMain = ReportPath;
            //    clsReport.strQueryMain = SQLQuery;
            //    clsReport.strDSNMain = DataSourceName;

            //    FM.prcShowReport(FormCaption);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }


        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 275;
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


        private void cboPayMode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPayMode.DisplayLayout.Bands[0].Columns["PayMode"].Width = cboPayMode.Width;
            cboPayMode.DisplayLayout.Bands[0].Columns["PayMode"].Header.Caption = "Pay Mode";
            cboPayMode.DisplayMember = "PayMode";
            cboPayMode.ValueMember = "PayMode";
        }

        private void cboEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Width = cboEmpType.Width;
            cboEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Employee Type";
            cboEmpType.DisplayMember = "EmpType";
            cboEmpType.ValueMember = "EmpType";
        }

        private void cboStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Width = cboStatus.Width;
            cboStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Header.Caption = "Employee Status";
            cboStatus.DisplayMember = "EmpStatus";
            cboStatus.ValueMember = "EmpStatus";
        }

        private void cboDType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDType.DisplayLayout.Bands[0].Columns["DType"].Width = cboEmpType.Width;
            cboDType.DisplayLayout.Bands[0].Columns["DType"].Header.Caption = "Employee Type";
            cboDType.DisplayLayout.Bands[0].Columns["SL"].Hidden = true;
            cboDType.DisplayMember = "DType";
            cboDType.ValueMember = "SL";
        }



        private void optCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboUnit_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboPaySource_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboPayMode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpStatus_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnPreview_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void prcCalucalateTotal()
        {
            double dblTtlEmp = 0;
            double dblTotal = 0;
            double dblTtl1000 = 0;
            double dblTtl500 = 0;
            double dblTtl100 = 0;
            double dblTtl50 = 0;
            double dblTtl20 = 0;
            double dblTtl10 = 0;
            double dblTtl5 = 0;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
            {
                if (fncValidateDouble(row.Cells["Person"].Value.ToString()) > 0)
                {
                    dblTtlEmp += fncValidateDouble(row.Cells["Person"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["SalaryTtl"].Value.ToString()) > 0)
                {
                    dblTotal += fncValidateDouble(row.Cells["SalaryTtl"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK1000"].Value.ToString()) > 0)
                {
                    dblTtl1000 += fncValidateDouble(row.Cells["TK1000"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK500"].Value.ToString()) > 0)
                {
                    dblTtl500 += fncValidateDouble(row.Cells["TK500"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK100"].Value.ToString()) > 0)
                {
                    dblTtl100 += fncValidateDouble(row.Cells["TK100"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK50"].Value.ToString()) > 0)
                {
                    dblTtl50 += fncValidateDouble(row.Cells["TK50"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK20"].Value.ToString()) > 0)
                {
                    dblTtl20 += fncValidateDouble(row.Cells["TK20"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK10"].Value.ToString()) > 0)
                {
                    dblTtl10 += fncValidateDouble(row.Cells["TK10"].Value.ToString());
                }
                if (fncValidateDouble(row.Cells["TK5"].Value.ToString()) > 0)
                {
                    dblTtl5 += fncValidateDouble(row.Cells["TK5"].Value.ToString());
                }
            }
            txtTtlEmp.Text = dblTtlEmp.ToString();
            txtTotal.Text = dblTotal.ToString();
            txt1000.Text = dblTtl1000.ToString();
            txt500.Text = dblTtl500.ToString();
            txt100.Text = dblTtl100.ToString();
            txt50.Text = dblTtl50.ToString();
            txt20.Text = dblTtl20.ToString();
            txt10.Text = dblTtl10.ToString();
            txt5.Text = dblTtl5.ToString();

            txt1.Text = dblTtl1000.ToString();
            txt2.Text = dblTtl500.ToString();
            txt3.Text = dblTtl100.ToString();
            txt4.Text = dblTtl50.ToString();
            txt6.Text = dblTtl20.ToString();
            txt7.Text = dblTtl10.ToString();
            txt8.Text = dblTtl5.ToString();

            //txtInWords.Text = clsProc.GTRInwordsFormatBD(txtTotal.Text, "BDT", "Only");

        }
        private Double fncValidateDouble(string value)
        {
            Double dbl;
            try
            {
                dbl = Double.Parse(value);
            }
            catch (Exception)
            {
                dbl = 0;
            }
            return dbl;
        }

        private Boolean fncBlank()
        {
            if (this.check1000.Checked == false && this.check500.Checked == false && this.check100.Checked == false && this.check50.Checked == false && this.check20.Checked == false && this.check10.Checked == false && this.check5.Checked == false)
            {
                MessageBox.Show("Please Check Mark Which Note You Want to Change.");
                check1000.Focus();
                return true;
            }


            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (fncBlank())
            {
                return;
            }
            
            
            Int64 Ttl = Convert.ToInt64(txtTotal.Value.ToString());
            Int64 Ttl1000 = Convert.ToInt64(txt1000.Value.ToString());
            Int64 Ttl500 = Convert.ToInt64(txt500.Value.ToString());
            Int64 Ttl100 = Convert.ToInt64(txt100.Value.ToString());
            Int64 Ttl50 = Convert.ToInt64(txt50.Value.ToString());
            Int64 Ttl20 = Convert.ToInt64(txt20.Value.ToString());
            Int64 Ttl10 = Convert.ToInt64(txt10.Value.ToString());
            Int64 Ttl5 = Convert.ToInt64(txt5.Value.ToString());

            Int64 T1000 = Convert.ToInt64(txt1.Value.ToString());
            Int64 T500 = Convert.ToInt64(txt2.Value.ToString());
            Int64 T100 = Convert.ToInt64(txt3.Value.ToString());
            Int64 T50 = Convert.ToInt64(txt4.Value.ToString());
            Int64 T20 = Convert.ToInt64(txt6.Value.ToString());
            Int64 T10 = Convert.ToInt64(txt7.Value.ToString());
            Int64 T5 = Convert.ToInt64(txt8.Value.ToString());


            Int64 Tt1 = Convert.ToInt64(txt1.Value.ToString()) * 1000;
            Int64 Tt2 = Convert.ToInt64(txt2.Value.ToString()) * 500;
            Int64 Tt3 = Convert.ToInt64(txt3.Value.ToString()) * 100;
            Int64 Tt4 = Convert.ToInt64(txt4.Value.ToString()) * 50;
            Int64 Tt6 = Convert.ToInt64(txt6.Value.ToString()) * 20;
            Int64 Tt7 = Convert.ToInt64(txt7.Value.ToString()) * 10;
            Int64 Tt8 = Convert.ToInt64(txt8.Value.ToString()) * 5;

            Int64 TtlSum = Tt1 + Tt2 + Tt3 + Tt4 + Tt6 + Tt7 + Tt8;
            String R = Convert.ToString(TtlSum - Ttl);

            if (TtlSum != Ttl)
            {
                MessageBox.Show("Total amount do not match. Difference amount.[" + R + "].Please insert correct amount.");
            }

            else
            {
                ArrayList arQuery = new ArrayList();
                GTRLibrary.clsConnection clsCon = new clsConnection();

                string sqlQuery = "";
                Int32 NewId = 0;
                Int32 SL = 1; 

                try
                {


                    //1000 tk & 500 Tk Change

                    //int a = int.Parse(dtxt1.Text.ToString());
                    ////int b = int.Parse(dtxt2.Text.ToString());

                    //for (int i = 0; i < a; i = 0)
                    //{
                    //    foreach (UltraGridRow row in this.gridDetails.Rows)
                    //    {
                    //        if (a > 0)
                    //        {
                    //            if (int.Parse(row.Cells["tk1000"].Value.ToString()) > 0)
                    //            {
                    //                row.Cells["tk1000"].Value = int.Parse(row.Cells["tk1000"].Value.ToString()) - 1;
                    //                row.Cells["tk500"].Value = int.Parse(row.Cells["tk500"].Value.ToString()) + 2;
                    //                a = a - 1;
                    //            }

                    //        }
                    //    }
                    //}


                    //End Code
                    
                    
                    
                    sqlQuery = "Truncate Table tblTempMDeno";
                    arQuery.Add(sqlQuery);

                    foreach (UltraGridRow row in this.gridDetails.Rows)
                    {



                        sqlQuery = "Insert Into tblTempMDeno (ComId,Band,Person,SalaryTtl,TK1000,TK500,TK100,TK50,TK20,TK10,TK5,SL)" +
                                   " Values (" + Common.Classes.clsMain.intComId + ",'" + row.Cells["Band"].Text.ToString() +
                                   "','" + row.Cells["Person"].Text.ToString() + "','" + row.Cells["SalaryTtl"].Text.ToString() +
                                   "','" + row.Cells["TK1000"].Text.ToString() + "','" + row.Cells["TK500"].Text.ToString() +
                                   "','" + row.Cells["TK100"].Text.ToString() + "','" + row.Cells["TK50"].Text.ToString() +
                                   "','" + row.Cells["TK20"].Text.ToString() + "','" + row.Cells["TK10"].Text.ToString() + "','" + row.Cells["TK5"].Text.ToString() + "'," + SL + ")";
                        arQuery.Add(sqlQuery);

                        SL = SL + 1;

                    }

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);


                    clsConnection clscon = new clsConnection();
                    dsList = new System.Data.DataSet();

                    sqlQuery = "Exec prcProcessManualDeno " + Common.Classes.clsMain.intComId + ",'" + txt1.Value.ToString()
                            + "','" + txt2.Value.ToString() + "','" + txt3.Value.ToString() + "','" + txt4.Value.ToString()
                            + "','" + txt6.Value.ToString() + "','" + txt7.Value.ToString() + "','" + txt8.Value.ToString()
                            + "','" + txt1000.Value.ToString() + "','" + txt500.Value.ToString() + "','" + txt100.Value.ToString()
                            + "','" + txt50.Value.ToString() + "','" + txt20.Value.ToString() + "','" + txt10.Value.ToString()
                            + "','" + txt5.Value.ToString() + "' ";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                    //sqlQuery = "Exec prcProcessManualDeno " + Common.Classes.clsMain.intComId + ",'" + txt1.Value.ToString()
                    //    + "','" + txt2.Value.ToString() + "','" + txt3.Value.ToString() + "','" + txt4.Value.ToString()
                    //    + "','" + txt6.Value.ToString() + "','" + txt7.Value.ToString() + "','" + txt8.Value.ToString()
                    //    + "','" + check1000.Tag.ToString() + "','" + check500.Tag.ToString() + "','" + check100.Tag.ToString()
                    //    + "','" + check50.Tag.ToString() + "','" + check20.Tag.ToString() + "','" + check10.Tag.ToString()
                    //    + "','" + check5.Tag.ToString() + "' ";
                    //clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                    //arQuery.Add(sqlQuery);
                    //clsCon.GTRSaveDataWithSQLCommand(arQuery);


                    dsList.Tables[0].TableName = "tblCal";
                    gridDetails.DataSource = dsList.Tables["tblCal"];
                    prcCalucalateTotal();



                    check1000.Checked = false;
                    check500.Checked = false;
                    check100.Checked = false;
                    check50.Checked = false;
                    check20.Checked = false;
                    check10.Checked = false;
                    check5.Checked = false;

                    MessageBox.Show("Update Successfully Complete.");

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
        }


        private void btnCalCulate_Click(object sender, EventArgs e)
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            string sqlquary;


            try
            {

                if (cboDType.Text == "Salary")
                {
                    if (gridBand.ActiveRow.Cells["varName"].Text.ToString() == "=ALL=")
                    {
                        sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',1,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','=ALL=','" + cboStatus.Value.ToString() + "',0";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                        dsList.Tables[0].TableName = "tblCal";

                        gridDetails.DataSource = dsList.Tables["tblCal"];

                        prcCalucalateTotal();

                    }

                    else
                    {

                        sqlquary = "Delete From tblTempMDeno";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                        foreach (UltraGridRow row in this.gridBand.Rows)
                        {
                            if (row.Cells["Chk"].Value.ToString() == "1")
                            {

                                sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',1,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + row.Cells["varName"].Text.ToString() + "','" + cboStatus.Value.ToString() + "',0";
                                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);



                            }


                        }
                        sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',1,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','Band','" + cboStatus.Value.ToString() + "',1";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                        dsList.Tables[0].TableName = "tblCal";
                        gridDetails.DataSource = dsList.Tables["tblCal"];
                        prcCalucalateTotal();
                    }

                }


               if (cboDType.Text == "Festival Bonus")
                {
                    if (gridBand.ActiveRow.Cells["varName"].Text.ToString() == "=ALL=")
                    {
                        sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',2,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','=ALL=','" + cboStatus.Value.ToString() + "',0";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                        dsList.Tables[0].TableName = "tblCal";

                        gridDetails.DataSource = dsList.Tables["tblCal"];

                        prcCalucalateTotal();

                    }

                    else
                    {

                        sqlquary = "Delete From tblTempMDeno";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                        foreach (UltraGridRow row in this.gridBand.Rows)
                        {
                            if (row.Cells["Chk"].Value.ToString() == "1")
                            {

                                sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',2,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + row.Cells["varName"].Text.ToString() + "','" + cboStatus.Value.ToString() + "',0";
                                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);



                            }


                        }
                        sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',2,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','Band','" + cboStatus.Value.ToString() + "',1";
                        clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                        dsList.Tables[0].TableName = "tblCal";
                        gridDetails.DataSource = dsList.Tables["tblCal"];
                        prcCalucalateTotal();
                    }

                }


               if (cboDType.Text == "Advance Salary")
               {
                   if (gridBand.ActiveRow.Cells["varName"].Text.ToString() == "=ALL=")
                   {
                       sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',3,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','=ALL=','" + cboStatus.Value.ToString() + "',0";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       dsList.Tables[0].TableName = "tblCal";

                       gridDetails.DataSource = dsList.Tables["tblCal"];

                       prcCalucalateTotal();

                   }

                   else
                   {

                       sqlquary = "Delete From tblTempMDeno";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       foreach (UltraGridRow row in this.gridBand.Rows)
                       {
                           if (row.Cells["Chk"].Value.ToString() == "1")
                           {

                               sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',3,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + row.Cells["varName"].Text.ToString() + "','" + cboStatus.Value.ToString() + "',0";
                               clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);



                           }


                       }
                       sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',3,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','Band','" + cboStatus.Value.ToString() + "',1";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       dsList.Tables[0].TableName = "tblCal";
                       gridDetails.DataSource = dsList.Tables["tblCal"];
                       prcCalucalateTotal();
                   }

               }


               if (cboDType.Text == "KPI Incentive")
               {

                       sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',4,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','=ALL=','" + cboStatus.Value.ToString() + "',0";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       dsList.Tables[0].TableName = "tblCal";

                       gridDetails.DataSource = dsList.Tables["tblCal"];

                       prcCalucalateTotal();


               }



               if (cboDType.Text == "Earn Leave")  //Earn Leave
               {
                   if (gridBand.ActiveRow.Cells["varName"].Text.ToString() == "=ALL=")
                   {
                       sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',5,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','=ALL=','" + cboStatus.Value.ToString() + "',0";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       dsList.Tables[0].TableName = "tblCal";

                       gridDetails.DataSource = dsList.Tables["tblCal"];

                       prcCalucalateTotal();

                   }

                   else
                   {

                       sqlquary = "Delete From tblTempMDeno";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       foreach (UltraGridRow row in this.gridBand.Rows)
                       {
                           if (row.Cells["Chk"].Value.ToString() == "1")
                           {

                               sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',5,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + row.Cells["varName"].Text.ToString() + "','" + cboStatus.Value.ToString() + "',0";
                               clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);



                           }


                       }
                       sqlquary = "Exec prcrptManualDeno " + Common.Classes.clsMain.intComId + ", 'Admin',5,'" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','Band','" + cboStatus.Value.ToString() + "',1";
                       clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                       dsList.Tables[0].TableName = "tblCal";
                       gridDetails.DataSource = dsList.Tables["tblCal"];
                       prcCalucalateTotal();
                   }

               }


            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void gridDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                //gridDetails.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;

                //Set Caption
                //gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["Person"].Header.Caption = "Person";
                gridDetails.DisplayLayout.Bands[0].Columns["SalaryTtl"].Header.Caption = "SalaryTtl";
                gridDetails.DisplayLayout.Bands[0].Columns["TK1000"].Header.Caption = "1000";
                gridDetails.DisplayLayout.Bands[0].Columns["TK500"].Header.Caption = "500";
                gridDetails.DisplayLayout.Bands[0].Columns["TK100"].Header.Caption = "100";
                gridDetails.DisplayLayout.Bands[0].Columns["TK50"].Header.Caption = "50";
                gridDetails.DisplayLayout.Bands[0].Columns["TK20"].Header.Caption = "20";
                gridDetails.DisplayLayout.Bands[0].Columns["TK10"].Header.Caption = "10";
                gridDetails.DisplayLayout.Bands[0].Columns["TK5"].Header.Caption = "5";

                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 80;
                gridDetails.DisplayLayout.Bands[0].Columns["Person"].Width = 50;
                gridDetails.DisplayLayout.Bands[0].Columns["SalaryTtl"].Width = 100;
                gridDetails.DisplayLayout.Bands[0].Columns["TK1000"].Width = 55;
                gridDetails.DisplayLayout.Bands[0].Columns["TK500"].Width = 55;
                gridDetails.DisplayLayout.Bands[0].Columns["TK100"].Width = 55;
                gridDetails.DisplayLayout.Bands[0].Columns["TK50"].Width = 55;
                gridDetails.DisplayLayout.Bands[0].Columns["TK20"].Width = 55;
                gridDetails.DisplayLayout.Bands[0].Columns["TK10"].Width = 54;
                gridDetails.DisplayLayout.Bands[0].Columns["TK5"].Width = 54;

                //this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                //   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                ////Stop Cell Modify
                //gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                //gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                //gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].CellActivation = Activation.NoEdit;
                //gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                //gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;
                //gridDetails.DisplayLayout.Bands[0].Columns["GS"].CellActivation = Activation.NoEdit;
                //gridDetails.DisplayLayout.Bands[0].Columns["OTRate"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Row Hight
                //gridDetails.DisplayLayout.Override.DefaultRowHeight = 20;

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
                                        
            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Denomination Sheet" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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
            GridToToExcel.Export(gridDetails, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }

        private void check1000_CheckedChanged(object sender, EventArgs e)
        {
            check1000.Tag = 0;
            if (check1000.Checked == true)
            {
                check1000.Tag = 1;
            }
        }

        private void check500_CheckedChanged(object sender, EventArgs e)
        {
            check500.Tag = 0;
            if (check500.Checked == true)
            {
                check500.Tag = 1;
            }
        }

        private void check100_CheckedChanged(object sender, EventArgs e)
        {
            check100.Tag = 0;
            if (check100.Checked == true)
            {
                check100.Tag = 1;
            }
        }

        private void check50_CheckedChanged(object sender, EventArgs e)
        {
            check50.Tag = 0;
            if (check50.Checked == true)
            {
                check50.Tag = 1;
            }
        }

        private void check20_CheckedChanged(object sender, EventArgs e)
        {
            check20.Tag = 0;
            if (check20.Checked == true)
            {
                check20.Tag = 1;
            }
        }

        private void check10_CheckedChanged(object sender, EventArgs e)
        {
            check10.Tag = 0;
            if (check10.Checked == true)
            {
                check10.Tag = 1;
            }
        }

        private void check5_CheckedChanged(object sender, EventArgs e)
        {
            check5.Tag = 0;
            if (check5.Checked == true)
            {
                check5.Tag = 1;
            }
        }

        private void txt1_ValueChanged(object sender, EventArgs e)
        {
            dtxt1.Text = (int.Parse(txt1000.Text.ToString()) - int.Parse(txt1.Text.ToString())).ToString();
        }

        private void txt2_ValueChanged(object sender, EventArgs e)
        {
            dtxt2.Text = (int.Parse(txt500.Text.ToString()) - int.Parse(txt2.Text.ToString())).ToString();
        }







   }
}