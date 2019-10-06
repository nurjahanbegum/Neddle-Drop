using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Text;
using GTRLibrary;
using System.Windows.Forms;

namespace GTRHRIS.Attendence.FormReport
{
    public partial class frmrptJobCardSpecial : Form
    {
        string strTranWith = "";
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        GTRLibrary.clsProcedure clsProc =new GTRLibrary.clsProcedure();
           Common.FormEntry.frmMaster FM;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;

        public frmrptJobCardSpecial(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmrptJobCardSpecial_Load(object sender, EventArgs e)
        {
            try
            {
                
                Tree.BackColor = this.BackColor;
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
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcrptJobCard " + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Tree";
                dsList.Tables[1].TableName = "Grid";
                dsList.Tables[2].TableName = "Sect";
                dsList.Tables[3].TableName = "Line";
                dsList.Tables[4].TableName = "Floor";

                Tree.Nodes.Clear();
                prcGenerateTreeView(Tree.Nodes, 0, dsList.Tables[0]);
                Tree.Select();

                //Grid 
                gridEmployee.DataSource = null;
                gridEmployee.DataSource = dsList.Tables["Grid"];

                gridSect.DataSource = null;
                gridSect.DataSource = dsList.Tables["Sect"];
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
            try
            {
                cboFloor.DataSource = null;
                cboFloor.DataSource = dsList.Tables["Floor"];
                cboLine.DataSource = null;
                cboLine.DataSource = dsList.Tables["line"];

            }
            catch (Exception ex)
            {
                
                throw(ex);
            }
        }
        protected void prcGenerateTreeView(TreeNodeCollection parentNode, int parentID, DataTable mytab)
        {
            foreach (DataRow dta in mytab.Rows)
            {
                if (Convert.ToInt32(dta["ParentId"]) == parentID)
                {
                    String key = dta["DeptID"].ToString();
                    String text = dta["DepartName"].ToString();
                    TreeNodeCollection newParentNode = parentNode.Add(key, text,key).Nodes;

                    prcGenerateTreeView(newParentNode, Convert.ToInt32(dta["DeptID"]), mytab);
                }
            }
            Tree.ExpandAll();
        }

        private void frmrptJobCardSpecial_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            clsProc = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridEmployee_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide Column
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
                //Set Width
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Width = 310;  
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100; 
                //Set Caption
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Id";
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

                //Change alternate color
                gridEmployee.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridEmployee.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridEmployee.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridEmployee.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridEmployee.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void optInv_ValueChanged(object sender, EventArgs e)
        {

            gridEmployee.Enabled = false;
            tabSection.Enabled = false;
            if (optInv.Value.ToString().ToUpper() == "Department".ToUpper())
            {
                tabSection.Enabled = true;
            }
            else
            {
                gridEmployee.Enabled = true;
            }
        }

        private void optSection_ValueChanged(object sender, EventArgs e)
        {
            tabSection.Tabs["tabTree"].Visible = true;
            tabSection.Tabs["tabGrid"].Visible = true;
            if(optSection.Value.ToString().ToUpper()=="Grid".ToUpper())
            {
                tabSection.Tabs["tabTree"].Visible = false;
            }
            else
            {
                tabSection.Tabs["tabGrid"].Visible = false;
            }
        }

        private void gridSect_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide Column
                gridSect.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
                gridSect.DisplayLayout.Bands[0].Columns["DeptID"].Hidden = true;

                //Set Width
                gridSect.DisplayLayout.Bands[0].Columns["SectName"].Width = gridSect.Width;  
                //gridSect.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100;
                //Set Caption
                //gridSect.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Id";
                gridSect.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section Name";

                //Change alternate color
                gridSect.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridSect.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridSect.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridSect.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridSect.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboFloor_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFloor.DisplayLayout.Bands[0].Columns["varName"].Width = cboFloor.Width;
            cboFloor.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Floor";

        }

        private void cboLine_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboLine.DisplayLayout.Bands[0].Columns["varName"].Width = cboLine.Width;
            cboLine.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Line";
        }

        private void btnPrevew_Click(object sender, EventArgs e)
        {

            string ReportPath = "";
            string SQLQuery = "";
            Int32 DeptId = 0;
            if(optSection.Value.ToString().ToUpper()=="Grid".ToUpper())
            {
                DeptId =Int32.Parse(gridSect.ActiveRow.Cells["Deptid"].Value.ToString());
            }
            else
            {
                DeptId = Int32.Parse(Tree.SelectedNode.ImageKey.ToString());
            }
            if(optInv.Value.ToString().ToUpper()=="Department".ToUpper())
            {
                ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCard.rdlc";
                SQLQuery = "Exec rptJobCard " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId + ", '" + cboFloor.Text + "','" + cboLine.Text + "', 0 ";
                
            }
            else
            {
                ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCard.rdlc";
                SQLQuery = "Exec rptJobCard " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "',0, '" + cboFloor.Text + "','" + cboLine.Text + "',"+gridEmployee.ActiveRow.Cells["Empid"].Value+" ";
                
            }

            string DataSourceName = "DataSet1";
            string FormCaption = "Report :: Job Card ...";

            clsReport.strReportPathMain = ReportPath;
            clsReport.dsReport = dsDetails;
            clsReport.strDSNMain = DataSourceName;
            Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
            Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
            FM.prcShowReport(FormCaption);

            //GTRLibrary.clsReport.strReportPathMain = ReportPath;
            //GTRLibrary.clsReport.strQueryMain = SQLQuery;
            //GTRLibrary.clsReport.strDSNMain = DataSourceName;

            //FM.prcShowReport(FormCaption);
        }


       
    }
}
