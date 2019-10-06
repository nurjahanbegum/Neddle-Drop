using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Master
{
    public partial class frmCompany : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        private clsProcedure clsProc = new clsProcedure();
        private clsMain clsM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;

        private Common.FormEntry.frmMaster FM;
        public frmCompany(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmCompany_Load(object sender, EventArgs e)
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
                        clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetCompany 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "CompanyList";
                dsList.Tables[1].TableName = "CompanyCombo";

                gridCompany.DataSource = null;
                gridCompany.DataSource = dsList.Tables["CompanyList"];
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

        private void gridCompany_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide Columns
                gridCompany.DisplayLayout.Bands[0].Columns["comId"].Hidden = true;    //SC Id
                
                //Set Width
                gridCompany.DisplayLayout.Bands[0].Columns["comCode"].Width = 80;       //SC Code
                gridCompany.DisplayLayout.Bands[0].Columns["comName"].Width = 180;      //Name
                gridCompany.DisplayLayout.Bands[0].Columns["comAddress"].Width = 140;      //Address
                gridCompany.DisplayLayout.Bands[0].Columns["comPhone"].Width = 150;      //Phone
                gridCompany.DisplayLayout.Bands[0].Columns["contPerson"].Width = 150;

                //Set Caption
                gridCompany.DisplayLayout.Bands[0].Columns["comId"].Header.Caption = "Sister Concern Id";
                gridCompany.DisplayLayout.Bands[0].Columns["comCode"].Header.Caption = "Code";
                gridCompany.DisplayLayout.Bands[0].Columns["comName"].Header.Caption = "Name";
                gridCompany.DisplayLayout.Bands[0].Columns["comAddress"].Header.Caption = "Address";
                gridCompany.DisplayLayout.Bands[0].Columns["comPhone"].Header.Caption = "Phone";
                gridCompany.DisplayLayout.Bands[0].Columns["contPerson"].Header.Caption = "Contract Person";

                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

                //Change alternate color
                gridCompany.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridCompany.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridCompany.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridCompany.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridCompany.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcClearData()
        {
            //Clear all
            this.txtId.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtCompanyAddress.Text = "";
            this.txtCompanyphone.Text = "";
            this.txtCompanyEmail.Text = "";
            this.txtCompanyFax.Text = "";
            this.txtCompanyAlias.Text = "";
            this.cboCompanyType.Text = "";
            this.txtCompanyWeb.Text = "";
            this.txtCompanyContractP.Text = "";
            this.txtCompanyContractPD.Text = "";
            this.ckdCurrencySymble.CheckedValue = false;
            this.ckdGroup.CheckedValue = false;
            this.ckdZeroBalance.CheckedValue = false;

            this.btnDelete.Enabled = false;

            this.btnSave.Text = "&Save";
            this.txtCode.Focus();
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetCompany " + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();
                        clsConnection clsCon = new clsConnection();

            try
            {
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];

                    this.txtId.Text = dr["comId"].ToString();
                    this.txtCode.Text = dr["Comcode"].ToString();
                    this.txtName.Text = dr["comName"].ToString();
                    this.txtCompanyAddress.Text = dr["comAddress"].ToString();
                    this.txtCompanyphone.Text = dr["comphone"].ToString();
                    this.txtCompanyEmail.Text = dr["comEmail"].ToString();
                    this.txtCompanyFax.Text = dr["comFax"].ToString();
                    this.txtCompanyWeb.Text = dr["comWeb"].ToString();
                    this.cboCompanyType.Text = dr["comType"].ToString();
                    this.txtCompanyAlias.Text = dr["comAlias"].ToString();
                    this.txtCompanyContractP.Text = dr["contPerson"].ToString();
                    this.txtCompanyContractPD.Text = dr["contDesig"].ToString();

                    this.ckdGroup.Checked = false;
                    if (Int16.Parse(dr["IsGroup"].ToString()) == 1)
                    {
                        this.ckdGroup.Checked = true;
                    }
                    this.ckdCurrencySymble.Checked = false;
                    if (Int16.Parse(dr["IsShowCurrencySymbol"].ToString()) == 1)
                    {
                        this.ckdCurrencySymble.Checked = true;
                    }

                    this.ckdZeroBalance.Checked = false;
                    if (Int16.Parse(dr["IsShowZeroBalance"].ToString()) == 0)
                    {
                        this.ckdZeroBalance.Checked = true;
                    }

                    this.btnSave.Text = "&Update";
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

        private void prcLoadCombo()
        {
            try
            {
                cboCompanyType.DataSource = null;
                cboCompanyType.DataSource = dsList.Tables["CompanyCombo"];
                cboCompanyType.DisplayMember = "varName";
                cboCompanyType.ValueMember = "varName";

                //Set Width
                cboCompanyType.DisplayLayout.Bands[0].Columns["varName"].Width = 250;
                //Set Caption
                cboCompanyType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Business Type";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void gridCompany_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if(gridCompany.ActiveRow.IsFilterRow == true )
                {
                    return;
                }

                prcClearData();
                prcDisplayDetails(gridCompany.ActiveRow.Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Boolean fncBlank()
        {
            if (this.txtCode.Text.Length == 0)
            {
                MessageBox.Show("Please provide sister concern code.");
                txtCode.Focus();
                return true;
            }
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide sister concern name.");
                txtName.Focus();
                return true;
            }

            if (this.txtCompanyAddress.Text.Length == 0)
            {
                MessageBox.Show("Please provide sister concern Address.");
                txtCompanyAddress.Focus();
                return true;
            }

            //if (this.txtCompanyContractP.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide contract person.");
            //    txtCompanyContractP.Focus();
            //    return true;
            //}

            //if (this.txtCompanyContractPD.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide designation of contact person.");
            //    txtCompanyContractPD.Focus();
            //    return true;
            //}

            //if (this.cboCompanyType.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide business type.");
            //    cboCompanyType.Focus();
            //    return true;
            //}

            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete sister concern information of [" + txtName.Text + "]", "",System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
                        clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";
                sqlQuery = "Delete from tblCat_Company Where comId = " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                txtCode.Focus();

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
                //Member Master Table
                if (btnSave.Text.ToString() != "&Save")
                {
                    // Update
                    sqlQuery = " Update tblCat_Company Set comcode = '" + txtCode.Text.ToString() + "', comName='" +
                               txtName.Text.ToString() + "',comAddress='" + txtCompanyAddress.Text.Trim() +
                               "',comPhone='" + txtCompanyphone.Text.Trim() + "',comFax='" + txtCompanyFax.Text.Trim() +
                               "',comEmail='" + txtCompanyEmail.Text.Trim() + "',comweb='" + txtCompanyWeb.Text.Trim() +
                               "',comAlias='" + txtCompanyAlias.Text.Trim() + "',contPerson='" +
                               txtCompanyContractP.Text.Trim() + "',contDesig='" + txtCompanyContractPD.Text.Trim() +
                               "',comType='" + cboCompanyType.Value + "',IsShowCurrencySymbol=" + ckdCurrencySymble.Tag +
                               ",IsGroup=" + ckdGroup.Tag + ",IsShowZeroBalance=" + ckdZeroBalance.Tag + ""
                               + " Where comId = " + Int32.Parse(txtId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    //Generating New Id
                    sqlQuery = "Select Isnull(Max(comId),0)+1 As NewId from tblCat_Company";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert data
                    sqlQuery = "Insert Into tblCat_Company (comId, aId, comcode, comName,comAddress,comPhone,comFax,comEmail,comWeb,comType,comAlias,contPerson,contDesig,IsGroup,IsShowCurrencySymbol,IsShowZeroBalance)"
                               + " Values (" + NewId + ", " + NewId + ", '" + txtCode.Text.ToString() + "', '" +
                               txtName.Text.ToString() + "','" + txtCompanyAddress.Text.ToString() + "','" +
                               txtCompanyphone.Text.ToString() + "','" + txtCompanyFax.Text.ToString() + "','" +
                               txtCompanyEmail.Text.ToString() + "','" + txtCompanyWeb.Text.ToString() + "','" +
                               cboCompanyType.Value + "','" + txtCompanyAlias.Text.ToString() + "','" +
                               txtCompanyContractP.Text.ToString() + "','" + txtCompanyContractPD.Text.ToString() + "'," +
                               ckdGroup.Tag + "," + ckdCurrencySymble.Tag + ", " + ckdZeroBalance.Tag + ")";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }
                prcClearData();
                txtCode.Focus();

                prcLoadList();
                //prcLoadCombo();
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

        private void ckdGroup_CheckedChanged(object sender, EventArgs e)
        {
            ckdGroup.Tag = 0;
            if (ckdGroup.Checked == true)
            {
                ckdGroup.Tag = "1";
            }
        }

        private void ckdCurrencySymble_CheckedChanged(object sender, EventArgs e)
        {
            ckdCurrencySymble.Tag = 0;
            if (ckdCurrencySymble.Checked == true)
            {
                ckdCurrencySymble.Tag = "1";
            }
        }

        private void ckdZeroBalance_CheckedChanged(object sender, EventArgs e)
        {
            ckdZeroBalance.Tag = 0;
            if (ckdZeroBalance.Checked == true)
            {
                ckdZeroBalance.Tag = "1";
            }
        }

        private void frmCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCode);
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtName);
        }

        private void txtCompanyAddress_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyAddress);
        }

        private void txtCompanyphone_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyphone);
        }

        private void txtCompanyFax_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyFax);
        }

        private void txtCompanyEmail_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyEmail);
        }

        private void txtCompanyWeb_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyWeb);
        }

        private void cboCompanyType_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCode);
        }

        private void txtCompanyAlias_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyAlias);
        }

        private void txtCompanyContractP_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyContractP);
        }

        private void txtCompanyContractPD_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtCompanyContractPD);
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyAddress_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyphone_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyFax_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyEmail_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyWeb_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCompanyType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyAlias_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyContractP_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCompanyContractPD_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyFax_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyWeb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboCompanyType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyAlias_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyContractP_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCompanyContractPD_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboCompanyType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cboCompanyType.Text.Length > 0)
            {
                if (cboCompanyType.IsItemInList() == false)
                {
                    MessageBox.Show("Please provide valid data [or select from list]");
                    cboCompanyType.Focus();
                }
            }
        }
        
    }
}
