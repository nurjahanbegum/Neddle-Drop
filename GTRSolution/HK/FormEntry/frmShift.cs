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

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmShift : Form
    {
        private string strTranWith = "";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();
        private Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
        private int secId_update = 0; // used for update section

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmShift(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmShift_FormClosing(object sender, FormClosingEventArgs e)
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

        public Boolean fncBlank()
        {
            //if(txtCode.Text.ToString().Trim()=="")
            //{
            //    MessageBox.Show("Provide Shift Code.");
            //    txtCode.Focus();
            //    return true;
            //}
            if (txtName.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Shift Name.");
                txtName.Focus();
                return true;
            }
            //if (cboType .Text.ToString().Trim() == "")
            //{
            //    MessageBox.Show("Provide Shift Type.");
            //    cboType.Focus();
            //    return true;
            //}

            //if (cboCategor.Text.ToString().Trim() == "")
            //{
            //    MessageBox.Show("Provide Shift Category.");
            //    cboCategor.Focus();
            //    return true;
            //}
            if (txtInTime .Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide In Time.");
                txtInTime.Focus();
                return true;
            }
            if (txtOutTime.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Out Time.");
                txtOutTime.Focus();
                return true;
            }
            if (txtShiftLate .Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Allowed Late Time.");
                txtShiftLate.Focus();
                return true;
            }
            if (txtRegHrs.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Regular Houre.");
                txtRegHrs.Focus();
                return true;
            }
            if (txtLunch .Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Lunch Time.");
                txtLunch.Focus();
                return true;
            }
            if (txtLunchIn.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Lunch In Time.");
                txtLunchIn.Focus();
                return true;
            }
            if (txtLunchOut.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Lunch Out Time.");
                txtLunchOut.Focus();
                return true;
            }
            if (txtTiffin .Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Tiffin-1 Time.");
                txtTiffin.Focus();
                return true;
            }
            if (txtTiffinIn.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Tiffin In Time.");
                txtTiffinIn.Focus();
                return true;
            }
            if (txtTiffin1.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Provide Tiffin-2  Time.");
                txtTiffin1.Focus();
                return true;
            }
            return false;
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetShift  " + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Shift";
                dsList.Tables[1].TableName = "Type";
                dsList.Tables[2].TableName = "Category";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Shift"];
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
            cboType.DataSource = dsList.Tables["Type"];

            cboCategor.DataSource = null;
            cboCategor.DataSource = dsList.Tables["Category"];
        }

        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec prcGetShift  " + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, sqlQuery);
                dsDetail.Tables[0].TableName = "Shift";
                DataRow dr;

                if (dsDetail.Tables["Shift"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["Shift"].Rows[0];
                    txtId.Text = dr["ShiftId"].ToString();
                    txtCode.Text = dr["ShiftCode"].ToString();
                    txtName.Text = dr["ShiftName"].ToString();
                    txtDescription.Text = dr["ShiftDesc"].ToString();
                    cboType.Text = dr["ShiftType"].ToString();
                    cboCategor.Text = dr["ShiftCat"].ToString();
                    txtInTime.Text = dr["ShiftIn"].ToString();
                    txtOutTime.Text = dr["ShiftOut"].ToString();
                    txtShiftLate.Text = dr["ShiftLate"].ToString();
                    txtLunch.Text = dr["LunchTime"].ToString();
                    txtLunchIn.Text = dr["LunchIn"].ToString();
                    txtLunchOut.Text = dr["LunchOut"].ToString();
                    txtTiffin.Text = dr["TiffinTime"].ToString();
                    txtTiffinIn.Text = dr["TiffinIn"].ToString();
                    txtTiffin1.Text = dr["TiffinTime1"].ToString();
                    txtTiffinIn1.Text = dr["TiffinTimeIn1"].ToString();
                    txtTiffin2.Text = dr["TiffinTime2"].ToString();
                    txtTiffinIn2.Text = dr["TiffinTimeIn2"].ToString();
                    txtRegHrs.Text = dr["RegHour"].ToString();

                    if (dr["IsInactive"].ToString() == "1")
                    {
                        chkIsInactive.Checked = true;
                    }
                    else
                    {
                        chkIsInactive.Checked = false;
                    }
                    this.btnSave.Text = " &Update";
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

        public void prcClearData()
        {
            txtId.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            cboType.Text = "";
            cboCategor.Text = "";
            txtInTime.Text = "00:00:00";
            txtOutTime.Text = "00:00:00";
            txtShiftLate.Text = "00:00:00";
            txtLunch.Text = "00:00:00";
            txtLunchIn.Text = "00:00:00";
            txtLunchOut.Text = "00:00:00";
            txtTiffin.Text = "00:00:00";
            txtTiffinIn.Text = "00:00:00";
            txtTiffin1.Text = "00:00:00";
            txtTiffinIn1.Text = "00:00:00";
            txtTiffin2.Text = "00:00:00";
            txtTiffinIn2.Text = "00:00:00";
            txtRegHrs.Text = "00:00:00";
            chkIsInactive.Checked = false ;

            txtCode.Focus();
            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;

        }

        private void frmShift_Load(object sender, EventArgs e)
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
            gridList.DisplayLayout.Bands[0].Columns["ShiftId"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["ShiftCode"].Header.Caption = "Code";
            gridList.DisplayLayout.Bands[0].Columns["ShiftName"].Header.Caption = "Shift Name";
            gridList.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Type";
            gridList.DisplayLayout.Bands[0].Columns["ShiftCat"].Header.Caption = "Category";
            gridList.DisplayLayout.Bands[0].Columns["ShiftIn"].Header.Caption = "In Time";
            gridList.DisplayLayout.Bands[0].Columns["ShiftOut"].Header.Caption = "Out Time";
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Header.Caption = "Inactive";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["ShiftCode"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["ShiftName"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["ShiftType"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["ShiftCat"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["ShiftIn"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["ShiftOut"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Width = 80;

            //Set column Style
            gridList.DisplayLayout.Bands[0].Columns["IsInactive"].Style = ColumnStyle.CheckBox;


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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                if (btnSave.Text.ToString().ToUpper() != "&Save".ToUpper())
                {
                    //Update  
                    //--------------------
                    sqlQuery = "Update tblCat_Shift Set ShiftCode = '" + txtId.Text.ToString().Trim() +
                               "', ShiftName = '" + txtName.Text.ToString().Trim() + "', ShiftDesc = '" +
                               txtDescription.Text.ToString().Trim() + "', ShiftType = 'G', ShiftCat = 'G', ShiftIn = '" +  
                               (txtInTime.Value.TimeOfDay.ToString().Trim()) + "', ShiftOut = '" +
                                (txtOutTime.Value.TimeOfDay.ToString().Trim()) + "', ShiftLate = '" +
                                (txtShiftLate.Value.TimeOfDay.ToString().Trim()) + "', LunchTime = '" +
                                (txtLunch.Value.TimeOfDay.ToString().Trim()) + "', LunchIn = '" +
                                (txtLunchIn.Value.TimeOfDay.ToString().Trim()) +
                               "', LunchOut = '" + (txtLunchOut.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinTime = '" + (txtTiffin.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinIn = '" + (txtTiffinIn.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinTime1 = '" + (txtTiffin1.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinTimeIn1 = '" + (txtTiffinIn1.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinTime2 = '" + (txtTiffin2.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinTimeIn2 = '" + (txtTiffinIn2.Value.TimeOfDay.ToString().Trim()) +
                               "', TiffinOut = '00:00',RegHour = '" + (txtRegHrs.Value.TimeOfDay.ToString().Trim()) +
                               "',  IsInactive = " + chkIsInactive.Tag + 
                               " where ShiftId = " + int.Parse(txtId.Text.ToString().Trim()) + "";
                    //----------------------
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    sqlQuery = " Select Isnull(Max(ShiftId),0)+1 As NewId from tblCat_Shift ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    //--------------------------
                    sqlQuery =
                        " Insert Into tblCat_Shift(ShiftId, ShiftCode, ShiftName, ShiftDesc, ShiftType, ShiftCat, ShiftIn, ShiftOut, ShiftLate, LunchTime, LunchIn, LunchOut, TiffinTime, TiffinIn, TiffinTime1,TiffinTimeIn1,TiffinTime2,TiffinTimeIn2,TiffinOut, RegHour,  IsInactive, aId, PCName, LUserId,ComID)" +
                        " Values(" + NewId + ", '" + NewId + "','" + txtName.Text.ToString() + "','" + txtDescription.Text.ToString() + "','G','G','" + 
                         (txtInTime.Value.TimeOfDay.ToString().Trim()) + "','" + (txtOutTime.Value.TimeOfDay.ToString().Trim()) + "','" +
                         (txtShiftLate.Value.TimeOfDay.ToString().Trim()) + "','" + (txtLunch.Value.TimeOfDay.ToString().Trim()) + "','" + (txtLunchIn.Value.TimeOfDay.ToString().Trim()) + "','" + (txtLunchOut.Value.TimeOfDay.ToString().Trim()) + "','" +
                         (txtTiffin.Value.TimeOfDay.ToString().Trim()) + "','" + (txtTiffinIn.Value.TimeOfDay.ToString().Trim()) + "','" + (txtTiffin1.Value.TimeOfDay.ToString().Trim()) + "','" + (txtTiffinIn1.Value.TimeOfDay.ToString().Trim()) + "','" + 
                         (txtTiffin2.Value.TimeOfDay.ToString().Trim()) + "','" + (txtTiffinIn2.Value.TimeOfDay.ToString().Trim()) + "','00:00','" + (txtRegHrs.Value.TimeOfDay.ToString().Trim()) + "','" + chkIsInactive.Tag.ToString() + "'," +
                        NewId + ",'" + Common.Classes.clsMain.strComputerName + "'," + Common.Classes.clsMain.intUserId + "," + Common.Classes.clsMain.intComId + ")";
                    arQuery.Add(sqlQuery);
                    //----------------------------------------
                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);


                    MessageBox.Show("Data Saved Succefully");

                }


                prcClearData();
                prcLoadList();


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

            private void btnDelete_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show("Do you want to delete Shift information of [" +gridList.ActiveRow.Cells[1].Text.ToString() +"]", "",
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
                    sqlQuery = "Delete From tblCat_Shift where ShiftId =  " + Int32.Parse(txtId.Text) + " and ComID = " + Common.Classes.clsMain.intComId + "";
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
            }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void cboType_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void cboCategor_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }
        private void chkIsInactive_KeyDown(object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void cboCategor_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

      
        private void chkIsInactive_KeyPress (object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtName_KeyPress_1 (object sender, KeyPressEventArgs e)
            {
                clsProc.GTRSingleQuote((Int16) e.KeyChar);
            }

        private void txtName_KeyDown_1 (object sender, KeyEventArgs e)
            {
                clsProc.GTRTabMove((Int16) e.KeyCode);
            }

        private void cboType_InitializeLayout (object sender, InitializeLayoutEventArgs e)
            {
                cboType.DisplayLayout.Bands[0].Columns["ShiftType"].Header.Caption = "Type";
                cboType.DisplayLayout.Bands[0].Columns["ShiftType"].Width = cboType.Width;
                cboType.DataMember = "ShiftType";
            }

        private void cboCategor_InitializeLayout (object sender, InitializeLayoutEventArgs e)
            {
                cboCategor.DisplayLayout.Bands[0].Columns["ShiftCat"].Header.Caption = "Category";
                cboCategor.DisplayLayout.Bands[0].Columns["ShiftCat"].Width = cboCategor.Width;
                cboCategor.DataMember = "ShiftCat";
            }

        private void gridList_DoubleClick (object sender, EventArgs e)
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
            }

        private void chkIsInactive_CheckedChanged(object sender, EventArgs e)
        {
            chkIsInactive.Tag = 0;
            if (chkIsInactive.Checked == true)
            {
                chkIsInactive.Tag = 0;
            }
        }

        private void txtInTime_KeyDown_1(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtOutTime_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtShiftLate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtRegHrs_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunch_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunchIn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunchOut_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTiffin_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTiffinIn_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtTiffinOut_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtLunch_ValueChanged(object sender, EventArgs e)
        {

        }


        }
   }


