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

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmLoan : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmLoan(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                       Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmLoan_Load(object sender, EventArgs e)
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

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetLoan] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";
                dsList.Tables[1].TableName = "tblEmployee";
                dsList.Tables[2].TableName = "tblgridAdd";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];

                gridData.DataSource = null;
                gridData.DataSource = dsList.Tables["tblgridAdd"];

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtFrom.Value = firstDay;

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

        private void prcLoanGenerate()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetLoan] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                
                dsList.Tables[2].TableName = "tblgridAdd";

                gridData.DataSource = null;
                gridData.DataSource = dsList.Tables["tblgridAdd"];

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

        private void prcLoadCombo()
        {
            cboEmpID.DataSource = null;
            cboEmpID.DataSource = dsList.Tables["tblEmployee"];


            //cboEmpType.Text = "Worker";
            //cboWeekDay.Value = 6;

        }

        private void prcDisplayDetails(String strParam)
        {
            dsDetails = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec [prcGetLoan] " + Common.Classes.clsMain.intComId + " , " +
                                  Int32.Parse(strParam) + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";
                dsDetails.Tables[1].TableName = "tblgridAdd";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];

                    this.txtId.Text = dr["loanid"].ToString();
                    this.cboEmpID.Text = dr["EmpCode"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtSec.Text = dr["SectName"].ToString();
                    this.txtDesig.Text = dr["DesigName"].ToString();
                    this.dtFrom.Text = dr["dtFrom"].ToString();
                    this.dtTo.Text = dr["dtTo"].ToString();
                    this.txtAmt.Text = dr["Amount"].ToString();
                    this.txtInstallNo.Text = dr["InstNo"].ToString();
                    this.txtRate.Text = dr["Rate"].ToString();
                    this.txtInstallAmnt.Text = dr["InsAmount"].ToString();
                    this.txtOpBalance.Text = dr["OpBalance"].ToString();
                    this.txtClosingBalance.Text = dr["ClosingBalance"].ToString();
                    this.txtPaidAmnt.Text = dr["PaidAmount"].ToString();



                    gridData.DataSource = null;
                    gridData.DataSource = dsDetails.Tables["tblgridAdd"];

                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;

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

        private void prcClearData()
        {
            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtFrom.Value = firstDay;
            //dtTo.Value = DateTime.Now;
            cboEmpID.Text = "";
            txtName.Text = "";
            txtDesig.Text = "";
            txtSec.Text = "";
            txtAmt.Text = "";
            txtRate.Text = "";
            txtInstallNo.Text = "";
            txtInstallAmnt.Text = "";
            gridData.DataSource = null;
            gridData.DataSource = dsList.Tables["tblgridAdd"];

            this.txtPaidAmnt.Text = "0";
            this.txtOpBalance.Text = "0";
            this.txtClosingBalance.Text = "0";



            btnSave.Text = "&Save";
            btnDelete.Enabled = false;

        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Hidden Coloumn
                gridList.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true; //Employee ID
                gridList.DisplayLayout.Bands[0].Columns["loanid"].Hidden = true; //Employee ID
                //Grid Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80; //Employee code
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 110; //Employee Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 100; //Section
                gridList.DisplayLayout.Bands[0].Columns["Amount"].Width = 80; //Loan Amount 
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 100; //Designation 
                gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Width = 70; //From Date
                gridList.DisplayLayout.Bands[0].Columns["dtto"].Width = 70; //To Date

                //Caption

                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Amount";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Header.Caption = "From Date";
                gridList.DisplayLayout.Bands[0].Columns["dtto"].Header.Caption = "To Date";
                
                //date Formate
                gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Format = "dd-MMM-yyyy";
                gridList.DisplayLayout.Bands[0].Columns["dtto"].Format = "dd-MMM-yyyy";

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLoan_FormClosing(object sender, FormClosingEventArgs e)
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


        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridList.ActiveRow.IsFilterRow == false)
                {
                    prcClearData();

                    // prcLoadList();
                    //prcLoadCombo();
                    prcDisplayDetails(gridList.ActiveRow.Cells["loanid"].Value.ToString());
                    //cboEmpID.Text = "";

                    txtAmt.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboEmpID_ValueChanged(object sender, EventArgs e)
        {
            //if (this.cboEmpID.IsItemInList() == false)
            //{
            //    //MessageBox.Show("Please Provide valid data [or, select from list].");
            //    //cboEmpID.Focus();
            //    prcClearData();
            //    prcLoadCombo();
            //    return;
            //}


            //if (cboEmpID.Value == null)
            //{
            //    return;
            //}
            //prcDisplayDetails(cboEmpID.Value.ToString());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
            prcLoadList();
            cboEmpID.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;
            try
            {

                //Member Master Table
                if (btnSave.Text != "&Save")
                {

                    //sqlQuery = " Delete From tblLoan_Data  Where aid = " + Int32.Parse(txtId.Text.ToString()) + " ";
                    //arQuery.Add(sqlQuery);


                    //Update
                    sqlQuery = "Update tblloan set inputdate = '" + clsProc.GTRDate(this.dtInput.Value.ToString()) +
                               "', dtLoanFrom ='" + clsProc.GTRDate(this.dtFrom.Value.ToString()) + "', dtLoanTo ='" +
                               clsProc.GTRDate(this.dtTo.Value.ToString()) + "', Amount='"+ txtAmt.Text.ToString() +"', InstNo = '"+
                               txtInstallNo.Text.ToString() + "', Rate = '" + txtRate.Text.ToString() + "',InsAmount ='" + txtInstallAmnt.Text.ToString() + "',OpBalance = '" +
                               txtOpBalance.Text.ToString() + "',paidamount = '" + txtPaidAmnt.Text.ToString() + "',ClosingBalance = '" + txtClosingBalance.Text.ToString() + "', Remarks = '" +
                               txtRemarks.Text.ToString() + "',LUserId = " + GTRHRIS.Common.Classes.clsMain.intUserId + ",PCName ='" + 
                               Common.Classes.clsMain.strComputerName + "'  where loanid = '" + this.txtId.Text.ToString() + "' ";
                    arQuery.Add(sqlQuery);
                    fncGridDataUpdate(ref arQuery,txtId.Text.ToString());

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database  
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //NewId
                    sqlQuery = "Select Isnull(Max(loanId),0)+1 As NewId from tblloan";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert Data
                    //sqlQuery = " Delete  tblloan where aid = '" + cboEmpID.Value.ToString() + "' ";
                    //arQuery.Add(sqlQuery);

                    sqlQuery = " Insert Into tblloan(ComId,empid, empCode, empname, LoanID, inputdate,dtLoanFrom, dtLoanTo, Amount, InstNo, Rate, InsAmount,OpBalance,paidamount,ClosingBalance,Remarks,LUserId,PCName) "
                    + " Values ('" + Common.Classes.clsMain.intComId + "', '" + cboEmpID.Value.ToString() + "', '"+cboEmpID.Text.ToString()+"', '"+
                    txtName.Text.ToString() + "', '" + NewId + "', '" + clsProc.GTRDate(dtInput.Value.ToString()) + "','" +
                    clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + txtAmt.Text.ToString() + "', '" +
                    txtInstallNo.Text.ToString() + "','" + txtRate.Text.ToString() + "','" + txtInstallAmnt.Text.ToString() +
                    "','" + txtOpBalance.Text.ToString() + "', '" + txtPaidAmnt.Text.ToString() +
                    "','" + txtClosingBalance.Text.ToString() + "', '" + txtRemarks.Text.ToString() +
                    "'," + GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + Common.Classes.clsMain.strComputerName + "') ";
                    arQuery.Add(sqlQuery);

                    fncGridData(ref arQuery, NewId.ToString());

                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                 + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                 "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);
                    
                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                }
                prcClearData();
                prcLoadList();
                prcLoadCombo();

                cboEmpID.Focus();

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
        private void fncGridData(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";

            foreach (UltraGridRow row in this.gridData.Rows)
            {
                sqlQuery = "Insert Into tblLoan_Data (ComId, EmpId, LoanId,InstallmentNo,LoanAmt,instAmt,Balance,InterestAmt,FlatAmt,DeductAmt,dtFrom,dtTo,aId,Rate)" +
                           " Values ('" + Common.Classes.clsMain.intComId + "', '" + cboEmpID.Value.ToString() + "', " + Int32.Parse(strId) + ",'" + row.Cells["InstallmentNo"].Text.ToString() + 
                           "','" + row.Cells["LoanAmt"].Text.ToString() + "','" + row.Cells["instAmt"].Text.ToString() + 
                           "','" + row.Cells["Balance"].Text.ToString() + "','" + row.Cells["InterestAmt"].Text.ToString() + 
                           "','" + row.Cells["FlatAmt"].Text.ToString() + "','" + row.Cells["DeductAmt"].Text.ToString() +
                           "','" + row.Cells["dtFrom"].Text.ToString() + "','" + row.Cells["dtTo"].Text.ToString() +
                           "'," + Int32.Parse(strId) + "," + txtRate.Value.ToString() + ")";
                arQuery.Add(sqlQuery);

            }
        }

        private void fncGridDataUpdate(ref ArrayList arQuery, string strId)
        {
            string sqlQuery = "";

            foreach (UltraGridRow row in this.gridData.Rows)
            {


                sqlQuery = "Update tblLoan_Data Set dtFrom = '" + row.Cells["dtFrom"].Text.ToString() + "', dtTo = '" + row.Cells["dtTo"].Text.ToString() +
                           "', FlatAmt = '" + row.Cells["FlatAmt"].Text.ToString() + 
                           "', DeductAmt = '" + row.Cells["DeductAmt"].Text.ToString() + 
                           "' Where aId = " + Int32.Parse(strId) + " and InstallmentNo = '" + row.Cells["InstallmentNo"].Text.ToString() + "'";
                arQuery.Add(sqlQuery);

            }
        }
     

        private void dtInputDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void dtTo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtDesig_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtAmt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtInstallNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtInstallAmnt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtPaidAmnt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void btnAdd_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void gridData_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void btnDelete_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }


        private void gridData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
            //Grid Width
            gridData.DisplayLayout.Bands[0].Columns["dtFrom"].Width = 90; //Employee code
            gridData.DisplayLayout.Bands[0].Columns["dtTo"].Width = 90; //Employee Name
            gridData.DisplayLayout.Bands[0].Columns["InstallmentNo"].Width = 80; //Section
            gridData.DisplayLayout.Bands[0].Columns["LoanAmt"].Width = 80; //Loan Amount
            gridData.DisplayLayout.Bands[0].Columns["instAmt"].Width = 90; //Install. Amount 
            gridData.DisplayLayout.Bands[0].Columns["Balance"].Width = 90; //Loan Balance 
            gridData.DisplayLayout.Bands[0].Columns["InterestAmt"].Width = 90; //Interest Amount
            gridData.DisplayLayout.Bands[0].Columns["FlatAmt"].Width = 80; //Flat Interest Amount
            gridData.DisplayLayout.Bands[0].Columns["DeductAmt"].Width = 80; //Interest Amount
            //Caption

            gridData.DisplayLayout.Bands[0].Columns["dtFrom"].Header.Caption = "From Date";
            gridData.DisplayLayout.Bands[0].Columns["dtTo"].Header.Caption = "To Date";
            gridData.DisplayLayout.Bands[0].Columns["InstallmentNo"].Header.Caption = "Install. No";
            gridData.DisplayLayout.Bands[0].Columns["LoanAmt"].Header.Caption = "Loan Amount";
            gridData.DisplayLayout.Bands[0].Columns["instAmt"].Header.Caption = "Install. Amount";
            gridData.DisplayLayout.Bands[0].Columns["Balance"].Header.Caption = "Loan Balance";
            gridData.DisplayLayout.Bands[0].Columns["InterestAmt"].Header.Caption = "Interest Amt";
            gridData.DisplayLayout.Bands[0].Columns["FlatAmt"].Header.Caption = "Flat Interest";
            gridData.DisplayLayout.Bands[0].Columns["DeductAmt"].Header.Caption = "Deduct Amt";
            
            //Date formate
            gridData.DisplayLayout.Bands[0].Columns["dtFrom"].Format = "dd-MMM-yyyy";
            gridData.DisplayLayout.Bands[0].Columns["dtTo"].Format = "dd-MMM-yyyy";

            //Select Full Row when click on any cell
            //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            //this.gridData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridData.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

            //Hiding +/- Indicator
            this.gridData.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Using Filter
                // e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         }

        private void txtInstallNo_Leave(object sender, EventArgs e)
        {

        }

        //private void txtInstallNo_ValueChanged(object sender, EventArgs e)
        //{
        //    if (txtAmt.Text.Length == 0)
        //    {
        //        txtAmt.Value = 0;
        //        txtInstallNo.Value = 0;
        //        txtInstallAmnt.Value = 0;
        //    }

        //    //if (double.Parse(txtAmt.Value.ToString()) > 0 && double.Parse(txtRate.Value.ToString()) >= 0)
        //    //{
        //    //    txtInstallAmnt.Value = ((double.Parse(txtAmt.Value.ToString())) / (double.Parse(txtInstallNo.Value.ToString())));
        //    //    txtFlat.Value = ((((double.Parse(txtAmt.Value.ToString())) * (double.Parse(txtRate.Value.ToString()))) / 100) / (double.Parse(txtInstallNo.Value.ToString())));
        //    //}

        //    if (double.Parse(txtAmt.Value.ToString()) > 0)
        //    {
        //        txtInstallAmnt.Value = ((double.Parse(txtAmt.Value.ToString())) / (double.Parse(txtInstallNo.Value.ToString())));
        //    }

        //    if (double.Parse(txtRate.Value.ToString()) >= 0)
        //    {
        //        txtInstallAmnt.Value = ((double.Parse(txtAmt.Value.ToString())) / (double.Parse(txtInstallNo.Value.ToString())));
        //        txtFlat.Value = ((((double.Parse(txtAmt.Value.ToString())) * (double.Parse(txtRate.Value.ToString()))) / 100) / (double.Parse(txtInstallNo.Value.ToString())));
        //    }
        //}

        private void btnDelete_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("Do you want to delete Loan for selected Employee.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            
            
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            try
            {

            sqlQuery = " Delete From tblLoan  Where loanid = " + Int32.Parse(txtId.Text.ToString()) + " ";
            arQuery.Add(sqlQuery);

            sqlQuery = " Delete From tblLoan_Data  Where loanid = " + Int32.Parse(txtId.Text.ToString()) + " ";
            arQuery.Add(sqlQuery);

            
            // Insert Information To Log File
            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
            arQuery.Add(sqlQuery);

            //Transaction with database  
            clsCon.GTRSaveDataWithSQLCommand(arQuery);

            MessageBox.Show("Data Delete Successfully");

            prcClearData();
            prcLoadList();
            prcLoadCombo();

            cboEmpID.Focus();

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

        private void txtPaidAmnt_Leave(object sender, EventArgs e)
        {
            if (txtPaidAmnt.Text.Length > 0 && int.Parse(txtOpBalance.Value.ToString()) == 0 )
            {
                txtOpBalance.Text = (double.Parse(txtAmt.Value.ToString()) - double.Parse(txtPaidAmnt.Value.ToString())).ToString();
            
            }
        }

        private void cboEmpID_Leave(object sender, EventArgs e)
        {
            if (this.cboEmpID.IsItemInList() == false)
            {
                //MessageBox.Show("Please Provide valid data [or, select from list].");
                //cboEmpID.Focus();
                prcClearData();
                prcLoadCombo();
                return;
            }


            if (cboEmpID.Value == null)
            {
                return;
            }
           // prcDisplayDetails(cboEmpID.Value.ToString());
        }

        private void cboEmpID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpID.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["SectName"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["DesigName"].Hidden = true;
            //Width
            cboEmpID.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 95;
            cboEmpID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150;

            cboEmpID.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
            cboEmpID.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            cboEmpID.DisplayMember = "EmpCode";
            cboEmpID.ValueMember = "empId";
        }

        private void cboEmpID_RowSelected(object sender, RowSelectedEventArgs e)
        {
            try
            {
                if (cboEmpID.Value != null)
                {

                    //

                    txtName.Text = cboEmpID.ActiveRow.Cells["empName"].Value.ToString();
                    txtDesig.Text = cboEmpID.ActiveRow.Cells["DesigName"].Value.ToString();
                    txtSec.Text = cboEmpID.ActiveRow.Cells["sectName"].Value.ToString();
                    //cboEmpID.Tag = cboEmpID.ActiveRow.Cells["id"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            prcLoanGenerate();

            Double LoanAmt = Convert.ToInt64(double.Parse(txtAmt.Value.ToString()));
            Double installAmt = Convert.ToInt64((double.Parse(txtAmt.Value.ToString())) / (double.Parse(txtInstallNo.Value.ToString())));
            Double a = int.Parse(txtInstallNo.Value.ToString());
            Double T = int.Parse(txtAmt.Value.ToString());
            Double D = installAmt;
            Double R = 0;
            Int64 F = 0;
            Double AD = (installAmt * a) - LoanAmt;
            
            if (txtInstallNo.Text.Length > 0 && int.Parse(txtInstallNo.Value.ToString()) > 0)
            {

                for (int i = 0; (i) < a; (i)++)
                {
                    {
                        DataRow dr;
                        if ((btnSave.Text == "&Save") || (btnSave.Text == "&Update"))
                        {
                            dr = dsList.Tables["tblgridAdd"].NewRow();
                            dsList.Tables["tblgridAdd"].Rows.Add(dr);
                        }
                        else
                        {
                            dr = dsDetails.Tables["tblgridAdd"].NewRow();

                            dsDetails.Tables["tblgridAdd"].Rows.Add(dr);
                        }
                    }

                    gridData.Rows[i].Cells["InstallmentNo"].Value = i + 1;

                    DateTime Y = dtFrom.DateTime.AddMonths(i);
                    gridData.Rows[i].Cells["dtFrom"].Value = Y;

                    DateTime dtlast = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, 1);
                    dtlast = dtlast.AddMonths(1);
                    dtlast = dtlast.AddDays(-(dtlast.Day));
                    dtTo.DateTime = dtlast;

                    DateTime Z = dtTo.DateTime.AddMonths(i);
                    gridData.Rows[i].Cells["dtTo"].Value = Z;


                    //Loan Process
                    gridData.Rows[i].Cells["LoanAmt"].Value = LoanAmt;
                    gridData.Rows[i].Cells["instAmt"].Value = installAmt;

                    if (T > 0 && i == 0)
                    {
                        gridData.Rows[i].Cells["Balance"].Value = T;

                        R = (((T * 12) / 100) / 12);
                        R = Math.Round(R, 0, MidpointRounding.AwayFromZero);
                        gridData.Rows[i].Cells["InterestAmt"].Value = R;

                        F = Convert.ToInt64 (R);

                    }
                    else
                    {


                        if (AD > 0 && i == (a - 1))
                        {

                            T = (T - D) - AD;
                            gridData.Rows[i].Cells["Balance"].Value = T;

                            R = (((T * 12) / 100) / 12);
                            R = Math.Round(R, 0, MidpointRounding.AwayFromZero);
                            gridData.Rows[i].Cells["InterestAmt"].Value = R;


                            F = Convert.ToInt64(F + R);

                            Double M = installAmt - AD;
                            gridData.Rows[i].Cells["instAmt"].Value = M;
                        }

                        else
                        {
                            T = T - D;
                            gridData.Rows[i].Cells["Balance"].Value = T;

                            R = (((T * 12) / 100) / 12);
                            R = Math.Round(R, 0, MidpointRounding.AwayFromZero);
                            


                            gridData.Rows[i].Cells["InterestAmt"].Value = R;

                            F = Convert.ToInt64(F + R);
                        }

                        
                    }



                }


            }


            F = Convert.ToInt64(F / a);

            Double TtlDeductAmt = installAmt + F;
            txtInstallAmnt.Value = TtlDeductAmt;

            //gridData.Rows[i].Cells["FlatAmt"].Value = F;


            if (txtInstallNo.Text.Length > 0 && int.Parse(txtInstallNo.Value.ToString()) > 0)
            {

                for (int i = 0; (i) < a; (i)++)
                {

                    if (AD > 0 && i == (a - 1))
                    {
                        gridData.Rows[i].Cells["FlatAmt"].Value = F;

                        TtlDeductAmt = TtlDeductAmt - AD;
                        gridData.Rows[i].Cells["DeductAmt"].Value = TtlDeductAmt;
                    }
                    else
                    {
                        gridData.Rows[i].Cells["FlatAmt"].Value = F;
                        gridData.Rows[i].Cells["DeductAmt"].Value = TtlDeductAmt;
                    }
                }
            }

            //foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridData.Rows)
            //{


            //    //row.Cells["instAmt"].Value = txtInstallAmnt.Value.ToString();
            //    row.Cells["FlatAmt"].Value = F;
            //    row.Cells["DeductAmt"].Value = TtlDeductAmt;
            //    //row.Cells["Late"].Value = txtLate.Value.ToString();
            //    //row.Cells["WDay"].Value = txtWHDay.Text.ToString();
            //    //row.Cells["HDay"].Value = txtGHDay.Value.ToString();
            //    // }
            //} 
            //---


            int x = int.Parse(txtInstallNo.Text.ToString().Trim());
            dtTo.DateTime = dtFrom.DateTime.AddMonths(x).AddMonths(-1);

            DateTime lastDay = new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtTo.DateTime = lastDay;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr;
                if (btnSave.Text == "&Save")
                {
                    int instNo = int.Parse(gridData.Rows[gridData.Rows.Count - 1].Cells["InstallmentNo"].Text.ToString()) + 1;
                    dr = dsList.Tables["tblgridAdd"].NewRow();

                    DateTime dt = new DateTime();
                    DateTime dtTo = new DateTime();



                    dt = DateTime.Parse(gridData.Rows[gridData.Rows.Count - 1].Cells["dtFrom"].Value.ToString());
                    //dr = dsDetails.Tables["tblgridAdd"].NewRow();
                    dr["dtFrom"] = dt.AddMonths(1);

                    //To Date
                    dtTo = DateTime.Parse(gridData.Rows[gridData.Rows.Count - 1].Cells["dtTo"].Value.ToString());

                    DateTime dtlast = new DateTime(dtTo.Year, dtTo.Month, 1);
                    dtlast = dtlast.AddMonths(2);
                    dtlast = dtlast.AddDays(-(dtlast.Day));
                    dtTo = dtlast;

                    dr["dtTo"] = dtTo;
                    //dsDetails.Tables["tblgridAdd"].Rows.Add(dr);

                    //InstallmentNo And Amount
                    dr["InstallmentNo"] = instNo;
                    dr["instAmt"] = 0;

                    dsList.Tables["tblgridAdd"].Rows.Add(dr);
                }
                else
                {
                    DateTime dt = new DateTime();
                    DateTime dtTo = new DateTime();

                    int instNo = int.Parse(gridData.Rows[gridData.Rows.Count - 1].Cells["InstallmentNo"].Value.ToString()) + 1;

                    dt = DateTime.Parse(gridData.Rows[gridData.Rows.Count - 1].Cells["dtFrom"].Value.ToString());
                    dr = dsDetails.Tables["tblgridAdd"].NewRow();
                    dr["dtFrom"] = dt.AddMonths(1);

                    //To Date
                    dtTo = DateTime.Parse(gridData.Rows[gridData.Rows.Count - 1].Cells["dtTo"].Value.ToString());

                    DateTime dtlast = new DateTime(dtTo.Year, dtTo.Month, 1);
                    dtlast = dtlast.AddMonths(2);
                    dtlast = dtlast.AddDays(-(dtlast.Day));
                    dtTo = dtlast;

                    dr["dtTo"] = dtTo;
                    dsDetails.Tables["tblgridAdd"].Rows.Add(dr);

                    //InstallmentNo And Amount
                    dr["InstallmentNo"] = instNo;
                    dr["instAmt"] = 0;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            string ReportPath = "", SQLQuery1 = "", FormCaption = "", DataSourceName = "DataSet1";
            DataSourceName = "DataSet1";

            FormCaption = "Report :: Loan Information...";

            try
            {
                sqlQuery = "Delete tblTempLoan";
                arQuery.Add(sqlQuery);

                foreach (UltraGridRow row in this.gridData.Rows)
                {
                    sqlQuery = "Insert Into tblTempLoan (ComId, EmpId, InstallmentNo,LoanAmt,instAmt,Balance,InterestAmt,FlatAmt,DeductAmt,dtFrom,dtTo,Rate)" +
                               " Values ('" + Common.Classes.clsMain.intComId + "', '" + cboEmpID.Value.ToString() + "', '" + row.Cells["InstallmentNo"].Text.ToString() +
                               "','" + row.Cells["LoanAmt"].Text.ToString() + "','" + row.Cells["instAmt"].Text.ToString() +
                               "','" + row.Cells["Balance"].Text.ToString() + "','" + row.Cells["InterestAmt"].Text.ToString() +
                               "','" + row.Cells["FlatAmt"].Text.ToString() + "','" + row.Cells["DeductAmt"].Text.ToString() +
                               "','" + row.Cells["dtFrom"].Text.ToString() + "','" + row.Cells["dtTo"].Text.ToString() + "'," + txtRate.Value.ToString() + ")";
                    arQuery.Add(sqlQuery);

                }    

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);


                 ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptLoan.rdlc";
                 SQLQuery1 = "Exec [rptLoanTemp] " + Common.Classes.clsMain.intComId + ",'" + cboEmpID.Value.ToString() + "'";



                 clsReport.strReportPathMain = ReportPath;
                 clsReport.strQueryMain = SQLQuery1;
                 clsReport.strDSNMain = DataSourceName;

                 FM.prcShowReport(FormCaption);

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



        private void prcLoadLoan()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcProcessLoanUpdate " + Common.Classes.clsMain.intComId + ",'" + cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";

                gridData.DataSource = null;
                gridData.DataSource = dsList.Tables["tblgrid"];


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

        private void btnChange_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;


            try
            {
                sqlQuery = "Delete tblTempLoan";
                arQuery.Add(sqlQuery);

                foreach (UltraGridRow row in this.gridData.Rows)
                {
                    sqlQuery = "Insert Into tblTempLoan (ComId, EmpId, InstallmentNo,LoanAmt,instAmt,Balance,InterestAmt,FlatAmt,DeductAmt,dtFrom,dtTo,Rate)" +
                               " Values ('" + Common.Classes.clsMain.intComId + "', '" + cboEmpID.Value.ToString() + "', '" + row.Cells["InstallmentNo"].Text.ToString() +
                               "','" + row.Cells["LoanAmt"].Text.ToString() + "','" + row.Cells["instAmt"].Text.ToString() +
                               "','" + row.Cells["Balance"].Text.ToString() + "','" + row.Cells["InterestAmt"].Text.ToString() +
                               "','" + row.Cells["FlatAmt"].Text.ToString() + "','" + row.Cells["DeductAmt"].Text.ToString() +
                               "','" + row.Cells["dtFrom"].Text.ToString() + "','" + row.Cells["dtTo"].Text.ToString() + "'," + txtRate.Value.ToString() + ")";
                    arQuery.Add(sqlQuery);

                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcLoadLoan();

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
}
    





