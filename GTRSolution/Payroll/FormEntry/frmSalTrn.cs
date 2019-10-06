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
using Infragistics.Win.UltraWinGrid;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmSalTrn : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private DataView dvStyle;
        private DataView dvSpec;
        private DataView dvColor;

        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmSalTrn(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmSalTrn_Load(object sender, EventArgs e)
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
                sqlQuery = "Exec prcGetSalTrn " + Common.Classes.clsMain.intComId + ", 0, 0,'','','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'";
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
            txtAmount.Text = "";
            txtRemark.Text = "";
            cboSec.Text = "";
            cboBand.Text = "";
            cboEmp.Text = "";

            checkBox2.Checked = false;


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

        private void frmSalTrn_FormClosing(object sender, FormClosingEventArgs e)
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
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Trn Amount";
                gridDetails.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 100;
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Width = 140;
                gridDetails.DisplayLayout.Bands[0].Columns["Remarks"].Width = 160;

                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;


                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //gridDetails.DisplayLayout.Bands[0].Columns["isInactive"].Style =
                //   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //gridDetails.DisplayLayout.Bands[0].Columns["aId"].Style =
                //   Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerWithSpin;

                ////Select Full Row when click on any cell
                ////e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                ////Selection Style Will Be Row Selector
                ////gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////Stop Updating
                ////gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

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
                sqlQuery = "Exec prcGetSalTrn " + Common.Classes.clsMain.intComId + ", " + EmpId + "," + SectId + ",'" + Band + "','" + optCriteria.Value.ToString() + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];

                checkBox2.Checked = false;


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

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
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

                            sqlQuery = " Delete  tblSal_Transport Where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtInput =  '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' and ComID = " + Common.Classes.clsMain.intComId + "";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblSal_Transport (EmpId,dtInput,Amount,Remarks,Luserid,comid,pcname) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                                       row.Cells["Amount"].Value.ToString() + "','" + row.Cells["Remarks"].Value.ToString() + "'," + 
                                       Common.Classes.clsMain.intUserId +
                                       "," + Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "')";
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Transport Amount.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string sqlQuery = "";
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {

                if (btnDelete.Text.ToString() == " &Delete")
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Delete  tblSal_Transport Where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtInput =  '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' and ComID = " + Common.Classes.clsMain.intComId + "";
                            arQuery.Add(sqlQuery);

                        }
                    }


                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Delete SuccessFuly");
                }   
                
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
            //cboEmp.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            cboEmp.DisplayMember = "EmpName";
            cboEmp.ValueMember = "EmpId";
        }


        private void btnCalculate_Click(object sender, EventArgs e)
        {

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
            {
                if (row.Cells["empid"].Text.ToString().Length != 0)
                {
                    //RowID = row.Index + 1;
                    ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                    row.Cells["Amount"].Value = txtAmount.Text.ToString();
                    row.Cells["Remarks"].Value = txtRemark.Text.ToString();


                }
            }
            
            //ArrayList arQuery = new ArrayList();
            //clsConnection clsCon = new clsConnection();

            //Int32 rowCount, TotalAmount;

            //try
            //{
            //    for (rowCount = 0; rowCount < dsList.Tables["Grid"].Rows.Count; rowCount++)
            //    {
            //        int Value = Convert.ToInt16(txtTimes.Text.ToString());
            //        gridDetails.Rows[rowCount].Cells[11].Value = Value.ToString();

            //        int Times = Convert.ToInt16(gridDetails.Rows[rowCount].Cells[11].Value);
            //        int Rate = Convert.ToInt16(gridDetails.Rows[rowCount].Cells[10].Value);

            //        TotalAmount = Rate * Times; gridDetails.Rows[rowCount].Cells[12].Value = TotalAmount.ToString();

            //    }


            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    clsCon = null;
            //}
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



        //private void gridDetails_AfterCellActivate(object sender, EventArgs e)
        //{
        //    int rate, times;
        //    for (int i = 0; i < gridDetails.Rows.Count; i++)
        //    {
        //        if (int.TryParse(gridDetails.Rows[i].Cells[10].Value.ToString(), out rate) && int.TryParse(gridDetails.Rows[i].Cells[11].Value.ToString(), out times))
        //        {
        //            int TotalAmount = rate * times; gridDetails.Rows[i].Cells[12].Value = TotalAmount.ToString();
        //        }
        //    } 
        //}



    }
}
