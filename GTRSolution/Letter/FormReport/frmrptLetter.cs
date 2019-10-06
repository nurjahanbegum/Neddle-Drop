using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Letter.FormReport
{
    public partial class frmrptLetter : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptLetter(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlquary = "Exec prcrptLetterAll " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                dsList.Tables[0].TableName = "Criteria";
                dsList.Tables[1].TableName = "type";
                dsList.Tables[2].TableName = "EmpType";
                dsList.Tables[3].TableName = "Section";
                dsList.Tables[4].TableName = "Employee";


                gridCriteria.DataSource = dsList.Tables["Criteria"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                gridArea.DataSource = dsList.Tables["Section"];
                gridEmp.DataSource = dsList.Tables["Employee"];
                gridIncrType.DataSource = dsList.Tables["type"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clscon = null;
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void frmrptSales_Load(object sender, EventArgs e)
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

        private void frmrptSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridArea_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridArea.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            //gridArea.DisplayLayout.Bands[0].Columns["SLNO"].Hidden = true;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Width = 205;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridArea.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridArea.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridArea.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridArea.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridArea.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridArea.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string SQLQuery="",ReportPath="", Criteria = "", Status = "", SectId = "0", type = "", Lettertype = "", EmpId = "0";

                Criteria = gridCriteria.ActiveRow.Cells["CValue"].Value.ToString();
                Lettertype = gridIncrType.ActiveRow.Cells["CValue"].Value.ToString();
                if (Criteria == "Section")
                {
                    SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                }
                type = gridEmpType.ActiveRow.Cells["EmpType"].Value.ToString();
                if (Criteria == "Employee")
                {
                    EmpId = gridEmp.ActiveRow.Cells["EmpId"].Value.ToString();
                }

                if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Appointment")
                {
                    //if (gridCriteria.ActiveRow.Cells["CValue"].Value.ToString() == "dtJoin")
                    //{
                    if (type=="Worker")
                    { 
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAppointmentWorker.rdlc";
                        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                                    clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                    }

                    else
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAppointmentStaff.rdlc";
                        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                                    clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                    }
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "AppointmentDayShift")
                {

                        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAppointmentDayShift.rdlc";
                        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                                    clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "AppointmentNightShift")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAppointmentNightShift.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                                clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                }

                //else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Increment")
                //{
                //    if (gridCriteria.ActiveRow.Cells["CValue"].Value.ToString() == "Increment")
                //    {

                //        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptIncrement.rdlc";
                //        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " +
                //                   EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                //                   clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                //                   clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                //    }

                //    else
                //    {
                //        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptIncrement.rdlc";
                //        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " +
                //                   EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                //                   clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                //                   clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                //    }

                //}

                //else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Adjustment")
                //{
                //    if (gridCriteria.ActiveRow.Cells["CValue"].Value.ToString() == "Increment")
                //    {

                //        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAdjustment.rdlc";
                //        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " +
                //                   EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                //                   clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                //                   clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                //    }

                //    else
                //    {
                //        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAdjustment.rdlc";
                //        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " +
                //                   EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                //                   clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                //                   clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                //    }

                //}

                //else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Promotion")
                //{
                //    if (type == "Worker")
                //    {

                //        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptPromotionWorker.rdlc";
                //        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " +
                //                   EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                //                   clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                //                   clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                //    }

                //    else
                //    {
                //        ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptPromotionStaff.rdlc";
                //        SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " +
                //                   EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                //                   clsProc.GTRDate(dtFrom.Value.ToString()) + "','" +
                //                   clsProc.GTRDate(dtTo.Value.ToString()) + "'";
                //    }
                //}

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Increment")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptIncrement.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "YearlyIncrement")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptYearlyIncrement.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "IncrementPromotion")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptIncPromotion.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "YearlyIncrementPromotion")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptYearlyIncPromotion.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "EvaluationForm")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptWorkerEfficiencyEvaluation.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "SalaryCertificate")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptSalaryCertificate.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "EmpTransfar")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptTransferForm.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "DailyAttReg")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptDailyAttReg.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "DoctorCheckUp")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptDoctorCheckUp.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "NomineeForm")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptNomineeForm.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "EmpIncHistory")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptEmpIncHistory.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "EmpIncStatus")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptEmpIncStatus.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "LeaveRegister")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptLeaveRegister.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "PerformanceLetter")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptPerformanceLetter.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "RegisterBook")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptRegisterBook.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "ServiceBook")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptServiceBook.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId + ",'" + Lettertype + "','" + Criteria + "','" +
                                clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                                 "'"; // "','" + type + "'";
                    clsReport.rptList.Add(new subReport("rptServiceBook2", "EmpId", "DataSet1", "Exec GTRHRIS_AMAN.dbo.rptServiceBookIncLog  "));
                    clsReport.rptList.Add(new subReport("rptServiceBook3", "EmpId", "DataSet1", "Exec GTRHRIS_AMAN.dbo.rptLeaveForm"));
                    clsReport.rptList.Add(new subReport("rptServiceBook4", "EmpId", "DataSet1", "Exec GTRHRIS_AMAN.dbo.rptServiceBookIncLog  "));

                    //ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptServiceBook.rdlc";
                    //SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                    //           ",'" + Lettertype + "','" + Criteria + "','" +
                    //           clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                    //           "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "ServiceRecivedRecord")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptServiceRecivedRecord.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "LeaveApplication")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptLeaveApplication.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "Age&MedicalCertificate")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAge&MedicalCertificate.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "AgeVerification")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAgeVerification.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "WorkerEfficinencyEvaluation")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptWorkerEfficinencyEvaluation.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "WorkerRegister")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptWorkerRegister.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "PPELetter")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptPPELetter.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }


                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "TransferLetter")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptTransferLetter.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "AggrementLetter")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptAggrementLetter.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "JobApplication")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptJobApplication.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "ResignLetter")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptResignLetter.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "JoiningApplication")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptApplicationforJoining.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }

                else if (gridIncrType.ActiveRow.Cells["CValue"].Text.ToString() == "FinalSettelment")
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Letter\Reports\rptFinelSettlement.rdlc";
                    SQLQuery = "Exec rptAppointment " + Common.Classes.clsMain.intComId + "," + SectId + ", " + EmpId +
                               ",'" + Lettertype + "','" + Criteria + "','" +
                               clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) +
                               "'";
                }


                // clsCon.GTRSaveDataWithSQLCommand(SQLQuery);

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }



                string DataSourceName = "DataSet1";
                string FormCaption = "Report :: " + Lettertype + "";

                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                clsReport.strQueryMain = SQLQuery;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);


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

        private void gridArea_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyValue);
        }

        private void gridCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Width = 150;
            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Header.Caption = "Criteria";

            //Change alternate color
            gridCriteria.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridCriteria.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridCriteria.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridCriteria.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridCriteria.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridCriteria.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            //gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Width = 240;
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Employee Type";

            //Change alternate color
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmpType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmpType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmpType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridEmpType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmp.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 245;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
            
            //Change alternate color
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmp.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            //Filtering

            this.gridEmp.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        //private void gridIncrType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    gridIncrType.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
        //    gridIncrType.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
        //    gridIncrType.DisplayLayout.Bands[0].Columns["Type"].Width = 150;
        //    gridIncrType.DisplayLayout.Bands[0].Columns["Type"].Header.Caption = "Letter Type";

        //    //Change alternate color
        //    gridIncrType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
        //    gridIncrType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

        //    //Select Full Row when click on any cell
        //    e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

        //    //Selection Style Will Be Row Selector
        //    gridIncrType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

        //    //Stop Updating
        //    gridIncrType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

        //    //Hiding +/- Indicator
        //    gridIncrType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

        //    //Hide Group Box Display
        //    e.Layout.GroupByBox.Hidden = true;

        //    //Use Filtering
        //    gridIncrType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        //}

        private void gridCriteria_AfterRowActivate(object sender, EventArgs e)
        {
            if(gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper()=="Employee Wise".ToUpper())
            {
                gridEmp.Enabled = true;
                gridArea.Enabled = false;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Section Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = true;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Joining Date Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
                group1.Visible = true;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Increment Date Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
                group1.Visible = true;
            }
            else
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
                group1.Visible = false;
            }
        }

        private void gridIncrType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridIncrType.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridIncrType.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridIncrType.DisplayLayout.Bands[0].Columns["Type"].Width = 230;
            gridIncrType.DisplayLayout.Bands[0].Columns["Type"].Header.Caption = "Letter Type";

            //Change alternate color
            gridIncrType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridIncrType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridIncrType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridIncrType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridIncrType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
        }
    }
}
