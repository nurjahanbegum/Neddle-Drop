using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using GTRLibrary;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmMLPross : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private System.Data.DataSet dsDetails;
        private System.Data.DataSet dsLeaveBalance;
        private clsProcedure clsProc = new clsProcedure();

        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private int secId_update = 0; // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmMLPross(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmMLPross_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetail = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        public Boolean fncBlank()
        {
            //if(cboEmpCode.Text.Trim() == "")
            //{
            //    MessageBox.Show("Provide Employee Code.");
            //    cboEmpCode.Focus();
            //    return true;
            //}
            //else if (cboEmpCode.IsItemInList(cboEmpCode.Text) == false)
            //{
            //    MessageBox.Show("Select Employee Code From.");
            //    cboEmpCode.Focus();
            //    return true;
            //}

            return false;
        }

        public float hour(string str)
        {
            float hh, mm, time;
            hh = float.Parse(str.Substring(0,str.IndexOf(":")-1));
            mm = float.Parse(str.Substring(str.IndexOf(":") + 1,str.Length-1 ));
            time = hh + mm;
            return time;
        }



        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetMLPross -1," + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Leave";
                dsList.Tables[1].TableName = "EmpCode";
                dsList.Tables[2].TableName = "EntryToday";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["EntryToday"];

                CheckEdit.Checked = false;
                btnSave.Visible = false;
            
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

        public void prcLoadListIndividual(string str)
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = " Select A.lvId, A.empId, B.empCode, B.empName, A.lvType, CONVERT(varchar,A.dtFrom,106) as dtFrom, CONVERT(Varchar, A.dtTo,106)As dtTo, A.totalDay, A.lvApp, A.Remark  From tblLeave_Avail A Inner Join tblEmp_Info B On B.empID = A.EmpId where A.empid = '" + txtEmpID.Text.ToString() + "' order by lvId desc";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                dsList.Tables[0].TableName = "Leave";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Leave"];

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



        public void prcLoadCombo()
        {
            //cboType .DataSource = null;
            //cboType.DataSource = dsList.Tables["Leave"];

            //cboEmpCode.DataSource = null;
            //cboEmpCode.DataSource = dsList.Tables["EmpCode"];


        }

        public void prcDisplayDetails(string strParam, string dt)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();

            string sqlQuery1 = "";
            Int64 ChkML = 0;

            sqlQuery1 = "Select dbo.fncCheckML (" + Common.Classes.clsMain.intComId + ", " + Int32.Parse(strParam) + ")";
            ChkML = clsCon.GTRCountingDataLarge(sqlQuery1);

            try
            {
                string sqlQuery = "Exec prcGetMLPross  " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId + "," + ChkML + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, sqlQuery);
                dsDetail.Tables[0].TableName = "Shift";
                DataRow dr;

                if (dsDetail.Tables["Shift"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["Shift"].Rows[0];
                    txtId.Text = dr["lvId"].ToString();
                    txtEmpID.Text = dr["EmpCode"].ToString();
                    txtName.Text = dr["EmpName"].ToString();
                    txtType.Text = dr["lvType"].ToString();
                    txtDuration.Text = dr["totalDay"].ToString();
                    txtSect.Text = dr["SectName"].ToString();
                    txtGS.Text = dr["GS"].ToString();
                    txtBS.Text = dr["BS"].ToString();
                    dtFrom.Text = dr["dtFrom"].ToString();
                    dtTo.Text = dr["dtTo"].ToString();

                    txtFirstMonth.Text = dr["FirstSalMonth"].ToString();
                    txtSecondMonth.Text = dr["SecondSalMonth"].ToString();
                    txtThirdMonth.Text = dr["ThirdSalMonth"].ToString();

                    txtFirstAmt.Text = dr["FirstAmt"].ToString();
                    txtSecondAmt.Text = dr["SecondAmt"].ToString();
                    txtThirdAmt.Text = dr["ThirdAmt"].ToString();
                    txtTtlAmt.Text = dr["TtlAmount"].ToString();

                    txtFirstDays.Text = dr["FirstDays"].ToString();
                    txtSecondDays.Text = dr["SecondDays"].ToString();
                    txtThirdDays.Text = dr["ThirdDays"].ToString();
                    txtTtlDays.Text = dr["TtlDays"].ToString();

                    txtBonus.Text = dr["OtherBonus"].ToString();
                    txtDeduct.Text = dr["OtherDeduct"].ToString();

                    txtFirstInstall.Text = dr["FirstPayment"].ToString();
                    txtSecondInstall.Text = dr["LastPayment"].ToString();
                    txtRemarks.Text = dr["MLRemarks"].ToString();

                    txtNetAmt.Text = dr["NetPayable"].ToString();

                    checkPaid1.Checked = false;
                    checkPaid2.Checked = false;
                    ChkNoPay.Checked = false;

                    if (dr["FirstPaid"].ToString() == "1")
                    {
                        checkPaid1.Checked = true;
                    }
                    if (dr["LastPaid"].ToString() == "1")
                    {
                        checkPaid2.Checked = true;
                    }
                    if (dr["NoPay"].ToString() == "1")
                    {
                        ChkNoPay.Checked = true;
                    }

                }

                CheckEdit.Checked = false;
                btnSave.Visible = false;

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

        public void prcClearData()
        {

            txtId.Text = "";
            txtEmpID.Text = "";
            txtName.Text = "";
            txtType.Text = "";
            txtDuration.Value = "";
            txtSect.Text = "";

            txtGS.Value = "0";
            txtBS.Value = "0";

            txtFirstMonth.Value = "";
            txtSecondMonth.Value = "";
            txtThirdMonth.Value = "";

            txtFirstDays.Value = "";
            txtSecondDays.Value = "";
            txtThirdDays.Value = "";
            txtTtlDays.Value = "";


            txtFirstAmt.Value = "";
            txtSecondAmt.Value = "";
            txtThirdAmt.Value = "";
            txtTtlAmt.Value = "";

            txtBonus.Value = "0";
            txtDeduct.Value = "0";
            txtNetAmt.Value = "0";
            txtFirstInstall.Value = "0";
            txtSecondInstall.Value = "0";

            txtRemarks.Value = "";

            checkPaid1.Checked = false;
            checkPaid2.Checked = false;
            ChkNoPay.Checked = false;
            CheckEdit.Checked = false;
            btnSave.Visible = false;



        }

        private void frmMLPross_Load(object sender, EventArgs e)
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

 

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
           prcClearData();
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        
        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTotalDays_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtApproved_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtTotalDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtApproved_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtDateTo_Leave(object sender, EventArgs e)
        {
            //int x = (dtDateTo.DateTime - dtDateFrom.DateTime).Days + 1;
            //txtTotalDays.Text = x.ToString();
        }

        private void cboEmpCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void gridList_DoubleClick_1(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["lvId"].Value.ToString(), DateTime.Today.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void dtInput_KeyDown(object sender, KeyEventArgs e)
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

        private void dtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtDateFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtDateTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridList.DisplayLayout.Bands[0].Columns["lvId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Code";
            gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
            gridList.DisplayLayout.Bands[0].Columns["lvType"].Header.Caption = "Type";
            gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Header.Caption = "From(Date)";
            gridList.DisplayLayout.Bands[0].Columns["dtTo"].Header.Caption = "To(Date)";
            gridList.DisplayLayout.Bands[0].Columns["totalDay"].Header.Caption = "Days";
            gridList.DisplayLayout.Bands[0].Columns["lvApp"].Header.Caption = "Approved";
            gridList.DisplayLayout.Bands[0].Columns["Remark"].Header.Caption = "Remark";

            //Set 
            gridList.DisplayLayout.Bands[0].Columns["empCode"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 130;
            gridList.DisplayLayout.Bands[0].Columns["lvType"].Width = 50;
            gridList.DisplayLayout.Bands[0].Columns["dtFrom"].Width = 90;
            gridList.DisplayLayout.Bands[0].Columns["dtTo"].Width = 90;
            gridList.DisplayLayout.Bands[0].Columns["totalDay"].Width = 50;
            gridList.DisplayLayout.Bands[0].Columns["lvApp"].Width = 70;
            gridList.DisplayLayout.Bands[0].Columns["Remark"].Width = 150;



            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["lvId"].OriginalValue.ToString(), DateTime.Today.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboDuration_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void checkPaid1_CheckedChanged(object sender, EventArgs e)
        {
            checkPaid1.Tag = 0;
            if (checkPaid1.Checked == true)
            {
                checkPaid1.Tag = 1;
            }
        }

        private void checkPaid2_CheckedChanged(object sender, EventArgs e)
        {
            checkPaid2.Tag = 0;
            if (checkPaid2.Checked == true)
            {
                checkPaid2.Tag = 1;
            }
        }

        private void ChkNoPay_CheckedChanged(object sender, EventArgs e)
        {
            ChkNoPay.Tag = 0;
            if (ChkNoPay.Checked == true)
            {
                ChkNoPay.Tag = 1;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            try
            {
                string ReportPath = "", SQLQuery = "", FormCaption = "", DataSourceName = "DataSet1";
                DataSourceName = "DataSet1";

                FormCaption = "Report :: ML Report...";

                ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptMLLetter.rdlc";
                SQLQuery = "Exec [rptML] " + Common.Classes.clsMain.intComId + ",'" + txtId.Text.ToString() + "'";

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
        }

        private void btnPross_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to ML Process of [" + gridList.ActiveRow.Cells[2].Text.ToString() + "]", "",
                    System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                if (ChkNoPay.Checked == true)
                {

                    sqlQuery = "Delete tblML Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    //Save Data 
                    sqlQuery = " Insert into tblML (lvId,ComId,EmpId,dtInput,dtFrom,dtTo,TotalDay,lvType,lvApp,GS,BS,PCName,LUserId)"
                               + " Select lvId,ComId,EmpId,dtInput,dtFrom,dtTo,TotalDay,lvType,lvApp,'" + txtGS.Value.ToString() + "','" + 
                               txtBS.Value.ToString() + "','" + Common.Classes.clsMain.strComputerName +
                               "'," + Common.Classes.clsMain.intUserId + " from tblLeave_Avail Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    sqlQuery = "prcProcessML " + Common.Classes.clsMain.intComId + ",'" + txtId.Text.ToString() + "',1";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Update tblML Set FirstPaid ='0',LastPaid = '0', NoPay ='" + ChkNoPay.Tag.ToString()
                                                                + "',Remarks = '" + txtRemarks.Value.ToString()
                                                                + "' Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                }

                else
                {

                    sqlQuery = "Delete tblML Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    //Save Data 
                    sqlQuery = " Insert into tblML (lvId,ComId,EmpId,dtInput,dtFrom,dtTo,TotalDay,lvType,lvApp,GS,BS,PCName,LUserId)"
                               + " Select lvId,ComId,EmpId,dtInput,dtFrom,dtTo,TotalDay,lvType,lvApp,'" + txtGS.Value.ToString() + "','" + 
                               txtBS.Value.ToString() + "','" + Common.Classes.clsMain.strComputerName +
                               "'," + Common.Classes.clsMain.intUserId + " from tblLeave_Avail Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    sqlQuery = " Update tblML Set FirstPaid =1 Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "prcProcessML " + Common.Classes.clsMain.intComId + ",'" + txtId.Text.ToString() + "',0";
                    arQuery.Add(sqlQuery);

                }


                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Updated Successfully.");

                prcLoadList();
                prcClearData();

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

        private void CheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit.Tag = 0;

            if (CheckEdit.Checked == true)
            {
                btnSave.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";

            try
            {

                   sqlQuery = " Update tblML Set FirstPaid ='" + checkPaid1.Tag.ToString() + "',LastPaid ='" + 
                        checkPaid2.Tag.ToString() + "',OtherBonus = '" + 
                        txtBonus.Value.ToString() + "',OtherDeduct = '" + 
                        txtDeduct.Value.ToString() + "' Where lvId = '" + txtId.Text.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "prcProcessML " + Common.Classes.clsMain.intComId + ",'" + txtId.Text.ToString() + "',2";
                    arQuery.Add(sqlQuery);


                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("Data Updated Successfully.");

                prcLoadList();
                prcClearData();

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
