using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.HR.FormEntry
{
    public partial class frmHayInfo : Form
    {
        string strTranWith = "";
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
         clsProcedure clsProc = new  clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmHayInfo(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmHayInfo_Load(object sender, EventArgs e)
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

        private void frmHayInfo_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void frmHayInfo_FormClosing(object sender, FormClosingEventArgs e)
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
             clsConnection clsCon = new  clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetHaySystem " + Common.Classes.clsMain.intComId + ", 0";
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

            cboUnder.ValueMember = "HId";
            cboUnder.DisplayMember = "HName";
        }

        private void prcClearData()
        {
            txtCode.Text="";
            txtName.Text = "";
            txtNameShort.Text = "";
            cboUnder.Value = null;
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
            if (MessageBox.Show("Do you want to delete Group/End Group information of [" + txtName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
             clsConnection clsCon = new  clsConnection();
            try
            {
                //sqlQuery = "Exec prcTran_HaySystem 'DELETE', " + Common.Classes.clsMain.intComId + ", " + Int32.Parse(txtCode.Tag.ToString()) + ",'" + txtCode.Text.ToString() + "', '', '', '"+optType.Tag.ToString()+"', " + cboUnder.Value + "";
                string sqlQuery = "";
                sqlQuery = "Exec prcTran_HaySystem 'DELETE', " + Common.Classes.clsMain.intComId + ", " + Int32.Parse(txtCode.Tag.ToString()) + ",'" + txtCode.Text.ToString() + "', '','"+optType.Tag.ToString()+"', " + cboUnder.Value + "";
                arQuery.Add(sqlQuery);
                    // Insert Information To Log File
                    sqlQuery= "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);
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
            }
        }

        private void optType_ValueChanged(object sender, EventArgs e)
        {
            optType.Tag = optType.Value.ToString();
        }

        private Boolean fncBlank()
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide warehouse name.");
                txtName.Focus();
                return true;                    
            }
            if (txtNameShort.Text.Length == 0)
            {
                MessageBox.Show("Please provide warehouse name [short].");
                txtNameShort.Focus();
                return true;
            }
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
             clsConnection clsCon = new  clsConnection();

            string sqlQuery = "";
            try
            {
                //Member Master Table
                if (txtCode.Text.Length != 0)
                {
                    //Update
                    sqlQuery = "Exec prcTran_HaySystem 'Update', " + Common.Classes.clsMain.intComId + ", " + Int32.Parse(txtCode.Tag.ToString()) + ", '" + txtName.Text + "', '" + txtNameShort.Text + "', '" + optType.Tag.ToString() + "', " + cboUnder.Value + "";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    //add new
                    sqlQuery = "Exec prcTran_HaySystem 'NEW', " + Common.Classes.clsMain.intComId + ", 0, '" + txtName.Text + "', '" + txtNameShort.Text + "', '" + optType.Tag.ToString() + "', " + cboUnder.Value + "";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

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
            
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            
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
                    String key = dta["HID"].ToString();
                    String text = dta["HName"].ToString();
                    TreeNodeCollection newParentNode = parentNode.Add(key, text).Nodes;

                    prcGenerateTreeView(newParentNode, Convert.ToInt32(dta["HID"]), mytab);
                }
            }
        }

        private void txtNameShort_MouseClick(object sender, MouseEventArgs e)
        {
            //clsProc.GTRGotFocus(ref txtNameShort);
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
            clsProc.GTRUpperCase(txtNameShort.Text.ToString());
        }

        private void txtNameShort_Enter(object sender, EventArgs e)
        {
            //clsProc.GTRGotFocus(ref txtNameShort);
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
                string sqlQuery = "Exec prcGetHaySystem " + Common.Classes.clsMain.intComId + ", "+ Int32.Parse(param);
                dsDetails = new System.Data.DataSet();

                 clsConnection clsCon = new  clsConnection();
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    
                    dr = dsDetails.Tables["Details"].Rows[0];

                    this.txtCode.Value = dr["HCode"].ToString();
                    this.txtCode.Tag = dr["HId"].ToString();
                    this.txtName.Text = dr["HName"].ToString();
                    this.txtNameShort.Text = dr["HNameShort"].ToString();
                    this.cboUnder.Value = dr["ParentId"].ToString();

                    if (dr["HType"].ToString() == "L")
                    {
                        optType.CheckedIndex = 1;
                    }

                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
