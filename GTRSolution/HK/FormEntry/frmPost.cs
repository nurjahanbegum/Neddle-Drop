using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;


namespace GTRHRIS.HK.FormEntry
{
    public partial class frmPost : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;


        public frmPost(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmPost_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtSName_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

 
        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetPost 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "Post";
                dsList.Tables[1].TableName = "Country";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Post"];
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
        //public void prcLoadCombo()
        //{

        //    try
        //    {
        //        cboCountry.DataSource = null;
        //        cboCountry .DataSource = dsList.Tables["Country"];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }

        //}
        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetPost " + Int32.Parse(strParam);
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "Post";
                DataRow dr;

                if (dsDetail.Tables["Post"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["Post"].Rows[0];
                    txtId.Text = dr["PostId"].ToString();
                    txtName.Text = dr["PostName"].ToString();
                    txtSName.Text = dr["PostNameShort"].ToString();
                  
                    btnSave.Text =" &Update";
                    btnDelete.Enabled = true;
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
            txtId.Text = "";
            txtName.Text = "";
            txtSName.Text = "";

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
        }

        private void frmPost_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridList.DisplayLayout.Bands[0].Columns["PostId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["PostName"].Header.Caption = "Post Name";
            gridList.DisplayLayout.Bands[0].Columns["PostNameShort"].Header.Caption = "Short Name";
            
            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["PostName"].Width = 300;
            gridList.DisplayLayout.Bands[0].Columns["PostNameShort"].Width = 150;
            
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

       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void cboCountry_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{

            
        //    //set Caption
        //    cboCountry.DisplayLayout.Bands[0].Columns["countryName"].Header.Caption = "Country";
            
        //    //set Width
        //    cboCountry.DisplayLayout.Bands[0].Columns["countryName"].Width  = cboCountry.Width;
            
        //    //initialize members
        //    cboCountry.DisplayMember = "countryName";
        //    cboCountry.ValueMember = "countryId";

        //}

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        public Boolean fncBlank()
        {
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide Name.");
                txtName.Focus();
                return true;
            }
           
            //if(this.cboCountry.Text.Trim().Length==0)
            //{
            //    MessageBox.Show("Please Provide Country Name.");
            //    cboCountry.Focus();
            //    return true;
            //}
            //else if (this.cboCountry.IsItemInList(cboCountry.Text.ToString().ToUpper())==false)
            //{
            //    MessageBox.Show("Please provide valid data [or Select Country Name From List ].");
            //    cboCountry.Focus();
            //    return true;
            //}

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
            Int32 NewId = 0;

            try
            {
                if (btnSave.Text.ToString() != "&Save")
                {
                    //Update     
                    sqlQuery = " Update tblCat_Post   Set PostName  ='" + txtName.Text.ToString() + "',  PostNameShort='" + txtSName.Text.ToString() + "'";
                    sqlQuery += " Where PostId = " + Int32.Parse(txtId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    sqlQuery = "Select Isnull(Max(PostId ),0)+1 As NewId from tblCat_Post  ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    sqlQuery = "Insert Into tblCat_Post( PostId,aId,PostName ,PostNameShort, PCName, LUserId ) ";
                    sqlQuery = sqlQuery + " Values (" + NewId + ", " + NewId + ", '" + txtName.Text.ToString() + "','" + txtSName.Text.ToString() + "','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "')";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }
                prcClearData();
                txtName.Focus();

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Post information of [" + txtName.Text + "]", "",
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
                sqlQuery = "Delete from tblCat_Post Where PostId   = " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                txtName.Focus();

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

        private void cboCountry_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        
    }
}
