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
    public partial class frmDepartment : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmDepartment(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmDepartment_FormClosing(object sender, FormClosingEventArgs e)
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
            //e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
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
                string SqlQuery = "Exec prcGetDept '" + Common.Classes.clsMain.intComId + "',0";
                clsCon.GTRFillDatasetWithSQLCommand( ref dsList, SqlQuery );
                dsList.Tables[0].TableName = "Department";
                //dsList.Tables[1].TableName = "Business";
                
                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Department"];
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
                string SqlQuery = "Exec prcGetDept '" + Common.Classes.clsMain.intComId + "'," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "details";
                DataRow dr;

                if (dsDetail.Tables["details"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["details"].Rows[0];
                    txtId.Text = dr["DeptId"].ToString();
                    txtName.Text = dr["DeptName"].ToString();
                    txtNameB.Text = dr["DeptBangla"].ToString();
                    txtSlNo.Text = dr["SLNO"].ToString();
                    //cboBusiness.Text = dr["BUId"].ToString();
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

        //    cboBusiness.DataSource = null;
        //    cboBusiness.DataSource = dsList.Tables["Business"];

        }

        public void prcClearData()
        {
            txtId.Text = "";
            txtNameB.Text = "";
            txtName.Text = "";
            txtSlNo.Text = "0";
            //cboBusiness.Text = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;
        }
        public Boolean fncBlank()
        {
            if (this.txtName .Text.Length == 0)
            {
                MessageBox.Show("Please provide Department Name.");
                txtName.Focus();
                return true;
            }

            //if (this.cboBusiness.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide Business Name.");
            //    cboBusiness.Focus();
            //    return true;
            //}

            //if (this.cboBusiness.IsItemInList() == false)
            //{
            //    MessageBox.Show("Please provide valid Business [or, select from list item].");
            //    cboBusiness.Focus();
            //    return true;
            //}
            
            return false;
        }

        private void frmDepartment_Load(object sender, EventArgs e)
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
                    sqlQuery = " Update tblCat_Department  Set DeptName ='" + txtName.Text.ToString() + "', DeptBangla='" + txtNameB.Text.ToString() + "' , SLNO= '" + txtSlNo.Text.ToString() + "',ParentId = '1' ";
                    sqlQuery += " Where DeptId = " + Int32.Parse(txtId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    //sqlQuery = " Update D Set D.BUName = B.BUName from tblCat_Department D,tblCat_Business B Where D.BUId = B.BuId and D.DeptId = " + Int32.Parse(txtId.Text) + "";
                    //arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    sqlQuery = "Select Isnull(Max(DeptId),0)+1 As NewId from tblCat_Department ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    sqlQuery = "Insert Into tblCat_Department(SLNo, DeptId, aId,ComId,DeptName, DeptBangla,ParentId, PCName, LUserId) ";
                    sqlQuery = sqlQuery + " Values ('" + txtSlNo.Text.ToString() + "'," + NewId + "," + NewId + ",'" + Common.Classes.clsMain.intComId + "', '" + txtName.Text.ToString() + "', '" + txtNameB.Text.ToString() + "','1','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
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
                MessageBox.Show("Do you want to delete Department information of [" + txtName.Text + "]", "",
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
                sqlQuery = "Delete from tblCat_Department  Where DeptId  = " + Int32.Parse(txtId.Text);
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

            gridList.DisplayLayout.Bands[0].Columns["DeptId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = false;
            //gridList.DisplayLayout.Bands[0].Columns["BUId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Department";
            gridList.DisplayLayout.Bands[0].Columns["DeptBangla"].Header.Caption = "DeptBName";
            //gridList.DisplayLayout.Bands[0].Columns["BUName"].Header.Caption = "Business Unit";
           
            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["DeptName"].Width = 175;
            gridList.DisplayLayout.Bands[0].Columns["DeptBangla"].Width = 170;
            //gridList.DisplayLayout.Bands[0].Columns["BUName"].Width = 165;

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



        private void txtNameB_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSlNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        //private void cboBusiness_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    cboBusiness.DisplayLayout.Bands[0].Columns["BUName"].Width = cboBusiness.Width;
        //    cboBusiness.DisplayLayout.Bands[0].Columns["BUName"].Header.Caption = "Business Unit";
        //    cboBusiness.DisplayLayout.Bands[0].Columns["BUId"].Hidden = true;
        //    cboBusiness.DisplayMember = "BUName";
        //    cboBusiness.ValueMember = "BUId";
        //}
        
    }
}
