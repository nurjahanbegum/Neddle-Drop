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

namespace GTRHRIS.Master
{
    public partial class frmUserSalary : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();
        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private int secId_update = 0; // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmUserSalary(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmUserSalary_FormClosing(object sender, FormClosingEventArgs e)
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



        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetPermission_Salary  " + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblSalary";


                dsList.Tables[1].TableName = "tblName";
                dsList.Tables[2].TableName = "tblComp";
                prcModifyDataset();
                prcModifyDatasetCombo();

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblSalary"];
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
            cboName.DataSource = null;
            cboName.DataSource = dsList.Tables["tblName"];
            //cboName.Text = clsProc.GTRDecryptWord(dr["LUserName"].ToString());

            cboComp.DataSource = null;
            cboComp.DataSource = dsList.Tables["tblComp"];

            //cboDesig.DataSource = null;
            //cboDesig.DataSource = dsList.Tables["tblDesig"];
        }

        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetPermission_Salary  " + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, sqlQuery);
                dsDetail.Tables[0].TableName = "tblSalary";
                DataRow dr;

                if (dsDetail.Tables["tblSalary"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["tblSalary"].Rows[0];

                    txtMenu.Text = dr["aID"].ToString();
                    txtId.Text = dr["LUserID"].ToString();
                    txtUserName.Text = dr["LUserName"].ToString();
                    cboName.Text = clsProc.GTRDecryptWord(dr["LUserName"].ToString());
                    //cboName.Text = dr["LUserName"].ToString();
                    cboComp.Text = dr["comName"].ToString();

                    //txtEmpName.Text = dr["EmpName"].ToString();
                    txtAmount.Text = dr["Amount"].ToString();


                    if (dr["isActiveSalary"].ToString() == "1")
                    {
                        chkActiveSalary.Checked = true;
                    }
                    else
                    {
                        chkActiveSalary.Checked = false;
                    }

                    if (dr["isActiveSalaryLess"].ToString() == "1")
                    {
                        chkActiveSalaryLess.Checked = true;
                    }
                    else
                    {
                        chkActiveSalaryLess.Checked = false;
                    }

                    if (dr["isActiveSalaryOver"].ToString() == "1")
                    {
                        chkActiveSalaryOver.Checked = true;
                    }
                    else
                    {
                        chkActiveSalaryOver.Checked = false;
                    }

                    this.btnSave.Text = "&Update";
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
            txtMenu.Text = "";
            txtId.Text = "";
            txtUserName.Text = "";
            //txtEmpName.Text = "";
            txtAmount.Text = "";

            cboName.Value = "";
            cboComp.Value = "";


            chkActiveSalary.Checked = false ;
            chkActiveSalaryLess.Checked = false;
            chkActiveSalaryOver.Checked = false;

            btnSave.Text = "&Save";

        }

        private void frmUserSalary_Load(object sender, EventArgs e)
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
            gridList.DisplayLayout.Bands[0].Columns["aID"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["LuserID"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["LUserName"].Header.Caption = "User Name";
            gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
            gridList.DisplayLayout.Bands[0].Columns["comName"].Header.Caption = "Company Name";
            gridList.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Amount";
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalary"].Header.Caption = "Non Mgt Active";
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalaryLess"].Header.Caption = "Mgt Less Active";
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalaryOver"].Header.Caption = "Mgt Over Active";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["LUserName"].Width = 70;
            gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 130;
            gridList.DisplayLayout.Bands[0].Columns["comName"].Width = 200;
            gridList.DisplayLayout.Bands[0].Columns["Amount"].Width = 70;
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalary"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalaryLess"].Width = 105;
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalaryOver"].Width = 105;

            //Set column Style
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalary"].Style = ColumnStyle.CheckBox;
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalaryLess"].Style = ColumnStyle.CheckBox;
            gridList.DisplayLayout.Bands[0].Columns["isActiveSalaryOver"].Style = ColumnStyle.CheckBox;


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
            //e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void cboName_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboName.DisplayLayout.Bands[0].Columns["LUserName"].Width = cboName.Width;
            cboName.DisplayLayout.Bands[0].Columns["LUserName"].Header.Caption = "User Name";
            cboName.DisplayLayout.Bands[0].Columns["LUserID"].Hidden = true;
            cboName.DisplayMember = "LUserName";
            cboName.ValueMember = "LUserID";
        }

        private void cboComp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboComp.DisplayLayout.Bands[0].Columns["comName"].Width = cboName.Width;
            cboComp.DisplayLayout.Bands[0].Columns["comName"].Header.Caption = "Company Name";
            cboComp.DisplayLayout.Bands[0].Columns["ComID"].Hidden = true;
            cboComp.DisplayMember = "comName";
            cboComp.ValueMember = "ComID";
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            try
            {

                if (btnSave.Text != "&Save")
                {

                    sqlQuery = "Update tblLogin_UserSalary Set Amount = '" + txtAmount.Text.ToString().Trim() +
                               "', isActiveSalary = '" + chkActiveSalary.Tag.ToString() + "',isActiveSalaryLess = '" +
                               chkActiveSalaryLess.Tag.ToString() + "',isActiveSalaryOver = '" +
                               chkActiveSalaryOver.Tag.ToString() + "',ComID = " + this.cboComp.Value.ToString() + "  where aID = " + int.Parse(txtMenu.Text.ToString().Trim()) + "";
                    //----------------------
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");


                }
                else
                {
                    //Insert
                    //--------------------
                    sqlQuery = "Insert into tblLogin_UserSalary (LUserID,EmpID,isActiveSalary,isActiveSalaryLess,isActiveSalaryOver,ComID,Amount)"
                    + " Values (" + this.cboName.Value.ToString() + ",0,'" + chkActiveSalary.Tag.ToString() 
                                   + "','" + chkActiveSalaryLess.Tag.ToString() + "','" + chkActiveSalaryOver.Tag.ToString() 
                                   + "'," + this.cboComp.Value.ToString() + ",'" + txtAmount.Text.ToString().Trim() +"')";
                    //----------------------
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','Update')";
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


        private void btnCancel_Click(object sender, EventArgs e)
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();
                btnSave.Enabled = true;
            }


        private void gridList_DoubleClick (object sender, EventArgs e)
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
            }


        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtEmpNamee_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void chkIsInactive_CheckedChanged(object sender, EventArgs e)
        {
            chkActiveSalary.Tag = 0;
            if (chkActiveSalary.Checked == true)
            {
                chkActiveSalary.Tag = 1;
            }
        }

        private void chkActiveSalaryLess_CheckedChanged(object sender, EventArgs e)
        {
            chkActiveSalaryLess.Tag = 0;
            if (chkActiveSalaryLess.Checked == true)
            {
                chkActiveSalaryLess.Tag = 1;
            }
        }

        private void chkActiveSalaryOver_CheckedChanged(object sender, EventArgs e)
        {
            chkActiveSalaryOver.Tag = 0;
            if (chkActiveSalaryOver.Checked == true)
            {
                chkActiveSalaryOver.Tag = 1;
            }
        }

        //private Boolean fncBlank()
        //{

        //    if (this.cboName.Text.Length == 0)
        //    {
        //        MessageBox.Show("Please provide employee name");
        //        cboName.Focus();
        //        return true;
        //    }
        //}

        public void prcModifyDataset()
        {
            for (int i = 0; i <= dsList.Tables[0].Rows.Count - 1; i++)
            {
                dsList.Tables[0].Rows[i]["LUserName"] = clsProc.GTRDecryptWord(dsList.Tables[0].Rows[i]["LUserName"].ToString());
            }
        }

        public void prcModifyDatasetCombo()
        {
            for (int i = 0; i <= dsList.Tables[1].Rows.Count - 1; i++)
            {
                dsList.Tables[1].Rows[i]["LUserName"] = clsProc.GTRDecryptWord(dsList.Tables[1].Rows[i]["LUserName"].ToString());
            }
        }







        }
   }


