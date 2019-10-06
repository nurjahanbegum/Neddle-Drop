using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmApplicantList : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmApplicantList(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                                Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridList.DisplayLayout.Bands[0].Columns["AppId"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["DistId"].Hidden = true;
                
                //Grid Width
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 80; //Applicants code
                gridList.DisplayLayout.Bands[0].Columns["App_Code"].Width = 100; //Applicants code
                gridList.DisplayLayout.Bands[0].Columns["AppName"].Width = 140; //Applicants Name

                gridList.DisplayLayout.Bands[0].Columns["dtApp"].Width = 120; //dtApply
                gridList.DisplayLayout.Bands[0].Columns["mobileself"].Width = 130; //Mobile Self
                gridList.DisplayLayout.Bands[0].Columns["AppEmail"].Width = 140; //Applicants Email 
                gridList.DisplayLayout.Bands[0].Columns["DistName"].Width = 140; //District
                gridList.DisplayLayout.Bands[0].Columns["AppType"].Width = 120; //Job Type

                gridList.DisplayLayout.Bands[0].Columns["isPassed"].Width = 80; //Is Passed
                gridList.DisplayLayout.Bands[0].Columns["isWaiting"].Width = 80; //Is Waiting
                gridList.DisplayLayout.Bands[0].Columns["isFaild"].Width = 80; //Is Faild
                gridList.DisplayLayout.Bands[0].Columns["isAppointed"].Width = 80; //Is Appointed
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 100; //dtJoin
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Width = 120; //Remarks
                gridList.DisplayLayout.Bands[0].Columns["Status"].Width = 100; //Remarks

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Header.Caption = "Is Checked";
                gridList.DisplayLayout.Bands[0].Columns["App_Code"].Header.Caption = "Applicants code";
                gridList.DisplayLayout.Bands[0].Columns["AppName"].Header.Caption = "Applicants Name";

                gridList.DisplayLayout.Bands[0].Columns["dtApp"].Header.Caption = "Application Date";
                gridList.DisplayLayout.Bands[0].Columns["mobileself"].Header.Caption = "Mobile";
                gridList.DisplayLayout.Bands[0].Columns["AppEmail"].Header.Caption = "E-Mail";
                gridList.DisplayLayout.Bands[0].Columns["DistName"].Header.Caption = "District";
                gridList.DisplayLayout.Bands[0].Columns["AppType"].Header.Caption = "Job Type";

                gridList.DisplayLayout.Bands[0].Columns["isPassed"].Header.Caption = "Is Passed";
                gridList.DisplayLayout.Bands[0].Columns["isWaiting"].Header.Caption = "Is Waiting";
                gridList.DisplayLayout.Bands[0].Columns["isFaild"].Header.Caption = "Is Faild";
                gridList.DisplayLayout.Bands[0].Columns["isAppointed"].Header.Caption = "Is Appointed";
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";
                gridList.DisplayLayout.Bands[0].Columns["Status"].Header.Caption = "Status";

                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridList.DisplayLayout.Bands[0].Columns["isPassed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridList.DisplayLayout.Bands[0].Columns["isWaiting"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridList.DisplayLayout.Bands[0].Columns["isFaild"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridList.DisplayLayout.Bands[0].Columns["isAppointed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;


                //Select Full Row when click on any cell
               // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

              
                //gridList.Columns["Photo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Image;
                this.gridList.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.DarkCyan;
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;
                //Selection Style Will Be Row Selector
                this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                //this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                // Set the scroll style to immediate so the rows get scrolled immediately
                // when the vertical scrollbar thumb is dragged.
                e.Layout.ScrollStyle = ScrollStyle.Immediate;

                // ScrollBounds of ScrollToFill will prevent the user from scrolling the
                // grid further down once the last row becomes fully visible.
                e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetApplicantsList] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";               

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblgrid"];
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

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmApplicantList_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void frmApplicantList_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            //Int32 NewId = 0;
            try
            {
                foreach (UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["isChecked"].Value.ToString() == "1")
                    {
                        //sqlQuery = " Delete  tblJobApp_Info where AppId = '" + row.Cells["AppId"].Text.ToString() + "' ";
                        //    arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblJobApp_Info Set isPassed = '" + row.Cells["isPassed"].Value.ToString() +
                                   "', isWaiting ='" + row.Cells["isWaiting"].Value.ToString() + "', isFaild = '" +
                                   row.Cells["isFaild"].Value.ToString() + "', isAppointed = '" +
                                   row.Cells["isAppointed"].Value.ToString() + "', dtJoin = '" +
                                   row.Cells["dtJoin"].Value.ToString() + "', Status = '"+row.Cells["Status"].Text.ToString() +"' where AppId = '" +
                                   row.Cells["AppId"].Value.ToString() + "' ";
                        arQuery.Add(sqlQuery);
                    }
                }
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                MessageBox.Show("Data Update Successfully");

                prcLoadList();
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

        private void gridList_CellChange(object sender, CellEventArgs e)
        {
            //gridList.ActiveRow.Cells["isFaild"].Value = 1;
            //if (gridList.ActiveRow.Cells["isFaild"].Value.ToString() == "1")
            //{
            //    gridList.ActiveRow.Cells["isFaild"].Value = 0;
            //    gridList.ActiveRow.Cells["isPassed"].Value = 1;
            //    gridList.ActiveRow.Cells["dtJoin"].Value = DateTime.Today.ToString();
            //}
            //else
            //{
            //    gridList.ActiveRow.Cells["isFaild"].Value = 1;
            //}
            //gridList.ActiveRow.Cells["isFaild"].Value = 1;
            //if (gridList.ActiveRow.Cells["isFaild"].Value.ToString() == "1")
            //{
            //    gridList.ActiveRow.Cells["isPassed"].Value = 0;
            //}
            //else
            //{
            //    gridList.ActiveRow.Cells["isPassed"].Value = 1;
            //}

        }

        private void gridList_ClickCell(object sender, ClickCellEventArgs e)
        {
           // gridList.ActiveRow.Cells["isFaild"].Value = 1;
            if (gridList.ActiveRow.Cells["isFaild"].Value.ToString() == "1")
            {
                gridList.ActiveRow.Cells["isFaild"].Value = 0;
                gridList.ActiveRow.Cells["isWaiting"].Value = 0;
                gridList.ActiveRow.Cells["isAppointed"].Value = 0;

            }
            else
            {
                gridList.ActiveRow.Cells["isPassed"].Value = 0;
                gridList.ActiveRow.Cells["isWaiting"].Value = 0;
                gridList.ActiveRow.Cells["isAppointed"].Value = 0;
            }

            //gridList.ActiveRow.Cells["isPassed"].Value = 1;
            if (gridList.ActiveRow.Cells["isAppointed"].Value.ToString() == "0")
            {
                //gridList.ActiveRow.Cells["isAppointed"].Value = 1;
                        gridList.ActiveRow.Cells["isPassed"].Value = 0;
                        gridList.ActiveRow.Cells["isFaild"].Value = 0;
                        gridList.ActiveRow.Cells["isWaiting"].Value = 0;
                        gridList.ActiveRow.Cells["dtJoin"].Value = DateTime.Today.ToString();

                    }
                    else
                    {
                        //gridList.ActiveRow.Cells["isPassed"].Value = 1;
                    }
                }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string AppId = "";
                foreach (UltraGridRow row in gridList.Rows)
                {
                    //  if (row.Cells["empId"].Value.ToString() == "1")
                    //   {
                    AppId += row.Cells["AppId"].Value + ",";
                    //    }
                }
                AppId = AppId.Substring(0, AppId.Length - 1);

                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Employee Information ...";

                //string SectId = "0", type = "", EmpId = "0";
                // SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                // type = gridEmpStatus.ActiveRow.Cells["EmpStatus"].Value.ToString();
                // gridSection.ActiveRow.Cells["SectId"].Value.ToString();

                ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAppointment.rdlc";
                SQLQuery = "Exec [rptAppointment] " + Common.Classes.clsMain.intComId + ", '" + AppId + "' ";

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrevEng_Click(object sender, EventArgs e)
        {
            try
            {
                string AppId = "";
                foreach (UltraGridRow row in gridList.Rows)
                {
                    //  if (row.Cells["empId"].Value.ToString() == "1")
                    //   {
                    AppId += row.Cells["AppId"].Value + ",";
                    //    }
                }
                AppId = AppId.Substring(0, AppId.Length - 1);

                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Employee Information ...";

                //string SectId = "0", type = "", EmpId = "0";
                // SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                // type = gridEmpStatus.ActiveRow.Cells["EmpStatus"].Value.ToString();
                // gridSection.ActiveRow.Cells["SectId"].Value.ToString();

                ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAppointENG.rdlc";
                SQLQuery = "Exec [rptAppointment] " + Common.Classes.clsMain.intComId + ", '" + AppId + "' ";

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


      