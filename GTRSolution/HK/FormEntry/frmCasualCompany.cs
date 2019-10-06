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
    public partial class frmCasualCompany : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmCasualCompany(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmCasualCompany_FormClosing(object sender, FormClosingEventArgs e)
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
                string SqlQuery = "Exec prcGetCompanyCasual 0," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand( ref dsList, SqlQuery );
                
                dsList.Tables[0].TableName = "Company";
                
                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Company"];
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
                string SqlQuery = "Exec prcGetCompanyCasual  " + Int32.Parse(strParam) + "," + Common.Classes.clsMain.intComId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "details";
                DataRow dr;

                if (dsDetail.Tables["details"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["details"].Rows[0];
                    txtId.Text = dr["CSComId"].ToString();
                    txtName.Text = dr["CSComName"].ToString();
                    txtAdd.Text = dr["CSComAddress"].ToString();
                    txtPhone.Text = dr["CSComPhone"].ToString();
                   // txtNameB.Text = dr["DesigNameB"].ToString();


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

        }

        public void prcClearData()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtAdd.Text = "";
            txtPhone.Text = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;
        }
        public Boolean fncBlank()
        {
            if (this.txtName .Text.Length == 0)
            {
                MessageBox.Show("Please provide Casual Worker Company Name.");
                txtName.Focus();
                return true;
            }
            else if (this.txtAdd.Text.Length == 0)
            {
                MessageBox.Show("Please provide Casual Worker Company Address.");
                txtAdd.Focus();
                return true;
            }
            else if (this.txtPhone.Text.Length == 0)
            {
                MessageBox.Show("Please provide Casual Worker Phone Number.");
                txtPhone.Focus();
                return true;
            }
            
            return false;
        }

        private void frmCasualCompany_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
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
                    sqlQuery = " Update tblCat_CompanyCasual  Set CSComName ='" + txtName.Text.ToString() + "', CSComAddress='" + txtAdd.Text.ToString() + "',CSComPhone='" + txtPhone.Text.ToString() + "' ";
                    sqlQuery += " Where CSComId = " + Int32.Parse(txtId.Text);
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
                    sqlQuery = "Select Isnull(Max(CSComId),0)+1 As NewId from tblCat_CompanyCasual ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    sqlQuery = "Insert Into tblCat_CompanyCasual(ComId,CSComId,CSComName, CSComAddress,CSComPhone, PCName, LUserId) ";
                    sqlQuery = sqlQuery + " Values (" + Common.Classes.clsMain.intComId + "," + NewId + ",'" + txtName.Text.ToString() + "', '" + txtAdd.Text.ToString() + "','" + txtPhone.Text.ToString() + "','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
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
                MessageBox.Show("Do you want to delete Company information of [" + txtName.Text + "]", "",
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
                sqlQuery = "Delete from tblCat_CompanyCasual  Where CSComId  = " + Int32.Parse(txtId.Text) + " and ComID = " + Common.Classes.clsMain.intComId + " ";
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

            gridList.DisplayLayout.Bands[0].Columns["CSComId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["CSComPhone"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["CSComName"].Header.Caption = "Company Name";
            gridList.DisplayLayout.Bands[0].Columns["CSComAddress"].Header.Caption = "Company Address";
           
            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["CSComName"].Width = 200;
            gridList.DisplayLayout.Bands[0].Columns["CSComAddress"].Width = 200;

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
        
    }
}
