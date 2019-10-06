using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;


namespace GTRHRIS.HK.FormEntry
{
    public partial class frmShiftInput : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        //private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;


        public frmShiftInput(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmShiftInput_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtSName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetShiftInput " + Common.Classes.clsMain.intComId + ",0,0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "tblshift";
                dsList.Tables[1].TableName = "tblSec";
                dsList.Tables[2].TableName = "tblEmp";
                dsList.Tables[3].TableName = "tblBand";
                dsList.Tables[4].TableName = "tblShiftCode";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblshift"];

                gridSec.DataSource = null;
                gridSec.DataSource = dsList.Tables["tblSec"];

                gridEmp.DataSource = null;
                gridEmp.DataSource = dsList.Tables["tblEmp"];

                gridBand.DataSource = null;
                gridBand.DataSource = dsList.Tables["tblBand"];

                gridShift.DataSource = null;
                gridShift.DataSource = dsList.Tables["tblShiftCode"];

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

            try
            {
                //cboShiftType.DataSource = null;
                //cboShiftType .DataSource = dsList.Tables["Country"];
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetDistrict " + Int32.Parse(strParam);
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "District";
                DataRow dr;

                if (dsDetail.Tables["District"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["District"].Rows[0];
                    //txtId.Text = dr["DistId"].ToString();
                    //cboShiftType.Text = dr["ShiftType"].ToString();

                    btnSave.Text = " &Update";
                    btnDelete.Enabled = true;
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


        public void prcClearData()
        {
            //txtId.Text = "";
            //txtName.Text = "";

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
        }

        private void frmShiftInput_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
                gridEmp.Enabled = true;


                if (Common.Classes.clsMain.intComId == 2 || Common.Classes.clsMain.intComId == 3 || Common.Classes.clsMain.intComId == 5)
                {
                    chkSave.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridList.DisplayLayout.Bands[0].Columns["ShiftID"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["ShiftName"].Header.Caption = "Shift Name";
            gridList.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Shift Type";
            gridList.DisplayLayout.Bands[0].Columns["ShiftCat"].Header.Caption = "Shift Category";
            gridList.DisplayLayout.Bands[0].Columns["ShiftIn"].Header.Caption = "Shift In";
            gridList.DisplayLayout.Bands[0].Columns["ShiftOut"].Header.Caption = "Shift Out";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["ShiftName"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["ShiftType"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["ShiftCat"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["ShiftIn"].Width = 70;
            gridList.DisplayLayout.Bands[0].Columns["ShiftOut"].Width = 70;

            //Change alternate color
            gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboShiftType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //hide Column
            //cboShiftType.DisplayLayout.Bands[0].Columns["ShiftID"].Hidden = true;

            //set Caption
            //cboShiftType.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Country";

            //set Width
            //cboShiftType.DisplayLayout.Bands[0].Columns["ShiftType"].Width  = cboShiftType.Width;

            //initialize members
            //cboShiftType.DisplayMember = "ShiftType";
            //cboShiftType.ValueMember = "ShiftID";

        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        public Boolean fncBlank()
        {
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            try
            {
                //Get Shift Id
                string ShiftId = gridList.ActiveRow.Cells["ShiftId"].Value.ToString();

                //Get Sect Id
                string SectId = gridSec.ActiveRow.Cells["SectId"].Value.ToString();

                string Band = gridBand.ActiveRow.Cells["VarName"].Value.ToString();

                if (chkSave.Checked == true)
                {

                    //Delete Existing Data
                    if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                    {
                        sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                        arQuery.Add(sqlQuery);
                    }

                    else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                    {
                        sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                  " and EmpId in (Select EmpId from tblEmp_Info Where SectId=" + SectId + ")";
                        arQuery.Add(sqlQuery);
                    }

                    else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                    {
                        sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                  " and EmpId in (Select EmpId from tblEmp_Info Where Band='" + Band + "')";
                        arQuery.Add(sqlQuery);
                    }

                    else
                    {
                        foreach (UltraGridRow row in this.gridEmp.Rows)
                        {
                            if (row.Cells["Chk"].Value.ToString() == "1")
                            {
                                sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                          " and EmpId = " + row.Cells["EmpId"].Text.ToString() + " ";
                                arQuery.Add(sqlQuery);
                            }
                        }
                    }

                    //Loop Date wise
                    for (DateTime dtStart = dtDateFrom.DateTime; dtDateTo.DateTime >= dtStart; dtStart = dtStart.AddDays(1))
                    {
                        //Insert Data Based On Criteria
                        if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                        {
                            //Insert Data
                            sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate,  ShiftId, PCName, LUserId) " +
                                " Select ComId, EmpId, '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + "  from tblEmp_Info " +
                                " Where ComID = " + Common.Classes.clsMain.intComId + " And IsInactive = 0";
                            arQuery.Add(sqlQuery);

                            //Update Data
                            sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And IsInactive = 0";
                            arQuery.Add(sqlQuery);
                        }
                        else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                        {
                            ////Delete Existing Data
                            //sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' and SectId ="+SectId+"";
                            //arQuery.Add(sqlQuery);

                            sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate, ShiftId, PCName, LUserId) " +
                                " Select ComId, EmpId, '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + "  from tblEmp_Info " +
                                " Where ComID = " + Common.Classes.clsMain.intComId + " and " +
                                " SectId = " + SectId + " And IsInactive = 0";
                            arQuery.Add(sqlQuery);

                            //Update Data
                            sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And SectId = " + SectId + " And IsInactive = 0";
                            arQuery.Add(sqlQuery);
                        }

                        else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                        {
                            ////Delete Existing Data
                            //sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' and SectId ="+SectId+"";
                            //arQuery.Add(sqlQuery);

                            sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate, ShiftId, PCName, LUserId) " +
                                " Select ComId, EmpId, '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + "  from tblEmp_Info " +
                                " Where ComID = " + Common.Classes.clsMain.intComId + " and " +
                                " Band = '" + Band + "' And IsInactive = 0";
                            arQuery.Add(sqlQuery);

                            //Update Data
                            sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And Band = '" + Band + "' And IsInactive = 0";
                            arQuery.Add(sqlQuery);
                        }
                        else
                        {
                            foreach (UltraGridRow row in this.gridEmp.Rows)
                            {
                                if (row.Cells["Chk"].Value.ToString() == "1")
                                {
                                    sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate,  ShiftId, PCName, LUserId) " +
                                        " Values(" + Common.Classes.clsMain.intComId + ", " + row.Cells["EmpId"].Text.ToString() + ", '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + ")";
                                    arQuery.Add(sqlQuery);

                                    //Update Data
                                    sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And EmpID = " + row.Cells["EmpId"].Text.ToString() + " And IsInactive = 0";
                                    arQuery.Add(sqlQuery);

                                }
                            }

                        }

                    }
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Updated Successfully.");

                    RowUncheck();
                }

                //Checkbox uncheck Shift Data not update Employee information

                else

                {

                        //Delete Existing Data
                        if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                        {
                            sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                            arQuery.Add(sqlQuery);
                        }

                        else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                        {
                            sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                      " and EmpId in (Select EmpId from tblEmp_Info Where SectId=" + SectId + ")";
                            arQuery.Add(sqlQuery);
                        }

                        else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                        {
                            sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                      " and EmpId in (Select EmpId from tblEmp_Info Where Band='" + Band + "')";
                            arQuery.Add(sqlQuery);
                        }

                        else
                        {
                            foreach (UltraGridRow row in this.gridEmp.Rows)
                            {
                                if (row.Cells["Chk"].Value.ToString() == "1")
                                {
                                    sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                              " and EmpId = " + row.Cells["EmpId"].Text.ToString() + " ";
                                    arQuery.Add(sqlQuery);
                                }
                            }
                        }

                        //Loop Date wise
                        for (DateTime dtStart = dtDateFrom.DateTime; dtDateTo.DateTime >= dtStart; dtStart = dtStart.AddDays(1))
                        {
                            //Insert Data Based On Criteria
                            if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                            {
                                //Insert Data
                                sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate,  ShiftId, PCName, LUserId) " +
                                    " Select ComId, EmpId, '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + "  from tblEmp_Info " +
                                    " Where ComID = " + Common.Classes.clsMain.intComId + " And IsInactive = 0";
                                arQuery.Add(sqlQuery);

                                //Update Data
                                //sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And IsInactive = 0";
                                //arQuery.Add(sqlQuery);
                            }
                            else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                            {
                                ////Delete Existing Data
                                //sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' and SectId ="+SectId+"";
                                //arQuery.Add(sqlQuery);

                                sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate, ShiftId, PCName, LUserId) " +
                                    " Select ComId, EmpId, '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + "  from tblEmp_Info " +
                                    " Where ComID = " + Common.Classes.clsMain.intComId + " and " +
                                    " SectId = " + SectId + " And IsInactive = 0";
                                arQuery.Add(sqlQuery);

                                //Update Data
                                //sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And SectId = " + SectId + " And IsInactive = 0";
                                //arQuery.Add(sqlQuery);
                            }

                            else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                            {
                                ////Delete Existing Data
                                //sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' and SectId ="+SectId+"";
                                //arQuery.Add(sqlQuery);

                                sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate, ShiftId, PCName, LUserId) " +
                                    " Select ComId, EmpId, '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + "  from tblEmp_Info " +
                                    " Where ComID = " + Common.Classes.clsMain.intComId + " and " +
                                    " Band = '" + Band + "' And IsInactive = 0";
                                arQuery.Add(sqlQuery);

                                //Update Data
                                //sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And Band = '" + Band + "' And IsInactive = 0";
                                //arQuery.Add(sqlQuery);
                            }
                            else
                            {
                                foreach (UltraGridRow row in this.gridEmp.Rows)
                                {
                                    if (row.Cells["Chk"].Value.ToString() == "1")
                                    {
                                        sqlQuery = "Insert Into tblEmp_Shift (ComId, EmpId, dtDate,  ShiftId, PCName, LUserId) " +
                                            " Values(" + Common.Classes.clsMain.intComId + ", " + row.Cells["EmpId"].Text.ToString() + ", '" + clsProc.GTRDate(dtStart.Date.ToString()) + "', " + ShiftId + ", '" + Common.Classes.clsMain.strComputerName + "', " + Common.Classes.clsMain.intUserId + ")";
                                        arQuery.Add(sqlQuery);

                                        //Update Data
                                        //sqlQuery = "Update tblEmp_Info Set ShiftId = " + ShiftId + " Where ComID = " + Common.Classes.clsMain.intComId + " And EmpID = " + row.Cells["EmpId"].Text.ToString() + " And IsInactive = 0";
                                        //arQuery.Add(sqlQuery);

                                    }
                                }

                            }

                        }
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);
                        MessageBox.Show("Data Updated Successfully.");

                        RowUncheck();
               }

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

        public void RowUncheck()
        {
            foreach (UltraGridRow row in this.gridEmp.Rows)
            {
                if (row.Cells["Chk"].Value.ToString() == "1")
                    row.Cells["Chk"].Value = 0;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete employee shift information. ", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                //Get Shift Id
                string ShiftId = gridList.ActiveRow.Cells["ShiftId"].Value.ToString();

                //Get Sect Id
                string SectId = gridSec.ActiveRow.Cells["SectId"].Value.ToString();

                //Get Band
                string Band = gridBand.ActiveRow.Cells["VarName"].Value.ToString();

                //Get Again Shift Code
                string ShiftCode = gridShift.ActiveRow.Cells["ShiftId"].Value.ToString();

                //Delete Data
                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                    sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "'";
                    arQuery.Add(sqlQuery);
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                {
                    sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                              " and EmpId in (Select EmpId from tblEmp_Info Where SectId=" + SectId + ")";
                    arQuery.Add(sqlQuery);
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                {
                    sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                              " and EmpId in (Select EmpId from tblEmp_Info Where Band='" + Band + "')";
                    arQuery.Add(sqlQuery);
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Shift".ToUpper())
                {
                    sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                              " and EmpId in (Select EmpId from tblEmp_Info Where ShiftId='" + ShiftCode + "')";
                    arQuery.Add(sqlQuery);
                }

                else
                {
                    foreach (UltraGridRow row in this.gridEmp.Rows)
                    {
                        if (row.Cells["Chk"].Value.ToString() == "1")
                        {
                            sqlQuery = "Delete From tblEmp_Shift Where ComId = " + Common.Classes.clsMain.intComId + " And dtDate Between '" + clsProc.GTRDate(dtDateFrom.DateTime.ToString()) + "' and '" + clsProc.GTRDate(dtDateTo.DateTime.ToString()) + "' " +
                                      " and EmpId = " + row.Cells["EmpId"].Text.ToString() + " ";
                            arQuery.Add(sqlQuery);
                        }
                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                //txtName.Focus();

                prcLoadList();
                // prcLoadCombo();

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

        private void cboShiftType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void gridSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;

            //Set Caption
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section Name";

            //Set Width
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = gridSec.Width - 30;

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Using Filter
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            //Hide Column
            gridEmp.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;

            //Set Caption
            gridEmp.DisplayLayout.Bands[0].Columns["Chk"].Header.Caption = "Check";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee ID";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Set Width
            gridEmp.DisplayLayout.Bands[0].Columns["chk"].Width = 50;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 300;

            gridEmp.DisplayLayout.Bands[0].Columns["Chk"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

            //Change alternate color
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            //this.gridEmp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //this.gridEmp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridEmp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            this.gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
            this.gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Using Filter
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }


        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {
            gridSec.Enabled = false;
            gridEmp.Enabled = true;
            gridBand.Enabled = false;

            if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
            {
                gridSec.Enabled = false;
                gridEmp.Enabled = false;
                gridBand.Enabled = false;
                gridShift.Enabled = false;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
            {
                gridEmp.Enabled = false;
                gridBand.Enabled = false;
                gridSec.Enabled = true;
                gridShift.Enabled = false;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
            {
                gridEmp.Enabled = true;
                gridSec.Enabled = false;
                gridBand.Enabled = false;
                gridShift.Enabled = false;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
            {
                gridBand.Enabled = true;
                gridSec.Enabled = false;
                gridEmp.Enabled = false;
                gridShift.Enabled = false;
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Shift".ToUpper())
            {
                gridBand.Enabled = false;
                gridSec.Enabled = false;
                gridEmp.Enabled = false;
                gridShift.Enabled = true;
            }
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridBand.DisplayLayout.Bands[0].Columns["VarId"].Hidden = true;

            //Set Caption
            gridBand.DisplayLayout.Bands[0].Columns["VarName"].Header.Caption = "Band";

            //Set Width
            gridBand.DisplayLayout.Bands[0].Columns["VarName"].Width = gridBand.Width - 30;

            //Change alternate color
            gridBand.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridBand.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridBand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridBand.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridBand.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Using Filter
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridShift_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridShift.DisplayLayout.Bands[0].Columns["ShiftID"].Hidden = true;

            //Set Caption
            gridShift.DisplayLayout.Bands[0].Columns["ShiftName"].Header.Caption = "Shift Name";

            //Set Width
            gridShift.DisplayLayout.Bands[0].Columns["ShiftName"].Width = 200;

            //Change alternate color
            gridShift.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridShift.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridShift.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridShift.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridShift.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            gridEmp.Enabled = true;


            //Collecting Parameter Value

                string ShiftCode = gridShift.ActiveRow.Cells["ShiftId"].Value.ToString();


            try
            {
                string SqlQuery = "Exec prcGetShiftInput " + Common.Classes.clsMain.intComId + ",1,'" + ShiftCode + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);

                dsList.Tables[0].TableName = "tblEmployee";

                gridEmp.DataSource = null;
                gridEmp.DataSource = dsList.Tables["tblEmployee"];

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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridEmp.Rows)
                {
                    row.Cells["Chk"].Value = 1;
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridEmp.Rows)
                {
                    row.Cells["Chk"].Value = 0;
                }
            }
        }

        private void chkSave_CheckedChanged(object sender, EventArgs e)
        {
            chkSave.Tag = 0;
            if (chkSave.Checked == true)
            {
                chkSave.Tag = 1;
            }
        }


    }
}
