using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;
using System.Globalization;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmVariable : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmVariable(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmVariable_Load(object sender, EventArgs e)
        {
            prcLoadList();
            prcLoadCombo();
        }

        private void prcLoadList()
        {
            dsList = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();

            try
            {
                string sqlQuery = "Exec prcGetVariable 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "VariableList";
                dsList.Tables[1].TableName = "VariableType";
                dsList.Tables[2].TableName = "Module";

                //Load Module

                gridModule.DataSource = null;
                gridModule.DataSource = dsList.Tables["Module"];
                // Load All Variable
                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["VariableList"];
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

        private void prcLoadCombo()
        {
            cboVarType.DataSource = dsList.Tables["VariableType"];
            //cboVarType.DisplayMember = "varType";
            //cboVarType.ValueMember = "varType";
        }

        private void frmVariable_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//Variable Id
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 150;  //Variable Name
                gridList.DisplayLayout.Bands[0].Columns[2].Width = 150;  //Variabl Type
                gridList.DisplayLayout.Bands[0].Columns[3].Width = 100;  //Is Inactive

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Variable Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Variable Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Variable Type";
                gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Inactive";

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                e.Layout.GroupByBox.Hidden = true;

                // Set the scroll style to immediate so the rows get scrolled immediately
                // when the vertical scrollbar thumb is dragged.
                e.Layout.ScrollStyle = ScrollStyle.Immediate;

                // ScrollBounds of ScrollToFill will prevent the user from scrolling the
                // grid further down once the last row becomes fully visible.
                e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();

            try
            {


                string sqlQuery = "Exec prcGetVariable " + Int32.Parse(strParam);
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Variable";
                dsDetails.Tables[1].TableName = "Module";

                gridModule.DataSource = null;
                gridModule.DataSource = dsDetails.Tables["Module"];
                DataRow dr;
                if (dsDetails.Tables["Variable"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Variable"].Rows[0];

                    this.txtVarId.Text = dr["VarId"].ToString();
                    this.txtVarName.Text = dr["varName"].ToString();
                    this.txtRemarks.Text = dr["Remarks"].ToString();

                    this.cboVarType.Text = dr["varType"].ToString();
                    if (Int16.Parse(dr["IsInactive"].ToString()) == 0)
                    {
                        this.chkInactive.Checked = false;
                    }
                    else
                    {
                        this.chkInactive.Checked = true;
                    }

                    if (Int16.Parse(dr["IsChild"].ToString()) == 0)
                    {
                        this.chkChild.Checked = false;
                    }
                    else
                    {
                        this.chkChild.Checked = true;
                    }
                  
                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
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

        private void prcClearData()
        {
            this.txtVarId.Text = "";
            this.txtVarName.Text = "";
            this.cboVarType.Text = "";
            this.chkInactive.Checked = false;
            this.chkChild.Checked = false;
            txtRemarks.Text = "";
            this.btnDelete.Enabled = false;
            this.btnSave.Text= "&Save";

            this.txtVarId.Focus();
        }

        private Boolean fncBlank()
        {
            if (this.txtVarName.Text.Length == 0)
            {
                MessageBox.Show("Please provide Variable name.");
                txtVarName.Focus();
                return true;
            }
            if (this.cboVarType.Text.Length == 0)
            {
                MessageBox.Show("Please provide Variable Type.");
                cboVarType.Focus();
                return true;
            }
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
                string Module="";
                foreach(UltraGridRow row in gridModule.Rows)
                {
                    if(int.Parse(row.Cells["Mark"].Value.ToString())==1)
                    {
                        Module += row.Cells["moduleId"].Value+",";
                    }
                }
                Module = Module.Substring(0, Module.Length - 1);

                //Member Master Table
                if (txtVarId.Text.Length != 0)
                {
                    //Update data
                    sqlQuery = " Update tblCat_Variable Set varName = '" + txtVarName.Text.ToString() + "', varType='" + cboVarType.Text.ToString() + "', IsInactive=" + chkInactive.Tag
                        + ",ModuleId='" + Module + "',IsChild='" + chkChild.Tag + "',Remarks='"+txtRemarks.Text+"'  Where VarId = " + Int32.Parse(txtVarId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);
                    
                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //newId
                    sqlQuery = "Select Isnull(Max(VarId),0)+1 As NewId from tblCat_Variable";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert Data
                    sqlQuery = "Insert Into tblCat_Variable (VarId, aId, varName, varType, IsInactive,ModuleId,Remarks,IsChild) "
                         + " Values (" + NewId + ", " + NewId + ", '" + txtVarName.Text.ToString() + "', '" + cboVarType.Text.ToString() + "', " + chkInactive.Tag 
                         + ",'"+Module+"','"+txtRemarks.Text+"',"+chkChild.Tag+")";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                }
                prcClearData();
                txtVarName.Focus();

                prcLoadList();
                prcLoadCombo();
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

        private void chkInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInactive.Checked)
                chkInactive.Tag = 1;
            else
                chkInactive.Tag = 0;
        }

        private void txtVarName_MouseClick(object sender, MouseEventArgs e)
        {
            clM.GTRGotFocus(ref txtVarName);
        }

        private void txtVarName_Leave(object sender, EventArgs e)
        {
            txtVarName.Text = txtVarName.Text.ToString();
        }

        private void txtVarName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtVarName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboVarType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboVarType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void chkInactive_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete variable information of [" + txtVarName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                //Delete data
                sqlQuery = "Delete from tblCat_Variable Where varId = " + Int32.Parse(txtVarId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                    + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                prcClearData();
                txtVarName.Focus();

                prcLoadList();
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
            prcLoadList();
        }

        private void cboVarType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboVarType.ValueMember = "varType";
            cboVarType.DisplayMember = "varType";
            cboVarType.DisplayLayout.Bands[0].Columns["varType"].Width = cboVarType.Width;

        }

        private void gridModule_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridModule.DisplayLayout.Bands[0].Columns["moduleId"].Hidden = true;//MOduleID
                gridModule.DisplayLayout.Bands[0].Columns["moduleCaption"].Width = 230;  //Module Name
                gridModule.DisplayLayout.Bands[0].Columns["Mark"].Width = 50;  //Module Name
                
                gridModule.DisplayLayout.Bands[0].Columns["moduleCaption"].Header.Caption = "Module Name ";
                gridModule.DisplayLayout.Bands[0].Columns["Mark"].Header.Caption = "Mark";
                //Change alternate color
                gridModule.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridModule.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;
                gridModule.DisplayLayout.Bands[0].Columns["Mark"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //Select Full Row when click on any cell
               // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                gridModule.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
              //  gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
              gridModule.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
              //  gridModule.DisplayLayout.Override.HeaderClickAction=;
                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
              //  gridModule.DisplayLayout.Bands[0].HeaderVisible = false;
                //Use Filtering
                gridModule.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void chkChild_CheckedChanged(object sender, EventArgs e)
        {
            chkChild.Tag = 0;
            if(chkChild.Checked==true)
            {
                chkChild.Tag = 1;
            }
        }
    }
}