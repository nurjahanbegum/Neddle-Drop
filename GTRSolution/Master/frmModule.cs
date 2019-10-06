using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmModule : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        Common.Classes.clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmModule(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmModule_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmModule_Load(object sender, EventArgs e)
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

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetModule 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "ModuleList";

                gridList.DataSource = null;
                gridList.DataSource = dsList;
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

        private void prcLoadCombo()
        {
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//ModuleId
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 150;  //ModuleName
                gridList.DisplayLayout.Bands[0].Columns[2].Width = 300;  //ModuleCaption
                gridList.DisplayLayout.Bands[0].Columns[3].Width = 80;  //IsInactive

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Module Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Module Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Module Caption";
                gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Inactive";

                //Show Check Box Columns
                this.gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtModuleName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtModuleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtModuleName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtModuleName);
        }

        private void txtModuleName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtModuleName);
        }

        private void txtModuleName_Leave(object sender, EventArgs e)
        {
            txtModuleName.Text = txtModuleName.Text.ToString();
        }

        private void txtModuleCaption_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtModuleCaption_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtModuleCaption_Leave(object sender, EventArgs e)
        {
            txtModuleCaption.Text = txtModuleCaption.Text.ToString();
        }

        private void txtModuleCaption_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtModuleCaption);
        }

        private void txtModuleCaption_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtModuleCaption);
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetModule " + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "Module";

            DataRow dr;
            if (dsDetails.Tables["Module"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["Module"].Rows[0];

                this.txtModuleId.Text = dr["ModuleId"].ToString();
                this.txtModuleName.Text = dr["ModuleName"].ToString();
                this.txtModuleCaption.Text = dr["ModuleCaption"].ToString();
                if (Int16.Parse(dr["IsInactive"].ToString()) == 0)
                {
                    this.chkInactive.Checked = false;
                }
                else
                {
                    this.chkInactive.Checked = true;
                }

                this.btnSave.Text = "&Update";
                this.btnDelete.Enabled = true;
            }
        }

        private void prcClearData()
        {
            this.txtModuleId.Text = "";
            this.txtModuleName.Text = "";
            this.txtModuleCaption.Text = "";
            this.chkInactive.Checked = false;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            this.txtModuleName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            try
            {
                //Member Master Table
                if (txtModuleId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblModule Set ModuleName = '" + txtModuleName.Text.ToString() + "', ModuleCaption='" + txtModuleCaption.Text.ToString() + "', "
                        + " IsInactive=" + chkInactive.Tag + ", PCName='" + Common.Classes.clsMain.strComputerName + "', LUserId = " + Common.Classes.clsMain.intUserId + ""
                        + " Where ModuleId = " + Int32.Parse(txtModuleId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(ModuleId),0)+1 As NewId from tblModule";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Insert Into tblModule (ModuleId, aId, ModuleName, ModuleCaption, IsInactive, PCName, LUserId) "
                        + " Values (" + NewId + ", " + NewId + ", '" + txtModuleName.Text.ToString() + "', '" + txtModuleCaption.Text.ToString() + "', " + chkInactive.Tag + ", '" + Common.Classes.clsMain.strComputerName + "'," + Common.Classes.clsMain.intUserId + ")";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Saved Successfully");
                }
                prcClearData();
                txtModuleName.Focus();

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete module information of [" + txtModuleName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblModule Where ModuleId = " + Int32.Parse(txtModuleId.Text);
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
                    txtModuleName.Focus();

                    prcLoadList();
                    prcLoadList();
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
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        private Boolean fncBlank()
        {
            if (this.txtModuleName.Text.Length == 0)
            {
                MessageBox.Show("Please provide module name.");
                txtModuleName.Focus();
                return true;
            }
            if (this.txtModuleCaption.Text.Length == 0)
            {
                MessageBox.Show("Please provide module caption.");
                txtModuleCaption.Focus();
                return true;
            }
            return false;
        }

        private void txtModuleName_ValueChanged(object sender, EventArgs e)
        {
        }

        private void chkInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInactive.Checked)
                chkInactive.Tag = 1;
            else
                chkInactive.Tag = 0;
        }

        private void chkInactive_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
    }
}