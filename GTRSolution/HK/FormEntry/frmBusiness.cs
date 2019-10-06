using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;
using System.Windows.Forms;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmBusiness : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmBusiness(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmBusiness_FormClosing(object sender, FormClosingEventArgs e)
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
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtNameBangla_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        
        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtNameBangla_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetCorporateBusiness 0,'Business'";
                clsCon.GTRFillDatasetWithSQLCommand( ref dsList, SqlQuery );

                dsList.Tables[0].TableName = "Business";
                dsList.Tables[1].TableName = "Corporate";
                
                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Business"];
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                clsCon = null;
            }
        }

        public void prcDisplayDetails( string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetCorporateBusiness " + Int32.Parse(strParam) + ",'Business'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "details";
                DataRow dr;

                if (dsDetail.Tables["details"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["details"].Rows[0];
                    txtId.Text = dr["BUId"].ToString();
                    txtName.Text = dr["BUName"].ToString();
                    this.cboCorporate.Value = dr["CPId"].ToString();


                    btnSave.Text = "&Update";
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

        public void prcLoadCombo()
        {

            cboCorporate.DataSource = null;
            cboCorporate.DataSource = dsList.Tables["Corporate"];

        }

        public void prcClearData()
        {
            txtId.Text = "";
            txtName.Text = "";
            cboCorporate.Value = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;
        }
        public Boolean fncBlank()
        {
            if (this.txtName .Text.Length == 0)
            {
                MessageBox.Show("Please provide Business Name.");
                txtName.Focus();
                return true;
            }

            if (this.cboCorporate.Text.Length == 0)
            {
                MessageBox.Show("Please provide Corporate Name.");
                cboCorporate.Focus();
                return true;
            }

            if (this.cboCorporate.IsItemInList() == false)
            {
                MessageBox.Show("Please provide valid Corporate [or, select from list item].");
                cboCorporate.Focus();
                return true;
            }
            
            return false;
        }

        private void frmBusiness_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();

                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
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
                if (btnSave.Text.ToString()!= "&Save")
                {
                    //Update     
                    sqlQuery = " Update tblCat_Business  Set BUName ='" + txtName.Text.ToString() + "',CPId = '" + cboCorporate.Value.ToString() + "',CPName ='" + cboCorporate.Text.ToString() + "'";
                    sqlQuery += " Where BUId = " + Int32.Parse(txtId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    sqlQuery = "Select Isnull(Max(BUId),0)+1 As NewId from tblCat_Business ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    sqlQuery = "Insert Into tblCat_Business(BUId,BUName, CPId, CPName, ComId, PCName, LUserId) ";
                    sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtName.Text.ToString() + "','" + cboCorporate.Value.ToString() + "','" + cboCorporate.Text.ToString() + "','" + Common.Classes.clsMain.intComId + "','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                    int add = arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }
                prcClearData();
                txtName.Focus();

                prcLoadList();
                prcLoadCombo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
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
                MessageBox.Show("Do you want to delete Business information of [" + txtName.Text + "]", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                //Delete Data
                sqlQuery = "Delete from tblCat_Business  Where BUId  = " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                txtName.Focus();

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            gridList.DisplayLayout.Bands[0].Columns["BUId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["CPId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["BUName"].Header.Caption = "Business Name";
            gridList.DisplayLayout.Bands[0].Columns["CPName"].Header.Caption = "Corporate Name";
           
            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["BUName"].Width = 235;
            gridList.DisplayLayout.Bands[0].Columns["CPName"].Width = 165;

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
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }


        private void txtId_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboCorporate_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboCorporate.DisplayLayout.Bands[0].Columns["CPName"].Width = cboCorporate.Width;
            cboCorporate.DisplayLayout.Bands[0].Columns["CPName"].Header.Caption = "Corporate";
            cboCorporate.DisplayLayout.Bands[0].Columns["CPId"].Hidden = true;
            cboCorporate.DisplayMember = "CPName";
            cboCorporate.ValueMember = "CPId";
        }
        
    }
}
