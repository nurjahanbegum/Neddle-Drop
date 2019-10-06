using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;

namespace GTRHRIS.Campus.FormEntry
{
    public partial class frmExamMarkEntry : Form
    {
        string strValue = "";

        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        string Data = "";

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmExamMarkEntry(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }
        private void fncGridData(ref ArrayList arQuery, String newID, String Saleid)
        {
            //Common.Classes.clsConnection clsCon = new Common.Classes.clsConnection("CustBill");
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
             if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;

            try
            {
                //Member Master Table
               
                if (btnSave.Text.ToString() != "&Save")
                    try
                    {
                        foreach (UltraGridRow row in this.gridList.Rows)
                        {
                            if (row.Cells["isChecked"].Value.ToString() == "1")
                            {
                                sqlQuery = " Delete  tblExamMark_Info where empid = '" + row.Cells["empid"].Text.ToString() + "' and sesn =  '" + cboSesn.Text.ToString() + "' and classId = '"
                                +cboClass.Value.ToString()+"' and ExamId = '"+cboType.Value.ToString()+"' ";
                                arQuery.Add(sqlQuery);
                                sqlQuery = "insert into tblExamMark_Info (ComId, empId,EmpCode, EmpName,sesn,ClassId,ExamId, SubId, SubTtlMark, GetMark, dtInput,PCName)"
                                                        + "values (" + clsMain.intComId + ", '" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["EmpCode"].Value.ToString() + "', '" +
                                                        row.Cells["EmpName"].Value.ToString() + "', '" + cboSesn.Text.ToString() + "','" +
                                                        cboClass.Value.ToString() + "', '" + cboType.Value.ToString() + "', '" + cboSub.Value.ToString() + "','" + row.Cells["marks"].Value.ToString() + "','" +
                                                        row.Cells["GetMark"].Value.ToString() + "','" + clsProc.GTRDate(dtInput.Value.ToString()) + "','" + clsMain.strComputerName + "')";
                                arQuery.Add(sqlQuery);
                            }
                        }
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);
                        MessageBox.Show("Data Update Successfully");

                        prcLoadList();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        clsCon = null;
                    }
                else
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0)
                        {
                            sqlQuery = "insert into tblExamMark_Info (ComId, empId,EmpCode, EmpName,sesn,ClassId,ExamId, SubId, SubTtlMark, GetMark, dtInput,PCName)"
                                                    + "values (" + clsMain.intComId + ", '" + row.Cells["empid"].Text.ToString() + "', '" + row.Cells["EmpCode"].Value.ToString() + "', '"+
                                                    row.Cells["EmpName"].Value.ToString() + "', '" + cboSesn.Text.ToString() + "','" +
                                                    cboClass.Value.ToString() + "', '" + cboType.Value.ToString() + "', '" + cboSub.Value.ToString() + "','" + row.Cells["marks"].Value.ToString() + "','" +
                                                    row.Cells["GetMark"].Value.ToString() + "','" + clsProc.GTRDate(dtInput.Value.ToString()) + "','" + clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);
                        }
                    }
                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully.");
                }
                prcClearData();
                cboSesn.Focus();

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

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Hide column
                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;  //Country Name
                
                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 120;
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 200;
                gridList.DisplayLayout.Bands[0].Columns["marks"].Width = 120; 
                gridList.DisplayLayout.Bands[0].Columns["isChecked"].Width = 65;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["Getmark"].Width = 100;  //

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";

                gridList.DisplayLayout.Bands[0].Columns["marks"].Header.Caption = "Subject Marks";
                gridList.DisplayLayout.Bands[0].Columns["Getmark"].Header.Caption = "Get Marks";
                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["marks"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
               // this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

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

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetMarkEntry] " + Common.Classes.clsMain.intComId +", "+cboSesn.Value.ToString()+", "+cboClass.Value.ToString()+", "+cboSub.Value.ToString()+", 0 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblSesn";
                dsList.Tables[2].TableName = "tblClass";
                dsList.Tables[3].TableName = "tblSub";
                dsList.Tables[4].TableName = "tblExamtype";
                
                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];

                cboType.Enabled = false;
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
            cboSesn.DataSource = null;
            cboSesn.DataSource = dsList.Tables["tblSesn"];

            cboClass.DataSource = null;
            cboClass.DataSource = dsList.Tables["tblClass"];

            cboSub.DataSource = null;
            cboSub.DataSource = dsList.Tables["tblSub"];

            cboType.DataSource = null;
            cboType.DataSource = dsList.Tables["tblExamtype"];
        }

        private void frmExamMarkEntry_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();
                groupData.Enabled = false;
                btnFillData.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmExamMarkEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            uTab = null;
            clsProc = null;
        }

        private void prcClearData()
        {
            this.gridList.DataSource = null;
            this.cboSesn.Value = 0;
            this.dtInput.Value = null;

            this.cboSub.Value = 0;
            this.cboClass.Value = 0;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.cboSesn.Focus();

            dtInput.Value = DateTime.Now.ToString();
        }
        private Boolean fncBlank()
        {
           if (this.cboType.Text.Length == 0)
            {
                MessageBox.Show("Please provide Exam Type.");
                cboType.Focus();
                return true;
           }
          if (this.cboSesn.Text.Length == 0)
          {
             MessageBox.Show("Please provide Session.");
             cboSesn.Focus();
             return true;
            }
           if (this.cboSub.Text.Length == 0)
            {
               MessageBox.Show("Please provide Subject");
               cboSub.Focus();
               return true;
            }
            if (this.cboClass.Text.Length == 0)
             {
                MessageBox.Show("Please provide Class");
                cboClass.Focus();
                return true;
             }
        return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete Leave Which Are shown in the Grid" , "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery=new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            try
            {
                string sqlQuery = "";
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["isChecked"].Value.ToString() == "1")
                    {
                        //RowID = row.Index + 1;
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblAttfixed As A

                        sqlQuery = " Delete  tblExamMark_Info where empid = '" + row.Cells["empid"].Text.ToString() + "' ";
                        arQuery.Add(sqlQuery);

                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                cboSesn.Focus();
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

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetFixAtt] " + Int32.Parse(strParam)+","+Common.Classes.clsMain.intComId ;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblReleased";

                DataRow dr;
                if (dsDetails.Tables["tblReleased"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblReleased"].Rows[0];

                    this.txtId.Text = dr["relid"].ToString();
                    this.cboSesn.Value = dr["empid"].ToString();
                    this.dtInput.Text = dr["reldate"].ToString();
                    
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    prcClearData();
            //    prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboCountryName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboCountryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboEmpID_RowSelected(object sender, RowSelectedEventArgs e)
        {
            try
            {
                if (cboSesn.Value != null)
                {
                    //
                    //txtName.Text = cboSesn.ActiveRow.Cells["empName"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
        }

        private void dtJoinDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
        private void cboAddList_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}
            
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetMarkEntry] " + Common.Classes.clsMain.intComId + ",'" + cboSesn.Value.ToString() + "','" + cboClass.Value.ToString() + "','" + cboSub.Value + "',1 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];
                cboType.Enabled = true;

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No Data Found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btnSave.Text = "&Update";
            btnDelete.Enabled = true;
        }

        private void btnFillData_Click(object sender, EventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
            {
                if (row.Cells["isChecked"].Value.ToString() == "1")
                {
                    row.Cells["Getmark"].Value = txtMark.Value.ToString();
                }
            } 
        }

        private void cboEmpID_ValueChanged(object sender, EventArgs e)
        {
         
            if(cboSesn.Value == null)
                return;
            
            strValue = cboSesn.Value.ToString();
        }

        private void cboSection_ValueChanged(object sender, EventArgs e)
        {
            if (cboSub.Value == null)
                return;
            
            strValue = cboSub.Value.ToString();
        }

        private void cboShiftTime_ValueChanged(object sender, EventArgs e)
        {
            if (cboClass.Value == null)
                return;
            
            strValue = cboClass.Value.ToString();
        }

      
        private void ultraButton1_Click(object sender, EventArgs e)
        {

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection("GTRHRIS");
            //dsDetails = new System.Data.DataSet();

            try
            {
                //string sqlQuery = "Exec [prcGetFixAtt] 1," + Common.Classes.clsMain.intComId + ",'" + optCriteria.Value + "','" + strValue + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "','" + clsProc.GTRDate(dtInputDate.Value.ToString()) + "' ";
                //clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblFixData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblFixData"];

                if (dsDetails.Tables["tblFixData"].Rows.Count > 0)
                {
                    btnFillData.Enabled = true;
                    groupData.Enabled = true;

                    //cboStatus1.Text = "P";
                    //dtTimeIn.Text = "1-1-1900 08:00";
                    //dtTimeOut.Text = "1-1-1900 08:00";
                    //dtOt.Text = "1-1-1900 08:00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    row.Cells["isChecked"].Value = 1;
                }
            }
            else 
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    row.Cells["isChecked"].Value = 0;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupData.Enabled = true;
                btnFillData.Enabled = true;
            }
            else 
            {
                groupData.Enabled = false;
                btnFillData.Enabled = false;
            }
        }
        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboSesn_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSesn.DisplayLayout.Bands[0].Columns["sesn"].Width = cboSesn.Width;
            cboSesn.DisplayLayout.Bands[0].Columns["sesn"].Header.Caption = "Session";
        }

        private void cboClass_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboClass.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboClass.DisplayLayout.Bands[0].Columns["SectName"].Width = cboClass.Width;
            cboClass.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Class";

            cboClass.DisplayMember = "SectName";
            cboClass.ValueMember = "SectId";
        }

        private void cboSub_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSub.DisplayLayout.Bands[0].Columns["subjectId"].Hidden = true;
            cboSub.DisplayLayout.Bands[0].Columns["subjectName"].Width = cboSub.Width;
            cboSub.DisplayLayout.Bands[0].Columns["subjectName"].Header.Caption = "Class";

            cboSub.DisplayMember = "subjectName";
            cboSub.ValueMember = "subjectId";
        }

        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["ExamId"].Hidden = true;
            cboType.DisplayLayout.Bands[0].Columns["ExamType"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["ExamType"].Header.Caption = "Class";

            cboType.DisplayMember = "ExamType";
            cboType.ValueMember = "ExamId";
        }
    }
}
