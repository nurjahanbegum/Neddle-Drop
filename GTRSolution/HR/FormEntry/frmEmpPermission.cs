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
using System.Data.OleDb;
using System.Net;
using System.IO.Ports;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace GTRHRIS.HR.FormEntry
{
    public partial class frmEmpPermission : Form
    {
        private string strValue = "";

        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private string Data = "";

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmpPermission(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmEmpPermission_Load(object sender, EventArgs e)
        {
            try
            {
                lblCaption.Text = this.Tag + " Permission..";
                prcClearData();
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmEmpPermission_FormClosing(object sender, FormClosingEventArgs e)
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


        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcGetEmpPermission] 0," + Common.Classes.clsMain.intComId + "," + Common.Classes.clsMain.intUserId + ",'0'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";
                dsList.Tables[1].TableName = "tblPName";
                //dsList.Tables[2].TableName = "tblSect";
                //dsList.Tables[3].TableName = "tblSubSect";
                //dsList.Tables[4].TableName = "tblBand";
                //dsList.Tables[5].TableName = "tblShift";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];

                gridRpt.DataSource = null;
                gridRpt.DataSource = dsList.Tables["tblPName"];


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


        }

        private void prcClearData()
        {
            this.gridList.DataSource = null;
            this.btnSave.Text = "&Approve";
            this.btnDelete.Enabled = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";
                gridList.DisplayLayout.Bands[0].Columns["ComName"].Header.Caption = "Company Name";

                gridList.DisplayLayout.Bands[0].Columns["EmpId"].Width = 70;
                gridList.DisplayLayout.Bands[0].Columns["ComName"].Width = 200;
	
                this.gridList.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;


               // this.gridList.DisplayLayout.Bands[0].Columns["dtPunchDate"].Format = "dd-MMM-yyyy";

                //Stop Cell Modify
                gridList.DisplayLayout.Bands[0].Columns["EmpId"].CellActivation = Activation.NoEdit;
                gridList.DisplayLayout.Bands[0].Columns["empName"].CellActivation = Activation.NoEdit;

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;


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


        private Boolean fncBlank()
        {


            //if (optCriteria.Value == "All")
            //{
            //    //Data = "";
            //    if (this.cboDept.Text.Length == 0)
            //    {
            //        MessageBox.Show("Please provide Department");
            //        cboDept.Focus();
            //        return true;
            //    }
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
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", Option = "";

            Option = gridRpt.ActiveRow.Cells["PName"].Value.ToString();

            try
            {
                //Member Master Table

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = "Exec PrcProcessPermission 0," + Common.Classes.clsMain.intComId + "," + 
                                        Common.Classes.clsMain.intUserId + ",'" + Option + "','" + 
                                        row.Cells["empid"].Text.ToString() + "','" + 
                                        row.Cells["Remarks"].Text.ToString() + "','" + 
                                        Common.Classes.clsMain.strComputerName + "'";
                            arQuery.Add(sqlQuery);

                            //prcMail();

                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                                       sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Approve')";
                            arQuery.Add(sqlQuery);
                        }
                    }


                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Permission Approved Succefully Complete.");


                prcClearData();
                prcLoadData();
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
            if (
                MessageBox.Show("Do you want to Delete FixAttendance Which Are shown in the Grid", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "";
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                {
                    if (row.Cells["empid"].Text.ToString().Length != 0)
                    {
                        //RowID = row.Index + 1;
                        ///CONVERT(VARCHAR,OtHour,108) AS  FROM  tblOTPermission As A

                        sqlQuery = " Delete  tblOTPermission where empid = '" + row.Cells["empid"].Text.ToString() +
                                   "' and dtPunchDate =  '" + row.Cells["dtPunchDate"].Text.ToString() + "'";
                        arQuery.Add(sqlQuery);

                    }
                }

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                //prcClearData();
                prcLoadData();
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


        private void gridList_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked == true)
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



        private void gridRpt_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridRpt.DisplayLayout.Bands[0].Columns["SL"].Hidden = true;

            gridRpt.DisplayLayout.Bands[0].Columns["PName"].Header.Caption = "Permission";

            gridRpt.DisplayLayout.Bands[0].Columns["PName"].Width = 275;
            //Change alternate color
            gridRpt.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridRpt.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridRpt.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridRpt.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridRpt.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridReportCategory.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void prcLoadData()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            string sqlQuery = "", Option = "";

            Option = gridRpt.ActiveRow.Cells["PName"].Value.ToString();

            lblCaption.Text = Option + " Permission..";

            try
            {
                sqlQuery = "Exec [prcGetEmpPermission] 1," + Common.Classes.clsMain.intComId + "," + Common.Classes.clsMain.intUserId + ",'" + Option + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblData"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }


        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //if (fncBlank())
            //{
            //    return;
            //}

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            string sqlQuery = "", Option = "";

            Option = gridRpt.ActiveRow.Cells["PName"].Value.ToString();

            lblCaption.Text = Option + " Permission..";

            try
            {
                sqlQuery = "Exec [prcGetEmpPermission] 1," + Common.Classes.clsMain.intComId + "," + Common.Classes.clsMain.intUserId + ",'" + Option + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "tblData";

                gridList.DataSource = null;
                gridList.DataSource = dsDetails.Tables["tblData"];


                if (dsDetails.Tables["tblData"].Rows.Count > 0)
                {
                    btnLoad.Enabled = true;
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
                //   throw;
            }
        }

        private void btnDisApprove_Click(object sender, EventArgs e)
        {

            if ( MessageBox.Show("Are you sure permission disapprove this employee?", "",
                    System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            
            if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "", Option = "";

            Option = gridRpt.ActiveRow.Cells["PName"].Value.ToString();

            try
            {
                //Member Master Table

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridList.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = "Exec PrcProcessPermission 1," + Common.Classes.clsMain.intComId + "," +
                                        Common.Classes.clsMain.intUserId + ",'" + Option + "','" +
                                        row.Cells["empid"].Text.ToString() + "','" +
                                        row.Cells["Remarks"].Text.ToString() + "','" +
                                        Common.Classes.clsMain.strComputerName + "'";
                            arQuery.Add(sqlQuery);

                            //prcMail();

                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                       + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                                       sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Disapprove')";
                            arQuery.Add(sqlQuery);
                        }
                    }


                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Disapprove Succefully Done.");

                prcClearData();
                prcLoadData();
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


        //private void prcMail()
        //{

        //    try
        //    {
        //        #region Sending Mail

        //        string fromAdd = "system@gtrbd.com";
        //        string fromAddCaption = "";
        //        string passMail = "count123456";
        //        string toAdd = "";
        //        string strSubject = "";
        //        string strBody = "";
        //        string strSendMail = "";

        //        string empName = "";
        //        string empEmail = "";
        //        string empMailPass = "";
        //        string deskEmail = "";
        //        string rptEmail = "";

        //        DataSet ds = new DataSet();
        //        string sqlQuery = "Exec WebprcMailLeave " + txtLeaveId.Text.ToString().Replace("'", "") + ", 0";
        //        clsCon.GTRFillDatasetWithSQLCommand(ref ds, sqlQuery);
        //        ds.Tables[0].TableName = "Mail";

        //        DataRow[] dr = ds.Tables["Mail"].Select();
        //        foreach (var dr2 in dr)
        //        {
        //            empName = dr2["empName"].ToString();
        //            empEmail = dr2["empEmail"].ToString();
        //            empMailPass = dr2["empPass"].ToString();
        //            deskEmail = dr2["deskEmail"].ToString();
        //            rptEmail = dr2["rptEmail"].ToString();
        //        }
        //        dr = null;

        //        if(intStatus == 1)
        //        {
        //            //Mail To Final Approval
        //            //fromAdd = empEmail.ToString();
        //            fromAddCaption = empName.ToString();
        //            //passMail = empMailPass.ToString();
        //            toAdd = txtAppMail.Text.ToString();
        //            strSubject = "Leave Application";
        //            strBody = clsMail.fncMailBodyLeave("FinalApprove", ref ds);
        //            strSendMail = clsCom.fncSendMail("CDOSYS", fromAdd, fromAddCaption, toAdd, "", "", strSubject,
        //                                             strBody, passMail);
        //            strSendMail = "";                     
        //        }
        //        else if (intStatus == 2 || intStatus == 4)
        //        {
        //            //Mail To Applicant
        //            //fromAdd = txtrptMail.Text.ToString();
        //            fromAddCaption = txtrptMail.Text.ToString();
        //            //passMail = txtrptPass.Text.ToString();
        //            toAdd = empEmail;
        //            strSubject = "Leave Application Confirmation";
        //            strBody = clsMail.fncMailBodyLeave("Confirmation", ref ds);
        //            strSendMail = clsCom.fncSendMail("CDOSYS", fromAdd, fromAddCaption, toAdd, "", "", strSubject,
        //                                             strBody, passMail);
        //            strSendMail = "";
        //        }
        //        else if (intStatus == 3)
        //        {
        //            //Mail To Applicant
        //            //fromAdd = txtrptMail.Text.ToString();
        //            fromAddCaption = txtrptMail.Text.ToString();
        //            //passMail = txtrptPass.Text.ToString();
        //            toAdd = empEmail;
        //            strSubject = "Leave Application Confirmation";
        //            strBody = clsMail.fncMailBodyLeave("Confirmation", ref ds);
        //            strSendMail = clsCom.fncSendMail("CDOSYS", fromAdd, fromAddCaption, toAdd, "", "", strSubject,
        //                                             strBody, passMail);
        //            strSendMail = "";
        //        }
        //        #endregion Sending Mail
        //    }

        //private void prcMail()
        //{

        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        //SmtpClient SmtpServer = new SmtpClient("server.networxhost.com");
        //        SmtpClient SmtpServer = new SmtpClient("smtp.gtrbd.com");
        //        mail.From = new MailAddress("system@gtrbd.com");
        //        mail.To.Add("asad@gtrbd.com");
        //        mail.Subject = "Test Mail";
        //        mail.Body = "Dear Applicant \n" +
        //                    "This is a system generated email to inform you that your leave application has been approved by your final approval supervisor. \n" +
        //                    "The following are the details of his/her leave application :-  \n \n" +
        //                    "Applicant Information \n" +
        //                    "ID No	:	GTRC-34 ";
        //        //mail.Body = "First Line \n second line";



        //        System.Net.Mail.Attachment attachment;
        //        attachment = new System.Net.Mail.Attachment("e:/Regency.txt");
        //        mail.Attachments.Add(attachment);

        //        SmtpServer.Port = 587;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("system@gtrbd.com", "count123456");
        //        SmtpServer.EnableSsl = true;

        //        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        //        SmtpServer.Send(mail);
        //        MessageBox.Show("mail Send");
        //        mail.Attachments.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //}

    }
}