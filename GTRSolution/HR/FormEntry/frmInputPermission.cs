using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using GTRLibrary;
using Infragistics.Win.UltraWinGrid;


namespace GTRHRIS.HR.FormEntry
{
    public partial class frmInputPermission : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;


        public frmInputPermission(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmInputPermission_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmInputPermission_Load(object sender, EventArgs e)
        {
            try
            {
                lblCaption.Text = this.Tag + " Entry ...";
                prcLoad();
                //prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetPermission_Input 1,'" + cboType.Text.ToString() + "','" + cboPName.Text.ToString() + "','" + this.Tag.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "Varible";
                dsList.Tables[1].TableName = "tblEmpType";
                dsList.Tables[2].TableName = "tblPName";
                dsList.Tables[3].TableName = "tblComp";
                dsList.Tables[4].TableName = "tblFirstId";
                dsList.Tables[5].TableName = "tblFinalId";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Varible"];
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

        public void prcLoad()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetPermission_Input  0,'','','" + this.Tag.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "Varible";
                dsList.Tables[1].TableName = "tblEmpType";
                dsList.Tables[2].TableName = "tblPName";
                //dsList.Tables[3].TableName = "tblComp";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Varible"];
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
            cboType.DataSource = dsList.Tables["tblEmpType"];

            cboPName.DataSource = null;
            cboPName.DataSource = dsList.Tables["tblPName"];

            //uddComp.DataSource = null;
            //uddComp.DataSource = dsList.Tables["tblComp"];
            //gridList.DisplayLayout.Bands[0].Columns["tblComp"].ValueList = uddComp;

            //uddDateType.DataSource = null;
            //uddDateType.DataSource = dsList.Tables["tblDateType"];
            //gridList.DisplayLayout.Bands[0].Columns["DateType"].ValueList = uddDateType;
        }

        public void prcLoadComboList()
        {
            cboType.DataSource = null;
            cboType.DataSource = dsList.Tables["tblEmpType"];
            
            cboPName.DataSource = null;
            cboPName.DataSource = dsList.Tables["tblPName"];

            uddComp.DataSource = null;
            uddComp.DataSource = dsList.Tables["tblComp"];
            gridList.DisplayLayout.Bands[0].Columns["ComName"].ValueList = uddComp;

            uddFirstId.DataSource = null;
            uddFirstId.DataSource = dsList.Tables["tblFirstId"];
            gridList.DisplayLayout.Bands[0].Columns["FirstAppId"].ValueList = uddFirstId;

            uddFinalId.DataSource = null;
            uddFinalId.DataSource = dsList.Tables["tblFinalId"];
            gridList.DisplayLayout.Bands[0].Columns["FinalAppId"].ValueList = uddFinalId;
        }

        public void prcDisplayDetails(string strParam)
        {
        }

        public void prcClearData()
        {
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }



        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridList.DisplayLayout.Bands[0].Columns["PId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["PType"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["Flag"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["ComId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["AppFirst"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["AppFinal"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["FirstAppIdPrev"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["FinalAppIdPrev"].Hidden = true;


            //Set Caption
            //gridList.DisplayLayout.Bands[0].Columns["SubBand"].Header.Caption = this.Tag.ToString();
            gridList.DisplayLayout.Bands[0].Columns["ComName"].Header.Caption = "Company Name";
            gridList.DisplayLayout.Bands[0].Columns["FirstAppId"].Header.Caption = "First Approve Id";
            gridList.DisplayLayout.Bands[0].Columns["FinalAppId"].Header.Caption = "Final Approve Id";
            gridList.DisplayLayout.Bands[0].Columns["isInactive"].Header.Caption = "isInactive";
            gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["ComName"].Width = 220;
            gridList.DisplayLayout.Bands[0].Columns["FirstAppId"].Width = 120;
            gridList.DisplayLayout.Bands[0].Columns["FinalAppId"].Width = 120;
            gridList.DisplayLayout.Bands[0].Columns["isInactive"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["Remarks"].Width = 220;

            //Change alternate color
            gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //this.gridList.DisplayLayout.Bands[0].Columns["dtDate"].Format = "dd-MMM-yyyy";

            gridList.DisplayLayout.Bands[0].Columns["isInactive"].Style =
               Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            //gridList.DisplayLayout.Bands[0].Columns["aId"].Style =
            //   Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerWithSpin;

            gridList.DisplayLayout.Bands[0].Columns["ComName"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;
            gridList.DisplayLayout.Bands[0].Columns["FirstAppId"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;
            gridList.DisplayLayout.Bands[0].Columns["FinalAppId"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;


            //Select Full Row when click on any cell
            //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            //gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dsList.Tables["Varible"].Rows.Count.ToString());
            for (int rowCount = dsList.Tables["Varible"].Rows.Count - 1; rowCount >= 0; rowCount--)
            {
                if (dsList.Tables["Varible"].Rows[rowCount][0].ToString().Trim().Length == 0)
                {
                    dsList.Tables["Varible"].Rows[rowCount].Delete();
                }
            }
        }

        public Boolean fncBlank(string tbl, int Rowno)
        {
            //if (dsList.Tables[tbl].Rows[Rowno][2].ToString().Length == 0)
            //{
            //    MessageBox.Show("Provide " + this.Tag + " Name.");
            //    return true;
            //}

            //if (dsList.Tables[tbl].Rows[Rowno][4].ToString().Length == 0)
            //{
            //    MessageBox.Show("Provide First Approval ID.");
            //    return true;
            //}

            //if (dsList.Tables[tbl].Rows[Rowno][5].ToString().Length == 0)
            //{
            //    MessageBox.Show("Provide Final Approval ID.");
            //    return true;
            //}


            //if (dsList.Tables[tbl].Rows[Rowno][4].ToString().Length == 0)
            //{
            //    dsList.Tables[tbl].Rows[Rowno][4] = 0;
            //}

            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;
            try
            {
                int rowCount;
                for (rowCount = 0; rowCount < dsList.Tables["Varible"].Rows.Count; rowCount++)
                {
                    if (fncBlank("Varible", rowCount))
                    {
                        return;
                    }
                }

                for (rowCount = 0; rowCount < dsList.Tables["Varible"].Rows.Count; rowCount++)
                {

                    if (dsList.Tables["Varible"].Rows[rowCount][0].ToString().Trim().Length > 0 &&
                        dsList.Tables["Varible"].Rows[rowCount]["Flag"].ToString() == "1")
                    {
                        //Update Table
                        sqlQuery = "Update tblInput_Permission Set FirstAppId = '" + dsList.Tables["Varible"].Rows[rowCount][4] +
                                   "', FinalAppId = '" + dsList.Tables["Varible"].Rows[rowCount][5] + "', isInactive = '" +
                                   dsList.Tables["Varible"].Rows[rowCount][8] + "', Remarks = '" +
                                   dsList.Tables["Varible"].Rows[rowCount][9] + "', AppFirst = 0, AppFinal=1,LUserId = " + 
                                   Common.Classes.clsMain.intUserId + ",PCName ='" + 
                                   Common.Classes.clsMain.strComputerName + "' where PId = '" +
                                   dsList.Tables["Varible"].Rows[rowCount][0] + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblInput_Permission Set AppFirst = 1,AppFinal = 1 Where FirstAppId = FinalAppId and PId ='" +
                                    dsList.Tables["Varible"].Rows[rowCount][0] + "'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblEmpApprove_Info Set FirstAppId = '" + dsList.Tables["Varible"].Rows[rowCount]["FirstAppId"] + "' Where FirstAppId = '" + dsList.Tables["Varible"].Rows[rowCount]["FirstAppIdPrev"] + "' and Approved = 0";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblEmpApprove_Info Set FinalAppId = '" + dsList.Tables["Varible"].Rows[rowCount]["FinalAppId"] + "' Where FinalAppId ='" + dsList.Tables["Varible"].Rows[rowCount]["FinalAppIdPrev"] + "' and Approved = 0";
                        arQuery.Add(sqlQuery);

                    }

                    else if (dsList.Tables["Varible"].Rows[rowCount][0].ToString().Trim().Length == 0)
                    {
                        // Insert To Table
                        sqlQuery = "Insert into tblInput_Permission(PType,EmpType,ComName,FirstAppId,FinalAppId,AppFirst,AppFinal,ComId,isInactive,Remarks,LUserId,PCName) values('" +
                                   cboPName.Text.ToString() + "','" + cboType.Text.ToString() + "', '" +     
                                   dsList.Tables["Varible"].Rows[rowCount][3] + "','" +
                                   dsList.Tables["Varible"].Rows[rowCount][4] + "','" +
                                   dsList.Tables["Varible"].Rows[rowCount][5] + "',0,1,0,'" +
                                   dsList.Tables["Varible"].Rows[rowCount][8] + "','" +
                                   dsList.Tables["Varible"].Rows[rowCount][9] + "'," + 
                                   Common.Classes.clsMain.intUserId + ",'" + 
                                   Common.Classes.clsMain.strComputerName + "')";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblInput_Permission Set AppFirst = 1,AppFinal = 1 Where FirstAppId = FinalAppId and PType = '" + 
                                    cboPName.Text.ToString() + "' and EmpType = '" + cboType.Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update P Set P.ComId = C.ComId from tblInput_Permission P,tblCat_Company C Where P.ComName COLLATE DATABASE_DEFAULT = C.ComName and P.ComId = 0 and P.PType = '" +
                                    cboPName.Text.ToString() + "' and P.EmpType = '" + cboType.Text.ToString() + "'";
                        arQuery.Add(sqlQuery);
                    }


                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved [Or/And ] Update Succefully");

                //prcClearData();
                prcLoadList();
                prcLoadComboList();
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

        private void gridList_AfterCellUpdate(object sender, CellEventArgs e)
        {
            gridList.ActiveRow.Cells[10].Value = 1;
            btnSave.Enabled = true;
        }

        private void gridList_AfterCellActivate(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete " + this.Tag + " information of [" + gridList.ActiveRow.Cells[1].Text.ToString() + "]", "",
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
                sqlQuery = "Delete From tblInput_Permission where PId = " + Int32.Parse(gridList.ActiveRow.Cells[0].Value.ToString());
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                //prcClearData();
                prcLoadList();
                prcLoadComboList();

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


        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dsList.Tables["Varible"].NewRow();

            dsList.Tables["Varible"].Rows.Add(dr);
        }

        private Boolean fncBlank()
        {
            if (this.cboType.Text.Length == 0)
            {
                MessageBox.Show("Please Provide Office Grade.");
                cboType.Focus();
                return true;
            }

            //if (this.cboGrade.IsItemInList() == false)
            //{
            //    MessageBox.Show("Please provide valid Band [or, select from list item].");
            //    cboBand.Focus();
            //    return true;
            //}
            return false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            if (fncBlank())
            {
                return;
            }
            
            try
            {

                prcLoadList();
                prcLoadComboList();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void cboPName_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPName.DisplayLayout.Bands[0].Columns["PName"].Width = cboPName.Width;
            cboPName.DisplayLayout.Bands[0].Columns["PName"].Header.Caption = "Permission Name";
            cboPName.DisplayLayout.Bands[0].Columns["SL"].Hidden = true;
            cboPName.DisplayMember = "PName";
            cboPName.ValueMember = "PName";
        }

        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["varName"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Emp Type";
            cboType.DisplayLayout.Bands[0].Columns["SL"].Hidden = true;
            cboType.DisplayMember = "varName";
            cboType.ValueMember = "varName";
        }


        private void uddComp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddComp.DisplayLayout.Bands[0].Columns["ComId"].Hidden = true;
            uddComp.DisplayLayout.Bands[0].Columns["ComName"].Width = 220;
            uddComp.DisplayLayout.Bands[0].Columns["ComName"].Header.Caption = "Company Name";
            uddComp.DisplayMember = "ComName";
            uddComp.ValueMember = "ComId";
        }
        private void uddComp_RowSelected(object sender, RowSelectedEventArgs e)
        {
            if (uddComp.ActiveRow == null)
            {
                return;
            }

            gridList.ActiveRow.Cells["ComName"].Value = uddComp.ActiveRow.Cells["ComName"].Value.ToString();
        }

        private void uddFirstId_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddFirstId.DisplayLayout.Bands[0].Columns["FirstAppId"].Hidden = true;
            //uddFirstId.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 70;
            //uddFirstId.DisplayLayout.Bands[0].Columns["EmpName"].Width = 220;
            //uddFirstId.DisplayLayout.Bands[0].Columns["EmpName"].Width = 220;
            //uddFirstId.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";
            //uddFirstId.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
            uddFirstId.DisplayMember = "FirstAppId";
            uddFirstId.ValueMember = "FirstAppId";
        }

        private void uddFirstId_RowSelected(object sender, RowSelectedEventArgs e)
        {
            if (uddFirstId.ActiveRow == null)
            {
                return;
            }

            gridList.ActiveRow.Cells["FirstAppId"].Value = uddFirstId.ActiveRow.Cells["FirstAppId"].Value.ToString();
        }


        private void uddFinalId_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddFinalId.DisplayLayout.Bands[0].Columns["FinalAppId"].Hidden = true;
            uddFinalId.DisplayMember = "FinalAppId";
            uddFinalId.ValueMember = "FinalAppId";
        }

        private void uddFinalId_RowSelected(object sender, RowSelectedEventArgs e)
        {
            if (uddFinalId.ActiveRow == null)
            {
                return;
            }

            gridList.ActiveRow.Cells["FinalAppId"].Value = uddFinalId.ActiveRow.Cells["FinalAppId"].Value.ToString();
        }








    }
}
