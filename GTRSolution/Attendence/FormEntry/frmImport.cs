using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using System.Data.SqlClient;
using System.Data.OleDb;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;


namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmImport : Form
    {
        private clsProcedure clsProc = new clsProcedure();
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;
        public frmImport(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            try
            {
                btnShow.Top = -100;
                prcLoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcLoadList()
        {
            dsList=new DataSet();
            clsConnection clsCon=new clsConnection();
            string sqlQuery = "";
            try
            {
                sqlQuery = "Exec prcGetUploadExcel " + clsMain.intComId ;
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "Grid";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["Grid"];
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                clsCon = null;
            }
        }

        private void frmImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text.Length == 0)
            {
                MessageBox.Show("Please select an excel file, using browse button");
                btnBrowse.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + txtFileName.Text.ToString() + "; Extended Properties='Excel 8.0; HDR=Yes; IMEX=1'";
            try
            {
                var da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", SourceConstr);
                var ds = new DataSet();
                da.Fill(ds);

                prcSaveData(ds);

                MessageBox.Show("Data uploaded successfully. [Total Rows : "+ ds.Tables[0].Rows.Count.ToString() +"]");
                btnProcess.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;    
            }
            
        }

        private void prcSaveData(DataSet ds)
        {
            clsConnection clsCon = new clsConnection();
            ArrayList arQuery = new ArrayList();

            try
            {
                // Clear Existing Data
                string query = "Truncate Table tblDN_xls";
                clsCon.GTRSaveDataWithSQLCommand(query);

                txtProcessNo.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
                string dtProcess = DateTime.Today.ToString("dd-MMM-yyyy");

                //Generating Insert Statement Row By Row
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString().Replace("'", "").Length > 0)
                    { 
                    string sqlQuery = "Insert Into tblDN_xls (ComId, xlsFileName, dtProcess, EntryNo, Consignee, BookingNo, Carrier, FVessel, ETDCGP, ETAHub, MVessel, ETDHub, ETADestination, ContainerNo, " +
                        " SealNo, Size, Type, Shipper, PO, ItemNo, ttlCartoon, ttlPCS, ttlCBM, ttlGrossWT, Mode, ttlLP, dtRcvdCGO, dtDocRcvd, dtStuffing, " +
                        " Desitnation, ShipBillNo, VAT, HBLNo, OBLNo, Remarks, PayTerms, NoOfOriginal) " +
                        " Values(" + Common.Classes.clsMain.intComId + ",'" + txtFileName.Tag.ToString() + "', '" + dtProcess + "','" + txtProcessNo.Text.ToString() + "', '" + ds.Tables[0].Rows[i][0].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][1].ToString().Replace("'", "") + "','" + ds.Tables[0].Rows[i][2].ToString().Replace("'", "") + "', " + " '" + ds.Tables[0].Rows[i][3].ToString().Replace("'", "") + "', '" + clsProc.GTRDate(ds.Tables[0].Rows[i][4].ToString().Replace("'", "")) + "', " +
                        " '" + ds.Tables[0].Rows[i][5].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][6].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][7].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][8].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][9].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][10].ToString().Replace("'", "") + "', " +
                        " '" + ds.Tables[0].Rows[i][11].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][12].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][13].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][14].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][15].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][16].ToString().Replace("'", "").Replace("NA", "0").Replace("N/A","0") + "', " +
                        " '" + ds.Tables[0].Rows[i][17].ToString().Replace("'", "").Replace("NA", "0").Replace("N/A", "0") + "', '" + ds.Tables[0].Rows[i][18].ToString().Replace("'", "").Replace("NA", "0").Replace("N/A", "0") + "', '" + ds.Tables[0].Rows[i][19].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][20].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][21].ToString().Replace("'", "").Replace("NA", "0").Replace("N/A", "0") + "', '" + ds.Tables[0].Rows[i][22].ToString().Replace("'", "") + "', " +
                        " '" + ds.Tables[0].Rows[i][23].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][24].ToString().Replace("'", "''") + "', '" + ds.Tables[0].Rows[i][25].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][26].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][27].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][28].ToString().Replace("'", "") + "', " +
                        " '" + ds.Tables[0].Rows[i][29].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][30].ToString().Replace("'", "''") + "', '" + ds.Tables[0].Rows[i][31].ToString().Replace("'", "") + "', '" + ds.Tables[0].Rows[i][32].ToString().Replace("'", "") + "')";
                    arQuery.Add(sqlQuery);
                    }
                }

                //Transaction with database server
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            String input = string.Empty;
            String input2 = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select an excel file ";
            dialog.Filter = "Excel files [97-2003] (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            //dialog.InitialDirectory = @"C:\";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
               input = dialog.FileName.ToString();
               input2 = dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1);
            }
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            txtFileName.Text = input;
            txtFileName.Tag = input2;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            clsConnection clsCon = new clsConnection();
            ArrayList arQuery = new ArrayList();
            try
            {
                string SQLQuery = "Exec prcProcessDN " + Common.Classes.clsMain.intComId + ", '" + txtFileName.Tag.ToString() + "', '" + txtProcessNo.Text.ToString() + "', " + Common.Classes.clsMain.intUserId + ", '" + Common.Classes.clsMain.strComputerName + "'";
                arQuery.Add(SQLQuery);

                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                MessageBox.Show("Data processed successfully.");

                btnProcess.Enabled = false;
                prcLoadList();
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

        //private void prcBulkCopy()
        //{
        //    if (txtFileName.Text.Length == 0)
        //    {
        //        MessageBox.Show("Please select an excel file, using browse button");
        //        btnBrowse.Focus();
        //        return;
        //    }

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + txtFileName.Text.ToString() + "; Extended Properties='Excel 8.0; HDR=No; IMEX=1'";

        //    OleDbConnection OleDbCon = new OleDbConnection(SourceConstr);
        //    clsConnection clsCon = new clsConnection();
        //    OleDbCommand OleDbCmd;

        //    try
        //    {
        //        // Clear Existing Data
        //        string query = "Truncate Table tblDN_xls";
        //        clsCon.GTRSaveDataWithSQLCommand(query);

        //        //Getting Excel Data
        //        query = "Select * from [Sheet1$]";
        //        //  -----------   Load Excel Data To DataTable
        //        //  DataTable dtExcel = new DataTable();
        //        //  dtExcel.TableName = "Sheet1";
        //        //  OleDbDataAdapter data = new OleDbDataAdapter(query, con);
        //        //  data.Fill(dtExcel);

        //        //Series of commands to bulk copy data from the excel file into our SQL table
        //        OleDbCmd = new OleDbCommand(query, OleDbCon);
        //        OleDbCon.Open();
        //        OleDbDataReader dr = OleDbCmd.ExecuteReader();

        //        //Inserting data to sql server
        //        SqlBulkCopy bulkCopy = new SqlBulkCopy(clsCon.strConnection.Replace("GTRSystem", "GTRHRIS"));
        //        bulkCopy.DestinationTableName = "tblDN_xls";
        //        bulkCopy.WriteToServer(dr);

        //        OleDbCon.Close();
        //        bulkCopy = null;
        //        dr = null;

        //        MessageBox.Show("Upload completed.");

        //        btnProcess.Enabled = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        OleDbCmd = null;
        //        OleDbCon = null;
        //        clsCon = null;
        //    }
        //}

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grida.
                gridList.DisplayLayout.Bands[0].Columns["dtProcess"].Width = 90;
                gridList.DisplayLayout.Bands[0].Columns["Consignee"].Width = 130;
                gridList.DisplayLayout.Bands[0].Columns["Carrier"].Width = 100;
                gridList.DisplayLayout.Bands[0].Columns["EntryNo"].Width = 120;
                gridList.DisplayLayout.Bands[0].Columns["xlsFileName"].Width = 400;

                gridList.DisplayLayout.Bands[0].Columns["dtProcess"].Header.Caption = "Processed Date";
                gridList.DisplayLayout.Bands[0].Columns["Consignee"].Header.Caption = "Consignee";
                gridList.DisplayLayout.Bands[0].Columns["Carrier"].Header.Caption = "Carrier";
                gridList.DisplayLayout.Bands[0].Columns["EntryNo"].Header.Caption = "Entry No";
                gridList.DisplayLayout.Bands[0].Columns["xlsFileName"].Header.Caption = "Excel File Name";

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

                //Row Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if(btnDelete.Visible)
            { 
                btnDelete.Visible = false; 
            }
            else
            {
                btnDelete.Visible = true; 
            }
            
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if(gridList.Rows.Count==0)
            {
                return;
            }
            btnDelete.Tag = gridList.ActiveRow.Cells["EntryNo"].Value.ToString();
            btnDelete.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Do you want to delete Process information of [" + btnDelete.Tag + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            clsConnection clsCon = new clsConnection();
            ArrayList arQuery = new ArrayList();
            try
            {
                string SQLQuery = "Delete From tblDN_xls_Main where EntryNo='"+btnDelete.Tag+"'";
                arQuery.Add(SQLQuery);
                // Insert Information To Log File
                SQLQuery = "Insert Into Gtrsystem.dbo.tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           SQLQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(SQLQuery);

                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                MessageBox.Show("Data Delete  successfully.");

               // btnProcess.Enabled = false;
                prcLoadList();
                btnDelete.Visible = false;
                btnDelete.Enabled = false;
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
    }
}
