using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using System.Collections;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Attendence.FormReport
{
    public partial class frmIdCardPrint : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmIdCardPrint(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                              Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmIdCardPrint_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
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
                string sqlQuery = "Exec [prcIdcardPrint] " + Common.Classes.clsMain.intComId + ",'','','' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGridFrom";
                dsList.Tables[1].TableName = "tblGridTo";


                gridFrom.DataSource = null;
                gridFrom.DataSource = dsList.Tables["tblGridFrom"];

                gridTo.DataSource = null;
                gridTo.DataSource = dsList.Tables["tblGridTo"];
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

        private void gridFrom_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Grid Width

                gridFrom.DisplayLayout.Bands[0].Columns["empid"].Hidden = true; //Employee ID
                gridFrom.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55; //Short Name
                gridFrom.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100; //Employee code
                gridFrom.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150; //Employee Name

                //Caption
                gridFrom.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridFrom.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridFrom.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Coloumn Styole
                this.gridFrom.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //Stop Updating
                // this.gridFrom.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Column Activation
                gridFrom.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridFrom.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                //Hiding +/- Indicator
                this.gridFrom.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
                e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.DarkCyan;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridTo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Grid Width

                gridTo.DisplayLayout.Bands[0].Columns["empid"].Hidden = true; //Employee ID
                gridTo.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55; //Short Name
                gridTo.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100; //Employee code
                gridTo.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150; //Employee Name

                //Caption
                gridTo.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridTo.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridTo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Coloumn Styole
                this.gridTo.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //Stop Updating
                // this.gridFrom.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Column Activation
                gridTo.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridTo.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                //Hiding +/- Indicator
                this.gridTo.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridFrom.Rows)
            {
                if (row.Cells["isChecked"].Value.ToString() == "1")
                {
                    DataRow dr;
                    dr = dsList.Tables["tblGridTo"].NewRow();
                    dr["eMPID"] = row.Cells["Empid"].Value;
                    dr["eMPCode"] = row.Cells["eMPCode"].Value;
                    dr["EmpName"] = row.Cells["EmpName"].Value;
                    dr["IsChecked"] = 0;

                    dsList.Tables["tblgridTo"].Rows.Add(dr);
                    row.Hidden = true;
                    row.Cells["IsChecked"].Value = 0;
                    //sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOut,OtHour,Status,Remarks,Luserid,comid,pcname) "
                    //    + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["timein"].Text.ToString() + "','" + row.Cells["timeout"].Text.ToString() + "','" + row.Cells["otHour"].Value.ToString() + "','" + row.Cells["Status"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "'," + Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ",'" + Common.Classes.clsMain.strComputerName + "')";

                }
            }
            //for (int i = 0; i < arQuery.Count;i++ )
            //{
            //    gridFrom.Rows[i].Delete(false);
            //}
        }

        private void frmIdCardPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            uTab = null;
            clsProc = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFrom_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;

            prcLoadList();

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridTo.Rows)
            {
                if (row.Cells["isChecked"].Value.ToString() == "1")
                {
                    DataRow dr;
                    dr = dsList.Tables["tblGridFrom"].NewRow();
                    dr["eMPID"] = row.Cells["Empid"].Value;
                    dr["eMPCode"] = row.Cells["eMPCode"].Value;
                    dr["EmpName"] = row.Cells["EmpName"].Value;
                    dr["IsChecked"] = 0;

                    dsList.Tables["tblgridFrom"].Rows.Add(dr);
                    row.Hidden = true;
                    row.Cells["IsChecked"].Value = 0;

                    //sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOut,OtHour,Status,Remarks,Luserid,comid,pcname) "
                    //    + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["timein"].Text.ToString() + "','" + row.Cells["timeout"].Text.ToString() + "','" + row.Cells["otHour"].Value.ToString() + "','" + row.Cells["Status"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "'," + Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ",'" + Common.Classes.clsMain.strComputerName + "')";

                }
                //}
                gridFrom.DisplayLayout.Bands[0].Columns["Empid"].SortIndicator = SortIndicator.Ascending;
            }
        }

        private void btnAllTo_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridFrom.Rows)
            {
                if (row.Cells["isChecked"].Value.ToString() == "0")
                {
                    DataRow dr;
                    dr = dsList.Tables["tblGridTo"].NewRow();
                    dr["eMPID"] = row.Cells["Empid"].Value;
                    dr["eMPCode"] = row.Cells["eMPCode"].Value;
                    dr["EmpName"] = row.Cells["EmpName"].Value;
                    dr["IsChecked"] = 0;

                    dsList.Tables["tblgridTo"].Rows.Add(dr);
                    row.Hidden = true;
                    row.Cells["IsChecked"].Value = 0;
                    //sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOut,OtHour,Status,Remarks,Luserid,comid,pcname) "
                    //    + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["timein"].Text.ToString() + "','" + row.Cells["timeout"].Text.ToString() + "','" + row.Cells["otHour"].Value.ToString() + "','" + row.Cells["Status"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "'," + Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ",'" + Common.Classes.clsMain.strComputerName + "')";

                }
            }
            //for (int i = 0; i < arQuery.Count;i++ )
            //{
            //    gridFrom.Rows[i].Delete(false);
            //}
        }

        private void btnAllFrom_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;

            prcLoadList();

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridTo.Rows)
            {
                if (row.Cells["isChecked"].Value.ToString() == "0")
                {
                    DataRow dr;
                    dr = dsList.Tables["tblGridFrom"].NewRow();
                    dr["eMPID"] = row.Cells["Empid"].Value;
                    dr["eMPCode"] = row.Cells["eMPCode"].Value;
                    dr["EmpName"] = row.Cells["EmpName"].Value;
                    dr["IsChecked"] = 0;

                    dsList.Tables["tblgridFrom"].Rows.Add(dr);
                    row.Hidden = true;
                    row.Cells["IsChecked"].Value = 0;

                    //sqlQuery = " Insert Into tblAttfixed(empid,dtPunchDate,TimeIn,TimeOut,OtHour,Status,Remarks,Luserid,comid,pcname) "
                    //    + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["dtPunchDate"].Text.ToString() + "','" + row.Cells["timein"].Text.ToString() + "','" + row.Cells["timeout"].Text.ToString() + "','" + row.Cells["otHour"].Value.ToString() + "','" + row.Cells["Status"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "'," + Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ",'" + Common.Classes.clsMain.strComputerName + "')";

                }
                //}
                gridFrom.DisplayLayout.Bands[0].Columns["Empid"].SortIndicator = SortIndicator.Ascending;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

         try
          {

              string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

              DataSourceName = "DataSet1";
              FormCaption = "Report :: Employee Information ...";
             
             if (Common.Classes.clsMain.intComId == 2 || Common.Classes.clsMain.intComId == 3 || Common.Classes.clsMain.intComId == 4)
              {

                  string empId = "";
                  foreach (UltraGridRow row in gridTo.Rows)
                  {
                      //  if (row.Cells["empId"].Value.ToString() == "1")
                      //   {
                      empId += row.Cells["empId"].Value + ",";
                      //    }
                  }
                  empId = empId.Substring(0, empId.Length - 1);



                  //string SectId = "0", type = "", EmpId = "0";
                  // SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                  // type = gridEmpStatus.ActiveRow.Cells["EmpStatus"].Value.ToString();
                  // gridSection.ActiveRow.Cells["SectId"].Value.ToString();

                  ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIDCardPrint.rdlc";
                  SQLQuery = "Exec [rptIdCardPrint] " + Common.Classes.clsMain.intComId + ", '" + empId + "' ";

              }

              else
              {

                  string empId = "";
                  foreach (UltraGridRow row in gridTo.Rows)
                  {
                      //  if (row.Cells["empId"].Value.ToString() == "1")
                      //   {
                      empId += row.Cells["empId"].Value + ",";
                      //    }
                  }
                  empId = empId.Substring(0, empId.Length - 1);

                  //string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                  //DataSourceName = "DataSet1";
                  //FormCaption = "Report :: Employee Information ...";


                  ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIDCardPrint.rdlc";
                  SQLQuery = "Exec [rptIdCardPrint] " + Common.Classes.clsMain.intComId + ", '" + empId + "' ";

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
                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = SQLQuery;
                //clsReport.strDSNMain = DataSourceName;

                //FM.prcShowReport(FormCaption);
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

        private void btnJoin_Click(object sender, EventArgs e)
        {
            prcLoadJoinList();

        }

        private void prcLoadJoinList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcIdcardPrint] " + Common.Classes.clsMain.intComId + ",'JoinDate','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGridFrom";
                dsList.Tables[1].TableName = "tblGridTo";


                gridFrom.DataSource = null;
                gridFrom.DataSource = dsList.Tables["tblGridFrom"];

                gridTo.DataSource = null;
                gridTo.DataSource = dsList.Tables["tblGridTo"];

                checkBox2.Checked = true;
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridFrom.Rows)
                {
                    row.Cells["isChecked"].Value = 1;
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridFrom.Rows)
                {
                    row.Cells["isChecked"].Value = 0;
                }
            }
        }



     }
 }
