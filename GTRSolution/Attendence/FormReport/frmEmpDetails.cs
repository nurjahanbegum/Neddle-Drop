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

namespace GTRHRIS.Attendence.FormReport
{
    public partial class frmEmpDetails : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmEmpDetails(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptEmpDetails  " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Section";
                dsList.Tables[1].TableName = "Employee";
                dsList.Tables[2].TableName = "ReportCategory";


                gridSection.DataSource = dsList.Tables["Section"];
                gridEmployeeID.DataSource = dsList.Tables["Employee"];
                gridrptCategory.DataSource = dsList.Tables["ReportCategory"];

                dtFirst.Value = DateTime.Now;
                dtLast.Value = DateTime.Now;

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

        private void gridSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSection.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSection.DisplayLayout.Bands[0].Columns["SectName"].Width = 221;
            gridSection.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSection.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSection.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSection.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSection.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSection.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridEmployeeID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 215;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmployeeID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmployeeID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmployeeID.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }
        
        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {                        
            gridSection.Enabled = true;
            gridEmployeeID.Enabled = true;
            lvlFirst.Visible = false;
            lvlLast.Visible = false;
            dtFirst.Visible = false;
            dtLast.Visible = false;

            if(optCriteria.Value.ToString() =="All")
            {
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                lvlFirst.Visible = false;
                lvlLast.Visible = false;
                dtFirst.Visible = false;
                dtLast.Visible = false;
            }
            else if (optCriteria.Value.ToString() == "Section")
            {
                gridSection.Enabled = true;
                gridEmployeeID.Enabled = false;
                lvlFirst.Visible = false;
                lvlLast.Visible = false;
                dtFirst.Visible = false;
                dtLast.Visible = false;
            }
            else if (optCriteria.Value.ToString() == "Employee")
            {
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = true;
                lvlFirst.Visible = false;
                lvlLast.Visible = false;
                dtFirst.Visible = false;
                dtLast.Visible = false;
            }
            else if (optCriteria.Value.ToString() == "Date")
            {
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                lvlFirst.Visible = true;
                lvlLast.Visible = true;
                dtFirst.Visible = true;
                dtLast.Visible = true;
            }
        }
        private void frmEmpDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmEmpDetails_Load(object sender, EventArgs e)
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();


            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Employee Details Information...";

                string SQLQuery = "";

                string SectId = "0", EmpId = "0",Datewise = "0";

                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                {
                    SectId = gridSection.ActiveRow.Cells["SectId"].Value.ToString();
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Date".ToUpper())
                {
                    Datewise = "1";
                }

                if ((gridrptCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Employee Details"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptEmpDetails.rdlc";
                    SQLQuery = "Exec rptEmployeeDetails " + Common.Classes.clsMain.intComId + ", '" + SectId + "', '" + EmpId + "','" + Datewise + "','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "' ";
                }
                else if ((gridrptCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Service Book"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptServiceBook.rdlc";
                    SQLQuery = "Exec rptEmployeeDetails " + Common.Classes.clsMain.intComId + ", '" + SectId + "', '" + EmpId + "','" + Datewise + "','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "' ";
                }


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
                clsCon = null;
            }
        }

        private void gridrptCategory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridrptCategory.DisplayLayout.Bands[0].Columns["rptid"].Hidden = true;
            gridrptCategory.DisplayLayout.Bands[0].Columns["rptname"].Width = 190;
            gridrptCategory.DisplayLayout.Bands[0].Columns["rptname"].Header.Caption = "Report Type";

            //Change alternate color
            gridrptCategory.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridrptCategory.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridrptCategory.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridrptCategory.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridrptCategory.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridReportCategory.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

      }
  }

