using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using GTRLibrary;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmDesigCapacity : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();
        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private int secId_update = 0; // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmDesigCapacity(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmDesigCapacity_FormClosing(object sender, FormClosingEventArgs e)
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

        public Boolean fncBlank()
        {

            if (cboDept .Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Department.");
                cboDept.Focus();
                return true;
            }

            if (cboSection.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Section.");
                cboSection.Focus();
                return true;
            }


            return false;
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetDesigCapacity  " + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblDesigCapacity";
                dsList.Tables[1].TableName = "tblDept";
                dsList.Tables[2].TableName = "tblSection";
                dsList.Tables[3].TableName = "tblDesig";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblDesigCapacity"];
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
            cboDept.DataSource = null;
            cboDept.DataSource = dsList.Tables["tblDept"];

            cboSection.DataSource = null;
            cboSection.DataSource = dsList.Tables["tblSection"];

            cboDesig.DataSource = null;
            cboDesig.DataSource = dsList.Tables["tblDesig"];
        }

        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetDesigCapacity  " + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, sqlQuery);
                dsDetail.Tables[0].TableName = "tblDesigCapacity";
                DataRow dr;

                if (dsDetail.Tables["tblDesigCapacity"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["tblDesigCapacity"].Rows[0];
                    
                    txtId.Text = dr["aId"].ToString();
                    txtDesigID.Text = dr["DesigId"].ToString();
                    this.cboDept.Value = dr["DeptId"].ToString();
                    this.cboSection.Value = dr["SectID"].ToString();
                    this.cboDesig.Text = dr["Band"].ToString();
                    txtCapacity.Text = dr["Capacity"].ToString();


                    if (dr["IsInactive"].ToString() == "1")
                    {
                        chkIsInactive.Checked = true;
                    }
                    else
                    {
                        chkIsInactive.Checked = false;
                    }
                    this.btnSave.Text = " &Update";
                    this.btnDelete.Enabled = true;
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
            txtId.Text = "";
            txtDesigID.Text = "";

            txtCapacity.Text = "";
            cboDept.Text = "";
            cboSection.Text = "";
            cboDesig.Text = "";

            chkIsInactive.Checked = false ;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;

        }

        private void frmDesigCapacity_Load(object sender, EventArgs e)
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

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide column
            gridList.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Department";
            gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            gridList.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
            gridList.DisplayLayout.Bands[0].Columns["Capacity"].Header.Caption = "Strenght";
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Header.Caption = "Inactive";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["DeptName"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 200;
            gridList.DisplayLayout.Bands[0].Columns["Band"].Width = 220;
            gridList.DisplayLayout.Bands[0].Columns["Capacity"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Width = 80;

            //Set column Style
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Style = ColumnStyle.CheckBox;


            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                if (btnSave.Text.ToString().ToUpper() != "&Save".ToUpper())
                {
                    //Update  
                    //--------------------
                    sqlQuery = "Update tblCat_DesigCapacity Set Band = '" + cboDesig.Text.ToString() +
                               "', DesigName = '" + cboDesig.Text.ToString() + "',DeptID = '" + cboDept.Value.ToString() + "', SectID = '" +
                               cboSection.Value.ToString() + "', Capacity = '" +
                               txtCapacity.Text.ToString().Trim() + "', IsInactive = '" + 
                               chkIsInactive.Tag.ToString() + "' where aId = " + int.Parse(txtId.Text.ToString().Trim()) + "";
                    //----------------------
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    sqlQuery = " Select Isnull(Max(aId),0)+1 As NewId from tblCat_DesigCapacity ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    //--------------------------
                    sqlQuery =
                        " Insert Into tblCat_DesigCapacity(Band, DesigName, DeptID, SectID,Capacity,IsInactive, aId, PCName, LUserId,ComID)" +
                        " Values('" + cboDesig.Text.ToString() + "','" + cboDesig.Text.ToString() + "','" + cboDept.Value.ToString() + "','" + cboSection.Value.ToString() + "','" + 
                                  txtCapacity.Text.ToString().Trim() + "','" + chkIsInactive.Tag.ToString() + "'," + NewId + ",'" + 
                                  Common.Classes.clsMain.strComputerName + "'," + 
                                  Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ")";
                    arQuery.Add(sqlQuery);
                    //----------------------------------------
                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);


                    MessageBox.Show("Data Saved Succefully");

                }

                prcClearData();
                prcLoadList();
                prcLoadCombo();

            }
            catch(Exception ex)
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
                if (MessageBox.Show("Do you want to delete Band capacity information of [" +gridList.ActiveRow.Cells[1].Text.ToString() +"]", "",
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
                    sqlQuery = "Delete From tblCat_DesigCapacity where ComID = " + Common.Classes.clsMain.intComId + " and aId = " + int.Parse(txtId.Text.ToString().Trim()) + "";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Deleted Successfully.");

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

        private void btnCancel_Click(object sender, EventArgs e)
            {
                prcClearData();
            }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void cboType_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void cboCategor_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }
        private void chkIsInactive_KeyDown(object sender, KeyEventArgs e)
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

        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void cboCategor_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

      
        private void chkIsInactive_KeyPress (object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtName_KeyPress_1 (object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtName_KeyDown_1 (object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }



        private void gridList_DoubleClick (object sender, EventArgs e)
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
            }


        private void txtInTime_KeyDown_1(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtOutTime_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtShiftLate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRegHrs_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunch_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunchIn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunchOut_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTiffin_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTiffinIn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTiffinOut_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunch_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboDept_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDept.DisplayLayout.Bands[0].Columns["DeptName"].Width = cboDept.Width;
            cboDept.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Department";
            cboDept.DisplayLayout.Bands[0].Columns["DeptId"].Hidden = true;
            cboDept.DisplayMember = "DeptName";
            cboDept.ValueMember = "DeptId";
        }

        private void cboSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSection.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSection.Width;
            cboSection.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            cboSection.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboSection.DisplayMember = "SectName";
            cboSection.ValueMember = "SectId";
        }

        private void cboDesig_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDesig.DisplayLayout.Bands[0].Columns["varName"].Width = cboDesig.Width;
            cboDesig.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";
            cboDesig.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            cboDesig.DisplayMember = "varName";
            cboDesig.ValueMember = "varName";
        }

        private void chkIsInactive_CheckedChanged(object sender, EventArgs e)
        {
            chkIsInactive.Tag = 0;
            if (chkIsInactive.Checked == true)
            {
                chkIsInactive.Tag = 1;
            }
        }


        }
   }


