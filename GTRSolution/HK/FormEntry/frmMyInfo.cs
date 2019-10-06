using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmMyInfo : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmMyInfo(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMyInfo_FormClosing(object sender, FormClosingEventArgs e)
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

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                String SqlQuery = "select EmpId from tblLogin_User where LUserId = "+Common.Classes.clsMain.intUserId+" ";
                Int32 Id = 604; //clsCon.GTRCountingData(SqlQuery);
                
                SqlQuery = "Exec prcGetNotification " + Common.Classes.clsMain.intComId + ", " + Id + ", '" + clsProc.GTRDate(System.DateTime.Today.Date.ToString())+ "' ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "Attendent";


                gridAtt.DataSource = null;
                gridAtt.DataSource = dsList.Tables["Attendent"];
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
         }
        public void prcDisplayDetails(string strParam)
        {
        }

        public void prcClearData()
        {
        }

        private void frmMyInfo_Load(object sender, EventArgs e)
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

        private void gridAtt_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridAtt.DisplayLayout.Bands[0].Columns["dtPunchDate"].Header.Caption = "Punch Date";
            gridAtt.DisplayLayout.Bands[0].Columns["inTime"].Header.Caption = "In Time";
            gridAtt.DisplayLayout.Bands[0].Columns["outTime"].Header.Caption = "Out Time";
            gridAtt.DisplayLayout.Bands[0].Columns["Status"].Header.Caption = "Sts";
            gridAtt.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

            gridAtt.DisplayLayout.Bands[0].Columns["dtPunchDate"].Width = 70;
            gridAtt.DisplayLayout.Bands[0].Columns["inTime"].Width = 70;
            gridAtt.DisplayLayout.Bands[0].Columns["outTime"].Width = 70;
            gridAtt.DisplayLayout.Bands[0].Columns["Status"].Width = 30;
            gridAtt.DisplayLayout.Bands[0].Columns["Remarks"].Width = 100;

            
            //Change alternate color
            gridAtt.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridAtt.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridAtt.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridAtt.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridAtt.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }
    }
}
