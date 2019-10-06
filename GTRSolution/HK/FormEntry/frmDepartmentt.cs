using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmDepartmentt : Form
    {
        string strTranWith = "";
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();
        clsMain clsMain = new clsMain();
        private int secId_update = 0;  // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmDepartmentt(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmDepartmentt_Load(object sender, EventArgs e)
        {
            try
            {
                Tree.BackColor = this.BackColor;
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmDepartmentt_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void frmDepartmentt_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            clsProc = null;
        }

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetDepartment " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Tree";
                dsList.Tables[1].TableName = "Combo";

                Tree.Nodes.Clear();
                prcGenerateTreeView(Tree.Nodes, 0, dsList.Tables[0]);
                Tree.Select(); 
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
            cboUnder.DataSource = null;
            cboUnder.DataSource = dsList.Tables["Combo"];

            cboUnder.ValueMember = "DeptId";
            cboUnder.DisplayMember = "DeptName";
        }

        private void prcClearData()
        {
            txtCode.Text="";
            txtName.Text = "";
            txtNameB.Text = "";
            cboUnder.Value = null;
            optType.Enabled = true;
            optType.CheckedIndex = 0;

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;

            optType.Focus();
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
            if (MessageBox.Show("Do you want to delete information of [" + txtName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            try
            {
                //
                string sqlQuery = "Exec prcTran_Dept 'DELETE', " + Common.Classes.clsMain.intComId + ", " +
                           Int32.Parse(txtCode.Tag.ToString()) + ",'" + txtCode.Text.ToString() + "', '','" +
                           optType.Tag.ToString() + "', " + cboUnder.Value + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into gtrHris.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction With Database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                
                prcClearData();
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
                arQuery = null;
            }
        }

        private void optType_ValueChanged(object sender, EventArgs e)
        {
            optType.Tag = optType.Value.ToString();
            txtNameB.Visible = false;
            ultraLabel3.Visible = false;
            if (optType.Tag.ToString() == "L")
            {
                txtNameB.Visible = true;
                ultraLabel3.Visible = true;
            }
        }

        private Boolean fncBlank()
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide warehouse name.");
                txtName.Focus();
                return true;                    
            }
            //if (txtNameShort.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide name name [short].");
            //    txtNameShort.Focus();
            //    return true;
            //}
            if (cboUnder.Text.Length == 0)
            {
                MessageBox.Show("Please provide under group name.");
                cboUnder.Focus();
                return true;
            }
            if (cboUnder.IsItemInList() == false)
            {
                MessageBox.Show("Please provide valid group from under group list [or select from list].");
                cboUnder.Focus();
                return true;
            }
            if(optType.Tag.ToString()=="L")
            {
                if(cboUnder.Value.ToString()=="0")
                {
                    MessageBox.Show("Please provide valid group from under group list [Base only to create group].");
                    cboUnder.Focus();
                    return true;
                }
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
            try
            {
                //Member Master Table
                if (btnSave.Text != "&Save")
                {
                    //Update
                    sqlQuery = "Exec prcTran_Dept 'Update', " + Common.Classes.clsMain.intComId + ", " +
                               Int32.Parse(txtCode.Tag.ToString()) + ", '" + txtName.Text + "', '" +
                               optType.Tag.ToString() + "', " + cboUnder.Value + ", " +
                               Common.Classes.clsMain.strComputerName + ", " + Common.Classes.clsMain.intUserId + "";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into GTRHRIS.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);
                    
                    //Update Section
                    if (optType.Tag.ToString() == "L")
                    {
                        sqlQuery = "Update tblCat_Section Set SectName = '" + txtName.Text.ToString().Trim() +
                                   "', SectNameB = '" + txtNameB.Text.ToString().Trim() + "',DeptID = " + cboUnder.Value +
                                   "  where SectId = '" + secId_update + "'";
                        arQuery.Add(sqlQuery);


                        // Insert Information To Log File
                        sqlQuery = "Insert Into GTRHRIS.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" +
                                   sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                    }



                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {

                    //add new Department
                    sqlQuery = "Exec prcTran_Dept 'NEW', " + Common.Classes.clsMain.intComId + ", 0, '" + txtName.Text +
                               "',  '" + optType.Tag.ToString() + "', " + cboUnder.Value + ", " +
                               Common.Classes.clsMain.strComputerName + ", " + Common.Classes.clsMain.intUserId + "";
                    arQuery.Add(sqlQuery);


                    // Insert Information To Log File
                    sqlQuery = "Insert Into GTRHRIS.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);
                    
                    //----------------------
                    //add new Section
                    if (optType.Tag.ToString() == "L")
                    {
                        Int32 NewId = 0, NewIdDpt = 0;
                        sqlQuery = "Select Isnull(Max(SectId ),0)+1 As NewId from tblCat_Section  ";
                        NewId = clsCon.GTRCountingData(sqlQuery);


                        sqlQuery = "Select Isnull(Max(DeptID),0)+1 from tblCat_Department   ";
                        NewIdDpt = clsCon.GTRCountingData(sqlQuery);

                        sqlQuery =
                            "Insert Into tblCat_Section( ComId, SectId, SectName, SectNameB, DeptID, aId , SLNo, PCName, LUserId) values('" +
                            Common.Classes.clsMain.intComId + "','" + NewId + "','" + txtName.Text.Trim().ToString() +
                            "','" +
                            txtNameB.Text.Trim().ToString() + "','" + NewIdDpt + "','" + NewId + "','" + NewId + "','" +
                            Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "')";
                        arQuery.Add(sqlQuery);


                    // Insert Information To Log File
                    sqlQuery = "Insert Into GTRHRIS.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);
                    }
                    //-----------------------
                    
                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }
                prcClearData();
                prcLoadList();
                prcLoadCombo();
                optType.Focus();
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

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboUnder_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void optType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboUnder_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            cboUnder.DisplayLayout.Bands[0].Columns[0].Hidden = true;

            cboUnder.DisplayLayout.Bands[0].Columns[2].Width = 150;
            cboUnder.DisplayLayout.Bands[0].Columns[1].Width = 350;
            cboUnder.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Code";
            cboUnder.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Name";
        }

        private void txtName_MouseClick(object sender, MouseEventArgs e)
        {
            clsMain.GTRGotFocus(ref txtName);
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            clsMain.GTRGotFocus(ref txtName);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void cboUnder_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            clsProc.GTRUpperCase(txtName.Text.ToString());
        }

        // create tree 
        protected void prcGenerateTreeView(TreeNodeCollection parentNode, int parentID, DataTable mytab)
        {
            foreach (DataRow dta in mytab.Rows)
            {
                if (Convert.ToInt32(dta["ParentId"]) == parentID)
                {
                    String key = dta["DeptId"].ToString();
                    String text = dta["DeptName"].ToString();
                    TreeNodeCollection newParentNode = parentNode.Add(key, text).Nodes;

                    prcGenerateTreeView(newParentNode, Convert.ToInt32(dta["DeptId"]), mytab);
                }
            }
        }

        private void txtNameShort_MouseClick(object sender, MouseEventArgs e)
        {
            clsMain.GTRGotFocus(ref txtNameB);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNameShort_Leave(object sender, EventArgs e)
        {
            clsProc.GTRUpperCase(txtNameB.Text.ToString());
        }

        private void txtNameShort_Enter(object sender, EventArgs e)
        {
            clsMain.GTRGotFocus(ref txtNameB);
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void Tree_DoubleClick(object sender, EventArgs e)
        {
            //Tree.Nodes[Tree.SelectedNode.Index].Text
            string str = Tree.SelectedNode.Name.ToString();
            prcClearData();
            prcDisplayDetails(str);
            
        }

        private void prcDisplayDetails(string param)
        {
            try
            {
                string sqlQuery = "Exec prcGetDepartment " + Common.Classes.clsMain.intComId + ", "+ Int32.Parse(param);
                dsDetails = new System.Data.DataSet();

                clsConnection clsCon = new clsConnection();
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    
                    dr = dsDetails.Tables["Details"].Rows[0];

                    this.txtCode.Value = dr["DeptCode"].ToString();
                    this.txtCode.Tag = dr["DeptId"].ToString();
                    this.txtName.Text = dr["DeptName"].ToString();
                    this.cboUnder.Value = dr["ParentId"].ToString();

                    if (dr["DeptType"].ToString() == "L")
                    {
                        optType.CheckedIndex = 1;
                        txtNameB.Visible = true;
                        this.txtNameB.Text = dr["SectNameB"].ToString();
                        secId_update = Int32.Parse(dr["SectId"].ToString()) ;
                    }

                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
                }
                optType.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
