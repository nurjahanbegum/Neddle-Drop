using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using GTRLibrary;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmCadreInput : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmCadreInput(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmCadreInput_FormClosing(object sender, FormClosingEventArgs e)
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
                string SqlQuery = "Exec prcGetCadreUnit  " + Common.Classes.clsMain.intComId + ",'" + this.Tag.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);

                dsList.Tables[0].TableName = "Varible";
                dsList.Tables[1].TableName = "MPosition";

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
            cboMPosition.DataSource = null;
            cboMPosition.DataSource = dsList.Tables["MPosition"];
        }

        private void cboMPosition_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboMPosition.DisplayLayout.Bands[0].Columns["MPosition"].Width = cboMPosition.Width;
            cboMPosition.DisplayLayout.Bands[0].Columns["MPosition"].Header.Caption = "Main Position";
            cboMPosition.DisplayLayout.Bands[0].Columns["MCDID"].Hidden = true;
            cboMPosition.DisplayMember = "MPosition";
            cboMPosition.ValueMember = "MCDID";
        }

        public void prcDisplayDetails(string strParam)
        {
        }

         public void prcClearData()
         {
             btnSave.Enabled = false;
             btnDelete.Enabled = false;

             cboMPosition.Text = "";
             txtPosition.Text = "";
             txtCadre.Text = "";


         }

         private void frmCadreInput_Load(object sender, EventArgs e)
         {
             try
             {
                 lblCaption.Text = this.Tag + " Input ...";
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
             //Hide Column
             gridList.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
             gridList.DisplayLayout.Bands[0].Columns["CDId"].Hidden = true;
             gridList.DisplayLayout.Bands[0].Columns["MCDId"].Hidden = true;
             gridList.DisplayLayout.Bands[0].Columns["Flag"].Hidden = true;

             //Set Caption
             gridList.DisplayLayout.Bands[0].Columns["MPosition"].Header.Caption = "Header";
             gridList.DisplayLayout.Bands[0].Columns["Position"].Header.Caption = "Position";
             gridList.DisplayLayout.Bands[0].Columns["IdealSet"].Header.Caption = "Ideal Set";
             gridList.DisplayLayout.Bands[0].Columns["Ideal"].Header.Caption = "Ideal Cadre";
             gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

             //Set Width
             gridList.DisplayLayout.Bands[0].Columns["MPosition"].Width = 170;
             gridList.DisplayLayout.Bands[0].Columns["Position"].Width = 360;
             gridList.DisplayLayout.Bands[0].Columns["IdealSet"].Width = 180;
             gridList.DisplayLayout.Bands[0].Columns["Ideal"].Width = 90;
             gridList.DisplayLayout.Bands[0].Columns["Remarks"].Width = 120;

             //gridList.DisplayLayout.Bands[0].Columns["isInactive"].Style =
             //    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
             //gridList.DisplayLayout.Bands[0].Columns["aId"].Style =
             //   Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerWithSpin;

             //Stop Cell Modify
             gridList.DisplayLayout.Bands[0].Columns["MPosition"].CellActivation = Activation.NoEdit;
             //gridList.DisplayLayout.Bands[0].Columns["Position"].CellActivation = Activation.NoEdit;
             //gridList.DisplayLayout.Bands[0].Columns["IdealSet"].CellActivation = Activation.NoEdit;

             //Change alternate color
             gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
             gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

         private void btnAddNew_Click(object sender, EventArgs e)
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
                     ////Update     
                     //sqlQuery = " Update tblCat_CadreUnit  Set SectName ='" + txtName.Text.ToString() + "', SectNameB='" + txtNameB.Text.ToString() + "' , SLNO= '" + txtSlNo.Text.ToString() + "',DeptId = '" + cboDept.Value.ToString() + "',DeptName ='" + cboDept.Text.ToString() + "' ";
                     //sqlQuery += " Where SectId = " + Int32.Parse(txtId.Text);
                     //arQuery.Add(sqlQuery);

                     //clsCon.GTRSaveDataWithSQLCommand(arQuery);

                     //MessageBox.Show("Data Updated Succefully");
                 }
                 else
                 {
                     sqlQuery = "Select Isnull(Max(CDId),0)+1 As NewId from tblCat_Cadre ";
                     NewId = clsCon.GTRCountingData(sqlQuery);
                     //Insert to Table
                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '2','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '3','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '4','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '5','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '6','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '7','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '8','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_CadreUnit(CDId,Position,IdealSet,Ideal,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','0','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '9','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     arQuery.Add(sqlQuery);

                     sqlQuery = "Insert Into tblCat_Cadre(CDId,Position,IdealSet,MCDID,MPosition,ComId,PCName,LUserId) ";
                     sqlQuery = sqlQuery + " Values (" + NewId + ",'" + txtPosition.Text.ToString() + "','" + txtCadre.Text.ToString() + "','" + cboMPosition.Value.ToString() + "','" + cboMPosition.Text.ToString() + "', '" + Common.Classes.clsMain.intComId + "','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                     int add = arQuery.Add(sqlQuery);

                     // Insert Information To Log File
                     sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                         + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                     arQuery.Add(sqlQuery);

                     clsCon.GTRSaveDataWithSQLCommand(arQuery);

                     MessageBox.Show("Data Saved Succefully");
                 }
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
                 arQuery = null;
                 clsCon = null;
             }
         }


         private void btnCancel_Click(object sender, EventArgs e)
         {
             prcClearData();
             
             //MessageBox.Show(dsList.Tables["Varible"].Rows.Count.ToString());
             for (int rowCount = dsList.Tables["Varible"].Rows.Count - 1; rowCount >= 0 ; rowCount --)
             {
                 if (dsList.Tables["Varible"].Rows[rowCount][0].ToString().Trim().Length == 0)
                 {
                     dsList.Tables["Varible"].Rows[rowCount].Delete();
                 }
             }

         }

        public Boolean fncBlank()//(string tbl, int Rowno )
         {

             if (this.cboMPosition.Text.Length == 0)
             {
                 MessageBox.Show("Please provide Main Position Name.");
                 cboMPosition.Focus();
                 return true;
             }

             if (this.cboMPosition.IsItemInList() == false)
             {
                 MessageBox.Show("Please provide valid Main Position [or, select from list item].");
                 cboMPosition.Focus();
                 return true;
             }

             if (this.txtPosition.Text.Length == 0)
             {
                 MessageBox.Show("Please Provide Position Name.");
                 txtPosition.Focus();
                 return true;
             }

             if (this.txtCadre.Text.Length == 0)
             {
                 MessageBox.Show("Please Provide Ideal Cadre.");
                 txtCadre.Focus();
                 return true;
             }

             return false;
            
         }

        public Boolean fncBlankGrid(string tbl, int Rowno )
        {


            if (dsList.Tables[tbl].Rows[Rowno][4].ToString().Length == 0)
            {
                MessageBox.Show("Provide Position.");
                return true;
            }

            if (dsList.Tables[tbl].Rows[Rowno][5].ToString().Length == 0)
            {
                MessageBox.Show("Provide Ideal Set.");
                return true;
            }

            if (dsList.Tables[tbl].Rows[Rowno][6].ToString().Length == 0)
            {
                dsList.Tables[tbl].Rows[Rowno][6] = 0;
            }

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
                    if (fncBlankGrid("Varible", rowCount))
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
                        sqlQuery = "Update tblCat_CadreUnit Set IdealSet ='" + dsList.Tables["Varible"].Rows[rowCount][5]
                                   + "' where CDId = '" + dsList.Tables["Varible"].Rows[rowCount][3] + "'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblCat_Cadre Set IdealSet ='" + dsList.Tables["Varible"].Rows[rowCount][5]
                                    + "' where CDId = '" + dsList.Tables["Varible"].Rows[rowCount][3] + "'";
                        arQuery.Add(sqlQuery);

                        sqlQuery = "Update tblCat_CadreUnit Set IdealSet ='" + dsList.Tables["Varible"].Rows[rowCount][5]
                                   + "', Ideal = '" + dsList.Tables["Varible"].Rows[rowCount][6]
                                   + "', Remarks = '" + dsList.Tables["Varible"].Rows[rowCount][7]
                                   + "',PCName = '" + Common.Classes.clsMain.strComputerName
                                   + "',LUserId = " + Common.Classes.clsMain.intUserId
                                   + "   where aId = '" + dsList.Tables["Varible"].Rows[rowCount][0] + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                    }

                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Saved Or Update Succefully");

                prcClearData();
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

        private void gridList_AfterCellUpdate(object sender, CellEventArgs e)
         {
             gridList.ActiveRow.Cells[8].Value = 1;
             btnSave.Enabled = true;
         }

         private void gridList_AfterCellActivate(object sender, EventArgs e)
         {
             btnDelete.Enabled = true;
         }

         private void btnDelete_Click(object sender, EventArgs e)
         {
             if (MessageBox.Show("Do you want to delete "+this.Tag+" information of [" + gridList.ActiveRow.Cells[1].Text.ToString() + "]", "",
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
                 sqlQuery = "Delete From tblCat_Variable where VarId = " + Int32.Parse(gridList.ActiveRow.Cells[0].Value.ToString());
                 arQuery.Add(sqlQuery);

                 // Insert Information To Log File
                 sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                            + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                            sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                 arQuery.Add(sqlQuery);
                 clsCon.GTRSaveDataWithSQLCommand(arQuery);

                 MessageBox.Show("Data Deleted Successfully.");

                 prcClearData();
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


    }
}
