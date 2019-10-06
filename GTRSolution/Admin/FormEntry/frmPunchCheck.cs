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

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmPunchCheck : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmPunchCheck(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmPunchCheck_FormClosing(object sender, FormClosingEventArgs e)
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
            try
            {
                string sqlQuery = "Exec [prcGetEmployeePunch] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                //Tab : Basic
                dsList.Tables[0].TableName = "tblempid";
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
            cboCode.DataSource = null;
            cboCode.DataSource = dsList.Tables["tblempid"];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnPunch_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcProcessPunchCheck " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + cboCode.Text.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                if (dsList.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                }

                dsList.Tables[0].TableName = "Punch";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Punch"];

                prcLoadList();
                prcLoadCombo();
                               
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

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 75; //Short Name
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 150; //Country Name
                gridList.DisplayLayout.Bands[0].Columns["CardNo"].Width = 105; //Shift
                gridList.DisplayLayout.Bands[0].Columns["dtPunchtime"].Width = 95;  //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp Code";
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["CardNo"].Header.Caption = "Card No";
                gridList.DisplayLayout.Bands[0].Columns["dtPunchtime"].Header.Caption = "Punch time";


                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //gridList.DisplayLayout.Override.AllowMultiCellOperations = AllowMultiCellOperation.All;

              

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                ////Stop Updating-Asad
                //this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

                ////RowHeight
                gridList.DisplayLayout.Override.DefaultRowHeight = 20;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboCode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCode.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;

            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            cboCode.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";

            cboCode.DisplayLayout.Bands[0].Columns["empName"].Width = 120;
            cboCode.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Name";

            cboCode.DisplayMember = "empCode";
            cboCode.ValueMember = "empId";
        }

        private void frmPunchCheck_Load(object sender, EventArgs e)
        {
            prcLoadList();
            prcLoadCombo();
        }


    }
}
