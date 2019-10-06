using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using GTRLibrary;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Microsoft.Reporting.WinForms;

namespace GTRHRIS.Common.FormEntry
{
    public partial class frmrptViewer : Form
    {
        //Variable For Main Report
        private string strMainRP = "";
        private string strMainDSN = "";
        private string strMainQuery = "";

        //Variabl For Sub Report
        private string strRFN = "";
        private string strDSN = "";
        private string strQuery = "";
        private DataSet dsReport;

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptViewer(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        public void prcDisplayReport()
        {
            try
            {
                dsReport = new DataSet();
                dsReport = clsReport.dsReport;

                // For Display Main Report
                strMainRP = clsReport.strReportPathMain;
                strMainDSN = clsReport.strDSNMain;
                strMainQuery =clsReport.strQueryMain;

                //Processing Report
                prcProcessReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dsReport = null;
            }
        }

        private void prcProcessReport()
        {
            try
            {
                //Clear Data Source
                this.rptViewer.LocalReport.DataSources.Clear();

                //reset the report viewer
                 this.rptViewer.Reset();
                
                //Refresh Report
                this.rptViewer.Refresh();

                //set processing to local mode
                this.rptViewer.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
                
                //set external Image (we will use external image like C:\abc.gif) 
                this.rptViewer.LocalReport.EnableExternalImages = true;
                
                //load .rdlc file and add a datasource
                this.rptViewer.LocalReport.ReportPath = strMainRP;
                
                //Generate a event to process sub-report at the time of loading main report
                rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(prcProcessSubReport);
        
                //Retrieve Data
                this.rptViewer.LocalReport.DataSources.Add(this.GetData());

                //Refresh Report
                this.rptViewer.RefreshReport();
                
                this.rptViewer.SetDisplayMode(DisplayMode.PrintLayout);
                //this.rptViewer.SetDisplayMode(DisplayMode.Normal);
                this.rptViewer.ZoomMode = ZoomMode.PageWidth;

                //Convert to PDF Format
                prcExportReport();

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        private void prcExportReport()
        {
            try
            {
                Microsoft.Reporting.WinForms.Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = null;
                string encoding = null;
                string extension = null;
                string strrptPath = @"C:\gtReports\";
                string strFileName = clsMain.strReportName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + clsMain.strExtension;

                if (!Directory.Exists(strrptPath))
                {
                    Directory.CreateDirectory(strrptPath);
                }

                Byte[] bytes;
                // bytes = rptViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                // bytes = rptViewer.LocalReport.Render("excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                bytes = rptViewer.LocalReport.Render(clsMain.strFormat, null, out mimeType, out encoding, out extension, out streamids, out warnings);

                FileStream fs = new FileStream(strrptPath + strFileName, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                System.Diagnostics.Process.Start((strrptPath + strFileName));
                clsMain.strReportName = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmrptViewer_Load(object sender, EventArgs e)
        {
            try
            {
                //To Processing Report
                prcDisplayReport();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Retrieve Data For Main Report
        private Microsoft.Reporting.WinForms.ReportDataSource GetData()
        {
            ////System.Data.DataSet 
            //System.Data.DataSet ds = new System.Data.DataSet();
            
            //clsConnection clsCon = new clsConnection();
            //clsCon.GTRFillDatasetWithSQLCommand(ref ds, strMainQuery);
            //clsCon = null;

            return new Microsoft.Reporting.WinForms.ReportDataSource(strMainDSN, dsReport.Tables[0]);
        }

        //Processing Sub-Reporting
        private void prcProcessSubReport(object sender, SubreportProcessingEventArgs e)
        {
            //Declare a data table
            DataTable dtSub = new DataTable();
            string sqlQuery = "", param="";

            prcGetSubReportDetails(e.ReportPath);
            param = strRFN.Length == 0 ? "" : e.Parameters[strRFN].Values[0].ToString();
            sqlQuery = strQuery + " " + param;

            //Ready a datatable for report based on parameter data
            dtSub = prcGetDataSub(sqlQuery);

            //Processing sub report data
            e.DataSources.Add(new ReportDataSource(strDSN, dtSub));
        }

        //Retrieve Data For Sub Report
        private DataTable prcGetDataSub(string strQuery)
        {
            //System.Data.DataSet 
            System.Data.DataSet ds = new System.Data.DataSet();
            clsConnection clsCon = new clsConnection();
            try
            {
                //SQL Query (Here i use Store procedure)
                clsCon.GTRFillDatasetWithSQLCommand(ref ds, strQuery);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }
            return ds.Tables[0];
        }

        private void rptViewer_Load(object sender, EventArgs e)
        {
        }

        private void frmrptViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // It will use to close Tab from frmMaster
            try
            {
                //int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
                //uTab.Tabs.RemoveAt(index);
                //Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.Reset();
                //uTab = null;
                //FM = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcGetSubReportDetails(string rptPath)
        {
            foreach (var lst in clsReport.rptList)
            {
                if (lst.strRptPathSub.ToUpper() == rptPath.ToUpper())
                {
                    strDSN = lst.strDSNSub;
                    strQuery = lst.strQuerySub;
                    strRFN = lst.strRFNSub;
                }
            }
        }

        private void frmrptViewer_Resize(object sender, EventArgs e)
        {
            // Set window state to maximize
            //this.WindowState = FormWindowState.Maximized;
        }
    }
}
