using System;
using System.Collections;
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
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Attendence.FormReport
{
    public partial class frmrptJobCardB : Form
    {
        string strTranWith = "";
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        GTRLibrary.clsProcedure clsProc =new GTRLibrary.clsProcedure();
           Common.FormEntry.frmMaster FM;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;

        public frmrptJobCardB(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmrptJobCard_Load(object sender, EventArgs e)
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
                //dsList.Tables[0].TableName = "Tree";
                dsList.Tables[0].TableName = "Grid";
                dsList.Tables[1].TableName = "Sect";
                dsList.Tables[2].TableName = "Line";
                dsList.Tables[3].TableName = "Floor";
                dsList.Tables[4].TableName = "Band";

                //Tree.Nodes.Clear();
                //prcGenerateTreeView(Tree.Nodes, 0, dsList.Tables[0]);
                //Tree.Select();

                //Grid 
                gridEmployee.DataSource = null;
                gridEmployee.DataSource = dsList.Tables["Grid"];

                gridSect.DataSource = null;
                gridSect.DataSource = dsList.Tables["Sect"];

                gridBand.DataSource = dsList.Tables["Band"];

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtDateFrom.Value = firstDay;

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddMonths(1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtDateTo.Value = lastDay;
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

        private void frmrptJobCard_FormClosing(object sender, FormClosingEventArgs e)
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
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 30;
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Width = 280;  
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100; 
                
                //Set Caption
                gridEmployee.DisplayLayout.Bands[0].Columns["isCheck"].Header.Caption = "Check";
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Id";
                gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                this.gridEmployee.DisplayLayout.Bands[0].Columns["isCheck"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Change alternate color
                gridEmployee.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridEmployee.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                ////Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                ////Selection Style Will Be Row Selector
                //this.gridEmployee.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////Stop Updating
                //this.gridEmployee.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

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

        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            gridEmployee.Enabled = false;
            gridSect.Enabled = false;
            cboLine.Enabled = false;
            cboFloor.Enabled = false;
            gridBand.Enabled = false;

            if (optCriteria.Value.ToString().ToUpper() == "Department".ToUpper())
            {
                gridSect.Enabled = true;
                cboLine.Enabled = true;
                cboFloor.Enabled = true;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
            {
                gridEmployee.Enabled = true;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Line".ToUpper())
            {
                gridBand.Enabled = true;
            }
        }

        private void gridSect_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide Column
                gridSect.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;

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
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            
            string ReportPath = "";
            string SQLQuery = "", Band = "=All=", strFloor = "", strLine = "";
            Int32 DeptId = 0, EmpId=0;


            if (optrptFormat.Value.ToString().ToUpper() == "General".ToUpper())
            {

                //Collecting Parameter Value
                if (optCriteria.Value.ToString().ToUpper() == "Department".ToUpper())
                {
                    DeptId = Int32.Parse(gridSect.ActiveRow.Cells["SectId"].Value.ToString());
                    strFloor = cboFloor.Text.ToString();
                    strLine = cboLine.Text.ToString();

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardB.rdlc";
                    SQLQuery = "Exec rptJobCardB " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId.ToString() + ", '" + strFloor + "','" + Band + "', " + EmpId + " ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                    clsMain.strReportName = "Job Card";

                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridEmployee.Rows)
                    {
                        if (row.Cells["EmpId"].Text.ToString().Length != 0 &&
                            row.Cells["isCheck"].Value.ToString() == "1")
                        {
                            EmpId = Int32.Parse(row.Cells["empid"].Text.ToString());

                            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardB.rdlc";
                            SQLQuery = "Exec rptJobCardB " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId.ToString() + ", '','" + Band + "', " + EmpId + " ";
                            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                            clsMain.strReportName = "Job Card";
                        }
                    }

                }
                else if (optCriteria.Value.ToString().ToUpper() == "Line".ToUpper())
                {
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardB.rdlc";
                    SQLQuery = "Exec rptJobCardB " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId.ToString() + ", '" + strFloor + "','" + Band + "', " + EmpId + " ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                    clsMain.strReportName = "Job Card";

                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                }



            }


            else
            {

                if (optCriteria.Value.ToString().ToUpper() == "Department".ToUpper())
                {
                    DeptId = Int32.Parse(gridSect.ActiveRow.Cells["SectId"].Value.ToString());
                    strFloor = cboFloor.Text.ToString();
                    strLine = cboLine.Text.ToString();

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardDetails.rdlc";
                    SQLQuery = "Exec rptJobCardMonthlyProcess " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId.ToString() + ", '" + strFloor + "','" + strLine + "', " + EmpId + "";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                    clsMain.strReportName = "Job Card Details";

                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    EmpId = Int32.Parse(gridEmployee.ActiveRow.Cells["EmpId"].Value.ToString());

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardDetails.rdlc";
                    SQLQuery = "Exec rptJobCardMonthlyProcess " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId.ToString() + ", '" + strFloor + "','" + strLine + "', " + EmpId + "";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                    clsMain.strReportName = "Job Card Details";

                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Line".ToUpper())
                {
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardB.rdlc";
                    SQLQuery = "Exec rptJobCardB " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "'," + DeptId.ToString() + ", '" + strFloor + "','" + Band + "', " + EmpId + " ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                    clsMain.strReportName = "Job Card";

                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                }


            }


            string DataSourceName = "DataSet1";
            string FormCaption = "Report :: Job Card ...";

            //GTRLibrary.clsReport.strReportPathMain = ReportPath;
            //GTRLibrary.clsReport.strQueryMain = SQLQuery;
            //GTRLibrary.clsReport.strDSNMain = DataSourceName;

            //clsMain.strExtension = optFormat.Value.ToString();
            //clsMain.strFormat = optFormat.Text.ToString();

            //FM.prcShowReport(FormCaption);


            clsReport.strReportPathMain = ReportPath;
            clsReport.dsReport = dsDetails;
            clsReport.strDSNMain = DataSourceName;
            Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
            Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
            FM.prcShowReport(FormCaption);





            //clsReport.strReportPathMain = ReportPath;
            //clsReport.strQueryMain = SQLQuery;
            //clsReport.strDSNMain = DataSourceName;

            //clsMain.strExtension = optFormat.Value.ToString();
            //clsMain.strFormat = optFormat.Text.ToString();
        }

        private void optrptFormat_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void optInv_KeyDown(object sender, KeyEventArgs e)
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

        private void gridSect_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Int16)e.KeyCode == 13)
            {
                cboFloor.Focus();
            }
        }

        private void gridEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Int16)e.KeyCode == 13)
            {
                cboFloor.Focus();
            }
        }

        private void cboFloor_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        private void cboLine_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (dtDateFrom.DateTime.Month == 1)
            {
                var firstDay = new DateTime(dtDateFrom.DateTime.Year - 1, dtDateFrom.DateTime.Month + 11, 1);
                dtDateFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month);
                var lastDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month, DaysInMonth);


                dtDateTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month - 1);
                var lastDay = new DateTime(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month - 1, DaysInMonth);
                var firstDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month - 1, 1);
                dtDateFrom.Value = firstDay;
                dtDateTo.Value = lastDay;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dtDateFrom.DateTime.Month == 12)
            {
                var firstDay = new DateTime(dtDateFrom.DateTime.Year + 1, dtDateFrom.DateTime.Month - 11, 1);
                dtDateFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month);
                var lastDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month, DaysInMonth);


                dtDateTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month + 1);
                var lastDay = new DateTime(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month + 1, DaysInMonth);
                var firstDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month + 1, 1);
                dtDateFrom.Value = firstDay;
                dtDateTo.Value = lastDay;
            }
        }
        private void gridBand_InitializeLayout_1(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 175;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Line";

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
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void ultraPanel1_PaintClient(object sender, PaintEventArgs e)
        {

        }       
    }
}
