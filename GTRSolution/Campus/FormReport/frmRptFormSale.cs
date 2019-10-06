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
//using GTRLibrary;

namespace GTRHRIS.Campus.FormReport
{
    public partial class frmRptFormSale : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmRptFormSale(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsList = new System.Data.DataSet();
            try
            {
                string sqlquary = "Exec prcGetFormSale  " + Common.Classes.clsMain.intComId;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Session";
                dsList.Tables[1].TableName = "Class";

                gridSession.DataSource = null;
                gridSession.DataSource = dsList.Tables["Session"];

                gridClass.DataSource = null;
                gridClass.DataSource = dsList.Tables["Class"];

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtDateFrom.Value = firstDay;

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
            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Form Sales Report...";

                string Type = " ";

                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                    Type = optCriteria.Value.ToString();
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Sub".ToUpper())
                {
                    Type = optCriteria.Value.ToString();
                }
                if (optCriteria.Value.ToString().ToUpper() == "Non".ToUpper())
                {
                    Type = optCriteria.Value.ToString();
                }

                if (gridSession.ActiveRow.Cells["sesn"].Text == "ALL")
                {
                    //gridSession.ActiveRow.Cells["sesn"].Value = 0;
                }

                rptQuery = "Exec gtrhris.dbo.rptFormSale " + Common.Classes.clsMain.intComId + ",'" + Type + "', 0, " + gridClass.ActiveRow.Cells["clsId"].Value + ", '" + gridSession.ActiveRow.Cells["sesn"].Value + "', '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0 ";

                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);
                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = rptQuery;
                //clsReport.strDSNMain = DataSourceName;
                //clsReport.dsReport = dsDetails;


                //FM.prcShowReport(FormCaption);
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
            }
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Campus\Reports\rptFormSale.rdlc";
            //ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptEmpDetails.rdlc";
            prcShowReport();
        }

        private void gridSession_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSession.DisplayLayout.Bands[0].Columns["sesn"].Width = 200;
            gridSession.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";

            //Change alternate color
            gridSession.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSession.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSession.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSession.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSession.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridClass_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridClass.DisplayLayout.Bands[0].Columns["clsId"].Hidden = true;
            gridClass.DisplayLayout.Bands[0].Columns["clsName"].Width = 200;
            gridClass.DisplayLayout.Bands[0].Columns["clsName"].Header.Caption = "Class Name";

            //Change alternate color
            gridClass.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridClass.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridClass.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridClass.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridClass.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void frmRptFormSale_Load(object sender, EventArgs e)
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

        private void frmRptFormSale_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }
      }
  }


