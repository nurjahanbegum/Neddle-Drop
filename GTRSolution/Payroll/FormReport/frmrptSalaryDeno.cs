using System;
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

namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmrptSalaryDeno : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptSalaryDeno(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptSalarySheet " + Common.Classes.clsMain.intComId + ", 'Admin'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[2].TableName = "Section"; 
                dsList.Tables[4].TableName = "PayMode";
                dsList.Tables[6].TableName = "EmpType";
                dsList.Tables[7].TableName = "EmpStatus";
                dsList.Tables[8].TableName = "Band";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridSec.DataSource = dsList.Tables["Section"];
                gridBand.DataSource = dsList.Tables["Band"];

                
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

                cboPayMode.Text = "=ALL=";
                cboEmpType.Text = "=ALL=";
                cboStatus.Text = "=ALL=";

            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }

        private void frmrptSalaryDeno_Load(object sender, EventArgs e)
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

        private void frmrptSalaryDeno_FormClosing(object sender, FormClosingEventArgs e)
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
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 130;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";

            //Change alternate color
            gridBand.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridBand.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridBand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridBand.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

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

            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "";
                string Check1000 = "", Check500 = "", Check100 = "", Check50 = "", Check20 = "", Check10 = "", Check5 = "";

                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

                Check1000 = check1000.Tag.ToString();
                Check500 = check500.Tag.ToString();
                Check100 = check100.Tag.ToString();
                Check50 = check50.Tag.ToString();
                Check20 = check20.Tag.ToString();
                Check10 = check10.Tag.ToString();
                Check5 = check5.Tag.ToString();

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Salary Information...";



                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumDenomination.rdlc";
                    SQLQuery = "Exec [rptSalaryDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','" + Check1000 + "','" + Check500 + "','" + Check100 + "','" + Check50 + "','" + Check20 + "','" + Check10 + "','" + Check5 + "'";


                    clsReport.strReportPathMain = ReportPath;
                    clsReport.dsReport = dsDetails;
                    clsReport.strDSNMain = DataSourceName;
                    Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                    Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                    FM.prcShowReport(FormCaption);


                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = SQLQuery;
                //clsReport.strDSNMain = DataSourceName;

                //FM.prcShowReport(FormCaption);
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






   }
}