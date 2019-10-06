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
    public partial class frmSubject : Form
    {
        DataSet dsList;
        DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmSubject(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
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
                    //Update     
                    sqlQuery = " Update tbl_subject  Set SubjectCode ='" + txtCode.Text.ToString() + "', SubjectName='" + txtName.Text.ToString() + "' , ShortName= '" + txtShrtName.Text.ToString() + "', DepartmentId='" +
                        cboDepartment.Value.ToString() + "', marks = '"+txtMark.Text.ToString()+"', credit = '"+txtCredit.Text.ToString()+"', cr_Cost = '"+txtCost.Text.ToString()+"', houre = '"+
                        dtHour.Value.ToString()+"' Where SubjectId = " + Int32.Parse(txtId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(subjectId),0)+1 As NewId from tbl_Subject";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    //Insert to Table
                    sqlQuery = "Insert Into tbl_Subject(SubjectId, SubjectCode, SubjectName, ShortName,DepartmentId,ComId, UserId,marks,credit,cr_Cost,houre) ";
                    sqlQuery = sqlQuery + " Values (" + NewId+ ",'"+txtCode.Text.ToString()+"', '"+txtName.Text.ToString()+"','"+txtShrtName.Text.ToString()+"','"+
                    cboDepartment.Value.ToString()+"','" + Common.Classes.clsMain.intComId + "','" + Common.Classes.clsMain.intUserId + "','"+
                    txtMark.Text.ToString()+"', '"+txtCredit.Text.ToString()+"', '"+txtCost.Text.ToString()+"','"+dtHour.Value.ToString()+"')" ;
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into gtrSystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }
                prcClearData();
                txtCode.Focus();

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
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns["SubjectId"].Hidden = true;  //area Code
                gridList.DisplayLayout.Bands[0].Columns["Subjectcode"].Width = 120;  //Operator Name
                gridList.DisplayLayout.Bands[0].Columns["SubjectName"].Width = 180;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["ShortName"].Width = 120;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["marks"].Width = 120;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["credit"].Width = 100;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["Cr_Cost"].Width = 100;  //Short Name
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Hidden = true;  //Country Name

                gridList.DisplayLayout.Bands[0].Columns["Subjectcode"].Header.Caption = "Code";
                gridList.DisplayLayout.Bands[0].Columns["SubjectName"].Header.Caption = "Subject Name";
                gridList.DisplayLayout.Bands[0].Columns["shortname"].Header.Caption = "Short Name";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Dept. Name";
                gridList.DisplayLayout.Bands[0].Columns["marks"].Header.Caption = "Marks";
                gridList.DisplayLayout.Bands[0].Columns["credit"].Header.Caption = "Credit";
                gridList.DisplayLayout.Bands[0].Columns["Cr_Cost"].Header.Caption = "Credit Cost";

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

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetSubject] "+ clsMain.intComId +",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                
                dsList.Tables[0].TableName = "List";
                dsList.Tables[1].TableName = "tblDept";



                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["List"];

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
            cboDepartment.DataSource = null;
            cboDepartment.DataSource = dsList.Tables["tblDept"];

            //cboDepartment.DisplayMember = "countryName";
            //cboDepartment.ValueMember = "countryId";
        }

        private void frmSubject_Load(object sender, EventArgs e)
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

        private void frmSubject_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
            clsProc = null;
        }

        private void prcClearData()
        {
            this.txtId.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtShrtName.Text = "";
            this.cboDepartment.Text = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
            this.txtCode.Focus();
        }

        private Boolean fncBlank()
        {
            if (this.txtCode.Text.Length == 0)
            {
                MessageBox.Show("Please provide Code.");
                txtCode.Focus();
                return true;
            }
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide Name.");
                txtName.Focus();
                return true;
            }
            
            //if (this.cboDepartment.Text.Length == 0)
            //{
            //    MessageBox.Show("Please provide  Name.");
            //    cboDepartment.Focus();
            //    return true;
            //}
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to delete Subject information of [" + txtName.Text + "]", "",
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
                sqlQuery = "Delete from tbl_Subject  Where SubjectId  = " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','Delete')";
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

        private void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetails = new DataSet();

            try
            {
                string sqlQuery = "Exec prcGetSubject  "+ clsMain.intComId + ", "+ Int32.Parse(strParam)+" ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblDept";

                DataRow dr;
                if (dsDetails.Tables["tblDept"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblDept"].Rows[0];

                    this.txtId.Text = dr["SubjectId"].ToString();
                    this.txtCode.Text = dr["SubjectCode"].ToString();
                    this.txtName.Text = dr["SubjectName"].ToString();
                    this.txtShrtName.Text = dr["shortName"].ToString();
                    this.cboDepartment.Value = dr["SectId"].ToString();
                    this.txtMark.Text = dr["marks"].ToString();
                    this.txtCredit.Text = dr["credit"].ToString();
                    this.dtHour.Text = dr["houre"].ToString();
                    this.txtCost.Text = dr["Cr_Cost"].ToString();

                    //this.cboDepartment.Text = dr["countryName"].ToString();

                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells["subjectId"].Value.ToString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            if (txtCode.ForeColor != Color.Black)
            {
                txtCode.Text = "";
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtCode_MouseClick(object sender, MouseEventArgs e)
        {
            //clsProc.GTRGotFocus(ref txtCode);
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            if (txtCode.Text.Length <= 0)
            {
                txtCode.Text = "Subject Code";
                txtCode.ForeColor = Color.Gray;
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            if(txtName.ForeColor != Color.Black)
            {
                txtName.Text = "";
            }
        }
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void cboDepartment_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //select A.,A.,

            cboDepartment.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboDepartment.DisplayLayout.Bands[0].Columns["SectName"].Width = cboDepartment.Width;
            cboDepartment.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Department";

            cboDepartment.ValueMember = "SectId";
            cboDepartment.DisplayMember = "SectName";

            

            //cboDepartment.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Country Short Name";
            //cboDepartment.DisplayLayout.Bands[0].Columns[4].Header.Caption = "Currency Name";
        }

        private void txtName_MouseClick(object sender, MouseEventArgs e)
        {
           // clsProc.GTRGotFocus(ref txtName);
        }

        private void txtMark_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCredit_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtHour_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (txtName.Text.Length <= 0)
            {
                txtName.Text = "Subject Name";
                txtName.ForeColor = Color.Gray;
            }
        }

        private void txtShrtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtCost_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
    }
}
