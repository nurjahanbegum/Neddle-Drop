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
using System.Data.OleDb;
using Infragistics.Win.UltraWinGrid;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmTiffin : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private DataView dvStyle;
        private DataView dvSpec;
        private DataView dvColor;

        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmTiffin(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmTiffin_Load(object sender, EventArgs e)
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
                sqlQuery = "Exec prcGetTiffin " + Common.Classes.clsMain.intComId + ", 0, 0,'','','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblBand";
                dsList.Tables[3].TableName = "tblEmp";


                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];

                this.dtFrom.Value = DateTime.Now;

                if (dtFrom.DateTime.Month == 1)
                {
                    if (dtFrom.DateTime.Day <= 6)
                    {

                        var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                        var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);
                        dtFrom.Value = lastDay;
                    }
                    else
                    {

                        DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        lastDay = lastDay.AddMonths(1);
                        lastDay = lastDay.AddDays(-(lastDay.Day));
                        dtFrom.Value = lastDay;
                    }
                }

                else
                {

                    if (dtFrom.DateTime.Day <= 6)
                    {
                        var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month - 1);
                        var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month - 1, DaysInMonth);
                        dtFrom.Value = lastDay;
                    }

                    else
                    {
                        DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        lastDay = lastDay.AddMonths(1);
                        lastDay = lastDay.AddDays(-(lastDay.Day));
                        dtFrom.Value = lastDay;
                    }

                }


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
            cboSec.Text = "";
            cboBand.Text = "";
            cboEmp.Text = "";

            checkBox2.Checked = false;

            this.dtFrom.Value = DateTime.Now;

            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtFrom.Value = firstDay;

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

        private void frmTiffin_FormClosing(object sender, FormClosingEventArgs e)
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
                gridDetails.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Department";
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Amount";


                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Width = 135;
                gridDetails.DisplayLayout.Bands[0].Columns["DeptName"].Width = 140;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Width = 140;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 95;
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Width = 85;


                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["DeptName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

//                gridDetails.DisplayLayout.Bands[0].Columns["Grade"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;

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
                sqlQuery = "Exec prcGetTiffin " + Common.Classes.clsMain.intComId + ", " + EmpId + "," + SectId + ",'" + Band + "','" + optCriteria.Value.ToString() + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];

  //              gridDetails.DisplayLayout.Bands[0].Columns["Grade"].ValueList = uddBand;

                //checkBox2.Checked = true;


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

            string Description = "";
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dtFrom.DateTime.Month).ToString();
            Description = strMonthName + "-" + (dtFrom.DateTime.Year);


            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            
            string sqlQuery = "";
            Int32 NewId = 0;
            Int32 RowID;

            try
            {
                //Member Master Table
                if (btnSave.Text.ToString() == "&Save")
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Delete  tblTiffin where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and ComID = " + Common.Classes.clsMain.intComId + " and ProssType = '" + Description + "'";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblTiffin (EmpID,Band,Amount,ProssType,dtDate,ComID) "
                                       + " Values ('" + row.Cells["Empid"].Text.ToString() + "', '" +
                                       row.Cells["Band"].Text.ToString() + "','" +
                                       row.Cells["Amount"].Value.ToString() + "','" + Description + "','" +
                                       clsProc.GTRDate(dtFrom.Value.ToString()) + "'," + Common.Classes.clsMain.intComId + ")";
                            arQuery.Add(sqlQuery);


                            sqlQuery = "Update A Set A.SectID = E.SectID,A.DesigID = E.DesigID, A.DeptID = E.DeptID"
                                       + " from tblTiffin A,tblEmp_info E Where A.EmpID = E.EmpID and A.ComID = " + Common.Classes.clsMain.intComId
                                       + " and A.ProssType = '" + Description + "'";
                            arQuery.Add(sqlQuery);

                        }
                    }




                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully.");
                }
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
                for (rowCount = 0; rowCount < dsList.Tables["Grid"].Rows.Count; rowCount++)
                {

                  gridDetails.Rows[rowCount].Cells[8].Value = txtAmount.Text.ToString();

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


        //private void uddBand_RowSelected(object sender, RowSelectedEventArgs e)
        //{

        //    if (uddBand.ActiveRow == null)
        //    {
        //        return;
        //    }

        //    gridDetails.ActiveRow.Cells["Grade"].Value = uddBand.ActiveRow.Cells["Grade"].Value.ToString();

        //}

        //private void uddBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    uddBand.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
        //    uddBand.DisplayMember = "Grade";
        //    uddBand.ValueMember = "Grade";

        //}

        //private void cboGrade_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Width = cboGrade.Width;
        //    cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
        //    cboGrade.DisplayMember = "Grade";
        //    cboGrade.ValueMember = "Grade";
        //}

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            String input = string.Empty;
            String input2 = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select an excel file ";
            dialog.Filter = "Excel files [97-2003] (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            //dialog.InitialDirectory = @"C:\";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                input = dialog.FileName.ToString();
                input2 = dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1);
            }
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            txtFileName.Text = input;
            txtFileName.Tag = input2;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text.Length == 0)
            {
                MessageBox.Show("Please select an excel file, using browse button");
                btnBrowse.Focus();
                return;
            }

            DateTime lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtFrom.Value = lastDay;

            Cursor.Current = Cursors.WaitCursor;
            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + txtFileName.Text.ToString() + "; Extended Properties='Excel 8.0; HDR=Yes; IMEX=1'";
            try
            {
                var da = new OleDbDataAdapter("SELECT * FROM [Tiffin$]", SourceConstr);
                var ds = new DataSet();
                da.Fill(ds);

                btnImport.Text = "Pls Wait";

                prcSaveData(ds);

                MessageBox.Show("Data uploaded successfully. [Total Rows : " + ds.Tables[0].Rows.Count.ToString() + "]");
                //btnProcess.Enabled = true;
                btnImport.Text = "&2. Import";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void prcSaveData(DataSet ds)
        {
            clsConnection clsCon = new clsConnection();
            ArrayList arQuery = new ArrayList();
            string sqlQuery = "";


            DateTime lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtFrom.Value = lastDay;

            try
            {
                // Clear Existing Data
                string query = "Truncate Table tblTempGrade";
                clsCon.GTRSaveDataWithSQLCommand(query);

                txtProcessNo.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
                string dtProcess = DateTime.Today.ToString("dd-MMM-yyyy");

                //Generating Insert Statement Row By Row
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString().Replace("'", "").Length > 0)
                    {

                        sqlQuery = "Insert Into tblTempGrade (ComID, aID,EmpID,Amount, dtDate) " +
                        " Values(" + Common.Classes.clsMain.intComId + ",'" + ds.Tables[0].Rows[i][0].ToString().Replace("'", "") + "','" + ds.Tables[0].Rows[i][1].ToString().Replace("'", "") + "','" + ds.Tables[0].Rows[i][2].ToString().Replace("'", "") + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "')";
                        arQuery.Add(sqlQuery);
                        
                     
                        
                        //sqlQuery = "Insert Into tblTempCount (ComId, xlsFileName, dtProcess, EntryNo, " +
                        //" empid,dtPunchDate,TimeIn,TimeOut,OTHour,Status,Remarks,PCName,LUserId) " +
                        //" Values(" + Common.Classes.clsMain.intComId + "," +
                        //" '" + txtFileName.Tag.ToString() + "', " +
                        //" '" + dtProcess + "'," +
                        //" '" + txtProcessNo.Text.ToString() + "'," +

                        //" '" + ds.Tables[0].Rows[i][0].ToString().Replace("'", "") + "', " +
                        //" '" + clsProc.GTRDate(ds.Tables[0].Rows[i][1].ToString().Replace("'", "")) + "', " +
                        //" '" + clsProc.GTRTime(ds.Tables[0].Rows[i][2].ToString().Replace("'", "")) + "', " +
                        //" '" + clsProc.GTRTime(ds.Tables[0].Rows[i][3].ToString().Replace("'", "")) + "', " +
                        //" '" + clsProc.GTRTime(ds.Tables[0].Rows[i][4].ToString().Replace("'", "")) + "', " +
                        //" '" + ds.Tables[0].Rows[i][5].ToString().Replace("'", "") + "', " +
                        //" '" + ds.Tables[0].Rows[i][14].ToString().Replace("'", "") + "', " +
                        //" '" + Common.Classes.clsMain.strComputerName + "', " +
                        //    // " '" + ds.Tables[0].Rows[i][5].ToString().Replace("'", "").Replace("NA", "0").Replace("N/A", "0") + "', '" +
                        //" '" + Common.Classes.clsMain.intUserId + "')";

                        //arQuery.Add(sqlQuery);
                    }
                    else
                        break;
                }


                sqlQuery = " exec  [prcProcessTiffin] " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' ";
                arQuery.Add(sqlQuery);

                //Transaction with database server
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Tiffin amount this Employee?", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }


            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;
            try
            {
                if (btnDelete.Text.ToString() == " &Delete")
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {
                            //RowID = row.Index + 1;
                            ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                            sqlQuery = " Delete  tblTiffin where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtDate =  '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' and ComID = " + Common.Classes.clsMain.intComId + "";
                            arQuery.Add(sqlQuery);

                        }
                    }

                }
                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Delete SuccessFully");

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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string Description = "";

                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(dtFrom.DateTime.Month).ToString();


                Description = strMonthName + "-" + (dtFrom.DateTime.Year);

                
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "";
                Band = "=ALL=";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Tiffin...";

                ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptTiffin.rdlc";
                SQLQuery = "Exec [rptTiffinSheet] " + Common.Classes.clsMain.intComId + ", '" + Description + "','0','0','" + Band + "',1";


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

            string Description = "";

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dtFrom.DateTime.Month).ToString();


            Description = strMonthName + "-" + (dtFrom.DateTime.Year);
            
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            string Band = "";

            Band = "=ALL=";



            String sqlquary = "Exec [rptTiffinSheet] " + Common.Classes.clsMain.intComId + ", '" + Description + "',0,0,'" + Band + "',1";
            clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

            dsList.Tables[0].TableName = "Tiffin";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["Tiffin"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Tiffin List" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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
            GridToToExcel.Export(gridExcel, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }







    }
}
