using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Master
{
    public partial class frmWebMenu : Form
    {

        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        private System.Data.DataView dvgrid;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmWebMenu(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmWebMenu_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmWebMenu_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
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
                string sqlQuery = "Exec [WebprcGetMenu_Web] 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "MenuList";
                dsList.Tables[1].TableName = "MenuAll";
                dsList.Tables[2].TableName = "asdf";
                dsList.Tables[3].TableName = "GroupList"; 
                dsList.Tables[4].TableName = "FieldName";
                dsList.Tables[5].TableName = "FieldOperator";

                dvgrid = dsList.Tables["MenuList"].DefaultView;
                gridList.DataSource = null;
                gridList.DataSource = dvgrid;
            }
            finally
            {
                clsCon = null;
            }
        }

        private void prcLoadCombo()
        {

            cboGroup.DataSource = null;
            cboGroup.DataSource = dsList.Tables["GroupList"];
            cboGroup.ValueMember = "Menuid";
            cboGroup.DisplayMember = "MenuName";


            cboFilterFName.DataSource = null;
            cboFilterFName.DataSource = dsList.Tables["FieldName"];

            cboFilterOperator.DataSource = null;
            cboFilterOperator.DataSource = dsList.Tables["FieldOperator"];


            if(cboFilterFName.Rows.Count==0)
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

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                // A.MenuId, A.MenuName, A.menuLink, A.parentId, A.isInactive, A.isDefault

                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns["isDefault"].Hidden = true;//Module Name
                gridList.DisplayLayout.Bands[0].Columns["parentId"].Hidden = true;  //Group Name

                gridList.DisplayLayout.Bands[0].Columns["MenuId"].Width = 80;//Menu Id
                gridList.DisplayLayout.Bands[0].Columns["MenuName"].Width = 180;  //Menu Name
                gridList.DisplayLayout.Bands[0].Columns["menuLink"].Width = 190;//Group Id
                gridList.DisplayLayout.Bands[0].Columns["isInactive"].Width = 90;//Module Id

                gridList.DisplayLayout.Bands[0].Columns["MenuId"].Header.Caption = "Menu ID";
                gridList.DisplayLayout.Bands[0].Columns["MenuName"].Header.Caption = "Menu Name";
                gridList.DisplayLayout.Bands[0].Columns["menuLink"].Header.Caption = "Menu Link";
                gridList.DisplayLayout.Bands[0].Columns["isInactive"].Header.Caption = "Inactive";

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

                //Show Check Box Columns
                ///this.gridList.DisplayLayout.Bands[0].Columns[5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                gridList.DisplayLayout.Bands[0].Columns["isInactive"].Style =Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Use Filtering
                this.gridList.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
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

        //private void txtMenuName_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtMenuName);
        //}

        //private void txtMenuName_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtMenuName);
        //}

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
            txtMenuLink.Text = txtMenuLink.Text.Trim();
        }

        //private void txtMenuCaption_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtMenuLink);
        //}

        //private void txtMenuCaption_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtMenuLink);
        //}

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec [WebprcGetMenu_Web] " + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

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
                    this.txtMenuLink.Text = dr["menuLink"].ToString();
                    this.cboGroup.Value = dr["Parentid"].ToString();
                    this.chkDropdown.Checked = Boolean.Parse(dr["isInactive"].ToString());

                    //this.cboGroup.Text = dr["mMenuGroupCaption"].ToString();
                    //A.parentId, A.isInactive, A.isDefault
                    //this.chkDropdown.Checked = false;

                    //if (Int16.Parse(dr["isInactive"].ToString()) == 1)
                    //{
                    //    this.chkDropdown.Checked = true;
                    //}
                    //else
                    //{
                    //    this.chkDropdown.Checked = false;
                    //}
                    //if (Int16.Parse(dr["IsDropDownParent"].ToString()) == 1)
                    //{
                    //    this.chkIsDDParent.Checked = true;
                    //}
                    //this.cboDDParent.Value = dr["DropdownParentId"].ToString();

                    cboGroup.ValueMember = "Menuid";
                    cboGroup.DisplayMember = "MenuName";


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
            this.txtMenuLink.Text = "";
            this.cboGroup.Text = null;
            //this.cboGroup.DataSource = null;
           
            chkDropdown.Checked = false;
           
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

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            string sqlQuery = "";
            Int32 NewId = 0, ImageExist = 0, menuImageSize = 0;

            try
            {
                //Member Master Table
                if (txtMenuId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblWeb_Menu Set MenuName = '" + txtMenuName.Text.ToString() + "', MenuLink='" + txtMenuLink.Text.ToString() + "', ";
                    sqlQuery += " parentid = " + cboGroup.Value + ", ";
                    sqlQuery += " isInActive="+ chkDropdown.Tag.ToString() +"";
                    sqlQuery += " Where MenuId= " + Int32.Parse(txtMenuId.Text);

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Updated Successfully");
                    }
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(MenuId),0)+1 As NewId from tblWeb_Menu";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Insert Into tblWeb_Menu (MenuId, MenuName, MenuLink,Parentid, isInactive) ";
                    sqlQuery = sqlQuery + " Values (" + NewId + ", '" + txtMenuName.Text.ToString() + "', '" + txtMenuLink.Text.ToString() + "', " + cboGroup.Value + ","+chkDropdown.Tag.ToString()+")";

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Saved Successfully");
                    }
                }

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
                clsCon = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
            prcLoadCombo();
        }

        private void chkDropdown_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDropdown.Checked)
                chkDropdown.Tag = 1;
            else
                chkDropdown.Tag = 0;
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete menu information of [" + txtMenuName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblWeb_Menu Where MenuId = " + Int32.Parse(txtMenuId.Text);
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

            if (this.txtMenuLink.Text.Length == 0)
            {
                MessageBox.Show("Please provide menu caption.");
                txtMenuLink.Focus();
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
            return false;

        }

        private void cboModule_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtFormName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
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

        private void optImageUse_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtImageName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtImageName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }


        private void cboGroup_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGroup.DisplayLayout.Bands[0].Columns["Menuid"].Hidden = true;
            cboGroup.DisplayLayout.Bands[0].Columns["MenuName"].Width = cboGroup.Width;
//            cboGroup.DisplayLayout.Bands[0].Columns[3].Hidden = true;   //Module Id

            cboGroup.DisplayLayout.Bands[0].Columns["Menuid"].Header.Caption = "Group Name";
            cboGroup.DisplayLayout.Bands[0].Columns["MenuName"].Header.Caption = "Group Caption";

            cboGroup.ValueMember = "Menuid";
            cboGroup.DisplayMember = "MenuName";


        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog diagOpen = new OpenFileDialog();
                //diagOpen.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                diagOpen.Filter = "Icon Files(*.ico)|*.ico";
               
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }

        private void picPreview_Click(object sender, EventArgs e)
        {

        }

        private void chkDropdown_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDDParent_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
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