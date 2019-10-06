using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Office.Interop.Word;
using DataTable = System.Data.DataTable;

namespace GTRHRIS.Campus
{
    public partial class frmSubTemp : Form
    {
        private DataSet dsList;
        private DataSet dsDetails;
        private DataView dvReference = new DataView(); //Grid Reference
        private DataView dvReference2 = new DataView(); //Combo Reference
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmSubTemp(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmSubTemp_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            dvReference = null;
            dvReference2 = null;

            FM = null;
            clsProc = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prcLoadList()
        {
            dsList = new DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec [prcGetSubTemp]" + clsMain.intComId;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tbl_Temp";
                dsList.Tables[1].TableName = "subject";
                dsList.Tables[2].TableName = "class";
                dsList.Tables[3].TableName = "gridList";
                dsList.Tables[4].TableName = "session";
                dsList.Tables[5].TableName = "Exam";

                gridTemp.DataSource = null;
                gridTemp.DataSource = dsList.Tables["tbl_Temp"];

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["gridList"];

                prcInitializeVoucherGrid();
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
            uddFeeTp.DataSource = null;
            uddFeeTp.DataSource = dsList.Tables["subject"];
            uddFeeTp.DisplayLayout.Bands[0].Columns["subjectId"].ValueList = uddFeeTp;

            //Class
            cboClass.DataSource = null;
            cboClass.DataSource = dsList.Tables["class"];

            cbosesn.DataSource = null;
            cbosesn.DataSource = dsList.Tables["session"];

            cboExam.DataSource = null;
            cboExam.DataSource = dsList.Tables["Exam"];
        }

        private void gridTemp_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            gridTemp.DisplayLayout.Bands[0].Columns["SubjectName"].Hidden = true; // SubjectId    
            gridTemp.DisplayLayout.Bands[0].Columns["SubjectName"].Width = 300; // SubjectName 
            gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].Width = 300; // SubjectName 
            gridTemp.DisplayLayout.Bands[0].Columns["subjectCode"].Width = 100; // SubjectName 
            gridTemp.DisplayLayout.Bands[0].Columns["marks"].Width = 100; // SubjectName 

            //Caption
            gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].Header.Caption = "Subject";
            gridTemp.DisplayLayout.Bands[0].Columns["SubjectName"].Header.Caption = "Subject";
            gridTemp.DisplayLayout.Bands[0].Columns["subjectCode"].Header.Caption = "Code";
            gridTemp.DisplayLayout.Bands[0].Columns["marks"].Header.Caption = "Marks";

            gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;

