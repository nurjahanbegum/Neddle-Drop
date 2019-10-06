using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;
namespace GTRHRIS.HK.FormEntry
{
    public partial class frmDesignation : Form
    
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmDesignation(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmDesignation_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetDesig 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Desig";
                dsList.Tables[1].TableName = "tblGrade";

                gridUnit.DataSource = null;
                gridUnit.DataSource = dsList.Tables["Desig"];
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

        private void prcClearData()
        {
            this.txtUnitId.Text = "";
            this.txtName.Text = "";
            this.txtNameB.Text = "";
            this.txtBonus.Text = "0";
            cboGrade.Text = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            this.txtName.Focus();
        }

        private Boolean fncBlank()
        {
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide designation name.");
                txtName.Focus();
                return true;
            }

            if (this.cboGrade.Text.Length == 0)
            {
                MessageBox.Show("Provide Grade.");
                cboGrade.Focus();
                return true;
            }
            return false;
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetDesig " + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            clsConnection clsCon = new clsConnection();

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "Details";

            DataRow dr;
            if (dsDetails.Tables["Details"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["Details"].Rows[0];

                this.txtUnitId.Text = dr["DesigId"].ToString();
                this.txtName.Text = dr["DesigName"].ToString();
                this.txtNameB.Text = dr["DesigNameB"].ToString();
                this.txtBonus.Text = dr["AttBonus"].ToString();
                this.cboGrade.Text = dr["Grade"].ToString();

                this.btnSave.Text = "&Update";
                this.btnDelete.Enabled = true;
            }
        }

        private void gridUnit_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridUnit.DisplayLayout.Bands[0].Columns[0].Hidden = true;//Unit Id
                gridUnit.DisplayLayout.Bands[0].Columns[1].Width = 260;  //Unit Name

                gridUnit.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Desig Id";
                gridUnit.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Desig Name";
                gridUnit.DisplayLayout.Bands[0].Columns[1].Header.Caption = "AttBonus";

                //Change alternate color
                gridUnit.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridUnit.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridUnit.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridUnit.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridUnit.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
                //Filter Contains Deafault
                e.Layout.Override.FilterOperatorDefaultValue = FilterOperatorDefaultValue.Contains;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridUnit_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridUnit.ActiveRow.Cells[0].Value.ToString());
        }

        private void frmDesignation_Load(object sender, EventArgs e)
        {
            prcLoadList();
            prcLoadCombo();
        }

        public void prcLoadCombo()
        {

            cboGrade.DataSource = null;
            cboGrade.DataSource = dsList.Tables["tblGrade"];

        }

        private void txtName_MouseClick(object sender, MouseEventArgs e)
        {
            ////clsProc.GTRGotFocus(ref txtName);
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            txtName.Text = txtName.Text.ToString();
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            ////clsProc.GTRGotFocus(ref txtName);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            ArrayList arQuery=new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;
            try
            {
                //Member Master Table
                if (txtUnitId.Text.Length != 0)
                {
                    //Update data
                    sqlQuery = " Update tblCat_Desig Set DesigName = '" + txtName.Text.ToString() + "', DesigNameB = '" + txtNameB.Text.ToString() + "', AttBonus = '" + txtBonus.Text.ToString() + "',Grade = '" + cboGrade.Value.ToString() + "', PCName = '" + Common.Classes.clsMain.strComputerName + "', LUserId =" + Common.Classes.clsMain.intUserId + "  " +
                               " Where DesigId = " + Int32.Parse(txtUnitId.Text);
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
                    //NewId
                    sqlQuery = "Select Isnull(Max(DesigId),0)+1 As NewId from tblCat_Desig";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert to Unit table
                    sqlQuery = "Insert Into tblCat_Desig(DesigId, aId, DesigName,DesigNameB,AttBonus,Grade, PCName, LUserId) "
                        + " Values (" + NewId + ", " + NewId + ", '" + txtName.Text.ToString() + "', '" + txtNameB.Text.ToString() + "','" + txtBonus.Text.ToString() + "','" + cboGrade.Value.ToString() + "','" + Common.Classes.clsMain.strComputerName + "'," + Common.Classes.clsMain.intUserId + ")";
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
                txtName.Focus();

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete designation  of [" + txtName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";
                sqlQuery = "Delete from tblCat_Desig Where DesigId = " + Int32.Parse(txtUnitId.Text);
                arQuery.Add(sqlQuery);
                
                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                    + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcClearData();
                txtName.Focus();

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

        private void cboGrade_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Width = cboGrade.Width;
            cboGrade.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";
            cboGrade.DisplayMember = "Grade";
            cboGrade.ValueMember = "Grade";
        }
    }
}