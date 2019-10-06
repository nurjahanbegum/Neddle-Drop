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
using System.Globalization;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmEmpCardAssign : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;


        public string GTRHexToDecCon(string proxCardNo)
        {
            string str = int.Parse(proxCardNo, NumberStyles.HexNumber).ToString();
            switch (str.Length)
            {
                case 3:
                    str = "0000000" + str;
                    break;
                case 4:
                    str = "000000" + str;
                    break;
                case 5:
                    str = "00000" + str;
                    break;
                case 6:
                    str = "0000" + str;
                    break;
                case 7:
                    str = "000" + str;
                    break;
                case 8:
                    str = "00" + str;
                    break;
                case 9:
                    str = "0" + str;
                    break;
            }
            return str;
        }

        //Left Code
        public static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }

        //Right Code
        public static string Right(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }


        public frmEmpCardAssign(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetCardAssign " + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblEmployee";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblEmployee"];


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

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetCardAssign " + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblCard";

                DataRow dr;
                if (dsDetails.Tables["tblCard"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["tblCard"].Rows[0];

                    this.txtId.Text = dr["EmpId"].ToString();
                    this.txtEmpCode.Value = dr["EmpCode"].ToString();
                    this.txtName.Text = dr["EmpName"].ToString();
                    this.dtJoinDate.Text = dr["dtJoin"].ToString();
                    this.dtCardAssign.Text = dr["dtCardAssign"].ToString();
                    this.txtCard.Text = dr["CardNo"].ToString();



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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["EmpId"].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmpCardAssign_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmEmpCardAssign_FormClosing(object sender, FormClosingEventArgs e)
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
            this.txtEmpCode.Text = "";
            this.txtName.Text = "";
            this.txtCard.Text = "";
            this.dtCardAssign.Value = DateTime.Now;
            this.dtJoinDate.Value = DateTime.Now;


            this.btnSave.Text = "&Save";
            this.txtEmpCode.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
             if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int64 NewId = 0;
            Int64 ChkCard = 0;

            DateTime firstDay = new DateTime(dtCardAssign.DateTime.Year, dtCardAssign.DateTime.Month, 1);
            dtFrom.Value = firstDay;

            sqlQuery = "Select dbo.fncCheckCardAssign (" + Common.Classes.clsMain.intComId + ", '" + this.txtCard.Text.ToString() + "')";
            ChkCard = clsCon.GTRCountingDataLarge(sqlQuery);

            if (ChkCard == 1)
            {
                MessageBox.Show("This Employee Card already Exist. Please input another Punch Card Number.");
                return;
            }

            try
            {
                    //Update data
                    sqlQuery = "Update tblEmp_Info Set CardNo = '" + txtCard.Text.ToString() + "' + 'Rel' Where ComId =  " + Common.Classes.clsMain.intComId + " and CardNo = '" + txtCard.Text.ToString() + "' and Len(CardNo)=10 and isinactive = 1";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Update tblEmp_Info Set dtCardAssign = '" + clsProc.GTRDate(dtCardAssign.Value.ToString()) + "',CardNo = '" + txtCard.Text.ToString() + "' Where ComId =  " + Common.Classes.clsMain.intComId + " and EmpId = " + Int32.Parse(txtId.Text.ToString());
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType,EmpId)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update','" + txtId.Value.ToString() + "')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Update R Set R.EmpId = E.EmpId from tblRawData R,tblEmp_Info E Where R.CardNo = E.CardNo and E.CardNo = '" + txtCard.Text.ToString() + "' and R.dtPunchDate >= '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "' and E.ComId =  " + Common.Classes.clsMain.intComId + "";
                    arQuery.Add(sqlQuery);


                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully.");

                prcClearData();
                txtEmpCode.Focus();

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

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                gridList.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;  //Country Name
              
                //Set Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;  //EmpCode
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 160;  //Name
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 100;  //Join Date
                gridList.DisplayLayout.Bands[0].Columns["dtCardAssign"].Width = 100;  //Card Assign Date
                gridList.DisplayLayout.Bands[0].Columns["CardNo"].Width = 100;  //Card No

                //Set Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name"; 
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridList.DisplayLayout.Bands[0].Columns["dtCardAssign"].Header.Caption = "Assign Date";
                gridList.DisplayLayout.Bands[0].Columns["CardNo"].Header.Caption = "CardNo";


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

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private Boolean fncBlank()
        {
            if (this.txtName.Text.Length == 0)
            {
                MessageBox.Show("Please provide Employee Name.");
                txtName.Focus();
                return true;
            }

           
            if (this.txtEmpCode.Text.Length == 0)
            {
                MessageBox.Show("Please provide Employee ID.");
                txtEmpCode.Focus();
                return true;
            }

            if (this.txtCard.Text.Length == 0)
            {
                MessageBox.Show("Please provide Card No.");
                txtCard.Focus();
                return true;
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
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

        private void txtNameShort_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNameShort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void dtJoinDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void dtReleasedDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        //private void txtCard_ValueChanged(object sender, EventArgs e)
        //{

        //    string TmpNum = "", TmpNum1 = "", TmpNum2 = "", Hex1 = "", Hex2 = "";

        //    if (txtCard.Text.Length == 8)
        //    {
                
        //        TmpNum = txtCard.Text.ToString();
        //        Hex1 = (Left(TmpNum,3));
        //        Hex2 = (Right(TmpNum, 5));

        //        int val1 = Convert.ToInt32(Hex1);
        //        string hexval1 = val1.ToString("X");

        //        int val2 = Convert.ToInt32(Hex2);
        //        string hexval2 = val2.ToString("X");

        //        if (hexval2.Length == 2)
        //        {
        //            hexval2 = "0" + hexval2;
        //        }

        //        if (hexval1.Length == 2 && hexval2.Length < 4)
        //        {
        //            hexval1 = hexval1 + "0";
        //        }
        //        else
        //        {
        //            hexval1 = hexval1;
        //        }

        //        TmpNum1 = hexval1 + hexval2;

        //        if (TmpNum1.Length == 2)
        //        {
        //            TmpNum2 =  "00000000" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 3)
        //        {
        //            TmpNum2 =  "0000000" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 4)
        //        {
        //            TmpNum2 =  "000000" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 5)
        //        {
        //            TmpNum2 =  "00000" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 6)
        //        {
        //            TmpNum2 =  "0000" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 7)
        //        {
        //            TmpNum2 =  "000" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 8)
        //        {
        //            TmpNum2 =  "00" + TmpNum1;
        //        }
        //        else if (TmpNum1.Length == 9)
        //        {
        //            TmpNum2 =  "0" + TmpNum1;
        //        }
        //        else
        //        {
        //            TmpNum2 =  TmpNum1;
        //        }


        //        txtCard.Text = TmpNum2;


        //    }

        //}




    }
}
