using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using GTRLibrary;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;


namespace GTRHRIS.Master
{
    public partial class frmDBBk : Form
    {
        private System.Data.DataSet dsList;
        private clsProcedure clsProc = new clsProcedure();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;
        private string F1 = "";

        public frmDBBk(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }
        private  void prcLoadList()
        {
            clsConnection clscon = new clsConnection();
            string sqlQuery;
            dsList = new System.Data.DataSet();
            try
            {
                sqlQuery = "Exec prcGetDatabaseBK ";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "DbName";
                gridDataBKUp.DataSource = null;
                gridDataBKUp.DataSource = dsList.Tables["DbName"];
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                clscon = null;
            }
        }

        private void frmDBBk_Load(object sender, EventArgs e)
        {
            try
            {
            prcLoadList();
            prcInitializeSingleGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }
        }
        private void prcInitializeSingleGrid()
        {
            DataTable dt = new DataTable("Single");
            
            dt.Columns.Add("F1", typeof(String));
            dt.Columns.Add("DR", typeof(Double));
            dt.Columns.Add("CR", typeof(Double));
            dt.Columns.Add("F2", typeof(String));
            dt.Columns.Add("TCR", typeof(Double));
            
            for (int i = 0; i < 15; i++)
                prcAddRow(ref dt);

            gridtest.DataSource = null;
            gridtest.DataSource = dt;
            
            //gridMultiple.DisplayLayout.Bands[0].Columns["AccId"].ValueList = uddAccmulti;
           // gridSingle.DisplayLayout.Bands[0].Columns["AccId"].ValueList = uddAccount;
            // gridMultiple.DisplayLayout.Bands[0].Columns["paymode"].ValueList = uddPaymode;
        }
        private void prcAddRow(ref DataTable dt)
        {
            dt.Rows.Add("", 0, 0, "",0);
        }
        private void gridDataBKUp_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                gridDataBKUp.DisplayLayout.Bands[0].Columns["dbName"].Header.Caption = "Database Name";
                gridDataBKUp.DisplayLayout.Bands[0].Columns["dbName"].Width = 200;
                gridDataBKUp.DisplayLayout.Bands[0].Columns["Mark"].Header.Caption = "Mark";
                gridDataBKUp.DisplayLayout.Bands[0].Columns["Mark"].Width = 120;
                gridDataBKUp.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
                gridDataBKUp.DisplayLayout.Bands[0].Columns["dbName"].CellActivation = Activation.NoEdit;
                gridDataBKUp.DisplayLayout.Bands[0].Columns["Mark"].Style =Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //Select Full Row when click on any cell
              //  e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                //Selection Style Will Be Row Selector
                this.gridDataBKUp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                //Stop Updating
               // this.gridDataBKUp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                //Hiding +/‐ Indicator
                this.gridDataBKUp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
                //Use Filtering
                this.gridDataBKUp.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmDBBk_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            uTab = null;
            FM = null;
            dsList = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRfrs_Click(object sender, EventArgs e)
        {
                        GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery;
         
            try
            {

                foreach (UltraGridRow row in gridDataBKUp.Rows)
                {
                    if (Int16.Parse(row.Cells["Mark"].Value.ToString()) == 1)
                    {
                        row.Appearance.BackColor = Color.Coral;
                        row.Cells["Status"].Value= "Running";
                        row.Cells["Status"].Refresh();
                        gridDataBKUp.Refresh();

                        sqlQuery = "Exec prcDBBackup '" + row.Cells["SLNo"].Value + "','','','" +Common.Classes.clsMain.intUserId + "'";
                        clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                        row.Appearance.BackColor = Color.BlanchedAlmond;
                        row.Cells["Status"].Value = "Complete";
                        row.Cells["Status"].Refresh();
                        gridDataBKUp.Refresh();
                    }
                }
                //        dbName = dbName.Substring(0, dbName.Length - 1);
               
               // MessageBox.Show("Data backup SuccessFully");
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

        private void gridtest_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridtest.DisplayLayout.Bands[0].Columns["F1"].Width = 100;
                gridtest.DisplayLayout.Bands[0].Columns["DR"].Width = 100;
                gridtest.DisplayLayout.Bands[0].Columns["CR"].Width = 100;
                gridtest.DisplayLayout.Bands[0].Columns["F2"].Width = 100;
                gridtest.DisplayLayout.Bands[0].Columns["F2"].CellActivation = Activation.Disabled;
                gridtest.DisplayLayout.Bands[0].Columns["TCR"].CellActivation = Activation.Disabled;
                //Change alternate color
                gridtest.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridtest.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;
                //Hiding +/- Indicator
                this.gridtest.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridtest_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if(e.Cell.Column.ToString().ToUpper()=="F1".ToUpper())
            {
                if((fncValidateDouble(gridtest.ActiveRow.Cells["DR"].Value.ToString())- fncValidateDouble(gridtest.ActiveRow.Cells["CR"].Value.ToString()))<=0)
                {
                    gridtest.ActiveRow.Cells["DR"].Activation = Activation.Disabled;
                    gridtest.ActiveRow.Cells["F2"].Activation = Activation.Disabled;
                    //gridtest.ActiveRow.Cells["CR"].Activated = true;
                    
                    gridtest.ActiveRow.Cells["F1"].Appearance.BackColor = Color.YellowGreen;
                    gridtest.ActiveRow.Cells["F1"].Appearance.FontData.Bold =  Infragistics.Win.DefaultableBoolean.True;
                }
                
                foreach (UltraGridRow row in gridtest.Rows)
                {
                    if(row.Cells["F2"].Text.ToUpper()==F1.ToString().ToUpper() && fncValidateDouble((gridtest.ActiveRow.Cells["CR"].Value.ToString()))>0)
                    {
                        row.Cells["F2"].Value= gridtest.ActiveRow.Cells["F1"].Value;
                    }
                }
            }
            // For Cradet Value 
            if (e.Cell.Column.ToString().ToUpper() == "CR".ToUpper())
            {
                gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["DR"].Value = gridtest.ActiveRow.Cells["CR"].Value;
                gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["TCR"].Value = gridtest.ActiveRow.Cells["CR"].Value;
                gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["F2"].Value = gridtest.ActiveRow.Cells["F1"].Value;
                gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["CR"].Activation = Activation.Disabled;
            }
            if (e.Cell.Column.ToString().ToUpper() == "DR".ToUpper())
            {
                if ((fncValidateDouble(gridtest.ActiveRow.Cells["TCR"].Value.ToString()) - fncValidateDouble(gridtest.ActiveRow.Cells["DR"].Value.ToString()))<=0)
                {
                    gridtest.Rows[gridtest.ActiveRow.Index].Cells["F1"].Activate();
                }
                else
                {
                    gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["DR"].Value = (fncValidateDouble(gridtest.ActiveRow.Cells["TCR"].Value.ToString()) - fncValidateDouble(gridtest.ActiveRow.Cells["DR"].Value.ToString()));
                    gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["TCR"].Value =gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["DR"].Value;
                    gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["F2"].Value = gridtest.ActiveRow.Cells["F2"].Value;
                    gridtest.Rows[gridtest.ActiveRow.Index + 1].Cells["CR"].Activation = Activation.Disabled;
                }
            }
        }
        private void gridtest_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
            gridtest.PerformAction(UltraGridAction.EnterEditMode);
        }
        private void gridtest_BeforeCellActivate(object sender, CancelableCellEventArgs e)
        {
            if(e.Cell.Column.ToString()=="F1")
            {
                if(fncValidateDouble(gridtest.ActiveRow.Cells["CR"].Value.ToString())>0)
                {
                  F1=gridtest.ActiveRow.Cells["F1"].Text;
                }
            }
        }
        public Double fncValidateDouble(string value)
        {
            Double dbl;
            try
            {
                dbl = Double.Parse(value);
            }
            catch (Exception)
            {
                dbl = 0;
            }
            return dbl;
        }
    }
}
