using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmModuleMenu : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        private System.Data.DataView dvgrid;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmModuleMenu(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmModuleMenu_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmModuleMenu_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();

                picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetMenu 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "MenuList";
                dsList.Tables[1].TableName = "ModuleList";
                dsList.Tables[2].TableName = "GroupList";
                dsList.Tables[3].TableName = "DropdownParent"; 
                dsList.Tables[4].TableName = "FiledName";
                dsList.Tables[5].TableName = "FiledOperator";

                dvgrid = dsList.Tables["MenuList"].DefaultView;
                gridList.DataSource = null;
                gridList.DataSource = dvgrid;
            }
            catch(Exception ex)
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
            try
            {


                // Module Combo
                cboModule.DataSource = null;
                cboModule.DataSource = dsList.Tables["ModuleList"];
                cboModule.DisplayMember = "ModuleCaption";
                cboModule.ValueMember = "ModuleId";


                cboFilterFName.DataSource = null;
                cboFilterFName.DataSource = dsList.Tables["FiledName"];

                cboFilterOperator.DataSource = null;
                cboFilterOperator.DataSource = dsList.Tables["FiledOperator"];


                if (cboFilterFName.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    cboFilterFName.Text = cboFilterFName.Rows[0].Cells["FieldName"].Text;
                }

                if (cboFilterOperator.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    cboFilterOperator.Text = cboFilterOperator.Rows[0].Cells["Operator"].Text;
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        private void prcLoadGroup()
        {
            try
            {
            cboGroup.Text = "";
            cboGroup.DataSource = null;
            if (cboModule.Value == null)
                return;
            //if (cboModule.IsItemInList() == false)
            //    return;

            DataView dv = new DataView(dsList.Tables["GroupList"], "ModuleId=" + cboModule.Value.ToString() + "", "mMenuGroupName", DataViewRowState.CurrentRows);
            cboGroup.DataSource = dv;//dsList.Tables["GroupList"].Select("ModuleId="+cboModule.Value);
            cboGroup.DisplayMember = "mMenuGroupCaption";
            cboGroup.ValueMember = "mMenuGroupId";
            }
            catch (Exception)
            {
            }
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//Menu Id
                gridList.DisplayLayout.Bands[0].Columns[1].Hidden = true;  //Menu Name
                gridList.DisplayLayout.Bands[0].Columns[2].Width = 180;  //Menu Caption
                gridList.DisplayLayout.Bands[0].Columns[3].Hidden = true;//Group Id
                gridList.DisplayLayout.Bands[0].Columns[4].Hidden = true;  //Group Name
                gridList.DisplayLayout.Bands[0].Columns[5].Width = 180;  //Group Caption
                gridList.DisplayLayout.Bands[0].Columns[6].Hidden = true;//Module Id
                gridList.DisplayLayout.Bands[0].Columns[7].Hidden = true;//Module Name
                gridList.DisplayLayout.Bands[0].Columns[8].Width = 180;  //Module Caption
                gridList.DisplayLayout.Bands[0].Columns[9].Hidden = true;//Module Id
                gridList.DisplayLayout.Bands[0].Columns[10].Hidden = true;//Module Name
                gridList.DisplayLayout.Bands[0].Columns[11].Hidden = true;//Module Id
                gridList.DisplayLayout.Bands[0].Columns[12].Width = 150;  //Form Name
                gridList.DisplayLayout.Bands[0].Columns[13].Hidden = true;//Form Location

                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Menu Caption";
                gridList.DisplayLayout.Bands[0].Columns[5].Header.Caption = "Group Caption";
                gridList.DisplayLayout.Bands[0].Columns[8].Header.Caption = "Module Caption";
                gridList.DisplayLayout.Bands[0].Columns[12].Header.Caption = "Form Name";

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

                //Use Filtering
                this.gridList.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;

                this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtMenuName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMenuName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMenuName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuName);
        }

        private void txtMenuName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuName);
        }

        private void txtMenuName_Leave(object sender, EventArgs e)
        {
            txtMenuName.Text = txtMenuName.Text.Trim();
        }

        private void txtMenuCaption_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtMenuCaption_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtMenuCaption_Leave(object sender, EventArgs e)
        {
            txtMenuCaption.Text = txtMenuCaption.Text.Trim();
        }

        private void txtMenuCaption_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuCaption);
        }

        private void txtMenuCaption_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuCaption);
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetMenu " + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Menu";

                DataRow dr;
                if (dsDetails.Tables["Menu"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Menu"].Rows[0];

                    this.txtMenuId.Text = dr["MenuId"].ToString();
                    this.txtMenuName.Text = dr["MenuName"].ToString();
                    this.txtMenuCaption.Text = dr["MenuCaption"].ToString();
                    this.txtFormName.Text = dr["frmName"].ToString();
                    this.txtFormLocation.Text = dr["frmLocation"].ToString();
                    this.cboModule.Value = dr["ModuleId"].ToString();
                    this.cboModule.Text = dr["ModuleCaption"].ToString();
                    this.cboGroup.Value = dr["mMenuGroupId"].ToString();
                    this.cboGroup.Text = dr["mMenuGroupCaption"].ToString();

                    if (Int16.Parse(dr["menuImageExist"].ToString())!=0)
                    {
                        optImageUse.CheckedIndex=1;
                        if (Int16.Parse(dr["menuImageSize"].ToString()) == 2)
                        {
                            optImageUse.CheckedIndex = 2;
                        }
                        txtImageName.Text = dr["menuImageName"].ToString();
                        //txtImageName.Tag = dr["menuImageName"].ToString();
                        picPreview.Image = new Bitmap(@"Z:\Com\pics\Icon\"+txtImageName.Text);
                    }

                    this.chkDropdown.Checked = false;
                    if (Int16.Parse(dr["IsDropDown"].ToString()) == 1)
                    {
                        this.chkDropdown.Checked = true;
                    }
                    this.chkIsDDParent.Checked = false;
                    if (Int16.Parse(dr["IsDropDownParent"].ToString()) == 1)
                    {
                        this.chkIsDDParent.Checked = true;
                    }
                    this.cboDDParent.Value = dr["DropdownParentId"].ToString();

                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                clsCon = null;
            }
        }

        private void prcClearData()
        {
            this.txtMenuId.Text = "";
            this.txtMenuName.Text = "";
            this.txtMenuCaption.Text = "";
            this.txtFormName.Text = "";
            this.txtFormLocation.Text = "";
            this.cboGroup.Value = null;
            this.cboGroup.DataSource = null;
            this.cboModule.Value = null;
            this.optImageUse.CheckedIndex = 0;
            picPreview.Image = null;

            chkDropdown.Checked = false;
            chkIsDDParent.Checked = false;
            
            chkIsDDParent.Enabled = false;
            cboDDParent.Enabled = false;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.txtMenuName.Focus();
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
            Int32 NewId = 0, ImageExist = 0, menuImageSize = 0;

            if (optImageUse.Value.ToString() != "0")
            {
                ImageExist = Int16.Parse(optImageUse.Value.ToString());
                menuImageSize = Int16.Parse(optImageUse.Value.ToString());
            }

            cboDDParent.Tag = "0";
            if(chkDropdown.Checked)
            {
                if(chkIsDDParent.Checked==false)
                {
                    cboDDParent.Tag = cboDDParent.Value.ToString();
                }
            }

            try
            {
                //Member Master Table
                if (txtMenuId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblModule_Menu Set MenuName = '" + txtMenuName.Text.ToString() + "', MenuCaption='" + txtMenuCaption.Text.ToString() + "', "
                        + " mMenuGroupId = " + cboGroup.Value + ", MenuImageExist = " + ImageExist + ", MenuImageSize = " + menuImageSize + ", menuImageName='" + txtImageName.Text.Trim() + "', "
                        + " frmName = '" + txtFormName.Text.Trim() + "', frmLocation = '" + txtFormLocation.Text.Trim() + "', IsDropDown="+ chkDropdown.Tag.ToString() +", IsDropDownParent="+ chkIsDDParent.Tag.ToString() +", DropDownParentId = "+ cboDDParent.Tag.ToString() +""
                        + ", PCName='" + Common.Classes.clsMain.strComputerName + "', LUserId = " + Common.Classes.clsMain.intUserId + ""
                        + " Where MenuId= " + Int32.Parse(txtMenuId.Text);
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
                    sqlQuery = "Select Isnull(Max(MenuId),0)+1 As NewId from tblModule_Menu";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Insert Into tblModule_Menu (MenuId, Aid, MenuName, MenuCaption, mMenuGroupId, menuImageExist, menuImageSize, menuImageName, frmName, frmLocation, IsDropDown, IsDropDownParent, DropDownParentId, PCName, LUserId) "
                        + " Values (" + NewId + ", " + NewId + ", '" + txtMenuName.Text.ToString() + "', '" + txtMenuCaption.Text.ToString() + "', " + cboGroup.Value + "," + ImageExist + "," + menuImageSize + ",'" + txtImageName.Text.Trim() + "', '" + txtFormName.Text.Trim() + "','" + txtFormLocation.Text.Trim() + "'," + chkDropdown.Tag.ToString() + "," + chkIsDDParent.Tag.ToString() + ", " + cboDDParent.Tag.ToString() + ", '" + Common.Classes.clsMain.strComputerName + "'," + Common.Classes.clsMain.intUserId + ")";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Saved Successfully");
                }

                #region CopyImage
                if (txtImageName.Text.Length != 0)
                {
                    if(txtImageName.Tag!=null)//If New Image then it will be copy else no need to copy
                    {
                    string strTarget = Common.Classes.clsMain.strPicPathIcon +@"\"+ txtImageName.Text;
                    File.Copy(txtImageName.Tag.ToString(), strTarget, true);                        
                    }
                }
                #endregion

                prcClearData();
                txtMenuName.Focus();

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
            if (MessageBox.Show("Do you want to delete menu information of [" + txtMenuName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblModule_Menu Where MenuId = " + Int32.Parse(txtMenuId.Text);
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
                    txtMenuName.Focus();

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
            if (this.txtMenuName.Text.Length == 0)
            {
                MessageBox.Show("Please provide menu name.");
                txtMenuName.Focus();
                return true;
            }
            if (this.txtMenuCaption.Text.Length == 0)
            {
                MessageBox.Show("Please provide menu caption.");
                txtMenuCaption.Focus();
                return true;
            }
            if (this.txtFormName.Text.Length == 0)
            {
                MessageBox.Show("Please provide form name.");
                txtFormName.Focus();
                return true;
            }
            if (this.txtFormLocation.Text.Length == 0)
            {
                MessageBox.Show("Please provide form location.");
                txtFormLocation.Focus();
                return true;
            }
            if (this.cboModule.Text.Length == 0)
            {
                MessageBox.Show("Please provide under module.");
                cboModule.Focus();
                return true;
            }

            if (this.cboModule.IsItemInList() == false)
            {
                MessageBox.Show("Please provide valid module.");
                cboModule.Focus();
                return true;
            }

            if (this.cboGroup.Text.Length == 0)
            {
                MessageBox.Show("Please provide under group.");
                cboGroup.Focus();
                return true;
            }

            if (this.cboGroup.IsItemInList() == false)
            {
                MessageBox.Show("Please provide valid group.");
                cboGroup.Focus();
                return true;
            }

            if (optImageUse.Value.ToString() != "0")
            {
                if (this.txtImageName.Text.Length == 0)
                {
                    MessageBox.Show("Please provide image name.");
                    txtImageName.Focus();
                    return true;
                }
            }

            if(chkDropdown.Checked)
            {
                if(chkIsDDParent.Checked==false)
                {
                    if (this.cboDDParent.Text.Length == 0)
                    {
                        MessageBox.Show("Please provide parent menu name for dropdown menu.");
                        cboDDParent.Focus();
                        return true;
                    }
                }
            }
            return false;
        }

        private void cboModule_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFormName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtFormName);
        }

        private void txtFormName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFormName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtFormName);
        }

        private void txtFormLocation_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboGroup_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFormName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtFormLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtFormName_Leave(object sender, EventArgs e)
        {
            txtFormName.Text = txtFormName.Text.ToString().Trim();
        }

        private void txtFormLocation_Leave(object sender, EventArgs e)
        {
            txtFormLocation.Text = txtFormLocation.Text.ToString().Trim();
        }

        private void txtFormLocation_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtFormLocation);
        }

        private void txtFormLocation_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtFormLocation);
        }

        private void optImageUse_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtImageName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtImageName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtImageName);
        }

        private void txtImageName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtImageName);
        }

        private void txtImageName_Leave(object sender, EventArgs e)
        {
            txtImageName.Text = txtImageName.Text.ToString().Trim();
        }

        private void txtImageName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void optImageUse_ValueChanged(object sender, EventArgs e)
        {
            txtImageName.Enabled = true;
            btnBrowse.Enabled = true;
            switch (optImageUse.Value.ToString())
            {
                case "0":
                    txtImageName.Text = "";
                    txtImageName.Enabled = false;
                    btnBrowse.Enabled = false;
                    picPreview.Image = null;
                    break;
            }
        }

        private void cboModule_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            cboModule.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboModule.DisplayLayout.Bands[0].Columns[1].Hidden = true;
            cboModule.DisplayLayout.Bands[0].Columns[2].Width = cboModule.Width;

            cboModule.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Module Name";
            cboModule.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Module Caption";   
        }

        private void cboGroup_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGroup.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboGroup.DisplayLayout.Bands[0].Columns[1].Hidden = true;
            cboGroup.DisplayLayout.Bands[0].Columns[2].Width = cboGroup.Width;
            cboGroup.DisplayLayout.Bands[0].Columns[3].Hidden = true;   //Module Id

            cboGroup.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Group Name";
            cboGroup.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Group Caption";
        }

        private void cboModule_RowSelected(object sender, RowSelectedEventArgs e)
        {
            prcLoadGroup();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog diagOpen = new OpenFileDialog();
                //diagOpen.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                diagOpen.Filter = "Icon Files(*.ico)|*.ico";
                if (diagOpen.ShowDialog() == DialogResult.OK)
                {
                    txtImageName.Text = diagOpen.FileName.Substring(diagOpen.FileName.LastIndexOf("\\") + 1);
                    txtImageName.Tag = diagOpen.FileName;
                    picPreview.Image = new Bitmap(diagOpen.FileName);
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }

        private void picPreview_Click(object sender, EventArgs e)
        {
        }

        private void cboGroup_RowSelected(object sender, RowSelectedEventArgs e)
        {
            prcLoadDropdownPerent();
        }

        private void prcLoadDropdownPerent()
        {
            cboDDParent.Text = "";
            cboDDParent.DataSource = null;
            if (cboGroup.Value == null)
                return;

            DataView dv = new DataView(dsList.Tables["DropdownParent"], "ModuleId=" + clsProc.GTRValidateDouble(cboModule.Value.ToString()) + " and mMenuGroupId="+cboGroup.Value.ToString()+"", "MenuName", DataViewRowState.CurrentRows);
            cboDDParent.DataSource = dv;
            cboDDParent.DisplayMember = "MenuCaption";
            cboDDParent.ValueMember = "MenuId";
        }

        private void cboDDParent_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboDDParent.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboDDParent.DisplayLayout.Bands[0].Columns[1].Hidden = true;
            cboDDParent.DisplayLayout.Bands[0].Columns[2].Width = cboDDParent.Width;
            cboDDParent.DisplayLayout.Bands[0].Columns[3].Hidden = true;
            cboDDParent.DisplayLayout.Bands[0].Columns[4].Hidden = true;

            cboDDParent.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Menu Name";
            cboDDParent.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Menu Caption";
        }

        private void chkDropdown_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDDParent_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void chkIsDDParent_CheckedChanged(object sender, EventArgs e)
        {
            chkIsDDParent.Tag = "0";
            if(chkIsDDParent.Checked)
            {
                chkIsDDParent.Tag = "1";
                cboDDParent.Enabled = false;
            }
        }

        private void chkDropdown_CheckedChanged(object sender, EventArgs e)
        {
            chkDropdown.Tag = "0";
            chkIsDDParent.Enabled = false;
            cboDDParent.Enabled = false;
            if(chkDropdown.Checked==true)
            {
                chkDropdown.Tag = "1";

                chkIsDDParent.Enabled = true;
                cboDDParent.Enabled = true;
            }
        }

        private void cboDDParent_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(cboDDParent.Text.Length>0)
            {
                if (cboDDParent.IsItemInList() == false)
                {
                    MessageBox.Show("Please provide valid data [or select from list].");
                    cboDDParent.Focus();
                }
            }
        }

        private void cboFilterOperator_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFilterOperator.DisplayLayout.Bands[0].Columns["Operator"].Header.Caption = "Field Name";
            cboFilterOperator.DisplayLayout.Bands[0].Columns["SLno"].Hidden = true;
            cboFilterOperator.DisplayLayout.Bands[0].Columns["Operator"].Width = cboFilterOperator.Width;
            cboFilterOperator.DisplayMember = "Operator";
            cboFilterOperator.ValueMember = "Operator";
        }

        private void cboFilterFName_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFilterFName.DisplayLayout.Bands[0].Columns["FieldValue"].Hidden = true;
            cboFilterFName.DisplayLayout.Bands[0].Columns["SlNo"].Hidden = true;
            cboFilterFName.DisplayLayout.Bands[0].Columns["FieldName"].Header.Caption = "Field Name";
            cboFilterFName.DisplayLayout.Bands[0].Columns["FieldName"].Width = cboFilterFName.Width;
            cboFilterFName.DisplayMember = "FieldName";
            cboFilterFName.ValueMember = "FieldValue";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dsList.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Data not found in grid to filter");
                return;
            }

            DataView dvSource = new DataView();
            try
            {
                dvSource = (DataView)gridList.DataSource;

                dvSource.RowFilter = "";
                if (txtFilterFValue.Text.Length > 0)
                {
                    string str = cboFilterOperator.Value.ToString().Trim().ToUpper() == "LIKE" ? "%" : "";
                    dvSource.RowFilter = cboFilterFName.Value.ToString() + " " + cboFilterOperator.Value.ToString() +
                                         " " + "'" + str + txtFilterFValue.Text.ToString() + str + "'";
                }
                gridList.DataSource = null;
                gridList.DataSource = dvSource;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dvSource = null;
            }
        }

        private void cboFilterFName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboFilterOperator_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFilterFValue_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

    }
}