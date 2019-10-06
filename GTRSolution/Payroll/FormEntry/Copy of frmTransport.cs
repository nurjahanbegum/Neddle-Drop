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
    public partial class frmTransport : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private DataView dvStyle;
        private DataView dvSpec;
        private DataView dvColor;

        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmTransport(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmTransport_Load(object sender, EventArgs e)
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
                sqlQuery = "Exec prcGetTransport " + Common.Classes.clsMain.intComId + ", 0, 0,'','','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtDate.Value.ToString()) + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblBand";
                dsList.Tables[3].TableName = "tblEmp";


                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["Grid"];

                this.dtDate.Value = DateTime.Now;

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
            txtTimes.Text = "";
            cboSec.Text = "";
            cboBand.Text = "";
            cboEmp.Text = "";

            checkBox2.Checked = false;

            this.dtDate.Value = DateTime.Now;
            this.dtFrom.Value = DateTime.Now;

            btnDelete.Enabled = false;
            btnSave.Text = "&Save";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTransport_FormClosing(object sender, FormClosingEventArgs e)
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
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 50; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].Header.Caption = "Gross";
                gridDetails.DisplayLayout.Bands[0].Columns["BusStop"].Header.Caption = "Bus Stoppage";
                gridDetails.DisplayLayout.Bands[0].Columns["Present"].Header.Caption = "Present";
                gridDetails.DisplayLayout.Bands[0].Columns["Trn"].Header.Caption = "Rate";
                gridDetails.DisplayLayout.Bands[0].Columns["Times"].Header.Caption = "Times";
                gridDetails.DisplayLayout.Bands[0].Columns["TotalAmount"].Header.Caption = "Total Amount";

                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 100;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 140;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].Width = 100;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Width = 100;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 80;
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].Width = 60;
                gridDetails.DisplayLayout.Bands[0].Columns["BusStop"].Width = 150;
                gridDetails.DisplayLayout.Bands[0].Columns["Present"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["Trn"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["Times"].Width = 60;
                gridDetails.DisplayLayout.Bands[0].Columns["TotalAmount"].Width = 100;

                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["DesigName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["BusStop"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Present"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Trn"].CellActivation = Activation.NoEdit;

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
                sqlQuery = "Exec prcGetTransport " + Common.Classes.clsMain.intComId + ", " + EmpId + "," + SectId + ",'" + Band + "','" + optCriteria.Value.ToString() + "','" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtDate.Value.ToString()) + "'";
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
                            //RowID = row.Index + 1;
                            ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                            sqlQuery = " Delete  tblProssAllowanceBill where empid = '" + row.Cells["empid"].Text.ToString() +
                                       "' and dtDate =  '" + clsProc.GTRDate(dtDate.Value.ToString()) + "' and ComID = " + Common.Classes.clsMain.intComId + " and AllowType = 'Transport'";
                            arQuery.Add(sqlQuery);


                            sqlQuery = " Insert Into tblProssAllowanceBill(EmpId,EmpName,GS,Present,Rate,Times,TotalAmount,dtDate,ProssType,AllowType,Luserid,comid,pcname) "
                                       + " Values ('" + row.Cells["empid"].Text.ToString() + "', '" +
                                       row.Cells["EmpName"].Text.ToString() + "','" +
                                       row.Cells["GS"].Text.ToString() + "','" +
                                       row.Cells["Present"].Text.ToString() + "','" +
                                       row.Cells["Trn"].Text.ToString() + "','" +
                                       row.Cells["Times"].Value.ToString() + "','" +
                                       row.Cells["TotalAmount"].Value.ToString() + "','" +
                                       clsProc.GTRDate(dtDate.Value.ToString()) + "',('" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'+' To '+'" + 
                                       clsProc.GTRDate(dtDate.Value.ToString()) + "'),'Transport'," + Common.Classes.clsMain.intUserId +
                                       "," + Common.Classes.clsMain.intComId + ",'" +
                                       Common.Classes.clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);

                        }
                    }

                    sqlQuery = "Update A Set A.SectID = E.SectID,A.DesigID = E.DesigID, A.DeptID = E.DeptID, A.Band = E.Band"
                               + " from tblProssAllowanceBill A,tblEmp_info E Where A.EmpID = E.EmpID and A.ComID = " + Common.Classes.clsMain.intComId
                               + " and A.dtDate = '" + clsProc.GTRDate(dtDate.Value.ToString())
                               + "' and A.ProssType = ('" + clsProc.GTRDate(dtFrom.Value.ToString()) + "'+' To '+'" + clsProc.GTRDate(dtDate.Value.ToString()) + "') and A.AllowType = 'Transport'";
                    arQuery.Add(sqlQuery);


                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','Insert')";
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
 

            if (dtDate.Text.Length == 0)
            {
                MessageBox.Show("Please provide requisition date.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtDate.Focus();
                return true;
            }
    
            
            //Check In Grid
            Int32 intAccEnter = 0;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
            {
                if (row.Cells["EmpID"].Text.ToString().Length > 0)
                {
                    intAccEnter++;

                    if (row.Cells["Times"].Text.ToString().Length == 0)
                    {
                        MessageBox.Show("Please provide Value");
                        row.Cells["Times"].Activate();
                        return true;
                    }
                    
                }
            }

            return false;


        }


        private void cboStyle_Validating(object sender, CancelEventArgs e)
        {
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete SRR information of [" + txtTimes.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string sqlQuery = "";
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection("");
            try
            {
                //sqlQuery = "Update tblStr_Issue_Main Set Isposted=2 where IssueId=" +
                //              int.Parse(txtsrid.Text.ToString()) + "";
                //arQuery.Add(sqlQuery);

                //sqlQuery = "Exec prcProcessStoreUnPost '" + clsProc.GTRDate(dtDate.Value.ToString()) + "'," +
                //          Common.Classes.clsMain.intComId + "";
                //arQuery.Add(sqlQuery);

                sqlQuery = "Delete from tblStr_Issue_Main  Where IssueId = " + Int64.Parse(txtTimes.Text.ToString()) + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into GTRHRIS.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Delete SuccessFuly");
                
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
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            Int32 rowCount, TotalAmount;

            try
            {
                for (rowCount = 0; rowCount < dsList.Tables["Grid"].Rows.Count; rowCount++)
                {
                    int Value = Convert.ToInt16(txtTimes.Text.ToString());
                    gridDetails.Rows[rowCount].Cells[11].Value = Value.ToString();

                    int Times = Convert.ToInt16(gridDetails.Rows[rowCount].Cells[11].Value);
                    int Rate = Convert.ToInt16(gridDetails.Rows[rowCount].Cells[10].Value);

                    TotalAmount = Rate * Times; gridDetails.Rows[rowCount].Cells[12].Value = TotalAmount.ToString();

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



        private void gridDetails_AfterCellActivate(object sender, EventArgs e)
        {
            int rate, times;
            for (int i = 0; i < gridDetails.Rows.Count; i++)
            {
                if (int.TryParse(gridDetails.Rows[i].Cells[10].Value.ToString(), out rate) && int.TryParse(gridDetails.Rows[i].Cells[11].Value.ToString(), out times))
                {
                    int TotalAmount = rate * times; gridDetails.Rows[i].Cells[12].Value = TotalAmount.ToString();
                }
            } 
        }



    }
}