            //Change alternate color
            gridTemp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridTemp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Hiding +/- Indicator
            this.gridTemp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }
        private void uddFeeTp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            uddFeeTp.DisplayLayout.Bands[0].Columns["subjectId"].Hidden = true;
            uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectCode"].Hidden = true;
            uddFeeTp.DisplayLayout.Bands[0].Columns["marks"].Hidden = true;

            uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectName"].Width = gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].Width;
            //uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectCode"].Width = gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].Width;
            //uddFeeTp.DisplayLayout.Bands[0].Columns["marks"].Width = gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].Width;
            uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectName"].Header.Caption = "Subject";
            //uddFeeTp.DisplayLayout.Bands[0].Columns["SubjectCode"].Header.Caption = "Code";
            //uddFeeTp.DisplayLayout.Bands[0].Columns["marks"].Header.Caption = "Marks";

            uddFeeTp.ValueMember = "subjectId";
            uddFeeTp.DisplayMember = "SubjectName";
        }
        private void cboClass_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboClass.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;

            cboClass.DisplayLayout.Bands[0].Columns["SectName"].Width = cboClass.Width;
            cboClass.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Class Name";

            cboClass.DisplayMember = "SectName";
            cboClass.ValueMember = "SectId";
        }

        private void txtTempNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void dtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (cboClass.Enabled)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }
            else
            {
                gridTemp.Rows[0].Cells["subjectId"].Activate();
                gridTemp.PerformAction(UltraGridAction.EnterEditMode);
            }
        }
        private void cboClass_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void prcInitializeVoucherGrid()
        {
            DataTable dt = new DataTable("tbl_Temp");
            dt.Columns.Add("subjectId", typeof(String));
            dt.Columns.Add("SubjectName", typeof(String));
            dt.Columns.Add("SubjectCode", typeof(String));
            dt.Columns.Add("marks", typeof(String));

            for (int i = 0; i < 15; i++)
                prcAddRow(ref dt);

            gridTemp.DataSource = null;
            gridTemp.DataSource = dt;

            gridTemp.DisplayLayout.Bands[0].Columns["subjectId"].ValueList = uddFeeTp;
        }
        private void prcAddRow(ref DataTable dt)
        {
            dt.Rows.Add("","","");
        }
        private void gridTemp_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Int16) e.KeyCode == 117)
            {
                e.Handled = true;
                txtDescription.Focus();
            }
            if ((Int16) e.KeyCode == 13)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }
        private void prcClearData()
        {
            txtTempNo.Text = "Auto Generate";
            txtTempNo.Tag = "0";
            cboClass.Value = null;
            txtDescription.Text = "";
            txtSumDebit.Text = "0.00";
            cbosesn.Text = null;
            cboExam.Text = null;
            gridTemp.DataSource = null;
            prcInitializeVoucherGrid();
            prcLoadList();
            prcLoadCombo();

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;

            tab.Tabs["Gen"].Selected = true;
            txtTempNo.Focus();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete Fee Temp . Temp No : [" + txtTempNo.Text + "]", "",System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";
                //Delete Database
                sqlQuery = "Delete tblFee_Tmp  Where FeeMgtID = " + double.Parse(txtTempNo.Tag.ToString()) + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + clsMain.intUserId + ", '" + this.Name.ToString() + "','" +sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Delete Succefully");

                prcClearData();
                gridList.DataSource = null;
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
            Int32 intId = 0; //Value update with number of rows affected in sql server
            try
            {
                //Member Master Table
                if(btnSave.Text.ToString() != "&Save")
                {
                    //Update
                    sqlQuery = "Delete From [tblSubtmp_Data] Where TmpId = " + double.Parse(txtId.Text.ToString()) + "";
                    arQuery.Add(sqlQuery);
                    foreach (UltraGridRow row in gridTemp.Rows)
                    {
                        if (row.Cells["SubjectId"].Text.ToString().Length == 0)
                        {
                            break;
                        }

                        Int32 RowId = row.Index + 1;
                        sqlQuery = "Insert Into tblSubtmp_Data ( comId,tmpId,subId, subCode, marks,RowNo) "
                        + " Values (" + clsMain.intComId + ", " + txtId.Text.ToString() + ", " + row.Cells["SubjectId"].Value.ToString() + ", '"+
                        row.Cells["subjectCode"].Value.ToString() + "', " + row.Cells["marks"].Text.ToString() + ", " + RowId + ")";
                        arQuery.Add(sqlQuery);
                    }
                    //Insert Information To Log File
                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    //NewId
                    sqlQuery = "Select Isnull(Max(TmpId),0)+1 As NewId from tblsubTmp_Data";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    foreach (UltraGridRow row in gridTemp.Rows)
                    {
                        if (row.Cells["SubjectId"].Text == "")
                        {
                            break;
                        }
                        Int32 RowId = row.Index + 1;
                        sqlQuery = "Insert Into tblSubtmp_Data ( comId,tmpId,subId,subCode, marks, RowNo) "
                        + " Values (" + clsMain.intComId + ", " + NewId + ", " + row.Cells["SubjectId"].Value.ToString() + ", '" + row.Cells["subjectCode"].Value.ToString() + "','" +
                        row.Cells["marks"].Value.ToString() + "',  " + RowId + ")";
                        arQuery.Add(sqlQuery);

                    }
                    sqlQuery = "Insert Into tblSubTmp_info (comId, TmpId, dtCreate, ClassId, Description, sesn, ttlExam, LUserId ) "
                               + " Values (" + clsMain.intComId + ", " + NewId + ", '" +clsProc.GTRDate(dtCreate.Value.ToString())+"'," + cboClass.Value.ToString() + ",'"+
                               txtDescription.Text.ToString()+"', '"+cbosesn.Text.ToString()+"', '"+cboExam.Text.ToString()+"',  "+clsMain.intUserId+")";
                    arQuery.Add(sqlQuery);

                     //Insert Information To Log File
                    sqlQuery = "Insert Into GTRSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + clsMain.intUserId + ", '" + this.Name.ToString() + "','" +sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }

                prcClearData();
                txtTempNo.Focus();

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

        private Boolean fncBlank()
        {
            if (txtTempNo.Text.Length == 0)
            {
                MessageBox.Show("Please provide Fee Temp no.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tab.Tabs["Gen"].Selected = true;
                txtTempNo.Focus();
                return true;
            }
            if (dtCreate.Text.Length == 0)
            {
                MessageBox.Show("Please provide From date.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tab.Tabs["Gen"].Selected = true;
                dtCreate.Focus();
                return true;
            }
            return false;
        }
        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns["TmpId"].Hidden = true;

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["dtCreate"].Width = 200;  //Create Date
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 180;  //Class
                //gridList.DisplayLayout.Bands[0].Columns["Description"].Hidden = true;  //Description

                gridList.DisplayLayout.Bands[0].Columns["dtCreate"].Header.Caption = "Create Date";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Class";
                //gridList.DisplayLayout.Bands[0].Columns["Description"].Header.Caption = "Description";

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

                this.gridList.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcDisplayDetails(gridList.ActiveRow.Cells["tmpid"].Text.ToString());
            
            if (txtId.Text != null)
            {
                tab.Tabs["Gen"].Selected = true;

                gridTemp.Rows[0].Cells["subjectid"].Activate();
                gridTemp.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                prcDisplayDetails(gridList.ActiveRow.Cells["tmpId"].Value.ToString());
            }
            return;
        }

        private void prcDisplayDetails(String strTmpId)
        {
            dsDetails = new DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "Exec [prcGetSubTemp] " + clsMain.intComId + ", " + Int32.Parse(strTmpId);
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";
                dsDetails.Tables[1].TableName = "GridData";

                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    //Info Data
                    DataRow dr = dsDetails.Tables["Details"].Rows[0];
                    dtCreate.Value = dr["dtCreate"].ToString();
                    cboClass.Value = dr["ClassId"].ToString();
                    cbosesn.Text = dr["sesn"].ToString();
                    cboExam.Text = dr["ttlExam"].ToString();
                    txtId.Value = dr["TmpId"].ToString();
                    DataRow[] dr2 = dsDetails.Tables["GridData"].Select();
                    foreach (DataRow dr3 in dr2)
                    {
                        if (double.Parse(dr3["RowNo"].ToString()) != 0)
                        {
                            Int32 rowNo = Int32.Parse(dr3["RowNo"].ToString()) - 1;

                            UltraGridRow row = gridTemp.Rows[rowNo];
                            row.Cells["subjectId"].Value = dr3["subId"].ToString();
                            row.Cells["subjectCode"].Value = dr3["subCode"].ToString();
                            row.Cells["marks"].Value = dr3["marks"].ToString();
                        }
                    }
                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;

                    prcLoadCombo();
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

        private void cboClass_Validating(object sender, CancelEventArgs e)
        {
            if (cboClass.Text.Length > 0)
            {
                if (cboClass.IsItemInList() == false)
                {
                    MessageBox.Show("Please provide valid Class [or select from list].");
                    cboClass.Value = null;
                    cboClass.Focus();
                }
            }
        }

        private void frmSubTemp_Load(object sender, EventArgs e)
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

        private void uddFeeTp_RowSelected(object sender, RowSelectedEventArgs e)
        {
            if (uddFeeTp.ActiveRow == null)
                return;

            gridTemp.ActiveRow.Cells["SubjectCode"].Value = uddFeeTp.ActiveRow.Cells["SubjectCode"].Text.ToString();
            gridTemp.ActiveRow.Cells["marks"].Value = uddFeeTp.ActiveRow.Cells["marks"].Text.ToString();
        }

        private void cbosesn_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cbosesn.DisplayLayout.Bands[0].Columns["sesn"].Width = cbosesn.Width;
            cbosesn.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
        }

        private void cboExam_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboExam.DisplayLayout.Bands[0].Columns["Exam"].Width = cboExam.Width;
            cboExam.DisplayLayout.Bands[0].Columns["Exam"].Header.Caption = "Exam";
        }
    }
}
