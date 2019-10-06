using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmFixAttMonthly : Form
    {
        string strValue = "";
        int whday = 0;

        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        string Data = "";
        DateTime firstDay;
        DateTime lastDay;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmFixAttMonthly(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }


        private void frmFixAttMonthly_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmFixAttMonthly_FormClosing(object sender, FormClosingEventArgs e)
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

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();
            cboSalDept.Value = 0;

            try
            {
                string sqlQuery = "Exec [prcGetManaulSalaryMng] 0," + Common.Classes.clsMain.intComId + ", '" + optCriteria.Value + "', '" + cboPross.Value + "','" + cboSalDept.Value + "',0 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblPross";
                dsList.Tables[2].TableName = "tblSalDept";
                dsList.Tables[3].TableName = "tblEmp";


                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];



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


        private void prcLoadSalaryData()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();
            cboSalDept.Value = 0;

            try
            {
                string sqlQuery = "Exec [prcGetManaulSalaryMng] 1," + Common.Classes.clsMain.intComId + ", '" + optCriteria.Value + "', '" + cboPross.Value + "','" + cboSalDept.Value + "',0 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];



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
            cboPross.DataSource = null;
            cboPross.DataSource = dsList.Tables["tblPross"];
            
            cboSalDept.DataSource = null;
            cboSalDept.DataSource = dsList.Tables["tblSalDept"];

            cboEmpId.DataSource = null;
            cboEmpId.DataSource = dsList.Tables["tblEmp"];

        }
        private void cboPross_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPross.DisplayLayout.Bands[0].Columns["ProssType"].Width = cboPross.Width;
            cboPross.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "Month-Year";
            cboPross.DisplayLayout.Bands[0].Columns["dtInput"].Hidden = true;
            cboPross.DisplayMember = "ProssType";
            cboPross.ValueMember = "ProssType";
        }

        private void cboSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSalDept.DisplayLayout.Bands[0].Columns["SalDeptName"].Width = cboSalDept.Width;
            cboSalDept.DisplayLayout.Bands[0].Columns["SalDeptName"].Header.Caption = "Salary Dept.";
            cboSalDept.DisplayLayout.Bands[0].Columns["SalId"].Hidden = true;
            cboSalDept.DisplayMember = "SalDeptName";
            cboSalDept.ValueMember = "SalId";
        }

        private void cboEmpId_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpName"].Width = cboEmpId.Width;
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee";
            cboEmpId.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            cboEmpId.DisplayMember = "EmpName";
            cboEmpId.ValueMember = "EmpId";
        }
        private void prcClearData()
        {

            this.gridList.DataSource = null;


            this.cboSalDept.Value = null;
            this.cboEmpId.Value = null;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            this.optCriteria.Value = "All";
            //groupData.Enabled = false;
            groupBoxCombo.Enabled = false;
            this.cboSalDept.Enabled = false;
            this.cboEmpId.Enabled = false;


        }
        private void btnSave_Click(object sender, EventArgs e)
        {
             if (fncBlank())
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
                //Member Master Table

                if (optCriteria.Value == "Released")
                {
                        foreach (UltraGridRow row in this.gridList.Rows)
                        {
                            if (row.Cells["isChecked"].Value.ToString() == "1")
                            {

                                sqlQuery = "Update tblProcessedDataSalMng Set Present = '" + row.Cells["Present"].Text.ToString() 
                                           + "',Absent = '" + row.Cells["Absent"].Text.ToString() 
                                           + "',DDay = '" + row.Cells["DDay"].Text.ToString() 
                                           + "',WDay = '" + row.Cells["WDay"].Text.ToString() 
                                           + "',HDay = '" + row.Cells["HDay"].Text.ToString() 
                                           + "',CL = '" + row.Cells["CL"].Text.ToString() 
                                           + "',SL = '" + row.Cells["SL"].Text.ToString() 
                                           + "',EL = '" + row.Cells["EL"].Text.ToString()
                                           + "',Adv = '" + row.Cells["Adv"].Text.ToString()
                                           + "',GP = '" + row.Cells["NPSalaryDed"].Text.ToString()
                                           + "',Loan = '" + row.Cells["Loan"].Text.ToString()
                                           + "',OthersDeduct = '" + row.Cells["OthDed"].Text.ToString()
                                           + "',InComeTax = '" + row.Cells["Tax"].Text.ToString() 
                                           + "',OthersAddition = '" + row.Cells["OThAllow"].Text.ToString()
                                           + "',Arrear = '" + row.Cells["Benifit"].Text.ToString() 
                                           + "',OTHrTtl = '" + row.Cells["OTHrTtl"].Text.ToString() 
                                           + "',OT = '" + row.Cells["Amt"].Text.ToString() 
                                           + "',OtherAllow = '" + row.Cells["OT"].Text.ToString() 
                                           + "',Trn = '" + row.Cells["Trn"].Text.ToString() 
                                           + "',MobileAllow = '" + row.Cells["Mobile"].Text.ToString()
                                           + "',ELAmount = '" + row.Cells["ELAmount"].Text.ToString()
                                           + "',PFOwn = '" + row.Cells["PFOwn"].Text.ToString()
                                           + "',PFCom = '" + row.Cells["PFCom"].Text.ToString()
                                           + "',PFProfit = '" + row.Cells["PFProfit"].Text.ToString()
                                           + "',MngType = '2' Where EmpID = '" + row.Cells["EmpId"].Text.ToString() 
                                           + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() + "'";
                                                                    
                                arQuery.Add(sqlQuery);

                                // Insert Information To Log File
                                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                    + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                                arQuery.Add(sqlQuery);

                                sqlQuery = "Exec [prcGetManaulSalaryMng] 3," + Common.Classes.clsMain.intComId + ", '" + optCriteria.Value + "','" + row.Cells["ProssType"].Text.ToString() + "','" + cboSalDept.Value + "','" + row.Cells["EmpId"].Text.ToString() + "'";
                                arQuery.Add(sqlQuery);

                            }
                        }
                }

                else   ///All Employee,SalDept Wise, Employee Wise
                {
                    foreach (UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = "Update tblProcessedDataSalMng Set Present = '" + row.Cells["Present"].Text.ToString()
                                       + "',Absent = '" + row.Cells["Absent"].Text.ToString()
                                       + "',DDay = '" + row.Cells["DDay"].Text.ToString()
                                       + "',WDay = '" + row.Cells["WDay"].Text.ToString()
                                       + "',HDay = '" + row.Cells["HDay"].Text.ToString()
                                       + "',CL = '" + row.Cells["CL"].Text.ToString()
                                       + "',SL = '" + row.Cells["SL"].Text.ToString()
                                       + "',EL = '" + row.Cells["EL"].Text.ToString()
                                       + "',Adv = '" + row.Cells["Adv"].Text.ToString()
                                       + "',Loan = '" + row.Cells["Loan"].Text.ToString()
                                       + "',OthersDeduct = '" + row.Cells["OthDed"].Text.ToString()
                                       + "',InComeTax = '" + row.Cells["Tax"].Text.ToString()
                                       + "',OthersAddition = '" + row.Cells["OThAllow"].Text.ToString()
                                       + "',Arrear = '" + row.Cells["Arear"].Text.ToString()
                                       + "',OTHrTtl = '" + row.Cells["OTHrTtl"].Text.ToString()
                                       + "',OT = '" + row.Cells["Amt"].Text.ToString()
                                       + "',OtherAllow = '" + row.Cells["OT"].Text.ToString()
                                       + "',Trn = '" + row.Cells["Trn"].Text.ToString()
                                       + "',MobileAllow = '" + row.Cells["Mobile"].Text.ToString()
                                       + "',MngType = '1' Where EmpID = '" + row.Cells["EmpId"].Text.ToString()
                                       + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() + "'";

                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                            arQuery.Add(sqlQuery);

                            sqlQuery = "Exec [prcGetManaulSalaryMng] 2," + Common.Classes.clsMain.intComId + ", '" + optCriteria.Value + "','" + row.Cells["ProssType"].Text.ToString() + "','" + cboSalDept.Value + "','" + row.Cells["EmpId"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);

                        }
                    }
                }


                        clsCon.GTRSaveDataWithSQLCommand(arQuery);
                        MessageBox.Show("Data Update Successfully Complete.");


                prcLoadSalaryData();
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

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                if (optCriteria.Value == "Released")
                {

                    //Hide column

                    //gridList.DisplayLayout.Bands[0].Columns["ComID"].Hidden = true;  //Country Name
                    gridList.DisplayLayout.Bands[0].Columns["WorkingDays"].Hidden = true;  //Working Days


                    ////Set Width
                    gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50;  //Short Name
                    gridList.DisplayLayout.Bands[0].Columns["EmpId"].Width = 65;
                    gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 120;
                    gridList.DisplayLayout.Bands[0].Columns["SalDeptName"].Width = 100;

                    gridList.DisplayLayout.Bands[0].Columns["Present"].Width = 45;  //
                    gridList.DisplayLayout.Bands[0].Columns["Absent"].Width = 45;  //
                    gridList.DisplayLayout.Bands[0].Columns["DDay"].Width = 45;  //
                    gridList.DisplayLayout.Bands[0].Columns["WDay"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["HDay"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["CL"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["SL"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["EL"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["AB"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["ADV"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Loan"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["OthDed"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Tax"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["NPSalaryDed"].Width = 85;  //
                    gridList.DisplayLayout.Bands[0].Columns["TotalDeduct"].Width = 70;  //
                    gridList.DisplayLayout.Bands[0].Columns["OThAllow"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["Benifit"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["OTHrTtl"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["Amt"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["OT"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Trn"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Mobile"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["ELAmount"].Width = 70;  //
                    gridList.DisplayLayout.Bands[0].Columns["TotalPay"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["PFOwn"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["PFCom"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["PFProfit"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["PFTotal"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["NetPay"].Width = 65;  //
                    gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 65;
                    gridList.DisplayLayout.Bands[0].Columns["Name"].Width = 100;
                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].Width = 130;  //

                    ////Set Caption
                    gridList.DisplayLayout.Bands[0].Columns["Present"].Header.Caption = "Pr";
                    gridList.DisplayLayout.Bands[0].Columns["Absent"].Header.Caption = "AB";
                    gridList.DisplayLayout.Bands[0].Columns["AB"].Header.Caption = "ABAmt";
                    gridList.DisplayLayout.Bands[0].Columns["WDay"].Header.Caption = "WD";
                    gridList.DisplayLayout.Bands[0].Columns["HDay"].Header.Caption = "HD";


                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "ProssType";
                    this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                    // Stop Cell Modify
                    gridList.DisplayLayout.Bands[0].Columns["EmpId"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["SalDeptName"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["GS"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["PFTotal"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["TotalDeduct"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["Amt"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["TotalPay"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["NetPay"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["Name"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].CellActivation = Activation.NoEdit;


                    //Change alternate color
                    gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                    gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                    //Select Column Color
                    gridList.DisplayLayout.Override.ActiveRowCellAppearance.BackColor = Color.Red;

                    //Column Color
                    gridList.DisplayLayout.Bands[0].Columns["EmpId"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["SalDeptName"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["GS"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["PFTotal"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["TotalDeduct"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["Amt"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["TotalPay"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["NetPay"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].CellAppearance.BackColor = Color.LightGreen;

                    //Select Full Row when click on any cell
                    //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                    //Selection Style Will Be Row Selector
                    // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                    //Stop Updating
                    this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

                    //Hiding +/- Indicator
                    this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                    ////RowHeight
                    gridList.DisplayLayout.Override.DefaultRowHeight = 24;

                    //Hide Group Box Display
                    e.Layout.GroupByBox.Hidden = true;

                    this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
                }

                else
                {

                    //Hide column

                    //gridList.DisplayLayout.Bands[0].Columns["ComID"].Hidden = true;  //Country Name
                    gridList.DisplayLayout.Bands[0].Columns["WorkingDays"].Hidden = true;  //Working Days


                    ////Set Width
                    gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50;  //Short Name
                    gridList.DisplayLayout.Bands[0].Columns["EmpId"].Width = 65;
                    gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 120;
                    gridList.DisplayLayout.Bands[0].Columns["SalDeptName"].Width = 100;

                    gridList.DisplayLayout.Bands[0].Columns["Present"].Width = 45;  //
                    gridList.DisplayLayout.Bands[0].Columns["Absent"].Width = 45;  //
                    gridList.DisplayLayout.Bands[0].Columns["DDay"].Width = 45;  //
                    gridList.DisplayLayout.Bands[0].Columns["WDay"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["HDay"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["CL"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["SL"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["EL"].Width = 32;  //
                    gridList.DisplayLayout.Bands[0].Columns["AB"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["ADV"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Loan"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["OthDed"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Tax"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["PF"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["TotalDeduct"].Width = 70;  //
                    gridList.DisplayLayout.Bands[0].Columns["OThAllow"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["Arear"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["OTHrTtl"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["Amt"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["OT"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Trn"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["Mobile"].Width = 50;  //
                    gridList.DisplayLayout.Bands[0].Columns["TotalPay"].Width = 60;  //
                    gridList.DisplayLayout.Bands[0].Columns["NetPay"].Width = 65;  //
                    gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 65;
                    gridList.DisplayLayout.Bands[0].Columns["Name"].Width = 100;
                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].Width = 130;  //

                    ////Set Caption
                    gridList.DisplayLayout.Bands[0].Columns["Present"].Header.Caption = "Pr";
                    gridList.DisplayLayout.Bands[0].Columns["Absent"].Header.Caption = "AB";
                    gridList.DisplayLayout.Bands[0].Columns["AB"].Header.Caption = "ABAmt";
                    gridList.DisplayLayout.Bands[0].Columns["WDay"].Header.Caption = "WD";
                    gridList.DisplayLayout.Bands[0].Columns["HDay"].Header.Caption = "HD";


                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "ProssType";
                    this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                    // Stop Cell Modify
                    gridList.DisplayLayout.Bands[0].Columns["EmpId"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["SalDeptName"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["GS"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["PF"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["TotalDeduct"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["Amt"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["TotalPay"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["NetPay"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["Name"].CellActivation = Activation.NoEdit;
                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].CellActivation = Activation.NoEdit;


                    //Change alternate color
                    gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                    gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                    //Select Column Color
                    gridList.DisplayLayout.Override.ActiveRowCellAppearance.BackColor = Color.Red;

                    //Column Color
                    gridList.DisplayLayout.Bands[0].Columns["EmpId"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["SalDeptName"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["GS"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["PF"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["TotalDeduct"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["Amt"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["TotalPay"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["NetPay"].CellAppearance.BackColor = Color.LightCyan;
                    gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.BackColor = Color.LightGreen;
                    gridList.DisplayLayout.Bands[0].Columns["ProssType"].CellAppearance.BackColor = Color.LightGreen;

                    //Select Full Row when click on any cell
                    //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                    //Selection Style Will Be Row Selector
                    // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                    //Stop Updating
                    this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

                    //Hiding +/- Indicator
                    this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                    ////RowHeight
                    gridList.DisplayLayout.Override.DefaultRowHeight = 24;

                    //Hide Group Box Display
                    e.Layout.GroupByBox.Hidden = true;

                    this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Boolean fncBlank()
        {

            if (optCriteria.Value == "All")
            {
                Data = "";
            }
            else if (optCriteria.Value == "Sec")
            {
                if (this.cboSalDept.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Section");
                    cboSalDept.Focus();
                    return true;
                }
            }
            else if (optCriteria.Value == "ShiftTime")
            {
                if (this.cboEmpId.Text.Length == 0)
                {
                    MessageBox.Show("Please provide Shift");
                    cboEmpId.Focus();
                    return true;
                }
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete Leave Which Are shown in the Grid" , "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery=new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["isChecked"].Value.ToString() == "1")
                    {
                        //RowID = row.Index + 1;
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                        sqlQuery = " Delete  tblLeave_Balance where empid = '" + row.Cells["empid"].Text.ToString() + "' ";
                        arQuery.Add(sqlQuery);

                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

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


        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    prcClearData();
            //    prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboCountryName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCountryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }
        private void dtJoinDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            if (optCriteria.Value == "All")
            {
                cboSalDept.Enabled = false;
                cboEmpId.Enabled = false;
                groupBoxCombo.Enabled = false;
            }
            else if (optCriteria.Value == "SalDept")
            {
                groupBoxCombo.Enabled = true;
                cboSalDept.Enabled = true;
                cboEmpId.Enabled = false;
            }
            else if (optCriteria.Value == "Employee")
            {
                groupBoxCombo.Enabled = true;
                cboSalDept.Enabled = false;
                cboEmpId.Enabled = true;
            }



        }

        private void cboAddList_Click(object sender, EventArgs e)
        {


            //if (cboSalDept == null)
            //{ cboSalDept.Value = 0; }

            //if (optCriteria.Value == "All")
            //{
            //   Data  = "";
            //}
            //else if (optCriteria.Value == "SalDept")
            //{
            //    Data = cboSalDept.Value.ToString();
            //}
            //else if (optCriteria.Value == "Employee")
            //{
            //    Data = "";
            //}

            int Rowcount;

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetManaulSalaryMng] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value + "', '" + cboPross.Value + "','" + cboSalDept.Value + "','" + cboEmpId.Value + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                //MessageBox.Show("Number of row(s) - " + dsDetails.Tables[0].Rows.Count);

                Rowcount = dsDetails.Tables[0].Rows.Count;
                uLvlCount.Text = Convert.ToString(Rowcount);

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }

            btnSave.Text = "&Update";
            btnDelete.Enabled = true;


        }



        private void cboSection_ValueChanged(object sender, EventArgs e)
        {
            if (cboSalDept.Value == null)
                return;
            
            strValue = cboSalDept.Value.ToString();
        }

        private void cboShiftTime_ValueChanged(object sender, EventArgs e)
        {
            if (cboEmpId.Value == null)
                return;
            
            strValue = cboEmpId.Value.ToString();
        }

      

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void checkBox2_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnDelete_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }



        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    row.Cells["isChecked"].Value = 1;
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    row.Cells["isChecked"].Value = 0;
                }
            }
        }

        private void gridList_TextChanged(object sender, EventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
            {
                row.Cells["isChecked"].Value = 0;
            }
        }















    }
}
