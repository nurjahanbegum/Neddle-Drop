using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmFixAttIndividual : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public  frmFixAttIndividual(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                                Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmFixAttIndividual_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtInputDate.DateTime = lastDay;

                dtSalMonth.DateTime = lastDay;

                DateTime firstDay = new DateTime(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month, 1);
                dtFrom.Value = firstDay;

                //Month wise Total Friday
                txtWDay.Value = CountFridays(firstDay, lastDay);
                //Month Total Days
                ttlWorkDay.Value = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);

                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - Int32.Parse(txtWDay.Value.ToString());


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
                string sqlQuery = "Exec [prcGetAttFixIndividual] " + Common.Classes.clsMain.intComId + ",0, 0,'" + dtInputDate.Value.ToString() + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";
                dsList.Tables[1].TableName = "tblCompany";
                dsList.Tables[2].TableName = "tblEmployee";


                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];

              
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

        private void prcLoadListAfterSave()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetAttFixIndividual] " + Common.Classes.clsMain.intComId + ",1, 0,'" + dtInputDate.Value.ToString() + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";
                dsList.Tables[1].TableName = "tblCompany";
                dsList.Tables[2].TableName = "tblEmployee";


                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];


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
            cboCom.DataSource = null;
            cboCom.DataSource = dsList.Tables["tblCompany"];
               
            cboCode.DataSource = null;
            cboCode.DataSource = dsList.Tables["tblEmployee"];

        }
        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetAttFixIndividual " + Common.Classes.clsMain.intComId + ",3," + Int32.Parse(strParam) + ",'" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "'  ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    this.cboCode.Value = dr["EmpId"].ToString();
                    this.txtId.Text = dr["EmpId"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtDesig.Text = dr["DesigName"].ToString();
                    this.txtSec.Text = dr["SectName"].ToString();
                    this.cboCom.Value = dr["ComName"].ToString();
                    
                    this.ttlWorkDay.Text = dr["WorkingDays"].ToString();
                    this.txtPresent.Text = dr["Present"].ToString();
                    this.txtAbs.Text = dr["Absent"].ToString();
                    this.txtLate.Text = dr["Late"].ToString();
                    this.txtCL.Text = dr["CL"].ToString();
                    this.txtSL.Text = dr["SL"].ToString();
                    this.txtEL.Text = dr["EL"].ToString();
                    this.txtWDay.Text = dr["Wday"].ToString();
                    this.txtHDay.Text = dr["HDay"].ToString();

                    this.txtPrdBns.Text = dr["PBonus"].ToString();
                    this.txtArrear.Text = dr["Arrear"].ToString();
                    this.txtShortHr.Text = dr["OtherAllow"].ToString();
                    this.txtOTHr.Text = dr["OTHr"].ToString();
                    this.txtAdvDed.Text = dr["Adv"].ToString();
                    this.txtOthDed.Text = dr["OthersDeduct"].ToString();
                    this.dtSalMonth.Value = dr["SalMonth"].ToString();
                    
                    this.btnSave.Text = "&Save";
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

        private void prcIdInfoLoad(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetAttFixIndividual " + Common.Classes.clsMain.intComId + ",2," + Int32.Parse(strParam) + ",'" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    this.cboCode.Value = dr["empid"].ToString();
                    this.txtId.Text = dr["EmpId"].ToString();
                    this.cboCom.Value = dr["ComName"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.txtDesig.Text = dr["DesigName"].ToString();
                    this.txtSec.Text = dr["SectName"].ToString();

                    this.btnSave.Text = "&Save";
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prcClearData()
        {

            cboCode.Text = "";
            txtName.Text = "";
            txtDesig.Text = "";
            txtSec.Text = "";

            txtPresent.Value = 0;
            txtAbs.Value = 0;
            txtLate.Value = 0;
            txtCL.Value = 0;
            txtSL.Value = 0;
            txtEL.Value = 0;
            txtHDay.Value = 0;
            txtPrdBns.Value = 0;
            txtArrear.Value = 0;
            txtShortHr.Value = 0;
            txtOTHr.Value = 0;
            txtAdvDed.Value = 0;
            txtOthDed.Value = 0;

            DateTime lastDay = new DateTime(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtSalMonth.Value = lastDay;

            DateTime firstDay = new DateTime(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month, 1);
            dtFrom.Value = firstDay;

            //Month wise Total Friday
            txtWDay.Value = CountFridays(firstDay, lastDay);
            //Month Total Days
            ttlWorkDay.Value = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);

            txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - Int32.Parse(txtWDay.Value.ToString());

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
            txtPresent.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
            prcLoadList();
            prcLoadCombo();
        }
        private void  frmFixAttIndividual_FormClosing(object sender, FormClosingEventArgs e)
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

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Grid Width
                gridList.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true; //Employee ID
                gridList.DisplayLayout.Bands[0].Columns["SalMonth"].Hidden = true; //SalMonth

                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80; //Employee code
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 100; //Employee Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 100; //Section 
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 100; //Designation 
                gridList.DisplayLayout.Bands[0].Columns["Present"].Width = 60; //Present
                gridList.DisplayLayout.Bands[0].Columns["Absent"].Width = 60; //Absent
                gridList.DisplayLayout.Bands[0].Columns["WDay"].Width = 60; //WeekDay
                gridList.DisplayLayout.Bands[0].Columns["HDay"].Width = 60; //HoliDay
                gridList.DisplayLayout.Bands[0].Columns["CL"].Width = 60; //CL
                gridList.DisplayLayout.Bands[0].Columns["SL"].Width = 60; //SL
                gridList.DisplayLayout.Bands[0].Columns["EL"].Width = 60; //EL
                gridList.DisplayLayout.Bands[0].Columns["OTHr"].Width = 60; //EL

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["Present"].Header.Caption = "Present";
                gridList.DisplayLayout.Bands[0].Columns["Absent"].Header.Caption = "Absent";
                gridList.DisplayLayout.Bands[0].Columns["WDay"].Header.Caption = "Week Day";
                gridList.DisplayLayout.Bands[0].Columns["HDay"].Header.Caption = "Holiday";
                gridList.DisplayLayout.Bands[0].Columns["CL"].Header.Caption = "CL";
                gridList.DisplayLayout.Bands[0].Columns["SL"].Header.Caption = "SL";
                gridList.DisplayLayout.Bands[0].Columns["EL"].Header.Caption = "EL";
                gridList.DisplayLayout.Bands[0].Columns["OTHr"].Header.Caption = "OT Hour";

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

        //private void gridList_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        prcClearData();
        //        prcDisplayDetails(gridList.ActiveRow.Cells["EmpId"].Value.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
            
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}


            DateTime lastDay = new DateTime(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtSalMonth.Value = lastDay;

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int64 NewId = 0;
            try
            {


                    //Data Delete
                    sqlQuery = "Delete  tblAttFixMonthly Where EmpID = '" + cboCode.Value.ToString() + "' And dtInput = '" + clsProc.GTRDate(dtSalMonth.Value.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                    //Insert Data
                    sqlQuery = " Insert Into tblAttFixMonthly (ComId,EmpID,WorkingDays,Present, Absent, Late, CL, SL,EL, Wday, HDay,PBonus,Arrear,ShortLeave,OTHr,Adv,OthersDeduct,dtInput,LUserId,PCName)"
                     + " Values (" + Common.Classes.clsMain.intComId + ",'" + cboCode.Value.ToString() + "','" + ttlWorkDay.Text.ToString() + "','" + txtPresent.Text.ToString() + "', '" +
                         txtAbs.Text.ToString() + "', '" + txtLate.Text.ToString() + "', '" + txtCL.Text.ToString() + "', '" +
                         txtSL.Text.ToString() + "','" + txtEL.Text.ToString() + "', '" + txtWDay.Text.ToString() + "', '" + 
                         txtHDay.Text.ToString() + "','" + txtPrdBns.Text.ToString() + "','" + 
                         txtArrear.Text.ToString() + "','" + txtShortHr.Text.ToString() + "','" + 
                         txtOTHr.Text.ToString() + "', '" + txtAdvDed.Text.ToString() + "', '" +
                         txtOthDed.Text.ToString() + "','" + clsProc.GTRDate(dtSalMonth.Value.ToString()) + "'," + 
                         GTRHRIS.Common.Classes.clsMain.intUserId + ",'" + Common.Classes.clsMain.strComputerName + "')";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Update A Set A.SectId = E.SectId, A.DesigId = E.DesigId from tblAttFixMonthly A,tblEmp_Info E Where A.EmpId = E.EmpId and E.EmpId = '" + cboCode.Value.ToString() + "' And A.dtInput = '" + clsProc.GTRDate(dtSalMonth.Value.ToString()) + "'";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                //}
                prcClearData();
                prcLoadListAfterSave();
                prcLoadCombo();
               
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
      

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to Delete Employee information of [" + cboCode.Text.ToString() + "]", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";
                //Delete Data
                sqlQuery = "Delete  tblAttFixMonthly Where EmpID = " + Int32.Parse(txtId.Value.ToString()) + " and comid = " + Common.Classes.clsMain.intComId + "  And dtInput = '" + clsProc.GTRDate(dtSalMonth.Value.ToString()) + "'";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcClearData();
                prcLoadListAfterSave();
                prcLoadCombo();
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

        private void cboCom_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCom.DisplayLayout.Bands[0].Columns["ComId"].Hidden = true;
            cboCom.DisplayLayout.Bands[0].Columns["comName"].Width = cboCom.Width;
            cboCom.DisplayLayout.Bands[0].Columns["comName"].Header.Caption = "Company Name";
            cboCom.DisplayMember = "comName";
            cboCom.ValueMember = "ComId";
        }

        private void cboCode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCode.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            cboCode.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80;
            cboCode.DisplayLayout.Bands[0].Columns["EmpName"].Width = 189;
            cboCode.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
            cboCode.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
            cboCode.ValueMember = "EmpId";
            cboCode.DisplayMember = "empcode";

        }


        private void cboCode_ValueChanged(object sender, EventArgs e)
        {
            //prcClearData();
            if (this.cboCode.IsItemInList() == false)
            {
                prcLoadCombo();
                return;
            }

            if (cboCode.Value == null)
            {
                return;
            }
            prcIdInfoLoad(cboCode.Value.ToString());
        }

        private void txtAbs_ValueChanged(object sender, EventArgs e)
        {
            if (txtPresent.Value == null)
            {
                txtPresent.Value = 0;
            }

            if (txtAbs.Text.Length == 0)
            {
                txtAbs.Value = 0;
            }

            if (double.Parse(txtAbs.Value.ToString()) >= 0)
            {
                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - (Int32.Parse(txtHDay.Value.ToString()) + Int32.Parse(txtWDay.Value.ToString()) + Int32.Parse(txtCL.Value.ToString()) + Int32.Parse(txtSL.Value.ToString()) + Int32.Parse(txtEL.Value.ToString()) + Int32.Parse(txtAbs.Value.ToString()));
            }
        }

        private void txtWDay_ValueChanged(object sender, EventArgs e)
        {
            if (txtWDay.Text.Length == 0)
            {
                txtWDay.Value = 0;
            }
            if (double.Parse(txtWDay.Value.ToString()) >= 0)
            {
                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - (Int32.Parse(txtHDay.Value.ToString()) + Int32.Parse(txtWDay.Value.ToString()) + Int32.Parse(txtCL.Value.ToString()) + Int32.Parse(txtSL.Value.ToString()) + Int32.Parse(txtEL.Value.ToString()) + Int32.Parse(txtAbs.Value.ToString()));
            }
        }

        private void txtCL_ValueChanged(object sender, EventArgs e)
        {
            if (txtCL.Text.Length == 0)
            {
                txtCL.Value = 0;
            }
            if (double.Parse(txtCL.Value.ToString()) >= 0)
            {
                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - (Int32.Parse(txtHDay.Value.ToString()) + Int32.Parse(txtWDay.Value.ToString()) + Int32.Parse(txtCL.Value.ToString()) + Int32.Parse(txtSL.Value.ToString()) + Int32.Parse(txtEL.Value.ToString()) + Int32.Parse(txtAbs.Value.ToString()));
            }
        }

        private void txtSL_ValueChanged(object sender, EventArgs e)
        {
            if (txtSL.Text.Length == 0)
            {
                txtSL.Value = 0;
            }
            
            if (double.Parse(txtSL.Value.ToString()) >= 0)
            {
                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - (Int32.Parse(txtHDay.Value.ToString()) + Int32.Parse(txtWDay.Value.ToString()) + Int32.Parse(txtCL.Value.ToString()) + Int32.Parse(txtSL.Value.ToString()) + Int32.Parse(txtEL.Value.ToString()) + Int32.Parse(txtAbs.Value.ToString()));
            }
        }

        private void txtEL_ValueChanged(object sender, EventArgs e)
        {
            if (txtEL.Text.Length == 0)
            {
                txtEL.Value = 0;
            }
            if (double.Parse(txtEL.Value.ToString()) >= 0)
            {
                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - (Int32.Parse(txtHDay.Value.ToString()) + Int32.Parse(txtWDay.Value.ToString()) + Int32.Parse(txtCL.Value.ToString()) + Int32.Parse(txtSL.Value.ToString()) + Int32.Parse(txtEL.Value.ToString()) + Int32.Parse(txtAbs.Value.ToString()));
            }
        }

        private void txtHDay_ValueChanged(object sender, EventArgs e)
        {
            if (txtHDay.Text.Length == 0)
            {
                txtHDay.Value = 0;
            }
            if (double.Parse(txtHDay.Value.ToString()) >= 0)
            {
                txtPresent.Value = Int32.Parse(ttlWorkDay.Value.ToString()) - (Int32.Parse(txtHDay.Value.ToString()) + Int32.Parse(txtWDay.Value.ToString()) + Int32.Parse(txtCL.Value.ToString()) + Int32.Parse(txtSL.Value.ToString()) + Int32.Parse(txtEL.Value.ToString()) + Int32.Parse(txtAbs.Value.ToString()));
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {

            DateTime lastDay = new DateTime(dtInputDate.DateTime.Year, dtInputDate.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtInputDate.Value = lastDay;

            prcLoadList();


        }
        private void dtInputDate_ValueChanged_1(object sender, EventArgs e)
        {
           
        }

        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCom_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtDesig_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPresent_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtAbs_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtWDay_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCL_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSL_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtEL_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtHDay_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtAvdDeduc_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSusDeduc_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMobDeduc_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtArrear_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (dtFrom.DateTime.Month == 1)
            {
                var firstDay = new DateTime(dtFrom.DateTime.Year - 1, dtFrom.DateTime.Month + 11, 1);
                dtFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);

                dtSalMonth.Value = lastDay;
                //Month wise Total Friday
                txtWDay.Value = CountFridays(firstDay, lastDay);
                //Month Total Days
                ttlWorkDay.Value = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month - 1);
                var lastDay = new DateTime(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month - 1, DaysInMonth);
                var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month - 1, 1);
                dtFrom.Value = firstDay;
                dtSalMonth.Value = lastDay;
                txtWDay.Value = CountFridays(firstDay, lastDay);

                ttlWorkDay.Value = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dtFrom.DateTime.Month == 12)
            {
                var firstDay = new DateTime(dtFrom.DateTime.Year + 1, dtFrom.DateTime.Month - 11, 1);
                dtFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
                var lastDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, DaysInMonth);


                dtSalMonth.Value = lastDay;
                txtWDay.Value = CountFridays(firstDay, lastDay);

                ttlWorkDay.Value = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month + 1);
                var lastDay = new DateTime(dtSalMonth.DateTime.Year, dtSalMonth.DateTime.Month + 1, DaysInMonth);
                var firstDay = new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month + 1, 1);
                dtFrom.Value = firstDay;
                dtSalMonth.Value = lastDay;
                txtWDay.Value = CountFridays(firstDay, lastDay);

                ttlWorkDay.Value = DateTime.DaysInMonth(dtFrom.DateTime.Year, dtFrom.DateTime.Month);
            }

            
        }

        private int CountFridays(DateTime startDate, DateTime endDate)
        {
            int FridayCount = 0;

            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1.0))
            {
                if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    FridayCount++;
                }
            }

            return FridayCount;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();

            DateTime lastDay = new DateTime(dtInputDate.DateTime.Year, dtInputDate.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtInputDate.Value = lastDay;

            try
            {
                string sqlQuery = "Exec [prcGetAttFixIndividual] " + Common.Classes.clsMain.intComId + ",1, 0,'" + dtInputDate.Value.ToString() + "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                dsList.Tables[0].TableName = "tblGrid";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];


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







    }
}