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

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmEmpDataInput : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private System.Data.DataView dvSection;
        private DataView dvGrid;

        private clsMain clsM = new clsMain();
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmpDataInput(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmEmpDataInput_FormClosing(object sender, FormClosingEventArgs e)
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


        private void btnSalProcessFull_Click(object sender, EventArgs e)
        {

            btnSalProcessFull.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {

                string sqlQuery = "Exec prcProcessEmpInput " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "'";
                arQuery.Add(sqlQuery);
          
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
              

                MessageBox.Show("New Employee Data Insert Complete");
                btnSalProcessFull.Text = "Transfer";
             
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

        private void btnProcess_Click(object sender, EventArgs e)
        {

            btnProcess.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {

                string sqlQuery = "Exec prcProcessMaindataInput " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "',0";
                arQuery.Add(sqlQuery);

                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("Attendance Data Insert Complete");
                btnProcess.Text = "Process Data Input";

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

        private void btnSalary_Click(object sender, EventArgs e)
        {
            btnSalary.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {

                string sqlQuery = "Exec prcProcessMaindataInput " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "',1";
                arQuery.Add(sqlQuery);

                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("Salary Data Insert Complete");
                btnSalary.Text = "Salary Data Input";

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



    }
}