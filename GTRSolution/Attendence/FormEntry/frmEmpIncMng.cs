using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GTRHRIS.Common;
using GTRHRIS.Attendence.FormEntry;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmEmpIncMng : Form
    {
        private string X = " ";
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private System.Data.DataView dvSection;
        private DataView dvGrid;

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmpIncMng(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmEmpIncMng_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmpIncMng_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void incrSal()
        {
            //if (txtAmt.Text.Length>0)
            //{
            //    txtAmt.Text = 
            //}

        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtIncDate.Value = firstDay;


            try
            {
                string sqlQuery = "Exec [prcGetIncrementMng] " + Common.Classes.clsMain.intComId + ", 0,0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblEmployee";
                dsList.Tables[1].TableName = "tblIncrementType";
                dsList.Tables[2].TableName = "tblsec";
                dsList.Tables[3].TableName = "tblCat_Desig";
                dsList.Tables[6].TableName = "tblType";
                dsList.Tables[7].TableName = "tblGridList";
                dsList.Tables[8].TableName = "tblGridInc";
                dsList.Tables[9].TableName = "FilterValue";
                dsList.Tables[10].TableName = "FilterOperetor";

                dvGrid = dsList.Tables["tblGridList"].DefaultView;
                gridList.DataSource = null;
                gridList.DataSource = dvGrid;

                dvGrid = dsList.Tables["tblGridInc"].DefaultView;
                gridInc.DataSource = null;
                gridInc.DataSource = dvGrid;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clsCon = null;
            }
        }

        private void prcLoadCombo()
        {
            cboEmpID.DataSource = null;
            cboEmpID.DataSource = dsList.Tables["tblEmployee"];
            cboEmpID.DisplayMember = "empcode";
            cboEmpID.ValueMember = "empid";

            cboType.DataSource = null;
            cboType.DataSource = dsList.Tables["tblIncrementType"];
            cboType.DisplayMember = "inctype";
            cboType.ValueMember = "inctypid";

            cboNewSection.DataSource = null;
            cboNewSection.DataSource = dsList.Tables["tblsec"];
            cboNewSection.DisplayMember = "sectname";
            cboNewSection.ValueMember = "sectid";

            cboSection.DataSource = null;
            cboSection.DataSource = dsList.Tables["tblsec"];
            cboSection.DisplayMember = "sectname";
            cboSection.ValueMember = "sectid";

            cboPrevDesig.DataSource = null;
            cboPrevDesig.DataSource = dsList.Tables["tblCat_Desig"];
            cboPrevDesig.DisplayMember = "designame";
            cboPrevDesig.ValueMember = "desigid";

            cboNewDesig.DataSource = null;
            cboNewDesig.DataSource = dsList.Tables["tblCat_Desig"];
            cboNewDesig.DisplayMember = "designame";
            cboNewDesig.ValueMember = "desigid";


            cboNewStatus.DataSource = null;
            cboNewStatus.DataSource = dsList.Tables["tblType"];
            cboNewStatus.DisplayMember = "varName";
            cboNewStatus.ValueMember = "varName";

            cboPrevStatus.DataSource = null;
            cboPrevStatus.DataSource = dsList.Tables["tblType"];
            cboPrevStatus.DisplayMember = "varName";
            cboPrevStatus.ValueMember = "varName";
        }

        private void prcDisplayDetails(string strParam)
        {
            dsDetails = new System.Data.DataSet();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                string sqlQuery = "Exec prcGetIncrementMng " + Common.Classes.clsMain.intComId + " , " +Int32.Parse(strParam) + ",0 ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];
                    this.txtID.Text = dr["incid"].ToString();
                    this.cboEmpID.Value = dr["empid"].ToString();
                    this.txtDesignation.Text = dr["OldDesigName"].ToString();
                    this.cboSection.Text = dr["OldSectId"].ToString();
                    this.txtsect.Text = dr["OldSectName"].ToString();
                    this.dtIncDate.Value = dr["dtInc"].ToString();
                    this.cboType.Text = dr["incType"].ToString();
                    this.cboPrevDesig.Text = dr["OldDesigName"].ToString();
                    this.txtPrevSalary.Text = dr["OldSal"].ToString();
                    this.txtNewSal.Text = dr["newSal"].ToString();
                    this.txtPer.Text = dr["Percentage"].ToString();

                    this.txtAmt.Text = "0";
                    txtIncAmount.Text = dr["amount"].ToString();

                    this.txtBS.Text = dr["BS"].ToString();
                    this.txtOTA.Text = dr["HR"].ToString();

                    this.txtPrevGS.Text = "0";
                    this.txtPrevBS.Text = "0";
                    this.txtPrevOTA.Text = "0";

                    this.cboNewStatus.Text = dr["NewStatus"].ToString();
                    this.cboPrevStatus.Text = dr["OldStatus"].ToString();

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

        private void prcClearData()
        {

            txtID.Text = "";
            cboEmpID.Text = "";
            cboEmpID.Value = null;

            txtName.Text = "";
            txtsect.Text = "";
            txtDesignation.Text = "";
            cboSection.Text = "";

            dtIncDate.Value = DateTime.Now;
            cboType.Text = "";
            cboPrevDesig.Text = "";
            cboNewDesig.Text = "";

            txtPrevSalary.Text = "";

            txtAmt.Value = 0;

            txtIncAmount.Value=0;

            txtPer.Value = 0;
            txtNewSal.Value = 0;

            txtBS.Value = 0;
            txtOTA.Value = 0;

            txtPrevGS.Value = 0;
            txtPrevBS.Value = 0;
            txtPrevOTA.Value = 0;

            cboPrevStatus.Value = null;
            cboNewSection.Value = null;
            cboSection.Value = null;

            cboNewStatus.Value = null;


            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            //this.txtCode.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            var firstDay = new DateTime(dtIncDate.DateTime.Year, dtIncDate.DateTime.Month, 1);
            dtIncDate.Value = firstDay;

            try
            {
                if (dtIncDate.Value != null)
                {
                    dtIncDate.Value = clsProc.GTRDate(dtIncDate.Value.ToString());
                }
                if (cboNewSection.Text.Length == 0)
                {
                    cboNewSection.Value = 0;
                }
                if (cboNewDesig.Text.Length == 0)
                {
                    cboNewDesig.Value = 0;
                }
                if (cboPrevStatus.Text.Length == 0)
                {
                    cboPrevStatus.Value = 0;
                }

                if (cboNewStatus.Text.Length == 0)
                {
                    cboNewStatus.Value = 0;
                }
                //Member Master Table
                if (btnSave.Text != "&Save")
                {


                    if (cboType.Text == "Increment with Promotion")
                    {
                        //Update
                        sqlQuery = "Update tblEmp_Incr Set  IncType = '" + cboType.Text + "', dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "', Amount = '" + txtIncAmount.Text +
                                   "', Percentage = '" + clsProc.GTRValidateDouble(txtPer.Text.ToString()) + "', OldSal =  " + clsProc.GTRValidateDouble(txtPrevSalary.Text.ToString()) + ", NewSal = " + clsProc.GTRValidateDouble(txtNewSal.Text.ToString()) +
                                   ", OldDesigId =" + cboPrevDesig.Value.ToString() + ", NewDesigId = " + cboNewDesig.Value.ToString() + ", BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + ",HR =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) +
                                   ", OldSectId = '" + cboSection.Value.ToString() + "', NewSectId = " + cboNewSection.Value +
                                   ",OldStatus = '" + cboPrevStatus.Value.ToString()
                                   + "', NewStatus =  '" + cboNewStatus.Value.ToString() +
                                   "', LUserId = '" + Common.Classes.clsMain.intUserId + "', PCName = '" + Common.Classes.clsMain.strComputerName + "' " +
                                   " Where IncId = '" + txtID.Value.ToString() + "' And dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' ";
                        arQuery.Add(sqlQuery);


                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                        // Update tblemp_Info Desig
                        sqlQuery = "Update tblemp_Info Set  DesigID = '" + cboNewDesig.Value.ToString() + "',SectID = '" + cboNewSection.Value.ToString() + "',EmpType ='" + (cboNewStatus.Text.ToString()) + "'  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Update Bepza & Regency Grade
                        sqlQuery = "Update E set E.BepzaGrade = D.Grade, E.OfficeGrade = D.OffGrade from tblEmp_Info E,tblCat_Desig D Where E.DesigID = D.DesigId and E.EmpId = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        //Update Gross,BS,HR,MA Salary
                        sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString() 
                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) 
                                   + ", dtIncrement = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) 
                                   + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);



                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Updated Successfully");
                    }


                    else if (cboType.Text == "Promotion with Adjustment")
                    {
                        //Update
                        sqlQuery = "Update tblEmp_Incr Set  IncType = '" + cboType.Text + "', dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "', Amount = '" + txtIncAmount.Text +
                                   "', Percentage = '" + clsProc.GTRValidateDouble(txtPer.Text.ToString()) + "', OldSal =  " + clsProc.GTRValidateDouble(txtPrevSalary.Text.ToString()) + ", NewSal = " + clsProc.GTRValidateDouble(txtNewSal.Text.ToString()) +
                                   ", OldDesigId =" + cboPrevDesig.Value.ToString() + ", NewDesigId = " + cboNewDesig.Value.ToString() + ", BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + ",HR =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) +
                                   ", OldSectId = '" + cboSection.Value.ToString() + "', NewSectId = " + cboNewSection.Value +
                                   ",OldStatus = '" + cboPrevStatus.Value.ToString()
                                   + "', NewStatus =  '" + cboNewStatus.Value.ToString() +
                                   "', LUserId = '" + Common.Classes.clsMain.intUserId + "', PCName = '" + Common.Classes.clsMain.strComputerName + "' " +
                                   " Where IncId = '" + txtID.Value.ToString() + "' And dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' ";
                        arQuery.Add(sqlQuery);


                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                        //Update Designation
                        sqlQuery = "Update tblemp_Info Set  DesigID = '" + cboNewDesig.Value.ToString() + "',SectID = '" + cboNewSection.Value.ToString() + "',EmpType ='" + (cboNewStatus.Text.ToString()) + "'  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        //Update Bepza & Regency Grade
                        sqlQuery = "Update E set E.BepzaGrade = D.Grade, E.OfficeGrade = D.OffGrade from tblEmp_Info E,tblCat_Desig D Where E.DesigID = D.DesigId and E.EmpId = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        //Update Gross,BS,HR,MA Salary
                        sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                   + "  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);



                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Updated Successfully");
                    }

                    else if (cboType.Text == "Confirmation")
                    {

                        if (txtAmt.Text == "0")
                        {
                            //Update
                            sqlQuery = "Update tblEmp_Incr Set  IncType = '" + cboType.Text + "', dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "', Amount = '" + txtIncAmount.Text +
                                       "', Percentage = '" + clsProc.GTRValidateDouble(txtPer.Text.ToString()) + "', OldSal =  " + clsProc.GTRValidateDouble(txtPrevSalary.Text.ToString()) + ", NewSal = " + clsProc.GTRValidateDouble(txtPrevSalary.Text.ToString()) +
                                       ", OldDesigId =" + cboPrevDesig.Value.ToString() + ", NewDesigId = " + cboNewDesig.Value.ToString() + ", BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + ",HR =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) +
                                       ", OldSectId = '" + cboSection.Value.ToString() + "', NewSectId = " + cboNewSection.Value +
                                       ",OldStatus = '" + cboPrevStatus.Value.ToString()
                                       + "', NewStatus =  '" + cboNewStatus.Value.ToString() +
                                       "', LUserId = '" + Common.Classes.clsMain.intUserId + "', PCName = '" + Common.Classes.clsMain.strComputerName + "' " +
                                       " Where IncId = '" + txtID.Value.ToString() + "' And dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' ";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                            arQuery.Add(sqlQuery);

                            // Update  tblemp_Info
                            sqlQuery = "Update tblemp_Info Set  IsConfirm = '1', dtConfirm = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                            arQuery.Add(sqlQuery);


                            //Transaction with database
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);

                            MessageBox.Show("Data Updated Successfully");
                        }

                        else
                        {

                            //Update
                            sqlQuery = "Update tblEmp_Incr Set  IncType = '" + cboType.Text + "', dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "', Amount = '" + txtIncAmount.Text +
                                       "', Percentage = '" + clsProc.GTRValidateDouble(txtPer.Text.ToString()) + "', OldSal =  " + clsProc.GTRValidateDouble(txtPrevSalary.Text.ToString()) + ", NewSal = " + clsProc.GTRValidateDouble(txtNewSal.Text.ToString()) +
                                       ", OldDesigId =" + cboPrevDesig.Value.ToString() + ", NewDesigId = " + cboNewDesig.Value.ToString() + ", BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + ",HR =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) +
                                       ", OldSectId = '" + cboSection.Value.ToString() + "', NewSectId = " + cboNewSection.Value +
                                       ",OldStatus = '" + cboPrevStatus.Value.ToString()
                                       + "', NewStatus =  '" + cboNewStatus.Value.ToString() +
                                       "', LUserId = '" + Common.Classes.clsMain.intUserId + "', PCName = '" + Common.Classes.clsMain.strComputerName + "' " +
                                       " Where IncId = '" + txtID.Value.ToString() + "' And dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' ";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                            arQuery.Add(sqlQuery);

                            // Update  tblemp_Info - Gross,BS,HR,MA Salary
                            sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                                                   + ", IsConfirm = '1', dtConfirm = '" + clsProc.GTRDate(dtIncDate.Value.ToString())
                                                                   + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                            arQuery.Add(sqlQuery);


                            //Transaction with database
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);

                            MessageBox.Show("Data Updated Successfully");

                        }
                    }

                    else 
                    {
                        //Update
                        sqlQuery = "Update tblEmp_Incr Set  IncType = '" + cboType.Text + "', dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "', Amount = '" + txtIncAmount.Text +
                                   "', Percentage = '" + clsProc.GTRValidateDouble(txtPer.Text.ToString()) + "', OldSal =  " + clsProc.GTRValidateDouble(txtPrevSalary.Text.ToString()) + ", NewSal = " + clsProc.GTRValidateDouble(txtNewSal.Text.ToString()) +
                                   ", OldDesigId =" + cboPrevDesig.Value.ToString() + ", NewDesigId = " + cboNewDesig.Value.ToString() + ", BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + ",HR =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) +
                                   ", OldSectId = '" + cboSection.Value.ToString() + "', NewSectId = " + cboNewSection.Value +
                                   ",OldStatus = '" + cboPrevStatus.Value.ToString()
                                   + "', NewStatus =  '" + cboNewStatus.Value.ToString() +
                                   "', LUserId = '" + Common.Classes.clsMain.intUserId + "', PCName = '" + Common.Classes.clsMain.strComputerName + "' " +
                                   " Where IncId = '" + txtID.Value.ToString() + "' And dtInc = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' ";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                        //Update Gross,BS,HR,MA Salary
                        sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                   + ", dtIncrement = '" + clsProc.GTRDate(dtIncDate.Value.ToString())
                                   + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);


                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Updated Successfully");
                    }

                }
                else
                {

                    if (cboType.Text == "Increment with Promotion")
                    {
                        //NewId
                        sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                        NewId = clsCon.GTRCountingData(sqlQuery);

                        //Insert Data
                        sqlQuery = "INSERT into dbo.tblEmp_Incr(IncId, IncType, EmpId, dtInc, Amount, Percentage, OldSal, NewSal, OldDesigId, NewDesigId, BS,HR,OldSectId, NewSectId,OldStatus, NewStatus,IsInactive, ComId, LUserId, PCName)"
                                   + " Values (" + NewId + ", '" + cboType.Text + "','" + this.cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "','" + txtIncAmount.Text + "','" + txtPer.Text + "', '" + txtPrevSalary.Text + "','" +
                                   txtNewSal.Text.ToString() + "', " + cboPrevDesig.Value.ToString() + ",'" + cboNewDesig.Value.ToString() + "', '" + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + "','" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) + "','" + cboSection.Value.ToString() + "', '" + cboNewSection.Value.ToString() + "', '" +
                                   (cboPrevStatus.Text.ToString()) + "','" + (cboNewStatus.Text.ToString()) + "', '0', " +
                                   Common.Classes.clsMain.intComId + ", " + Common.Classes.clsMain.intUserId +
                                   ",'" + Common.Classes.clsMain.strComputerName + "')";
                        arQuery.Add(sqlQuery);


                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        // Update tblemp_Info Desig
                        sqlQuery = "Update tblemp_Info Set  DesigID = '" + cboNewDesig.Value.ToString() + "',SectID = '" + cboNewSection.Value.ToString() + "',EmpType ='" + (cboNewStatus.Text.ToString()) + "'  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        //Update Bepza & Regency Grade
                        sqlQuery = "Update E set E.BepzaGrade = D.Grade, E.OfficeGrade = D.OffGrade from tblEmp_Info E,tblCat_Desig D Where E.DesigID = D.DesigId and E.EmpId = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        //Update Gross,BS,HR,MA Salary
                        sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                   + ", dtIncrement = '" + clsProc.GTRDate(dtIncDate.Value.ToString())
                                   + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        //Increment Permission
                        sqlQuery = "prcProcessEmpApproval " + Common.Classes.clsMain.intComId + "," + NewId + ",'Increment',1";
                        arQuery.Add(sqlQuery);


                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Saved Successfully");
                    }

                    else if (cboType.Text == "Promotion with Adjustment")
                    {
                        //NewId
                        sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                        NewId = clsCon.GTRCountingData(sqlQuery);

                        //Insert Data
                        sqlQuery = "INSERT into dbo.tblEmp_Incr(IncId, IncType, EmpId, dtInc, Amount, Percentage, OldSal, NewSal, OldDesigId, NewDesigId, BS,HR,OldSectId, NewSectId,OldStatus, NewStatus,IsInactive, ComId, LUserId, PCName)"
                                   + " Values (" + NewId + ", '" + cboType.Text + "','" + this.cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "','" + txtIncAmount.Text + "','" + txtPer.Text + "', '" + txtPrevSalary.Text + "','" +
                                   txtPrevSalary.Text.ToString() + "', " + cboPrevDesig.Value.ToString() + ",'" + cboNewDesig.Value.ToString() + "', '" + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + "','" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) + "','" + cboSection.Value.ToString() + "', '" + cboNewSection.Value.ToString() + "', '" +
                                   (cboPrevStatus.Text.ToString()) + "','" + (cboNewStatus.Text.ToString()) + "', '0', " +
                                   Common.Classes.clsMain.intComId + ", " + Common.Classes.clsMain.intUserId +
                                   ",'" + Common.Classes.clsMain.strComputerName + "')";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        // Update tblemp_Info Desig
                        sqlQuery = "Update tblemp_Info Set  DesigID = '" + cboNewDesig.Value.ToString() + "',SectID = '" + cboNewSection.Value.ToString() + "',EmpType ='" + (cboNewStatus.Text.ToString()) + "'  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        ////Update Bepza & Regency Grade
                        //sqlQuery = "Update E set E.BepzaGrade = D.Grade, E.OfficeGrade = D.OffGrade from tblEmp_Info E,tblCat_Desig D Where E.DesigID = D.DesigId and E.EmpId = '" + this.cboEmpID.Value.ToString() + "'";
                        //arQuery.Add(sqlQuery);

                        //Update Gross,BS,HR,MA Salary
                        sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                   + "  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);


                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Saved Successfully");
                    }

                    else if (cboType.Text == "Confirmation")
                    {

                        if (txtAmt.Text == "0")
                        {
                            //NewId
                            sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                            NewId = clsCon.GTRCountingData(sqlQuery);

                            //Insert Data
                            sqlQuery = "INSERT into dbo.tblEmp_Incr(IncId, IncType, EmpId, dtInc, Amount, Percentage, OldSal, NewSal, OldDesigId, NewDesigId, BS,HR,OldSectId, NewSectId,OldStatus, NewStatus,IsInactive, ComId, LUserId, PCName)"
                                       + " Values (" + NewId + ", '" + cboType.Text + "','" + this.cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "','" + txtIncAmount.Text + "','" + txtPer.Text + "', '" + txtPrevSalary.Text + "','" +
                                       txtPrevSalary.Text.ToString() + "', " + cboPrevDesig.Value.ToString() + ",'" + cboPrevDesig.Value.ToString() + "', '" + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + "','" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) + "','" + cboSection.Value.ToString() + "', '" + cboSection.Value.ToString() + "', '" +
                                       (cboPrevStatus.Text.ToString()) + "','" + (cboPrevStatus.Text.ToString()) + "', '0', " +
                                       Common.Classes.clsMain.intComId + ", " + Common.Classes.clsMain.intUserId +
                                       ",'" + Common.Classes.clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                            arQuery.Add(sqlQuery);

                            // Update GS in tblemp_Info
                            sqlQuery = "Update tblemp_Info Set  IsConfirm = '1', dtConfirm = '" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                            arQuery.Add(sqlQuery);


                            //Transaction with database
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);

                            MessageBox.Show("Data Saved Successfully");
                        }

                        else
                        {
                            //NewId
                            sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                            NewId = clsCon.GTRCountingData(sqlQuery);

                            //Insert Data
                            sqlQuery = "INSERT into dbo.tblEmp_Incr(IncId, IncType, EmpId, dtInc, Amount, Percentage, OldSal, NewSal, OldDesigId, NewDesigId, BS,HR, OldSectId, NewSectId,OldStatus, NewStatus,IsInactive, ComId, LUserId, PCName)"
                                       + " Values (" + NewId + ", '" + cboType.Text + "','" + this.cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "','" + txtIncAmount.Text + "','" + txtPer.Text + "', '" + txtPrevSalary.Text + "','" +
                                       txtNewSal.Text.ToString() + "', " + cboPrevDesig.Value.ToString() + ",'" + cboPrevDesig.Value.ToString() + "', '" + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + "','" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) + "', '" + cboSection.Value.ToString() + "', '" + cboSection.Value.ToString() + "', '" +
                                       (cboPrevStatus.Text.ToString()) + "','" + (cboPrevStatus.Text.ToString()) + "', '0', " +
                                       Common.Classes.clsMain.intComId + ", " + Common.Classes.clsMain.intUserId +
                                       ",'" + Common.Classes.clsMain.strComputerName + "')";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                            arQuery.Add(sqlQuery);


                            // Update  tblemp_Info - Gross,BS,HR,MA Salary
                            sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                                                   + ", IsConfirm = '1', dtConfirm = '" + clsProc.GTRDate(dtIncDate.Value.ToString())
                                                                   + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                            arQuery.Add(sqlQuery);

                            // Insert Information To Log File
                            sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                       + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                       "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                            arQuery.Add(sqlQuery);


                            //Increment Permission
                            sqlQuery = "prcProcessEmpApproval " + Common.Classes.clsMain.intComId + "," + NewId + ",'Increment',1";
                            arQuery.Add(sqlQuery);


                            //Transaction with database
                            clsCon.GTRSaveDataWithSQLCommand(arQuery);

                            MessageBox.Show("Data Saved Successfully");

                        }

                    }

                    else
                    {
                        //NewId
                        sqlQuery = "Select Isnull(Max(IncId),0)+1 As NewId from tblEmp_Incr";
                        NewId = clsCon.GTRCountingData(sqlQuery);

                        //Insert Data
                        sqlQuery = "INSERT into dbo.tblEmp_Incr(IncId, IncType, EmpId, dtInc, Amount, Percentage, OldSal, NewSal, OldDesigId, NewDesigId, BS,HR,OldSectId, NewSectId,OldStatus, NewStatus,IsInactive, ComId, LUserId, PCName)"
                                   + " Values (" + NewId + ", '" + cboType.Text + "','" + this.cboEmpID.Value.ToString() + "','" + clsProc.GTRDate(dtIncDate.Value.ToString()) + "','" + txtIncAmount.Text + "','" + txtPer.Text + "', '" + txtPrevSalary.Text + "','" +
                                   txtNewSal.Text.ToString() + "', " + cboPrevDesig.Value.ToString() + ",'" + cboPrevDesig.Value.ToString() + "', '" + clsProc.GTRValidateDouble(txtBS.Text.ToString()) + "','" + clsProc.GTRValidateDouble(txtOTA.Value.ToString()) + "','" + cboSection.Value.ToString() 
                                   + "', '" + cboSection.Value.ToString() + "', '" +
                                   (cboPrevStatus.Text.ToString()) + "','" + (cboPrevStatus.Text.ToString()) + "', '0', " +
                                   Common.Classes.clsMain.intComId + ", " + Common.Classes.clsMain.intUserId +
                                   ",'" + Common.Classes.clsMain.strComputerName + "')";
                        arQuery.Add(sqlQuery);


                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        //Update Gross,BS,HR,MA Salary
                        sqlQuery = "Update tblemp_Info Set  TS = '" + txtNewSal.Text.ToString() + "', GS = '" + txtNewSal.Text.ToString()
                                   + "',BS = " + clsProc.GTRValidateDouble(txtBS.Text.ToString())
                                   + ", OtherAllow =" + clsProc.GTRValidateDouble(txtOTA.Value.ToString())
                                   + ", dtIncrement = '" + clsProc.GTRDate(dtIncDate.Value.ToString())
                                   + "' WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        //Increment Permission
                        sqlQuery = "prcProcessEmpApproval " + Common.Classes.clsMain.intComId + "," + NewId + ",'Increment',1";
                        arQuery.Add(sqlQuery);


                        //Transaction with database
                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Saved Successfully");
                    }
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
            prcLoadList();
            prcLoadCombo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to delete Increament information of [" + txtName.Text.ToString() + "]", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                if (cboType.Text == "Promotion")
                {
                    //Delete Data                
                sqlQuery = "Delete from tblEmp_Incr Where incID = " + Int32.Parse(txtID.Value.ToString());
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                }

                else
                {
                    //Delete Data
                    sqlQuery = "Update tblemp_Info Set  TS = '" + txtPrevSalary.Text + "', GS = '" + txtPrevSalary.Text + "'  WHERE empid = '" + this.cboEmpID.Value.ToString() + "'";
                    arQuery.Add(sqlQuery);

                    sqlQuery = "Delete from tblEmp_Incr Where incID = " + Int32.Parse(txtID.Value.ToString());
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                }

                prcClearData();
                prcLoadList();
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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridList.ActiveRow.Cells["incid"].Value.ToString());

                if (txtID.Text != "")
                {
                    tabEmployee.Tabs["Entry"].Selected = true;
                    //prcCalucalateTotal();

                    txtAmt.Focus();

                    //gridTest.Rows[0].Cells["TypeID"].Activate();
                    //gridTest.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Boolean fncBlank()
        {
            //General Information
            tabEmployee.Tabs[0].Selected = true;

            if (this.cboType.Text.Length == 0)
            {
                MessageBox.Show("Please provide Increment Type");
                cboType.Focus();
                return true;
            }
            ///
            //if (cboType.Text != "Increment")
            //{
            //    if (this.cboNewGrade.Text.Length == 0)
            //    {
            //        MessageBox.Show("Please provide Grade");
            //        cboNewGrade.Focus();
            //        return true;
            //    }
            //}
            if (this.cboType.Text.Length == 0)
            {
                MessageBox.Show("Please provide religion");
                cboType.Focus();
                return true;
            }
            if (this.txtAmt.Text.Length == 0)
            {
                MessageBox.Show("Please provide Increment Amount");
                txtAmt.Focus();
                return true;
            }

            if (this.txtPer.Text.Length == 0)
            {
                MessageBox.Show("Please provide Increment Percentage");
                txtPer.Focus();
                return true;
            }

            return false;
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridList.DisplayLayout.Bands[0].Columns["aid"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["aincid"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["IncID"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["empID"].Hidden = true;


                gridList.DisplayLayout.Bands[0].Columns["empCode"].Width = 70;
                gridList.DisplayLayout.Bands[0].Columns["empName"].Width = 150;

                //gridList.DisplayLayout.Bands[0].Columns["empNameCode"].Width = 70;



                gridList.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Code"; //empCode
                gridList.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Emp. Name";
                gridList.DisplayLayout.Bands[0].Columns["IncType"].Header.Caption = "Increment Type";
                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["dtInc"].Header.Caption = "Date";
                gridList.DisplayLayout.Bands[0].Columns["OldEmpType"].Header.Caption = "Old Emp Type";
                gridList.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "New Emp Type";
                gridList.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Increment Amount";
                gridList.DisplayLayout.Bands[0].Columns["OldSal"].Header.Caption = "Prev Salary";
                gridList.DisplayLayout.Bands[0].Columns["NewSal"].Header.Caption = "New Salary";
                gridList.DisplayLayout.Bands[0].Columns["BS"].Header.Caption = "Basic";
                gridList.DisplayLayout.Bands[0].Columns["HR"].Header.Caption = "House Rent";
                gridList.DisplayLayout.Bands[0].Columns["MA"].Header.Caption = "Medical Allow";


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
                //this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtCodeTran_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtCodeFigure_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }




        private void prcFilterSection(ref UltraCombo cbo)
        {
            cbo.DataSource = null;
            //if (cboDepartment.Value == null)
            //    return;

            DataView dv = new DataView();
            try
            {
                dv = dvSection;
                dv.RowFilter = "";
                dv.RowFilter = "DeptId = '" + clsProc.GTRValidateDouble(cboEmpID.Value.ToString()).ToString() + "'";
                cbo.DataSource = dv;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        private void cboReligion_Validated(object sender, EventArgs e)
        {
            if (cboType.Text.Length > 0)
            {
                if (cboType.IsItemInList() == false)
                {
                    MessageBox.Show("Press Select From The List");
                    cboType.Focus();
                    return;
                }
            }
        }

    

        private void cboFilterFName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void chkInactive_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }


        private void cboEmpID_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewDesig_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtAmt_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtPer_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewGrade_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewSalSec_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewStatus_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboNewSection_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void cboEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewDesig_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void txtAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void txtPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewGrade_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewSalSec_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboNewSection_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short) e.KeyChar);
        }

        private void cboEmpID_Enter(object sender, EventArgs e)
        {

        }

        private void txtAmt_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtAmt);
        }

        private void txtPer_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtAmt);
        }

        private void txtAmt_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtAmt);
        }

        private void txtPer_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtPer);
        }

        private void cboEmpID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            cboEmpID.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 90;
            cboEmpID.DisplayLayout.Bands[0].Columns["empName"].Width = 180;
            cboEmpID.DisplayLayout.Bands[0].Columns["sectid"].Hidden = true;


            cboEmpID.DisplayLayout.Bands[0].Columns["empID"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["SectName"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["DesigName"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["TS"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["GS"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["BS"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["HR"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["MA"].Hidden = true;

            cboEmpID.DisplayLayout.Bands[0].Columns["Grade"].Hidden = true;
            cboEmpID.DisplayLayout.Bands[0].Columns["EmpType"].Hidden = true;


            //cboEmpID.DisplayLayout.Bands[0].Columns["EmpCode"].Width = cboEmpID.Width;

            cboEmpID.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp. Code";
            cboEmpID.DisplayLayout.Bands[0].Columns["empName"].Header.Caption = "Employee Name";

            //cboEmpID.DisplayMember = "empcode";
            //cboEmpID.ValueMember = "empid";
            //cboEmpID.Tag = "vAccountCode";
        }

        private void cboFilterFName_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void cboEmpID_RowSelected(object sender, RowSelectedEventArgs e)
        {

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();
            
            try
            {
                if (cboEmpID.Value != null)
                {


                    string sqlQuery1 = "", sqlQuery2 = "";
                    Int64 ActiveSalary = 0;

                    //Salary Permission Code
                    sqlQuery1 = "Exec prcPermission_SalaryUser " + Common.Classes.clsMain.intComId + " ," + GTRHRIS.Common.Classes.clsMain.intUserId + ", " + cboEmpID.Value.ToString() + " ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery1);

                    sqlQuery2 = "Select dbo.fncCheckEmpSalary (" + Common.Classes.clsMain.intComId + ", " + GTRHRIS.Common.Classes.clsMain.intUserId + ")";
                    ActiveSalary = clsCon.GTRCountingDataLarge(sqlQuery2);


                    if (ActiveSalary == 1)
                    {

                        txtName.Text = cboEmpID.ActiveRow.Cells["empName"].Value.ToString();
                        txtDesignation.Text = cboEmpID.ActiveRow.Cells["DesigName"].Value.ToString();
                        cboSection.Value = cboEmpID.ActiveRow.Cells["sectid"].Value.ToString();
                        txtsect.Text = cboEmpID.ActiveRow.Cells["sectName"].Value.ToString();
                        txtPrevSalary.Text = cboEmpID.ActiveRow.Cells["Gs"].Value.ToString();
                        txtPrevGS.Text = cboEmpID.ActiveRow.Cells["GS"].Value.ToString();
                        txtPrevBS.Text = cboEmpID.ActiveRow.Cells["BS"].Value.ToString();
                        txtPrevOTA.Text = cboEmpID.ActiveRow.Cells["HR"].Value.ToString();
                        cboPrevDesig.Text = cboEmpID.ActiveRow.Cells["DesigName"].Value.ToString();
                        cboPrevStatus.Text = cboEmpID.ActiveRow.Cells["EmpType"].Value.ToString();

                        cboNewSection.Text = cboEmpID.ActiveRow.Cells["sectid"].Value.ToString();
                        cboNewDesig.Text = cboEmpID.ActiveRow.Cells["DesigName"].Value.ToString();
                        cboNewStatus.Text = cboEmpID.ActiveRow.Cells["EmpType"].Value.ToString();



                        string sqlQuery = "Exec [prcGetIncrementMng] " + Common.Classes.clsMain.intComId + ", '" + cboEmpID.Value.ToString() + "','" + cboEmpID.Value.ToString() + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                        dsList.Tables[0].TableName = "tblIncList";

                        dvGrid = dsList.Tables["tblIncList"].DefaultView;
                        gridInc.DataSource = null;
                        gridInc.DataSource = dvGrid;

                    }

                    else
                    {
                        MessageBox.Show("You have no permission to access this Employee ID.Please communicate with Administrator.");
                        return;
                    }
                                  
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   throw;
            }

            finally
            {
                clsCon = null;
            }
        }

        private void cboType_TextChanged(object sender, EventArgs e)
        {

            if (cboType.Text == "Increment")
            {
                //cboNewDesig.Value = cboPrevDesig.Value;
                //cboNewStatus.Value = cboPrevStatus.Value;
                //cboNewSection.Value = cboSection.Value;
                txtAmt.Enabled = true;
                cboNewDesig.Enabled = false;
                cboNewStatus.Enabled = false;
                cboNewSection.Enabled = false;

            }
            else if (cboType.Text == "Promotion")
            {
                
                cboNewDesig.Enabled = true;
                cboNewStatus.Enabled = true;
                cboNewSection.Enabled = true;

                txtPer.Text = "0";
                txtNewSal.Text = "0";
                txtAmt.Text = "0";
                txtBS.Text = "0";
                txtOTA.Text = "0";

                txtPer.Enabled = false;
                txtNewSal.Enabled = false;
                txtAmt.Enabled = false;
                txtBS.Enabled = false;
                txtOTA.Enabled = false;

            }
            else if (cboType.Text == "Increment with Promotion")
            {
                
                txtPer.Enabled = true;
                txtNewSal.Enabled = true;
                txtAmt.Enabled = true;
                txtBS.Enabled = true;
                txtOTA.Enabled = true;


                cboNewDesig.Enabled = true;
                cboNewStatus.Enabled = true;
                cboNewSection.Enabled = true;


            }

            else if (cboType.Text == "Adjustment")
            {
                //cboNewDesig.Value = cboPrevDesig.Value;
                //cboNewStatus.Value = cboPrevStatus.Value;
                //cboNewSection.Value = cboSection.Value;
                txtAmt.Enabled = true;
                cboNewDesig.Enabled = false;
                cboNewStatus.Enabled = false;
                cboNewSection.Enabled = false;

            }

            else if (cboType.Text == "Revised")
            {
                //cboNewDesig.Value = cboPrevDesig.Value;
                //cboNewStatus.Value = cboPrevStatus.Value;
                //cboNewSection.Value = cboSection.Value;
                txtAmt.Enabled = true;
                cboNewDesig.Enabled = false;
                cboNewStatus.Enabled = false;
                cboNewSection.Enabled = false;

            }

            else if (cboType.Text == "Confirmation")
            {
                //cboNewDesig.Value = cboPrevDesig.Value;
                //cboNewStatus.Value = cboPrevStatus.Value;
                //cboNewSection.Value = cboSection.Value;
                txtAmt.Enabled = true;
                cboNewDesig.Enabled = false;
                cboNewStatus.Enabled = false;
                cboNewSection.Enabled = false;

            }

            //else
            //{

            //    MessageBox.Show("Please Select The Type From The List");
            //    cboType.Focus();

            //}

        }

        private void txtNewSal_ValueChanged(object sender, EventArgs e)
        {
        }

        private void txtAmt_ValueChanged(object sender, EventArgs e)
        {
            if (cboType.Text == "Increment")
            {
                if (txtAmt.Text.Length == 0)
                {
                    txtNewSal.Value = 0;
                    txtAmt.Value = 0;
                    txtBS.Value = 0;
                    txtOTA.Value = 0;
                    txtPer.Value = 0;
                    txtIncAmount.Value = 0;
                }

                if (double.Parse(txtAmt.Value.ToString()) > 0)
                {
                    
                    txtNewSal.Value = double.Parse(txtPrevSalary.Value.ToString()) + double.Parse(txtAmt.Value.ToString());
                    txtBS.Value = Math.Round((double.Parse(txtNewSal.Value.ToString()) * 60) / 100);
                    txtOTA.Value = Math.Round((double.Parse(txtNewSal.Value.ToString())) - (double.Parse(txtBS.Value.ToString())));
                    
                    Int64 Amount;
                    Amount = Convert.ToInt64(double.Parse(txtNewSal.Value.ToString()) - double.Parse(txtPrevSalary.Value.ToString()));

                    txtIncAmount.Value = Amount;

                    txtPer.Value = ((double.Parse(txtIncAmount.Value.ToString())) * 100 / (double.Parse(txtPrevSalary.Value.ToString())));


                }
            }

            else if (cboType.Text == "Increment with Promotion")
            {
                if (txtAmt.Text.Length == 0)
                {
                    txtNewSal.Value = 0;
                    txtAmt.Value = 0;
                    txtBS.Value = 0;
                    txtOTA.Value = 0;
                    txtPer.Value = 0;
                    txtIncAmount.Value = 0;
                }

                if (double.Parse(txtAmt.Value.ToString()) > 0)
                {

                    txtNewSal.Value = double.Parse(txtPrevSalary.Value.ToString()) + double.Parse(txtAmt.Value.ToString());

                    txtBS.Value = Math.Round((double.Parse(txtNewSal.Value.ToString()) * 60) / 100);
                    txtOTA.Value = Math.Round((double.Parse(txtNewSal.Value.ToString())) - (double.Parse(txtBS.Value.ToString())));

                    txtIncAmount.Value = double.Parse(txtAmt.Value.ToString());

                    txtPer.Value = ((double.Parse(txtIncAmount.Value.ToString())) * 100 / (double.Parse(txtPrevSalary.Value.ToString())));

                }
            }

            else if (cboType.Text == "Adjustment")
            {
                if (txtAmt.Text.Length == 0)
                {
                    txtNewSal.Value = 0;
                    txtAmt.Value = 0;
                    txtBS.Value = 0;
                    txtOTA.Value = 0;
                    txtPer.Value = 0;
                    txtIncAmount.Value = 0;
                }

                if (double.Parse(txtAmt.Value.ToString()) > 0)
                {

                    txtNewSal.Value = double.Parse(txtPrevSalary.Value.ToString()) + double.Parse(txtAmt.Value.ToString());

                    txtBS.Value = Math.Round((double.Parse(txtNewSal.Value.ToString()) * 60) / 100);
                    txtOTA.Value = Math.Round((double.Parse(txtNewSal.Value.ToString())) - (double.Parse(txtBS.Value.ToString())));

                    txtIncAmount.Value = double.Parse(txtAmt.Value.ToString());

                    txtPer.Value = ((double.Parse(txtIncAmount.Value.ToString())) * 100 / (double.Parse(txtPrevSalary.Value.ToString())));

                }
            }

            else if (cboType.Text == "Revised")
            {
                if (txtAmt.Text.Length == 0)
                {
                    txtNewSal.Value = 0;
                    txtAmt.Value = 0;
                    txtBS.Value = 0;
                    txtOTA.Value = 0;
                    txtPer.Value = 0;
                    txtIncAmount.Value = 0;
                }

                if (double.Parse(txtAmt.Value.ToString()) > 0)
                {

                    txtNewSal.Value = double.Parse(txtPrevSalary.Value.ToString()) + double.Parse(txtAmt.Value.ToString());

                    txtBS.Value = Math.Round((double.Parse(txtNewSal.Value.ToString()) * 60) / 100);
                    txtOTA.Value = Math.Round((double.Parse(txtNewSal.Value.ToString())) - (double.Parse(txtBS.Value.ToString())));

                    txtIncAmount.Value = double.Parse(txtAmt.Value.ToString());

                    txtPer.Value = ((double.Parse(txtIncAmount.Value.ToString())) * 100 / (double.Parse(txtPrevSalary.Value.ToString())));

                }
            }

            else if (cboType.Text == "Confirmation")
            {
                if (txtAmt.Text.Length == 0)
                {
                    txtNewSal.Value = 0;
                    txtAmt.Value = 0;
                    txtBS.Value = 0;
                    txtOTA.Value = 0;
                    txtPer.Value = 0;
                    txtIncAmount.Value = 0;
                }

                if (double.Parse(txtAmt.Value.ToString()) > 0)
                {

                    txtNewSal.Value = double.Parse(txtPrevSalary.Value.ToString()) + double.Parse(txtAmt.Value.ToString());

                    txtBS.Value = Math.Round((double.Parse(txtNewSal.Value.ToString()) * 60) / 100);
                    txtOTA.Value = Math.Round((double.Parse(txtNewSal.Value.ToString())) - (double.Parse(txtBS.Value.ToString())));

                    txtIncAmount.Value = double.Parse(txtAmt.Value.ToString());

                    txtPer.Value = ((double.Parse(txtIncAmount.Value.ToString())) * 100 / (double.Parse(txtPrevSalary.Value.ToString())));

                }
            }

        }

        private void txtPrevSalary_Leave(object sender, EventArgs e)
        {
            if(txtPrevSalary.Text.Length == 0)
            {
                txtAmt.Value = "";
            }
        }

        private void cboEmpID_ValueChanged(object sender, EventArgs e)
        {
            txtNewSal.Value = 0;
            txtAmt.Value = 0;
            txtBS.Value = 0;
            txtOTA.Value = 0;
            txtPer.Value = 0;

        }

        private void cboPrevSalSec_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboSection_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboPrevStatus_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboNewStatus_ValueChanged(object sender, EventArgs e)
        {

        }


        private void txtPer_ValueChanged(object sender, EventArgs e)
        {            
            
            //if (txtPer.Text.Length == 0)
            //{
            //    txtNewSal.Value = 0;
            //    txtAmt.Value = 0;
            //    txtBS.Value = 0;
            //    txtHR.Value = 0;
            //    txtMA.Value = 0;
            //    txtPer.Value = 0;
            //}

            //if (double.Parse(txtPer.Value.ToString()) > 0)
            //{
                //txtAmt.Value = ((double.Parse(txtPer.Value.ToString()))  / (double.Parse(txtPrevSalary.Value.ToString())));
                //txtNewSal.Value = (double.Parse(txtPrevSalary.Value.ToString()) + (double.Parse(txtAmt.Value.ToString())));
                //txtBS.Value = Math.Round((double.Parse(txtNewSal.Value.ToString()) - 200) / 1.4);
                //txtHR.Value = (double.Parse(txtNewSal.Value.ToString()) - ((double.Parse(txtBS.Value.ToString())) + 200));
                //txtMA.Value = 200;
            }

        private void gridInc_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridInc.DisplayLayout.Bands[0].Columns["aincid"].Hidden = true;
                gridInc.DisplayLayout.Bands[0].Columns["IncID"].Hidden = true;
                gridInc.DisplayLayout.Bands[0].Columns["empID"].Hidden = true;
                gridInc.DisplayLayout.Bands[0].Columns["empCode"].Hidden = true;

                gridInc.DisplayLayout.Bands[0].Columns["IncType"].Width = 150;
                gridInc.DisplayLayout.Bands[0].Columns["dtInc"].Width = 120;
                gridInc.DisplayLayout.Bands[0].Columns["Amount"].Width = 90;

                gridInc.DisplayLayout.Bands[0].Columns["IncType"].Header.Caption = "Increment Type";
                gridInc.DisplayLayout.Bands[0].Columns["dtInc"].Header.Caption = "Date";
                gridInc.DisplayLayout.Bands[0].Columns["Amount"].Header.Caption = "Amount";


                //Change alternate color
                gridInc.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridInc.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridInc.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridInc.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                //this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
                //e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridInc_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                prcClearData();
                prcDisplayDetails(gridInc.ActiveRow.Cells["incid"].Value.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }






        }

    }
