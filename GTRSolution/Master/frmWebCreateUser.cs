using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Master
{
    public partial class frmWebCreateUser : Form
    {
        //System.Data.DataSet dsList;
        //System.Data.DataSet dsFilter;
        System.Data.DataSet dsFilter1;
        System.Data.DataSet dsFilter2;
        //System.Data.DataSet dsDetails;
        private System.Data.DataView dvgrid;


        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmWebCreateUser(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmWebCreateUser_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmWebCreateUser_Load(object sender, EventArgs e)
        {
            prcLoadList();
            prcLoadCombo();

            if (cboRefName.Text == "Employee")
            {
                cboRelation.Enabled = false;
                cboRelation.Value = 0;

            }
            else
            {
                cboRelation.Enabled = true;

            }
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec WebprcGetWebUser " + Common.Classes.clsMain.intUserId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblWeb_User";
                dsList.Tables[1].TableName = "tblWeb_User_1";
                dsList.Tables[2].TableName = "tblWeb_User_Category";
                dsList.Tables[3].TableName = "tblWeb_User_Type";
                dsList.Tables[4].TableName = "tblWeb_SecurityQues";
                dsList.Tables[5].TableName = "tblCat_Contact";
                dsList.Tables[6].TableName = "tblCat_CustomerInfo";
                dsList.Tables[7].TableName = "FieldName";
                dsList.Tables[8].TableName = "FieldOperator";
                //dsList.Tables[7].TableName = "tblWeb_User_Category";


                dvgrid = dsList.Tables["tblWeb_User"].DefaultView;
                gridList.DataSource = null;
                gridList.DataSource = dvgrid;

 
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

            cboUsType.DataSource = null;
            cboUsType.DataSource = dsList.Tables["tblWeb_User_Type"];
            cboUsType.ValueMember = "userTypeId";
            cboUsType.DisplayMember = "userTypeName";



            cboQuestion.DataSource = null;
            cboQuestion.DataSource = dsList.Tables["tblWeb_SecurityQues"];



            cboGroup.DataSource = null;
            cboGroup.DataSource = dsList.Tables["tblWeb_User_Category"];
            cboGroup.ValueMember = "ID";
            cboGroup.DisplayMember = "Name";
            
            cboRelation.DataSource = null;
            cboRelation.DataSource = dsList.Tables["tblCat_Contact"];
            cboRelation.ValueMember = "ID";
            cboRelation.DisplayMember = "Name";


            cboFilterFName.DataSource = null;
            cboFilterFName.DataSource = dsList.Tables["FieldName"];

            cboFilterOperator.DataSource = null;
            cboFilterOperator.DataSource = dsList.Tables["FieldOperator"];


            //cboRelation.DataSource = null;
            //cboRelation.DataSource = dsList.Tables["tblWeb_User_Category"];

            //cboRelation.DataSource = null;
            //cboRelation.DataSource = dsList.Tables["Employee"];
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                ////select A.UserID, A.UserName ,  A.UserPass,A.SecQuestion,A.SecAnswer,A.userTypeId,A.userCatId, A.RelId,A.DisplayName ,B.userCatName,C.userTypeName,
		        ///case A.IsInactive When 0 Then 'False' Else 'True' End As IsInactive
	            ////From tblWeb_User As A
                //Setup Grid

                gridList.DisplayLayout.Bands[0].Columns["UserName"].Width = 110;  //User Name
                gridList.DisplayLayout.Bands[0].Columns["DisplayName"].Width = 200;  //Is Inactive
                gridList.DisplayLayout.Bands[0].Columns["userCatName"].Width = 100;  //Is Inactive
                gridList.DisplayLayout.Bands[0].Columns["userTypeName"].Width = 100;  //Is Inactive
                gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Width = 100;  //Is Inactive

                gridList.DisplayLayout.Bands[0].Columns["UserID"].Hidden = true;//User Id
                gridList.DisplayLayout.Bands[0].Columns["RelId"].Hidden = true;  //Is Inactive
                gridList.DisplayLayout.Bands[0].Columns["UserPass"].Hidden = true;  //User Password
                gridList.DisplayLayout.Bands[0].Columns["SecQuestion"].Hidden = true;//Group Id
                gridList.DisplayLayout.Bands[0].Columns["SecAnswer"].Hidden = true;  //Group Name
                gridList.DisplayLayout.Bands[0].Columns["userTypeName"].Hidden = true;  //Is Inactive
                gridList.DisplayLayout.Bands[0].Columns["userCatId"].Hidden = true;  //Is Inactive
                gridList.DisplayLayout.Bands[0].Columns["userTypeId"].Hidden = true;  //Is Inactive

                gridList.DisplayLayout.Bands[0].Columns["UserName"].Header.Caption = "User Name";
                gridList.DisplayLayout.Bands[0].Columns["DisplayName"].Header.Caption = "Display Name";
                gridList.DisplayLayout.Bands[0].Columns["userCatName"].Header.Caption = "User Category";
                gridList.DisplayLayout.Bands[0].Columns["userTypeName"].Header.Caption = "User Type";
                gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Header.Caption = "Inactive";

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Show Check Box Columns
                this.gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        //private void txtUserName_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtUserName);
        //}

        //private void txtUserName_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtUserName);
        //}

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            txtUserName.Text = txtUserName.Text.ToString();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text.ToString();
        }

        //private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtPassword);
        //}

        //private void txtPassword_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtPassword);
        //}

        private void txtConfirmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        //private void txtConfirmPassword_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtConfirmPassword);
        //}

        //private void txtConfirmPassword_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtConfirmPassword);
        //}

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            txtConfirmPassword.Text = txtConfirmPassword.Text.ToString();
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec WebprcGetWebUser " + Common.Classes.clsMain.intUserId + "," + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "User";

            DataRow dr;
            if (dsDetails.Tables["User"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["User"].Rows[0];

                this.txtUserId.Text = dr["UserID"].ToString();
                this.txtUserName.Text = dr["UserName"].ToString();
                this.txtPassword.Text = dr["UserPass"].ToString();
                this.txtConfirmPassword.Text = dr["UserPass"].ToString();
                this.chkInactive.Checked = Boolean.Parse(dr["IsInactive"].ToString());

                this.cboQuestion.Text = dr["SecQuestion"].ToString();
                this.cboAnswer.Text = dr["SecAnswer"].ToString();
                this.txtDisplayName.Text = dr["DisplayName"].ToString();

                this.cboUsType.Value = dr["userTypeId"].ToString();
                this.cboGroup.Value = dr["userCatId"].ToString();
                this.cboRefName.Value = dr["refid"].ToString();
                this.cboRelation.Value = dr["RelId"].ToString();

                //this.chkInactive.CheckedValue = dr["isInactive"].ToString();
                this.txtPassword.Enabled = false;
                this.txtConfirmPassword.Enabled = false;

                this.btnSave.Text = "&Update";
                this.btnDelete.Enabled = true;
            }
        }

        private void prcClearData()
        {
            this.txtUserId.Text = "";
            this.txtUserName.Text = "";
            this.txtDisplayName.Text = "";
            this.txtPassword.Text = "";
            this.txtConfirmPassword.Text = "";


            this.cboQuestion.Text = "";
            this.cboAnswer.Text = "";

            this.cboUsType.Text = "";
            this.cboRefName.Text = "";
            this.cboGroup.Value = null;
            this.cboRelation.Value = null;
            
            this.chkInactive.Checked = false;

            this.txtPassword.Enabled = true;
            this.txtConfirmPassword.Enabled = true;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            this.txtUserName.Focus();
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
            Int32 NewId = 0;

            try
            {
                //Member Master Table
                if (txtUserId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblWeb_User Set UserName = '" + txtUserName.Text.ToString() + "', UserPass='" + txtPassword.Text.ToString() + "', ";
                    sqlQuery = sqlQuery + " SecQuestion ='" + cboQuestion.Text + "', SecAnswer ='" + cboAnswer.Text + "',IsInactive=" + chkInactive.Tag + ", DisplayName ='" + txtDisplayName.Text + "',userTypeId = " + cboUsType.Value + " , userCatId = '" + cboGroup.Value + "',RefId =  " + cboRefName.Value + ",RelId = '" + cboRelation.Value + "'  Where UserID = '" + Int32.Parse(txtUserId.Text)+"'";

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    //if (NewId > 0)
                    //{
                        MessageBox.Show("Data Updated Successfully");
                    //}
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(UserID),0)+1 As NewId from tblWeb_User";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "insert into tblWeb_User  (userId,userName,userPass,SecQuestion,SecAnswer,isInactive,userTypeId,userCatId,RefId,RelId,DisplayName)";
                    sqlQuery = sqlQuery + " Values (" + NewId + ", '" + txtUserName.Text.ToString() + "', '" + txtPassword.Text.ToString() + "','" + cboQuestion.Text.ToString() + "','" + cboAnswer.Text.ToString() + "'," + chkInactive.Tag + "," + cboUsType.Value + " ," + cboGroup.Value + ", " + cboRefName.Value + ",'" + cboRelation.Value + "','" + txtDisplayName.Text.ToString() + "')";

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Saved Successfully");
                    }
                }
                prcClearData();
                txtUserName.Focus();

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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete user information of [" + txtUserName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblWeb_User Where UserID = " + Int32.Parse(txtUserId.Text);
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
                    txtUserName.Focus();

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
            if (this.txtUserName.Text.Length == 0)
            {
                MessageBox.Show("Please provide user name.");
                txtUserName.Focus();
                return true;
            }
            if (this.txtPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide user password.");
                txtPassword.Focus();
                return true;
            }
            if (this.txtConfirmPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide user confirm password.");
                txtConfirmPassword.Focus();
                return true;
            }
            if (this.txtPassword.Text.Trim() != this.txtConfirmPassword.Text.Trim())
            {
                MessageBox.Show("User password & comfirm password should be same.");
                txtPassword.Focus();
                return true;
            }

            if (this.cboGroup.Text.ToString() == "Client")
            {
                if (this.cboRelation.Text.ToString().Length == 0)
                {
                    MessageBox.Show("Please provide Relation.");
                    cboRelation.Focus();
                    return true;
                }
            }


            if (this.cboGroup.Text.Length == 0)
            {
                MessageBox.Show("Please provide under group.");
                cboGroup.Focus();
                return true;
            }
            return false;
        }

        private void cboModule_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
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

        private void txtEmpCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtEmpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboGroup_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGroup.DisplayMember = "name";
            cboGroup.ValueMember = "Id";

            cboGroup.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
            cboGroup.DisplayLayout.Bands[0].Columns["name"].Width = cboGroup.Width;
            cboGroup.DisplayLayout.Bands[0].Columns["name"].Header.Caption = "User Category";
        }

        private void cboRelation_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {


            cboRelation.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
            cboRelation.DisplayLayout.Bands[0].Columns["name"].Width = cboRelation.Width;

            cboRelation.DisplayLayout.Bands[0].Columns["id"].Header.Caption = "Code";
            cboRelation.DisplayLayout.Bands[0].Columns["name"].Header.Caption = "Name";

            cboRelation.DisplayMember = "name";
            cboRelation.ValueMember = "id";
        }

        
        private void txtDisplayName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboUsType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboRefName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboGroup_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        //private void txtDisplayName_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref  txtDisplayName);
        //}

        //private void cboAnswer_Enter(object sender, EventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref cboAnswer);
        //}

        private void txtDisplayName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboQuestion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboUsType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboRefName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboRelation_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtDisplayName_Leave(object sender, EventArgs e)
        {
            txtDisplayName.Text = txtDisplayName.Text.ToString();
        }

        private void cboAnswer_Leave(object sender, EventArgs e)
        {
            //cboAnswer.Text = cboAnswer.Text.ToString();
        }

        //private void txtDisplayName_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref txtDisplayName);
        //}

        //private void cboAnswer_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clsProc.GTRGotFocus(ref cboAnswer);
        //}

        private void cboUsType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboUsType.DisplayMember = "userTypeName";
            cboUsType.ValueMember = "userTypeId";

            cboUsType.DisplayLayout.Bands[0].Columns["userTypeId"].Hidden = true;
            cboUsType.DisplayLayout.Bands[0].Columns["userTypeName"].Width = cboUsType.Width;
            cboUsType.DisplayLayout.Bands[0].Columns["userTypeName"].Header.Caption = "User Type";
        }

        private void cboQuestion_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboQuestion.DisplayMember = "Question";
            cboQuestion.ValueMember = "QuestionID";

            cboQuestion.DisplayLayout.Bands[0].Columns["QuestionID"].Hidden = true;
            cboQuestion.DisplayLayout.Bands[0].Columns["Question"].Width = cboQuestion.Width;
            cboQuestion.DisplayLayout.Bands[0].Columns["Question"].Header.Caption = "Question";
        }

        private void cboGroup_Leave(object sender, EventArgs e)
        {


            if (cboGroup.Text.Length > 0)
            {
                prcLoadlist1(cboGroup.Text.ToString());
            }
            else
            {
              
            }

        }

        private void prcLoadlist1(string txt)
        {
            dsFilter1 = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            string sqlQuery;
            try
            {
                //string str = "%" + txt + "%";
                if (txt == "Employee")
                {
                    sqlQuery = "select empID as ID,'[ '+ empCode +' ] -  ' + empName COLLATE DATABASE_DEFAULT  as Name from tblEmp_Info order by EmpID";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter1, sqlQuery);
                }
                else if (txt == "Client")
                {
                    sqlQuery = "select Custid  as ID,'[ '+ Custcode +' ] -  ' + Custname  COLLATE DATABASE_DEFAULT as Name  from tblCat_CustomerInfo order by custname";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter1, sqlQuery);
                }
                else if (txt == "Supplier")
                {
                    sqlQuery = "select supplierId  as ID,'[ '+ SupplierCode +' ] -  ' + supplierName COLLATE DATABASE_DEFAULT as Name from tblCat_Supplier order by supplierName";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter1, sqlQuery);
                }
                else if (txt == "Dealer")
                {
                    sqlQuery = "select Dlrid  as ID,'[ '+ DlrCode +' ] -  ' + DlrName COLLATE DATABASE_DEFAULT as Name from tblCat_DealerInfo order by DlrName";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter1, sqlQuery);
                }
                
                dsFilter1.Tables[0].TableName = "as";
                cboRefName.DataSource = null;
                cboRefName.DataSource = dsFilter1.Tables["as"];
                cboRefName.DisplayMember = "name";
                cboRefName.ValueMember = "id";

            }
            catch (Exception ex)
            {
               // throw (ex);
            }
        }

        private void cboRefName_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {


            cboRefName.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
            cboRefName.DisplayLayout.Bands[0].Columns["name"].Width = cboRefName.Width;

            cboRefName.DisplayLayout.Bands[0].Columns["id"].Header.Caption = "Code";
            cboRefName.DisplayLayout.Bands[0].Columns["name"].Header.Caption = "Name";

            cboRefName.DisplayMember = "name";
            cboRefName.ValueMember = "id";
        }

        private void cboRefName_Leave(object sender, EventArgs e)
        {

            if (cboGroup.Text != "Employee")
            {
                if (cboRefName.Text.Length > 0)
                {
                    cboRelation.Enabled = true;
                    cboRelation.Focus();
                    prcLoadlist2(cboGroup.Text.ToString(), cboRefName.Value.ToString());
                }
            }
            else {
                cboRelation.Enabled = false;
                cboRelation.Value = 0;

                btnSave.Focus();
            }

            //            "select contID,'[ '+ contCode +' ] -  ' + contName COLLATE DATABASE_DEFAULT as Name from tblCat_Contact where TypeName = 'Customer' and DataID = 685"
        }


        private void prcLoadlist2(string txt , string int1)
        {
            dsFilter2 = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            string sqlQuery;
            try
            {
                //string str = "%" + txt + "%";
                if (txt == "Client" || txt == "Customer")
                {
                    sqlQuery = "select contID as ID,'[ '+ contCode +' ] -  ' + contName COLLATE DATABASE_DEFAULT as Name from tblCat_Contact where TypeName = 'Customer' and DataID =  "+int1+" ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter2, sqlQuery);
                }
                else if (txt == "Supplier")
                {
                    sqlQuery = "select contID as ID,'[ '+ contCode +' ] -  ' + contName COLLATE DATABASE_DEFAULT as Name from tblCat_Contact where TypeName = 'Supplier' and DataID =  " + int1 + "  order by Name";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter2, sqlQuery);
                }
                else if (txt == "Dealer")
                {
                    sqlQuery = "select contID  as ID,'[ '+ contCode +' ] -  ' + contName COLLATE DATABASE_DEFAULT as Name from tblCat_Contact where TypeName = 'Supplier' and DataID =  " + int1 + "  order by Name";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsFilter2, sqlQuery);
                }

                dsFilter2.Tables[0].TableName = "as";
                cboRelation.DataSource = null;
                cboRelation.DataSource = dsFilter2.Tables["as"];
                cboRelation.DisplayMember = "name";
                cboRelation.ValueMember = "id";

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void cboGroup_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (cboGroup.Text.Length > 0)
            {
                if (cboGroup.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide Valid Data[Or Select List]");
                    cboGroup.Value = 0;
                    cboGroup.Text = "";
 
                    cboGroup.Focus();
                }
            }

        }

        private void cboRelation_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cboRelation.Text.Length > 0)
            {
                if (cboRelation.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide Valid Data [ Or Select List]");
                    cboRelation.Value = 0;
                    cboRelation.Text = "";

                    cboRelation.Focus();
                }
            }
        }

        private void cboRefName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cboRefName.Text.Length > 0)
            {
                if (cboRefName.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide Valid Data [ Or Select List]");
                    cboRefName.Value = 0;
                    cboRefName.Text = "";

                    cboRefName.Focus();
                }
            }
        }

        private void cboUsType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cboUsType.Text.Length > 0)
            {
                if (cboUsType.IsItemInList() == false)
                {
                    MessageBox.Show("Please Provide Valid Data [ Or Select List]");
                    cboUsType.Value = 0;
                    cboUsType.Text = "";

                    cboUsType.Focus();
                }
            }
        }

        private void cboRelation_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboGroup_ValueChanged(object sender, EventArgs e)
        {
            if (cboGroup.Text.Length > 0)
            {
                prcLoadlist1(cboGroup.Text.ToString());
            }
            else
            {

            }
        }

        private void cboRefName_ValueChanged(object sender, EventArgs e)
        {

            if (cboGroup.Text != "Employee")
            {
                if (cboRefName.Text.Length > 0)
                {
                    cboRelation.Enabled = true;
                    cboRelation.Focus();
                    prcLoadlist2(cboGroup.Text.ToString(), cboRefName.Value.ToString());
                }
            }
            else
            {
                cboRelation.Enabled = false;
                cboRelation.Value = 0;

                btnSave.Focus();
            }
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

        private void cboFilterFName_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFilterFName.DisplayLayout.Bands[0].Columns["FieldValue"].Hidden = true;
            cboFilterFName.DisplayLayout.Bands[0].Columns["SlNo"].Hidden = true;
            cboFilterFName.DisplayLayout.Bands[0].Columns["FieldName"].Header.Caption = "Field Name";
            cboFilterFName.DisplayLayout.Bands[0].Columns["FieldName"].Width = cboFilterFName.Width;
            cboFilterFName.DisplayMember = "FieldName";
            cboFilterFName.ValueMember = "FieldValue";
        }

        private void cboFilterOperator_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFilterOperator.DisplayLayout.Bands[0].Columns["Operator"].Header.Caption = "Field Name";
            cboFilterOperator.DisplayLayout.Bands[0].Columns["SLno"].Hidden = true;
            cboFilterOperator.DisplayLayout.Bands[0].Columns["Operator"].Width = cboFilterOperator.Width;
            cboFilterOperator.DisplayMember = "Operator";
            cboFilterOperator.ValueMember = "Operator";
        }

        private void txtFilterFValue_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

    }
}
