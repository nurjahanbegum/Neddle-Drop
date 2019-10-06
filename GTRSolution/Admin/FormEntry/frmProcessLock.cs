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

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmProcessLock : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();
        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private int secId_update = 0; // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmProcessLock(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmProcessLock_FormClosing(object sender, FormClosingEventArgs e)
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

            if (cboType .Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Lock Type");
                cboType.Focus();
                return true;
            }

            if (dtDate.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Date");
                dtDate.Focus();
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
                string sqlQuery = "Exec prcGetProcessLock  " + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblType";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];
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
            cboType.DataSource = null;
            cboType.DataSource = dsList.Tables["tblType"];

        }

        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetProcessLock  " + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, sqlQuery);
                dsDetail.Tables[0].TableName = "tblGrid";
                DataRow dr;

                if (dsDetail.Tables["tblGrid"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["tblGrid"].Rows[0];
                    
                    txtId.Text = dr["aId"].ToString();
                    this.cboType.Text = dr["LockType"].ToString();
                    this.dtDate.Value = dr["dtDate"].ToString();


                    if (dr["isLock"].ToString() == "1")
                    {
                        chkLock.Checked = true;
                    }
                    else
                    {
                        chkLock.Checked = false;
                    }
                    
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
            cboType.Text = "";

            dtDate.Value = DateTime.Now;

            chkLock.Checked = false;
            chkIsInactive.Checked = false ;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;

        }

        private void frmProcessLock_Load(object sender, EventArgs e)
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
            gridList.DisplayLayout.Bands[0].Columns["ComID"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["LockType"].Header.Caption = "Lock Type";
            gridList.DisplayLayout.Bands[0].Columns["dtDate"].Header.Caption = "dtDate";

            gridList.DisplayLayout.Bands[0].Columns["isLock"].Header.Caption = "Lock";
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Header.Caption = "Inactive";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["LockType"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["dtDate"].Width = 200;
            
            gridList.DisplayLayout.Bands[0].Columns["IsLock"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Width = 80;

            //Set column Style
            gridList.DisplayLayout.Bands[0].Columns["IsLock"].Style = ColumnStyle.CheckBox;
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Style = ColumnStyle.CheckBox;

            //Date Format
            this.gridList.DisplayLayout.Bands[0].Columns["dtDate"].Format = "dd-MMM-yyyy";


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
                    sqlQuery = "Update tblProcessLock Set LockType = '" + cboType.Text.ToString() +
                               "', dtDate = '" + clsProc.GTRDate(dtDate.Value.ToString()) + "',IsLock= '" + chkLock.Tag.ToString() + "',IsInactive = '" + 
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
                    //sqlQuery = " Select Isnull(Max(aId),0)+1 As NewId from tblCat_DesigCapacity ";
                    //NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    //--------------------------
                    sqlQuery =
                        " Insert Into tblProcessLock(ComID, LockType, dtDate, IsLock,IsInactive,PCName, LUserId)" +
                        " Values(" + Common.Classes.clsMain.intComId + ",'" + cboType.Text.ToString() + "','" +
                                  clsProc.GTRDate(dtDate.Value.ToString()) + "','" + chkLock.Tag.ToString() + "','" + chkIsInactive.Tag.ToString() + "','" + 
                                  Common.Classes.clsMain.strComputerName + "'," + Common.Classes.clsMain.intUserId + ")";
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
                if (MessageBox.Show("Do you want to delete lock information of [" +gridList.ActiveRow.Cells[2].Text.ToString() +"]", "",
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
                    sqlQuery = "Delete From tblProcessLock where ComID = " + Common.Classes.clsMain.intComId + " and aId = " + int.Parse(txtId.Text.ToString().Trim()) + "";
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
            cboType.DisplayLayout.Bands[0].Columns["varName"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Lock Type";
            cboType.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            cboType.DisplayMember = "varName";
            cboType.ValueMember = "varName";
        }



        private void chkIsInactive_CheckedChanged(object sender, EventArgs e)
        {
            chkIsInactive.Tag = 0;
            if (chkIsInactive.Checked == true)
            {
                chkIsInactive.Tag = 1;
            }
        }

        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            chkLock.Tag = 0;
            if (chkLock.Checked == true)
            {
                chkLock.Tag = 1;
            }
        }


        }
   }


