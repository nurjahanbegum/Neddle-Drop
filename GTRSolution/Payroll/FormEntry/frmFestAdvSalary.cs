using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmFestAdvSalary : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmFestAdvSalary(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                String sqlquary = "Exec [rptFestAdvanceSheet] " + Common.Classes.clsMain.intComId + ", '1','1', '1','1','1','1','1',6";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblBand";
                dsList.Tables[3].TableName = "tblGrid";

                gridProssType.DataSource = null;
                gridProssType.DataSource = dsList.Tables["ProssType"];

                gridSec.DataSource = null;
                gridSec.DataSource = dsList.Tables["tblSect"];

                gridBand.DataSource = null;
                gridBand.DataSource = dsList.Tables["tblBand"];

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["tblGrid"];


            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }       



        private void frmFestAdvSalary_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmFestAdvSalary_Load(object sender, EventArgs e)
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


        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 285;
            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "Process Type";

            //Change alternate color
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridProssType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridProssType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridProssType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridProssType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 180;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 125;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";

            //Change alternate color
            gridBand.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridBand.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridBand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridBand.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridBand.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }



        private void gridDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                gridDetails.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["ProssType"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["Amount"].Hidden = true;

                //Set Caption
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 60; 
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridDetails.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].Header.Caption = "Gross";
                gridDetails.DisplayLayout.Bands[0].Columns["BS"].Header.Caption = "Basic";
                gridDetails.DisplayLayout.Bands[0].Columns["NetPayable"].Header.Caption = "NetPayable";



                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 120;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].Width = 80;
                gridDetails.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 110;
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["BS"].Width = 70;
                gridDetails.DisplayLayout.Bands[0].Columns["NetPayable"].Width = 100;



                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["Band"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["dtJoin"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["GS"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["BS"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;


                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

        private void btnLoad_Click(object sender, EventArgs e)
        {

            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            string Band = "";
            string SectId = "0", EmpId = "0",ProssType = "";


            ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

            SectId = gridSec.ActiveRow.Cells["SectId"].Value.ToString();

            Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();


            try
            {
                sqlQuery = "Exec [rptFestAdvanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','1', '1','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','1','" + Band + "','1',7";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["tblGrid"];


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



        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "", sqlQuery1 = "";
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

                            sqlQuery = " Update tblFestAdvSalary Set NetPayable = '" + row.Cells["NetPayable"].Text.ToString()
                                                             + "',EditYN = '1' Where EmpID = '" + row.Cells["EmpID"].Text.ToString()
                                                             + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString()
                                                             + "' and ComID = " + Common.Classes.clsMain.intComId + "";
                            arQuery.Add(sqlQuery);

                            sqlQuery1 = "Exec [rptFestAdvanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','1', '1','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + row.Cells["EmpID"].Text.ToString() + "','1','1',8";
                            arQuery.Add(sqlQuery1);

                        }
                    }

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);



                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Update Successfully.");
                }

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Festival Advance Salary Amount.", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
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

                            sqlQuery = " Delete  tblFestAdvSalary Where EmpId = '" + row.Cells["empid"].Text.ToString() +
                                       "' and ProssType =  '" + row.Cells["ProssType"].Text.ToString() + "' and ComID = " + Common.Classes.clsMain.intComId + "";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            prcLoadList();

            btnDelete.Enabled = false;
            btnSave.Text = "&Save";

        }




      }
  }

