using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using GTRHRIS.Common.Classes;




namespace GTRHRIS.TManagement
{
    public partial class frmTalManagement : Form
    {
        
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;
        clsConnection clsCon=new clsConnection();
        private clsProcedure clsProc=new clsProcedure();
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsdisplayDetails;
        private clsMain clsm = new clsMain(); 
        
                
        public frmTalManagement(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                                Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;// private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab; (declare korta hoba)
            FM = fm;//private Common.FormEntry.frmMaster FM; (declare korta hoba)
        }


        private void frmTalManagement_Load_1(object sender, EventArgs e)
        {
            try
            {

                prcLoadList();
                prcloadCombo();
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
                string sqlQuery = "[Exec prcGetTal_EmpBeha]" + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblDesignation";
                dsList.Tables[2].TableName = "tblSection";
                dsList.Tables[3].TableName = "tblTalEmpBeha";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGride"];

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

        private void prcloadCombo()
        {
            try
            {
                cboCode.DataSource = null;
                cboCode.DataSource = dsList.Tables["tblDesignation"];
                cboDesignation.DataSource = null;
                cboDesignation.DataSource = dsList.Tables["tblSection"];
                cboSection.DataSource = null;
                cboSection.DataSource = dsList.Tables["tblTalEmpBeha"];

                cboDatetime.Value = DateTime.Today;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            
        }

        private void prcDisplayDetails(string strparam)
        {
           
            dsdisplayDetails = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new clsConnection();
            string sqlQuery = "", sqlQuery1 = "";
            try
            {
                sqlQuery = "[Exec prcGetTal_EmpBeha]" + Common.Classes.clsMain.intComId + "  ,"+Int32.Parse(strparam)+" ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList,sqlQuery);

                dsdisplayDetails.Tables[0].TableName = "Details";
                DataRow drow;
                if (dsdisplayDetails.Tables["Details"].Rows.Count > 0)
                {
                    drow = dsdisplayDetails.Tables["Details"].Rows[0];
                    this.txtID.Text = drow["TalendID"].ToString();
                    this.cboCode.Value = drow["EmpId"].ToString();
                    this.cboName.Text = drow["EmpName"].ToString();
                    this.cboDesignation.Value = drow["DesigID"].ToString();
                    this.cboSection.Value = drow["SectId"].ToString();
                    this.txtRemarks.Text = drow["Remarks"].ToString();
                    this.cboDatetime.Value = drow["dtDate"];
                    this.cboBehaviour.Text = drow["Behaviour"].ToString();
                    this.cboBehResult.Text = drow["[Behaviour Result]"].ToString();
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Grid Width
                gridList.DisplayLayout.Bands[0].Columns["TalendID"].Width =70 ;
                gridList.DisplayLayout.Bands[0].Columns["Empid"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70;
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 100;
                gridList.DisplayLayout.Bands[0].Columns["EmpDesig"].Width = 100;
                gridList.DisplayLayout.Bands[0].Columns["EmpSect"].Width = 100;
                gridList.DisplayLayout.Bands[0].Columns["dtDate"].Width = 90;
                gridList.DisplayLayout.Bands[0].Columns["Behaviour"].Width = 100;
                gridList.DisplayLayout.Bands[0].Columns["Behaviour Result"].Width = 100;

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["TalendID"].Header.Caption = "Talend ID";
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                gridList.DisplayLayout.Bands[0].Columns["EmpDesig"].Header.Caption = "Employee Designation";
                gridList.DisplayLayout.Bands[0].Columns["EmpSect"].Header.Caption = "Employee Section";
                gridList.DisplayLayout.Bands[0].Columns["dtDate"].Header.Caption = "Date";
                gridList.DisplayLayout.Bands[0].Columns["Behaviour"].Header.Caption = "Behaviour";
                gridList.DisplayLayout.Bands[0].Columns["Behaviour Result"].Header.Caption = "Behaviour Result";

                this.gridList.DisplayLayout.Bands[0].Columns["dtDate"].Format = "dd-MMM-yyyy";

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Using Filter
                e.Layout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void prcCleareData()
        {
            txtID.Text = "";
            cboCode.Value = "";
            cboName.Value = " ";
            cboDesignation.Value = "";
            cboSection.Value = "";
            cboDatetime.Value = "";
            txtRemarks.Text = "";
            cboBehaviour.Value = "";
            cboBehResult.Value = "";
            
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cboCode_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboCode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboName_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboDesignation_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboDesignation_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboSection_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboSection_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboDatetime_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboDatetime_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboBehaviour_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboBehaviour_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboBehResult_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cboBehResult_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
